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
	public sealed class ExpressionMemberCheck_XmlNameAction : ExpressionMemberCheck
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
				case "create":					
				case "add_ai":					
				case "clear_ai":				
				case "log":						
				case "set_variable":			
				case "set_timer":				
				case "incoming_message":		
				case "big_message":				
				case "end_mission":				
				case "incoming_comms_text":		
				case "set_object_property":		
				case "set_fleet_property":
                case "set_comms_button":
                case "set_gm_button":
                case "gm_button":
                case "clear_comms_button":
                case "clear_gm_button":
                case "addto_object_property":	
				case "copy_object_property":	
				case "set_relative_position":	
				case "set_to_gm_position":		
				case "set_skybox_index":		
				case "warning_popup_message":	
				case "set_difficulty_level":	
				case "set_player_grid_damage":	
				case "play_sound_now":			
				case "set_damcon_members":		
				case "direct":					
				case "start_getting_keypresses_from":   
				case "end_getting_keypresses_from": 
				case "set_ship_text":
                case "set_special":
                case "set_side_value":
					return container.Statement.Name;
                case "destroy":
                case "destroy_near":
                    return "<destroy>";
				//...
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
				//Only set the node name if this is a straight 1 to 1 check value, otherwise the name would be modified by other code that will follow
				if (!String.IsNullOrEmpty(value) && !value.Contains(' ') && !value.Contains('>') && !value.Contains('<'))
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

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is the main check that checks for the xml node name.
        /// </summary>
        public ExpressionMemberCheck_XmlNameAction()
			: base("", ExpressionMemberValueDescriptions.Check_XmlNameA)
		{
			List<ExpressionMember> eML;

			#region create

			eML = this.Add("create");
			eML.Add(new ExpressionMemberCheck_CreateType());
			#endregion

			#region destroy + destroy_near

			eML = this.Add("<destroy>");
			eML.Add(new ExpressionMemberCheck_Named_Nameless());

			#endregion
			
            AddSeparator();

			#region set_object_property

			eML = this.Add("set_object_property");
			eML.Add(new ExpressionMemberCheck_PropertySet());

			#endregion

			#region addto_object_property

			eML = this.Add("addto_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyAdd());

			#endregion

			#region copy_object_property

			eML = this.Add("copy_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyCopy());

			#endregion

			AddSeparator();

			#region set_fleet_property

			eML = this.Add("set_fleet_property");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.PropertyF, "property"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueF, "value"));
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("fleet "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.FleetIndex, "fleetIndex"));
			eML.Add(new ExpressionMemberCheck_BadFleetProperty());

            #endregion

            #region set_comms_button

            eML = this.Add("set_comms_button");
            eML.Add(new ExpressionMember("with text "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "text"));
            eML.Add(new ExpressionMember(". Optional Bits: Side:"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SideValue, "sideValue"));

            #endregion

            #region clear_comms_button

            eML = this.Add("clear_comms_button");
            eML.Add(new ExpressionMember("with text "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "text"));

            #endregion

            #region set_gm_button

            eML = this.Add("set_gm_button");
            eML.Add(new ExpressionMember("with text "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "text"));
            eML.Add(new ExpressionMember(". Optional Bits: Position X:"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.GMX, "x"));
            eML.Add(new ExpressionMember(" Position Y:"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.GMY, "y"));
            eML.Add(new ExpressionMember(" Button Height:"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.GMH, "h"));
            eML.Add(new ExpressionMember(" Button Width:"));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.GMW, "w"));

            #endregion

            #region clear_gm_button

            eML = this.Add("clear_gm_button");
            eML.Add(new ExpressionMember("with text "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "text"));

            #endregion

            #region set_relative_position

            eML = this.Add("set_relative_position");
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("name "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name2"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "distance"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Angle, "angle"));
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
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name1"));

			#endregion

			#region set_to_gm_position

			eML = this.Add("set_to_gm_position");
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Radius, "distance"));
			eML.Add(new ExpressionMember("meters "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Angle, "angle"));
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
			eML.Add(new ExpressionMemberCheck_AddAIType());
			#endregion

			#region clear_ai

			eML = this.Add("clear_ai");
			eML.Add(new ExpressionMember("for "));
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMemberCheck_Name_GM(ExpressionMemberValueDescriptions.NameAll));

			#endregion

			#region direct

			eML = this.Add("direct");
			eML.Add(new ExpressionMember("object "));
			eML.Add(new ExpressionMember("named "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name", true));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMemberCheck_Point_TargetName());
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("throttle "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueF, "scriptThrottle", true));
			eML.Add(new ExpressionMember("(THIS STATEMENT IS DEPRECATED AND ONLY WORKS FOR GENERICMESHES) "));
			eML.Add(new ExpressionMemberCheck_DirectConversion());

			#endregion

			AddSeparator();

			#region set_variable

			eML = this.Add("set_variable");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameVariable, "name",true));
			eML.Add(new ExpressionMemberCheck_SetVariable());

			#endregion
			            
            #region set_timer

			eML = this.Add("set_timer");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameTimer, "name",true));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Seconds, "seconds"));
			eML.Add(new ExpressionMember("second(s)"));

			#endregion

            #region set_ship_text
            eML = this.Add("set_ship_text");

            eML.Add(new ExpressionMember("for "));
            eML.Add(new ExpressionMember("object "));
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));

            eML.Add(new ExpressionMember("change name to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameWithComma, "newname"));
            eML.Add(new ExpressionMember("change race to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameWithComma, "race"));
            eML.Add(new ExpressionMember("change class to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameWithComma, "class"));
            eML.Add(new ExpressionMember("change desc to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameWithComma, "desc"));
            eML.Add(new ExpressionMember("on 2nd SCI scan, show "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameWithComma, "scan_desc"));
            eML.Add(new ExpressionMember("when hailed, reply with "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Name, "hailtext"));

            #endregion

            #region Set_side_value
            eML = this.Add("set_side_value");
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SideValue, "value", true));
            eML.Add(new ExpressionMember("for "));
             eML.Add(new ExpressionMember("object "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));
         //   eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name", true));
            #endregion

            #region set_special
            eML = this.Add("set_special");
            eML.Add(new ExpressionMember("for AI ship "));
            eML.Add(new ExpressionMember("with "));
            eML.Add(new ExpressionMember("name "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name", true));
            eML.Add(new ExpressionMember(". Set ship"));
            eML.Add(new ExpressionMember(" sub "));
            eML.Add(new ExpressionMember("type "));
            eML.Add(new ExpressionMember("as "));
            eML.Add(new ExpressionMember("<unspecified>", ExpressionMemberValueDescriptions.ShipState, "ship"));
            eML.Add(new ExpressionMember(", with "));
            eML.Add(new ExpressionMember("captain "));
            eML.Add(new ExpressionMember("personality "));
            eML.Add(new ExpressionMember("<unspecified>", ExpressionMemberValueDescriptions.CaptainState, "captain"));
            eML.Add(new ExpressionMember("and "));
            eML.Add(new ExpressionMember("Add", ExpressionMemberValueDescriptions.SpecialSwitchState, "clear"));
            eML.Add(new ExpressionMember(" special ability "));
            eML.Add(new ExpressionMember("<unspecified>", ExpressionMemberValueDescriptions.SpecialState, "ability"));


            #endregion

            AddSeparator();

			#region incoming_message

			eML = this.Add("incoming_message");
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MessageFrom, "from"));
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("audio "));
			eML.Add(new ExpressionMember("file "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MessageFileName, "fileName"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("media "));
			eML.Add(new ExpressionMember("type "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MessageMediaType, "mediaType"));

			#endregion

			#region incoming_comms_text

			eML = this.Add("incoming_comms_text");
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MessageFrom, "from"));
			eML.Add(new ExpressionMember("with type "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.MessageType, "type"));
			eML.Add(new ExpressionMember("and body: "));
			eML.Add(new ExpressionMember_Body());

			#endregion

			#region warning_popup_message

			eML = this.Add("warning_popup_message");
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("text "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "message"));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("consoles: "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Consoles, "consoles"));
            eML.Add(new ExpressionMember(". On ship "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));


            #endregion

            #region start_getting_keypresses_from

            eML = this.Add("start_getting_keypresses_from");
			
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Consoles, "consoles"));
			
			#endregion

			#region end_getting_keypresses_from

			eML = this.Add("end_getting_keypresses_from");

            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Consoles, "consoles"));


			#endregion
            
			#region big_message

			eML = this.Add("big_message");
			eML.Add(new ExpressionMember("with "));
			eML.Add(new ExpressionMember("title: "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Title, "title"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("subtitles: "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Subtitle, "subtitle1"));
			eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Subtitle, "subtitle2"));

			#endregion

			#region play_sound_now

			eML = this.Add("play_sound_now");
			eML.Add(new ExpressionMember("using "));
			eML.Add(new ExpressionMember("audio "));
			eML.Add(new ExpressionMember("file "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SoundFileName, "filename"));

			#endregion

			AddSeparator();

			#region set_skybox_index

			eML = this.Add("set_skybox_index");
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Index, "index"));

			#endregion

			#region set_difficulty_level

			eML = this.Add("set_difficulty_level");
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Difficulty, "value"));

			#endregion

			#region set_player_grid_damage

			eML = this.Add("set_player_grid_damage");
			eML.Add(new ExpressionMember("value "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Damage, "value"));
            eML.Add(new ExpressionMember("on ship "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));
            eML.Add(new ExpressionMember(", "));
			eML.Add(new ExpressionMember("node "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NodeIndex, "index"));
			eML.Add(new ExpressionMember("of "));
			eML.Add(new ExpressionMember("ship's "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SystemType, "systemType"));
			eML.Add(new ExpressionMember("system "));
			eML.Add(new ExpressionMember("counting "));
			eML.Add(new ExpressionMember("from "));
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.CountFrom, "countFrom"));
			eML.Add(new ExpressionMemberCheck_BadShipSystem());

			#endregion

			#region set_damcon_members

			eML = this.Add("set_damcon_members");
			eML.Add(new ExpressionMember("count "));
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.DCAmount, "value"));
			eML.Add(new ExpressionMember("in "));
			eML.Add(new ExpressionMember("team "));
			eML.Add(new ExpressionMember("number "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Teamindex, "team_index"));
            eML.Add(new ExpressionMember(". On Ship "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(ExpressionMemberValueDescriptions.NameAll));

            #endregion

            AddSeparator();

			#region log

			eML = this.Add("log");
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Text, "text"));

			#endregion

			eML = this.Add("end_mission");

			//Finally, add the unknown option
			eML = this.Add("");
			eML.Add(new ExpressionMember_Unknown());
		}
	}

}
