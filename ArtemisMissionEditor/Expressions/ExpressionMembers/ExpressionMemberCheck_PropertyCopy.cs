﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is for "type" in [copy_property] statement, it is hidden since "add to property" statement has property name as a member.
    /// </summary>
    public sealed class ExpressionMemberCheck_PropertyCopy : ExpressionMemberCheck
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
    				Log.Add("Warning! Attempt to copy read-only property " + container.GetAttribute("property") + " detected in event: " + container.Statement.Parent.Name + "!");
                }
            }

			if (Mission.Current.Loading && value == "<UNKNOWN_PROPERTY>")
				Log.Add("Warning! Unknown property " + container.GetAttribute("property") + " detected in event: " + container.Statement.Parent.Name + "!");

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Adds "
		/// </summary>
		private void ____Add_Name(List<ExpressionMember> eML, ExpressionMemberValueDescription type)
		{
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name1", "<none>", "player_slot1"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name2", "<none>", "player_slot2"));
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for "type" in [copy_property] statement, it is hidden since "add to property" statement has property name as a member.
        /// </summary>
        public ExpressionMemberCheck_PropertyCopy()
			: base()
		{
			List<ExpressionMember> eML;

            foreach (var property in ExpressionMemberObjectProperty.ObjectProperties)
            {
                eML = this.Add(property.Name);

    			eML.Add(new ExpressionMember("<property>", ExpressionMemberValueDescriptions.WritableObjectProperty, "property"));
                if (property.ObjectDescription != null)
                {
                    ____Add_Name(eML, property.ObjectDescription);
                }
                else
                {
                    eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS GLOBAL!)"));
                }
                if (property.IsReadOnly)
                {
        			eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS READ ONLY!)"));
                }
            }

			#region <UNKNOWN_PROPERTY>

			eML = this.Add("<UNKNOWN_PROPERTY>");
  			eML.Add(new ExpressionMember("<property>", ExpressionMemberValueDescriptions.WritableProperty, "property"));
			____Add_Name(eML, ExpressionMemberValueDescriptions.NameAll);
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));

			#endregion
		}
	}
}
