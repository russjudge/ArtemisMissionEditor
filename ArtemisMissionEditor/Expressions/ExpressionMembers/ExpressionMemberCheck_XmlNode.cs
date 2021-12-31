using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is the main check that checks for the xml node type.
    /// </summary>
    public sealed class ExpressionMemberCheck_XmlNode : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Kind == MissionStatementKind.Commentary)
				return "Commentary";
			if (container.Statement.Kind == MissionStatementKind.Condition)
				return "Condition";
			if (true)
				return "Action";
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is the main check that checks for the xml node type.
        /// </summary>
        public ExpressionMemberCheck_XmlNode()
			: base()
		{
			this.Add("Commentary").Add(new ExpressionMember_Commentary());
			this.Add("Condition").Add(new ExpressionMemberCheck_XmlNameCondition());
			this.Add("Action").Add(new ExpressionMemberCheck_XmlNameAction());
		}
	}
}