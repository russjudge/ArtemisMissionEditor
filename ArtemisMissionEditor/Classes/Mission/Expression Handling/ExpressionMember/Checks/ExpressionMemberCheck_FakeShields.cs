using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_FakeShields : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			string f = container.GetAttribute("fakeShieldsFront");
			string r = container.GetAttribute("fakeShieldsRear");
			int fi, ri;
			Helper.IntTryParse(f, out fi);
			Helper.IntTryParse(r, out ri);

			if ((f == null || fi == -1) && (r == null || ri == -1))
				return _choices[0]; // no fake shields

			if (r == null || ri == -1)
				return _choices[1]; // fake front shields

			if (f == null || fi == -1)
				return _choices[2]; // fake rear shields

			if (true)
				return _choices[3]; // fake front/rear shields
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //no fake shields
			{
				container.SetAttribute("fakeShieldsFront", null);
				container.SetAttribute("fakeShieldsRear", null);
			}
			if (value == _choices[1]) //fake front shields
			{
				container.SetAttributeIfNull("fakeShieldsFront", "0");
				container.SetAttribute("fakeShieldsRear", null);
			}
			if (value == _choices[2]) //fake rear shields
			{
				container.SetAttribute("fakeShieldsFront", null);
				container.SetAttributeIfNull("fakeShieldsRear", "0");
			}
			if (value == _choices[3]) //fake front shields
			{
				container.SetAttributeIfNull("fakeShieldsFront", "0");
				container.SetAttributeIfNull("fakeShieldsRear", "0");
			}
			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_FakeShields()
			: base("", "check_fakeShields")
		{
			List<ExpressionMember> eML;

			eML = this.Add("no fake shields"); //_choices[0]

			eML = this.Add("fake front shields"); //_choices[1]
			eML.Add(new ExpressionMember("<>", "fakeShieldsFR", "fakeShieldsFront"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", "hasFakeShldFreq", "hasFakeShldFreq"));

			eML = this.Add("fake rear shields"); //_choices[2]
			eML.Add(new ExpressionMember("<>", "fakeShieldsFR", "fakeShieldsRear"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", "hasFakeShldFreq", "hasFakeShldFreq"));

			eML = this.Add("fake front/rear shields"); //_choices[3]
			eML.Add(new ExpressionMember("<>", "fakeShieldsF", "fakeShieldsFront"));
			eML.Add(new ExpressionMember("<>", "fakeShieldsR", "fakeShieldsRear"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("<>", "hasFakeShldFreq", "hasFakeShldFreq"));
		}
	}
}