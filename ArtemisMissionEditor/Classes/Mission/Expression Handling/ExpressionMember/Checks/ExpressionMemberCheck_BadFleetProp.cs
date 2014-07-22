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

	public sealed class ExpressionMemberCheck_BadFleetProp : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (ExpressionMemberValueDescription.GetItem("property_f").Editor.ValueToDisplayList.ContainsKey(container.GetAttribute("property", ExpressionMemberValueDescription.GetItem("property_f").DefaultIfNull)))
				return _choices[0]; // ok
			else
				return _choices[1]; // not ok
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (Mission.Current.Loading && value == "!ok")
				Log.Add("Warning! Unknown  fleet property " + container.GetAttribute("property_f") + " detected in event: " + container.ParentStatement.Parent.Name + "!");
			
			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Creates the Point/GM Pos check, specifying what should be added as a prefix to the x/y/z coords and what should substitute the "point"
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="pointCaption"></param>
		public ExpressionMemberCheck_BadFleetProp()
			: base("", "<label>")
		{
			List<ExpressionMember> eML;

			eML = this.Add("ok"); //_choices[0]
			eML.Add(new ExpressionMember(""));

			eML = this.Add("!ok"); //_choices[1]
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));
		}
	}
}