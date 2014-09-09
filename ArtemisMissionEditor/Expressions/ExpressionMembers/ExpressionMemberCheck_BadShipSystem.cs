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
    /// This check appends a warning when an unknown ship system is encountered.
    /// </summary>
    public sealed class ExpressionMemberCheck_BadShipSystem : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (ExpressionMemberValueDescriptions.SystemType.Editor.XmlValueToDisplay.ContainsKey(container.GetAttribute("systemType", ExpressionMemberValueDescriptions.SystemType.DefaultIfNull)))
				return Choices[0]; // ok
			else
				return Choices[1]; // not ok
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (Mission.Current.Loading && value == "!ok")
				Log.Add("Warning! Unknown ship system " + container.GetAttribute("systemType") + " detected in event: " + container.Statement.Parent.Name + "!");
			
			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check appends a warning when an unknown ship system is encountered.
        /// </summary>
        public ExpressionMemberCheck_BadShipSystem()
			: base("", ExpressionMemberValueDescriptions.Label)
		{
			List<ExpressionMember> eML;

			eML = this.Add("ok"); //Choices[0]
			eML.Add(new ExpressionMember(""));

			eML = this.Add("!ok"); //Choices[1]
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN SHIP SYSTEM NAME)"));
		}
	}
}