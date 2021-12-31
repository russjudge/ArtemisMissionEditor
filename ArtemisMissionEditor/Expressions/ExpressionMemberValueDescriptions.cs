using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor.Expressions
{
    // Aliases defined in order to make declarations below fit on the screen better
    using EMVB = ExpressionMemberValueBehaviorInXml;
    using EMVT = ExpressionMemberValueType;
    using EMVE = ExpressionMemberValueEditors;
    using EMVD = ExpressionMemberValueDescription;

    /// <summary>
    /// Contains all the instances of ExpressionMemberValueDescription used by the mission editor. 
    /// </summary>
    public static class ExpressionMemberValueDescriptions
    {
        /// <summary> Caption or Label (used often when plain text is displayed) </summary>
        public static EMVD Label =                      new EMVD(EMVT.Nothing,      EMVB.NotStored,         EMVE.Label,null,null,"","");
            
        /// <summary> Blank aka NOTHING (often used by hidden checks) </summary>
        public static EMVD Blank =                      new EMVD(EMVT.Nothing,      EMVB.NotStored,         EMVE.Nothing,null,null,"","");
            
        /// <summary> Commentary expression for editing comments </summary>
        public static EMVD Commentary =                 new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultString,null,null,"","","");

        /// <summary> Root check member for expression evaluation of an action </summary>
        public static EMVD Check_XmlNameA =             new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.XmlNameActionCheck);

        /// <summary> Root check member for expression evaluation of a condition </summary>
        public static EMVD Check_XmlNameC =             new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.XmlNameConditionCheck);
            
        /// <summary> Unknown expression for editing something not known to the editor </summary>
        public static EMVD Unknown =                    new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultString);

        /// <summary> Check point / gm position from [create, destroy, ...] </summary>
        public static EMVD Check_Point_GMPos =          new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);

        /// <summary> "x" coordinate from [create, destroy, ...] </summary>
        public static EMVD X =                          new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, 0.0, 100000.0, "(", ", ");
        /// <summary> "y" coordinate from [create, destroy, ...] </summary>
        public static EMVD Y =                          new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, -100000.0, 100000.0, "", ", ");
        /// <summary> "z" coordinate from [create, destroy, ...] </summary>
        public static EMVD Z =                          new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, 0.0, 100000.0, "", ") ");

        /// <summary> "x" coordinate unbbound from [create, destroy, ...] </summary>
        public static EMVD XUnbound =                   new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, null,null,"(",", ");
        /// <summary> "y" coordinate unbbound from [create, destroy, ...] </summary>
        public static EMVD YUnbound =                   new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, null, null, "", ", ");
        /// <summary> "z" coordinate unbbound from [create, destroy, ...] </summary>
        public static EMVD ZUnbound =                   new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, null, null, "", ") ");

        /// <summary> Check line/circle from [create] </summary>
        public static EMVD Check_Line_Circle =          new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "radius" from [create] </summary>
        public static EMVD Radius =                     new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 100000);
        /// <summary> "radius" (hidden) from [create] </summary>
        public static EMVD RadiusHidden =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.Nothing);

        /// <summary> Check hullID / hullraceKeys from [create] </summary>
        public static EMVD Check_ID_Keys =              new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "hullID" from [create] </summary>
        public static EMVD HullID =                     new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.HullID,null,null,"\"","\" ","");
        /// <summary> "raceKeys" from [create] </summary>
        public static EMVD RaceKeys =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.RaceKeys,null,null,"\"","\" ","");
        /// <summary> "hullKeys" from [create] </summary>
        public static EMVD HullKeys =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.HullKeys,null,null,"\"","\" ","");

        /// <summary> Check "has fake shields" from [create] </summary>
        public static EMVD Check_FakeShields =          new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "fakeShieldsFront/Rear" from [create] </summary>
        public static EMVD FakeShieldsFR =              new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, -1, 1000,"(",") ");
        /// <summary> "fakeShieldsFront" from [create] </summary>    
        public static EMVD FakeShieldsF =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, -1, 1000,"(",", ");
        /// <summary> "fakeShieldsRear" from [create] </summary>    
        public static EMVD FakeShieldsR =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, -1, 1000,"",") ");
        /// <summary> "beaconEffect" from [create] </summary>
        public static EMVD BeaconEffect =               new EMVD(EMVT.VarBool,      EMVB.StoredWhenFilled,  EMVE.DefaultBool,  "Attract", "Repel");
        /// <summary> "hasFakeShldFreq" from [create] </summary>
        public static EMVD HasFakeShldFrequency =       new EMVD(EMVT.VarBool,      EMVB.StoredWhenFilled,  EMVE.DefaultBool, 
                                                        "no fake frequency", "fake frequency");

        /// <summary> "use_gm_position/selection"    from [create, destroy, ...] </summary>
        public static EMVD UseGM =                      new EMVD(EMVT.VarString,    EMVB.StoredAsIs,        EMVE.Nothing);
        /// <summary> "type" from [create]  </summary>
        public static EMVD UseSlot =                    new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 7);
        /// <summary> "type" from [create]  </summary>
        public static EMVD Type =                       new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.CreateType);
        /// <summary> "name" from [create, destroy, ...] </summary>
        public static EMVD NameWithPeriod =             new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString, null, null, "\"", "\". ");
        /// <summary> "name" from [create, destroy, ...] </summary>
        public static EMVD Name =                       new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString, null, null, "\"", "\" ");
        /// <summary> "name" from [create, destroy, ...] </summary>    
        public static EMVD NameWithComma =              new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString, null, null, "\"", "\", ");
        /// <summary> "angle" from [create, ...] </summary>
        public static EMVD Angle =                      new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 360);
        /// <summary> Deprecated "angle" from [create, ...], only null is legal </summary>
        public static EMVD AngleDeprecated =            new EMVD(EMVT.VarEnumString,EMVB.StoredWhenFilled,  EMVE.DefaultString);
        /// <summary> "fleetnumber" from [create, ...] </summary>
        public static EMVD FleetNumberOrNone =          new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, -1, 99);
        /// <summary> "podnumber" from [create] </summary>
        public static EMVD PodNumber =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 9);
        public static EMVD accent_color =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 99);
        /// <summary> "warp/jump" from [create] </summary>
        public static EMVD Check_DriveType =            new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DriveType, null, null, "", " ");
        public static EMVD DriveType =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.Nothing);
        public static EMVD player_slot =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 7);
        public static EMVD bay_slot =                   new EMVD(EMVT.VarInteger,   EMVB.StoredAsIs,        EMVE.DefaultInteger, 0, 31);
        public static EMVD TagSlot =                    new EMVD(EMVT.VarInteger,   EMVB.StoredAsIs,        EMVE.DefaultInteger, 0, 3);
        /// <summary> "MonsterType" from [create] </summary>
        public static EMVD MonsterType =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.MonsterType, 0, 8, "", "");
        public static EMVD SensorRange =                new EMVD(EMVT.VarInteger,   EMVB.StoredAsIs,        EMVE.SensorRange, 0, 5, "", "");
        /// <summary> "pickupType" from [create] </summary>
        public static EMVD AnomalyType =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.AnomalyType, 0, 9, "", "");
        /// <summary> "meshFileName" from [create] </summary>
        public static EMVD MeshFileName =               new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PathEditor,
                                                        "DeleD Mesh Files|*.dxs|All Files|*.*;Select Delgine mesh file",
                                                        Forms.PathRelativityMode.RelativeToArtemisFolder, "\"", "\" ");
        /// <summary> "textureFileName" from [create] </summary>
        public static EMVD TextureFileName =            new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PathEditor,
                                                        "PNG Files|*.png|All Files|*.*;Select texture file",
                                                        Forms.PathRelativityMode.RelativeToArtemisFolder, "\"", "\" ");
        /// <summary> "hullRace" from [create] </summary>
        public static EMVD HullRace =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString, null, null, "\"", "\" ");
        /// <summary> "hullType" from [create] </summary>
        public static EMVD HullType =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString, null, null, "\"", "\" ");
        /// <summary> "ColorR" from [create] </summary>
        public static EMVD ColorR =                     new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0,1.0,"(",", ");
        /// <summary> "ColorG" from [create] </summary>
        public static EMVD ColorG =                     new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, 0.0, 1.0, "", ", ");
        /// <summary> "ColorB" from [create] </summary>
        public static EMVD ColorB =                     new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, 0.0, 1.0, "", ") ");

        /// <summary> "count" from [create] </summary>
        public static EMVD Count =                      new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, null,""," ","0");
        /// <summary> "startAngle/endAngle" from [create] </summary>
        public static EMVD AngleUnbound =               new EMVD(EMVT.VarDouble,    EMVB.StoredAsIs,        EMVE.DefaultDouble);
        /// <summary> "randomRange" from [create] </summary>
        public static EMVD RandomRange =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 100000);
        /// <summary> "randomSeed" from [create] </summary>
        public static EMVD RandomSeed =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0);

        /// <summary> Check named / nameless from [destroy/destroy_near] </summary>
        public static EMVD Check_Named_Nameless =       new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "type" (hidden) from [destroy] </summary>
        public static EMVD TypeH =                      new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.Nothing);

        /// <summary> Check name / gm selection from [destroy, ...] </summary>
        public static EMVD Check_Name_GMSelection =     new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);

        /// <summary> Check point / name /gm pos from [destroy_near] </summary>
        public static EMVD Check_Point_Name_GM =        new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);

        /// <summary> "text" from [log, ...] </summary>
        public static EMVD Text =                       new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ");

        public static EMVD GMX = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0);

        public static EMVD GMY = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0);

        public static EMVD GMH = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0);

        public static EMVD GMW = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0);

        /// <summary> Check exact/rndInt/rndFloat from [set_variable] </summary>
        public static EMVD Check_SetVariable =          new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.SetVariableCheck);
        /// <summary> "value" from [set_variable,if_variable] </summary>
        public static EMVD Value =                      new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0);
        /// <summary> "integer" from [set_variable] </summary>
        public static EMVD VariableType =               new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.VariableType);

        /// <summary> "type" from [add_ai] </summary>
        public static EMVD TypeAI =                     new EMVD(EMVT.VarEnumString,EMVB.StoredWhenFilled,  EMVE.AIType,null,null,"","");
        /// <summary> Check distance in/out nebula from [add_ai] </summary>
        public static EMVD Check_DistanceNebula =       new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DistanceNebulaCheck,null,null,""," ");
        /// <summary> Check distance from [add_ai] </summary>
        public static EMVD Check_DistanceNoNebula =     new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DistanceNoNebulaCheck,null,null,""," ");
        /// <summary> "value" radius from [add_ai] </summary>
        public static EMVD ValueRadius =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,100000,""," ");
        /// <summary> "value" radius (without trailing space) from [add_ai] </summary>
        public static EMVD ValueRadiusQ =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,100000,"","");
        /// <summary> "value" throttle from [add_ai] </summary>
        public static EMVD Throttle =                   new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0,null,"","","0.0");
        
        /// <summary> Check targetName / point from [direct]  </summary>
        public static EMVD Check_Point_TargetName =     new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck,null,null,""," ");
        /// <summary> Check convert angle from [create] </summary>
        public static EMVD Check_ConvertAngle =         new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.ConvertAngleCheck);
        /// <summary> Check convert direct    from [direct]  </summary>
        public static EMVD Check_ConvertDirect =        new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.ConvertDirectCheck,null,null,""," ");
        /// <summary> Check convert specialAbilityBits from [set_object_property] </summary>
        public static EMVD Check_ConvertSpecialAbilityBits = new EMVD(EMVT.VarString, EMVB.NotStored,       EMVE.ConvertSpecialAbilityBitsCheck,null,null,""," ");

        /// <summary> "seconds" from [set_timer] </summary>
        public static EMVD Seconds =                    new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,null,""," ","0");

        /// <summary> "fileName" from [incoming_message] </summary>
        public static EMVD MessageFileName =            new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PathEditor,
                                                        "OGG Audio Files|*.ogg|All Files|*.*;Select message audio file",
                                                        Forms.PathRelativityMode.RelativeToMissionFolder, "\"", "\" ");
        /// <summary> "fileName" from [play_sound_now] </summary>
        public static EMVD SoundFileName =              new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PathEditor,
                                                        "WAVE Audio Files|*.wav|All Files|*.*;Select sound file",
                                                        Forms.PathRelativityMode.RelativeToArtemisFolder, "\"", "\" ");
        /// <summary> "from" from [incoming_message] </summary>
        public static EMVD MessageFrom =                new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ");
        /// <summary> "mediaType" from [incoming_message] </summary>
        public static EMVD MessageMediaType =           new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ","0");

        /// <summary> "consoles" from [warning_popup_message, start/end_getting_keypresses] </summary>
        public static EMVD Consoles =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.ConsoleList,null,null,"(",") ");

        /// <summary> [type] from [incoming_comms_text] 
        public static EMVD CommTypes =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.CommTypes,null,null,"\"","\" ");

        /// <summary> [body] from [incoming_comms_text] 
        public static EMVD Body =                       new EMVD(EMVT.Body,         EMVB.NotStored,         EMVE.DefaultBody,null,null,"","","");
        /// <summary> "title" from [big_message] </summary>
        public static EMVD Title =                      new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ");
        /// <summary> "subtitle" from [big_message] </summary>
        public static EMVD Subtitle =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ");

        /// <summary> "property" from [if_object_property] </summary>
        public static EMVD ReadableProperty =           new EMVD(EMVT.VarEnumString,EMVB.StoredWhenFilled,  EMVE.ReadableProperty,null,null,"\"","\" ","angle");
        /// <summary> "property" from [copy_object_property] </summary>
        public static EMVD WritableObjectProperty =     new EMVD(EMVT.VarEnumString,EMVB.StoredWhenFilled,  EMVE.WritableObjectProperty,null,null,"\"","\" ","angle");
        /// <summary> "property" from [set_object_property] </summary>
        public static EMVD WritableProperty =           new EMVD(EMVT.VarEnumString,EMVB.StoredWhenFilled,  EMVE.WritableProperty,null,null,"\"","\" ","angle");
        // values from [set_object_property]


  //      public static EMVD SensorRange = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0, 5, "", " ");
        
        public static EMVD Int_NegInf_PosInf =          new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,null,null,""," ");
        public static EMVD Int_0_PosInf =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,null,""," ");
        public static EMVD Int_40_300 =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,40,300,""," ");
        public static EMVD Int_0_100k =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,100000,""," ");
        public static EMVD Int_0_100 =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,0,100,""," ");
        public static EMVD Int_0_4 =                    new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.WarpState,0,4,""," ");
        public static EMVD Int_Minus100k_100k =         new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger,-100000,100000,""," ");
        public static EMVD Flt_NegInf_PosInf =          new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,null,null,""," ");
        public static EMVD Flt_0_PosInf =               new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0,null,""," ");
        public static EMVD Flt_0_100k =                 new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0,100000.0,""," ");
        public static EMVD tII1_3 =                     new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 2, "", " ");
        public static EMVD Flt_0_100 =                  new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,0.0,100.0,""," ");
        public static EMVD Flt_Minus100k_100k =         new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble,-100000.0,100000.0,""," ");
        public static EMVD Bool_Is_Isnt =               new EMVD(EMVT.VarBool,      EMVB.StoredWhenNonDefault, EMVE.DefaultPresent,"is","is not",""," ", "0");
        public static EMVD Bool_Do_Dont =               new EMVD(EMVT.VarBool,      EMVB.StoredWhenNonDefault, EMVE.DefaultPresent,"don't","do",""," ", "0");
        public static EMVD Bool_Yes_No =                new EMVD(EMVT.VarBool,      EMVB.StoredWhenFilled,  EMVE.DefaultBool,"no","yes",""," ");
        /// <summary> value for eliteAIType </summary>
        public static EMVD EliteAIType =                new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.EliteAIType,0,2,"\"","\" ");
        /// <summary> value for specialAbilityBits </summary>
        public static EMVD EliteAbilityBits =           new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.EliteAbilityBits,0,null,""," ");

        /// <summary> float value with no trailing space </summary>
        public static EMVD ValueFQ =                    new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DefaultDouble, null, null, "", "");

        /// <summary> "property" from [set_fleet_property]  </summary>
        public static EMVD PropertyF =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PropertyFleet, null, null, "\"", "\" ", "fleetSpacing");

        /// <summary> "fleetIndex" from [set_fleet_property] </summary>
        public static EMVD FleetIndex =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 99,""," ","0");
       // public static EMVD GMButtontext = new EMVD(EMVT.VarInteger, EMVB.StoredWhenFilled, EMVE.DefaultInteger, 0, 99, "", " ", "0");

       // eML.Add(new ExpressionMember("<>",, ExpressionMemberValueDescriptions.GMButtontext, "text"));

        /// <summary> "index" from [set_skybox_index] </summary>
        public static EMVD Index =                      new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.SkyboxIndex, 0, null,""," ","0");
        /// <summary> "value" from [set_difficulty_level] </summary>
        public static EMVD Difficulty =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.Difficulty, 1, 11,""," ","1");

        /// <summary> "value" from [set_player_grid_damage] </summary>
        public static EMVD Damage =                     new EMVD(EMVT.VarDouble,    EMVB.StoredWhenFilled,  EMVE.DamageValue,0.0,1.0,""," ","0.0");
        /// <summary> "index" from [set_player_grid_damage] </summary>
        public static EMVD NodeIndex =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 100,""," ","0");

        /// <summary> "systemType" from [set_player_grid_damage] </summary>
        public static EMVD SystemType =                 new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.ShipSystem,null,null,"\"","\" ","systemBeam");
        /// <summary> "countFrom" from [set_player_grid_damage] </summary>
        public static EMVD CountFrom =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.CountFrom,null,null,""," ","front");

        /// <summary> "index" from [set_damcon_members] </summary>
        public static EMVD Teamindex =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.TeamIndex, 0, 2,""," ","0");
        /// <summary> "value" from [set_damcon_members] </summary>
        public static EMVD DCAmount =                   new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.TeamAmount, 0, 6,""," ","0");

        /// <summary> "comparator" from [if_variable] </summary>
        public static EMVD Comparator =                 new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.Comparator,null,null,""," ","EQUALS");
        /// <summary> "value" from [if_damcon_members] </summary>
        public static EMVD DCAmountF =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.TeamAmount, null, null,""," ","0");

        /// <summary> Check allships/fleetnumber from [if_fleet_count] </summary>
        public static EMVD Check_AllShips_FleetNumber = new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "fleetnumber" from [if_fleet_number] </summary>
        public static EMVD FleetNumberIf =              new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 99,""," ");
        /// <summary> Check allships/fleetnumber from [if_gm_key] </summary>
        public static EMVD Check_Letter_KeyID =         new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);

        /// <summary> "nebType" from [create] </summary>
        public static EMVD NebulaType =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.NebulaType, 1, 3);
        /// <summary> "compare" from [if_in_nebula] </summary>
        public static EMVD NebulaTypeOrNone =           new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.NebulaTypeOrNone, 0, 3);

        /// <summary> "value" from [if_scan_level] </summary>
        public static EMVD ScanLevel =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 2, "","");
        /// <summary> Value of monster "age" property </summary>
        public static EMVD MonsterAge =                 new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.MonsterAge, 1, 3, ""," ");

        /// <summary> "value" from [if_gm_key] </summary>
        public static EMVD KeyID =                      new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.DefaultInteger, 0, 128,""," ");
        /// <summary> keyText from [if_gm_key] </summary>
        public static EMVD Letter =                     new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.DefaultString,null,null,"\"","\" ");
        public static EMVD GMTextIF = new EMVD(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.DefaultString, null, null, "\"", "\" ");

        /// <summary> Check if_(not)_exists from [if_exists,if_not_exists] </summary>
        public static EMVD Check_Existence =            new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);

        /// <summary> Check inside/outside from [if_inside/outside_box/sphere] </summary>
        public static EMVD Check_In_Out =               new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> Check box/sphere from [if_inside/outside_box/sphere] </summary>
        public static EMVD Check_Box_Sphere =           new EMVD(EMVT.VarString,    EMVB.NotStored,         EMVE.DefaultCheck);
        /// <summary> "text" from [if_gm_button,clear_gm_button] </summary>
        public static EMVD TextGMButton =               new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.GMButtonText, null, null, "\"", "\" ");

        /// <summary> "text" from [if_comms_button,clear_comms_button] </summary>
        public static EMVD TextCommsButton = new EMVD(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.CommsButtonText, null, null, "\"", "\" ");

        /// <summary> "name" from [if_timer] </summary>
        public static EMVD NameTimer =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.TimerName,null,null,"\"","\" ");
        /// <summary> "name" from [if_variable] </summary>
        public static EMVD NameVariable =               new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.VariableName,null,null,"\"","\" ");
       // public static EMVD GMText = new EMVD(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.VariableName, null, null, "\"", "\" ");
        /// <summary> "id" from [spawn_external_program] </summary>
        public static EMVD ExternalProgramID =          new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.ExternalProgramID,null,null,"\"","\" ");

        /// <summary> Object "name" from [create] </summary>
        public static EMVD NameAll =                    new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedAllName,null,null,"\"","\" ");
        /// <summary> Object "name" from [create] </summary>
        public static EMVD NameAllWithColon =           new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedAllName,null,null,"\"","\": ");
        /// <summary> Station "name" from [create] </summary>
        public static EMVD NameStation =                new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedStationName,null,null,"\"","\" ");
        /// <summary> Monster "name" from [create] </summary>
        public static EMVD NameMonster =                new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedMonsterName,null,null,"\"","\" ");
        /// <summary> Generic mesh "name" from [create] </summary>
        public static EMVD NameGenericMesh =            new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedGenericMeshName,null,null,"\"","\" ");
        /// <summary> Player "name" of player from [create] </summary>
        public static EMVD NamePlayer =                 new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.PlayerNames,null,null,"\"","\" ");
        /// <summary> Enemy ship "name" from [create] </summary>
        public static EMVD NameEnemy =                  new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedEnemyName,null,null,"\"","\" ");
        /// <summary> Enemy ship "name" from [create] </summary>
        public static EMVD NameNeutral =                new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedNeutralName,null,null,"\"","\" ");
        /// <summary> AI (neutral or enemy) ship "name" from [create] </summary>
        public static EMVD NameAIShip =                 new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedAIShipName,null,null,"\"","\" ");
        /// <summary> Ship (neutral, enemy, or player) "name" from [create] </summary>
        public static EMVD NameShip =                   new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedShipName,null,null,"\"","\" ");
        /// <summary> Ship (neutral, enemy, or player) "name" from [create] </summary>
        public static EMVD NameShipOrMonster =          new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedShipOrMonsterName,null,null,"\"","\" ");
        /// <summary> AI (neutral or enemy) ship or monster "name" from [create] </summary>
        public static EMVD NameAIShipOrMonster =        new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.NamedAIShipOrMonsterName,null,null,"\"","\" ");
        /// <summary> Scannable object "name" from [create] </summary>
        public static EMVD NameScannableObject =        new EMVD(EMVT.VarString,    EMVB.StoredWhenFilled,  EMVE.ScannableObjectName,null,null,"\"","\" ");

        /// <summary> "ship" from set_special </summary>
        public static EMVD ShipState =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.SpecialShipType, -1, 2,"\"","\" ");
        /// <summary> "captain" from set_special </summary>
        public static EMVD CaptainState =               new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.SpecialCaptainType, -1, 5, "\"", "\" ");
        public static EMVD SpecialState = new EMVD(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.SpecialSpecialState, null, null, "\"", "\" ");
        public static EMVD SpecialSwitchState = new EMVD(EMVT.VarString, EMVB.StoredWhenFilled, EMVE.SpecialSpecialSwitchState, null, null, "", " ");

        /// <summary> "sideValue" from set_side_value </summary>
        public static EMVD SideValue =                  new EMVD(EMVT.VarInteger,   EMVB.StoredWhenFilled,  EMVE.Side, 0, 31);
    }
}
