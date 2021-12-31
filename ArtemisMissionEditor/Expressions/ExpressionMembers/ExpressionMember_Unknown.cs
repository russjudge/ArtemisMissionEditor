using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// The expression member that represents an unknown statement
    /// </summary>
    public class ExpressionMember_Unknown : ExpressionMember
	{
		/// <summary>
		/// Gets the Xml representing this unknown member (returns whole Xml element).
		/// </summary>
		public override string GetValue(ExpressionMemberContainer container)
		{
			XmlDocument xDoc = new XmlDocument();
			XmlElement eResult = xDoc.CreateElement(container.Statement.Name);
			
			foreach (KeyValuePair<string, string> kvp in container.Statement.GetAttributes())
			{
				XmlAttribute cAtt = xDoc.CreateAttribute(kvp.Key);
				cAtt.Value = kvp.Value;
				eResult.Attributes.Append(cAtt);
			}
			if (!String.IsNullOrEmpty(container.Statement.Body))
				eResult.InnerText = container.Statement.Body;
			return eResult.OuterXml;
		}

        /// <summary>
        /// Set value of the current member (whole Xml element is supplied)
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			XmlDocument xDoc = new XmlDocument();
			try
			{
				xDoc.LoadXml(value);
				container.Statement.FromXml(xDoc.ChildNodes[0]);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + " " +ex.StackTrace);
			}
		}

        /// <summary>
        /// Get value of current member to display in the label (meaning, exactly what will appear in GUI labels).
        /// </summary>
        public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			return GetValue(container);
		}

        public ExpressionMember_Unknown() : base("", ExpressionMemberValueDescriptions.Unknown) { }
	}

}