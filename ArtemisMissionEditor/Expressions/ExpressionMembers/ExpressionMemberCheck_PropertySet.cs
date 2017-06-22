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
    /// This check is for "type" in [set_property] statement, it is hidden since "add to property" statement has property name as a member.
    /// </summary>
    public sealed class ExpressionMemberCheck_PropertySet : ExpressionMemberCheck
	{
        /// <summary>
        /// This function is called when check needs to decide which list of ExpressionMembers to output. 
        /// After it is called, SetValue will be called, to allow for error correction. 
        /// </summary>
        /// <example>If input is wrong, decide will choose something, and then the input will be corrected in the SetValue function</example>
        public override string Decide(ExpressionMemberContainer container)
		{
			string type = container.GetAttribute("property", ExpressionMemberValueDescriptions.Property.DefaultIfNull);

			switch (type)
			{
                case "nonPlayerSpeed":          return "<ENMYSP>";
                case "nebulaIsOpaque":          return "<NEBULAROP>";
                case "sensorSetting":           return "<SENSOR>";
                case "nonPlayerShield":         return "<ENMYSP>";
                case "nonPlayerWeapon":         return "<ENMYSP>";
                case "playerWeapon":            return "<ENMYSP>";
                case "playerShields":           return "<ENMYSP>";
                case "coopAdjustmentValue":     return "<DEFAULT>";
                // case "gameTimeLimit":           return "<DEFAULT>";
                //EVERYTHING
                case "positionX":				return "<FLT0...100K>";
				case "positionY": 				return "<FLT-100K...100K>";
				case "positionZ": 				return "<FLT0...100K>";
				case "deltaX": 					return "<FLT-100K...100K>";
				case "deltaY": 					return "<FLT-100K...100K>";
				case "deltaZ": 					return "<FLT-100K...100K>";
				case "angle": 					return "<DEFAULT>";
				case "pitch": 					return "<DEFAULT>";
				case "roll": 					return "<DEFAULT>";
                case "sideValue": return "<DEFAULT>";
				//VALUES FOR GENERIC MESHES		
				case "blocksShotFlag": 			return "<BOOLYESNO>";
				case "pushRadius": 				return "<FLT-+INF>";
				case "pitchDelta": 				return "<DEFAULT>";
				case "rollDelta": 				return "<DEFAULT>";
				case "angleDelta": 				return "<DEFAULT>";
				case "artScale": 				return "<DEFAULT>";
				//VALUES FOR STATIONS			
				case "shieldState": 			return "<FLT-+INF>";
				case "canBuild": 				return "<BOOLYESNO>";
				case "missileStoresHoming": 	return "<INT0...+INF>";
				case "missileStoresNuke": 		return "<INT0...+INF>";
				case "missileStoresMine": 		return "<INT0...+INF>";
				case "missileStoresECM": 		return "<INT0...+INF>";
                case "missileStoresPShock":     return "<INT0...+INF>";
                //VALUES FOR SHIELDED SHIPS		
                case "throttle": 				return "<DEFAULT>";
				case "steering": 				return "<DEFAULT>";
				case "topSpeed": 				return "<DEFAULT>";
				case "turnRate": 				return "<DEFAULT>";
				case "shieldStateFront": 		return "<INT-+INF>";
				case "shieldMaxStateFront": 	return "<INT-+INF>";
				case "shieldStateBack": 		return "<INT-+INF>";
				case "shieldMaxStateBack": 		return "<INT-+INF>";
				case "shieldsOn": 				return "<BOOLYESNO>";
				case "triggersMines": 			return "<BOOLYESNO>";
				case "systemDamageBeam": 		return "<DEFAULT>";
				case "systemDamageTorpedo": 	return "<DEFAULT>";
				case "systemDamageTactical": 	return "<DEFAULT>";
				case "systemDamageTurning": 	return "<DEFAULT>";
				case "systemDamageImpulse": 	return "<DEFAULT>";
				case "systemDamageWarp": 		return "<DEFAULT>";
				case "systemDamageFrontShield":	return "<DEFAULT>";
				case "systemDamageBackShield": 	return "<DEFAULT>";
				case "shieldBandStrength0": 	return "<DEFAULT>";
				case "shieldBandStrength1": 	return "<DEFAULT>";
				case "shieldBandStrength2": 	return "<DEFAULT>";
				case "shieldBandStrength3": 	return "<DEFAULT>";
				case "shieldBandStrength4": 	return "<DEFAULT>";
				//VALUES FOR ENEMIES			
				case "targetPointX": 			return "<FLT0...100K>";
				case "targetPointY": 			return "<FLT-100K...100K>";
				case "targetPointZ": 			return "<FLT0...100K>";
				case "hasSurrendered": 			return "<BOOLYESNO>";
                case "tauntImmunityIndex":      return "<tII1_3>";
				case "eliteAIType": 			return "<ELITEAITYPE>";
				case "eliteAbilityBits": 		return "<ELITEABILITYBITS>";
				case "eliteAbilityState": 		return "<DEFAULT>";
				case "surrenderChance":			return "<INT0...100>";
				//VALUES FOR NEUTRALS			
				case "exitPointX": 				return "<FLT0...100K>";
				case "exitPointY": 				return "<FLT-100K...100K>";
				case "exitPointZ": 				return "<FLT0...100K>";
				//VALUES FOR PLAYERS			
				case "countHoming": 			return "<INT0...+INF>";
				case "countNuke": 				return "<INT0...+INF>";
				case "countMine": 				return "<INT0...+INF>";
				case "countECM": 				return "<INT0...+INF>";
				case "energy": 					return "<INT0...+INF>";
				case "warpState":				return "<INT0...4>";
				case "currentRealSpeed":		return "<READ_ONLY>";
                case "totalCoolant":            return "<INT0...+INF>";
				//DEFAULT CASE
				default:
					return "<UNKNOWN_PROPERTY>";
			}
		}

        /// <summary>
        /// Called after Decide has made its choice, or, as usual for ExpressionMembers, after user edited the value through a Dialog.
        /// For checks, SetValue must change the attributes/etc of the statement according to the newly chosen value
        /// <example>If you chose "Use GM ...", SetValue will set "use_gm_..." attribute to ""</example>
        /// </summary>
        protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
            if (value == "<BOOLYESNO>")
            {
                string flag = container.GetAttribute("value");
                if (flag == null || !Helper.IntTryParse(flag))
                    container.SetAttribute("value","0");
				else if (Helper.StringToInt(flag) == 1)
					container.SetAttribute("value", "1");
				else
					container.SetAttribute("value", "0");
            }
            //if (value == "<INVALID_PROPERTY>")
            //{
            //    value = "<DEFAULT>";
            //    container.SetAttribute("property", "positionX");
            //}
            if (Mission.Current.Loading && value == "<READ_ONLY>")
				Log.Add("Warning! Attempt to set read-only property " + container.GetAttribute("property") + " detected in event: " + container.Statement.Parent.Name + "!");
			if (Mission.Current.Loading && value == "<UNKNOWN_PROPERTY>")
				Log.Add("Warning! Unknown property " + container.GetAttribute("property") + " detected in event: "+container.Statement.Parent.Name+"!");

			base.SetValueInternal(container, value);
		}

		/// <summary>
		/// Adds "(type) "
		/// </summary>
		private void ____Add_Property(List<ExpressionMember> eML)
		{
			eML.Add(new ExpressionMember("<property>", ExpressionMemberValueDescriptions.Property, "property"));
		}

		/// <summary>
		/// Adds "
		/// </summary>
        /// 
        /// <summary>
        /// Adds "for object [selected by gm or named]"
        /// </summary>
        private void ____Add_Name(List<ExpressionMember> eML, ExpressionMemberValueDescription name = null)
        {
            name = name ?? ExpressionMemberValueDescriptions.NameAll;

            eML.Add(new ExpressionMember("for "));
            eML.Add(new ExpressionMember("object "));
            eML.Add(new ExpressionMemberCheck_Name_GM_Slot(name));
        }
		//private void ____Add_Name(List<ExpressionMember> eML)
		//{
  //          name = name ?? ExpressionMemberValueDescriptions.NameAll;

  //          eML.Add(new ExpressionMember("for "));
		//	eML.Add(new ExpressionMember("object "));
  //          eML.Add(new ExpressionMemberCheck_Name_GM(name));
  //          eML.Add(new ExpressionMember("name "));
		//	eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.NameAll, "name"));
		//}

        /// <summary>
        /// Represents a single member in an expression, which provides branching via checking a condition.
        /// This check is for "type" in [set_property] statement, it is hidden since "add to property" statement has property name as a member.
        /// </summary>
        public ExpressionMemberCheck_PropertySet()
			: base()
		{
            List<ExpressionMember> eML;

            eML = this.Add("<SENSOR>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.SensorRange, "value"));
            //____Add_Name(eML);

            eML = this.Add("<NEBULAROP>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Bool_Yes_No, "value"));
            //____Add_Name(eML);

            eML = this.Add("<ENMYSP>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_PosInf, "value"));
            eML.Add(new ExpressionMember("%.  (valid appears to be 40% - 300%)"));
            //____Add_Name(eML);

            eML = this.Add("<ENMYSH>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_PosInf, "value"));
            //____Add_Name(eML);

            eML = this.Add("<ENMYWP>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_PosInf, "value"));
            //____Add_Name(eML);



            #region <INT-+INF>    (push radius)

            eML = this.Add("<INT-+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_NegInf_PosInf, "value"));
            ____Add_Name(eML);

            #endregion

            #region <FLT-+INF>    (push radius)

            eML = this.Add("<FLT-+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_NegInf_PosInf, "value"));
            ____Add_Name(eML);

            #endregion

			#region <INT0...+INF>    (amount of missiles)

			eML = this.Add("<INT0...+INF>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>",ExpressionMemberValueDescriptions.Int_0_PosInf, "value"));
			____Add_Name(eML);

			#endregion

			#region <FLT0...+INF>    (coordinate x-z)

			eML = this.Add("<FLT0...+INF>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_0_PosInf, "value"));
            ____Add_Name(eML);

            #endregion

            #region <INT0...100>    (surrenderChance)

            eML = this.Add("<INT0...100>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_100, "value"));
            ____Add_Name(eML);

            #endregion

			#region <INT0...4>    (warpState)

            eML = this.Add("<INT0...4>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Int_0_4, "value"));
            ____Add_Name(eML);

            #endregion
			
			#region <FLT0...100K>    (coordinate x-z)

			eML = this.Add("<FLT0...100K>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_0_100k, "value"));
            ____Add_Name(eML);

            #endregion
            eML = this.Add("<tII1_3>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.tII1_3, "value"));
            ____Add_Name(eML);

//#endregion
           

            #region <FLT-100K...100K>    (coordinate y, delta)

            eML = this.Add("<FLT-100K...100K>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Flt_Minus100k_100k, "value"));
            ____Add_Name(eML);

            #endregion

            #region <BOOLYESNO>    (coordinate y, delta)

            eML = this.Add("<BOOLYESNO>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.Bool_Yes_No, "value"));
            ____Add_Name(eML);

            #endregion

            #region <ELITEAITYPE> (eliteAIType)

            eML = this.Add("<ELITEAITYPE>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.EliteAIType, "value"));
            ____Add_Name(eML);

            #endregion

            #region <ELITEABILITYBITS> (eliteAbilityBits)

            eML = this.Add("<ELITEABILITYBITS>");
            ____Add_Property(eML);
            eML.Add(new ExpressionMember("to "));
            eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.EliteAbilityBits, "value"));
            ____Add_Name(eML);

            #endregion

            #region <DEFAULT>    (...)

			eML = this.Add("<DEFAULT>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueF, "value"));
			____Add_Name(eML);

			#endregion

			#region <READ_ONLY>

			eML = this.Add("<READ_ONLY>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueF, "value"));
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! THIS PROPERTY IS READ ONLY)"));

			#endregion
			
			#region <UNKNOWN_PROPERTY>  

			eML = this.Add("<UNKNOWN_PROPERTY>");
			____Add_Property(eML);
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("<>", ExpressionMemberValueDescriptions.ValueF, "value"));
			____Add_Name(eML);
			eML.Add(new ExpressionMember("(WARNING! UNKNOWN PROPERTY NAME)"));

			#endregion
		}
	}
}