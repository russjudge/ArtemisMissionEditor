using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ArtemisMissionEditor.SpaceMap
{
    internal class FakeShieldsConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
                return true;
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
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
                    if (!(Helper.IntTryParse(coords[0], out tmpx) && Helper.IntTryParse(coords[1], out tmpy) && Helper.IntTryParse(coords[2], out tmpz)&& (tmpz==1||tmpz==0)))
                        throw null;
                    FakeShields p = new FakeShields(true);
                    p.Front = tmpx;
                    p.Rear = tmpy;
                    p.hasFrequency = tmpz==1;
                    return p;
                }
                catch { }
                // if we got this far, complain that we
                // couldn't parse the string
                //
                throw new ArgumentException("Can not convert '" + (string)value + "' to FakeShields");
            }

            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is FakeShields)
            {
                FakeShields p = (FakeShields)value;
                return p.Front.ToString() + ", " + p.Rear.ToString() + ", " + (p.hasFrequency ? "1" : "0");
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    [TypeConverter(typeof(FakeShieldsConverter))]
    public struct FakeShields
    {
        private int _Front;
        [DisplayName("\tFront"), Description("Front fake shields of the generic mesh, -1 means no fake shields"),DefaultValue(-1)]
        public int Front { get { return _Front; } set { _Front = value; } }

        private int _Rear;
        [DisplayName("\tRear"), Description("Rear fake shields of the generic mesh, -1 means no fake shields"), DefaultValue(-1)]
        public int Rear { get { return _Rear; } set { _Rear = value; } }

        private bool _hasFrequency;
        [DisplayName("Has frequency"), Description("Indicates whether fake shields have a frequency graph (phaser resistance)"), DefaultValue(false)]
        public bool hasFrequency { get { return _hasFrequency; } set { _hasFrequency = value; } }

        public static bool operator ==(FakeShields c1, FakeShields c2) { return c1._Front == c2._Front && c1._Rear == c2._Rear && c1._hasFrequency == c2._hasFrequency; }
        public static bool operator !=(FakeShields c1, FakeShields c2) { return !(c1 == c2); }
        public override bool Equals(object obj) { if (obj.GetType() == typeof(FakeShields)) return this == (FakeShields)obj; return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }
        
        public FakeShields(bool useThisConstructor, int front = -1, int rear = -1, bool hasFrequency = false)
        { this._Front = front; this._Rear = rear; this._hasFrequency = hasFrequency; }
    }

}
