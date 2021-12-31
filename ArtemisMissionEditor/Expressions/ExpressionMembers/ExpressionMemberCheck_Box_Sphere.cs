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
    /// This check is for "box" or "sphere" for [if_inside/outside_box/sphere].
    /// </summary>
    public sealed class ExpressionMemberCheck_Box_Sphere : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Name == "if_outside_box" || container.Statement.Name == "if_inside_box")
				return Choices[0]; // box
			else
				return Choices[1]; // sphere
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0])
				container.Statement.Name = container.Statement.Name.Replace("sphere","box");
			else
				container.Statement.Name = container.Statement.Name.Replace("box", "sphere");

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for "box" or "sphere" for [if_inside/outside_box/sphere].
        /// </summary>
        public ExpressionMemberCheck_Box_Sphere()
            : base("", ExpressionMemberValueDescriptions.Check_Box_Sphere)
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("box"); //Choices[0]
			eML.Add(new ExpressionMember("defined "));
			eML.Add(new ExpressionMember("by "));
			eML.Add(new ExpressionMember("corner "));
			eML.Add(new ExpressionMember("points "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "leastX", true));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "leastZ", true));
			eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "mostX", true));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "mostZ", true));

			eML = this.Add("sphere"); //Choices[1]
			eML.Add(new ExpressionMember("centered "));
			eML.Add(new ExpressionMember("at "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.X, "centerX", true));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Y, "centerY", true));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Z, "centerZ", true));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("radius "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "radius"));
		}


	}
}