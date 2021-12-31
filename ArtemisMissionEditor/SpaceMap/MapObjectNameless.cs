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
using ArtemisMissionEditor.Expressions;

namespace ArtemisMissionEditor.SpaceMap
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

    public sealed class NebulaTypeConverter : ExpressionMemberValueEditorConverter
    {
        public NebulaTypeConverter() : base(ExpressionMemberValueEditors.NebulaType) { }
    }

    public enum MapObjectNamelessType
    {
        nothing,
        nebulas,
        asteroids,
        mines
    }

    public sealed class MapObjectNameless : MapObjectBase
    {
        public static readonly int _minRadius = 0;
        public static readonly int _maxRadius = 100000;
        public static readonly int _minCount = 0;
        //public static readonly int _maxCount = 500;  //This is written in mission docs, but actually isn't checked
        public static readonly int _maxCount = 100000;

        //IMPORTED
        [Browsable(false)]
        public bool Imported { get; set; }

        //COORDINATES
        public Coordinates3DUnbound _coordinatesStart;
        [DisplayName("\t\tStart Coordinates"), Description("The coordinates of the object's start point on the space map"), Category("\t\t\tPosition")]
        public Coordinates3DUnbound CoordinatesStart { get { return _coordinatesStart; } set { _coordinatesStart = value; if (_radius > 0) _coordinatesEnd = value; } }

        public Coordinates3DUnbound _coordinatesEnd;
        [DisplayName("\tEnd Coordinates"), Description("The coordinates of the object's end point on the space map"), Category("\t\t\tPosition")]
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
        [DisplayName("Random seed"), Description("Indicates random seed used to produce nameless entities' individual coordinates. NOTE: Using the same random seed with same other parameters will create the same nameless object distribution over every mission playthrough. However, the editor cannot accurately recreate the random position generation algorithm that Artemis uses, therefore nameless entities' positions you will see in the editor will not be identical to what you will see in-game."), DefaultValue(0)]
        public int randomSeed { get { return _randomSeed; } set { if (value < 0) _randomSeed = 0; else _randomSeed = value; } }

        private int _nebType;
        [DisplayName("Color"), Browsable(false), Description("Indicates the nebula color."), DefaultValue(0), TypeConverter(typeof(NebulaTypeConverter))]
        public string NebulaTypeToString_Display
        {
            get { return IntToStandardValue(MethodBase.GetCurrentMethod(), _nebType); }
            set { _nebType = StandardValueToInt(MethodBase.GetCurrentMethod(), value, 0); }
        }

        //TYPE
        public MapObjectNamelessType _type;
        [Browsable(false)]
        public string TypeToString
        {
            get
            {
                switch (_type)
                {
                    case MapObjectNamelessType.asteroids:
                        return "asteroids";
                    case MapObjectNamelessType.mines:
                        return "mines";
                    case MapObjectNamelessType.nebulas:
                        return "nebulas";
                    case MapObjectNamelessType.nothing:
                        return "nothing";
                    default:
                        return "";
                }
            }
        }

        [DisplayName("Type"), Description("Indicates object's type (nebula, asteroid etc.)"), Category("ID"), TypeConverter(typeof(NamelessTypeConverter))]
        public string TypeToString_Display
        {
            get { return ConvertPascalToDisplay(TypeToString); }

            set
            {
                SetBrowsableProperty(nameof(NebulaTypeToString_Display), false);
                switch (value.ToLower())
                {
                    case "asteroids":
                        _type = MapObjectNamelessType.asteroids;
                        break;
                    case "mines":
                        _type = MapObjectNamelessType.mines;
                        break;
                    case "nebulas":
                        SetBrowsableProperty(nameof(NebulaTypeToString_Display), true);
                        _type = MapObjectNamelessType.nebulas;
                        break;
                    case "nothing":
                        _type = MapObjectNamelessType.nothing;
                        break;
                    default:
                        _type = MapObjectNamelessType.nothing;
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
                    case MapObjectNamelessType.asteroids:
                        return "Asteroid";
                    case MapObjectNamelessType.mines:
                        return "Mine    ";
                    case MapObjectNamelessType.nebulas:
                        return "Nebula  ";
                    case MapObjectNamelessType.nothing:
                        return "Nothing ";
                    default:
                        return _type.ToString();
                }
            }
        }

        #region SHARED

        public static MapObjectNameless NewFromXml(XmlNode item)
        {
            if (!(item is XmlElement))
                return null;
            string type = "";
            MapObjectNameless mo = null;
            foreach (XmlAttribute att in item.Attributes)
                if (att.Name == "type") type = att.Value.ToString();

            switch (type)
            {
                case "nebulas":
                    mo = new MapObjectNameless(type: type);
                    break;
                case "asteroids":
                    mo = new MapObjectNameless(type: type);
                    break;
                case "mines":
                    mo = new MapObjectNameless(type: type);
                    break;
                default:
                    return null;
            }

            mo.FromXml(item);
            return mo;
        }

        #endregion

        //COPIER
        public void Copy(bool excludeStartCoordinate, MapObjectNameless source)
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
            if (this._type == MapObjectNamelessType.nothing)
                this._type = source._type;
        }

        /// <summary>
        /// Output Xml text that represents this nameless object
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="missingProperties">Will be filled with names of parameters that are missing. Currently this method does not fill it, but added for consistency with Nameds and future use.</param>
        /// <returns></returns>
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

            if (_nebType > 0)
                __AddNewAttribute(xDoc, create, "nebType", _nebType.ToString());

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
                    case "nebType":
                        _nebType = Helper.StringToInt(att.Value);
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
        public MapObjectNameless(int? nposX = null, int posY = 0, int posZ = 0, string type = "")
        {
            int posX = nposX ?? Space.MaxX;
            _type = MapObjectNamelessType.nothing;
            TypeToString_Display = type;
            _coordinatesStart = new Coordinates3DUnbound(true, posX, posY, posZ, true);
            _coordinatesEnd = this._coordinatesStart;
            _count = 1;
            Imported = false;
        }
    }
}
