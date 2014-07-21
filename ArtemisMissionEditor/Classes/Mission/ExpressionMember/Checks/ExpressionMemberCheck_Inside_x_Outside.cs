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

	public sealed class ExpressionMemberCheck_Inside_x_Outside : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.ParentStatement.Name == "if_inside_sphere" || container.ParentStatement.Name == "if_inside_box")
				return _choices[0]; // inside
			else
				return _choices[1]; // outside
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0])
				container.ParentStatement.Name = container.ParentStatement.Name.Replace("outside","inside");
			else
				container.ParentStatement.Name = container.ParentStatement.Name.Replace("inside", "outside");

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Inside_x_Outside()
			: base("", EMVD.GetItem("check_in/out"))
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("inside"); //_choices[0]

			eML = this.Add("outside"); //_choices[1]
			
		}


	}
}