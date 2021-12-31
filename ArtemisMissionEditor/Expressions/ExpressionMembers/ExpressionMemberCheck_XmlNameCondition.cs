using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Represents a single member in an expression, which provides branching via checking a condition.
    /// This check is the main check that checks for the xml node name.
    /// </summary>
    public sealed class ExpressionMemberCheck_XmlNameCondition : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			switch (container.Statement.Name)
			{
				case "if_variable":				
				case "if_difficulty":			
				case "if_damcon_members":		
				case "if_fleet_count":			
				case "if_docked":				
                case "if_player_is_targeting":
                case "if_timer_finished":
                case "if_object_property":
                case "if_comms_button":
                case "if_gm_key":
                case "if_gm_button":
                case "if_client_key":
                case "if_distance":
                case "if_external_program_active":
                case "if_external_program_finished":
                case "if_scan_level":
                case "if_monster_tag_matches":
                case "if_object_tag_matches":
                case "if_in_nebula":
                    return container.Statement.Name;
                case "if_exists":
                case "if_not_exists":
                    return "<existence>";
                case "if_inside_sphere":
                case "if_outside_sphere":
                case "if_inside_box":
                case "if_outside_box":
                    return "<location>";
			}

			//fallback option
			return "";
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value != null)
			{
				//Only set the node name if this is a straight 1 to 1 check value, otherwise the name would be modified by other checks that follow
				if (!String.IsNullOrEmpty(value) && !value.Contains(' ') && !value.Contains('>') && !value.Contains('<'))
					container.Statement.Name = value;
				//For custom one-to-one
				//if (value == "<DIRECT_DETECTED>")
				//{
				//}
				//For all many-to-one
				if (value == "<existence>")
					if (container.Statement.Name != "if_exists" && container.Statement.Name != "if_not_exists")
						container.Statement.Name = "if_exists";
				if (value == "<location>")
					if (container.Statement.Name != "if_inside_sphere" && container.Statement.Name != "if_outside_sphere" && container.Statement.Name != "if_inside_box" && container.Statement.Name != "if_outside_box")
						container.Statement.Name = "if_inside_sphere";

				base.SetValueInternal(container, value);
			}
		}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is the main check that checks for the xml node name.
        /// </summary>
        public ExpressionMemberCheck_XmlNameCondition()
			: base("", ExpressionMemberValueDescriptions.Check_XmlNameC)
		{
			List<ExpressionMember> eML;

			#region if_variable

			eML = this.Add("if_variable");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameVariable, "name", true));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "value",true));

			#endregion

			#region if_timer_finished

			eML = this.Add("if_timer_finished");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameTimer, "name", true));
			eML.Add(new ExpressionMember("has "));
			eML.Add(new ExpressionMember("finished"));

			#endregion

			#region if_external_program_active

			eML = this.Add("if_external_program_active");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ExternalProgramID, "id", true));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("running"));

			#endregion

			#region if_external_program_finished

			eML = this.Add("if_external_program_finished");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ExternalProgramID, "id", true));
			eML.Add(new ExpressionMember("has "));
			eML.Add(new ExpressionMember("finished"));

			#endregion

			AddSeparator();

            #region if_damcon_members

            eML = this.Add("if_damcon_members");
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NamePlayer));
            eML.Add(new ExpressionMember("in "));
			eML.Add(new ExpressionMember("team "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Teamindex, "team_index"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.DCAmountF, "value", true));

			#endregion

			#region if_fleet_count

			eML = this.Add("if_fleet_count");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_0_100k, "value", true));
			eML.Add(new ExpressionMemberCheck_AllShips_FleetNumber());
            eML.Add(new ExpressionMember("on "));
            eML.Add(new ExpressionMember("side "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SideValue, "sideValue"));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Bool_Do_Dont, "countSurrendered"));
            eML.Add(new ExpressionMember("count "));
            eML.Add(new ExpressionMember("surrendered "));
            eML.Add(new ExpressionMember("ships "));
			
			#endregion

			AddSeparator();

			#region if_docked

			eML = this.Add("if_docked");
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NamePlayer, false, "player_name"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Bool_Is_Isnt, "not"));
			eML.Add(new ExpressionMember("docked "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("station "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameStation, "name", true));

			#endregion

			#region if_in_nebula

			eML = this.Add("if_in_nebula");
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NamePlayer));
			eML.Add(new ExpressionMember("is "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NebulaTypeOrNone, "compare", true));
            eML.Add(new ExpressionMember("nebula"));

            #endregion

            #region if_player_is_targeting

            eML = this.Add("if_player_is_targeting");
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NamePlayer,
				mandatory:false,
				nameAttributeName:"player_name"));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMember("targeting "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll,
				useGMSelectionAttributeName: "<none>",
				playerSlotAttributeName:"target_slot"));

			#endregion

			AddSeparator();

			#region if_exists / if_not_exists

			eML = this.Add("<existence>");
			eML.Add(new ExpressionMemberCheck_Existence());

			#endregion

			#region if_object_property

			eML = this.Add("if_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyIf());

			#endregion

			#region if_monster_tag_matches

			eML = this.Add("if_monster_tag_matches");
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameMonster, "name", true));
			eML.Add(new ExpressionMember("has "));
			eML.Add(new ExpressionMember("tag "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "string"));

			#endregion

			#region if_object_tag_matches

			eML = this.Add("if_object_tag_matches");
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAIShip, "objectName", true));
			eML.Add(new ExpressionMember("has "));
			eML.Add(new ExpressionMember("tag "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "string"));

			#endregion
			
			#region if_scan_level

			eML = this.Add("if_scan_level");
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameScannableObject));
            eML.Add(new ExpressionMember("for "));
            eML.Add(new ExpressionMember("side "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SideValue, "side"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ScanLevel, "value", true));

			#endregion

			#region if_inside/outside_box/sphere

			eML = this.Add("<location>");
            eML.Add(new ExpressionMember(" "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));
            eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_Inside_Outside());
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMemberCheck_Box_Sphere());

			#endregion

            #region if_distance

            eML = this.Add("if_distance");
            eML.Add(new ExpressionMember("between "));
            eML.Add(new ExpressionMember("object "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name1", "<none>", "player_slot1"));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("object "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll, true, "name2", "<none>", "player_slot2"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_0_PosInf, "value", true));

            #endregion

			AddSeparator();

			#region if_difficulty

			eML = this.Add("if_difficulty");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Comparator, "comparator"));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_100, "value", true));

			#endregion

			AddSeparator();

            #region if_comms_button

            eML = this.Add("if_comms_button");
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.TextCommsButton, "text", true));

            #endregion

			#region if_gm_key

			eML = this.Add("if_gm_key");
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMemberCheck_Letter_ID());

            #endregion

            #region if_gm_button

            eML = this.Add("if_gm_button");
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.TextGMButton, "text", true));

            #endregion

            #region if_client_key

            eML = this.Add("if_client_key");
            eML.Add(new ExpressionMember("the "));
            eML.Add(new ExpressionMemberCheck_Letter_ID());

            #endregion

			//Finally, add the unknown option
			eML = this.Add("");
			eML.Add(new ExpressionMember_Unknown());
		}
	}

}
