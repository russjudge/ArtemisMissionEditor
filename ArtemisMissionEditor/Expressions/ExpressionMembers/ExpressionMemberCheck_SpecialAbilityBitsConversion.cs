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
    /// This check is providing of an option to convert a deprecated [set_object_property specialAbilityBits] statement into [set_special] statements.
    /// </summary>
    public sealed class ExpressionMemberCheck_SpecialAbilityBitsConversion : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            return Choices[0];
        }

        /// <summary>
        /// Add a set_special statement for a single bit in a set_object_property specialAbilityBits statement.
        /// </summary>
        private void ConvertSpecialAbility(ref string xml, ExpressionMemberContainer container, int bit, string ability)
        {
            string attName = container.GetAttribute("name") ?? "<null>";
            string attValue = container.GetAttribute("value") ?? "";
            int intValue = Helper.StringToInt(attValue);

            xml += "<set_special name=\"" + attName + "\" ability=\"" + ability + "\" ";
            if ((intValue & bit) == 0)
                xml += "clear=\"yes\" ";
            xml += "/>\n";
        }

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value.
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            if (value == Choices[1]) //convert
            {
                string attName = container.GetAttribute("name") ?? "<null>";
                string attValue = container.GetAttribute("value") ?? "";

                string xml = "";
                ConvertSpecialAbility(ref xml, container, 1, "Stealth");
                ConvertSpecialAbility(ref xml, container, 2, "LowVis");
                ConvertSpecialAbility(ref xml, container, 4, "Cloak");
                ConvertSpecialAbility(ref xml, container, 8, "HET");
                ConvertSpecialAbility(ref xml, container, 16, "Warp");
                ConvertSpecialAbility(ref xml, container, 32, "Teleport");
                ConvertSpecialAbility(ref xml, container, 64, "Tractor");
                ConvertSpecialAbility(ref xml, container, 128, "Drones");
                ConvertSpecialAbility(ref xml, container, 256, "AntiMine");
                ConvertSpecialAbility(ref xml, container, 512, "AntiTorp");
                ConvertSpecialAbility(ref xml, container, 1024, "ShldDrain");
                ConvertSpecialAbility(ref xml, container, 2048, "ShldVamp");
                ConvertSpecialAbility(ref xml, container, 4096, "TeleBack");
                ConvertSpecialAbility(ref xml, container, 8192, "ShldReset");

                // Replace the original statement with the new XML.  Use the clipboard so that ctrl-Z works to revert.
                Mission.Current.StatementDelete();
                Clipboard.SetText(xml);
                Mission.Current.StatementPaste();

                string msg = "SpecialAbilityBits statement (name: \"" + attName + "\" value: \"" + attValue + "\") was converted to set_special statements";
                Log.Add(msg);
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is providing of an option to convert a deprecated [direct] statement into [set_special] statement.
        /// </summary>
        public ExpressionMemberCheck_SpecialAbilityBitsConversion()
            : base("", ExpressionMemberValueDescriptions.Check_ConvertSpecialAbilityBits)
        {
            List<ExpressionMember> eML;
            eML = this.Add("Do nothing"); //Choices[0]
            eML = this.Add("Convert to set_special"); //Choices[1]
        }
    }
}
