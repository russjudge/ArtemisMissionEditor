using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	
	
	
	

	public sealed class ExpressionMemberCheck_DirectConversion : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			return _choices[0]; // deprecated
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[1]) //convert
			{
				string msg = "Direct statement (name: \"" + (container.GetAttribute("name") ?? "<null>") + "\" ";

				value = "add_ai";
				container.ParentStatement.Name = value;
				if (!String.IsNullOrEmpty(container.GetAttribute("targetName")))
				{
					//ATTACK
					container.SetAttribute("type", "TARGET_THROTTLE");
					container.SetAttribute("value1", container.GetAttribute("scriptThrottle"));
					msg += "targetName: \"" + container.GetAttribute("targetName") + "\" scriptThrottle \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"TARGET_THROTTLE\" statement";
				}
				else
				{
					//POINT_THROTTLE
					container.SetAttribute("type", "POINT_THROTTLE");
					container.SetAttribute("value1", container.GetAttribute("pointX"));
					container.SetAttribute("value2", container.GetAttribute("pointY"));
					container.SetAttribute("value3", container.GetAttribute("pointZ"));
					container.SetAttribute("value4", container.GetAttribute("scriptThrottle"));
					msg += "target point: \"" + container.GetAttribute("pointX") + ", " + container.GetAttribute("pointY") + ", " + container.GetAttribute("pointZ") + "\" scriptThrottle: \"" + container.GetAttribute("scriptThrottle") + ") was converted to add_ai \"POINT_THROTTLE\" statement";
				}

				Log.Add(msg);
			}

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Creates the Point/GM Pos check, specifying what should be added as a prefix to the x/y/z coords and what should substitute the "point"
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="pointCaption"></param>
		public ExpressionMemberCheck_DirectConversion()
			: base("", "check_convertd")
		{
			List<ExpressionMember> eML;

			eML = this.Add("Do nothing"); //_choices[0]

			eML = this.Add("Convert to add_ai"); //_choices[1]
		}
	}
}