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
    /// This check is for "hullID" vs "hullKeys" and "raceKeys" in [create] statement.
    /// </summary>
    public sealed class ExpressionMemberCheck_HullID_HullRaceKeys : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
            //if ((container.GetAttribute("hullKeys") == null && container.GetAttribute("raceKeys") == null) || !string.IsNullOrEmpty(container.GetAttribute("hullID")))
            //|| !string.IsNullOrEmpty(container.GetAttribute("hullID")
            if (container.GetAttribute("hullID") == null )
                return Choices[HullRaceKey]; // hullKeys / raceKeys
			else
                return Choices[HullID]; // hullID
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
            if (value == Choices[HullID]) //hullID
			{
				container.SetAttributeIfNull("hullID", "");
				container.SetAttribute("hullKeys", null);
				container.SetAttribute("raceKeys", null);
			}
            if (value == Choices[HullRaceKey]) //hullKeys / raceKeys
			{
				container.SetAttribute("hullID", null);
				container.SetAttributeIfNull("hullKeys", "");
				container.SetAttributeIfNull("raceKeys", "");
			}

			base.SetValueInternal(container, value);
		}

        const int HullID = 1;
        const int HullRaceKey = 0;
       
        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for "hullID" vs "hullKeys" and "raceKeys" in [create] statement.
        /// </summary>
        public ExpressionMemberCheck_HullID_HullRaceKeys(bool mandatory = true)
			: base("", ExpressionMemberValueDescriptions.Check_ID_Keys)
		{
			List<ExpressionMember> eML;

			eML = this.Add("race/hull keys"); //Choices[1]
            //In case this is hull/race keys, it looks like: "... and race/hull keys "Kralien" "Battleship"
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.RaceKeys, "raceKeys", false));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HullKeys, "hullKeys", mandatory));

            eML = this.Add("hull ID"); //Choices[0]
            //In case this is hullID, it looks like this : "... and hull ID 5003"
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HullID, "hullID", mandatory));


		}
	}
}