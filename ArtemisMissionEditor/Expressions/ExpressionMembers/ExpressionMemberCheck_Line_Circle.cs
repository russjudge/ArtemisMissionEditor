using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is for line vs circle in [create] statement for nameless objects.
    /// </summary>
    public sealed class ExpressionMemberCheck_Line_Circle : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("radius") == null || container.GetAttribute("radius") == "0")
				return Choices[0]; // line
			if ((container.GetAttribute("startAngle")!=null) || (container.GetAttribute("endAngle")!=null))
				return Choices[2];
			else
				return Choices[1]; // circle
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //line
				container.SetAttribute("radius", null);
			else //circle
				container.SetAttributeIfNull("radius", "1");

			if (value == Choices[2])
			{
				container.SetAttributeIfNull("startAngle", "0");
				container.SetAttributeIfNull("endAngle", "0");
			}
			if (value == Choices[1])
			{
				container.SetAttribute("startAngle", null);
				container.SetAttribute("endAngle", null);
			}

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for line vs circle in [create] statement for nameless objects.
        /// </summary>
        public ExpressionMemberCheck_Line_Circle()
			: base("", ExpressionMemberValueDescriptions.Check_Line_Circle)
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("line"); //Choices[0]
			//In case this is line, it looks like this : "... in a line from point (x, y, z) to point (x, y, z)"
			eML.Add(new ExpressionMember("from "	));
			eML.Add(new ExpressionMemberCheck_Point_GM("start","point",true));
			eML.Add(new ExpressionMember("to "		));
			eML.Add(new ExpressionMember("point "	));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.XUnbound			, "endX"));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.YUnbound			, "endY"));
			eML.Add(new ExpressionMember("<>",		ExpressionMemberValueDescriptions.ZUnbound			, "endZ"));

			eML = this.Add("circle"); //Choices[1]
			//In case this is using GM pos, it looks like: "... in a circle centered at GM position with the radius of r"
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "		));
			eML.Add(new ExpressionMemberCheck_Point_GM("start", "point", true));
			eML.Add(new ExpressionMember("with "	));
			eML.Add(new ExpressionMember("the "		));
			eML.Add(new ExpressionMember("radius "	));
			eML.Add(new ExpressionMember("of "		));
			eML.Add(new ExpressionMember("<>"		, ExpressionMemberValueDescriptions.Radius	, "radius"));

			eML = this.Add("arc"); //Choices[2]
			//In case this is using GM pos, it looks like: "... in a circle centered at GM position with the radius of r"
			eML.Add(new ExpressionMember("from "    ));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.AngleUnbound, "startAngle"));
			eML.Add(new ExpressionMember("degrees " ));
			eML.Add(new ExpressionMember("to "      ));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.AngleUnbound, "endAngle"));
			eML.Add(new ExpressionMember("degrees " ));
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "      ));
			eML.Add(new ExpressionMemberCheck_Point_GM("start", "point", true));
			eML.Add(new ExpressionMember("with "    ));
			eML.Add(new ExpressionMember("the "     ));
			eML.Add(new ExpressionMember("radius "  ));
			eML.Add(new ExpressionMember("of "      ));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "radius"));
		}
	}
}