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
    /// This check is providing options for distance checks in [add_ai]'s CHASE entries.
    /// </summary>
    public sealed class ExpressionMemberCheck_DistanceNoNebula : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            if (container.GetAttribute("value1") == null)
                return Choices[0]; // anywhere
            if (true)
                return Choices[1]; //limited
        }

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            if (value == Choices[0])  // anywhere
            {
                container.SetAttribute("value1", null);
            }
            if (value == Choices[1])  // limited
            {
                container.SetAttributeIfNull("value1", "0");
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is providing options for distance checks in [add_ai]'s CHASE entries.
        /// </summary>
        public ExpressionMemberCheck_DistanceNoNebula()
            : base("", ExpressionMemberValueDescriptions.Check_DistanceNoNebula)
        {
            List<ExpressionMember> eML;
            
            eML = this.Add("anywhere"); //Choices[0]
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("the "));
            eML.Add(new ExpressionMember("map\" ")); 

            eML = this.Add("closer than"); //Choices[1]
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueRadiusQ, "value1"));
            eML.Add(new ExpressionMember("\" "));
        }
    }
}
