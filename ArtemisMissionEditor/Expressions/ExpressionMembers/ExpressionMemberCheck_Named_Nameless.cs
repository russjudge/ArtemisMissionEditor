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
    /// This check is for named vs nameless object selection from [destroy] and [destroy_near] statements.
    /// </summary>
    public sealed class ExpressionMemberCheck_Named_Nameless : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.Statement.Name == "destroy")
				return Choices[0]; // destroy (named object)
			if (container.GetAttribute("type") == "asteroids")
				return Choices[1]; // destroy_near asteroids
			if (container.GetAttribute("type") == "mines")
				return Choices[2]; // destroy_near mines
            if (container.GetAttribute("type") == "nebulas")
                return Choices[3]; // destroy_near nebulas
            if (container.GetAttribute("type") == "whales")
                return Choices[4]; // destroy_near whales
            if (container.GetAttribute("type") == "drones")
                return Choices[5]; // destroy_near drones
            if (true)
				return Choices[6]; // destroy_near all
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //destroy named object
				container.Statement.Name = "destroy";
			else // destroy_near
				container.Statement.Name = "destroy_near";

			if (value == Choices[1]) // destroy asteroids
				container.SetAttribute("type", "asteroids");
			if (value == Choices[2]) // destroy mines
				container.SetAttribute("type", "mines");
			if (value == Choices[3]) // destroy nebulas
				container.SetAttribute("type", "nebulas");
            if (value == Choices[4]) // destroy whales
                container.SetAttribute("type", "whales");
            if (value == Choices[5]) // destroy drones
                container.SetAttribute("type", "drones");
            if (value == Choices[6]) // destroy all
                container.SetAttribute("type", "all");

			base.SetValueInternal(container, value);
		}
		
		/// <summary>
		/// Adds " near "
		/// </summary>
		private void ____Add_Nameless(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<type>", ExpressionMemberValueDescriptions.TypeH, "type"));
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "radius"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMemberCheck_Point_Name_GM());

		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for named vs nameless object selection from [destroy] and [destroy_near] statements.
        /// </summary>
        public ExpressionMemberCheck_Named_Nameless()
			: base("", ExpressionMemberValueDescriptions.Check_Named_Nameless)
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("named object"); //Choices[0]
			eML.Add(new ExpressionMemberCheck_Name_GM(ExpressionMemberValueDescriptions.NameAll));

			eML = this.Add("asteroids"); //Choices[1]
			____Add_Nameless(eML);

			eML = this.Add("mines"); //Choices[2]
			____Add_Nameless(eML); 
			
			eML = this.Add("nebulas"); //Choices[3]
			____Add_Nameless(eML);

            eML = this.Add("whales"); //Choices[4]
            ____Add_Nameless(eML);

            eML = this.Add("drones"); //Choices[5]
            ____Add_Nameless(eML);
            
            eML = this.Add("all"); //Choices[6]
            ____Add_Nameless(eML);
		}
	}
}