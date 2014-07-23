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

	/// <summary>
	/// Main check that checks for xml node name
	/// </summary>
	public sealed class ExpressionMemberCheck_XmlNameCondition: ExpressionMemberCheck
	{
		public override string Decide(ExpressionMemberContainer container)
		{
			switch (container.ParentStatement.Name)
			{
				case "if_variable":				
				case "if_difficulty":			
				case "if_damcon_members":		
				case "if_fleet_count":			
				case "if_docked":				
                case "if_player_is_targeting":
                case "if_timer_finished":
                case "if_object_property":
                case "if_gm_key":
                case "if_client_key":
                case "if_distance":
                    return container.ParentStatement.Name;
                case "if_exists":
                case "if_not_exists":
                    return "<existance>";
                case "if_inside_sphere":
                case "if_outside_sphere":
                case "if_inside_box":
                case "if_outside_box":
                    return "<location>";
			}

			//fallback option
			return "";
		}

		protected override void SetValueInternal(ExpressionMemberContainer container, string value)
		{
			if (value != null)
			{
				//Only set the node name if this is a straight 1 to 1 check value, otherwise the name would be modified by other checks that follow
				if (value != "" && !value.Contains(' ') && !value.Contains('>') && !value.Contains('<'))
					container.ParentStatement.Name = value;
				//For custom one-to-one
				//if (value == "<DIRECT_DETECTED>")
				//{
				//}
				//For all many-to-one
				if (value == "<existance>")
				    if (container.ParentStatement.Name != "if_exists" && container.ParentStatement.Name != "if_not_exists")
				        container.ParentStatement.Name = "if_exists";
				if (value == "<location>")
					if (container.ParentStatement.Name != "if_inside_sphere" && container.ParentStatement.Name != "if_outside_sphere" && container.ParentStatement.Name != "if_inside_box" && container.ParentStatement.Name != "if_outside_box")
						container.ParentStatement.Name = "if_inside_sphere";

				base.SetValueInternal(container, value);
			}
		}

		public ExpressionMemberCheck_XmlNameCondition()
			: base("", "check_XmlNameC")
		{
			List<ExpressionMember> eML;

			#region if_variable

			eML = this.Add("if_variable");
			eML.Add(new ExpressionMember("<>", "name_var", "name", true));
			eML.Add(new ExpressionMember("<>", "comparator", "comparator"));
			eML.Add(new ExpressionMember("<>", "value_f", "value",true));

			#endregion

			#region if_timer_finished

			eML = this.Add("if_timer_finished");
			eML.Add(new ExpressionMember("<>", "name_timer", "name", true));
			eML.Add(new ExpressionMember("has "));
			eML.Add(new ExpressionMember("finished"));

			#endregion

			AddSeparator();

			#region if_damcon_members

			eML = this.Add("if_damcon_members");
			eML.Add(new ExpressionMember("in "));
			eML.Add(new ExpressionMember("team "));
			eML.Add(new ExpressionMember("<>", "teamindex", "team_index"));
			eML.Add(new ExpressionMember("<>", "comparator", "comparator"));
			eML.Add(new ExpressionMember("<>", "dcamountf", "value", true));

			#endregion

			#region if_fleet_count

			eML = this.Add("if_fleet_count");
			eML.Add(new ExpressionMember("<>", "comparator", "comparator"));
			eML.Add(new ExpressionMember("<>", "value_f", "value", true));
			eML.Add(new ExpressionMemberCheck_AllShips_x_FleetNumber());
			
			#endregion

			AddSeparator();
			
			#region if_docked

			eML = this.Add("if_docked");
			eML.Add(new ExpressionMember("to "));
			eML.Add(new ExpressionMember("station "));
			eML.Add(new ExpressionMember("<>", "name_station", "name", true));

			#endregion

            #region if_player_is_targeting

            eML = this.Add("if_player_is_targeting");
			eML.Add(new ExpressionMember("<>", "name_all", "name", true));

			#endregion

			AddSeparator();

			#region if_exists / if_not_exists

			eML = this.Add("<existance>");
			eML.Add(new ExpressionMember("<>", "name_all", "name", true));
			eML.Add(new ExpressionMemberCheck_Existance());

			#endregion

			#region if_object_property

			eML = this.Add("if_object_property");
			eML.Add(new ExpressionMemberCheck_PropertyIf_H());

			#endregion

			#region if_inside/outside_box/sphere

			eML = this.Add("<location>");
			eML.Add(new ExpressionMember("<>", "name_all", "name", true));
			eML.Add(new ExpressionMember("is "));
			eML.Add(new ExpressionMemberCheck_Inside_x_Outside());
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMemberCheck_Box_x_Sphere());

			#endregion

            #region if_distance

            eML = this.Add("if_distance");
            eML.Add(new ExpressionMember("between "));
			eML.Add(new ExpressionMember("<>", "name_all", "name1", true));
            eML.Add(new ExpressionMember("and "));
			eML.Add(new ExpressionMember("<>", "name_all", "name2", true));
            eML.Add(new ExpressionMember("<>", "comparator", "comparator"));
            eML.Add(new ExpressionMember("<>", "value_f", "value", true));

            #endregion

			AddSeparator();

			#region if_difficulty

			eML = this.Add("if_difficulty");
			eML.Add(new ExpressionMember("<>", "comparator", "comparator"));
			eML.Add(new ExpressionMember("<>", "value_f", "value", true));

			#endregion

			AddSeparator();

			#region if_gm_key

			eML = this.Add("if_gm_key");
			eML.Add(new ExpressionMember("the "));
			eML.Add(new ExpressionMemberCheck_Letter_x_ID());

			#endregion

            #region if_client_key

            eML = this.Add("if_client_key");
            eML.Add(new ExpressionMember("the "));
            eML.Add(new ExpressionMemberCheck_Letter_x_ID());

            #endregion

			//Finally, add the unknown option
			eML = this.Add("");
			eML.Add(new ExpressionMember_Unknown());
		}
	}

}