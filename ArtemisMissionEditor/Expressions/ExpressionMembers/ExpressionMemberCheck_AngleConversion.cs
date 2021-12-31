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
    /// This check is providing of an option to convert a deprecated [create genericMesh angle] attribute into a [set_object_property] statement.
    /// </summary>
    public sealed class ExpressionMemberCheck_AngleConversion : ExpressionMemberCheck
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
        /// Add a set_object_property statement for the angle in a create genericMesh statement.
        /// </summary>
        private void ConvertAngle(ref string xml, ExpressionMemberContainer container, string attValue)
        {
            string attName = container.GetAttribute("name");

            xml += "<set_object_property name=\"" + attName + "\" property=\"angle\" value=\"(" + attValue + " + 180) * 3.14159/180\" />\n";
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
                string attValue = container.GetAttribute("angle") ?? "";

                // Replace the original statement with the new XML.  Use the clipboard so that ctrl-Z works to revert.

                Mission.Current.StatementDelete();

                // Remove angle attribute
                container.SetAttribute("angle", null);
                string xml = container.Statement.ToXml(new XmlDocument()).OuterXml;
                ConvertAngle(ref xml, container, attValue);
                Clipboard.SetText(xml);

                Mission.Current.StatementPaste();

                string msg = "Create genericMesh angle property (name: \"" + attName + "\" angle: \"" + attValue + "\") was converted to a set_object_property statement";
                Log.Add(msg);
            }

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is providing of an option to convert a deprecated [direct] statement into [set_special] statement.
        /// </summary>
        public ExpressionMemberCheck_AngleConversion()
            : base("", ExpressionMemberValueDescriptions.Check_ConvertAngle)
        {
            List<ExpressionMember> eML;
            eML = this.Add("Do nothing"); //Choices[0]
            eML = this.Add("Convert to set_object_property"); //Choices[1]
        }
    }
}
