using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_Existance : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.ParentStatement.Name == "if_exists")
				return _choices[0]; // if object exists
			else
				return _choices[1]; // if object doesnt exist
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) 
				container.ParentStatement.Name = "if_exists";
			else 
				container.ParentStatement.Name = "if_not_exists";

			base.SetValueInternal(container, value);
		}
		
		public ExpressionMemberCheck_Existance()
			: base("", "check_existance")
		{
			List<ExpressionMember> eML;
			eML = Add("exists"); //_choices[0]
			eML = Add("does not exist"); //_choices[1]
			
		}


	}
}