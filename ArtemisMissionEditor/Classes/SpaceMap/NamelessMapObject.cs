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

    public sealed class NamelessTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override TypeConverter.StandardValuesCollection
        GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> tmp = new List<string>();

            tmp.Add("Asteroids");
            tmp.Add("Mines");
            tmp.Add("Nebulas");

            return new StandardValuesCollection(tmp.ToArray());
        }

    }

    public enum NamelessMapObjectType
    {
        nothing,
        nebulas,
        asteroids,
        mines
    }

    //NANASHI RENJU, DETTE KOI!!!
    public sealed class NamelessMapObject
    {
        public static int _minRadius = 0;
        public static int _maxRadius = 100000;
        public static int _minCount = 0;
        //public static int _maxCount = 500;  //ISNT CHECKED LOL!
        public static int _maxCount = 100000;

        //IMPORTED
		[Browsable(false)]
        public bool Imported { get; set; }

        //COORDINATES
        public Coordinates3DUnbound _coordinatesStart;
        [DisplayName("\t\tStart Coordinates"), Description("The coordinates of the object start point's abscissa, height, ordinate on the space map"), Category("\t\t\tPosition")]
        public Coordinates3DUnbound CoordinatesStart { get { return _coordinatesStart; } set { _coordinatesStart = value; if (_radius > 0) _coordinatesEnd = value; } }

        public Coordinates3DUnbound _coordinatesEnd;
        [DisplayName("\tEnd Coordinates"), Description("The coordinates of the object end point's abscissa, height, ordinate on the space map"), Category("\t\t\tPosition")]
		public Coordinates3DUnbound CoordinatesEnd { get { return _coordinatesEnd; } set { _coordinatesEnd = value; if (_coordinatesEnd != null && _coordinatesEnd != _coordinatesStart) _radius = 0; } }
        private bool ShouldSerializeCoordinatesEnd()
        {
            return _radius ==0;
        }
        //ANGLES

        private double? _aStart;
        [DisplayName("\t\tStart Angle"), Description("Indicates object's start angle in degrees relative to the upright position, clockwise rotation considered positive"), Category("\t\t\tPosition"), DefaultValue(0)]
		public double? A_Start_deg
        {
            get { return _aStart; }
            set
            {
                if (value == null) { _aStart = null; return; }
                _aStart = value;
            }
        }
        [Browsable(false)]
        public double? A_Start_rad
        {
            get { if (_aStart==null)return null; return (_aStart) / 360.0 * Math.PI * 2.0; }
            set
            {
                if (value == null) { _aStart = null; return; }
				_aStart = Math.Round((value ?? 0) / 2.0 / Math.PI * 3600.0) / 10.0;
            }
        }

		private double? _aEnd;
        [DisplayName("\tEnd Angle"), Description("Indicates object's end angle in degrees relative to the upright position, clockwise rotation considered positive"), Category("\t\t\tPosition"), DefaultValue(0)]
		public double? A_End_deg
        {
            get { return _aEnd; }
            set
            {
                if (value == null) { _aEnd = null; return; }
                _aEnd = value;
            }
        }
        [Browsable(false)]
        public double? A_End_rad
        {
            get { if (_aEnd == null) return null; return (_aEnd) / 360.0 * Math.PI * 2.0; }
            set
            {
                if (value == null) { _aEnd = null; return; }
				_aEnd = Math.Round((value ?? 0) / 2.0 / Math.PI * 3600.0) / 10.0;
            }
        }

        //PROPERTIES
        private int _count;
        [DisplayName("Count"), Description("Indicates amount of nameless entities spawned"), DefaultValue(0)]
        public int count
        {
            get { return _count; }
            set
            {
                if (value < _minCount)
                    value = _minCount;
                if (value > _maxCount)
                    value = _maxCount;
                _count = value;
            }
        }

        private int _radius;
        [DisplayName("Radius"), Description("Indicates radius spread for the nameless entities spawned"), Category("\t\t\tPosition"), DefaultValue(0)]
        public int radius
        {
            get { return _radius; }
            set
            {
                if (value < _minRadius)
                    value = _minRadius;
                if (value > _maxRadius)
                    value = _maxRadius;
                _radius = value;

                if (_radius != 0)
                    _coordinatesEnd = _coordinatesStart;
            }
        }
        private int _randomRange;
        [DisplayName("Random range"), Description("Indicates random deviation from the normal position for every nameless entity spawned"), DefaultValue(0)]
        public int randomRange
        {
            get { return _randomRange; }
            set
            {
                if (value < _minRadius)
                    value = _minRadius;
                if (value > _maxRadius)
                    value = _maxRadius;
                _randomRange = value;
            }
        }
        private int _randomSeed;
        [DisplayName("Random seed"), Description("Indicates random seed used to produce nameless entities's individual coordinates. NOTE: Using same random seed with same other parameters will create same nameless object distribution over every mission playthrough. However, editor cannot accurately recreate random position generation algorithm that Artemis uses, therefore nameless entities's position you will see in the editor will not be identical to what you will see ingame."), DefaultValue(0)]
        public int randomSeed { get { return _randomSeed; } set { if (value < 0) _randomSeed = 0; else _randomSeed = value; } }

        //TYPE
        public NamelessMapObjectType _type;
        [Browsable(false)]
        public string TypeToString
        {
            get
            {
                switch (_type)
                {
                    case NamelessMapObjectType.asteroids:
                        return "asteroids";
                    case NamelessMapObjectType.mines:
                        return "mines";
                    case NamelessMapObjectType.nebulas:
                        return "nebulas";
                    case NamelessMapObjectType.nothing:
                        return "nothing";
                    default:
                        return "";
                }
            }

        }
        [DisplayName("Type"), Description("Indicates object's type (nebula, asteroid etc.)"), Category("ID"), TypeConverter(typeof(NamelessTypeConverter))]
        public string TypeToString_Display
        {
            get
            {
                int i;
                string s = TypeToString;

                if (string.IsNullOrEmpty(s)) return string.Empty;

                for (i = s.Length - 1; i >= 0; i--)
                {
                    if (char.IsUpper(s[i]))
                    {
                        s = s.Insert(i, " ");
                    }
                }
                return char.ToUpper(s[0]) + s.Substring(1);

            }

            set
            {
                switch (value.ToLower())
                {
                    case "asteroids":
                        _type = NamelessMapObjectType.asteroids;
                        break;
                    case "mines":
                        _type = NamelessMapObjectType.mines;
                        break;
                    case "nebulas":
                        _type = NamelessMapObjectType.nebulas;
                        break;
                    case "nothing":
                        _type = NamelessMapObjectType.nothing;
                        break;
                    default:
                        _type = NamelessMapObjectType.nothing;
                        break;
                }
            }
        }
        [Browsable(false)]
        public string TypeToStringShort
        {
            get
            {
                switch (_type)
                {
                    case NamelessMapObjectType.asteroids:
                        return "Asteroid";
                    case NamelessMapObjectType.mines:
                        return "Mine    ";
                    case NamelessMapObjectType.nebulas:
                        return "Nebula  ";
                    case NamelessMapObjectType.nothing:
                        return "Nothing ";
                    default:
                        throw new Exception("FAIL! Nameless bject has an unnormal Type: "+_type.ToString());
                }
            }
        }

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

        private static void __AddNewAttribute(XmlDocument doc, XmlElement element, string name, string value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value;
            element.Attributes.Append(att);
        }

        private static void __RememberPropertyIfMissing(List<string> missingProperties, string pName, bool required)
        {
            if (!required) return;
            missingProperties.Add(pName);
        }

        public static NamelessMapObject NewFromXML(XmlNode item)
        {
			if (!(item is XmlElement))
				return null;
			string type = "";
            NamelessMapObject mo = null;
            foreach (XmlAttribute att in item.Attributes)
                if (att.Name == "type") type = att.Value.ToString();

            switch (type)
            {
                case "nebulas":
                    mo = new NamelessMapObject(type: type);
                    break;
                case "asteroids":
                    mo = new NamelessMapObject(type: type);
                    break;
                case "mines":
                    mo = new NamelessMapObject(type: type);
                    break;
                default:
                    return null;
            }

            mo.FromXml(item);
            return mo;
        }

        #endregion

        //COPIER
        public void Copy(bool excludeStartCoordinate, NamelessMapObject source)
        {
            if (source == null)
                return;
            if (!excludeStartCoordinate)
                this._coordinatesStart = source._coordinatesStart;
            this._coordinatesEnd = this._coordinatesStart + source._coordinatesEnd - source._coordinatesStart;
            this._aEnd = source._aEnd;
            this._aStart = source._aStart;
            this._count = source._count;
            this._radius = source._radius;
            this._randomRange = source._randomRange;
            this._randomSeed = source._randomSeed;
            if (this._type == NamelessMapObjectType.nothing)
                this._type = source._type;
        }

        //TO XML
        public XmlElement ToXml(XmlDocument xDoc, List<string> missingProperties)
        {
            XmlElement create = xDoc.CreateElement("create");

            __AddNewAttribute(xDoc, create, "type", TypeToString);

            __AddNewAttribute(xDoc, create, "startX", Helper.DoubleToString(_coordinatesStart.X));
            __AddNewAttribute(xDoc, create, "startY", Helper.DoubleToString(_coordinatesStart.Y));
            __AddNewAttribute(xDoc, create, "startZ", Helper.DoubleToString(_coordinatesStart.Z));

			if (radius == 0)
			{
				__AddNewAttribute(xDoc, create, "endX", Helper.DoubleToString(_coordinatesEnd.X));
				__AddNewAttribute(xDoc, create, "endY", Helper.DoubleToString(_coordinatesEnd.Y));
				__AddNewAttribute(xDoc, create, "endZ", Helper.DoubleToString(_coordinatesEnd.Z));
			}

            if (A_Start_deg != null || A_End_deg != null)
            {
                __AddNewAttribute(xDoc, create, "startAngle", (A_Start_deg??0).ToString());
                if (_aStart != _aEnd)
                    __AddNewAttribute(xDoc, create, "endAngle", (A_End_deg??0).ToString());
            }

            __AddNewAttribute(xDoc, create, "count", _count.ToString());

            __AddNewAttribute(xDoc, create, "radius", _radius.ToString());

            if (_randomSeed > 0)
                __AddNewAttribute(xDoc, create, "randomSeed", _randomSeed.ToString());

            if (_randomRange > 0)
                __AddNewAttribute(xDoc, create, "randomRange", _randomRange.ToString());

            return create;
        }

        //FROM XML
        public void FromXml(XmlNode item)
        {
            bool end_C_specified = false;
            bool end_A_specified = false;
            foreach (XmlAttribute att in item.Attributes)
            {

                switch (att.Name)
                {
                    case "startX":
                        _coordinatesStart.X = Helper.StringToDouble(att.Value);
                        break;
                    case "startY":
                        _coordinatesStart.Y = Helper.StringToDouble(att.Value);
                        break;
                    case "startZ":
                        _coordinatesStart.Z = Helper.StringToDouble(att.Value);
                        break;
                    case "endX":
                        _coordinatesEnd.X = Helper.StringToDouble(att.Value);
                        end_C_specified = true;
                        break;
                    case "endY":
                        _coordinatesEnd.Y = Helper.StringToDouble(att.Value);
                        end_C_specified = true;
                        break;
                    case "endZ":
                        _coordinatesEnd.Z = Helper.StringToDouble(att.Value);
                        end_C_specified = true;
                        break;
                    case "startAngle":
                        A_Start_deg = Helper.StringToDouble(att.Value);
                        break;
                    case "endAngle":
                        A_End_deg = Helper.StringToDouble(att.Value);
                        end_A_specified = true;
                        break;
                    case "randomSeed":
                        _randomSeed = Helper.StringToInt(att.Value);
                        break;
                    case "count":
                        _count = Helper.StringToInt(att.Value);
                        break;
                    case "radius":
                        _radius = Helper.StringToInt(att.Value);
                        break;
                    case "randomRange":
                        _randomRange = Helper.StringToInt(att.Value);
                        break;

                }
            }
            if (!end_A_specified) _aEnd = _aStart;
			if (!end_C_specified) _coordinatesEnd = _coordinatesStart;
        }

        //CONSTRUCTOR
        public NamelessMapObject(int? nposX = null, int posY=0, int posZ = 0, string type = "")
        {
            int posX = nposX ?? SpaceMap._maxX;
			_type = NamelessMapObjectType.nothing;
            TypeToString_Display = type;
            _coordinatesStart = new Coordinates3DUnbound(true, posX, posY, posZ, true);
            _coordinatesEnd = this._coordinatesStart;
            _count = 1;
            Imported = false;
        }


    }
}
