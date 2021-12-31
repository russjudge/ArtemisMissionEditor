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
    /// This check is for fake shields in [create] statement with "genericMesh" type.
    /// </summary>
    public sealed class ExpressionMemberCheck_FakeShields : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			string f = container.GetAttribute("fakeShieldsFront");
			string r = container.GetAttribute("fakeShieldsRear");
			int fi, ri;
			Helper.IntTryParse(f, out fi);
			Helper.IntTryParse(r, out ri);

			if ((f == null || fi == -1) && (r == null || ri == -1))
				return Choices[0]; // no fake shields

			if (r == null || ri == -1)
				return Choices[1]; // fake front shields

			if (f == null || fi == -1)
				return Choices[2]; // fake rear shields

			if (true)
				return Choices[3]; // fake front/rear shields
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == Choices[0]) //no fake shields
			{
				container.SetAttribute("fakeShieldsFront", null);
				container.SetAttribute("fakeShieldsRear", null);
			}
			if (value == Choices[1]) //fake front shields
			{
				container.SetAttributeIfNull("fakeShieldsFront", "0");
				container.SetAttribute("fakeShieldsRear", null);
			}
			if (value == Choices[2]) //fake rear shields
			{
				container.SetAttribute("fakeShieldsFront", null);
				container.SetAttributeIfNull("fakeShieldsRear", "0");
			}
			if (value == Choices[3]) //fake front shields
			{
				container.SetAttributeIfNull("fakeShieldsFront", "0");
				container.SetAttributeIfNull("fakeShieldsRear", "0");
			}
			base.SetValueInternal(container, value);
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for fake shields in [create] statement with "genericMesh" type.
        /// </summary>
        public ExpressionMemberCheck_FakeShields()
			: base("", ExpressionMemberValueDescriptions.Check_FakeShields)
		{
			List<ExpressionMember> eML;

			eML = this.Add("no fake shields"); //Choices[0]

			eML = this.Add("fake front shields"); //Choices[1]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FakeShieldsFR, "fakeShieldsFront"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HasFakeShldFrequency, "hasFakeShldFreq"));

			eML = this.Add("fake rear shields"); //Choices[2]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FakeShieldsFR, "fakeShieldsRear"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HasFakeShldFrequency, "hasFakeShldFreq"));

			eML = this.Add("fake front/rear shields"); //Choices[3]
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FakeShieldsF, "fakeShieldsFront"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FakeShieldsR, "fakeShieldsRear"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.HasFakeShldFrequency, "hasFakeShldFreq"));
		}
	}
}