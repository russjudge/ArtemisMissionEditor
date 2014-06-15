using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	/// <summary>
	/// Unknown statement which is allowed to be edited "as is" with this special class
	/// </summary>
	public class ExpressionMember_Unknown : ExpressionMember
	{
		/// <summary>
		/// Gets the xml representing this unknown member
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
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
			if (container.Statement.Body != "")
				eResult.InnerText = container.Statement.Body;
			return eResult.OuterXml;
		}

		/// <summary>
		/// Attempt to parse the value as an xml
		/// </summary>
		/// <param name="container"></param>
		/// <param name="value"></param>
		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			XmlDocument xDoc = new XmlDocument();
			try
			{
				xDoc.LoadXml(value);
				container.Statement.FromXml(xDoc.ChildNodes[0]);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			return GetValue(container);
		}

		public ExpressionMember_Unknown()
			: base("", EMVD.GetItem("unknown"))
		{

		}
	}

}