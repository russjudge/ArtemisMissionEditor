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
    /// This check is for inside vs outside in [if_inside/outside_box/sphere] statements.
    /// </summary>
    public sealed class ExpressionMemberCheck_Inside_Outside : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Name == "if_inside_sphere" || container.Statement.Name == "if_inside_box")
				return Choices[0]; // inside
			else
				return Choices[1]; // outside
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0])
				container.Statement.Name = container.Statement.Name.Replace("outside", "inside");
			else
				container.Statement.Name = container.Statement.Name.Replace("inside", "outside");

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for inside vs outside in [if_inside/outside_box/sphere] statements.
        /// </summary>
        public ExpressionMemberCheck_Inside_Outside()
			: base("", ExpressionMemberValueDescriptions.Check_In_Out)
		{
            List<ExpressionMember> eML; 
            eML = Add("inside"); //Choices[0]
            eML = Add("outside"); //Choices[1]
		}


	}
}