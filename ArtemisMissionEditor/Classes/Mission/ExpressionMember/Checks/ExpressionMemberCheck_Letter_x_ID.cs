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

	public sealed class ExpressionMemberCheck_Letter_x_ID : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (string.IsNullOrWhiteSpace(container.GetAttribute("value")))
				return _choices[0]; // text
			else
				return _choices[1]; // id
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //id
				container.SetAttribute("value", null);
			else						//text
				container.SetAttributeIfNull("value", "0");

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Creates the Point/GM Pos check, specifying what should be added as a prefix to the x/y/z coords and what should substitute the "point"
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="pointCaption"></param>
		public ExpressionMemberCheck_Letter_x_ID()
			: base("", EMVD.GetItem("check_letter/id"))
		{
			List<ExpressionMember> eML;

			eML = this.Add("letter"); //_choices[0]
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("letter"), "keyText"));
			eML.Add(new ExpressionMember("key"));

			eML = this.Add("key with scancode"); //_choices[1]
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("keyid"), "value"));
		}
	}
}