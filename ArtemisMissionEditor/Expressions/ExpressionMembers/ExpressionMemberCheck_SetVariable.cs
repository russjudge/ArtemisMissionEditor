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
    /// This check is for [set_variable] statement (exact value vs random int vs random float).
    /// </summary>
    public sealed class ExpressionMemberCheck_SetVariable : ExpressionMemberCheck
	{
        enum Choice { Exact, RandomInt, RandomFloat };

        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("value") == null && (container.GetAttribute("randomIntLow") != null || container.GetAttribute("randomIntHigh") != null))
				return Choices[(int)Choice.RandomInt];
			if (container.GetAttribute("value") == null && (container.GetAttribute("randomFloatLow") != null || container.GetAttribute("randomFloatHigh") != null))
				return Choices[(int)Choice.RandomFloat];
			else
				return Choices[(int)Choice.Exact];
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[(int)Choice.RandomInt])
			{
				container.SetAttributeIfNull("randomIntLow", "0");
				container.SetAttributeIfNull("randomIntHigh", "0");
				container.SetAttribute("randomFloatLow", null);
				container.SetAttribute("randomFloatHigh", null);
				container.SetAttribute("value", null);
			}
			if (value == Choices[(int)Choice.RandomFloat])
			{
				container.SetAttribute("randomIntLow", null);
				container.SetAttribute("randomIntHigh", null);
				container.SetAttributeIfNull("randomFloatLow", "0.0");
				container.SetAttributeIfNull("randomFloatHigh", "0.0");
				container.SetAttribute("value", null);
			}
			if (value == Choices[(int)Choice.Exact])
			{
				container.SetAttribute("randomIntLow", null);
				container.SetAttribute("randomIntHigh", null);
				container.SetAttribute("randomFloatLow", null);
				container.SetAttribute("randomFloatHigh", null);
				container.SetAttributeIfNull("value", "0.0");
			}
				
			base.SetValueInternal(container, value);

			if (container.GetAttribute("integer") != null && container.GetAttribute("integer") != "yes")
			{
				container.SetAttribute("integer", null);
			}
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for [set_variable] statement (exact value vs random int vs random float).
        /// </summary>
        public ExpressionMemberCheck_SetVariable()
			: base("", ExpressionMemberValueDescriptions.Check_SetVariable)
		{
			List<ExpressionMember> eML;

			eML = this.Add("<to>"); //Choices[0]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "value"));
			eML.Add(new ExpressionMember("as ")); 
			eML.Add(new ExpressionMember("Float", ExpressionMemberValueDescriptions.VariableType, "integer"));
			
			eML = this.Add("to a random integer"); //Choices[1]
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("the ")); 
			eML.Add(new ExpressionMember("range ")); 
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_NegInf_PosInf, "randomIntLow"));
			eML.Add(new ExpressionMember("... "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_NegInf_PosInf, "randomIntHigh")); 
			eML.Add(new ExpressionMember("as ")); 
			eML.Add(new ExpressionMember("Integer", ExpressionMemberValueDescriptions.VariableType, "integer"));

			eML = this.Add("to a random float"); //Choices[2]
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("the ")); 
			eML.Add(new ExpressionMember("range "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "randomFloatLow"));
			eML.Add(new ExpressionMember("... "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "randomFloatHigh")); 
		}
	}
}
