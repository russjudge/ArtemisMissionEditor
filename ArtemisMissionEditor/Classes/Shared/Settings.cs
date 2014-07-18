using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Win32;

namespace ArtemisMissionEditor
{

    [Serializable]
    public sealed class Settings
    {
        public  static string _programDataFolder = @"%appdata%\ArtemisMissionEditor\";
        public  static string _fileName = "settings.binary";
        private static int LastVersion = 3;

        private static Settings _current;
        [Browsable(false)]
        public static Settings Current
        {
            get { return _current; }
            set
            {
                _current = value;
                if (Program.FS != null)
                {
                    Program.FS.pgMisc.SelectedObject = Current;
                    Program.FS.pgColor.SelectedObject = new DictionaryPropertyGridAdapter(Settings.Current._bindsBrushColor);
                }
            }
        }

        private static VesselData _vesselData;
        public static VesselData VesselData
        {
            get
            {
                if (_vesselData == null) _vesselData = new VesselData();
                return _vesselData;
            }
            set { _vesselData = value; }
        }

		#region Misc. Settings

        private int _version;

        [DisplayName("Add failure comments"), Description("Add comments about failure when saving create statements from the space map."), DefaultValue(false)]
        public bool AddFailureComments{get;set;}

        /// <summary>
        /// When inserting new node, insert it over or under the selected element
        /// </summar>y
        [DisplayName("Insert new element over selected element"), Description("Whether to insert new elements over selected elements or under selected elements in the tree view."), DefaultValue(true)]
        public bool InsertNewOverElement{get;set;}

        /// <summary>
        /// When moving node inside node, it should end up at last or first position
        /// </summary>
        [DisplayName("Place nodes dragged inside a folder to the last position"), Description("Whether the nodes dragged into a folder should be put to the last position or to the first position."), DefaultValue(true)]
        public bool DragIntoFolderToLastPosition{get;set;}

		/// <summary>
		/// Wether the default (`) map scale will be padded or zommed in 100%
		/// </summary>
        [DisplayName("Space map is padded by default"), Description("If true, space map will be padded (fully zoomed out) when reset key is pressed or space map is opened.\r\nIf false, space map will instead be fit into window ."), DefaultValue(true)]
        public bool DefaultSpaceMapScalePadded { get; set; }

		/// <summary>
		/// Show y-coordinate lines for nameless objects on the space map
		/// </summary>
        [DisplayName("Use Y for nameless"), Description("Consider Y coordinate when displaying nameless (nebulas, asteroids, mines) objects on the space map.\r\nIf true, objects with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
        public bool UseYForNameless{get;set;}

		/// <summary>
		/// Show y-coordinate lines for named objects on the space map
		/// </summary>
        [DisplayName("Use Y for named"), Description("Consider Y coordinate when displaying named (enemies, stations, ...) objects on the space map.\r\nIf true, objects with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
        public bool UseYForNamed{get;set;}

		/// <summary>
		/// Show y-coordinate lines for named objects on the space map
		/// </summary>
		[DisplayName("Use Y for selection"), Description("Consider Y coordinate when displaying selections like spheres.\r\nIf true, selections that have y-coordinate with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
		public bool UseYForSelection { get; set; }

		/// <summary>
		/// How big are the y-coord lines on the space map
		/// </summary>
        [DisplayName("Y scale"), Description("Object's Y value is multiplied by this value when it is displayed on the space map.\r\nEx: if Y scale = 7, objects with X=0 Y=1000 Z=0 and X=0 Y=0 Z=7000 will be displayed on the screen in the same position."), DefaultValue(7.0)]
        public double YScale{get;set;}

        /// <summary>
        /// Show numbers near labels for expression editing
        /// </summary>
        [DisplayName("Show label numbers"), Description("Show hotkey numbers near expression labels."), DefaultValue(false)]
        public bool ShowLabelNumbers{get;set;}

        /// <summary>
        /// Amount of names displayed in one submenu for timers and variables
        /// </summary>
        [DisplayName("Names per submenu"), Description("Amount of names to be showed per submenu when showing lists of names (of timers, values, or named objects)."), DefaultValue(15)]
        public int NamesPerSubmenu { get; set; }

		/// <summary>
		/// Shall the spacemap display names consisting of whitespaces decorated with angle brackets
		/// </summary>
        [DisplayName("Mark whitespace names on space map"), Description("Mark object names consisting only of whitespaces with <> brackets."), DefaultValue(false)]
		public bool MarkWhitespaceNamesOnSpaceMap{get;set;}

		/// <summary>
		/// Focus statement window when pasting into selected node
		/// </summary>
        [DisplayName("Focus statements when pasting into node"), Description("When pasting a statement while focus is on a node, focus the newly inserted statement."), DefaultValue(false)]
		public bool FocusOnStatementPaste { get; set; }

		/// <summary>
		/// Show start node's statements in background when editing on space map
		/// </summary>
        [DisplayName("Show background statements on space map"), Description("Whether to show statements from the start block (or chosen event) in the background when editing objects on the space map."), DefaultValue(true)]
		public bool ShowStartStatementsInBackground { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Use generic mesh color"), Description("Use generic mesh color when drawing generic mesh on space map."), DefaultValue(true)]
		public bool UseGenericMeshColor { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Minimal luminance"), Description("If use generic mesh color is true then what is the minimal luminance to use generic mesh color instead of basic color (set this higher to prevent very dark generic meshes from being invisible).\r\nValid values from 0 to 1."), DefaultValue(0.05)]
		public double MinimalLuminance { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Space map min size"), Description("Minimal possible space map size, in pixels"), DefaultValue(100)]
		public int MinimalSpaceMapSize { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Space map max size"), Description("Maximal possible space map size, in pixels.\r\nIf you see a lot of blinking when zoomed to the max, but not when zooming to a lesser degree, and moving map around, you can reduce this value to prevent zooming that far."), DefaultValue(15000)]
		public int MaximalSpaceMapSize { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Select label when using plus/minus"), Description("When using plus/minus keys to edit statement values, while focus is on the statement tree view, should the label that was changed be selected or not."), DefaultValue(false)]
		public bool SelectLabelWhenUsingPlusMinus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Clear start node's contents on delete"), Description("Clear start node contents when delete command is called on it. If true, attempt to delete start node (alone or  If false, start node is unaffected by delete."), DefaultValue(true)]
		public bool ClearStartNodeOnDelete { get; set; }

		private int _autoSaveInterval;
		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Autosave interval"), Description("Defines interval (in minutes) between autosaves. Autosave saves current mission state to a file in program folder (%APPDATA%\\ArtemisMissionEditor\\). You can manually load mission file from that folder if you need, like a program crashes or computer reboots unexpectedly. 0 means autosave is disabled."), DefaultValue(5)]
		public int AutoSaveInterval
		{
			get
			{
				return _autoSaveInterval;
			}
			set
			{
				value = value >= 0 ? value : 0;
				_autoSaveInterval = value;
				if (Program.FM != null)
				{
					Program.FM._FM_t_AutoUpdateTimer.Enabled = _autoSaveInterval != 0;
					Program.FM._FM_t_AutoUpdateTimer.Interval = 1 + _autoSaveInterval * 60 * 1000;
				}
			}
		}

		private int _autoSaveFilesCount;
		/// <summary>
		/// 
		/// </summary>
		[DisplayName("Autosave files count"), Description("Defines amount of files that would be kept. If more than this amount of autosave files are present in the program folder, oldest file will be deleted."), DefaultValue(10)]
		public int AutoSaveFilesCount { get { return _autoSaveFilesCount; } set { value = value >= 1 ? value : 1; _autoSaveFilesCount = value; } }


		#endregion

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public string NewMissionStartBlock { get; set; }

		public static  void MakeSureProgramDataFolderExists(string fileName)
        {
            if (fileName.Contains(@"\"))
            {
                if (!Directory.Exists(fileName.Substring(0, fileName.LastIndexOf(@"\") + 1)))
                    Directory.CreateDirectory(fileName.Substring(0, fileName.LastIndexOf(@"\") + 1));
            }
        }

        public static bool Load()
        {
            string fileName = Environment.ExpandEnvironmentVariables(_programDataFolder + _fileName);
            FileStream istream = null;
            try
            {
                BinaryFormatter iser = new BinaryFormatter();
                istream = new FileStream(fileName, FileMode.Open);
                Settings.Current = (Settings)(iser.Deserialize(istream));
                istream.Close();

                if (Settings.Current._version != Settings.LastVersion)
                {
                    Settings.Current.UpdateSettings();
                    Settings.Save();
                }
                
                return true;
            }
            catch (Exception e)
            {
                if (istream!=null)
                    istream.Close();
                
                Log.Add("Loading settings file has failed with the following exception:");
                Log.Add(e.Message);

                return false;
            }
        }

        public static bool Save()
        {
            string fileName = Environment.ExpandEnvironmentVariables(_programDataFolder + _fileName);
            FileStream ostream = null;
            try
            {
                BinaryFormatter oser = new BinaryFormatter();
                MakeSureProgramDataFolderExists(fileName);
                ostream = new FileStream(fileName, FileMode.Create);
                oser.Serialize(ostream, Settings.Current);
                ostream.Close();
                return true;
            }
            catch (Exception e)
            {
                if (ostream != null)
                    ostream.Close();
                Log.Add("Creating settings file has failed with the following exception:");
                Log.Add(e.Message);
                return false;
            }
        }

        //use key.SetValue and key.GetValue
        public static RegistryKey GetRegistryKey()
        {
            RegistryKey key;
            key = Registry.CurrentUser.OpenSubKey("Software\\ArtemisMissionEditor", true);
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey("Software\\ArtemisMissionEditor");
            return key;
        }

        [Browsable(false)]
        public string DefaultVesselDataPath { get; set; }

        //Bindings
        public Dictionary<Keys, KeyCommands> _bindsKeyboardKeys;
        public List<KeyCommands> _bindsKeyboardCommandsGlobal;
        public Dictionary<MapColors, Color> _bindsBrushColor;
        public Dictionary<MapColors, HatchStyle> _bindsBrushStyle;
        public Dictionary<MapFonts, Font> _bindsFont;

        public void BindsKeyboard_LoadDefaults()
        {
            _bindsKeyboardKeys = new Dictionary<Keys, KeyCommands>();
            _bindsKeyboardCommandsGlobal = new List<KeyCommands>();

            _bindsKeyboardKeys.Add(Keys.Right | Keys.Shift, KeyCommands.MoveRight);
            _bindsKeyboardKeys.Add(Keys.Left | Keys.Shift, KeyCommands.MoveLeft);
            _bindsKeyboardKeys.Add(Keys.Up | Keys.Shift, KeyCommands.MoveUp);
            _bindsKeyboardKeys.Add(Keys.Down | Keys.Shift, KeyCommands.MoveDown);

            _bindsKeyboardKeys.Add(Keys.Right, KeyCommands.MoveSelectionRight);
            _bindsKeyboardKeys.Add(Keys.Left, KeyCommands.MoveSelectionLeft);
            _bindsKeyboardKeys.Add(Keys.Up, KeyCommands.MoveSelectionUp);
            _bindsKeyboardKeys.Add(Keys.Down, KeyCommands.MoveSelectionDown);

            _bindsKeyboardKeys.Add(Keys.Right | Keys.Control, KeyCommands.MoveSelectionFastRight);
            _bindsKeyboardKeys.Add(Keys.Left | Keys.Control, KeyCommands.MoveSelectionFastLeft);
            _bindsKeyboardKeys.Add(Keys.Up | Keys.Control, KeyCommands.MoveSelectionFastUp);
            _bindsKeyboardKeys.Add(Keys.Down | Keys.Control, KeyCommands.MoveSelectionFastDown);

            _bindsKeyboardKeys.Add(Keys.Add, KeyCommands.ZoomPlus);
            _bindsKeyboardKeys.Add(Keys.Subtract, KeyCommands.ZoomMinus);
            _bindsKeyboardKeys.Add(Keys.Oemtilde, KeyCommands.ResetMap);

            _bindsKeyboardKeys.Add(Keys.D1, KeyCommands.AddAnomaly);
            _bindsKeyboardKeys.Add(Keys.D2, KeyCommands.AddBlackHole);
            _bindsKeyboardKeys.Add(Keys.D3, KeyCommands.AddEnemy);
            _bindsKeyboardKeys.Add(Keys.D4, KeyCommands.AddGenericMesh);
            _bindsKeyboardKeys.Add(Keys.D5, KeyCommands.AddMonster);
            _bindsKeyboardKeys.Add(Keys.D6, KeyCommands.AddNeutral);
            _bindsKeyboardKeys.Add(Keys.D7, KeyCommands.AddPlayer);
            _bindsKeyboardKeys.Add(Keys.D8, KeyCommands.AddStation);
            _bindsKeyboardKeys.Add(Keys.D9, KeyCommands.AddWhales);

            _bindsKeyboardKeys.Add(Keys.I, KeyCommands.AddAsteroids);
            _bindsKeyboardKeys.Add(Keys.O, KeyCommands.AddMines);
			_bindsKeyboardKeys.Add(Keys.P, KeyCommands.AddNebulas);

            _bindsKeyboardKeys.Add(Keys.D0, KeyCommands.DeleteSelected);
            _bindsKeyboardKeys.Add(Keys.Delete, KeyCommands.DeleteSelected);
            _bindsKeyboardKeys.Add(Keys.Space, KeyCommands.MoveSelected);
            _bindsKeyboardKeys.Add(Keys.A | Keys.Control, KeyCommands.SelectAll);
            _bindsKeyboardKeys.Add(Keys.Home, KeyCommands.SelectPreviousNameless);
            _bindsKeyboardKeys.Add(Keys.End, KeyCommands.SelectNextNameless);
            _bindsKeyboardKeys.Add(Keys.PageUp, KeyCommands.SelectPreviousNamed);
            _bindsKeyboardKeys.Add(Keys.PageDown, KeyCommands.SelectNextNamed);

            _bindsKeyboardKeys.Add(Keys.Q, KeyCommands.SetStartAnglePositive);
            _bindsKeyboardKeys.Add(Keys.Q | Keys.Shift, KeyCommands.SetStartAngleNegative);
            _bindsKeyboardKeys.Add(Keys.Q | Keys.Control, KeyCommands.SetStartAngleNull);
            _bindsKeyboardKeys.Add(Keys.W, KeyCommands.SetEndAnglePositive);
            _bindsKeyboardKeys.Add(Keys.W | Keys.Shift, KeyCommands.SetEndAngleNegative);
            _bindsKeyboardKeys.Add(Keys.W | Keys.Control, KeyCommands.SetEndAngleNull);

            _bindsKeyboardKeys.Add(Keys.Escape | Keys.Shift, KeyCommands.FinishEditing);
            _bindsKeyboardKeys.Add(Keys.Enter | Keys.Shift, KeyCommands.FinishEditingForceYes);
            _bindsKeyboardKeys.Add(Keys.F4 | Keys.Alt, KeyCommands.FinishEditingForceNo);

            _bindsKeyboardKeys.Add(Keys.Escape, KeyCommands.FocusMap);

            _bindsKeyboardCommandsGlobal.Add(KeyCommands.FinishEditing);
            _bindsKeyboardCommandsGlobal.Add(KeyCommands.FinishEditingForceNo);
            _bindsKeyboardCommandsGlobal.Add(KeyCommands.FinishEditingForceYes);
            _bindsKeyboardCommandsGlobal.Add(KeyCommands.FocusMap);
        }

        public KeyCommands GetKeyCommand(Keys keyData)
        {
            if (_bindsKeyboardKeys.ContainsKey(keyData))
                return _bindsKeyboardKeys[keyData];
            else 
                return KeyCommands.KeyCommands_Nothing;
        }

        public bool KeyCommandRequiresFocus(KeyCommands command)
        {
            return !_bindsKeyboardCommandsGlobal.Contains(command);
        }

        public void BindsBrush_LoadDefaults()
        {
            _bindsBrushColor = new Dictionary<MapColors, Color>();
            _bindsBrushStyle = new Dictionary<MapColors, HatchStyle>();

            _bindsBrushColor.Add(MapColors.MapBackground,			Color.FromArgb(0, 0, 32));
            _bindsBrushColor.Add(MapColors.MapGridLine,				Color.FromArgb(0, 0, 192));
            _bindsBrushColor.Add(MapColors.MapSelection,			Color.FromArgb(192, 255, 255));
            _bindsBrushColor.Add(MapColors.MapSelectionRectangle,	Color.FromArgb(255, 128, 255, 255));
            _bindsBrushStyle.Add(MapColors.MapSelectionRectangle,	HatchStyle.WideDownwardDiagonal);
            _bindsBrushColor.Add(MapColors.MapSelectionDark,		Color.FromArgb(128, 192, 255, 255));
            _bindsBrushColor.Add(MapColors.Anomaly,					Color.FromArgb(255, 255, 255));
            _bindsBrushColor.Add(MapColors.Enemy,					Color.FromArgb(192, 0, 0));
            _bindsBrushColor.Add(MapColors.EnemyBG,					Color.FromArgb(192, 0, 0));
            _bindsBrushStyle.Add(MapColors.EnemyBG, 				HatchStyle.DarkHorizontal);
            _bindsBrushColor.Add(MapColors.Monster,					Color.FromArgb(192, 0, 192));
            _bindsBrushColor.Add(MapColors.Station,					Color.FromArgb(255, 255, 0));
            _bindsBrushColor.Add(MapColors.StationBG,				Color.FromArgb(192, 192, 0));
            _bindsBrushColor.Add(MapColors.Player,					Color.FromArgb(255, 0, 255));
            _bindsBrushColor.Add(MapColors.PlayerBG,				Color.FromArgb(255, 0, 255));
            _bindsBrushStyle.Add(MapColors.PlayerBG, 				HatchStyle.DarkHorizontal);
            _bindsBrushColor.Add(MapColors.Whale,					Color.FromArgb(0, 255, 0));
            _bindsBrushColor.Add(MapColors.WhaleBG,					Color.FromArgb(0, 255, 0));
            _bindsBrushStyle.Add(MapColors.WhaleBG, 				HatchStyle.DarkHorizontal);
            _bindsBrushColor.Add(MapColors.Neutral,					Color.FromArgb(0, 255, 255));
            _bindsBrushColor.Add(MapColors.NeutralBG,				Color.FromArgb(0, 255, 255));
            _bindsBrushStyle.Add(MapColors.NeutralBG, 				HatchStyle.DarkHorizontal);
            _bindsBrushColor.Add(MapColors.BlackHole,				Color.FromArgb(0, 0, 255));
            _bindsBrushColor.Add(MapColors.GenericMesh,				Color.FromArgb(255, 255, 255));
			_bindsBrushStyle.Add(MapColors.GenericMesh,				HatchStyle.DarkDownwardDiagonal);
            _bindsBrushColor.Add(MapColors.GenericMeshBG,			Color.FromArgb(255, 255, 255));
            _bindsBrushColor.Add(MapColors.MapBorderActive,			Color.FromArgb(0, 255, 0));
            _bindsBrushColor.Add(MapColors.MapBorderInactive,		Color.FromArgb(128, 0, 0));
            _bindsBrushColor.Add(MapColors.MapHeightMarker,			Color.FromArgb(192, 192, 64));
            _bindsBrushColor.Add(MapColors.MapNamelessHeightMarker,	Color.FromArgb(64, 255, 255, 128));
            _bindsBrushColor.Add(MapColors.Nebula,					Color.FromArgb(32, 255, 0, 255));
            _bindsBrushColor.Add(MapColors.NebulaBG,				Color.FromArgb(16, 255, 0, 255));
			_bindsBrushStyle.Add(MapColors.NebulaBG,				HatchStyle.Percent10);
            _bindsBrushColor.Add(MapColors.Asteroid,				Color.FromArgb(128, 255, 128, 0));
			_bindsBrushColor.Add(MapColors.AsteroidBG,				Color.FromArgb(48, 255, 128, 0));
			//_bindsBrushStyle.Add(MapColors.AsteroidBG,				HatchStyle.DiagonalCross);
            _bindsBrushColor.Add(MapColors.AsteroidBright,			Color.FromArgb(212, 255, 128, 0));
            _bindsBrushColor.Add(MapColors.Mine,					Color.FromArgb(192, 255, 255, 255));
			_bindsBrushColor.Add(MapColors.MineBG,					Color.FromArgb(96, 255, 255, 255));
			//_bindsBrushStyle.Add(MapColors.MineBG,					HatchStyle.Percent25);
            _bindsBrushColor.Add(MapColors.MineDark,				Color.FromArgb(96, 255, 255, 255));
        }

        public Color GetColor(MapColors mc)
        {
            Color tmp = Color.Gray;
            if (_bindsBrushColor.ContainsKey(mc))
                tmp = _bindsBrushColor[mc];
            if (!_bindsBrushColor.ContainsKey(mc) && (_bindsBrushColor.ContainsKey(MapColors.MapBackground)))
                tmp = Color.FromArgb(255 - _bindsBrushColor[MapColors.MapBackground].R,
                    255 - _bindsBrushColor[MapColors.MapBackground].G,
                    255 - _bindsBrushColor[MapColors.MapBackground].B);
            return tmp;
        }
        public Brush GetBrush(MapColors mc)
        {
            Color tmp = GetColor(mc);
            if (_bindsBrushStyle.ContainsKey(mc))
                return new HatchBrush(_bindsBrushStyle[mc], tmp,Color.Transparent);
            else
                return new SolidBrush(tmp);
        }

        public void BindsFont_LoadDefaults()
        {
            _bindsFont = new Dictionary<MapFonts, Font>();
            _bindsFont.Add(MapFonts.QuadrantText, new Font(FontFamily.GenericSansSerif, 18));
            _bindsFont.Add(MapFonts.ObjectText, new Font(FontFamily.GenericSansSerif, 16));
            _bindsFont.Add(MapFonts.FleetText, new Font(FontFamily.GenericSansSerif, 8));
        }

        public Font GetFont(MapFonts mf)
        {
            if (_bindsFont.ContainsKey(mf))
                return _bindsFont[mf];
            return new Font(FontFamily.GenericMonospace, 24);
        }

        static Settings()
        {
            Current = new Settings();
            VesselData = new VesselData();
        }

        public Settings()
        {
			_version = Settings.LastVersion;

            BindsBrush_LoadDefaults();
            BindsFont_LoadDefaults();
            BindsKeyboard_LoadDefaults();

            DefaultVesselDataPath = @"..\dat\vesselData.xml";
            
            AddFailureComments = false;
            InsertNewOverElement = true;
            DragIntoFolderToLastPosition = true;
			DefaultSpaceMapScalePadded = true;
			UseYForNameless = true;
			UseYForNamed = true;
			UseYForSelection = true;
			YScale = 7.0;
            ShowLabelNumbers = false;
            NamesPerSubmenu = 15;
			MarkWhitespaceNamesOnSpaceMap = false;
			FocusOnStatementPaste = false;
			ShowStartStatementsInBackground = true;
			UseGenericMeshColor = true;
			MinimalLuminance = 0.05;
			MinimalSpaceMapSize = 100;
			MaximalSpaceMapSize = 15000;

			//Version 2
			SelectLabelWhenUsingPlusMinus = false;

			//Version 3
			ClearStartNodeOnDelete = true;
			AutoSaveInterval = 5;
			AutoSaveFilesCount = 10;

			NewMissionStartBlock = "<create type=\"player\" name = \"Artemis\" x=\"50000\" y=\"0\" z=\"50000\"/>\r\n<set_difficulty_level value=\"5\"/>\r\n<set_skybox_index index=\"9\"/><big_message title=\"Unnamed mission\" subtitle1=\"by Unknown Author\" subtitle2=\"adventure for Artemis 2.1\"/>\r\n<set_timer name=\"start_mission_timer_1\" seconds=\"10\"/>\r\n<set_variable name=\"chapter_1\" value=\"1\"/>";

			if (Program.FM!=null)
				Program.FM.SubscribeToVesselDataUpdates(VesselData);
        }

        public void UpdateSettings()
        {
			if (_version > LastVersion)
			{
				_version = 0;
				Log.Add("Read unknown settings version "+_version);
			}
			for (; _version++ < LastVersion; )
            {
                Log.Add("Updating settings to version "+_version.ToString());
                switch(_version)
                {
                    //Forgot what changed here so will just fix everything in bulk
                    case 1:
                        BindsBrush_LoadDefaults();
                        BindsFont_LoadDefaults();
                        BindsKeyboard_LoadDefaults();
						UseGenericMeshColor = true;
						MinimalLuminance = 0.05;
						MinimalSpaceMapSize = 100;
						MaximalSpaceMapSize = 15000;
						NewMissionStartBlock = "<create type=\"player\" name = \"Artemis\" x=\"50000\" y=\"0\" z=\"50000\"/>\r\n<set_difficulty_level value=\"5\"/>\r\n<set_skybox_index index=\"9\"/><big_message title=\"Unnamed mission\" subtitle1=\"by Unknown Author\" subtitle2=\"adventure for Artemis 2.1\"/>\r\n<set_timer name=\"start_mission_timer_1\" seconds=\"10\"/>\r\n<set_variable name=\"chapter_1\" value=\"1\"/>";
						UseYForSelection = true;
                        break;
					case 2:
						BindsKeyboard_LoadDefaults();
						BindsBrush_LoadDefaults();
						SelectLabelWhenUsingPlusMinus = false;
						break;
					case 3:
						ClearStartNodeOnDelete = true;
						AutoSaveInterval = 5;
						AutoSaveFilesCount = 10;
						break;
                }
            }
			_version = LastVersion;
        }
    }
}
