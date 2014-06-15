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

	public sealed class ExpressionMemberCheck_Box_x_Sphere : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Name == "if_outside_box" || container.Statement.Name == "if_inside_box")
				return _choices[0]; // box
			else
				return _choices[1]; // sphere
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0])
				container.Statement.Name = container.Statement.Name.Replace("sphere","box");
			else
				container.Statement.Name = container.Statement.Name.Replace("box", "sphere");

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Box_x_Sphere()
			: base("", EMVD.GetItem("check_box/sph"))
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("box"); //_choices[0]
			eML.Add(new ExpressionMember("defined "));
			eML.Add(new ExpressionMember("by "));
			eML.Add(new ExpressionMember("corner "));
			eML.Add(new ExpressionMember("points "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("x"), "leastX", true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("z"), "leastZ", true));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("x"), "mostX", true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("z"), "mostZ", true));

			eML = this.Add("sphere"); //_choices[1]
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("x"), "centerX", true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("y"), "centerY", true));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("z"), "centerZ", true));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("radius "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("radius"), "radius"));
		}


	}
}