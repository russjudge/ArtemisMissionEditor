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

	public sealed class ExpressionMemberCheck_Point_x_GMPos : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("use_gm_position") == null)
				return _choices[0]; // point
			else
				return _choices[1]; // use_gm_pos
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //point
				container.SetAttribute("use_gm_position", null);
			else
				container.SetAttribute("use_gm_position", "");

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Creates the Point/GM Pos check, specifying what should be added as a prefix to the x/y/z coords and what should substitute the "point"
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="pointCaption"></param>
		public ExpressionMemberCheck_Point_x_GMPos(string prefix = "", string pointCaption = "point", bool unbound = false)
			: base("", EMVD.GetItem("check_point/gmpos"))
		{
			List<ExpressionMember> eML;
			string x = prefix == "" ? "x" : prefix + "X";
			string y = prefix == "" ? "y" : prefix + "Y";
			string z = prefix == "" ? "z" : prefix + "Z";
			string suffix = unbound ? "u" : "";

			eML = this.Add(pointCaption); //_choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("x" + suffix), x, true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("y" + suffix), y, true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("z" + suffix), z, true));

			eML = this.Add("GM position"); //_choices[1]
			//In case this is using GM pos, it looks like: "... at GM position"
			eML.Add(new ExpressionMember("", EMVD.GetItem("use_gm"), "use_gm_position"));
		}
	}
}