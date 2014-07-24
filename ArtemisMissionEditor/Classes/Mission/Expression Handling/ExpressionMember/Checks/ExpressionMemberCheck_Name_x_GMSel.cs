using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_Name_x_GMSel : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("use_gm_selection") == null)
				return _choices[0]; // name
			else
				return _choices[1]; // use_gm_sel
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //name
				container.SetAttribute("use_gm_selection", null);
			else // use_gm_sel
				container.SetAttribute("use_gm_selection", "");

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Name_x_GMSel(string ExpressionMemberValueDescriptionname = "name")
			: base("", "check_name/gmsel")
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("with name"); //_choices[0]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptionname	, "name"));

			eML = this.Add("selected by GM"); //_choices[1]
			eML.Add(new ExpressionMember("", "use_gm", "use_gm_selection"));
		}


	}
}