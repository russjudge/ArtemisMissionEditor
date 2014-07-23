using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
	
namespace ArtemisMissionEditor
{
    using EMVB = ExpressionMemberValueBehaviorInXml;
    using EMVT = ExpressionMemberValueType;
    using EMVE = ExpressionMemberValueEditor;
    
    /// <summary>
    /// Defines everything about the value of the expression member
    /// </summary>
    /// <remarks>
    /// This class defines everything about an expression member's value:
    /// - what data type is it?
    /// - is this value saved into mission XML file or not
    /// - how is this value saved (always or only when filled)
    /// - what editor is used for the value (like, damcon count, consoles list, generic integer input?)
    /// 
    /// We need to create another ExpressionMemberValueDescription if we need to add another different kind of value 
    /// (like, "capitainType" which has specific preset text values to choose from), or we need to have a value 
    /// that works a bit differently than any other currently used value 
    /// (for example, have a colon after it when put in the expresion, or offer dropdown menu of some kind).
    ///
    /// Otherwise, we can reuse an existing description.
    /// 
    /// All the different ExpressionMemberValueDescriptions are added into the static list in this class, and then they are got via GetItem
    /// </remarks>
    public class ExpressionMemberValueDescription
    {
        private static Dictionary<string,ExpressionMemberValueDescription> Items;

		public static ExpressionMemberValueDescription GetItem(string name)
		{
			if (Items.ContainsKey(name))
				return Items[name];
			else
				throw new NotImplementedException("FAIL! Trying to get a nonexistant ExpressionMemberValueDescription "+name);
		}

        /// <summary>
        /// Initialise the list of member descriptions
        /// </summary>
		static ExpressionMemberValueDescription()
        {
            Items = new Dictionary<string, ExpressionMemberValueDescription>();

            // Caption or Label (used often when plain text is displayed)
            Items.Add("<label>",		    new ExpressionMemberValueDescription(EMVT.Nothing,		EMVB.NotStored,			EMVE.Label,null,null,"",""));
            
            // Blank aka NOTHING (often used by hidden checks)
            Items.Add("<blank>",		    new ExpressionMemberValueDescription(EMVT.Nothing,		EMVB.NotStored,			EMVE.Nothing,null,null,"",""));
            
            // Commentary expression for editing comments
            Items.Add("commentary",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultString,null,null,"","",""));

            // Root check member for chosing the expression
            Items.Add("check_XmlNameA",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.XmlNameActionCheck));
            
            // Root check member for chosing the expression
            Items.Add("check_XmlNameC",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.XmlNameConditionCheck));
            
            // Unknown expression for editing something not known to the editor
            Items.Add("unknown",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultString));

            // Check point / gm position from [create, destroy, ...]
            Items.Add("check_point/gmpos",  new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            //  xyz coordinate from [create, destroy, ...]
            Items.Add("x",                  new ExpressionMemberValueDescription(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, 0.0, 100000.0, "(", ", "));
            Items.Add("y",				    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, -100000.0, 100000.0,"",", "));
            Items.Add("z",				    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, 0.0, 100000.0,"",") "));
            //  xyz coordinate unbbound from [create, destroy, ...]
            Items.Add("xu",				    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, null,null,"(",", "));
            Items.Add("yu",				    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, null,null,"",", "));
            Items.Add("zu",				    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, null,null,"",") "));

            // Check line/circle from [create]
            Items.Add("check_line/circle",  new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // radius from [create]
            Items.Add("radius",			    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 100000));
            // radius (hidden) from [create]
            Items.Add("radiusH",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.Nothing));

            // Check hullID / hullraceKeys from [create]
            Items.Add("check_ID/keys",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // hullID from [create]
            Items.Add("hullID",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.HullID,null,null,"\"","\" ",""));
            // raceKeys from [create]
            Items.Add("raceKeys",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.RaceKeys,null,null,"\"","\" ",""));
            // hullKeys from [create]
            Items.Add("hullKeys",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.HullKeys,null,null,"\"","\" ",""));

            // Check "has fake shields" from [create]
            Items.Add("check_fakeShields",  new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // fakeShields from [create]
            Items.Add("fakeShieldsFR",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, -1, 1000,"(",") "));
            Items.Add("fakeShieldsF",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, -1, 1000,"(",", "));
            Items.Add("fakeShieldsR",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, -1, 1000,"",") "));
            // hasFakeShldFreq from [create]
            Items.Add("hasFakeShldFreq",    new ExpressionMemberValueDescription(EMVT.VarBool,		EMVB.StoredWhenFilled,	EMVE.DefaultBool, 
                                            "no fake frequency", "fake frequency"));

            // use_gm_position/selection	from [create, destroy, ...]
            Items.Add("use_gm",             new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredAsIs,		EMVE.Nothing));
            // type from [create] 
            Items.Add("type",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.CreateType));
            // name from [create, destroy, ...]
            Items.Add("name",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		
			Items.Add("name_with_comma",    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\", "));
            // angle from [create, ...]
            Items.Add("angle",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble, 0.0, 360.0));
            // fleetnumber from [create, ...]
            Items.Add("fleetnumber",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, -1, 99));
            // podnumber from [create]
            Items.Add("podnumber",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 9));
            // meshFileName from [create]
            Items.Add("meshFileName",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PathEditor,
                                            "DeleD Mesh Files|*.dxs|All Files|*.*","Select Delgine mesh file","\"","\" "));
            // textureFileName from [create]
            Items.Add("textureFileName",    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PathEditor,
                                            "PNG Files|*.png|All Files|*.*", "Select texture file", "\"", "\" "));
            // hullRace from [create]
            Items.Add("hullRace",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString, null, null, "\"", "\" "));
            // hullType from [create]
            Items.Add("hullType",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString, null, null, "\"", "\" "));
            // Color R/G/B					from [create]
            Items.Add("colorR",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"(",", "));
            Items.Add("colorG",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"",", "));
            Items.Add("colorB",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"",") "));

            // count from [create]
            Items.Add("count",			    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, null,""," ","0"));
            // startAngle/endAngle from [create]
            Items.Add("angle_unbound",      new ExpressionMemberValueDescription(EMVT.VarDouble,    EMVB.StoredAsIs,        EMVE.DefaultDouble));
            // randomRange from [create]
            Items.Add("randomRange",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 100000));
            // randomSeed from [create]
            Items.Add("randomSeed",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0));

            // Check named / nameless from [destroy/destroy_near]
            Items.Add("check_named/nameless",new ExpressionMemberValueDescription(EMVT.VarString,   EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // type (hidden) from [destroy] 
            Items.Add("typeH",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.Nothing));

            // Check name / gm selection from [destroy, ...]
            Items.Add("check_name/gmsel",   new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));

            // Check point / name /gm pos from [destroy_near]
            Items.Add("check_p/n/gmpos",    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));

            // text from [log, ...]
            Items.Add("text",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));

            // Check exact/rndInt/rndFloat from [set_variable]
            Items.Add("check_setvar",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.SetVariableCheck));
            // value from [set_variable,if_variable]
            Items.Add("value",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0));
            // randInt Low/High from [set_variable]
            Items.Add("randInt",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0));
            // randFloat Low/High from [set_variable]
            Items.Add("randFloat",		    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0));

            // type from [add_ai] 
            Items.Add("type_ai",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.AIType,null,null,"",""));
            // Check distance in/out nebula from [add_ai] 
            Items.Add("check_dneb",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DistanceNebulaCheck,null,null,""," "));
            // value radius from [add_ai] 
            Items.Add("value_r",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,100000,""," "));
            // value radius (/w trailing) from [add_ai] 
            Items.Add("value_r|q",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,100000,"",""));			
            // value throttle from [add_ai]
            Items.Add("value_t",		    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,null,"","","0.0"));

            // Check targetName / point from [direct] 
            Items.Add("check_p/n",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted,null,null,""," "));
            // Check convert direct	from [direct] 
            Items.Add("check_convertd",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.ConvertDirectCheck,null,null,""," "));

            // seconds from [set_timer]
            Items.Add("seconds",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,null,""," ","0"));

            // fileName from [incoming_message]
            Items.Add("msgFileName",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PathEditor,
                                            "OGG Audio Files|*.ogg|All Files|*.*", "Select message audio file","\"","\" "));
            // fileName from [play_sound_now]
            Items.Add("soundFileName",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PathEditor,
                                            "WAVE Audio Files|*.wav|All Files|*.*", "Select sound file","\"","\" "));
            // from from [incoming_message]
            Items.Add("msg_from",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));
            // mediaType from [incoming_message]
            Items.Add("msgMediaType",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" ","0"));

            // consoles from [warning_popup_message, start/end_getting_keypresses] 
            Items.Add("consoles",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.ConsoleList,null,null,"(",") "));

            // <body> from [incoming_comms_text]
            Items.Add("<body>",			    new ExpressionMemberValueDescription(EMVT.Body,			EMVB.NotStored,			EMVE.DefaultBody,null,null,"","",""));
            // [title] from [big_message]
            Items.Add("title",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));
            // [subtitle] from [big_message]
            Items.Add("subtitle",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredAsIs,		EMVE.DefaultString,null,null,"\"","\" "));

            // [property] from [set_object_property] 
            Items.Add("property",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PropertyObject,null,null,"\"","\" ","angle"));
            // values from [set_object_property]
            Items.Add("int-+inf",	        new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,null,null,""," "));
            Items.Add("int0...+inf",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,null,""," "));
            Items.Add("int0...100k",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,100000,""," "));
            Items.Add("int0...100",	        new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,0,100,""," "));
            Items.Add("int0...4",	        new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.WarpState,0,4,""," "));
            Items.Add("int-100k...100k",    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger,-100000,100000,""," "));
            Items.Add("flt-+inf",	        new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,null,null,""," "));
            Items.Add("flt0...+inf",	    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,null,""," "));
            Items.Add("flt0...100k",	    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,100000.0,""," "));
            Items.Add("flt0...100",	        new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,0.0,100.0,""," "));
            Items.Add("flt-100k...100k",    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble,-100000.0,100000.0,""," "));
            Items.Add("boolyesno",	        new ExpressionMemberValueDescription(EMVT.VarBool,  	EMVB.StoredWhenFilled,	EMVE.DefaultBool,"no","yes",""," "));
            // value for eliteAIType  		
            Items.Add("eliteaitype",        new ExpressionMemberValueDescription(EMVT.VarInteger,  	EMVB.StoredWhenFilled,	EMVE.EliteAIType,0,2,"\"","\" "));
            // value for eliteAbilityBits	
            Items.Add("eliteabilitybits",   new ExpressionMemberValueDescription(EMVT.VarInteger, 	EMVB.StoredWhenFilled,	EMVE.EliteAbilityBits,0,null,""," "));

            // value from [set_object_property,...] 
            Items.Add("value_f",		    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DefaultDouble));

            // property from [set_fleet_property] 
            Items.Add("property_f", new ExpressionMemberValueDescription(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.PropertyFleet, null, null, "\"", "\" ", "fleetSpacing"));

            // fleetIndex from [set_fleet_property]
            Items.Add("fleetIndex",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 99,""," ","0"));

            // index from [set_skybox_index]
            Items.Add("index",			    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.SkyboxIndex, 0, null,""," ","0"));
            // value from [set_difficulty_level]
            Items.Add("difficulty",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.Difficulty, 1, 11,""," ","1"));

            // value from [set_player_grid_damage]
            Items.Add("damage",			    new ExpressionMemberValueDescription(EMVT.VarDouble,	EMVB.StoredWhenFilled,	EMVE.DamageValue,0.0,1.0,""," ","0.0"));
            // index from [set_player_grid_damage]
            Items.Add("nodeindex",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 100,""," ","0"));

            // systemType from [set_player_grid_damage]
            Items.Add("systemType",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.ShipSystem,null,null,"\"","\" ","systemBeam"));
            // countFrom from [set_player_grid_damage]
            Items.Add("countFrom",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.CountFrom,null,null,""," ","front"));

            // index from [set_damcon_members]
            Items.Add("teamindex",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.TeamIndex, 0, 2,""," ","0"));
            // value from [set_damcon_members]
            Items.Add("dcamount",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.TeamAmount, 0, 6,""," ","0"));

            // comparator from [if_variable]
            Items.Add("comparator",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.Comparator,null,null,""," ","EQUALS"));
            // value from [if_damcon_members]
            Items.Add("dcamountf",		    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.TeamAmountF, null, null,""," ","0"));

            // Check allships/fleetnumber from [if_fleet_count]
            Items.Add("check_all/fn",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // fleetnumber from [if_fleet_number]
            Items.Add("fleetnumber_if",	    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 99,""," "));
            // Check allships/fleetnumber from [if_gm_key]
            Items.Add("check_letter/id",    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // value from [if_gm_key]
            Items.Add("keyid",			    new ExpressionMemberValueDescription(EMVT.VarInteger,	EMVB.StoredWhenFilled,	EMVE.DefaultInteger, 0, 128,""," "));
            // keyText from [if_gm_key]
            Items.Add("letter",			    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));

            // Check if_(not)_exists from [if_exists,if_not_exists]
            Items.Add("check_existance",    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));

            // Check inside/outside from [if_inside/outside_box/sphere]
            Items.Add("check_in/out",       new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));
            // Check box/sphere from [if_inside/outside_box/sphere]
            Items.Add("check_box/sph",      new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.NotStored,			EMVE.DefaultCheckUnsorted));

            // name from [if_timer]
            Items.Add("name_timer",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.TimerName,null,null,"\"","\" "));
            // name from [if_variable]
            Items.Add("name_var",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.VariableName,null,null,"\"","\" "));
            // name from many places
            Items.Add("name_all",		    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.NamedAllName,null,null,"\"","\" "));
            Items.Add("name_all_with_colon",new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.NamedAllName,null,null,"\"","\": "));
            // name from [if_docked]
            Items.Add("name_station",	    new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.NamedStationName,null,null,"\"","\" "));
            // name from [create]
            Items.Add("name_createplayer",	new ExpressionMemberValueDescription(EMVT.VarString,	EMVB.StoredWhenFilled,	EMVE.PlayerNames,null,null,"\"","\" "));		

            // from set_special
            Items.Add("shipState",          new ExpressionMemberValueDescription(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.SpecialShipType, -1, 3,"\"","\" "));
            Items.Add("captainState",       new ExpressionMemberValueDescription(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.SpecialCapitainType, -1, 5,"\"","\" "));
            // sideValue from set_side_value
            Items.Add("sideValue",          new ExpressionMemberValueDescription(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.Side, 0, 31));
		}

        /// <summary> What is added to the left of the member value</summary>
        private string QuoteLeft;
        
        /// <summary> What is added to the right of the member value</summary>
		private string QuoteRight;

		/// <summary> Type of this member's value (like int, string, double...) </summary>
        public ExpressionMemberValueType Type;

		/// <summary> Behavior of this value when stored in XML </summary>
		public ExpressionMemberValueBehaviorInXml BehaviorInXml;
		
		/// <summary>
        /// Editor used for this value (usually defines what values to offer for picking, like, available hullid, property, variable, timer,...)
        /// </summary>
        public ExpressionMemberValueEditor Editor;
		
        /// <summary>
        /// Minimal inclusive boundary, null if value has no minimal boundary, used as displayed false value for bool type
        /// </summary>
        public object Min;

        /// <summary>
		/// Maximal inclusive boundary, null if value has no maximal boundary, used as displayed true value for bool type
        /// </summary>
        public object Max;

		/// <summary>
		/// Default value, to be substituted in case the value of the attribute is null
		/// </summary>
		public string DefaultIfNull;

		/// <summary>
		/// This value is displayed in GUI
		/// </summary>
		public bool IsDisplayed { get { return Editor.IsDisplayed; } }

		/// <summary>
		/// This value is interactive (can be focused and clicked and does something)
		/// </summary>
		/// <returns></returns>
		public bool IsInteractive { get { return Editor.IsInteractive; } }

		/// <summary>
		/// This value is serialized (saved to xml)
		/// </summary>
		/// <returns></returns>
		public bool IsSerialized { get { return BehaviorInXml != EMVB.NotStored; } }

		/// <summary>
		/// Convert display value to xml value
		/// </summary>
		public string ValueToXml(string value) { return Editor.ValueToXml(value, Type, Min, Max); }

		/// <summary>
		/// Convert xml value to display value
		/// </summary>
		public string ValueToDisplay(string value) { return Editor.ValueToDisplay(value, Type, Min, Max); }

		/// <summary>
		/// Takes value from XML (GetValue), returns value fit for displaying on-screen
		/// </summary>
		public string GetValueDisplay(ExpressionMemberContainer container)
		{
			if (Editor == EMVE.Nothing)
				return null;

			string result = Editor == EMVE.Label ? container.Member.Text : container.GetValue();

			result = ValueToDisplay(result);

			//TODO: Maybe some values need to be displayed differently when null or empty?
			if (String.IsNullOrEmpty(result))
				result = container.Member.Text;

			result = QuoteLeft + result + QuoteRight;

			return result;
		}

		private bool OnClick_private_RecursivelyActivate(ToolStripItem item, ref bool next, bool forward)
		{
			if (!(item is ToolStripMenuItem))
				return false;

			if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
			{
				for (int i = 0; i <((ToolStripMenuItem)item).DropDownItems.Count;i++)

					if (OnClick_private_RecursivelyActivate(((ToolStripMenuItem)item).DropDownItems[forward ? i 
                        : ((ToolStripMenuItem)item).DropDownItems.Count-1-i], ref next, forward))
						return true;
			}
			else
			{
				if (next)
				{
					item.PerformClick();
					return true;
				}
				next = item.Selected || next;
			}
			return false;

		}

		/// <summary>
		/// Expression member that's using our descriptor was clicked
		/// </summary>
		/// <param name="container"></param>
		public void OnClick(ExpressionMemberContainer container, NormalLabel l, Point curPos, EditorActivationMode mode)
		{
			if (   mode == EditorActivationMode.Plus01
				|| mode == EditorActivationMode.Plus
				|| mode == EditorActivationMode.Plus10
				|| mode == EditorActivationMode.Plus100
				|| mode == EditorActivationMode.Plus1000
				|| mode == EditorActivationMode.Minus01
				|| mode == EditorActivationMode.Minus
				|| mode == EditorActivationMode.Minus10
				|| mode == EditorActivationMode.Minus100
				|| mode == EditorActivationMode.Minus1000)
			{
				if ((container.Member as ExpressionMemberCheck == null)&&(Type == EMVT.VarDouble || Type == EMVT.VarInteger || Type == EMVT.VarBool))
				{
					bool changed = false;
					double delta = 0.0;
					delta = mode == EditorActivationMode.Plus01		? 0.1	: delta;
					delta = mode == EditorActivationMode.Plus		? 1		: delta;
					delta = mode == EditorActivationMode.Plus10		? 10	: delta;
					delta = mode == EditorActivationMode.Plus100	? 100	: delta;
					delta = mode == EditorActivationMode.Plus1000	? 1000	: delta;
					delta = mode == EditorActivationMode.Minus01	? -0.1	: delta;
					delta = mode == EditorActivationMode.Minus		? -1	: delta;
					delta = mode == EditorActivationMode.Minus10	? -10	: delta;
					delta = mode == EditorActivationMode.Minus100	? -100	: delta;
					delta = mode == EditorActivationMode.Minus1000	? -1000	: delta;

					switch (Type)
					{
						case EMVT.VarBool:
							int tmpBool;
							if (Helper.IntTryParse(container.GetValue(), out tmpBool))
							{
								if (tmpBool == 0)
									tmpBool = 1;
								else
									tmpBool = 0;
								container.SetValue(tmpBool.ToString());
								changed = true;
							}
							break;
						case EMVT.VarDouble:
							double tmpDouble;
							if (Helper.DoubleTryParse(container.GetValue(), out tmpDouble))
							{
								tmpDouble+=delta;
								if (Min != null && tmpDouble < (double)Min)
										tmpDouble = Max != null ? (double)Max : (double)Min;
								if (Max != null && tmpDouble > (double)Max)
									tmpDouble = Min != null ? (double)Min : (double)Max;
								container.SetValue(Helper.DoubleToString(tmpDouble));
								changed = true;
							}
							break;
						case EMVT.VarInteger:
							int tmpInt;
							if (Helper.IntTryParse(container.GetValue(), out tmpInt))
							{
								tmpInt += (int)Math.Round(delta);
								if (Min != null && tmpInt < (int)Min)
										tmpInt = Max != null ? (int)Max : (int)Min;
								if (Max != null && tmpInt > (int)Max)
									tmpInt = Min != null ? (int)Min : (int)Max;
								container.SetValue(tmpInt.ToString());
								changed = true;
							}
							break;
					}

					if (changed)
					{
						Mission.Current.UpdateStatementTree();
						Mission.Current.RegisterChange("Expression member value changed");
					}
					return;
				}
				else
				{
					return;
				}
			}
			
			ContextMenuStrip curCMS;
			if ((curCMS = Editor.PrepareContextMenuStrip(container, mode)) != null)
			{
				if (mode == EditorActivationMode.NextMenuItem)
				{
					if ((bool)curCMS.Tag)
					{
						ShowEditingGUI(container);
						return;
					}

					bool next = false;
					foreach (ToolStripItem item in curCMS.Items)
						if (OnClick_private_RecursivelyActivate(item, ref next, true))
						{
							curCMS.Close();
							return;
						}
					if (curCMS.Items[0] is ToolStripMenuItem && ((ToolStripMenuItem)curCMS.Items[0]).DropDownItems.Count > 0)
						((ToolStripMenuItem)curCMS.Items[0]).DropDownItems[0].PerformClick();
					else
						curCMS.Items[0].PerformClick();
					curCMS.Close();
					return;
				}
				if (mode == EditorActivationMode.PreviousMenuItem)
				{
					if ((bool)curCMS.Tag)
					{
						ShowEditingGUI(container);
						return;
					}

					bool next = false;
					for(int i=curCMS.Items.Count-1;i>=0;i--)
						if (OnClick_private_RecursivelyActivate(curCMS.Items[i], ref next, false))
						{
							curCMS.Close();
							return;
						}
					if (curCMS.Items[curCMS.Items.Count - 1] is ToolStripMenuItem && ((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).DropDownItems.Count > 0)
						((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).
                            DropDownItems[((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).
                            DropDownItems.Count - 1].
                            PerformClick();
					else
						curCMS.Items[curCMS.Items.Count - 1].PerformClick();
					curCMS.Close();
					return;
				}
				curCMS.Show(curPos);
			}
			else
				ShowEditingGUI(container);
		}

		public void ShowEditingGUI(ExpressionMemberContainer container)
		{
			Editor.ShowEditingGUI(container, this, DefaultIfNull);
		}
		
		public ExpressionMemberValueDescription(EMVT type, EMVB behavior, EMVE editor, object min = null, object max = null, 
                                                string quoteLeft = "", string quoteRight = " ", string defaultIfNull = null)
		{
			Type			= type;
			BehaviorInXml	= behavior;
			Editor			= editor;
			Min				= min;
			Max				= max;
			QuoteLeft		= quoteLeft;
			QuoteRight		= quoteRight;
			DefaultIfNull	= defaultIfNull;
		}
    }

}
