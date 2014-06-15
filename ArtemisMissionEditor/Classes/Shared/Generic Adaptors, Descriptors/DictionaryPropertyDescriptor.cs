using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace ArtemisMissionEditor
{
    public sealed class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        //PropertyDescriptor provides 3 constructors. We want the one that takes a string and an array of attributes:

        IDictionary _dictionary;
        object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        //The attributes are used by PropertyGrid to organise the properties into categories, to display help text and so on. 
        //We don't bother with any of that at the moment, so we simply pass null.
        //The first interesting member is the PropertyType property. We just get the object out of the dictionary and ask it:
        public override Type PropertyType { get { if (_dictionary[_key] == null) return typeof(object); return _dictionary[_key].GetType(); } }

        //If you knew that all of your values were strings, for example, you could just return typeof(string).
        //Then we implement SetValue and GetValue:
        //The component parameter passed to these two methods is whatever value was returned from ICustomTypeDescriptor.GetPropertyOwner. 
        //If it weren't for the fact that we need the dictionary object in PropertyType, we could avoid using the _dictionary member, and just grab it using this mechanism.
        public override void SetValue(object component, object value) { _dictionary[_key] = value; }
        public override object GetValue(object component) { return _dictionary[_key]; }

        #region Boring Stuff

        public override bool IsReadOnly { get { return false; } }
        public override Type ComponentType { get { return null; } }
        public override bool CanResetValue(object component) { return false; }
        public override void ResetValue(object component) { }
        public override bool ShouldSerializeValue(object component) { return false; }

        #endregion

    }
}
