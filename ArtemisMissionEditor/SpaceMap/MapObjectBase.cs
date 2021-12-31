using System;
using System.Reflection;
using System.ComponentModel;
using System.Xml;
using System.Collections.Generic;

namespace ArtemisMissionEditor.SpaceMap
{
    public class MapObjectBase
    {
        /// <summary>
        /// Set the Browsable property.
        /// NOTE: Be sure to decorate the property with [Browsable(true)] or [Browsable(false)]
        /// </summary>
        /// <param name="PropertyName">Name of the variable</param>
        /// <param name="bIsBrowsable">Browsable Value</param>
        protected void SetBrowsableProperty(string strPropertyName, bool bIsBrowsable)
        {
            // Get the Descriptor's Properties
            PropertyDescriptor theDescriptor = TypeDescriptor.GetProperties(this.GetType())[strPropertyName];

            // Get the Descriptor's "Browsable" Attribute
            BrowsableAttribute theDescriptorBrowsableAttribute = (BrowsableAttribute)theDescriptor.Attributes[typeof(BrowsableAttribute)];
            FieldInfo isBrowsable = theDescriptorBrowsableAttribute.GetType().GetField("Browsable", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);

            // Set the Descriptor's "Browsable" Attribute
            isBrowsable.SetValue(theDescriptorBrowsableAttribute, bIsBrowsable);
        }

        private ExpressionMemberValueEditorConverter GetConverter(MethodBase method)
        {
            string propertyName = method.Name.Replace("set_", "").Replace("get_", "");
            PropertyDescriptorCollection pdc = System.ComponentModel.TypeDescriptor.GetProperties(this);
            PropertyDescriptor pd = pdc[propertyName];
            TypeConverter tc = pd.Converter;
            var emvec = tc as ExpressionMemberValueEditorConverter;
            return emvec;
        }

        protected string IntToStandardValue(MethodBase method, int intValue)
        {
            return GetConverter(method).IntToStandardValue(intValue);
        }

        protected int StandardValueToInt(MethodBase method, string standardValue, int defaultIntValue)
        {
            ExpressionMemberValueEditorConverter converter = GetConverter(method);
            if (converter.Editor.MenuValueToXml.ContainsKey(standardValue))
            {
                string xmlValue = converter.Editor.MenuValueToXml[standardValue];
                return Convert.ToInt32(xmlValue);
            }
            return defaultIntValue;
        }

        public static string ConvertPascalToDisplay(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (char.IsUpper(s[i]))
                {
                    s = s.Insert(i, " ");
                }
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

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
    }
}
