using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ArtemisMissionEditor
{
    internal class Coordinates3DConverter : ExpandableObjectConverter
    {

        public override bool CanConvertFrom(
              ITypeDescriptorContext context, Type t)
        {

            if (t == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(
              ITypeDescriptorContext context,
              CultureInfo info,
               object value)
        {

            if (value is string)
            {
                try
                {
                    string tmp = (string)value;
                    double tmpx, tmpy, tmpz;
                    tmp = tmp.Replace(" ", "");
                    string[] coords = tmp.Split(',');
                    if (coords.Count() != 3)
                        throw null;
					if (!(Helper.DoubleTryParse(coords[0], out tmpx) && Helper.DoubleTryParse(coords[1], out tmpy) && Helper.DoubleTryParse(coords[2], out tmpz)))
                        throw null;
                    Coordinates3D p = new Coordinates3D(true);
                    p.X = tmpx;
                    p.Y = tmpy;
                    p.Z = tmpz;
                    return p;
                }
                catch { }
                // if we got this far, complain that we
                // couldn't parse the string
                //
                throw new ArgumentException(
                   "Can not convert '" + (string)value +
                            "' to Position");

            }

            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(
                 ITypeDescriptorContext context,
                 CultureInfo culture,
                 object value,
                 Type destType)
        {
            if (destType == typeof(string) && value is Coordinates3D)
            {
                Coordinates3D p = (Coordinates3D)value;
                // simply build the string as "Last, First (Age)"
				return Helper.DoubleToString(p.X, false) + ", " + Helper.DoubleToString(p.Y, false) + ", " + Helper.DoubleToString(p.Z, false);
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    [TypeConverter(typeof(Coordinates3DConverter))]
    public struct Coordinates3D
    {
        public bool _allow_out_of_range;

        private double _x, _y, _z;

        [DisplayName("X"), Description("Abscissa, location on the axis going from right to left on the plane"), DefaultValue(0)]
        public double X
        {
            get { return _x; }
            set
            {
                if (!_allow_out_of_range && value < SpaceMap._minX)
                    value = SpaceMap._minX;
                if (!_allow_out_of_range && value > SpaceMap._maxX)
                    value = SpaceMap._maxX;
                _x = value;
            }
        }
        [DisplayName("Y"), Description("Height, location on the axis going from under the plane to over the plane"), DefaultValue(0)]
        public double Y
        {
            get { return _y; }
            set
            {
                if (!_allow_out_of_range && value < SpaceMap._minY)
                    value = SpaceMap._minY;
                if (!_allow_out_of_range && value > SpaceMap._maxY)
                    value = SpaceMap._maxY;
                _y = value;
            }
        }

        [DisplayName("Z"), Description("Ordinate, location on the axis going from top to botton on the plane"), DefaultValue(0)]
        public double Z
        {
            get { return _z; }
            set
            {
                if (!_allow_out_of_range && value < SpaceMap._minZ)
                    value = SpaceMap._minZ;
                if (!_allow_out_of_range && value > SpaceMap._maxZ)
                    value = SpaceMap._maxZ;
                _z = value;
            }
        }

        [Browsable(false)]
        public double X_Scr
        {
            get { return SpaceMap._maxX - _x; }
            set { X = SpaceMap._maxX - value; }
        }

        [Browsable(false)]
        public double Y_Scr
        {
            get { return _y; }
            set { Y = value; }
        }

        [Browsable(false)]
        public double Z_Scr
        {
            get { return _z; }
            set { Z = value; }
        }

		public void Validate()
		{
			bool tmp = _allow_out_of_range;
			_allow_out_of_range = false;
			X = X;
			Y = Y;
			Z = Z;
			_allow_out_of_range = tmp;
		}

        public static bool operator ==(Coordinates3D c1, Coordinates3D c2) { return c1._x == c2._x && c1._y == c2._y && c1._z == c2._z; }
        public static Coordinates3D operator +(Coordinates3D c1, Coordinates3D c2) { return new Coordinates3D(true, c1._x + c2._x, c1._y + c2._y, c1._z + c2._z,false); }
        public static Coordinates3D operator -(Coordinates3D c1, Coordinates3D c2) { return new Coordinates3D(true, c1._x - c2._x, c1._y - c2._y, c1._z - c2._z,false); }
        public static bool operator !=(Coordinates3D c1, Coordinates3D c2) { return !(c1 == c2); }
        public override bool Equals(object obj) { if (obj.GetType() == typeof(Coordinates3D)) return this == (Coordinates3D)obj; return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public Coordinates3D(bool useThisConstructor, double x = 0, double y = 0, double z = 0,bool useScreenCoords = true)
		{ this._x = (useScreenCoords ? SpaceMap._maxX - x : x); this._y = y; this._z = z; this._allow_out_of_range = false; Validate(); }

    }

    internal class Coordinates3DUnboundConverter : ExpandableObjectConverter
    {

        public override bool CanConvertFrom(
              ITypeDescriptorContext context, Type t)
        {

            if (t == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(
              ITypeDescriptorContext context,
              CultureInfo info,
               object value)
        {

            if (value is string)
            {
                try
                {
                    string tmp = (string)value;
                    int tmpx, tmpy, tmpz;
                    tmp = tmp.Replace(" ", "");
                    string[] coords = tmp.Split(',');
                    if (coords.Count() != 3)
                        throw null;
                    if (!(Helper.IntTryParse(coords[0], out tmpx) && Helper.IntTryParse(coords[1], out tmpy) && Helper.IntTryParse(coords[2], out tmpz)))
                        throw null;
					Coordinates3DUnbound p = new Coordinates3DUnbound(true, tmpx, tmpy, tmpz, false);
                    return p;
                }
                catch { }
                // if we got this far, complain that we
                // couldn't parse the string
                //
                throw new ArgumentException(
                   "Can not convert '" + (string)value +
                            "' to Position");

            }

            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(
                 ITypeDescriptorContext context,
                 CultureInfo culture,
                 object value,
                 Type destType)
        {
            if (destType == typeof(string) && value is Coordinates3DUnbound)
            {
                Coordinates3DUnbound p = (Coordinates3DUnbound)value;
                // simply build the string as "Last, First (Age)"
                return p.X.ToString() + ", " + p.Y.ToString() + ", " + p.Z.ToString();
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    [TypeConverter(typeof(Coordinates3DUnboundConverter))]
    public struct Coordinates3DUnbound
    {
        private double _x, _y, _z;

        [DisplayName("X"), Description("Abscissa, location on the axis going from right to left on the plane"), DefaultValue(0)]
        public double X { get { return _x; } set { _x = value; } }

        [DisplayName("Y"), Description("Height, location on the axis going from under the plane to over the plane"), DefaultValue(0)]
        public double Y { get { return _y; } set { _y = value; } }

        [DisplayName("Z"), Description("Ordinate, location on the axis going from top to botton on the plane"), DefaultValue(0)]
        public double Z { get { return _z; } set { _z = value; } }

        [Browsable(false)]
        public double X_Scr
        {
            get { return SpaceMap._maxX - _x; }
            set { _x = SpaceMap._maxX - value; }
        }

        [Browsable(false)]
        public double Y_Scr
        {
            get { return _y; }
            set { _y = value; }
        }

        [Browsable(false)]
        public double Z_Scr
        {
            get { return _z; }
            set { _z = value; }
        }

        public static bool operator ==(Coordinates3DUnbound c1, Coordinates3DUnbound c2) { return c1._x == c2._x && c1._y == c2._y && c1._z == c2._z; }
        public static Coordinates3DUnbound operator +(Coordinates3DUnbound c1, Coordinates3DUnbound c2) { return new Coordinates3DUnbound(true, c1._x + c2._x, c1._y + c2._y, c1._z + c2._z, false); }
        public static Coordinates3DUnbound operator -(Coordinates3DUnbound c1, Coordinates3DUnbound c2) { return new Coordinates3DUnbound(true, c1._x - c2._x, c1._y - c2._y, c1._z - c2._z, false); }
        public static bool operator !=(Coordinates3DUnbound c1, Coordinates3DUnbound c2) { return !(c1 == c2); }
        public override bool Equals(object obj) { if (obj.GetType() == typeof(Coordinates3DUnbound)) return this == (Coordinates3DUnbound)obj; return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public Coordinates3DUnbound(bool useThisConstructor, double x = 0, double y = 0, double z = 0, bool useScreenCoords = false)
            
        { this._x = (useScreenCoords ? SpaceMap._maxX - x : x); this._y = y; this._z = z; }

    }
}
