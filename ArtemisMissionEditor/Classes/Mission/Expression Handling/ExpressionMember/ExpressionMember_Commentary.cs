using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;
	
	/// <summary>
	/// Commentary statement
	/// </summary>
	public class ExpressionMember_Commentary : ExpressionMember
	{
		/// <summary>
		/// Gets the xml representing this unknown member
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		public override string GetValue(ExpressionMemberContainer container)
		{
			return container.ParentStatement.Body;
		}

		/// <summary>
		/// Attempt to parse the value as an xml
		/// </summary>
		/// <param name="container"></param>
		/// <param name="value"></param>
		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			string old = container.ParentStatement.Body;
			if (value != null)
				container.ParentStatement.Body = value;
			else
				container.ParentStatement.Body = "";
			if (old != container.ParentStatement.Body)
			{
				try
				{
					string tmp = container.ParentStatement.ToXml(new XmlDocument()).OuterXml;
				}
				catch
				{
					container.ParentStatement.Body = old;
				}
			}

		}

		public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			string result = GetValue(container);
			return string.IsNullOrWhiteSpace(result) ? " - - - - - - - - - - " : result;
		}

		public ExpressionMember_Commentary()
			: base("", "commentary")
		{

		}
	}

}