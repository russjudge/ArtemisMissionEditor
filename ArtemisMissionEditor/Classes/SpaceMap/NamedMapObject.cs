using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Xml;

namespace ArtemisMissionEditor
{
    public sealed class HullIDConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection
        GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> tmp = new List<string>();

            tmp.Add("None");
			foreach (string id in Settings.VesselData.vesselList.Keys)
				tmp.Add(Settings.VesselData.VesselToString(id));

            return new StandardValuesCollection(tmp.ToArray());
        }
    }

    public class NamedMapObject
    {
        //IMPORTED
		[Browsable(false)]
        public bool Imported { get; set; }
        
        //COORDINATES
        public Coordinates3D _coordinates;
        [DisplayName("Coordinates"), Description("The coordinates of the object's center on the space map"), Category("\t\t\tPosition")]
        public virtual Coordinates3D Coordinates { get { return _coordinates; } set { _coordinates = value; } }

		//NAME
		private string _name;
		[DisplayName("Name"), Description("Indicates object's name, if any"), Category("ID")]
		public string name { get { return _name; } set { _name = value; } }

        #region SHARED
        
        public object GetPropertyByName(string pName)
        {
            Type t = GetType();
            PropertyInfo p = t.GetProperty(pName);
            return p.GetValue(this, null);
        }

        public void SetPropertyByName(string pName, object value)
        {
            Type t = GetType();
            PropertyInfo p = t.GetProperty(pName);
            p.SetValue(this, value, null);
        }

        protected static void __AddNewAttribute(XmlDocument doc, XmlElement element, string name, string value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value;
            element.Attributes.Append(att);
        }

        protected static void __RememberPropertyIfMissing(List<string> missingProperties, string pName, bool required)
        {
            if (!required) return;
            missingProperties.Add(pName);
        }

        public static NamedMapObject NewFromXml(XmlNode item)
        {
			if (!(item is XmlElement))
				return null;
			string type = "";
            NamedMapObject mo = null;
            foreach (XmlAttribute att in item.Attributes)
                if (att.Name == "type") type = att.Value.ToString();

            switch (type)
            {
                case "anomaly":
                    mo = new NamedMapObject_anomaly();
                    break;
                case "blackHole":
                    mo = new NamedMapObject_blackHole();
                    break;
                case "enemy":
                    mo = new NamedMapObject_enemy();
                    break;
                case "neutral":
                    mo = new NamedMapObject_neutral();
                    break;
                case "station":
                    mo = new NamedMapObject_station();
                    break;
                case "player":
                    mo = new NamedMapObject_player();
                    break;
                case "genericMesh":
                    mo = new NamedMapObject_genericMesh();
                    break;
                case "monster":
                    mo = new NamedMapObject_monster();
                    break;
				case "whale":
					mo = new NamedMapObject_whale();
					break;
				case "nebulas":
                    return null;
                case "asteroids":
                    return null;
                case "mines":
                    return null;
                default:
                    return null;
            }
            
            mo.FromXml(item);
            return mo;
        }

        public static bool IsPropertyPresent(string pName, NamedMapObject[] selection)
        {
            foreach (NamedMapObject item in selection)
                if (item.IsPropertyAvailable(pName))
                    return true;
            return false;
        }

        #endregion

        #region INHERITANCE

        //SELECTION
        public bool Selected;
        //Selected flag
        [Browsable(false)]
        public virtual bool _selectionAbsolute { get { return false; } }
        [Browsable(false)]
        public virtual int _selectionSize { get { return 15; } }

        //PROPERTIES
        //Is property available for this type of object
        public virtual bool IsPropertyAvailable(string pName) { if (pName == "x" || pName == "y" || pName == "z" || pName == "coordinates" || pName == "name") return true; return false; }
        //Is property mandatory for this type of object (server crash w/o this property)
        public virtual bool IsPropertyMandatory(string pName) { if (pName == "x" || pName == "y" || pName == "z" || pName == "coordinates") return true; return false; }
        
        //TYPE
        [Browsable(false)]
        public virtual string TypeToString { get { return "null"; } }
        [Browsable(false)]
        public virtual string TypeToStringShort { get { return "null"; } }
        [DisplayName("Type"), Description("Indicates object's type (enemy, station, neutral etc.)"), Category("ID")]
        public string TypeToString_Display
        {
            get
            {
                int i;
                string s = TypeToString;

                if (string.IsNullOrEmpty(s)) return string.Empty;

                for (i = s.Length-1; i >=0; i--)
                {
                    if (char.IsUpper(s[i]))
                    {
                        s = s.Insert(i, " ");
                    }
                }
                return char.ToUpper(s[0]) + s.Substring(1);
                
            }
        }

        //COPIER
        public virtual void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;
            
            //Coordinates
            if (!excludeCoordinates)
            {
                Coordinates3D tmp1 = new Coordinates3D();
                same = true; found = false;
                foreach (NamedMapObject item in source)
                    if (item.IsPropertyAvailable("coordinates"))
                        if (!found)
                        {
                            found = true;
                            tmp1 = item.Coordinates;
                        }
                        else
                            same = same && (item.Coordinates != tmp1);
                if (found && same)
                    this._coordinates = tmp1;
            }

			//Name
			string tmp2 = "";
			same = true; found = false;
			foreach (NamedMapObject item in source)
				if (item.IsPropertyAvailable("name"))
					if (!found)
					{
						found = true;
						tmp2 = item.name;
					}
					else
						same = same && (item.name != tmp2);
			if (found && same)
				this.name = tmp2;


        }

        //TO XML
        public virtual XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            XmlElement create = xDoc.CreateElement("create");

            __AddNewAttribute(xDoc, create, "type", type);

            __AddNewAttribute(xDoc, create, "x", Helper.DoubleToString(_coordinates.X));
            __AddNewAttribute(xDoc, create, "y", Helper.DoubleToString(_coordinates.Y));
            __AddNewAttribute(xDoc, create, "z", Helper.DoubleToString(_coordinates.Z));

			if (name != "")
				__AddNewAttribute(xDoc, create, "name", name);
			else
				__RememberPropertyIfMissing(missingProperties, "name", IsPropertyMandatory("name"));

            return create;
        }

        //FROM XML
        public virtual void FromXml(XmlNode item)
        {
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "x":
                        _coordinates.X = Helper.StringToDouble(att.Value);
                        break;
                    case "y":
						_coordinates.Y = Helper.StringToDouble(att.Value);
                        break;
                    case "z":
						_coordinates.Z = Helper.StringToDouble(att.Value);
                        break;
					case "name":
						_name = att.Value;
						break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObject(int posX = 0, int posY = 0, int posZ = 0, string name = "", bool makeSelected = false)
        { _coordinates = new Coordinates3D(true, posX, posY, posZ); Selected = makeSelected; _name = name; Imported = false; }
        
        #endregion
    }

    public class NamedMapObjectA : NamedMapObject
    {
        //ANGLE
        private double _a;
        [DisplayName("Angle"), Description("Indicates object's angle in degrees relative to the upright position, clockwise rotation considered positive"), Category("\t\t\tPosition"), DefaultValue(0)]
		public double A_deg
        {
            get { return _a; }
            set
            {
                while (value < 0) value += 360;
                while (value >= 360) value -= 360;
                _a = value;
            }
        }
        [Browsable(false)]
        public double A_rad
        {
            get { return (_a) / 360.0 * Math.PI * 2.0; }
            set
            {
                while (value < 0.0) value += 2.0 * Math.PI;
                while (value > 2.0 * Math.PI) value -= 2.0 * Math.PI;
                _a = Math.Round(value / 2.0 / Math.PI * 3600.0)/10.0;
            }
        }

        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName)
        {
            if (pName == "angle") return true;
            return base.IsPropertyAvailable(pName);
        }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return base.TypeToString; } }
        public override string TypeToStringShort { get { return base.TypeToString; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //Angle
            double tmp1 = 0;
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("angle"))
                    if (!found)
                    {
                        found = true;
                        tmp1 = ((NamedMapObjectA)item).A_deg;
                    }
                    else
                        same = same && (((NamedMapObjectA)item).A_deg != tmp1);
            if (found && same)
                this.A_deg = tmp1;

            
            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            __AddNewAttribute(xDoc, create, "angle", A_deg.ToString());

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "angle":
                        A_deg = Helper.StringToDouble(att.Value);
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObjectA(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "")
            : base(posX, posY, posZ, name, makeSelected)
        { this.A_rad = angle; }
        
        #endregion
    }

    public class NamedMapObjectAS : NamedMapObjectA
    {
        //Side
        private int? _sideValue;
        [DisplayName("Side"), Description("Side the entity is allied with (0 = none, 1 = enemy, 2+ = player)")]
        public int? sideValue { get { return _sideValue; } set { _sideValue = value; } }

        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName)
        {
            if (pName == "sideValue") return true;
            return base.IsPropertyAvailable(pName);
        }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return base.TypeToString; } }
        public override string TypeToStringShort { get { return base.TypeToString; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //Side
            int tmp1 = 0;
            same = true; found = false;
            foreach (NamedMapObject item in source)
            {
                if (item.IsPropertyAvailable("sideValue"))
                {
                    if (!found)
                    {
                        if (item as NamedMapObjectAS != null && ((NamedMapObjectAS)item).sideValue.HasValue)
                        {
                            found = true;
                            tmp1 = ((NamedMapObjectAS)item).sideValue.Value;
                        }
                        if (item as NamedMapObjectArhK != null && ((NamedMapObjectArhKS)item).sideValue.HasValue)
                        {
                            found = true;
                            tmp1 = ((NamedMapObjectArhKS)item).sideValue.Value;
                        }
                    }
                    else
                    {
                        int tmp2 = 0;
                        if (item as NamedMapObjectAS != null && ((NamedMapObjectAS)item).sideValue.HasValue)
                            tmp2 = ((NamedMapObjectAS)item).sideValue.Value;
                        if (item as NamedMapObjectArhK != null && ((NamedMapObjectArhKS)item).sideValue.HasValue)
                            tmp2 = ((NamedMapObjectArhKS)item).sideValue.Value;

                        same = same && (tmp2 != tmp1);
                    }
                }
            }
            if (found && same)
                this.sideValue = tmp1;

            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            if (sideValue.HasValue)
                __AddNewAttribute(xDoc, create, "sideValue", sideValue.ToString());

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "sideValue":
                        sideValue = Helper.StringToInt(att.Value);
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObjectAS(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", int? _sideValue = null)
            : base(posX, posY, posZ, makeSelected, angle, name)
        { sideValue = _sideValue; }
        
        #endregion
    }

    public class NamedMapObjectArhK : NamedMapObjectA
    {
        //RACE KEYS && HULL KEYS

        //Only for easy output to xml
        [Browsable(false)] 
        public List<string> raceKeys
        {
            get
            {
                List<string> tmp = new List<string>();
                foreach (string item in _race_Names.Split(' '))
                    if(item!="") tmp.Add(item);
                foreach (string item in _race_Keys.Split(' '))
                    if (item != "") tmp.Add(item);
                foreach (string item in _race_Unrecognised.Split(' '))
                    if (item != "") tmp.Add(item);
                return tmp;
            }
        }
        
        //Only for easy output to xml
        [Browsable(false)]
        public List<string> hullKeys
        {
            get
            {
                List<string> tmp = new List<string>();
                foreach (string item in _vessel_BroadTypes.Split(' '))
                    if (item != "") tmp.Add(item);
                foreach (string item in _vessel_ClassNames.Split(' '))
                    if (item != "") tmp.Add(item);
                foreach (string item in _vessel_Unrecognised.Split(' '))
                    if (item != "") tmp.Add(item);
                return tmp;
            }
        }

        private string _race_Names;

        [DisplayName("names"), Category("\t\traceKeys"), Description("List of object's race names (from vesselData.xml)"), DefaultValue("< none >"), EditorAttribute(typeof(CheckedListBoxUITypeEditor_raceNames), typeof(System.Drawing.Design.UITypeEditor))]
        public string RaceNames_Display { get { if (_race_Names == "") return "< none >"; return _race_Names; } set { _race_Names = value.Replace("< none >", ""); RaceNames_ValidateSort(); } }
        [Browsable(false)]
        public string RaceNames { get { return _race_Names; } set { _race_Names = value; RaceNames_ValidateSort(); } }
        public void RaceNames_Add(string value) { _race_Names = (_race_Names == "" ? "" : _race_Names + " ") + value; RaceNames_ValidateSort(); }
        private void RaceNames_ValidateSort()
        {
            string[] tmp = _race_Names.Split(' ');
            List<string> tmp2 = new List<string>();
            foreach (string item in Settings.VesselData.raceNames)
                if (tmp.Contains(item))
                    tmp2.Add(item);
            tmp2.Sort();
            if (tmp2.Count == 0)
                _race_Names = "";
            else
                _race_Names = tmp2.Aggregate((alfa, beta) => alfa + " " + beta);
        }

        private string _race_Keys;

        [DisplayName("keys"), Category("\t\traceKeys"), Description("List of object's race keys (from vesselData.xml)"), DefaultValue("< none >"), EditorAttribute(typeof(CheckedListBoxUITypeEditor_raceKeys), typeof(System.Drawing.Design.UITypeEditor))]
        public string RaceKeys_Display { get { if (_race_Keys == "") return "< none >"; return _race_Keys; } set { _race_Keys = value.Replace("< none >", ""); RaceKeys_ValidateSort(); } }
        [Browsable(false)]
        public string RaceKeys { get { return _race_Keys; } set { _race_Keys = value; RaceKeys_ValidateSort(); } }
        public void RaceKeys_Add(string value) { _race_Keys = (_race_Keys == "" ? "" : _race_Keys + " ") + value; RaceKeys_ValidateSort(); }
        private void RaceKeys_ValidateSort()
        {
            string[] tmp = _race_Keys.Split(' ');
            List<string> tmp2 = new List<string>();
            foreach (string item in Settings.VesselData.raceKeys)
                if (tmp.Contains(item))
                    tmp2.Add(item);
            tmp2.Sort();
            if (tmp2.Count == 0)
                _race_Keys = "";
            else
                _race_Keys = tmp2.Aggregate((alfa, beta) => alfa + " " + beta);
        }

        private string _race_Unrecognised;
        [DisplayName("unrecognised"), Category("\t\traceKeys"), Description("Race names and keys that were not recognised by the program (not present in current vesselData.xml) are put here."), DefaultValue("")]
        public string RaceUnrecognised { get { return _race_Unrecognised; } set { _race_Unrecognised = value; } }
        public void RaceUnrecognised_Add(string value) { _race_Unrecognised = (_race_Unrecognised == "" ? "" : _race_Unrecognised + " ") + value; }

        private string _vessel_ClassNames;

        [DisplayName("classNames"), Category("\thullKeys"), Description("List of object's vessel class names (from vesselData.xml)"), DefaultValue("< none >"), EditorAttribute(typeof(CheckedListBoxUITypeEditor_vesselClassNames), typeof(System.Drawing.Design.UITypeEditor))]
        public string VesselClassNames_Display { get { if (_vessel_ClassNames == "") return "< none >"; return _vessel_ClassNames; } set { _vessel_ClassNames = value.Replace("< none >", ""); VesselClassNames_ValidateSort(); } }
        [Browsable(false)]
        public string VesselClassNames { get { return _vessel_ClassNames; } set { _vessel_ClassNames = value; VesselClassNames_ValidateSort(); } }
        public void VesselClassNames_Add(string value) { _vessel_ClassNames = (_vessel_ClassNames == "" ? "" : _vessel_ClassNames + " ") + value; VesselClassNames_ValidateSort(); }
        private void VesselClassNames_ValidateSort()
        {
            string[] tmp = _vessel_ClassNames.Split(' ');
            List<string> tmp2 = new List<string>();
            foreach (string item in Settings.VesselData.vesselClassNames)
                if (tmp.Contains(item))
                    tmp2.Add(item);
            tmp2.Sort();
            if (tmp2.Count == 0)
                _vessel_ClassNames = "";
            else
                _vessel_ClassNames = tmp2.Aggregate((alfa, beta) => alfa + " " + beta);
        }

        private string _vessel_BroadTypes;

        [DisplayName("broadTypes"), Category("\thullKeys"), Description("List of object's vessel broad types (from vesselData.xml)"), DefaultValue("< none >"), EditorAttribute(typeof(CheckedListBoxUITypeEditor_vesselBroadTypes), typeof(System.Drawing.Design.UITypeEditor))]
        public string VesselBroadTypes_Display { get { if (_vessel_BroadTypes == "") return "< none >"; return _vessel_BroadTypes; } set { _vessel_BroadTypes = value.Replace("< none >",""); VesselBroadTypes_ValidateSort(); } }
        [Browsable(false)]
        public string VesselBroadTypes { get { return _vessel_BroadTypes; } set { _vessel_BroadTypes = value; VesselBroadTypes_ValidateSort(); } }
        public void VesselBroadTypes_Add(string value) { _vessel_BroadTypes = (_vessel_BroadTypes == "" ? "" : _vessel_BroadTypes + " ") + value; VesselBroadTypes_ValidateSort(); }
        private void VesselBroadTypes_ValidateSort()
        {
            string[] tmp = _vessel_BroadTypes.Split(' ');
            List<string> tmp2 = new List<string>();
            foreach (string item in Settings.VesselData.vesselBroadTypes)
                if (tmp.Contains(item))
                    tmp2.Add(item);
            tmp2.Sort();
            if (tmp2.Count == 0)
                _vessel_BroadTypes = "";
            else
                _vessel_BroadTypes = tmp2.Aggregate((alfa, beta) => alfa + " " + beta);
        }

        private string _vessel_Unrecognised;
        [DisplayName("unrecognised"), Category("\thullKeys"), Description("Hull types and keys that were not recognised by the program (not present in current vesselData.xml) are put here."), DefaultValue("")]
        public string VesselUnrecognised { get { return _vessel_Unrecognised; } set { _vessel_Unrecognised = value; } }
        public void VesselUnrecognised_Add(string value) { _vessel_Unrecognised = (_vessel_Unrecognised == "" ? "" : _vessel_Unrecognised + " ") + value; }

        

        private string _hullID;

        [Browsable(false)]
        public string hullID { get { return _hullID; } set { _hullID = value; } }

        [DisplayName("Hull ID"), Description("Indicates object's exact vessel ID (from vesselData.xml)"), DefaultValue("< none >"), TypeConverter(typeof(HullIDConverter))]
        public string hullID_Display
        {
            get 
            { 
                if (_hullID=="")
                    return "< none >";
                if (!Settings.VesselData.vesselList.ContainsKey(_hullID))
                    return "[" + _hullID + "] Vessel does not exist";
                return Settings.VesselData.VesselToString(_hullID);
            }

            set
            {
                int tmp;
                _hullID = value;
                if (_hullID == "") return;
                if (_hullID == null) throw new ArgumentNullException("Attempt to assign null to hullID!");
                
                if (Helper.IntTryParse(_hullID, out tmp)) return;
                _hullID = _hullID.Replace("[", "").Replace(" ","");
                if (_hullID.IndexOf("]") == -1) { _hullID = ""; return; }
                _hullID = _hullID.Substring(0,_hullID.IndexOf("]"));
                if (Helper.IntTryParse(_hullID, out tmp)) return;
                _hullID = "";
            }
        }

        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { 
            if (pName == "raceKeys") return true;
            if (pName == "hullKeys") return true;
            if (pName == "hullID") return true;
            return base.IsPropertyAvailable(pName); 
        }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return base.TypeToString; } }
        public override string TypeToStringShort { get { return base.TypeToString; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //racekeys
            string tmp1 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("raceKeys"))
                    if (!found)
                    {
                        found = true;
                        tmp1 = ((NamedMapObjectArhK)item)._race_Keys;
                    }
                    else
                        same = same && (((NamedMapObjectArhK)item)._race_Keys != tmp1);
            if (found && same)
                this._race_Keys = tmp1;
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("raceKeys"))
                    if (!found)
                    {
                        found = true;
                        tmp1 = ((NamedMapObjectArhK)item)._race_Names;
                    }
                    else
                        same = same && (((NamedMapObjectArhK)item)._race_Names != tmp1);
            if (found && same)
                this._race_Names = tmp1;

            //hullkeys
            string tmp2 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("hullKeys"))
                    if (!found)
                    {
                        found = true;
                        tmp2 = ((NamedMapObjectArhK)item)._vessel_BroadTypes;
                    }
                    else
                        same = same && (((NamedMapObjectArhK)item)._vessel_BroadTypes != tmp2);
            if (found && same)
                this._vessel_BroadTypes = tmp2;
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("hullKeys"))
                    if (!found)
                    {
                        found = true;
                        tmp2 = ((NamedMapObjectArhK)item)._vessel_ClassNames;
                    }
                    else
                        same = same && (((NamedMapObjectArhK)item)._vessel_ClassNames != tmp2);
            if (found && same)
                this._vessel_ClassNames = tmp2;

            //hullID
            string tmp3 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("hullID"))
                    if (!found)
                    {
                        found = true;
                        tmp3 = ((NamedMapObjectArhK)item)._hullID;
                    }
                    else
                        same = same && (((NamedMapObjectArhK)item)._hullID != tmp3);
            if (found && same)
                this._hullID = tmp3;

            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);


            if (raceKeys.Count() > 0)
                __AddNewAttribute(xDoc, create, "raceKeys", raceKeys.Aggregate((i, j) => i + " " + j));
            else
                __RememberPropertyIfMissing(missingProperties, "raceKeys", IsPropertyMandatory("raceKeys") && _hullID == "");

            if (hullKeys.Count() > 0)
                __AddNewAttribute(xDoc, create, "hullKeys", hullKeys.Aggregate((i, j) => i + " " + j));
            else
                __RememberPropertyIfMissing(missingProperties, "hullKeys", IsPropertyMandatory("hullKeys") && _hullID == "");

            if (_hullID!="")
                __AddNewAttribute(xDoc, create, "hullID", _hullID);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "raceKeys":
                        foreach (string str in att.Value.Split(' '))
                        {
                            if (Settings.VesselData.raceKeys.Contains(str))
                                RaceKeys_Add(str);
                            else if (Settings.VesselData.raceNames.Contains(str))
                                RaceNames_Add(str);
                            else 
                                RaceUnrecognised_Add(str);
                        }
                        break;
                    case "hullKeys":
                        foreach (string str in att.Value.Split(' '))
                        {
                            if (Settings.VesselData.vesselBroadTypes.Contains(str))
                                VesselBroadTypes_Add(str);
                            else if (Settings.VesselData.vesselClassNames.Contains(str))
                                VesselClassNames_Add(str);
                            else
                                VesselUnrecognised_Add(str);
                        }
                        break;
                    case "hullID":
                        _hullID = att.Value;
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObjectArhK(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _race_Keys = "", string _race_Names = "", string _vessel_ClassNames = "", string _vessel_BroadTypes = "")
            : base(posX, posY, posZ, makeSelected, angle, name)
        { this._race_Keys = _race_Keys; this._race_Names = _race_Names; this._race_Unrecognised = ""; this._vessel_ClassNames = _vessel_ClassNames; this._vessel_BroadTypes = _vessel_BroadTypes; this._vessel_Unrecognised = ""; this._hullID = ""; }
        
        #endregion
    }

    public class NamedMapObjectArhKS : NamedMapObjectArhK
    {
        //Side
        private int? _sideValue;
        [DisplayName("Side"), Description("Side the entity is allied with (0 = none, 1 = enemy, 2+ = player)")]
        public int? sideValue { get { return _sideValue; } set { _sideValue = value; } }

        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName)
        {
            if (pName == "sideValue") return true;
            return base.IsPropertyAvailable(pName);
        }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return base.TypeToString; } }
        public override string TypeToStringShort { get { return base.TypeToString; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //Side
            int tmp1 = 0;
            same = true; found = false;
            foreach (NamedMapObject item in source)
            {
                if (item.IsPropertyAvailable("sideValue"))
                {
                    if (!found)
                    {
                        if (item as NamedMapObjectAS != null && ((NamedMapObjectAS)item).sideValue.HasValue)
                        {
                            found = true;
                            tmp1 = ((NamedMapObjectAS)item).sideValue.Value;
                        }
                        if (item as NamedMapObjectArhK != null && ((NamedMapObjectArhKS)item).sideValue.HasValue)
                        {
                            found = true;
                            tmp1 = ((NamedMapObjectArhKS)item).sideValue.Value;
                        }
                    }
                    else
                    {
                        int tmp2 = 0;
                        if (item as NamedMapObjectAS != null && ((NamedMapObjectAS)item).sideValue.HasValue)
                            tmp2 = ((NamedMapObjectAS)item).sideValue.Value;
                        if (item as NamedMapObjectArhK != null && ((NamedMapObjectArhKS)item).sideValue.HasValue)
                            tmp2 = ((NamedMapObjectArhKS)item).sideValue.Value;

                        same = same && (tmp2 != tmp1);
                    }
                }
            }
            if (found && same)
                this.sideValue = tmp1;

            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            if (sideValue.HasValue)
                __AddNewAttribute(xDoc, create, "sideValue", sideValue.ToString());

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "sideValue":
                        sideValue = Helper.StringToInt(att.Value);
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObjectArhKS(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _race_Keys = "", string _race_Names = "", string _vessel_ClassNames = "", string _vessel_BroadTypes = "", int? _sideValue = null)
            : base(posX, posY, posZ, makeSelected, angle, name, _race_Keys, _race_Names, _vessel_ClassNames, _vessel_BroadTypes)
        {
            sideValue = _sideValue;
        }
        
        #endregion
    }

    public sealed class NamedMapObject_anomaly : NamedMapObject
    {
        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return 10; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return "anomaly"; } }
        public override string TypeToStringShort { get { return "ANM"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_anomaly(int posX = 0, int posY = 0, int posZ = 0, string name = "", bool makeSelected = false)
            : base(posX, posY, posZ, name, makeSelected)
        { }

        #endregion
    }

    public sealed class NamedMapObject_blackHole : NamedMapObject
    {
        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return true; } }
        public override int _selectionSize { get { return 2550; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return "blackHole"; } }
        public override string TypeToStringShort { get { return "BHL"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_blackHole(int posX = 0, int posY = 0, int posZ = 0, string name = "", bool makeSelected = false)
            : base(posX, posY, posZ, name, makeSelected)
        { }

        #endregion
    }

    public sealed class NamedMapObject_monster : NamedMapObjectA
    {
        #region INHERITANCE

        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return "monster"; } }
        public override string TypeToStringShort { get { return "MON"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_monster(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "")
            : base(posX, posY, posZ, makeSelected, angle, name)
        { }

        #endregion
    }

    public sealed class NamedMapObject_player : NamedMapObjectAS
    {
        #region INHERITANCE
        
        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return 30; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

        //TYPE
        public override string TypeToString { get { return "player"; } }
        public override string TypeToStringShort { get { return "PLR"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_player(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "")
            : base(posX, posY, posZ, makeSelected, angle, name)
        { }

        #endregion
    }

	public sealed class NamedMapObject_whale : NamedMapObjectA
	{
		//POD NUMBER
		private string _podnumber;
		[DisplayName("Pod Number"), Description("Whale pod number, identifies packs of whales that travel together")]
        public string podnumber { get { return _podnumber; } set { _podnumber = value; } }

		#region INHERITANCE

		//SELECTION
		public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
		public override int _selectionSize { get { return 30; } }

		//PROPERTIES
		public override bool IsPropertyAvailable(string pName)
		{
			if (pName == "podnumber") return true;
			return base.IsPropertyAvailable(pName);
		}
		public override bool IsPropertyMandatory(string pName) { return base.IsPropertyMandatory(pName); }

		//TYPE
		public override string TypeToString { get { return "whale"; } }
		public override string TypeToStringShort { get { return "WHL"; } }

		//COPIER
		public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

		//TO XML
		public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
		{
			if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
			XmlElement create = base.ToXml(xDoc, missingProperties, type);

			__AddNewAttribute(xDoc, create, "podnumber", _podnumber.ToString());

			return create;
		}

		//FROM XML
		public override void FromXml(XmlNode item)
		{
			base.FromXml(item);
			foreach (XmlAttribute att in item.Attributes)
				switch (att.Name)
				{
					case "podnumber":
						_podnumber = att.Value;
						break;
				}
		}

		//CONSTRUCTOR
		public NamedMapObject_whale(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string podnumber = "0")
			: base(posX, posY, posZ, makeSelected, angle, name)
		{ _podnumber = podnumber; }

		#endregion
	}

    public sealed class NamedMapObject_station : NamedMapObjectArhKS
    {
        #region INHERITANCE
        
        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { 
            if (pName == "name") return true;
            return base.IsPropertyMandatory(pName); 
        }

        //TYPE
        public override string TypeToString { get { return "station"; } }
        public override string TypeToStringShort { get { return "STN"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_station(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _race_Keys = "", string _race_Names = "", string _vessel_ClassNames = "", string _vessel_BroadTypes = "")
            : base(posX, posY, posZ, makeSelected, angle, name, _race_Keys, _race_Names, _vessel_ClassNames, _vessel_BroadTypes)
        { }

        #endregion
    }

    public sealed class NamedMapObject_neutral : NamedMapObjectArhKS
    {
        #region INHERITANCE
        
        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return 30; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { return base.IsPropertyAvailable(pName); }
        public override bool IsPropertyMandatory(string pName) { 
            if (pName == "raceKeys") return true;
            if (pName == "hullKeys") return true;
            return base.IsPropertyMandatory(pName); 
        }

        //TYPE
        public override string TypeToString { get { return "neutral"; } }
        public override string TypeToStringShort { get { return "NEU"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source) { base.Copy(excludeCoordinates, source); }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            //foreach (XmlAttribute att in item.Attributes)
            //    switch (att.Name)
            //    {

            //    }
        }

        //CONSTRUCTOR
        public NamedMapObject_neutral(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _race_Keys = "", string _race_Names = "", string _vessel_ClassNames = "", string _vessel_BroadTypes = "")
            : base(posX, posY, posZ, makeSelected, angle, name, _race_Keys, _race_Names, _vessel_ClassNames, _vessel_BroadTypes)
        { }

        #endregion
    }

    public sealed class NamedMapObject_enemy : NamedMapObjectArhKS
    {
        //FLEET NUMBER
        private string _fleetnumber;
        [DisplayName("Fleet Number"), Description("Enemy fleet number, identifies packs of ships that travel together, following fleet leader")]
        public string fleetnumber { get { return _fleetnumber; } set {_fleetnumber = value; } }

        #region INHERITANCE
        
        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return 30; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { 
            if (pName == "fleetnumber") return true; 
            return base.IsPropertyAvailable(pName); 
        }
        public override bool IsPropertyMandatory(string pName) { 
            if (pName == "raceKeys") return true;
            if (pName == "hullKeys") return true;
            return base.IsPropertyMandatory(pName); 
        }

        //TYPE
        public override string TypeToString { get { return "enemy"; } }
        public override string TypeToStringShort { get { return "ENM"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //fleetnumber
            string tmp1 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("fleetnumber"))
                    if (!found)
                    {
                        found = true;
                        tmp1 = ((NamedMapObject_enemy)item)._fleetnumber;
                    }
                    else
                        same = same && (((NamedMapObject_enemy)item)._fleetnumber != tmp1);
            if (found && same)
                this._fleetnumber = tmp1;
            
                        
            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            __AddNewAttribute(xDoc, create, "fleetnumber", _fleetnumber.ToString());

            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "fleetnumber":
                        _fleetnumber = att.Value;
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObject_enemy(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _race_Keys = "", string _race_Names = "", string _vessel_ClassNames = "", string _vessel_BroadTypes = "", string fleetnumber = "-1")
            : base(posX, posY, posZ, makeSelected, angle, name, _race_Keys, _race_Names, _vessel_ClassNames, _vessel_BroadTypes)
        { _fleetnumber = fleetnumber; }

        #endregion
    }

    public sealed class NamedMapObject_genericMesh : NamedMapObjectA
    {
        private string _meshFileName;
        [DisplayName("Mesh Filename"), Description("Path to the mesh file, relative to the game directory")]
        public string meshFileName { get { return _meshFileName; } set { _meshFileName = value; } }

        private string _textureFileName;
        [DisplayName("Texture Filename"), Description("Path to the texture file, relative to the game directory)")]
        public string textureFileName { get { return _textureFileName; } set { _textureFileName = value; } }

		private string _hullRace;
		[DisplayName("Hull Race"), Category("ID"), Description("(unknown what this means, probably not used yet)")]
		public string hullRace { get { return _hullRace; } set { _hullRace = value; } }

		private string _hullType;
        [DisplayName("Hull Type"), Category("ID"), Description("(unknown what this means, probably not used yet)")]
		public string hullType { get { return _hullType; } set { _hullType = value; } }

        [Browsable(false)]
        public FakeShields _fakeShields;
        [DisplayName("Fake shields"), Description("Describes wether the generic mesh has fake shields")]
        public FakeShields fakeShields { get { return _fakeShields; } set { _fakeShields = value; } }
        private bool ShouldSerializefakeShields()
        {
            return _fakeShields != new FakeShields(true);
        }

        private Color _color;
        [DisplayName("Color"), Description("Describes what color the mesh and its name will have ingame")]
        public Color Color { get { return _color; } set { _color = value; } }
        [Browsable(false)]
        public double Color_Red
        {
            get { return (double)_color.R / 255; }
            set { _color = Color.FromArgb((int)Math.Round(255 * value), _color.G, _color.B); }
        }
        [Browsable(false)]
        public double Color_Green
        {
            get { return (double)_color.G / 255; }
            set { _color = Color.FromArgb(_color.R, (int)Math.Round(255 * value), _color.B); }
        }
        [Browsable(false)]
        public double Color_Blue
        {
            get { return (double)_color.B / 255; }
            set { _color = Color.FromArgb(_color.R, _color.G, (int)Math.Round(255 * value)); }
        }

        #region INHERITANCE
        
        //SELECTION
        public override bool _selectionAbsolute { get { return base._selectionAbsolute; } }
        public override int _selectionSize { get { return base._selectionSize; } }

        //PROPERTIES
        public override bool IsPropertyAvailable(string pName) { 
            if (pName == "meshFileName") return true;
            if (pName == "textureFileName") return true;
			if (pName == "hullRace") return true;
			if (pName == "hullType") return true;
			if (pName == "fakeShields") return true;
            if (pName == "fakeShieldsFront") return true;
            if (pName == "fakeShieldsRear") return true;
            if (pName == "hasFakeShldFreq") return true;
            if (pName == "color") return true;
            if (pName == "colorRed") return true;
            if (pName == "colorGreen") return true;
            if (pName == "colorBlue") return true;
            return base.IsPropertyAvailable(pName); 
        }
        public override bool IsPropertyMandatory(string pName) { 
            if (pName == "meshFileName") return true;
            if (pName == "textureFileName") return true;
            return base.IsPropertyMandatory(pName); 
        }

        //TYPE
        public override string TypeToString { get { return "genericMesh"; } }
        public override string TypeToStringShort { get { return "GEN"; } }

        //COPIER
        public override void Copy(bool excludeCoordinates, params NamedMapObject[] source)
        {
            bool same, found;
            if (source.Count() == 0)
                return;

            //meshFileName
            string tmp1 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("meshFileName"))
                    if (!found)
                    {
                        found = true;
                        tmp1 = ((NamedMapObject_genericMesh)item)._meshFileName;
                    }
                    else
                        same = same && (((NamedMapObject_genericMesh)item)._meshFileName != tmp1);
            if (found && same)
                this._meshFileName = tmp1;

            //textureFileName
            string tmp2 = "";
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("textureFileName"))
                    if (!found)
                    {
                        found = true;
                        tmp2 = ((NamedMapObject_genericMesh)item)._textureFileName;
                    }
                    else
                        same = same && (((NamedMapObject_genericMesh)item)._textureFileName != tmp2);
            if (found && same)
                this._textureFileName = tmp2;

			//hullRace
			string tmp5 = "";
			same = true; found = false;
			foreach (NamedMapObject item in source)
				if (item.IsPropertyAvailable("hullRace"))
					if (!found)
					{
						found = true;
						tmp5 = ((NamedMapObject_genericMesh)item)._hullRace;
					}
					else
						same = same && (((NamedMapObject_genericMesh)item)._hullRace != tmp5);
			if (found && same)
				this._hullRace = tmp5;

			//hullType
			string tmp6 = "";
			same = true; found = false;
			foreach (NamedMapObject item in source)
				if (item.IsPropertyAvailable("hullType"))
					if (!found)
					{
						found = true;
						tmp6 = ((NamedMapObject_genericMesh)item)._hullType;
					}
					else
						same = same && (((NamedMapObject_genericMesh)item)._hullType != tmp6);
			if (found && same)
				this._hullType = tmp6;

            //fakeShields
            FakeShields tmp3 = new FakeShields(true);
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("fakeShields"))
                    if (!found)
                    {
                        found = true;
                        tmp3 = ((NamedMapObject_genericMesh)item)._fakeShields;
                    }
                    else
                        same = same && (((NamedMapObject_genericMesh)item)._fakeShields != tmp3);
            if (found && same)
                this._fakeShields = tmp3;

            //Color
            Color tmp4 = new Color();
            same = true; found = false;
            foreach (NamedMapObject item in source)
                if (item.IsPropertyAvailable("color"))
                    if (!found)
                    {
                        found = true;
                        tmp4 = ((NamedMapObject_genericMesh)item)._color;
                    }
                    else
                        same = same && (((NamedMapObject_genericMesh)item)._color != tmp4);
            if (found && same)
                this._color = tmp4;
            
            base.Copy(excludeCoordinates, source);
        }

        //TO XML
        public override XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties, string type = "")
        {
            if (type == "") type = TypeToString;//If this was the first called ToXml - set this object's type
            XmlElement create = base.ToXml(xDoc, missingProperties, type);

            __AddNewAttribute(xDoc, create, "colorBlue", Helper.DoubleFormatToString("{0:0.0}",Color_Blue));

            __AddNewAttribute(xDoc, create, "colorGreen", Helper.DoubleFormatToString("{0:0.0}", Color_Green));

            __AddNewAttribute(xDoc, create, "colorRed", Helper.DoubleFormatToString("{0:0.0}", Color_Red));

            if (fakeShields.Front != -1 || IsPropertyMandatory("fakeShieldsFront"))
                __AddNewAttribute(xDoc, create, "fakeShieldsFront", String.Format("{0}", fakeShields.Front));
            else
                __RememberPropertyIfMissing(missingProperties, "fakeShieldsFront", IsPropertyMandatory("fakeShieldsFront"));

            if (fakeShields.Rear != -1 || IsPropertyMandatory("fakeShieldsRear"))
                __AddNewAttribute(xDoc, create, "fakeShieldsRear", String.Format("{0}", fakeShields.Rear));
            else
                __RememberPropertyIfMissing(missingProperties, "fakeShieldsRear", IsPropertyMandatory("fakeShieldsRear"));

            if (fakeShields.hasFrequency != false || IsPropertyMandatory("hasFakeShldFreq"))
                __AddNewAttribute(xDoc, create, "hasFakeShldFreq", String.Format("{0}", (fakeShields.hasFrequency ? 1 : 0)));
            else
                __RememberPropertyIfMissing(missingProperties, "hasFakeShldFreq", IsPropertyMandatory("hasFakeShldFreq"));

			if (hullRace != "")
				__AddNewAttribute(xDoc, create, "hullRace", String.Format("{0}", hullRace));
			else
				__RememberPropertyIfMissing(missingProperties, "hullRace", IsPropertyMandatory("hullRace"));

			if (hullType != "")
				__AddNewAttribute(xDoc, create, "hullType", String.Format("{0}", hullType));
			else
				__RememberPropertyIfMissing(missingProperties, "hullType", IsPropertyMandatory("hullType"));
			
			if (meshFileName != "")
                __AddNewAttribute(xDoc, create, "meshFileName", String.Format("{0}", meshFileName));
            else
                __RememberPropertyIfMissing(missingProperties, "meshFileName", IsPropertyMandatory("meshFileName"));

            if (textureFileName != "")
                __AddNewAttribute(xDoc, create, "textureFileName", String.Format("{0}", textureFileName));
            else
                __RememberPropertyIfMissing(missingProperties, "textureFileName", IsPropertyMandatory("textureFileName"));
            return create;
        }

        //FROM XML
        public override void FromXml(XmlNode item)
        {
            base.FromXml(item);
            foreach (XmlAttribute att in item.Attributes)
                switch (att.Name)
                {
                    case "meshFileName":
                        _meshFileName = att.Value;
                        break;
                    case "textureFileName":
                        _textureFileName = att.Value;
                        break;
                    case "fakeShieldsFront":
                        _fakeShields.Front = Helper.StringToInt(att.Value);
                        break;
                    case "fakeShieldsRear":
                        _fakeShields.Rear = Helper.StringToInt(att.Value);
                        break;
                    case "hasFakeShldFreq":
                        _fakeShields.hasFrequency = Helper.StringToInt(att.Value) == 0 ? false : true;
                        break;
                    case "colorRed":
                        Color_Red = Helper.StringToDouble(att.Value);
                        break;
                    case "colorGreen":
                        Color_Green = Helper.StringToDouble(att.Value);
                        break;
                    case "colorBlue":
                        Color_Blue = Helper.StringToDouble(att.Value);
                        break;
                }
        }

        //CONSTRUCTOR
        public NamedMapObject_genericMesh(int posX = 0, int posY = 0, int posZ = 0, bool makeSelected = false, int angle = 0, string name = "", string _hull_Race = "", string _hull_Type="")
            : base(posX, posY, posZ, makeSelected, angle, name)
		{ this._meshFileName = ""; this._textureFileName = ""; this._hullRace = _hull_Race; this._hullType = _hull_Type; this._fakeShields = new FakeShields(true); this._color = Color.FromArgb(255,0,255); }

        #endregion
    }


}
