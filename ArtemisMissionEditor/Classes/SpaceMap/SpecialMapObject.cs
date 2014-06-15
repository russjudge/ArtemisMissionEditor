using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Xml;

namespace ArtemisMissionEditor
{
    public enum SpecialMapObjectInOut 
    {
        Inside,
        Outside
    }
    public enum SpecialMapObjectSphereBox
    {
        Sphere,
        Box
    }

    public abstract class SpecialMapObject
    {
        public abstract bool ExecuteWheelAction(int delta, Keys modifierKeys, int x, int y, int pixelsInOne);

        public abstract bool ExecuteCommand(KeyCommands command, Keys modifierKeys, int x, int y, int pixelsInOne);

        public abstract bool ExecuteMouseAction(MouseButtons buttons, Keys modifierKeys, int x, int y, int pixelsInOne);

        protected static void __AddNewAttribute(XmlDocument doc, XmlElement element, string name, string value)
        {
            XmlAttribute att = doc.CreateAttribute(name);
            att.Value = value;
            element.Attributes.Append(att);
        }

        public abstract XmlNode ToXml(XmlDocument xDoc);

        public abstract void FromXml(XmlNode item);

        public static SpecialMapObject NewFromXml(XmlNode item)
        {
            SpecialMapObject smo = null;

            if (item.Name == "destroy_near")
                smo = new SpecialMapObject_DestroyCircle();
			if (item.Name == "add_ai" || item.Name == "direct")
				smo = new SpecialMapObject_PointTarget();
            if ((item.Name == "if_inside_box")
                || (item.Name == "if_inside_sphere")
                || (item.Name == "if_outside_box")
                || (item.Name == "if_outside_sphere"))
                smo = new SpecialMapObject_BoxSphere();

            if (smo == null)
                return null;

            smo.FromXml(item);
            return smo;
        }
    }

    public sealed class SpecialMapObject_DestroyCircle : SpecialMapObject
    {
        public static int _minRadius = 0;
        public static int _maxRadius = 100000;

        [DisplayName("Statement"), Description("Specifies the statement being edited"), Category("ID")]
        public string Statement { get { return "Destroy objects near..."; } }

        //COORDINATES
        public Coordinates3D _coordinates;
        [DisplayName("Coordinates"), Description("The coordinates of the point's abscissa, height, ordinate on the space map"), Category("\t\t\tPosition")]
        public Coordinates3D Coordinates { get { return _coordinates; } set { _coordinates = value; } }

        private int _radius;
        [DisplayName("Radius"), Description("Indicates radius within which nameless objects will be removed."), Category("\t\t\tPosition"), DefaultValue(0)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (value < _minRadius)
                    value = _minRadius;
                if (value > _maxRadius)
                    value = _maxRadius;
                _radius = value;

            }
        }

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
        [DisplayName("Object Type"), Description("Specifies nameless object type that is to be destroyed"), Category("ID"), TypeConverter(typeof(NamelessTypeConverter))]
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

        public override bool ExecuteWheelAction(int delta, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (modifierKeys == Keys.None)
            {
                _coordinates.Y_Scr += 10 * Math.Sign(delta);
                return true;
            }
            
            if (modifierKeys == Keys.Control)
            {
                _coordinates.Y_Scr += 100 * Math.Sign(delta);
                return true;
            }

            return false;
        }
        
        public override bool ExecuteCommand(KeyCommands command, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (command == KeyCommands.MoveSelected)
            {
                _coordinates.X_Scr = x;
                _coordinates.Z_Scr = y;
                return true;
            }

            return false;
        }

        public override bool ExecuteMouseAction(MouseButtons buttons, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (buttons == MouseButtons.Left && modifierKeys == Keys.None)
            {
                _coordinates.X_Scr = x;
                _coordinates.Z_Scr = y;
                return true;
            }

            if (buttons == MouseButtons.Left && modifierKeys == Keys.Alt)
            {
                Radius = (int)Math.Round(Math.Sqrt(Math.Pow(_coordinates.X_Scr - x, 2) + Math.Pow(_coordinates.Z_Scr - y, 2)));
                return true;
            }

            return false;
        }

        public override XmlNode ToXml(XmlDocument xDoc)
        {
            XmlElement statement = xDoc.CreateElement("destroy_near");

            __AddNewAttribute(xDoc, statement, "centerX", _coordinates.X.ToString());
            __AddNewAttribute(xDoc, statement, "centerY", _coordinates.Y.ToString());
            __AddNewAttribute(xDoc, statement, "centerZ", _coordinates.Z.ToString());
            __AddNewAttribute(xDoc, statement, "radius", Radius.ToString());
            __AddNewAttribute(xDoc, statement, "type", TypeToString);

            return statement;
        }

        public override void FromXml(XmlNode item)
        {
            if (item.Name == "destroy_near")
            {
                foreach (XmlAttribute att in item.Attributes)
                    switch (att.Name)
                    {
                        case "centerX":
                            _coordinates.X = Helper.StringToInt(att.Value);
                            break;
                        case "centerY":
                            _coordinates.Y = Helper.StringToInt(att.Value);
                            break;
                        case "centerZ":
                            _coordinates.Z = Helper.StringToInt(att.Value);
                            break;
                        case "radius":
                            Radius = Helper.StringToInt(att.Value);
                            break;
                        case "type":
                            TypeToString_Display = att.Value;
                            break;
                    }
            }
        }

		public SpecialMapObject_DestroyCircle()
		{
			_coordinates = new Coordinates3D();
			_radius = 0;
			_type = NamelessMapObjectType.nothing;
		}
    }


	public sealed class SpecialMapObject_PointTarget : SpecialMapObject
	{
		[DisplayName("Statement"), Description("Specifies the statement being edited"), Category("ID")]
		public string Statement { get { if (_nodeName == "add_ai") return "Add AI command \"POINT_THROTTLE\"..."; if (_nodeName == "direct") return "Direct generic mesh to..."; return ""; } }

		//COORDINATES
		public Coordinates3D _coordinates;
        [DisplayName("Coordinates"), Description("The coordinates of the object center's abscissa, height, ordinate on the space map"), Category("\t\t\tPosition")]
		public Coordinates3D Coordinates { get { return _coordinates; } set { _coordinates = value; } }

		public override bool ExecuteWheelAction(int delta, Keys modifierKeys, int x, int y, int pixelsInOne)
		{
			if (modifierKeys == Keys.None)
			{
				_coordinates.Y_Scr += 10 * Math.Sign(delta);
				return true;
			}

			if (modifierKeys == Keys.Control)
			{
				_coordinates.Y_Scr += 100 * Math.Sign(delta);
				return true;
			}

			return false;
		}

		private string _nodeName;

		private string _value4;

		private string _name;

        [DisplayName("Name"), Description("Name of the object that will receive the AI command.)"), Category("ID")]
        public string Name { get { return _name; } set { _name = value; } }

		private string _targetName;

		private string _useGMSelection;

		private string _type;

		public override bool ExecuteCommand(KeyCommands command, Keys modifierKeys, int x, int y, int pixelsInOne)
		{
			if (command == KeyCommands.MoveSelected)
			{
				_coordinates.X_Scr = x;
				_coordinates.Z_Scr = y;
				return true;
			}

			return false;
		}

		public override bool ExecuteMouseAction(MouseButtons buttons, Keys modifierKeys, int x, int y, int pixelsInOne)
		{
			if (buttons == MouseButtons.Left && modifierKeys == Keys.None)
			{
				_coordinates.X_Scr = x;
				_coordinates.Z_Scr = y;
				return true;
			}

			return false;
		}

		public override XmlNode ToXml(XmlDocument xDoc)
		{
			XmlElement statement = xDoc.CreateElement(_nodeName);

			if (_nodeName == "add_ai")
			{
				__AddNewAttribute(xDoc, statement, "value1", _coordinates.X.ToString());
				__AddNewAttribute(xDoc, statement, "value2", _coordinates.Y.ToString());
				__AddNewAttribute(xDoc, statement, "value3", _coordinates.Z.ToString());
				if (_value4 != null)
					__AddNewAttribute(xDoc, statement, "value4", _value4);
			}
			if (_nodeName == "direct")
			{
				__AddNewAttribute(xDoc, statement, "pointX", _coordinates.X.ToString());
				__AddNewAttribute(xDoc, statement, "pointY", _coordinates.Y.ToString());
				__AddNewAttribute(xDoc, statement, "pointZ", _coordinates.Z.ToString());
				if (_value4 != null)
					__AddNewAttribute(xDoc, statement, "scriptThrottle", _value4);
			}
			if (_type != null)
				__AddNewAttribute(xDoc, statement, "type", _type);
			if (_name != null)
				__AddNewAttribute(xDoc, statement, "name", _name);
			if (_targetName != null)
				__AddNewAttribute(xDoc, statement, "targetName", _targetName);
			if (_useGMSelection != null)
				__AddNewAttribute(xDoc, statement, "use_gm_selection", _useGMSelection);

			return statement;
		}

		public override void FromXml(XmlNode item)
		{
			_nodeName = item.Name;

			if (item.Name == "add_ai")
			{
				foreach (XmlAttribute att in item.Attributes)
					switch (att.Name)
					{
						case "value1":
							_coordinates.X = Helper.StringToInt(att.Value);
							break;
						case "value2":
							_coordinates.Y = Helper.StringToInt(att.Value);
							break;
						case "value3":
							_coordinates.Z = Helper.StringToInt(att.Value);
							break;
						case "value4":
							_value4 = att.Value;
							break;
						case "type":
							_type = att.Value;
							break;
						case "name":
							_name = att.Value;
							break;
						case "targetName":
							_targetName = att.Value;
							break;
						case "use_gm_selection":
							_useGMSelection = att.Value;
							break;
					}
			}
			if (item.Name == "direct")
			{
				foreach (XmlAttribute att in item.Attributes)
					switch (att.Name)
					{
						case "pointX":
							_coordinates.X = Helper.StringToInt(att.Value);
							break;
						case "pointY":
							_coordinates.Y = Helper.StringToInt(att.Value);
							break;
						case "pointZ":
							_coordinates.Z = Helper.StringToInt(att.Value);
							break;
						case "scriptThrottle":
							_value4 = att.Value;
							break;
						case "name":
							_name = att.Value;
							break;
						case "targetName":
							_targetName = att.Value;
							break;
					}
			}
		}

		public SpecialMapObject_PointTarget()
		{
			_coordinates = new Coordinates3D();
			_value4 = null;
			_type = null;
			_name = null;
			_useGMSelection = null;
		}
	}

    public sealed class SpecialMapObject_BoxSphere : SpecialMapObject
    {
        public static int _minRadius = 0;
        public static int _maxRadius = 100000;

        [DisplayName("Statement"), Description("Specifies the statement being edited"), Category("ID")]
        public string Statement { get { return "If object is " + (InOut == SpecialMapObjectInOut.Inside ? "inside " : "outside ") + (SphereBox == SpecialMapObjectSphereBox.Sphere ? "sphere" : "box"); } }

        //COORDINATES
        public Coordinates3D _coordinatesStart;
        [DisplayName("\t\tStart Coordinates"), Description("The coordinates of the sphere's center or rectangle's first edge point's abscissa, height, ordinate on the space map)"), Category("\t\t\tPosition")]
        public Coordinates3D CoordinatesStart { get { return _coordinatesStart; } set { _coordinatesStart = value; if (_radius > 0) _coordinatesEnd = value; } }

        public Coordinates3D _coordinatesEnd;
        [DisplayName("\tEnd Coordinates"), Description("The coordinates of the rectangle's second edge point's abscissa, height, ordinate on the space map"), Category("\t\t\tPosition")]
        public Coordinates3D CoordinatesEnd { get { return _coordinatesEnd; } set { _coordinatesEnd = value; if (_coordinatesEnd != _coordinatesStart) _radius = 0; } }
        private bool ShouldSerializeCoordinatesEnd()
        {
            return _radius == 0;
        }
        
        private int _radius;
        [DisplayName("Radius"), Description("Specifies sphere's radius"), Category("\t\t\tPosition"), DefaultValue(0)]
        public int Radius
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

        private string _nodeName;

        private string _name;
        [DisplayName("Name"), Description("Name of the object that will be checked by the condition."), Category("ID")]
        public string Name { get { return _name; } set { _name = value; } }

        [DisplayName("In/Out"), Description("Indicates wether the object should be located inside or outside for the condition to evaluate as TRUE")]
        public SpecialMapObjectInOut InOut
        {
            get
            {
                if (_nodeName == "if_inside_sphere" || _nodeName == "if_inside_box")
                    return SpecialMapObjectInOut.Inside;
                else
                    return SpecialMapObjectInOut.Outside;
            }
            set
            {
                if (value == SpecialMapObjectInOut.Inside)
                    if (_nodeName == "if_outside_box" || _nodeName == "if_inside_box")
                        _nodeName = "if_inside_box";
                    else
                        _nodeName = "if_inside_sphere";
                else
                    if (_nodeName == "if_outside_box" || _nodeName == "if_inside_box")
                        _nodeName = "if_outside_box";
                    else
                        _nodeName = "if_outside_sphere";
            }
        }

        [DisplayName("Sphere/Box"), Description("Indicates wether we are using a sphere or a box in this statement")]
        public SpecialMapObjectSphereBox SphereBox
        {
            get
            {
                if (_nodeName == "if_outside_box" || _nodeName == "if_inside_box")
                    return SpecialMapObjectSphereBox.Box;
                else
                    return SpecialMapObjectSphereBox.Sphere;
            }
            set
            {
                if (value == SpecialMapObjectSphereBox.Box)
                    if (_nodeName == "if_inside_box" || _nodeName == "if_inside_sphere")
                        _nodeName = "if_inside_box";
                    else
                        _nodeName = "if_outside_box";
                else
                    if (_nodeName == "if_inside_box" || _nodeName == "if_inside_sphere")
                        _nodeName = "if_inside_sphere";
                    else
                        _nodeName = "if_outside_sphere";
            }
        }

        public override bool ExecuteWheelAction(int delta, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (SphereBox == SpecialMapObjectSphereBox.Sphere)
            {
                if (modifierKeys == Keys.None)
                {
                    _coordinatesStart.Y_Scr += 10 * Math.Sign(delta);
                    _coordinatesEnd.Y_Scr += 10 * Math.Sign(delta);
                    return true;
                }

                if (modifierKeys == Keys.Control)
                {
                    _coordinatesStart.Y_Scr += 100 * Math.Sign(delta);
                    _coordinatesEnd.Y_Scr += 100 * Math.Sign(delta);
                    return true;
                }
            }

            if (modifierKeys == Keys.Alt)
            {
                if (InOut == SpecialMapObjectInOut.Inside)
                    InOut = SpecialMapObjectInOut.Outside;
                else
                    InOut = SpecialMapObjectInOut.Inside;
                return true;
            }
            return false;
        }

        public override bool ExecuteCommand(KeyCommands command, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (SphereBox == SpecialMapObjectSphereBox.Sphere)
            {
                if (command == KeyCommands.MoveSelected)
                {
                    _coordinatesStart.X_Scr = x;
                    _coordinatesStart.Z_Scr = y;
                    _coordinatesEnd.X_Scr = x;
                    _coordinatesEnd.Z_Scr = y;
                    return true;
                }
            }
            else
            {
                if (command == KeyCommands.MoveSelected)
                {
                    _coordinatesStart.X_Scr = x;
                    _coordinatesStart.Z_Scr = y;
                    return true;
                }
            }

            return false;
        }

        public override bool ExecuteMouseAction(MouseButtons buttons, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (buttons == MouseButtons.Left && modifierKeys == Keys.None)
            {
                _coordinatesStart.X_Scr = x;
                _coordinatesStart.Z_Scr = y;
                return true;
            }

            if (buttons == MouseButtons.Left && modifierKeys == (Keys.Alt | Keys.Control))
            {
                if (SphereBox == SpecialMapObjectSphereBox.Sphere)
                    SphereBox = SpecialMapObjectSphereBox.Box;
                else
                    SphereBox = SpecialMapObjectSphereBox.Sphere;
            }

            if (buttons == MouseButtons.Left && ((modifierKeys & Keys.Alt) == Keys.Alt))
            {
                if (SphereBox == SpecialMapObjectSphereBox.Box)
                {
                    _coordinatesEnd.X_Scr = x;
                    _coordinatesEnd.Z_Scr = y;
                    return true;
                }
                else
                {
                    Radius = (int)Math.Round(Math.Sqrt(Math.Pow(_coordinatesStart.X_Scr - x, 2) + Math.Pow(_coordinatesStart.Z_Scr - y, 2)));
                    return true;
                }
            }

            return false;
        }

        public override XmlNode ToXml(XmlDocument xDoc)
        {
            XmlElement statement = xDoc.CreateElement(_nodeName);

            if (SphereBox == SpecialMapObjectSphereBox.Sphere)
            {
                __AddNewAttribute(xDoc, statement, "centerX", _coordinatesStart.X.ToString());
                __AddNewAttribute(xDoc, statement, "centerY", _coordinatesStart.Y.ToString());
                __AddNewAttribute(xDoc, statement, "centerZ", _coordinatesStart.Z.ToString());
                __AddNewAttribute(xDoc, statement, "radius", Radius.ToString());
                if (_name != null)
                    __AddNewAttribute(xDoc, statement, "name", _name);
            }
            else
            {
                __AddNewAttribute(xDoc, statement, "leastX", Math.Min(_coordinatesStart.X,_coordinatesEnd.X).ToString());
                __AddNewAttribute(xDoc, statement, "leastZ", Math.Min(_coordinatesStart.Z,_coordinatesEnd.Z).ToString());
                __AddNewAttribute(xDoc, statement, "mostX", Math.Max(_coordinatesStart.X, _coordinatesEnd.X).ToString());
                __AddNewAttribute(xDoc, statement, "mostZ", Math.Max(_coordinatesStart.Z, _coordinatesEnd.Z).ToString());
                if (_name != null)
                    __AddNewAttribute(xDoc, statement, "name", _name);
            }

            return statement;
        }

        public override void FromXml(XmlNode item)
        {
            _nodeName = item.Name;

            if (SphereBox == SpecialMapObjectSphereBox.Sphere)
            {
                foreach (XmlAttribute att in item.Attributes)
                    switch (att.Name)
                    {
                        case "centerX":
                            _coordinatesStart.X = Helper.StringToInt(att.Value);
                            break;
                        case "centerY":
                            _coordinatesStart.Y = Helper.StringToInt(att.Value);
                            break;
                        case "centerZ":
                            _coordinatesStart.Z = Helper.StringToInt(att.Value);
                            break;
                        case "radius":
                            Radius = Helper.StringToInt(att.Value);
                            break;
                        case "name":
                            _name = att.Value;
                            break;
                    }

                _coordinatesEnd = _coordinatesStart;
            }
            else
            {
                foreach (XmlAttribute att in item.Attributes)
                    switch (att.Name)
                    {
                        case "leastX":
                            _coordinatesStart.X = Helper.StringToInt(att.Value);
                            break;
                        case "leastZ":
                            _coordinatesStart.Z = Helper.StringToInt(att.Value);
                            break;
                        case "mostX":
                            _coordinatesEnd.X = Helper.StringToInt(att.Value);
                            break;
                        case "mostZ":
                            _coordinatesEnd.Z = Helper.StringToInt(att.Value);
                            break;
                        case "name":
                            _name = att.Value;
                            break;
                    }
            }
        }

        public SpecialMapObject_BoxSphere()
        {
            _coordinatesStart = new Coordinates3D();
            _coordinatesEnd = new Coordinates3D();
            _radius = 0;
            _name = null;
        }
    }

}

