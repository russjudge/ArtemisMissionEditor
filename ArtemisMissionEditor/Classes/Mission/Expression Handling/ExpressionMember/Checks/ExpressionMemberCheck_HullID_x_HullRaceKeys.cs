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

	public sealed class ExpressionMemberCheck_HullID_x_HullRaceKeys : ExpressionMemberCheck
	{
        
		public override string Decide(ExpressionMemberContainer container)
		{
            //if ((container.GetAttribute("hullKeys") == null && container.GetAttribute("raceKeys") == null) || !string.IsNullOrEmpty(container.GetAttribute("hullID")))
            //|| !string.IsNullOrEmpty(container.GetAttribute("hullID")
            if (container.GetAttribute("hullID") == null )
                return _choices[HullRaceKey]; // hullID
			else
                return _choices[HullID]; // hullKeys / raceKeys
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
            if (value == _choices[HullID]) //hullID
			{
				container.SetAttributeIfNull("hullID", "");
				container.SetAttribute("hullKeys", null);
				container.SetAttribute("raceKeys", null);
			}
            if (value == _choices[HullRaceKey]) //hullKeys / raceKeys
			{
				container.SetAttribute("hullID", null);
				container.SetAttributeIfNull("hullKeys", "");
				container.SetAttributeIfNull("raceKeys", "");
			}

			base.SetValueInternal(container, value);
		}



        const int HullID = 1;
        const int HullRaceKey = 0;
		public ExpressionMemberCheck_HullID_x_HullRaceKeys(bool mandatory = true)
			: base("", "check_ID/keys")
		{
			List<ExpressionMember> eML;

			//R. Judge 01/19/2013 (switched around to make race/hull keys first).





            eML = this.Add("race/hull keys"); //_choices[1]
            //In case this is hull/race keys, it looks like: "... and race/hull keys "Kralien" "Battleship"
            eML.Add(new ExpressionMember("<>", "raceKeys", "raceKeys", mandatory));
            eML.Add(new ExpressionMember("<>", "hullKeys", "hullKeys", mandatory));


            eML = this.Add("hull ID"); //_choices[0]
            //In case this is hullID, it looks like this : "... and hull ID 5003"
            eML.Add(new ExpressionMember("<>", "hullID", "hullID", mandatory));


		}
	}
}