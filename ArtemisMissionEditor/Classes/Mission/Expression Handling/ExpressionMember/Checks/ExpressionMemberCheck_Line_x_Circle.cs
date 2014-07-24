using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_Line_x_Circle : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("radius") == null || container.GetAttribute("radius") == "0")
				return _choices[0]; // line
			if ((container.GetAttribute("startAngle")!=null) || (container.GetAttribute("endAngle")!=null))
				return _choices[2];
			else
				return _choices[1]; // circle
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //line
				container.SetAttribute("radius", null);
			else //circle
				container.SetAttributeIfNull("radius", "1");

			if (value == _choices[2])
			{
				container.SetAttributeIfNull("startAngle", "0");
				container.SetAttributeIfNull("endAngle", "0");
			}
			if (value == _choices[1])
			{
				container.SetAttribute("startAngle", null);
				container.SetAttribute("endAngle", null);
			}

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_Line_x_Circle()
			: base("", "check_line/circle")
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("line"); //_choices[0]
			//In case this is line, it looks like this : "... in a line from point (x, y, z) to point (x, y, z)"
			eML.Add(new ExpressionMember("from "	));
			eML.Add(new ExpressionMemberCheck_Point_x_GMPos("start","point",true));
			eML.Add(new ExpressionMember("to "		));
			eML.Add(new ExpressionMember("point "	));
			eML.Add(new ExpressionMember("<>",		"xu"			, "endX"));
			eML.Add(new ExpressionMember("<>",		"yu"			, "endY"));
			eML.Add(new ExpressionMember("<>",		"zu"			, "endZ"));

			eML = this.Add("circle"); //_choices[1]
			//In case this is using GM pos, it looks like: "... in a circle centered at GM position with the radius of r"
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "		));
			eML.Add(new ExpressionMemberCheck_Point_x_GMPos("start", "point", true));
			eML.Add(new ExpressionMember("with "	));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMember("radius "	));
			eML.Add(new ExpressionMember("of "		));
			eML.Add(new ExpressionMember("<>"		, "radius"	, "radius"));

			eML = this.Add("arc"); //_choices[2]
			//In case this is using GM pos, it looks like: "... in a circle centered at GM position with the radius of r"
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("<>", "angle_unbound", "startAngle"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", "angle_unbound", "endAngle"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMemberCheck_Point_x_GMPos("start","point",true));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("radius "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", "radius", "radius"));
		}


	}
}