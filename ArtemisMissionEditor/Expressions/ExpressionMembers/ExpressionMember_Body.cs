using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression that is linked to Xml element's body
    /// </summary>
    public sealed class ExpressionMember_Body : ExpressionMember
	{
        public override bool RequiresLinebreak { get { return true; } }

        /// <summary>
        /// Get value of current member (Xml element's body).
        /// </summary>
        public override string GetValue(ExpressionMemberContainer container)
		{
			return container.Statement.Body.Replace("\r\n", "").Replace("^", "\r\n");
		}

        /// <summary>
        /// Get value of current member to display in the text string representing the statement.
        /// </summary>
        public override string GetValueText(ExpressionMemberContainer container)
		{
			string result = container.Statement.Body.Replace("\r\n", "");

			while (result.Length > 0 && !Char.IsLetterOrDigit(result[0]))
				result = result.Remove(0, 1);

			return result;
		}

        /// <summary>
        /// Get value of current member to display in the label (meaning, exactly what will appear in GUI labels)
        /// </summary>
        public override string GetValueDisplay(ExpressionMemberContainer container)
		{
            string result = base.GetValueDisplay(container);

			while (result.Length > 0 && !Char.IsLetterOrDigit(result[0]))
				result = result.Remove(0, 1);

			result = "\"" + result + "\"";

			return result;
		}

        /// <summary>
        /// Set value of the current member, including all checks (like, sets to null if this is a "store only if filled" value etc...)
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			value = value.Replace("\r\n", "^");

			while (value.Length > 0 && (!Char.IsLetterOrDigit(value[0]) || value[0]=='^'))
				value = value.Remove(0, 1);

			if (!value.Contains('>') && !value.Contains('<'))
				container.Statement.Body = value;
			else
				Log.Add("Invalid xml node body!");
		}

        public ExpressionMember_Body() : base("", ExpressionMemberValueDescriptions.Body) { }
	}

}