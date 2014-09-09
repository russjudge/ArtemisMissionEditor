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
    /// This check is for name vs gm selection in multiple statements that do something to/with an object.
    /// </summary>
    public sealed class ExpressionMemberCheck_Name_GM : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("use_gm_selection") == null)
				return Choices[0]; // name
			else
				return Choices[1]; // use_gm_sel
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //name
				container.SetAttribute("use_gm_selection", null);
			else // use_gm_sel
				container.SetAttribute("use_gm_selection", "");

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for name vs gm selection in multiple statements that do something to/with an object.
        /// </summary>
        public ExpressionMemberCheck_Name_GM(ExpressionMemberValueDescription name = null)
			: base("", ExpressionMemberValueDescriptions.Check_Name_GMSelection)
		{
            name = name ?? ExpressionMemberValueDescriptions.Name;

            List<ExpressionMember> eML;

            eML = this.Add("with name"); //Choices[0]
			eML.Add(new ExpressionMember("<>", name	, "name"));

			eML = this.Add("selected by GM"); //Choices[1]
			eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.UseGM, "use_gm_selection"));
		}
	}
}