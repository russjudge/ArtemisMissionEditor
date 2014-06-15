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

	public sealed class ExpressionMemberCheck_Template : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			return "";
		}

		public override string GetValue(ExpressionMemberContainer container)
		{
			return base.GetValue(container);
		}

		public override string GetValueDisplay(ExpressionMemberContainer container)
		{
			return base.GetValueDisplay(container);
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Template(string text, ExpressionMemberValueDescription valueDescription, string nameXml = "", bool mandatory = false)
			: base(text, valueDescription, nameXml, mandatory)
		{

		}

		public ExpressionMemberCheck_Template()
			: base()
		{

		}
	}
}