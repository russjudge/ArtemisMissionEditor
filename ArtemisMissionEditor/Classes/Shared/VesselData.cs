using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtemisMissionEditor
{
	public delegate void VesselDataUpdateEventHandler();

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
        private static List<string> parserLog;

        private static VesselData _current;
        public static VesselData Current
        {
            get
            {
                if (_current == null) _current = new VesselData();
                return _current;
            }
            set { _current = value; }
        }

        static VesselData()
        {
            Current = new VesselData();
        }

		public event EventHandler Update;

		private void OnUpdate()
		{
			ExpressionMemberValueEditor.HullID.InvalidateCMS();
			ExpressionMemberValueEditor.RaceKeys.InvalidateCMS();
			ExpressionMemberValueEditor.HullKeys.InvalidateCMS();

			if (Update != null)
				Update(this, EventArgs.Empty);
		}

        public List<string> RaceNames { get; private set; }
        public List<string> RaceKeys { get; private set; }
        public List<string> VesselClassNames { get; private set; }
        public List<string> VesselBroadTypes { get; private set; }
        public Dictionary<string, Vessel> VesselList { get; private set; }
        public Dictionary<string, HullRace> HullRaceList { get; private set; }

        //possible races object can have
        public List<string> GetPossibleRaceIDs(List<string> checkedRaceNames, List<string> checkedRaceKeys)
        {
            List<string> possibleRaceIDs = new List<string>();
            bool acceptable;

            //If no race names or keys selected any race is possible 
            //if (checkedRaceNames.Count == 0 && checkedRaceKeys.Count == 0)
            //    foreach (KeyValuePair<string, HullRace> kvp in hullRaceList)
            //        possibleRaceIDs.Add(kvp.Key);
            //WRONG! If no selected = no possible!
            if (checkedRaceNames.Count == 0 && checkedRaceKeys.Count == 0)
                return possibleRaceIDs;
            
            //If something is selected first look for race names
            foreach (string raceName in checkedRaceNames)
                foreach (KeyValuePair<string, HullRace> kvp in HullRaceList)
                    if (kvp.Value.name == raceName)
                        if (!possibleRaceIDs.Contains(kvp.Key))
                            possibleRaceIDs.Add(kvp.Key);

            //If no race name found look for race that has all the keys
            if (possibleRaceIDs.Count == 0)
                foreach (KeyValuePair<string, HullRace> kvp in HullRaceList)
                {
                    acceptable = true;
                    foreach (string raceKey in checkedRaceKeys)
                        if (!kvp.Value.keys.Contains(raceKey))
                            acceptable = false;
                    if (acceptable)
                        if (!possibleRaceIDs.Contains(kvp.Key))
                            possibleRaceIDs.Add(kvp.Key);
                }

            return possibleRaceIDs;
        }

        public List<string> GetPossibleVesselIDs(List<string> possibleRaceIDs, List<string> checkedVesselClassNames, List<string> checkedVesselBroadTypes)
        {
            List<string> probableVesselIDs = new List<string>();
            List<string> possibleVesselIDs = new List<string>();
            bool acceptable;

            //First find all ships for the available races
            foreach (KeyValuePair<string, Vessel> kvp in VesselList)
                if (possibleRaceIDs.Contains(kvp.Value.side))
                    probableVesselIDs.Add(kvp.Key);

            //if no ships types are selected any ship of the race's ships is okay 
            //if (checkedVesselBroadTypes.Count == 0 && checkedVesselClassNames.Count == 0)
            //    foreach (int id in probableVesselIDs)
            //        possibleVesselIDs.Add(id);
            //(WRONG!) If no selected = no possible!
            if (checkedVesselBroadTypes.Count == 0 && checkedVesselClassNames.Count == 0)
                return possibleVesselIDs;

            //Find all ships with class  from those who are okay by race
            foreach (string vesselClassName in checkedVesselClassNames)
                foreach (string id in probableVesselIDs)
                    if (VesselList[id].classname == vesselClassName)
                        possibleVesselIDs.Add(id);

            //If no ship matches classname => find ships that fit all the broad types
            if (possibleVesselIDs.Count == 0)
                foreach (string id in probableVesselIDs)
                {
                    acceptable = true;
                    foreach (string vesselBroadType in checkedVesselBroadTypes)
                        if (!(VesselList[id].broadType == vesselBroadType))
                            acceptable = false;

                    if (acceptable)
                        possibleVesselIDs.Add(id);
                }

            //DO NOT! If keys contradict each other its a fail!
            //if (possibleVesselIDs.Count == 0)
            //    foreach (string vesselBroadType in checkedVesselBroadTypes)
            //        foreach (int id in probableVesselIDs)
            //            if (vesselList[id].broadType == vesselBroadType)
            //                possibleVesselIDs.Add(id);

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
                            if (!VesselBroadTypes.Contains(vessel.broadType))
                                VesselBroadTypes.Add(vessel.broadType);
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
                parserLog.Add("Could not locate vessel_data tag");
            if (xNodeList.Count > 1)
                parserLog.Add("More than one vessel_data tag present");
            if (xNodeList.Count == 1)
            {
                foreach (XmlAttribute att in xNodeList[0].Attributes)
                    if (att.Name == "version")
                        version = att.Value;
                if (String.IsNullOrEmpty(version))
                    parserLog.Add("Could not locate version attribute in vessel_data tag or it was blank");
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
                    parserLog.Add("Attempting to parse unknown version: \""+version+"\", will use oldest loading method possible.");
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
            catch (Exception e)
            {
				_Initialize();
				Log.Add("Problems when loading vesselData.xml");
                Log.Add("Unable to open file: " + path );
                Log.Add("Error message: ");
                Log.Add(e);
				OnUpdate();
				Log.Add("There were problems when loading vesselData.xml");
                return;
            }

			bool error = false;
			bool log = false;

            try
            {
                FromXml(xDoc);
            }
            catch (Exception e)
			{
				int errorPos = VesselList.Count+1;
				_Initialize();
				Log.Add("Unable to parse file: " + path);
				Log.Add("Error occured while parsing vessel #" + errorPos);
				Log.Add("Error message: ");
                Log.Add(e);
				error = true;
            }

            if (VesselData.parserLog.Count > 0)
            {
				log = true;
				Log.Add("Parser log:");
                foreach (string item in parserLog)
                    Log.Add(item);
            }
			OnUpdate();

			if (error)
				Log.Add("There were problems when loading vesselData.xml");
			else if (log)
				Log.Add("There were warnings when loading vesselData.xml");
        }

        private void _Initialize()
        {
            parserLog = new List<string>(); 
            
            RaceNames = new List<string>();
            RaceKeys = new List<string>();
            VesselBroadTypes = new List<string>();
            VesselClassNames = new List<string>();
            VesselList = new Dictionary<string, Vessel>();
            HullRaceList = new Dictionary<string, HullRace>();
        }
        
        public VesselData()
        {
            _Initialize();
        }
    }
}
