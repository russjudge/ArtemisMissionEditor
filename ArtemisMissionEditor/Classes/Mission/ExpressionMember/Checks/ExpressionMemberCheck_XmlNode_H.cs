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
	/// Main check that checks for xml node type
	/// </summary>
	public sealed class ExpressionMemberCheck_XmlNode_H : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.ParentStatement.Kind == MissionStatementKind.Commentary)
				return "Commentary";
			if (container.ParentStatement.Kind == MissionStatementKind.Condition)
				return "Condition";
			if (true)
				return "Action";
		}

		public ExpressionMemberCheck_XmlNode_H()
			: base()
		{

			this.Add("Commentary").Add(new ExpressionMember_Commentary());
			this.Add("Condition").Add(new ExpressionMemberCheck_XmlNameCondition());
			this.Add("Action").Add(new ExpressionMemberCheck_XmlNameAction());

		}
	}
}