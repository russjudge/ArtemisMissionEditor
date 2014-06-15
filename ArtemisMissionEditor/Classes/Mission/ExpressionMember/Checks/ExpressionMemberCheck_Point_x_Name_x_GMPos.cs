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

	public sealed class ExpressionMemberCheck_Point_x_Name_x_GMPos : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (!String.IsNullOrEmpty(container.GetAttribute("name")))
				return _choices[2]; // name
			if (container.GetAttribute("use_gm_position") != null)
				return _choices[1]; // use_gm_pos
			else
				return _choices[0]; // point
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{

			if (value == _choices[0]) //point
			{
				container.SetAttribute("use_gm_position", null);
				container.SetAttribute("name", null);
			}
			if (value == _choices[1])  //gm_pos
			{
				container.SetAttribute("use_gm_position", "");
				container.SetAttribute("name", null);
			}
			if (value == _choices[2]) // name
			{
				container.SetAttributeIfNull("name", " ");
			}
				
			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Point_x_Name_x_GMPos(bool unbound = false)
			: base("", EMVD.GetItem("check_p/n/gmpos"))
		{
			List<ExpressionMember> eML;
			
			string suffix = unbound ? "u" : "";
			
			eML = this.Add("point"); //_choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("x" + suffix), "centerX"));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("y" + suffix), "centerY"));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("z" + suffix), "centerZ"));
			
			eML = this.Add("GM position"); //_choices[1]
			//In case this is using GM pos, it looks like: "... at GM position"
			eML.Add(new ExpressionMember("", EMVD.GetItem("use_gm"), "use_gm_position"));

			eML = this.Add("named object"); //_choices[2]
			eML.Add(new ExpressionMember("", EMVD.GetItem("name_all"), "name"));
		}
	}
}