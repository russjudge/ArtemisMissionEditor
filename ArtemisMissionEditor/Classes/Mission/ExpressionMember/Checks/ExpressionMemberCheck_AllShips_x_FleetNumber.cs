using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
	using EMVE = ExpressionMemberValueEditor;

	public sealed class ExpressionMemberCheck_AllShips_x_FleetNumber : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.GetAttribute("fleetnumber") == null)
				return _choices[0]; // all ships 
			else
				return _choices[1]; // fleetnumber
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //point
				container.SetAttribute("fleetnumber", null);
			else
				container.SetAttributeIfNull("fleetnumber", "0.0");

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Creates the Point/GM Pos check, specifying what should be added as a prefix to the x/y/z coords and what should substitute the "point"
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="pointCaption"></param>
		public ExpressionMemberCheck_AllShips_x_FleetNumber()
			: base("", EMVD.GetItem("check_all/fn"))
		{
			List<ExpressionMember> eML;

			eML = this.Add("when counting all enemies"); //_choices[0]
			//In case this is point, it looks like this : "... at point (x, y, z)"

			eML = this.Add("in fleet number"); //_choices[1]
			//In case this is using GM pos, it looks like: "... at GM position"
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("fleetnumber_if"), "fleetnumber"));
		}
	}
}