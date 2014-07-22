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

	public sealed class ExpressionMemberCheck_Point_x_TargetName : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (!String.IsNullOrEmpty(container.GetAttribute("targetName")))
				return _choices[1]; // name
			else
				return _choices[0]; // point
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{

			if (value == _choices[0]) //point
			{
				container.SetAttribute("targetName", null);
			}
			if (value == _choices[1]) // name
			{
				container.SetAttributeIfNull("targetName", " ");
			}

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Point_x_TargetName()
			: base("", "check_p/n")
		{
			List<ExpressionMember> eML;

			eML = this.Add("point"); //_choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"
			eML.Add(new ExpressionMember("<>", "x", "pointX"));
			eML.Add(new ExpressionMember("<>", "y", "pointY"));
			eML.Add(new ExpressionMember("<>", "z", "pointZ"));

			eML = this.Add("object"); //_choices[1]
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("", "name_all", "targetName"));
		}
	}
}