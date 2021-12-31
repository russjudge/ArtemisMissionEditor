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
    /// This check is for letter vs key ID in [if_client_key] and [if_gm_key] statements.
    /// </summary>
    public sealed class ExpressionMemberCheck_Letter_ID : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (string.IsNullOrWhiteSpace(container.GetAttribute("value")))
				return Choices[0]; // text
			else
				return Choices[1]; // id
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //id
				container.SetAttribute("value", null);
			else						//text
				container.SetAttributeIfNull("value", "0");

			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for letter vs key ID in [if_client_key] and [if_gm_key] statements.
        /// </summary>
        public ExpressionMemberCheck_Letter_ID()
			: base("", ExpressionMemberValueDescriptions.Check_Letter_KeyID)
		{
			List<ExpressionMember> eML;

			eML = this.Add("letter"); //Choices[0]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Letter, "keyText"));
			eML.Add(new ExpressionMember("key"));

			eML = this.Add("key with scancode"); //Choices[1]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.KeyID, "value"));
		}
	}
}