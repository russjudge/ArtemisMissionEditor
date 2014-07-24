using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_SetVariable : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("value") == null && (container.GetAttribute("randomIntLow") != null || container.GetAttribute("randomIntHigh") != null))
				return _choices[1]; // random int
			if (container.GetAttribute("value") == null && (container.GetAttribute("randomFloatLow") != null || container.GetAttribute("randomFloatHigh") != null))
				return _choices[2]; // random float
			else
				return _choices[0]; // exact value
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{

			if (value == _choices[1]) //random int
			{
				container.SetAttributeIfNull("randomIntLow", "0");
				container.SetAttributeIfNull("randomIntHigh", "0");
				container.SetAttribute("randomFloatLow", null);
				container.SetAttribute("randomFloatHigh", null);
				container.SetAttribute("value", null);
			}
			if (value == _choices[2])  //random float
			{
				container.SetAttribute("randomIntLow", null);
				container.SetAttribute("randomIntHigh", null);
				container.SetAttributeIfNull("randomFloatLow", "0.0");
				container.SetAttributeIfNull("randomFloatHigh", "0.0");
				container.SetAttribute("value", null);
			}
			if (value == _choices[0]) // exact value
			{
				container.SetAttributeIfNull("value", "0.0");
			}
				
			base.SetValueInternal(container, value);
		}

		public ExpressionMemberCheck_SetVariable()
			: base("", "check_setvar")
		{
			List<ExpressionMember> eML;

			eML = this.Add("<to>"); //_choices[0]
			eML.Add(new ExpressionMember("<>", "value", "value")); 
			
			eML = this.Add("to a random integer"); //_choices[1]
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("the ")); 
			eML.Add(new ExpressionMember("range ")); 
			eML.Add(new ExpressionMember("<>", "randInt", "randomIntLow"));
			eML.Add(new ExpressionMember("... "));
			eML.Add(new ExpressionMember("<>", "randInt", "randomIntHigh")); 

			eML = this.Add("to a random float"); //_choices[2]
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("the ")); 
			eML.Add(new ExpressionMember("range "));
			eML.Add(new ExpressionMember("<>", "randFloat", "randomFloatLow"));
			eML.Add(new ExpressionMember("... "));
			eML.Add(new ExpressionMember("<>", "randFloat", "randomFloatHigh")); 
			
		}
	}
}