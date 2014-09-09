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
    /// This check is for "fleetnumber" versus "all ships" from [if_fleet_count]
    /// </summary>
    public sealed class ExpressionMemberCheck_AllShips_FleetNumber : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("fleetnumber") == null)
				return Choices[0]; // all ships 
			else
				return Choices[1]; // fleetnumber
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //point
				container.SetAttribute("fleetnumber", null);
			else
				container.SetAttributeIfNull("fleetnumber", "0.0");

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition. 
        /// This check is for "fleetnumber" versus "all ships" from [if_fleet_count]
        /// </summary>
        public ExpressionMemberCheck_AllShips_FleetNumber()
			: base("", ExpressionMemberValueDescriptions.Check_AllShips_FleetNumber)
		{
			List<ExpressionMember> eML;

			eML = this.Add("when counting all enemies"); //Choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"

			eML = this.Add("in fleet number"); //Choices[1]
			//In case this is using GM pos, it looks like: "... at GM position"
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FleetNumberIf, "fleetnumber"));
		}
	}
}