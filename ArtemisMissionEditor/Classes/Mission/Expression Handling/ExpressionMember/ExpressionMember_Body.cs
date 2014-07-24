using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	/// <summary>
	/// Allows editing of XML Body
	/// </summary>
	public sealed class ExpressionMember_Body : ExpressionMember
	{
		public override bool RequiresLinebreak { get { return true; } }

		/// <summary>
		/// Gets the xml representing this unknown member
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		public override string GetValue(ExpressionMemberContainer container)
		{
			return container.ParentStatement.Body.Replace("\r\n", "").Replace("^", "\r\n");
		}

		public override string GetValueText(ExpressionMemberContainer container)
		{
			string result = container.ParentStatement.Body.Replace("\r\n", "");

			while (result.Length > 0 && !Char.IsLetterOrDigit(result[0]))
				result = result.Remove(0, 1);

			return result;
		}

		public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			string result = base.GetValueDisplay(container);

			while (result.Length > 0 && !Char.IsLetterOrDigit(result[0]))
				result = result.Remove(0, 1);

			result = "\"" + result + "\"";

			return result;
		}

		/// <summary>
		/// Attempt to parse the value as an xml
		/// </summary>
		/// <param name="container"></param>
		/// <param name="value"></param>
		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			value = value.Replace("\r\n", "^");

			while (value.Length > 0 && (!Char.IsLetterOrDigit(value[0]) || value[0]=='^'))
				value = value.Remove(0, 1);

			if (!value.Contains('>') && !value.Contains('<'))
				container.ParentStatement.Body = value;
			else
				Log.Add("Invalid xml node body!");
		}

		public ExpressionMember_Body()
			: base("", "<body>")
		{

		}
	}

}