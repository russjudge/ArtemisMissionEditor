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
    /// This check is for "type" in [set_property] statement, it is hidden since "add to property" statement has property name as a member.
    /// </summary>
    public sealed class ExpressionMemberCheck_PropertySet : ExpressionMemberCheck
    {
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
        {
            string type = container.GetAttribute("property", ExpressionMemberValueDescriptions.WritableProperty.DefaultIfNull);

            ExpressionMemberObjectProperty property = ExpressionMemberObjectProperty.Find(type);
            if (property != null)
            {
                return property.Name;
            }

            return "<UNKNOWN_PROPERTY>";
        }

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
        {
            string type = container.GetAttribute("property", ExpressionMemberValueDescriptions.WritableProperty.DefaultIfNull);
            ExpressionMemberObjectProperty property = ExpressionMemberObjectProperty.Find(type);

            if (property != null)
            {
                string newType = property.ObsoletedByName;
                if (newType != null)
                {
                    // Convert old name to new name.
                    value = newType;
                    container.SetAttribute("property", newType);
                }
                else if (value != type)
                {
                    // Convert property name to correct case.
                    value = property.Name;
                    container.SetAttribute("property", property.Name);
                }

                if (Mission.Current.Loading && property.IsReadOnly)
                {
                    Log.Add("Warning! Attempt to set read-only property " + container.GetAttribute("property") + " detected in event: " + container.Statement.Parent.Name + "!");
                }
            }

            if (Mission.Current.Loading && value == "<UNKNOWN_PROPERTY>")
                Log.Add("Warning! Unknown property " + container.GetAttribute("property") + " detected in event: "+container.Statement.Parent.Name+"!");

            base.SetValueInternal(container, value);
        }

        /// <summary>
        /// Adds "(type) "
        /// </summary>
        private void ____Add_Property(List<ExpressionMember> eML)
        {
            eML.Add(new ExpressionMember("<property>", ExpressionMemberValueDescriptions.WritableProperty, "property"));
        }

        /// <summary>
        /// Adds "
        /// </summary>
        /// 
        /// <summary>
        /// Adds "for object [selected by gm or named]"
        /// </summary>
        private void ____Add_Name(List<ExpressionMember> eML, ExpressionMemberValueDescription type)
        {
            eML.Add(new ExpressionMember("for "));
            eML.Add(new ExpressionMember("object "));

            if ((type == ExpressionMemberValueDescriptions.NameAll) ||
                (type == ExpressionMemberValueDescriptions.NamePlayer) ||
                (type == ExpressionMemberValueDescriptions.NameShip))
                eML.Add(new ExpressionMemberCheck_Name_GM_Slot(type));
            else
                eML.Add(new ExpressionMemberCheck_Name_GM(type, true));
        }

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for "type" in [set_property] statement, it is hidden since "add to property" statement has property name as a member.
        /// </summary>
        public ExpressionMemberCheck_PropertySet()
            : base()
        {
            List<ExpressionMember> eML;

            foreach (var property in ExpressionMemberObjectProperty.ObjectProperties)
            {
                eML = this.Add(property.Name);

                ____Add_Property(eML);
                eML.Add(new ExpressionMember("to "));
                eML.Add(new ExpressionMember("<>", property.ValueDescription, "value"));
                if (property.Units != null)
                    eML.Add(new ExpressionMember(property.Units + " "));
                if (property.ObjectDescription != null)
                    ____Add_Name(eML, property.ObjectDescription);
                if (property.Name == "eliteAIType")
                {
                    eML.Add(new ExpressionMember("(THIS STATEMENT IS OBSOLETE, "));
                    eML.Add(new ExpressionMember("USE ADD_AI WITH TARGET_THROTTLE "));
                    eML.Add(new ExpressionMember("OR CHASE_PLAYER INSTEAD) "));
                }
                else if (property.Name == "willAcceptCommsOrders")
                {
                    eML.Add(new ExpressionMember("(THIS STATEMENT IS OBSOLETE, "));
                    eML.Add(new ExpressionMember("USE ADD_AI WITH FOLLOW_COMMS_ORDERS "));
                    eML.Add(new ExpressionMember("INSTEAD OF SETTING THIS TO 1) "));
                }
                else if (property.Name == "specialAbilityBits")
                {
                    eML.Add(new ExpressionMember("(THIS STATEMENT IS OBSOLETE) "));

                    // We require the user to click to convert since we can't cover cases where
                    // the value is an expression with a variable.
                    eML.Add(new ExpressionMemberCheck_SpecialAbilityBitsConversion());
                }
                else if (property.IsReadOnly)
                    eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS READ ONLY!)"));
            }

            #region <UNKNOWN_PROPERTY>  

            eML = this.Add("<UNKNOWN_PROPERTY>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "value"));
            ____Add_Name(eML, ExpressionMemberValueDescriptions.NameAll);
            eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));

            #endregion
        }
    }
}
