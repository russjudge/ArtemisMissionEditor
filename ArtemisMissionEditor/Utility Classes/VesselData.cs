using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtemisMissionEditor
{
	public sealed class HullRace
    {
        public string ID;
        public string name;
        public List<string> keys;

        public HullRace()
        {
            ID = "";
            keys = new List<string>();
        }
    }
    
    public struct BeamPort
    {
        public double x, y, z;
        public double damage, arcwidth, cycletime;
        public double range;
    }

	public struct DronePort
	{
		public double x, y, z;
		public double damage, cycletime;
		public double range;
	}

    public struct EnginePort
    {
        public double x, y, z;
    }

    public struct ManeuverPoint
    {
        public double x, y, z;
    }

    public struct ImpulsePoint
    {
        public double x, y, z;
    }

    public sealed class Vessel
    {
        public string uniqueID;
        public string side;
        public string classname;
        public string broadType;
        public string meshfile;
        public string diffuseFile;
		public string glowFile;
		public string specularFile;
        public double scale;
        public double pushRadius;
        public double shields_front;
        public double shields_back;
        public double preformance_turnrate;
        public double preformance_topspeed;
		public double carrier_compliment;
        public double fleet_ai_commonality;
        public List<BeamPort> beam_ports;
		public List<DronePort> drone_ports;
        public List<EnginePort> engine_ports;
        public List<ImpulsePoint> impulse_points;
        public List<ManeuverPoint> maneuver_points;

        public Vessel()
        {
            uniqueID = "";
            side="";
			classname="";
			broadType="";
			meshfile="";
			diffuseFile="";
			scale=0.0;
			pushRadius=0.0;
			shields_front=0.0;
			shields_back=0.0;
			preformance_turnrate=0.0;
			preformance_topspeed=0.0;
			carrier_compliment=0.0;

			beam_ports = new List<BeamPort>();
			drone_ports = new List<DronePort>();
            engine_ports = new List<EnginePort>();
            impulse_points = new List<ImpulsePoint>();
            maneuver_points = new List<ManeuverPoint>();
        }
    }

    public sealed class VesselData
    {
        private static List<string> ParserLog;

        private static VesselData _current = new VesselData();
        public static VesselData Current { get { return _current; } set { _current = value; OnVesselDataChanged(); } }

        public static event Action VesselDataChanged;
        private static void OnVesselDataChanged()
        {
            if (VesselDataChanged != null)
                VesselDataChanged();
        }

        public List<string> RaceNames { get; private set; }
        public List<string> RaceNamesSortedByLength { get; private set; }
        public List<string> RaceKeys { get; private set; }
        public List<string> VesselClassNames { get; private set; }
        public List<string> VesselClassNamesSortedByLength { get; private set; }
        public List<string> VesselBroadTypes { get; private set; }
        public Dictionary<string, Vessel> VesselList { get; private set; }
        public Dictionary<string, HullRace> HullRaceList { get; private set; }

        /// <summary>
        /// Prase specified race keys into lists of race names, race keys and unrecognized keys
        /// </summary>
        public Tuple<List<string>, List<string>, List<string>> ParseRaceKeys(string raceKeys) { return ParseRaceKeys(raceKeys.Split(' ').ToList()); }

        /// <summary>
        /// Prase specified race keys into lists of race names, race keys and unrecognized keys
        /// </summary>
        public Tuple<List<string>, List<string>, List<string>> ParseRaceKeys(List<string> raceKeys)
        {
            Tuple<List<string>, List<string>, List<string>> result = new Tuple<List<string>, List<string>, List<string>>(new List<string>(),new List<string>(), new List<string>());

            List<string> raceNames = VesselData.Current.RaceNamesSortedByLength;
            List<string> raceKeysLowered = new List<string>();
            for (int i = 0; i < raceKeys.Count; i++)
                raceKeysLowered.Add(raceKeys[i].ToLower());

            // Step 1: Check all at multi-word race names (case-insensitive)
            foreach (string curName in raceNames.FindAll((string s) => { return s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, raceKeysLowered, raceKeys, false);

            // Step 2: Check all single-word race names (case-sensitive)
            foreach (string curName in raceNames.FindAll((string s) => { return !s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, raceKeysLowered, raceKeys, true);

            // Step 3: Check all race keys (case-sensitive)
            foreach (string curKey in VesselData.Current.RaceKeys)
                ParseRaceHullKeys_CheckKey(curKey, result.Item2, result.Item3, raceKeysLowered, raceKeys, true);

            // Step 4: Check all single-word race names (case-insensitive)
            foreach (string curName in raceNames.FindAll((string s) => { return !s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, raceKeysLowered, raceKeys, false);

            // Step 5: Check all race keys (case-insensitive)
            foreach (string curKey in VesselData.Current.RaceKeys)
                ParseRaceHullKeys_CheckKey(curKey, result.Item2, result.Item3, raceKeysLowered, raceKeys, false);

            // What remains is unknown
            result.Item3.AddRange(raceKeys);

            return result;
        }

        /// <summary>
        /// Prase specified hull keys into lists of hull class names, hull broad keys and unrecognized keys
        /// </summary>
        public Tuple<List<string>, List<string>, List<string>> ParseHullKeys(string hullKeys) { return ParseHullKeys(hullKeys.Split(' ').ToList()); }

        /// <summary>
        /// Prase specified hull keys into lists of hull class names, hull broad keys and unrecognized keys
        /// </summary>
        public Tuple<List<string>, List<string>, List<string>> ParseHullKeys(List<string> hullKeys)
        {
            Tuple<List<string>, List<string>, List<string>> result = new Tuple<List<string>, List<string>, List<string>>(new List<string>(), new List<string>(), new List<string>());

            List<string> classNames = VesselData.Current.VesselClassNamesSortedByLength;
            List<string> hullKeysLowered = new List<string>();
            for (int i = 0; i < hullKeys.Count; i++) 
                hullKeysLowered.Add(hullKeys[i].ToLower());

            // Step 1: Check all at multi-word class names (case-insensitive)
            foreach (string curName in classNames.FindAll((string s) => { return s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, hullKeysLowered, hullKeys, false);
            
            // Step 2: Check all single-word class names (case-sensitive)
            foreach (string curName in classNames.FindAll((string s) => { return !s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, hullKeysLowered, hullKeys, true);

            // Step 3: Check all broad keys (case-sensitive)
            foreach (string curKey in VesselData.Current.VesselBroadTypes)
                ParseRaceHullKeys_CheckKey(curKey, result.Item2, result.Item3, hullKeysLowered, hullKeys, true);

            // Step 4: Check all single-word class names (case-insensitive)
            foreach (string curName in classNames.FindAll((string s) => { return !s.Contains(' '); }))
                ParseRaceHullKeys_CheckName(curName, result.Item1, result.Item3, hullKeysLowered, hullKeys, false);

            // Step 5: Check all broad keys (case-insensitive)
            foreach (string curKey in VesselData.Current.VesselBroadTypes)
                ParseRaceHullKeys_CheckKey(curKey, result.Item2, result.Item3, hullKeysLowered, hullKeys, false);

            // What remains is unknown
            result.Item3.AddRange(hullKeys);

            return result;
        }

        /// <summary>
        /// Part of logic from ParseRaceKeys and ParseHullKeys, checks for a single race/class name
        /// </summary>
        private void ParseRaceHullKeys_CheckName(string name, List<string> namesOutput, List<string> unrecognizedOutput, List<string> keysLowered, List<string> keys, bool caseSensitive)
        {
            string[] words = name.Split(' ');

            // Check if all words are present
            foreach (string word in words)
                if ((caseSensitive && !keys.Contains(word)) || (!caseSensitive && !keysLowered.Contains(word.ToLower())))
                    return;
            
            // If we found a whole class name - remove from source, add to list
            foreach (string word in words)
            {
                int pos = keysLowered.IndexOf(word.ToLower());
                keys.RemoveAt(pos);
                keysLowered.RemoveAt(pos);
            }

            if (namesOutput.Contains(name))
                unrecognizedOutput.Add(name);
            else
                namesOutput.Add(name);
        }

        /// <summary>
        /// Part of logic from ParseRaceKeys ParseHullKeys, checks for a single race/broad key
        /// </summary>
        private void ParseRaceHullKeys_CheckKey(string key, List<string> keysOutput, List<string> unrecognizedOutput, List<string> keysLowered, List<string> keys, bool caseSensitive)
        {
            if ((caseSensitive && !keys.Contains(key)) || (!caseSensitive && !keysLowered.Contains(key.ToLower())))
                return;
             
            int pos = keysLowered.IndexOf(key.ToLower());
            keys.RemoveAt(pos);
            keysLowered.RemoveAt(pos);

            if (keysOutput.Contains(key))
                unrecognizedOutput.Add(key);
            else
                keysOutput.Add(key);
        }

        /// <summary>
        /// Get the list of possible raceIDs that an object with provided racenames and keys can have
        /// </summary>
        public List<string> GetPossibleRaceIDs(string raceKeys)
        {
            List<string> possibleRaceIDs = new List<string>();
            
            // Case is irrelevant
            raceKeys = raceKeys.ToLower();
            List<string> keys = raceKeys.Split(' ').ToList();

            // Rate races according to the number of matches
            Dictionary<string, int> ratedRaces = new Dictionary<string, int>();
            int maxRating = 0;
            foreach (KeyValuePair<string, HullRace> kvp in HullRaceList)
            {
                int rating = 0;
                List<string> currentRaceWords = new List<string>();
                foreach (string word in kvp.Value.name.Split(' '))
                    currentRaceWords.Add(word.ToLower());
                foreach (string word in kvp.Value.keys)
                    currentRaceWords.Add(word.ToLower());
                foreach(string word in currentRaceWords)
                {
                    int indexOf = -1;
                    // Duplicates in the raceKeys do matter!
                    while ((indexOf = keys.IndexOf(word.ToLower(), indexOf + 1)) != -1)
                        rating++;
                }
                ratedRaces.Add(kvp.Value.ID, rating);
                maxRating = Math.Max(maxRating,rating);
            }
            
            // Races with maximum rating are all valid
            foreach(KeyValuePair<string, int> kvp in ratedRaces)
                if (kvp.Value == maxRating)
                    possibleRaceIDs.Add(kvp.Key);
            return possibleRaceIDs;
        }

        public List<string> GetPossibleVesselIDs(List<string> possibleRaceIDs, string hullKeys)
        {
            List<string> possibleVesselIDs = new List<string>();
            if (String.IsNullOrWhiteSpace(hullKeys))
                return possibleVesselIDs;

            // Case is irrelevant
            hullKeys = hullKeys.ToLower();
            List<string> keys = hullKeys.Split(' ').ToList();
                
            foreach (string raceID in possibleRaceIDs)
            {
                List<string> probableVesselIDs = new List<string>();

                //First find all ships for the race
                foreach (KeyValuePair<string, Vessel> kvp in VesselList)
                    if (kvp.Value.side == raceID)
                        probableVesselIDs.Add(kvp.Key);

                // Rate races according to the number of matches
                Dictionary<string, int> ratedVessels = new Dictionary<string, int>();
                int maxRating = 0;
                foreach (string vesselID in probableVesselIDs)
                {
                    int rating = 0;
                    List<string> currentHullWords = new List<string>();
                    foreach (string word in VesselList[vesselID].classname.Split(' '))
                        currentHullWords.Add(word.ToLower());
                    foreach (string word in VesselList[vesselID].broadType.Split(' '))
                        currentHullWords.Add(word.ToLower()); 
                    foreach (string word in currentHullWords)
                    {
                        int indexOf = -1;
                        // Duplicates in the hullKeys do matter!
                        while ((indexOf = keys.IndexOf(word.ToLower(), indexOf + 1)) != -1)
                            rating++;
                    }
                    
                    ratedVessels.Add(vesselID, rating);
                    maxRating = Math.Max(maxRating, rating);
                }

                // Vessels with maximum rating are all valid
                foreach (KeyValuePair<string, int> kvp in ratedVessels)
                    if (kvp.Value == maxRating)
                        possibleVesselIDs.Add(kvp.Key);
            }
            
            // Keyword "base" is special in that it converts objects of other kind into TSN Light Cruisers
            if (keys.Contains("base"))
            {
                List<string> newPossibleVesselIDs = new List<string>();
                bool nonStationEncountered = false;
                foreach (string id in possibleVesselIDs)
                {
                    if (VesselList[id].broadType.Split(' ').Contains("base"))
                        newPossibleVesselIDs.Add(id);
                    else
                        nonStationEncountered = true;
                }
                if (nonStationEncountered && !newPossibleVesselIDs.Contains("0"))
                    newPossibleVesselIDs.Insert(0, "0");
                possibleVesselIDs = newPossibleVesselIDs;
            }
            else // if (keys.Contains("base"))
            {
                List<string> newPossibleVesselIDs = new List<string>();
                bool stationEncountered = false;
                foreach (string id in possibleVesselIDs)
                {
                    if (!VesselList[id].broadType.Split(' ').Contains("base"))
                        newPossibleVesselIDs.Add(id);
                    else
                        stationEncountered = true;
                }
                if (stationEncountered && !newPossibleVesselIDs.Contains("0"))
                    newPossibleVesselIDs.Insert(0, "0");
                possibleVesselIDs = newPossibleVesselIDs;
            }

            return possibleVesselIDs;
        }

        public string VesselToString(string id)
        {
            Vessel v = VesselList[id];
            HullRace r = HullRaceList[v.side];
            return "[" + v.uniqueID.ToString() + "] " + r.name + " " + v.classname;
        }
        
        public string RaceToString(string id)
        {
            HullRace r = HullRaceList[id];
            return "[" + r.ID.ToString() + "] " + r.name;
        }

        private void FromXml_Version_1_56(XmlDocument xDoc)
        {
            XmlNodeList xNodeList;
            HullRace race;
            Vessel vessel;
            BeamPort bp;
			DronePort dp;
            EnginePort ep;
            ImpulsePoint ip;
            ManeuverPoint mp;

            xNodeList = xDoc.GetElementsByTagName("hullRace");
            foreach (XmlNode node in xNodeList)
            {
                race = new HullRace();

                foreach (XmlAttribute att in node.Attributes)
                {
                    switch (att.Name)
                    {
                        case "ID":
                            race.ID = att.Value;
                            break;
                        case "name":
                            race.name = att.Value;
                            if (!RaceNames.Contains(race.name))
                                RaceNames.Add(race.name);
                            break;
                        case "keys":

                            foreach (string s in att.Value.Split(' '))
                            {
                                race.keys.Add(s);
                                if (!RaceKeys.Contains(s))
                                    RaceKeys.Add(s);
                            }
                            break;
                    }
                }

                if (String.IsNullOrEmpty(race.ID))
                    continue;
                HullRaceList.Add(race.ID, race);
            }

            RaceNamesSortedByLength = RaceNames.ToList();
            RaceNamesSortedByLength.Sort(new Comparison<string>((string a, string b) =>
            {
                int diff = b.Split(' ').Length - a.Split(' ').Length;
                if (diff != 0) return diff;
                return String.Compare(a, b);
            }
            ));

            xNodeList = xDoc.GetElementsByTagName("vessel");
            foreach (XmlNode node in xNodeList)
            {
                vessel = new Vessel();

                foreach (XmlAttribute att in node.Attributes)
                {
                    switch (att.Name)
                    {
                        case "uniqueID":
                            vessel.uniqueID = att.Value;
                            break;
                        case "side":
                            vessel.side = att.Value;
                            break;
                        case "classname":
                            vessel.classname = att.Value;
                            if (!VesselClassNames.Contains(vessel.classname))
                                VesselClassNames.Add(vessel.classname);
                            break;
                        case "broadType":
                            vessel.broadType = att.Value;
                            foreach(string broadType in vessel.broadType.Split(' '))
                                if (!VesselBroadTypes.Contains(broadType))
                                    VesselBroadTypes.Add(broadType);
                            break;
                    }
                }
                
                if (String.IsNullOrEmpty(vessel.uniqueID))
                    continue;

                foreach (XmlNode subNode in node.ChildNodes)
                {
                    switch (subNode.Name)
                    {
                        case "art":
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "meshfile":
                                        vessel.meshfile = att.Value;
                                        break;
                                    case "diffuseFile":
                                        vessel.diffuseFile = att.Value;
                                        break;
									case "glowFile":
										vessel.glowFile = att.Value;
										break;
									case "specularFile":
										vessel.specularFile = att.Value;
										break;
									case "scale":
                                        vessel.scale = Helper.StringToDouble(att.Value);
                                        break;
                                    case "pushRadius":
                                        vessel.pushRadius = Helper.StringToDouble(att.Value);
                                        break;
                                }
                            }
                            break;

                        case "shields":
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "front":
                                        vessel.shields_front = Helper.StringToDouble(att.Value);
                                        break;
                                    case "back":
                                        vessel.shields_back = Helper.StringToDouble(att.Value);
                                        break;
                                }
                            }
                            break;

						case "carrier":
							foreach (XmlAttribute att in node.Attributes)
							{
								switch (att.Name)
								{
									case "compliment":
										vessel.carrier_compliment = Helper.StringToDouble(att.Value);
										break;
								}
							}
							break;

						case "performance":
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "turnrate":
                                        vessel.preformance_turnrate = Helper.StringToDouble(att.Value);
                                        break;
                                    case "topspeed":
                                        vessel.preformance_topspeed = Helper.StringToDouble(att.Value);
                                        break;
                                }
                            }
                            break;

                        case "fleet_ai":
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "commonality":
                                        vessel.fleet_ai_commonality = Helper.StringToDouble(att.Value);
                                        break;
                                }
                            }

                            break;

                        case "beam_port":
                            bp = new BeamPort();
                            foreach (XmlAttribute att in subNode.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "x":
                                        bp.x = Helper.StringToDouble(att.Value);
                                        break;
                                    case "y":
                                        bp.y = Helper.StringToDouble(att.Value);
                                        break;
                                    case "z":
                                        bp.z = Helper.StringToDouble(att.Value);
                                        break;
                                    case "range":
                                        bp.range = Helper.StringToDouble(att.Value);
                                        break;
                                    case "damage":
                                        bp.damage = Helper.StringToDouble(att.Value);
                                        break;
                                    case "cycletime":
                                        bp.cycletime = Helper.StringToDouble(att.Value);
                                        break;
                                    case "arcwidth":
                                        bp.arcwidth = Helper.StringToDouble(att.Value);
                                        break;
                                }
                            }
                            vessel.beam_ports.Add(bp);
                            break;

						case "drone_port":
							dp = new DronePort();
							foreach (XmlAttribute att in subNode.Attributes)
							{
								switch (att.Name)
								{
									case "x":
										dp.x = Helper.StringToDouble(att.Value);
										break;
									case "y":
										dp.y = Helper.StringToDouble(att.Value);
										break;
									case "z":
										dp.z = Helper.StringToDouble(att.Value);
										break;
									case "range":
										dp.range = Helper.StringToDouble(att.Value);
										break;
									case "damage":
										dp.damage = Helper.StringToDouble(att.Value);
										break;
									case "cycletime":
										dp.cycletime = Helper.StringToDouble(att.Value);
										break;
								}
							}
							vessel.drone_ports.Add(dp);
							break;

                        case "engine_port":
                            ep = new EnginePort();
                            foreach (XmlAttribute att in subNode.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "x":
                                        ep.x = Helper.StringToDouble(att.Value);
                                        break;
                                    case "y":
                                        ep.y = Helper.StringToDouble(att.Value);
                                        break;
                                    case "z":
                                        ep.z = Helper.StringToDouble(att.Value);
                                        break;

                                }
                            }
                            vessel.engine_ports.Add(ep);
                            break;

                        case "impulse_point":
                            ip = new ImpulsePoint();
                            foreach (XmlAttribute att in subNode.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "x":
                                        ip.x = Helper.StringToDouble(att.Value);
                                        break;
                                    case "y":
                                        ip.y = Helper.StringToDouble(att.Value);
                                        break;
                                    case "z":
                                        ip.z = Helper.StringToDouble(att.Value);
                                        break;

                                }
                            }
                            vessel.impulse_points.Add(ip);
                            break;

                        case "maneuver_point":
                            mp = new ManeuverPoint();
                            foreach (XmlAttribute att in subNode.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "x":
                                        mp.x = Helper.StringToDouble(att.Value);
                                        break;
                                    case "y":
                                        mp.y = Helper.StringToDouble(att.Value);
                                        break;
                                    case "z":
                                        mp.z = Helper.StringToDouble(att.Value);
                                        break;

                                }
                            }
                            vessel.maneuver_points.Add(mp);
                            break;
                    }
                }
                VesselList.Add(vessel.uniqueID, vessel);
            }

            VesselClassNamesSortedByLength = VesselClassNames.ToList();
            VesselClassNamesSortedByLength.Sort(new Comparison<string>((string a, string b) =>
            {
                int diff = b.Split(' ').Length - a.Split(' ').Length;
                if (diff != 0) return diff;
                return String.Compare(a, b);
            }
            ));

                
        }

        public void FromXml(XmlDocument xDoc)
        {
            _Initialize();
            
            XmlNodeList xNodeList;
            string version;
            
            //Gotta know the version to know how to parse it            
            version = "";
            xNodeList = xDoc.GetElementsByTagName("vessel_data");
            if (xNodeList.Count == 0)
                ParserLog.Add("Could not locate vessel_data tag");
            if (xNodeList.Count > 1)
                ParserLog.Add("More than one vessel_data tag present");
            if (xNodeList.Count == 1)
            {
                foreach (XmlAttribute att in xNodeList[0].Attributes)
                    if (att.Name == "version")
                        version = att.Value;
                if (String.IsNullOrEmpty(version))
                    ParserLog.Add("Could not locate version attribute in vessel_data tag or it was blank");
            }

            //Parse based on the version
            switch (version){
                case "1.56":
                    FromXml_Version_1_56(xDoc);
                    break;
				case "1.65":
					FromXml_Version_1_56(xDoc);
					break;
                case "1.66":
                    FromXml_Version_1_56(xDoc);
                    break;
                case "1.7":
                    FromXml_Version_1_56(xDoc);
                    break;
                case "2.1":
                    FromXml_Version_1_56(xDoc);
                    break;
                default:
                    ParserLog.Add("Attempting to parse unknown version: \""+version+"\", will use oldest loading method possible.");
                    FromXml_Version_1_56(xDoc); //Using oldest method
                    break;
            }
        }

        public void Load(string path)
        {
            XmlDocument xDoc;

            xDoc = new XmlDocument();
            try
            {
                xDoc.Load(path);
            }
            catch (Exception ex)
            {
				_Initialize();
				Log.Add("Problems when loading vesselData.xml");
                Log.Add("Unable to open file: " + path );
                Log.Add("Error message: ");
                Log.Add(ex);
                OnVesselDataChanged(); 
				Log.Add("There were problems when loading vesselData.xml");
                return;
            }

			bool error = false;
			bool log = false;

            try
            {
                FromXml(xDoc);
            }
            catch (Exception ex)
			{
				int errorPos = VesselList.Count+1;
				_Initialize();
				Log.Add("Unable to parse file: " + path);
				Log.Add("Error occured while parsing vessel #" + errorPos);
				Log.Add("Error message: ");
                Log.Add(ex);
				error = true;
            }

            if (VesselData.ParserLog.Count > 0)
            {
				log = true;
				Log.Add("Parser log:");
                foreach (string item in ParserLog)
                    Log.Add(item);
            }
            OnVesselDataChanged();

			if (error)
				Log.Add("There were problems when loading vesselData.xml");
			else if (log)
				Log.Add("There were warnings when loading vesselData.xml");
        }

        private void _Initialize()
        {
            ParserLog = new List<string>(); 
            
            RaceNames = new List<string>();
            RaceKeys = new List<string>();
            RaceNamesSortedByLength = new List<string>();
            VesselBroadTypes = new List<string>();
            VesselClassNames = new List<string>();
            VesselClassNamesSortedByLength = new List<string>();
            VesselList = new Dictionary<string, Vessel>();
            HullRaceList = new Dictionary<string, HullRace>();
        }
        
        public VesselData()
        {
            _Initialize();
        }
    }
}
