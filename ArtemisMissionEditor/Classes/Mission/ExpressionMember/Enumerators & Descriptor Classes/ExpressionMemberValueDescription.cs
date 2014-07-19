using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	using EMVD = ExpressionMemberValueDescription;
	using EMVB = ExpressionMemberValueBehaviorInXml;
	using EMVT = ExpressionMemberValueType;
    using EMVE = ExpressionMemberValueEditor;
    using ArtemisMissionEditor.Classes.Mission.ExpressionMember.Checks;
	
    /// <summary>
    /// Defines everything about the value of the expression member, examples: Coordinate XZ, Coordinate Y, Ship name, ...
    /// </summary>
    public partial class ExpressionMemberValueDescription //define nonstatic members
    {
		/// <summary>
		/// If this member is quoted on the sides
		/// </summary>
		private string QuoteLeft;
		private string QuoteRight;

		/// <summary>
        /// Type of this member's value (like int, string, double...)
        /// </summary>
        public ExpressionMemberValueType Type; //TODO do i even need this???

		/// <summary>
		/// Behavior of this value when stored in XML
		/// </summary>
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
		/// <returns></returns>
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
		public bool IsSerialized { get { return BehaviorInXml != EMVB.Ignored; } }

		/// <summary>
		/// Convert display value to xml value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string ValueToXml(string value) { return Editor.ValueToXml(value, Type, Min, Max); }

		/// <summary>
		/// Convert xml value to display value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string ValueToDisplay(string value) { return Editor.ValueToDisplay(value, Type, Min, Max); }

		/// <summary>
		/// Takes value from XML (GetValue), returns value fit for displaying on-screen
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string GetValueDisplay(ExpressionMemberContainer container)
		{
			if (Editor == EMVE.Nothing)
				return null;

			string result = Editor == EMVE.Label ? container.Member.Text : container.GetValue();

			result = ValueToDisplay(result);

			//TODO: Change into something else sometimes! <-- WTF does this mean?
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

					if (OnClick_private_RecursivelyActivate(((ToolStripMenuItem)item).DropDownItems[forward ? i : ((ToolStripMenuItem)item).DropDownItems.Count-1-i], ref next, forward))
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
				if ((!container.Member.IsCheck)&&(Type == EMVT.VarDouble || Type == EMVT.VarInteger || Type == EMVT.VarBool))
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
			if ((curCMS = Editor.PrepareCMS(container, mode))!=null)
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
						((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).DropDownItems[((ToolStripMenuItem)curCMS.Items[curCMS.Items.Count - 1]).DropDownItems.Count - 1].PerformClick();
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
			Editor.ShowEditingGUI(container, Type, Min, Max, DefaultIfNull);
		}
		
		public ExpressionMemberValueDescription(EMVT type, EMVB behavior, EMVE editor, object min = null, object max = null, string quoteLeft = "", string quoteRight = " ", string defaultIfNull = null)
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

    public partial class ExpressionMemberValueDescription //define static members
    {
        private static Dictionary<string,EMVD> Items;

		public static EMVD GetItem(string name)
		{
			if (Items.ContainsKey(name))
				return Items[name];
			else
				throw new ExpressionException("FAIL! Trying to get a nonexistant EMVD "+name);
		}

        /// <summary>
        /// Initialise the list of member descriptions
        /// </summary>
		static ExpressionMemberValueDescription()
        {
            Items = new Dictionary<string, EMVD>();

			Items.Add("<label>",		    new EMVD(EMVT.Nothing,		EMVB.Ignored,			EMVE.Label,null,null,"",""));					// Caption or Label (used often when plain text is displayed)
			Items.Add("<blank>",		    new EMVD(EMVT.Nothing,		EMVB.Ignored,			EMVE.Nothing,null,null,"",""));					// Blank aka NOTHING (often used by hidden checks)

			Items.Add("commentary",		    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultString,null,null,"","",""));		// Commentary expression for editing comments
			
			Items.Add("check_XmlNameA",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.XmlNameActionCheck));						// Root check member for chosing the expression
			Items.Add("check_XmlNameC",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.XmlNameConditionCheck));					// Root check member for chosing the expression
			Items.Add("unknown",		    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultString));							// Unknown expression for editing something not known to the editor

			Items.Add("check_point/gmpos",  new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check point / gm position	from [create, destroy, ...]
            Items.Add("x",                  new EMVD(EMVT.VarDouble,    EMVB.AsIsWhenFilled,    EMVE.DefaultDouble, 0.0, 100000.0, "(", ", "));	//  x coordinate				from [create, destroy, ...]
			Items.Add("y",				    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, -100000.0, 100000.0,"",", "));//  y coordinate				from [create, destroy, ...]
			Items.Add("z",				    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, 0.0, 100000.0,"",") "));	//  z coordinate				from [create, destroy, ...]
			Items.Add("xu",				    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, null,null,"(",", "));		//  x coordinate unbound		from [create, destroy, ...]
			Items.Add("yu",				    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, null,null,"",", "));		//  y coordinate unbound		from [create, destroy, ...]
			Items.Add("zu",				    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, null,null,"",") "));		//  z coordinate unbound		from [create, destroy, ...]
			
			Items.Add("check_line/circle",  new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check line/circle			from [create]
			Items.Add("radius",			    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 100000));				// [radius]						from [create]
			Items.Add("radiusH",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.Nothing));									// [radius] hidden				from [create]
			
			Items.Add("check_ID/keys",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check hullID / hullraceKeys	from [create]
			Items.Add("hullID",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.HullID,null,null,"\"","\" ",""));			// [hullID]						from [create]
			Items.Add("raceKeys",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.RaceKeys,null,null,"\"","\" ",""));		// [raceKeys]					from [create]
			Items.Add("hullKeys",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.HullKeys,null,null,"\"","\" ",""));		// [hullKeys]					from [create]

			Items.Add("check_fakeShields",  new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check has fake shields    	from [create]
			Items.Add("fakeShieldsFR",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, -1, 1000,"(",") "));		//  fakeShields FR				from [create]
			Items.Add("fakeShieldsF",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, -1, 1000,"(",", "));		//  fakeShields FR				from [create]
			Items.Add("fakeShieldsR",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, -1, 1000,"",") "));		//  fakeShields FR				from [create]
			Items.Add("hasFakeShldFreq",    new EMVD(EMVT.VarBool,		EMVB.AsIsWhenFilled,	EMVE.DefaultBool, "no fake frequency", "fake frequency"));// [hasFakeShldFreq]	from [create]
			
			Items.Add("use_gm",             new EMVD(EMVT.VarString,	EMVB.AsIs,				EMVE.Nothing));									//  use_gm_position/selection	from [create, destroy, ...]
			Items.Add("type",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.CreateType));								// [type]						from [create] 
            Items.Add("name",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		// [name]						from [create, destroy, ...]
			Items.Add("name_with_comma",    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\", "));		
			Items.Add("angle",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble, 0.0, 360.0));				// [angle]						from [create, ...]
			Items.Add("fleetnumber",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, -1, 99));					// [fleetnumber]				from [create, ...]
			Items.Add("podnumber",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 9));					// [podnumber]					from [create]
			Items.Add("meshFileName",	    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PathEditor,"DeleD Mesh Files|*.dxs|All Files|*.*","Select Delgine mesh file","\"","\" "));// [meshFileName]			from [create]
			Items.Add("textureFileName",    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PathEditor,"PNG Files|*.png|All Files|*.*", "Select texture file", "\"", "\" "));// [textureFileName]	from [create]
			Items.Add("hullRace",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString, null, null, "\"", "\" "));	// [hullRace]					from [create]
			Items.Add("hullType",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString, null, null, "\"", "\" "));	// [hullType]					from [create]
			Items.Add("colorR",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"(",", "));			//  Color R/G/B					from [create]
			Items.Add("colorG",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"",", "));			//  Color R/G/B					from [create]
			Items.Add("colorB",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,1.0,"",") "));			//  Color R/G/B					from [create]
			
			Items.Add("count",			    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, null,""," ","0"));		// [count]						from [create]
            Items.Add("angle_unbound",      new EMVD(EMVT.VarDouble,   EMVB.AsIs,              EMVE.DefaultDouble));	        				//  startAngle/endAngle			from [create] (nameless)
            Items.Add("randomRange",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 100000));				// [randomRange]				from [create]
			Items.Add("randomSeed",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0));						// [randomSeed]					from [create]

			Items.Add("check_named/nameless",new EMVD(EMVT.VarString,   EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check named / nameless		from [destroy/destroy_near]
			Items.Add("typeH",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.Nothing));									//  type hidden					from [destroy] 

			Items.Add("check_name/gmsel",   new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check name / gm selection	from [destroy, ...]

			Items.Add("check_p/n/gmpos",    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check point / name /gm pos.	from [destroy_near]

			Items.Add("text",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		// [text]						from [log, ...]
			
			Items.Add("check_setvar",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.SetVariableCheck));						// Check exact/rndInt/rndFloat	from [set_variable]
			Items.Add("value",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0));						// [value]						from [set_variable,if_variable]
			Items.Add("randInt",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0));						//  randInt   Low/High			from [set_variable]
			Items.Add("randFloat",		    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0));						//  randFloat Low/High			from [set_variable]

			Items.Add("type_ai",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.AIType,null,null,"",""));					// [type]						from [add_ai] 
			Items.Add("check_dneb",		    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DistanceNebulaCheck,null,null,""," "));	// Check distance in/out nebula from [add_ai] 
			Items.Add("value_r",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,100000,""," "));			//  value radius				from [add_ai] 
			//Items.Add("value_r|q",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,100000,"","\" ","0"));	//  value radius /w trailing "  from [add_ai] 
			Items.Add("value_r|q",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,100000,"",""));			//  ^^ LOOKS BAD!
			//Items.Add("value_t",		    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,null,"","\" ","0.0"));	//  value throttle				from [add_ai]
			Items.Add("value_t",		    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,null,"","","0.0"));		//  ^^ LOOKS BAD!

			Items.Add("check_p/n",		    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted,null,null,""," "));	// Check targetName / point		from [direct] 
			Items.Add("check_convertd",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.ConvertDirectCheck,null,null,""," "));		// Check convert direct			from [direct] 

			Items.Add("seconds",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,null,""," ","0"));		// [seconds]					from [set_timer]

			Items.Add("msgFileName",	    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PathEditor,"OGG Audio Files|*.ogg|All Files|*.*","Select message audio file","\"","\" "));// [fileName]				from [incoming_message]
			Items.Add("soundFileName",	    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PathEditor,"WAVE Audio Files|*.wav|All Files|*.*","Select sound file","\"","\" "));// [fileName]				from [play_sound_now]
			Items.Add("msg_from",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		// [from]						from [incoming_message]
			Items.Add("msgMediaType",	    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" ","0"));	// [mediaType]					from [incoming_message]
            
            Items.Add("consoles",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.ConsoleList,null,null,"(",") "));		    // [consoles]					from [warning_popup_message]

			Items.Add("<body>",			    new EMVD(EMVT.Body,			EMVB.Ignored,			EMVE.DefaultBody,null,null,"","",""));		// <body>						from [incoming_comms_text]
			Items.Add("title",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		// [title]						from [big_message]
			Items.Add("subtitle",		    new EMVD(EMVT.VarString,	EMVB.AsIs,				EMVE.DefaultString,null,null,"\"","\" "));		// [subtitle]					from [big_message]

			Items.Add("property",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PropertyObject,null,null,"\"","\" ","angle"));// [property]	    		from [set_object_property] 
			Items.Add("int-+inf",	        new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,null,null,""," "));			//  value from -INF to +INF		from [set_object_property]
            Items.Add("int0...+inf",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,null,""," "));			//  value from 0 to +INF		from [set_object_property]
            Items.Add("int0...100k",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,100000,""," "));			//  value from 0 to 100k		from [set_object_property]
            Items.Add("int0...100",	        new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,0,100,""," "));	    		//  value from 0 to 100 		from [set_object_property]
            Items.Add("int0...4",	        new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.WarpState,0,4,""," "));	    			//  value from 0 to 4			from [set_object_property]
            Items.Add("int-100k...100k",    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger,-100000,100000,""," "));	//  value from -100k to 100k	from [set_object_property]
            Items.Add("flt-+inf",	        new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,null,null,""," "));			//  value from -INF to +INF		from [set_object_property]
            Items.Add("flt0...+inf",	    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,null,""," "));			//  value from 0 to +INF		from [set_object_property]
            Items.Add("flt0...100k",	    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,100000.0,""," "));		//  value from 0 to 100k		from [set_object_property]
            Items.Add("flt0...100",	        new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,0.0,100.0,""," "));	    	//  value from 0 to 100 		from [set_object_property]
            Items.Add("flt-100k...100k",    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble,-100000.0,100000.0,""," ")); //  value from -100k to 100k	from [set_object_property]
            Items.Add("boolyesno",	        new EMVD(EMVT.VarBool,  	EMVB.AsIsWhenFilled,	EMVE.DefaultBool,"no","yes",""," "));		    //  value yes,no          		from [set_object_property]
            Items.Add("eliteaitype",        new EMVD(EMVT.VarInteger,  	EMVB.AsIsWhenFilled,	EMVE.EliteAIType,0,2,"\"","\" "));		        //  value for eliteAIType  		from [set_object_property]
            Items.Add("eliteabilitybits",   new EMVD(EMVT.VarInteger, 	EMVB.AsIsWhenFilled,	EMVE.EliteAbilityBits,0,null,""," "));		    //  value for eliteAbilityBits	from [set_object_property]
			
            Items.Add("property_f",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PropertyFleet,null,null,"\"","\" ","fleetSpacing"));// [property]			from [set_fleet_property] 
            
			Items.Add("value_f",		    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DefaultDouble));							// [value]						from [set_object_property,...] 

			Items.Add("fleetIndex",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 99,""," ","0"));		// [fleetIndex]					from [set_fleet_property]
			
			Items.Add("index",			    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.SkyboxIndex, 0, null,""," ","0"));			// [index]						from [set_skybox_index]
			Items.Add("difficulty",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.Difficulty, 1, 11,""," ","1"));			// [value]						from [set_difficulty_level]
			
			Items.Add("damage",			    new EMVD(EMVT.VarDouble,	EMVB.AsIsWhenFilled,	EMVE.DamageValue,0.0,1.0,""," ","0.0"));		// [value]						from [set_player_grid_damage]
			Items.Add("nodeindex",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 100,""," ","0"));		// [index]						from [set_player_grid_damage]

			Items.Add("systemType",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.ShipSystem,null,null,"\"","\" ","systemBeam"));// [systemType]				from [set_player_grid_damage]
			Items.Add("countFrom",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.CountFrom,null,null,""," ","front"));		// [countFrom]					from [set_player_grid_damage]

			Items.Add("teamindex",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.TeamIndex, 0, 2,""," ","0"));				// [index]						from [set_damcon_members]
			Items.Add("dcamount",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.TeamAmount, 0, 6,""," ","0"));				// [value]						from [set_damcon_members]

			Items.Add("comparator",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.Comparator,null,null,""," ","EQUALS"));	//[comparator]					from [if_variable]
			Items.Add("dcamountf",		    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.TeamAmountF, null, null,""," ","0"));		// [value]						from [if_damcon_members]
			
			Items.Add("check_all/fn",	    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check allships/fleetnumber	from [if_fleet_count]
			Items.Add("fleetnumber_if",	    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 99,""," "));			// [fleetnumber]				from [if_fleet_number]
			Items.Add("check_letter/id",    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check allships/fleetnumber	from [if_gm_key]
			Items.Add("keyid",			    new EMVD(EMVT.VarInteger,	EMVB.AsIsWhenFilled,	EMVE.DefaultInteger, 0, 128,""," "));			// [value]						from [if_gm_key]
			Items.Add("letter",			    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.DefaultString,null,null,"\"","\" "));		// [keyText]					from [if_gm_key]

			Items.Add("check_existance",    new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check if_(not)_exists		from [if_exists,if_not_exists]
			
			Items.Add("check_in/out",       new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check inside/outside			from [if_inside/outside_box/sphere]
			Items.Add("check_box/sph",      new EMVD(EMVT.VarString,	EMVB.Ignored,			EMVE.DefaultCheckUnsorted));					// Check box/sphere				from [if_inside/outside_box/sphere]

            Items.Add("name_timer",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.TimerName,null,null,"\"","\" "));			// [name]						from [if_timer]
			Items.Add("name_var",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.VariableName,null,null,"\"","\" "));		// [name]						from [if_variable]
			Items.Add("name_all",		    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.NamedAllName,null,null,"\"","\" "));		// [name]						from [...]
			Items.Add("name_all_with_colon",new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.NamedAllName,null,null,"\"","\": "));		// [name]						from [...]
			Items.Add("name_station",	    new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.NamedStationName,null,null,"\"","\" "));	// [name]						from [if_docked]
			Items.Add("name_createplayer",	new EMVD(EMVT.VarString,	EMVB.AsIsWhenFilled,	EMVE.PlayerNames,null,null,"\"","\" "));		// [name]						from [create]

            Items.Add("shipState",          new EMVD(EMVT.VarInteger,   EMVB.AsIsWhenFilled,    EMVE.SpecialShipType, -1, 3,"\"","\" "));
            Items.Add("captainState",       new EMVD(EMVT.VarInteger,   EMVB.AsIsWhenFilled,    EMVE.SpecialCapitainType, -1, 5,"\"","\" "));
            Items.Add("sideValue",          new EMVD(EMVT.VarInteger,   EMVB.AsIsWhenFilled,    EMVE.Side, 0, 31));
            
            //Items.Add("newname", new EMVD(EMVT.VarString, EMVB.AsIs, EMVE.DefaultString, null, null, "\"", "\" "));		// [newname]					from [set_ship_text]
            //Items.Add("race", new EMVD(EMVT.VarString, EMVB.AsIs, EMVE.DefaultString, null, null, "\"", "\" "));		// [race]					from [set_ship_text]
            //Items.Add("class", new EMVD(EMVT.VarString, EMVB.AsIs, EMVE.DefaultString, null, null, "\"", "\" "));		// [class]					from [set_ship_text]
            //Items.Add("desc", new EMVD(EMVT.VarString, EMVB.AsIs, EMVE.DefaultString, null, null, "\"", "\" "));		// [desc]					from [set_ship_text]
            //Items.Add("scan_desc", new EMVD(EMVT.VarString, EMVB.AsIs, EMVE.DefaultString, null, null, "\"", "\" "));		// [scan_desc]					from [set_ship_text]
            
			
		}
    }

}
