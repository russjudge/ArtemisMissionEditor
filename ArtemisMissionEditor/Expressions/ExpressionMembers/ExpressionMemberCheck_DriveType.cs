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
    public sealed class ExpressionMemberCheck_DriveType : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            string jump = container.GetAttribute("jump");
            string warp = container.GetAttribute("warp");
            if (jump == "no")
                return Choices[1]; // Warp
            else if (jump == "yes")
                return Choices[2]; // Jump
            else if (warp == "yes")
                return Choices[1]; // Warp
            else if (warp == "no")
                return Choices[2]; // Jump
            else
                return Choices[0]; // Any
        }

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            if (value == Choices[0]) // Any
            {
                container.SetAttribute("warp", null);
                container.SetAttribute("jump", null);
            }
            else if (value == Choices[1]) // Warp
            {
                container.SetAttribute("warp", "yes");
                container.SetAttribute("jump", "no");
            }
            else if (value == Choices[2]) // Jump
            {
                container.SetAttribute("warp", "no");
                container.SetAttribute("jump", "yes");
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for the drive type used with create player.
        /// </summary>
        public ExpressionMemberCheck_DriveType()
            : base("", ExpressionMemberValueDescriptions.Check_DriveType)
        {
            List<ExpressionMember> eML;
            eML = Add("Any");  //Choices[0]

            eML = Add("Warp"); //Choices[1]
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.DriveType, "warp"));
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.DriveType, "jump"));

            eML = Add("Jump"); //Choices[2]
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.DriveType, "warp"));
            eML.Add(new ExpressionMember("", ExpressionMemberValueDescriptions.DriveType, "jump"));
        }
    }
}
