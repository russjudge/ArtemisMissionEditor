using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_Box_x_Sphere : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.ParentStatement.Name == "if_outside_box" || container.ParentStatement.Name == "if_inside_box")
				return _choices[0]; // box
			else
				return _choices[1]; // sphere
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0])
				container.ParentStatement.Name = container.ParentStatement.Name.Replace("sphere","box");
			else
				container.ParentStatement.Name = container.ParentStatement.Name.Replace("box", "sphere");

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Box_x_Sphere()
			: base("", "check_box/sph")
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("box"); //_choices[0]
			eML.Add(new ExpressionMember("defined "));
			eML.Add(new ExpressionMember("by "));
			eML.Add(new ExpressionMember("corner "));
			eML.Add(new ExpressionMember("points "));
			eML.Add(new ExpressionMember("<>", "x", "leastX", true));
			eML.Add(new ExpressionMember("<>", "z", "leastZ", true));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", "x", "mostX", true));
			eML.Add(new ExpressionMember("<>", "z", "mostZ", true));

			eML = this.Add("sphere"); //_choices[1]
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("<>", "x", "centerX", true));
			eML.Add(new ExpressionMember("<>", "y", "centerY", true));
			eML.Add(new ExpressionMember("<>", "z", "centerZ", true));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("radius "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", "radius", "radius"));
		}


	}
}