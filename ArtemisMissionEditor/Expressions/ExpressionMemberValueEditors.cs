using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Expressions
{
    /// <summary>
    /// Contains all instances of the ExpressionMemberValueEditor class used by the editor.
    /// </summary>
    public static class ExpressionMemberValueEditors
    {
        /// <summary> Value is not edited at all </summary>
        public static ExpressionMemberValueEditor Nothing;
        /// <summary> Value will appear as an uneditable label </summary>
        public static ExpressionMemberValueEditor Label;
        /// <summary> Default editor for a Check</summary>
        public static ExpressionMemberValueEditor DefaultCheck;
        
        // Default editors for all types
        public static ExpressionMemberValueEditor DefaultInteger;
        public static ExpressionMemberValueEditor DefaultBool;
        public static ExpressionMemberValueEditor DefaultDouble;
        public static ExpressionMemberValueEditor DefaultString;
        public static ExpressionMemberValueEditor DefaultBody;
        
        /// <summary> Root check for actions </summary>
        public static ExpressionMemberValueEditor XmlNameActionCheck;
        /// <summary> Root check for conditions </summary>
        public static ExpressionMemberValueEditor XmlNameConditionCheck;
        public static ExpressionMemberValueEditor CreateType;
        public static ExpressionMemberValueEditor SetVariableCheck;
        public static ExpressionMemberValueEditor AIType;
        public static ExpressionMemberValueEditor PropertyObject;
        public static ExpressionMemberValueEditor PropertyFleet;
        public static ExpressionMemberValueEditor SkyboxIndex;
        public static ExpressionMemberValueEditor Difficulty;
        public static ExpressionMemberValueEditor ShipSystem;
        public static ExpressionMemberValueEditor CountFrom;
        public static ExpressionMemberValueEditor DamageValue;
        public static ExpressionMemberValueEditor TeamIndex;
        public static ExpressionMemberValueEditor TeamAmount;
        public static ExpressionMemberValueEditor TeamAmountF;
        public static ExpressionMemberValueEditor SpecialShipType;
        public static ExpressionMemberValueEditor MonsterType;
        public static ExpressionMemberValueEditor DriveType;
        public static ExpressionMemberValueEditor SensorRange;
        public static ExpressionMemberValueEditor PlayerSlot;
        public static ExpressionMemberValueEditor AnomalyType;
        public static ExpressionMemberValueEditor SpecialCapitainType;
        public static ExpressionMemberValueEditor SpecialSpecialState;
        public static ExpressionMemberValueEditor SpecialSpecialSwitchState;

        public static ExpressionMemberValueEditor Side;
        public static ExpressionMemberValueEditor Comparator;
        public static ExpressionMemberValueEditor DistanceNebulaCheck;
        public static ExpressionMemberValueEditor ConvertDirectCheck;
        public static ExpressionMemberValueEditor TimerName;
        public static ExpressionMemberValueEditor GMButtonText;
        public static ExpressionMemberValueEditor CommsButtonText;
        public static ExpressionMemberValueEditor GMText;
        public static ExpressionMemberValueEditor VariableName;
        public static ExpressionMemberValueEditor NamedAllName;
        public static ExpressionMemberValueEditor NamedStationName;
        public static ExpressionMemberValueEditor ConsoleList;
        public static ExpressionMemberValueEditor EliteAIType;
        public static ExpressionMemberValueEditor EliteAbilityBits;
        public static ExpressionMemberValueEditor PlayerNames;
        public static ExpressionMemberValueEditor WarpState;
        public static ExpressionMemberValueEditor PathEditor;
        public static ExpressionMemberValueEditor HullID;
        public static ExpressionMemberValueEditor RaceKeys;
        public static ExpressionMemberValueEditor HullKeys;
        public static ExpressionMemberValueEditor VariableType;

        static ExpressionMemberValueEditors()
		{
			Nothing = new ExpressionMemberValueEditor(false, false);

			Label = new ExpressionMemberValueEditor(true, false);

            DefaultInteger = new ExpressionMemberValueEditor();
			DefaultDouble = new ExpressionMemberValueEditor();
			DefaultString = new ExpressionMemberValueEditor();
			DefaultBody = new ExpressionMemberValueEditor();
            DefaultBool = new ExpressionMemberValueEditor();
            DefaultBool.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultBool;

			CreateType = new ExpressionMemberValueEditor();
			CreateType.AddToDictionary("Anomaly",       "Anomaly");
			CreateType.AddToDictionary("blackHole",     "black hole");
            CreateType.AddToDictionary("enemy",         "enemy");
            CreateType.AddToDictionary("genericMesh",   "generic mesh");
            CreateType.AddToDictionary("monster",       "monster");
            CreateType.AddToDictionary("neutral",       "neutral");
            CreateType.AddToDictionary("player",        "player");
			CreateType.AddToDictionary("station",       "station");
			//CreateType.AddToDictionary("whale",         "whale");
			CreateType.AddToDictionary("asteroids",     "asteroids");
            CreateType.AddToDictionary("mines",         "mines");
            CreateType.AddToDictionary("nebulas",       "nebulas");
			CreateType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList; 

			DefaultCheck = new ExpressionMemberValueEditor();
			DefaultCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

            XmlNameActionCheck = new ExpressionMemberValueEditor_XmlName();
            XmlNameActionCheck.AddToDictionary("set_difficulty_level", "Set difficulty");

           XmlNameActionCheck.AddToDictionary("set_skybox_index", "Set Skybox");
           // XmlNameActionCheck.AddToDictionary("set_difficulty_level", "Set difficulty");
            XmlNameActionCheck.AddToDictionary("end_mission", "End Mission");
            XmlNameActionCheck.AddToDictionary("log", "Log new entry");
            XmlNameActionCheck.AddToDictionary("spawn_external_program", "Spawn External Program");

            //XmlNameActionCheck.AddToDictionary("<destroy>", "Destroy");
            XmlNameActionCheck.NewMenuGroup("Game Options");
            XmlNameActionCheck.AddToDictionary("create", "Create");
            XmlNameActionCheck.AddToDictionary("<destroy>", "Destroy");
            XmlNameActionCheck.NewMenuGroup("Create/Destroy");

            
            XmlNameActionCheck.AddToDictionary("set_comms_button", "Set Comms Button");
            XmlNameActionCheck.AddToDictionary("clear_comms_button", "Clear Comms Button");
            XmlNameActionCheck.AddToDictionary("set_gm_button", "Set GM Button");
            XmlNameActionCheck.AddToDictionary("clear_gm_button", "Clear GM Button");
            XmlNameActionCheck.AddToDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles: ");
            XmlNameActionCheck.AddToDictionary("end_getting_keypresses_from", "End getting keypresses from consoles: ");
            XmlNameActionCheck.NewMenuGroup("Buttons");

            XmlNameActionCheck.AddToDictionary("incoming_comms_text", "Show incoming text message");
            XmlNameActionCheck.AddToDictionary("incoming_message", "Show incoming audio message");
            XmlNameActionCheck.AddToDictionary("warning_popup_message", "Show warning popup message");
            XmlNameActionCheck.AddToDictionary("big_message", "Show big message on main screen");
            XmlNameActionCheck.AddToDictionary("play_sound_now", "Play sound on main screen");
            XmlNameActionCheck.NewMenuGroup("Messages");

            XmlNameActionCheck.AddToDictionary("set_damcon_members", "Set DamCon members");
            XmlNameActionCheck.AddToDictionary("set_player_grid_damage", "Set player ship's damage");
            XmlNameActionCheck.NewMenuGroup("Damage/Damcon");

            XmlNameActionCheck.AddToDictionary("set_timer", "Set Timer");
            XmlNameActionCheck.AddToDictionary("set_variable", "Set Variable");
            XmlNameActionCheck.NewMenuGroup("Triggers");

            XmlNameActionCheck.AddToDictionary("set_object_property", "Set property");        
            XmlNameActionCheck.AddToDictionary("addto_object_property", "Add to property");
            XmlNameActionCheck.AddToDictionary("copy_object_property", "Copy property");
            XmlNameActionCheck.AddToDictionary("set_ship_text", "Set text strings");
            XmlNameActionCheck.AddToDictionary("set_special", "Set Special");
            XmlNameActionCheck.AddToDictionary("set_side_value", "Set side");     
            XmlNameActionCheck.AddToDictionary("set_fleet_property", "Set property of Fleet");
            XmlNameActionCheck.NewMenuGroup("Properties");

            XmlNameActionCheck.AddToDictionary("set_relative_position", "Set Relative Position");
            XmlNameActionCheck.AddToDictionary("set_to_gm_position", "Set Relative to GM Position");
            XmlNameActionCheck.NewMenuGroup("Position");

            XmlNameActionCheck.AddToDictionary("clear_ai", "Clear AI command stack");
            XmlNameActionCheck.AddToDictionary("add_ai", "Add an AI command");
            XmlNameActionCheck.AddToDictionary("direct", "Direct Generic Mesh");
            XmlNameActionCheck.NewMenuGroup("AI");


            XmlNameActionCheck.AddToMenuDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles");
            XmlNameActionCheck.AddToMenuDictionary("end_getting_keypresses_from", "End getting keypresses from consoles");
            XmlNameActionCheck.AddToMenuDictionary("set_special", "Set ship's special values");
            XmlNameActionCheck.AddToMenuDictionary("set_side_value", "Set object's side");
            XmlNameActionCheck.AddToMenuDictionary("set_ship_text", "Set object's text");
            XmlNameActionCheck.AddToMenuDictionary("set_object_property", "Set property of object");
            XmlNameActionCheck.AddToMenuDictionary("copy_object_property", "Copy property of object");
            XmlNameActionCheck.AddToMenuDictionary("addto_object_property", "Add to property of object");
            XmlNameActionCheck.AddToMenuDictionary("set_fleet_property", "Set property of fleet");
            XmlNameActionCheck.AddToMenuDictionary("set_relative_position", "Set position relative to object");
            XmlNameActionCheck.AddToMenuDictionary("add_ai", "Add AI command");
            XmlNameActionCheck.AddToMenuDictionary("clear_ai", "Clear AI commands");
            XmlNameActionCheck.AddToMenuDictionary("set_to_gm_position", "Set position relative to GM position");
            XmlNameActionCheck.AddToMenuDictionary("create", "Create object(s)");
            XmlNameActionCheck.AddToMenuDictionary("<destroy>", "Destroy object(s)");
            XmlNameActionCheck.AddToMenuDictionary("direct", "Direct object to object / position");
           // XmlNameActionCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

            //XmlNameActionCheck = new ExpressionMemberValueEditor_XmlName();
            //// XmlNameActionCheck.NewMenuGroup("Interaction1");
            // XmlNameActionCheck.NewMenuGroup("Interaction");
            //XmlNameActionCheck.AddToDictionary("set_gm_button", "Set GM Button");
            //XmlNameActionCheck.AddToDictionary("clear_gm_button", "clear GM Button");
            //XmlNameActionCheck.AddToDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles: ");
            //XmlNameActionCheck.AddToDictionary("end_getting_keypresses_from", "End getting keypresses from consoles: ");
            //XmlNameActionCheck.NewMenuGroup("Ship Atributes");
            //XmlNameActionCheck.AddToDictionary("set_special", "Set");
            //XmlNameActionCheck.AddToDictionary("set_side_value", "Set side");
            //XmlNameActionCheck.AddToDictionary("set_ship_text", "Set text strings");
            //XmlNameActionCheck.AddToDictionary("clear_ai", "Clear AI command stack");
            //XmlNameActionCheck.AddToDictionary("add_ai", "Add an AI command");
            //XmlNameActionCheck.AddToDictionary("set_player_grid_damage", "Set player ship's damage");
            //XmlNameActionCheck.AddToDictionary("<destroy>", "Destroy");
            //XmlNameActionCheck.AddToDictionary("set_object_property", "Set property");
            //XmlNameActionCheck.AddToDictionary("set_fleet_property", "Set property");
            //XmlNameActionCheck.AddToDictionary("set_damcon_members", "Set DamCon members");
            //XmlNameActionCheck.NewMenuGroup("Object Properties");
            //XmlNameActionCheck.AddToDictionary("addto_object_property", "Add");
            //XmlNameActionCheck.AddToDictionary("copy_object_property", "Copy property");
            //XmlNameActionCheck.AddToDictionary("set_relative_position", "Set position");
            //XmlNameActionCheck.AddToDictionary("set_to_gm_position", "Set position");
            //XmlNameActionCheck.NewMenuGroup("Messages");
            //XmlNameActionCheck.AddToDictionary("incoming_comms_text", "Show incoming text message");
            //XmlNameActionCheck.AddToDictionary("incoming_message", "Show incoming audio message");
            //XmlNameActionCheck.AddToDictionary("warning_popup_message", "Show warning popup message");
            //XmlNameActionCheck.AddToDictionary("big_message", "Show big message on main screen");
            //XmlNameActionCheck.AddToDictionary("play_sound_now", "Play sound on main screen");
            //XmlNameActionCheck.AddToDictionary("log", "Log new entry");


            //XmlNameActionCheck.AddToMenuDictionary("set_gm_button", "Set GM Button");
            //XmlNameActionCheck.AddToMenuDictionary("clear_gm_button", "clear GM Button");
            //XmlNameActionCheck.AddToMenuDictionary("start_getting_keypresses_from", "Start getting keypresses from consoles: ");
            //XmlNameActionCheck.AddToMenuDictionary("end_getting_keypresses_from", "End getting keypresses from consoles: ");
            ////  XmlNameActionCheck.NewMenuGroup("Ship Atributes");
            //XmlNameActionCheck.AddToMenuDictionary("set_special", "Set");
            //XmlNameActionCheck.AddToMenuDictionary("set_side_value", "Set side");
            //XmlNameActionCheck.AddToMenuDictionary("set_ship_text", "Set text strings");
            //XmlNameActionCheck.AddToMenuDictionary("clear_ai", "Clear AI command stack");
            //XmlNameActionCheck.AddToMenuDictionary("add_ai", "Add an AI command");
            //XmlNameActionCheck.AddToMenuDictionary("set_player_grid_damage", "Set player ship's damage");
            //XmlNameActionCheck.AddToMenuDictionary("<destroy>", "Destroy");
            //XmlNameActionCheck.AddToMenuDictionary("set_object_property", "Set property");
            //XmlNameActionCheck.AddToMenuDictionary("set_fleet_property", "Set property");
            //XmlNameActionCheck.AddToMenuDictionary("set_damcon_members", "Set DamCon members");
            ////  XmlNameActionCheck.NewMenuGroup("Object Properties");
            //XmlNameActionCheck.AddToMenuDictionary("addto_object_property", "Add");
            //XmlNameActionCheck.AddToMenuDictionary("copy_object_property", "Copy property");
            //XmlNameActionCheck.AddToMenuDictionary("set_relative_position", "Set position");
            //XmlNameActionCheck.AddToMenuDictionary("set_to_gm_position", "Set position");
            //// XmlNameActionCheck.NewMenuGroup("Messages");
            //XmlNameActionCheck.AddToMenuDictionary("incoming_comms_text", "Show incoming text message");
            //XmlNameActionCheck.AddToMenuDictionary("incoming_message", "Show incoming audio message");
            //XmlNameActionCheck.AddToMenuDictionary("warning_popup_message", "Show warning popup message");
            //XmlNameActionCheck.AddToMenuDictionary("big_message", "Show big message on main screen");
            //XmlNameActionCheck.AddToMenuDictionary("play_sound_now", "Play sound on main screen");
            //XmlNameActionCheck.AddToMenuDictionary("log", "Log new entry");



            XmlNameActionCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_NestedList;
           // XmlNameActionCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;


            XmlNameConditionCheck = new ExpressionMemberValueEditor_XmlName();
			XmlNameConditionCheck.AddToDictionary("if_variable", "Variable");
			XmlNameConditionCheck.AddToDictionary("if_timer_finished", "Timer");
			XmlNameConditionCheck.AddToDictionary("if_damcon_members", "Amount of DamCon members");
			XmlNameConditionCheck.AddToDictionary("if_fleet_count", "Ship count");
			XmlNameConditionCheck.AddToDictionary("if_docked", "Player ship is docked");
			XmlNameConditionCheck.AddToDictionary("if_player_is_targeting", "Player ship is targeting");
			XmlNameConditionCheck.AddToDictionary("<existance>", "Object");
            XmlNameConditionCheck.AddToDictionary("<location>", "Object");
            XmlNameConditionCheck.AddToDictionary("if_object_property", "Property");
			XmlNameConditionCheck.AddToDictionary("if_distance", "Distance");
			XmlNameConditionCheck.AddToDictionary("if_difficulty", "Difficulty level");
            XmlNameConditionCheck.AddToDictionary("if_comms_button", "Comms Button pressed");
			XmlNameConditionCheck.AddToDictionary("if_gm_key", "GM key pressed");
            XmlNameConditionCheck.AddToDictionary("if_gm_button", "GM Button pressed");
            XmlNameConditionCheck.AddToDictionary("if_client_key", "Client pressed");
            XmlNameConditionCheck.AddToMenuDictionary("if_timer_finished", "Timer has finished");
			XmlNameConditionCheck.AddToMenuDictionary("if_damcon_members", "Amount of DamCon members");
			XmlNameConditionCheck.AddToMenuDictionary("if_fleet_count", "Ship count (in a fleet)");
            XmlNameConditionCheck.AddToMenuDictionary("<existance>", "Object exists / doesn't");
			XmlNameConditionCheck.AddToMenuDictionary("if_object_property", "Object property");
			XmlNameConditionCheck.AddToMenuDictionary("<location>", "Object is located");
			XmlNameConditionCheck.AddToMenuDictionary("if_distance", "Distance between objects");
            XmlNameConditionCheck.AddToMenuDictionary("if_comms_button", "Comms Button pressed");
			XmlNameConditionCheck.AddToMenuDictionary("if_gm_key", "GM pressed a key");
            XmlNameConditionCheck.AddToMenuDictionary("if_gm_button", "GM Button pressed");
            XmlNameConditionCheck.AddToMenuDictionary("if_client_key", "Client pressed a key");
			XmlNameConditionCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

			SetVariableCheck = new ExpressionMemberValueEditor();
			SetVariableCheck.AddToDictionary("<to>", "to");
			SetVariableCheck.AddToMenuDictionary("<to>", "to an exact value");
			SetVariableCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

			AIType = new ExpressionMemberValueEditor();
			AIType.AddToDictionary("TRY_TO_BECOME_LEADER", "TRY TO BECOME LEADER");
			AIType.AddToDictionary("CHASE_PLAYER", "CHASE PLAYER ");
			AIType.AddToDictionary("CHASE_AI_SHIP", "CHASE AI SHIP ");
			AIType.AddToDictionary("CHASE_STATION", "CHASE STATION ");
			AIType.AddToDictionary("CHASE_WHALE", "CHASE WHALE ");
			AIType.AddToDictionary("AVOID_WHALE", "AVOID WHALE ");
			AIType.AddToDictionary("AVOID_BLACK_HOLE", "AVOID BLACK HOLE ");
			AIType.AddToDictionary("CHASE_ANGER", "CHASE ANGER");
			AIType.AddToDictionary("CHASE_FLEET", "CHASE FLEET ");
			AIType.AddToDictionary("FOLLOW_LEADER", "FOLLOW LEADER");
			AIType.AddToDictionary("FOLLOW_COMMS_ORDERS", "FOLLOW COMMS ORDERS");
			AIType.AddToDictionary("LEADER_LEADS", "LEADER LEADS");
			AIType.AddToDictionary("SPCL_AI", "ELITE AI");
			AIType.AddToDictionary("DIR_THROTTLE", "DIR THROTTLE ");
			AIType.AddToDictionary("POINT_THROTTLE", "POINT THROTTLE ");
			AIType.AddToDictionary("TARGET_THROTTLE", "TARGET THROTTLE ");
			AIType.AddToDictionary("ATTACK", "ATTACK ");
			AIType.AddToDictionary("DEFEND", "DEFEND ");
			AIType.AddToDictionary("PROCEED_TO_EXIT", "PROCEED TO EXIT");
			AIType.AddToDictionary("FIGHTER_BINGO", "FIGHTER BINGO");
			AIType.AddToDictionary("LAUNCH_FIGHTERS", "LAUNCH FIGHTERS ");
			AIType.AddToDictionary("GUARD_STATION", "GUARD STATION ");
			AIType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			PropertyObject = new ExpressionMemberValueEditor();
            PropertyObject.AddToDictionary("coopAdjustmentValue", "Co-op Adjustment Value");
            PropertyObject.AddToDictionary("nonPlayerWeapon", "Enemy Damage");
            PropertyObject.AddToDictionary("nonPlayerShield", "Enemy Shield");
            PropertyObject.AddToDictionary("nonPlayerSpeed", "Enemy Speed");
            PropertyObject.AddToDictionary("commsObjectMasterVolume", "Game Comms Volume");
            PropertyObject.AddToDictionary("musicObjectMasterVolume", "Game Music Volume");
            PropertyObject.AddToDictionary("soundFXVolume", "Game Sound Volume");
            PropertyObject.AddToDictionary("gameTimeLimit", "Game Time Limit");
            PropertyObject.AddToDictionary("nebulaIsOpaque", "Nebular Hides From Sensors");
            PropertyObject.AddToDictionary("networkTickSpeed", "Network Tick Speed");
            PropertyObject.AddToDictionary("playerWeapon", "Player Damage");
            PropertyObject.AddToDictionary("playerShields", "Player Shield");
            PropertyObject.AddToDictionary("sensorSetting", "Sensor Range");

            // PropertyObject.AddToDictionary("roll", "roll");
            // PropertyObject.AddToDictionary("", "sideValue");
            PropertyObject.NewMenuGroup("Game Properties");
            PropertyObject.AddToDictionary("positionX", "positionX");
			PropertyObject.AddToDictionary("positionY", "positionY");
			PropertyObject.AddToDictionary("positionZ", "positionZ");
			PropertyObject.AddToDictionary("deltaX", "deltaX");
			PropertyObject.AddToDictionary("deltaY", "deltaY");
			PropertyObject.AddToDictionary("deltaZ", "deltaZ");
			PropertyObject.AddToDictionary("angle", "angle");
			PropertyObject.AddToDictionary("pitch", "pitch");
			PropertyObject.AddToDictionary("roll", "roll");
            PropertyObject.AddToDictionary("sideValue", "sideValue");
			PropertyObject.NewMenuGroup("All objects");
			PropertyObject.AddToDictionary("blocksShotFlag", "blocksShotFlag");
			PropertyObject.AddToDictionary("pushRadius", "pushRadius");
			PropertyObject.AddToDictionary("pitchDelta", "pitchDelta");
			PropertyObject.AddToDictionary("rollDelta", "rollDelta");
			PropertyObject.AddToDictionary("angleDelta", "angleDelta");
			PropertyObject.AddToDictionary("artScale", "artScale");
			PropertyObject.NewMenuGroup("Generic meshes");
			PropertyObject.AddToDictionary("shieldState", "shieldState");
			PropertyObject.AddToDictionary("canBuild", "canBuild");
			PropertyObject.AddToDictionary("missileStoresHoming", "missileStoresHoming");
			PropertyObject.AddToDictionary("missileStoresNuke", "missileStoresNuke");
			PropertyObject.AddToDictionary("missileStoresMine", "missileStoresMine");
			PropertyObject.AddToDictionary("missileStoresECM", "missileStoresECM");
            PropertyObject.AddToDictionary("missileStoresPShock", "missileStoresPShock");
            PropertyObject.NewMenuGroup("Stations");
			PropertyObject.AddToDictionary("throttle", "throttle");
			PropertyObject.AddToDictionary("steering", "steering");
			PropertyObject.AddToDictionary("topSpeed", "topSpeed");
			PropertyObject.AddToDictionary("turnRate", "turnRate");
			PropertyObject.AddToDictionary("shieldStateFront", "shieldStateFront");
			PropertyObject.AddToDictionary("shieldMaxStateFront", "shieldMaxStateFront");
			PropertyObject.AddToDictionary("shieldStateBack", "shieldStateBack");
			PropertyObject.AddToDictionary("shieldMaxStateBack", "shieldMaxStateBack");
			PropertyObject.AddToDictionary("shieldsOn", "shieldsOn");
			PropertyObject.AddToDictionary("triggersMines", "triggersMines");
			PropertyObject.AddToDictionary("systemDamageBeam", "systemDamageBeam");
			PropertyObject.AddToDictionary("systemDamageTorpedo", "systemDamageTorpedo");
			PropertyObject.AddToDictionary("systemDamageTactical", "systemDamageTactical");
			PropertyObject.AddToDictionary("systemDamageTurning", "systemDamageTurning");
			PropertyObject.AddToDictionary("systemDamageImpulse", "systemDamageImpulse");
			PropertyObject.AddToDictionary("systemDamageWarp", "systemDamageWarp");
			PropertyObject.AddToDictionary("systemDamageFrontShield", "systemDamageFrontShield");
			PropertyObject.AddToDictionary("systemDamageBackShield", "systemDamageBackShield");
			PropertyObject.AddToDictionary("shieldBandStrength0", "shieldBandStrength0");
			PropertyObject.AddToDictionary("shieldBandStrength1", "shieldBandStrength1");
			PropertyObject.AddToDictionary("shieldBandStrength2", "shieldBandStrength2");
			PropertyObject.AddToDictionary("shieldBandStrength3", "shieldBandStrength3");
			PropertyObject.AddToDictionary("shieldBandStrength4", "shieldBandStrength4");
			PropertyObject.NewMenuGroup("Shielded ships");
           // PropertyObject.AddToDictionary("setShipSide", "Set Ship Side");
            PropertyObject.AddToDictionary("targetPointX", "targetPointX");
			PropertyObject.AddToDictionary("targetPointY", "targetPointY");
			PropertyObject.AddToDictionary("targetPointZ", "targetPointZ");
			PropertyObject.AddToDictionary("hasSurrendered", "hasSurrendered");
            PropertyObject.AddToDictionary("tauntImmunityIndex", "tauntImmunityIndex");
            PropertyObject.AddToDictionary("eliteAIType", "eliteAIType");
			PropertyObject.AddToDictionary("eliteAbilityBits", "eliteAbilityBits");
			PropertyObject.AddToDictionary("eliteAbilityState", "eliteAbilityState");
			PropertyObject.AddToDictionary("surrenderChance", "surrenderChance");
			PropertyObject.NewMenuGroup("Enemies");
			PropertyObject.AddToDictionary("exitPointX", "exitPointX");
			PropertyObject.AddToDictionary("exitPointY", "exitPointY");
			PropertyObject.AddToDictionary("exitPointZ", "exitPointZ");
			PropertyObject.NewMenuGroup("Neutrals");
			PropertyObject.AddToDictionary("countHoming", "countHoming");
			PropertyObject.AddToDictionary("countNuke", "countNuke");
			PropertyObject.AddToDictionary("countMine", "countMine");
			PropertyObject.AddToDictionary("countECM", "countECM");
			PropertyObject.AddToDictionary("energy", "energy");
			PropertyObject.AddToDictionary("warpState", "warpState");
			PropertyObject.AddToDictionary("currentRealSpeed", "currentRealSpeed");
			PropertyObject.AddToDictionary("totalCoolant", "totalCoolant");
			PropertyObject.NewMenuGroup("Players");
			PropertyObject.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_NestedList;

			PropertyFleet = new ExpressionMemberValueEditor();
			PropertyFleet.AddToDictionary("fleetSpacing",   "spacing");
			PropertyFleet.AddToDictionary("fleetMaxRadius", "max radius");
			PropertyFleet.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			SkyboxIndex = new ExpressionMemberValueEditor();
            for (int i = 0; i <= 29; i++)
                SkyboxIndex.AddToDictionary(i.ToString(), i.ToString());
			SkyboxIndex.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListPlusDialog;

			Difficulty = new ExpressionMemberValueEditor();
            for (int i = 0; i <= 11; i++)
                Difficulty.AddToDictionary(i.ToString(), i.ToString());
			Difficulty.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			ShipSystem = new ExpressionMemberValueEditor();
			ShipSystem.AddToDictionary("systemBeam",            "Primary Beam");
			ShipSystem.AddToDictionary("systemTorpedo",         "Torpedo");
			ShipSystem.AddToDictionary("systemTactical",        "Sensors");
			ShipSystem.AddToDictionary("systemTurning",         "Maneuver");
			ShipSystem.AddToDictionary("systemImpulse",         "Impulse");
			ShipSystem.AddToDictionary("systemWarp",            "Warp");
			ShipSystem.AddToDictionary("systemFrontShield",     "Front Shield");
			ShipSystem.AddToDictionary("systemBackShield",      "Rear Shield");
			ShipSystem.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			CountFrom = new ExpressionMemberValueEditor();
			CountFrom.AddToDictionary("left",   "left");
			CountFrom.AddToDictionary("top",    "top");
			CountFrom.AddToDictionary("front",  "front");
			CountFrom.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			DamageValue = new ExpressionMemberValueEditor_Percent();
			DamageValue.AddToDictionary("0.0",  "0%");
			DamageValue.AddToDictionary("0.25", "25%");
			DamageValue.AddToDictionary("0.5",  "50%");
			DamageValue.AddToDictionary("0.75", "75%");
			DamageValue.AddToDictionary("1.0",  "100%");
			DamageValue.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListPlusDialog;

			TeamIndex = new ExpressionMemberValueEditor();
			TeamIndex.AddToDictionary("0", "0");
			TeamIndex.AddToDictionary("1", "1");
			TeamIndex.AddToDictionary("2", "2");
			TeamIndex.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			TeamAmount = new ExpressionMemberValueEditor();
			TeamAmount.AddToDictionary("0", "0");
			TeamAmount.AddToDictionary("1", "1");
			TeamAmount.AddToDictionary("2", "2");
			TeamAmount.AddToDictionary("3", "3");
			TeamAmount.AddToDictionary("4", "4");
			TeamAmount.AddToDictionary("5", "5");
			TeamAmount.AddToDictionary("6", "6");
			TeamAmount.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			TeamAmountF = new ExpressionMemberValueEditor();
			TeamAmountF.AddToDictionary("0", "0");
			TeamAmountF.AddToDictionary("1", "1");
			TeamAmountF.AddToDictionary("2", "2");
			TeamAmountF.AddToDictionary("3", "3");
			TeamAmountF.AddToDictionary("4", "4");
			TeamAmountF.AddToDictionary("5", "5");
			TeamAmountF.AddToDictionary("6", "6");
			TeamAmountF.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListPlusDialog;

            SpecialShipType = new ExpressionMemberValueEditor();
            SpecialShipType.AddToDictionary(null,   "Unspecified");
            SpecialShipType.AddToDictionary("-1",   "Nothing");
            SpecialShipType.AddToDictionary("0",    "Upgraded");
            SpecialShipType.AddToDictionary("1",    "Overpowered");
            SpecialShipType.AddToDictionary("2",    "Underpowered");
            //SpecialShipType.AddToDictionary("3",    "Dilapidated");
            SpecialShipType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            MonsterType = new ExpressionMemberValueEditor();
            MonsterType.AddToDictionary("0", "Classic");
            MonsterType.AddToDictionary("1", "Whale");
            MonsterType.AddToDictionary("2", "Shark");
            MonsterType.AddToDictionary("3", "Dragon");
            MonsterType.AddToDictionary("4", "Piranha");
            MonsterType.AddToDictionary("5", "Tube");
            MonsterType.AddToDictionary("6", "Bug");
            MonsterType.AddToDictionary("7", "Derelict");
            MonsterType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            DriveType = new ExpressionMemberValueEditor();
            DriveType.AddToDictionary("", "Any");
            DriveType.AddToDictionary("yes", "Warp");
            DriveType.AddToDictionary("no", "Jump");
            DriveType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            VariableType = new ExpressionMemberValueEditor();
            VariableType.AddToDictionary("", "Float");
            VariableType.AddToDictionary("yes", "Integer");
            VariableType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            SensorRange = new ExpressionMemberValueEditor();
            SensorRange.AddToDictionary("0", "Unlimited");
            SensorRange.AddToDictionary("1", "33K");
            SensorRange.AddToDictionary("2", "16K");
            SensorRange.AddToDictionary("3", "11K");
            SensorRange.AddToDictionary("4", "8K");
            SensorRange.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;


            AnomalyType = new ExpressionMemberValueEditor();
            AnomalyType.AddToDictionary("0", "Energy");
            AnomalyType.AddToDictionary("1", "DamCon");
            AnomalyType.AddToDictionary("2", "Heat");
            AnomalyType.AddToDictionary("3", "Scan");
            AnomalyType.AddToDictionary("4", "Beam");
            AnomalyType.AddToDictionary("5", "Speed");
            AnomalyType.AddToDictionary("6", "Shield");
            AnomalyType.AddToDictionary("7", "Code Case");
            AnomalyType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            SpecialCapitainType = new ExpressionMemberValueEditor();
            SpecialCapitainType.AddToDictionary(null,   "Unspecified");
            SpecialCapitainType.AddToDictionary("-1",   "Nothing");
            SpecialCapitainType.AddToDictionary("0",    "Cowardly");
            SpecialCapitainType.AddToDictionary("1",    "Brave");
            SpecialCapitainType.AddToDictionary("2",    "Bombastic");
            SpecialCapitainType.AddToDictionary("3",    "Seething");
            SpecialCapitainType.AddToDictionary("4",    "Duplicitous");
            SpecialCapitainType.AddToDictionary("5",    "Exceptional");
            SpecialCapitainType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            SpecialSpecialSwitchState = new ExpressionMemberValueEditor();
            SpecialSpecialSwitchState.AddToDictionary(null, "add");
            SpecialSpecialSwitchState.AddToDictionary("clear", "Clear");
            SpecialSpecialSwitchState.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            SpecialSpecialState = new ExpressionMemberValueEditor();
            SpecialSpecialState.AddToDictionary(null, "Unspecified");
            SpecialSpecialState.AddToDictionary("Stealth", "Stealth");
            SpecialSpecialState.AddToDictionary("LowVis", "LowVis");
            SpecialSpecialState.AddToDictionary("Cloak", "Cloak");
            SpecialSpecialState.AddToDictionary("HET", "HET");
            SpecialSpecialState.AddToDictionary("Warp", "Warp");
            SpecialSpecialState.AddToDictionary("Teleport", "Teleport");
            SpecialSpecialState.AddToDictionary("Tractor", "Tractor");
            SpecialSpecialState.AddToDictionary("AntiMine", "AntiMine");
            SpecialSpecialState.AddToDictionary("AntiTorp", "AntiTorp");
            SpecialSpecialState.AddToDictionary("ShldDrain", "ShldDrain");
            SpecialSpecialState.AddToDictionary("ShldVamp", "ShldVamp");
            SpecialSpecialState.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListWithFirstSeparated;

            Side = new ExpressionMemberValueEditor();
            Side.AddToDictionary(null,  "Default");
            Side.AddToDictionary("0",   "0 (No side)");
            Side.AddToDictionary("1",   "1 (Enemy)");
            Side.AddToDictionary("2",   "2 (Player)");
            Side.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultListPlusDialogWithFirstSepearted;

			Comparator = new ExpressionMemberValueEditor();
			Comparator.AddToDictionary("GREATER",       ">");
			Comparator.AddToDictionary("GREATER_EQUAL", ">=");
			Comparator.AddToDictionary("EQUALS",        "=");
			Comparator.AddToDictionary("NOT",           "!=");
			Comparator.AddToDictionary("LESS_EQUAL",    "<=");
			Comparator.AddToDictionary("LESS",          "<");
			Comparator.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			DistanceNebulaCheck = new ExpressionMemberValueEditor();
			DistanceNebulaCheck.AddToDictionary("anywhere2", "anywhere");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere", "anywhere on the map");
			DistanceNebulaCheck.AddToMenuDictionary("anywhere2", "anywhere outside a nebula or up to ... if inside");
			DistanceNebulaCheck.AddToMenuDictionary("closer than", "closer than ... if outside a nebula or up to ... if inside");
			DistanceNebulaCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

			ConvertDirectCheck = new ExpressionMemberValueEditor();
			ConvertDirectCheck.AddToDictionary("Do nothing", "(Click here to convert)");
			ConvertDirectCheck.AddToDictionary("Convert to add_ai", "YOU SHOULD NEVER SEE THIS");
			ConvertDirectCheck.AddToMenuDictionary("Do nothing", "Do nothing");
			ConvertDirectCheck.AddToMenuDictionary("Convert to add_ai", "Convert to add_ai");
			ConvertDirectCheck.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultCheck;

			TimerName = new ExpressionMemberValueEditor();
			TimerName.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_TimerNameList;
            Mission.NamesListUpdated += new Action(() => { TimerName.InvalidateContextMenuStrip(); });

			VariableName = new ExpressionMemberValueEditor();
			VariableName.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_VariableNameList;
            Mission.NamesListUpdated += new Action(() => { VariableName.InvalidateContextMenuStrip(); });

			GMButtonText = new ExpressionMemberValueEditor();
			GMButtonText.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_GMButtonTextList;
            Mission.NamesListUpdated += new Action(() => { GMButtonText.InvalidateContextMenuStrip(); });

			CommsButtonText = new ExpressionMemberValueEditor();
			CommsButtonText.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_CommsButtonTextList;
            Mission.NamesListUpdated += new Action(() => { CommsButtonText.InvalidateContextMenuStrip(); });

			NamedAllName = new ExpressionMemberValueEditor();
			NamedAllName.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_NamedAllNameList;
            Mission.NamesListUpdated += new Action(() => { NamedAllName.InvalidateContextMenuStrip(); });
			
			NamedStationName = new ExpressionMemberValueEditor();
			NamedStationName.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_NamedStationNameList;
            Mission.NamesListUpdated += new Action(() => { NamedStationName.InvalidateContextMenuStrip(); });

			ConsoleList = new ExpressionMemberValueEditor_ConsoleList();

			EliteAIType = new ExpressionMemberValueEditor();
			EliteAIType.AddToDictionary("0",    "behave like a normal ship");
			EliteAIType.AddToDictionary("0.0",  "behave like a normal ship");
			EliteAIType.AddToDictionary("1",    "follow nearest normal fleet");
			EliteAIType.AddToDictionary("1.0",  "follow nearest normal fleet");
			EliteAIType.AddToDictionary("2",    "ignore everything except players");
			EliteAIType.AddToDictionary("2.0",  "ignore everything except players");
			EliteAIType.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			EliteAbilityBits = new ExpressionMemberValueEditor_AbilityBits();

			PlayerNames = new ExpressionMemberValueEditor();
			PlayerNames.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_PlayerListPlusDialog;

			WarpState = new ExpressionMemberValueEditor();
			WarpState.AddToDictionary("0", "0");
			WarpState.AddToDictionary("1", "1");
			WarpState.AddToDictionary("2", "2");
			WarpState.AddToDictionary("3", "3");
			WarpState.AddToDictionary("4", "4");
			WarpState.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_DefaultList;

			PathEditor = new ExpressionMemberValueEditor_PathEditor();

			HullID = new ExpressionMemberValueEditor_HullID();
			HullID.PrepareContextMenuStripMethod = ExpressionMemberValueEditor.PrepareContextMenuStrip_HullIDList;
            VesselData.VesselDataChanged += new Action(() => { HullID.InvalidateContextMenuStrip(); });
	
			RaceKeys = new ExpressionMemberValueEditor_RaceKeys();
            VesselData.VesselDataChanged += new Action(() => { RaceKeys.InvalidateContextMenuStrip(); });

			HullKeys = new ExpressionMemberValueEditor_HullKeys();
            VesselData.VesselDataChanged += new Action(() => { HullKeys.InvalidateContextMenuStrip(); });
		}
    }
}
