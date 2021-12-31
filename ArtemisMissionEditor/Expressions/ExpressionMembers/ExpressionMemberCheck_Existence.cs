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
    /// This check is for [if_exists] vs [if_not_exists].
    /// </summary>
    public sealed class ExpressionMemberCheck_Existence : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Name == "if_exists")
				return Choices[0]; // if object exists
			else
				return Choices[1]; // if object doesn't exist
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) 
				container.Statement.Name = "if_exists";
			else 
				container.Statement.Name = "if_not_exists";

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for [if_exists] vs [if_not_exists].
        /// </summary>
        public ExpressionMemberCheck_Existence()
			: base("", ExpressionMemberValueDescriptions.Check_Existence)
		{
			List<ExpressionMember> eML;
			eML = Add("exists"); //Choices[0]
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name", useGMSelectionAttributeName:"<none>"));

			eML = Add("does not exist"); //Choices[1]
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name", useGMSelectionAttributeName:"<none>"));
		}
	}
}
