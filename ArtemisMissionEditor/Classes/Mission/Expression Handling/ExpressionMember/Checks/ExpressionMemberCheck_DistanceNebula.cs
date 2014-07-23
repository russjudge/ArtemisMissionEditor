using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	public sealed class ExpressionMemberCheck_DistanceNebula : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("value1") == null && container.GetAttribute("value2") == null)
				return _choices[0]; // anywhere/anywhere
			if (container.GetAttribute("value1") == null  && container.GetAttribute("value2") != null)
				return _choices[1]; // anywhere/limited
			if (true)
				return _choices[2]; //limited/limited
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0])  // anywhere/anywhere
			{
				container.SetAttribute("value1", null);
				container.SetAttribute("value2", null);
			}
			if (value == _choices[1])  // anywhere/limited
			{
				container.SetAttribute("value1", null);
				container.SetAttributeIfNull("value2", "0");
			}
			if (value == _choices[2])  // limited/limited
			{
				container.SetAttributeIfNull("value1", "0");
				container.SetAttributeIfNull("value2", "0");
			}

			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_DistanceNebula()
			: base("", "check_dneb")
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("anywhere"); //_choices[0]
			eML.Add(new ExpressionMember("on "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("map\" ")); 
			

			eML = this.Add("anywhere2"); //_choices[1]
			eML.Add(new ExpressionMember("outside "));
			eML.Add(new ExpressionMember("a "));
			eML.Add(new ExpressionMember("nebula "));
			eML.Add(new ExpressionMember("/ "));
			eML.Add(new ExpressionMember("closer "));
			eML.Add(new ExpressionMember("than "));
			eML.Add(new ExpressionMember("<>", "value_r", "value2"));
			eML.Add(new ExpressionMember("inside"));
			eML.Add(new ExpressionMember("a "));
			eML.Add(new ExpressionMember("nebula\" "));
						
			eML = this.Add("closer than"); //_choices[2]
			eML.Add(new ExpressionMember("<>", "value_r", "value1"));
			eML.Add(new ExpressionMember("outside "));
			eML.Add(new ExpressionMember("a "));
			eML.Add(new ExpressionMember("nebula "));
			eML.Add(new ExpressionMember("/ "));
			eML.Add(new ExpressionMember("<>", "value_r", "value2"));
			eML.Add(new ExpressionMember("inside "));
			eML.Add(new ExpressionMember("a "));
			eML.Add(new ExpressionMember("nebula\" "));
		}


	}
}