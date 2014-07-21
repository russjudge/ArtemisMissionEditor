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

	public sealed class ExpressionMemberCheck_Named_x_Nameless : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			if (container.ParentStatement.Name == "destroy")
				return _choices[0]; // destroy (named object)
			if (container.GetAttribute("type") == "asteroids")
				return _choices[1]; // destroy_near asteroids
			if (container.GetAttribute("type") == "mines")
				return _choices[2]; // destroy_near mines
            if (container.GetAttribute("type") == "nebulas")
                return _choices[3]; // destroy_near nebulas
            if (container.GetAttribute("type") == "whales")
                return _choices[4]; // destroy_near whales
            if (container.GetAttribute("type") == "drones")
                return _choices[5]; // destroy_near drones
            if (true)
				return _choices[6]; // destroy_near all
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value == _choices[0]) //destroy named object
				container.ParentStatement.Name = "destroy";
			else // destroy_near
				container.ParentStatement.Name = "destroy_near";

			if (value == _choices[1]) // destroy asteroids
				container.SetAttribute("type", "asteroids");
			if (value == _choices[2]) // destroy mines
				container.SetAttribute("type", "mines");
			if (value == _choices[3]) // destroy nebulas
				container.SetAttribute("type", "nebulas");
            if (value == _choices[4]) // destroy whales
                container.SetAttribute("type", "whales");
            if (value == _choices[5]) // destroy drones
                container.SetAttribute("type", "drones");
            if (value == _choices[6]) // destroy all
                container.SetAttribute("type", "all");

			base.SetValueInternal(container, value);
		}
		
		/// <summary>
		/// Adds " near "
		/// </summary>
		private void ____Add_Nameless(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<type>", EMVD.GetItem("typeH"), "type"));
			eML.Add(new ExpressionMember("within "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("radius"), "radius"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMemberCheck_Point_x_Name_x_GMPos());

		}

		public ExpressionMemberCheck_Named_x_Nameless()
			: base("", EMVD.GetItem("check_named/nameless"))
		{
			List<ExpressionMember> eML;
			
			eML = this.Add("named object"); //_choices[0]
			eML.Add(new ExpressionMemberCheck_Name_x_GMSel("name_all"));

			eML = this.Add("asteroids"); //_choices[1]
			____Add_Nameless(eML);

			eML = this.Add("mines"); //_choices[2]
			____Add_Nameless(eML); 
			
			eML = this.Add("nebulas"); //_choices[3]
			____Add_Nameless(eML);

            eML = this.Add("whales"); //_choices[4]
            ____Add_Nameless(eML);

            eML = this.Add("drones"); //_choices[5]
            ____Add_Nameless(eML);
            
            eML = this.Add("all"); //_choices[6]
            ____Add_Nameless(eML);
			
		}


	}
}