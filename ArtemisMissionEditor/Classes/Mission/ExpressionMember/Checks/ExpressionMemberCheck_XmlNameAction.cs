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

	/// <summary>
	/// Main check that checks for xml node name
	/// </summary>
	public sealed class ExpressionMemberCheck_XmlNameAction : ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			//All one-to-one connections
			switch (container.Statement.Name)
			{
				case "create":					return container.Statement.Name;
				case "add_ai":					return container.Statement.Name;
				case "clear_ai":				return container.Statement.Name;
				case "log":						return container.Statement.Name;
				case "set_variable":			return container.Statement.Name;
				case "set_timer":				return container.Statement.Name;
				case "incoming_message":		return container.Statement.Name;
				case "big_message":				return container.Statement.Name;
				case "end_mission":				return container.Statement.Name;
				case "incoming_comms_text":		return container.Statement.Name;
				case "set_object_property":		return container.Statement.Name;
				case "set_fleet_property":		return container.Statement.Name;
				case "addto_object_property":	return container.Statement.Name;
				case "copy_object_property":	return container.Statement.Name;
				case "set_relative_position":	return container.Statement.Name;
				case "set_to_gm_position":		return container.Statement.Name;
				case "set_skybox_index":		return container.Statement.Name;
				case "warning_popup_message":	return container.Statement.Name;
				case "set_difficulty_level":	return container.Statement.Name;
				case "set_player_grid_damage":	return container.Statement.Name;
				case "play_sound_now":			return container.Statement.Name;
				case "set_damcon_members":		return container.Statement.Name;
				case "direct":					return container.Statement.Name;
				case "start_getting_keypresses_from":   
				case "end_getting_keypresses_from": 
				case "set_ship_text":
					return container.Statement.Name;
				//...
			}

			//If blocks for all many-to-one connections
			if (container.Statement.Name == "destroy" || container.Statement.Name == "destroy_near")
				return "<destroy>";

			//fallback option
			return "";
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value != null)
			{
				//Only set the node name if this is a straight 1 to 1 check value, otherwise the name would be modified by other checks that follow
				if (value != "" && !value.Contains(' ') && !value.Contains('>') && !value.Contains('<'))
					container.Statement.Name = value;
				//For custom one-to-one
				//if (value == "<DIRECT_DETECTED>")
				
				//}
				//For all many-to-one
				if (value == "<destroy>")
					if (container.Statement.Name != "destroy" && container.Statement.Name != "destroy_near")
						container.Statement.Name = "destroy";

				base.SetValueInternal(container, value);
			}
		}

		public ExpressionMemberCheck_XmlNameAction()
			: base("", EMVD.GetItem("check_XmlNameA"))
		{
			List<ExpressionMember> eML;

			#region create

			eML = this.Add("create");
			eML.Add(new ExpressionMemberCheck_CreateType_H());
            eML.Add(new ExpressionMember("on side "));
            eML.Add(new ExpressionMember("<>", EMVD.GetItem("sideValue"), "sideValue"));
			#endregion

			#region destroy + destroy_near

			eML = this.Add("<destroy>");
			eML.Add(new ExpressionMemberCheck_Named_x_Nameless());

			#endregion
			#region set_ship_text
			eML = this.Add("set_ship_text");
			
			
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));

			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "name", true));

			eML.Add(new ExpressionMember("change name to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "newname"));
			eML.Add(new ExpressionMember("change race to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "race"));
			eML.Add(new ExpressionMember("change class to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "class"));
			eML.Add(new ExpressionMember("change desc to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "desc"));
			eML.Add(new ExpressionMember("on 2nd SCI scan, show "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "scan_desc"));
			eML.Add(new ExpressionMember("when hailed, reply with "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "hailtext"));

			#endregion
			AddSeparator();

			#region set_object_property

			eML = this.Add("set_object_property");
			eML.Add(new ExpressionMemberCheck_PropertySet_H());

			#endregion

			#region addto_object_property

			eML = this.Add("addto_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyAdd_H());

			#endregion

			#region copy_object_property

			eML = this.Add("copy_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyCopy_H());

			#endregion

			AddSeparator();

			#region set_fleet_property

			eML = this.Add("set_fleet_property");
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("property_f"), "property"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("value_f"), "value"));
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("fleet "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("fleetIndex"), "fleetIndex"));
			eML.Add(new ExpressionMemberCheck_BadFleetProp());

			#endregion

			#region set_relative_position

			eML = this.Add("set_relative_position");
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_all"), "name2"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("radius"), "distance"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("angle"), "angle"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("starboard "));
			eML.Add(new ExpressionMember("relative "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("movement "));
			eML.Add(new ExpressionMember("direction "));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("location "));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_all"), "name1"));

			#endregion

			#region set_to_gm_position

			eML = this.Add("set_to_gm_position");
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_x_GMSel("name_all"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("radius"), "distance"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("angle"), "angle"));
			eML.Add(new ExpressionMember("degrees "));
			eML.Add(new ExpressionMember("relative "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("GM "));
			eML.Add(new ExpressionMember("position"));

			#endregion

			AddSeparator();

			#region add_ai

			eML = this.Add("add_ai");
			eML.Add(new ExpressionMemberCheck_AIType_H());
			#endregion

			#region clear_ai

			eML = this.Add("clear_ai");
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_x_GMSel("name_all"));

			#endregion

			#region direct

			eML = this.Add("direct");
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_all"), "name", true));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMemberCheck_Point_x_TargetName());
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("value_f"), "scriptThrottle", true));
			eML.Add(new ExpressionMember("(THIS STATEMENT IS DEPRECATED AND ONLY WORKS FOR GENERICMESHES) "));
			eML.Add(new ExpressionMemberCheck_DirectConversion());

			#endregion

			AddSeparator();

			#region set_variable

			eML = this.Add("set_variable");
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_var"), "name",true));
			eML.Add(new ExpressionMemberCheck_SetVariable());

			#endregion
			#region set_special
			eML = this.Add("set_special");
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));

			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "name", true));

			eML.Add(new ExpressionMember("set ship state to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("shipState"), "ship"));
			eML.Add(new ExpressionMember("and set captain state to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("captainState"), "captain"));

			#endregion

			#region Set_side_value
            eML = this.Add("set_side_value");
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("name "));

            eML.Add(new ExpressionMember("<>", EMVD.GetItem("name"), "name", true));
            eML.Add(new ExpressionMember("and set side-value to "));
            //eML.Add(new ExpressionMember("<>", EMVD.GetItem("sideValue"), "sideValue"));
			#endregion
			#region set_timer

			eML = this.Add("set_timer");
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("name_timer"), "name",true));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("seconds"), "seconds"));
			eML.Add(new ExpressionMember("second(s)"));

			#endregion

			AddSeparator();

			#region incoming_message

			eML = this.Add("incoming_message");
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("msg_from"), "from"));
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("audio "));
			eML.Add(new ExpressionMember("file "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("msgFileName"), "fileName"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("media "));
			eML.Add(new ExpressionMember("type "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("msgMediaType"), "mediaType"));

			#endregion

			#region incoming_comms_text

			eML = this.Add("incoming_comms_text");
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("msg_from"), "from"));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("body: "));
			eML.Add(new ExpressionMember_Body());

			#endregion

			#region warning_popup_message

			eML = this.Add("warning_popup_message");
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("text "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("text"), "message"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("consoles: "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("consoles"), "consoles"));
			
			
			#endregion
			#region start_getting_keypresses_from

			eML = this.Add("start_getting_keypresses_from");
			
			eML.Add(new ExpressionMember("consoles: "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("consoles"), "consoles"));
			
			
			#endregion
			#region end_getting_keypresses_from

			eML = this.Add("end_getting_keypresses_from");

			eML.Add(new ExpressionMember("consoles: "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("consoles"), "consoles"));


			#endregion


			#region big_message

			eML = this.Add("big_message");
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("title: "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("title"), "title"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("subtitles: "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("subtitle"), "subtitle1"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("subtitle"), "subtitle2"));

			#endregion

			#region play_sound_now

			eML = this.Add("play_sound_now");
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("audio "));
			eML.Add(new ExpressionMember("file "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("soundFileName"), "filename"));

			#endregion

			AddSeparator();

			#region set_skybox_index

			eML = this.Add("set_skybox_index");
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("index"), "index"));

			#endregion

			#region set_difficulty_level

			eML = this.Add("set_difficulty_level");
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("difficulty"), "value"));

			#endregion

			#region set_player_grid_damage

			eML = this.Add("set_player_grid_damage");
			eML.Add(new ExpressionMember("value "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("damage"), "value"));
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("node "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("nodeindex"), "index"));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("ship's "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("systemType"), "systemType"));
			eML.Add(new ExpressionMember("system "));
			eML.Add(new ExpressionMember("counting "));
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("countFrom"), "countFrom"));
			eML.Add(new ExpressionMemberCheck_BadShipSystem());

			#endregion

			#region set_damcon_members

			eML = this.Add("set_damcon_members");
			eML.Add(new ExpressionMember("count "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("dcamount"), "value"));
			eML.Add(new ExpressionMember("in "));
			eML.Add(new ExpressionMember("team "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("teamindex"), "team_index"));

			#endregion

			AddSeparator();

			#region log

			eML = this.Add("log");
			eML.Add(new ExpressionMember("<>", EMVD.GetItem("text"), "text"));

			#endregion

			eML = this.Add("end_mission");

			//Finally, add the unknown option
			eML = this.Add("");
			eML.Add(new ExpressionMember_Unknown());
		}
	}

}