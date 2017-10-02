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
using ArtemisMissionEditor.SpaceMap;
using System.Runtime.Serialization;
using System.Reflection;

namespace ArtemisMissionEditor
{
    /// <summary>
    /// Contains all settings that are (mostly) user-editable.
    /// </summary>
    /// <remarks>
    /// NOTE TO SELF: 
    /// Never use auto-properties in a serializable class! 
    /// Because you can't change them to real properties later!
    /// </remarks>
    [Serializable]
    public sealed partial class Settings
    {
        private static readonly string FileName = "settings.binary";
        private static readonly int LastVersion = 8;
        
        public static readonly string ProgramDataFolder = @"%appdata%\ArtemisMissionEditor\";
        
        private static Settings _current = new Settings();
        [Browsable(false)]
        public static Settings Current { get { return _current; } set { _current = value; OnSettingsCurrentChanged(); } }

        public static event Action SettingsCurrentChanged;
        private static void OnSettingsCurrentChanged()
        {
            if (SettingsCurrentChanged != null)
                SettingsCurrentChanged();
            OnSettingAutoSaveIntervalChanged();
        }

        public static event Action SettingAutoSaveIntervalChanged;
        private static void OnSettingAutoSaveIntervalChanged()
        {
            if (SettingAutoSaveIntervalChanged != null)
                SettingAutoSaveIntervalChanged();
        }
        
        #region Miscellaneous Settings (edited in PropertyGrid)

        private int _version;

        [DisplayName("Form opacity"), Description("Additional forms that are not focused will be transparent if this value is set below 1."), DefaultValue(1.0)]
        public double FormOpacity
        {
            get { return _formOpacity; }
            set { _formOpacity = Math.Max(0.1, Math.Min(1.0, value)); Program.UpdateAllFormsOpacity(); }
        }
        private double _formOpacity;

        [DisplayName("Add failure comments"), Description("Add comments about failure when saving create statements from the space map."), DefaultValue(false)]
        public bool AddFailureComments { get { return _addFailureComments; } set { _addFailureComments = value; } }
        public bool _addFailureComments;

        /// <summary>
        /// When inserting new node, insert it over or under the selected element
        /// </summary>
        [DisplayName("Insert new element over selected element"), Description("Whether to insert new elements over selected elements or under selected elements in the tree view."), DefaultValue(true)]
        public bool InsertNewOverElement { get { return _insertNewOverElement; } set { _insertNewOverElement = value; } }
        public bool _insertNewOverElement;

        /// <summary>
        /// When moving node inside node, it should end up at last or first position
        /// </summary>
        [DisplayName("Place nodes dragged inside a folder to the last position"), Description("Whether the nodes dragged into a folder should be put to the last position or to the first position."), DefaultValue(true)]
        public bool DragIntoFolderToLastPosition { get { return _dragIntoFolderToLastPosition; } set { _dragIntoFolderToLastPosition = value; } }
        public bool _dragIntoFolderToLastPosition;

		/// <summary>
		/// Whether the default (`) map scale will be padded or zommed in 100%
		/// </summary>
        [DisplayName("Space map is padded by default"), Description("If true, space map will be padded (fully zoomed out) when reset key is pressed or space map is opened.\r\nIf false, space map will instead be fit into window ."), DefaultValue(true)]
        public bool DefaultSpaceMapScalePadded { get { return _defaultSpaceMapScalePadded; } set { _defaultSpaceMapScalePadded = value; } }
        public bool _defaultSpaceMapScalePadded;

		/// <summary>
		/// Show y-coordinate lines for nameless objects on the space map
		/// </summary>
        [DisplayName("Use Y for nameless"), Description("Consider Y coordinate when displaying nameless (nebulas, asteroids, mines) objects on the space map.\r\nIf true, objects with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
        public bool UseYForNameless { get { return _useYForNameless; } set { _useYForNameless = value; } }
        public bool _useYForNameless;

		/// <summary>
		/// Show y-coordinate lines for named objects on the space map
		/// </summary>
        [DisplayName("Use Y for named"), Description("Consider Y coordinate when displaying named (enemies, stations, ...) objects on the space map.\r\nIf true, objects with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
        public bool UseYForNamed { get { return _useYForNamed; } set { _useYForNamed = value; } }
        public bool _useYForNamed;

		/// <summary>
		/// Show y-coordinate lines for named objects on the space map
		/// </summary>
		[DisplayName("Use Y for selection"), Description("Consider Y coordinate when displaying selections like spheres.\r\nIf true, selections that have y-coordinate with nonzero-Y coordinate will appear with guidelines showing their distance to the zero Y plane."), DefaultValue(true)]
        public bool UseYForSelection { get { return _useYForSelection; } set { _useYForSelection = value; } }
        public bool _useYForSelection;

		/// <summary>
		/// How big are the y-coord lines on the space map
		/// </summary>
        [DisplayName("Y scale"), Description("Object's Y value is multiplied by this value when it is displayed on the space map.\r\nEx: if Y scale = 7, objects with X=0 Y=1000 Z=0 and X=0 Y=0 Z=7000 will be displayed on the screen in the same position."), DefaultValue(7.0)]
        public double YScale { get { return _yScale; } set { _yScale = value; } }
        public double _yScale;

        /// <summary>
        /// Show numbers near labels for expression editing
        /// </summary>
        [DisplayName("Show label numbers"), Description("Show hotkey numbers near expression labels."), DefaultValue(false)]
        public bool ShowLabelNumbers { get { return _showLabelNumbers; } set { _showLabelNumbers = value; } }
        public bool _showLabelNumbers;

        /// <summary>
        /// Amount of names displayed in one submenu for timers and variables
        /// </summary>
        [DisplayName("Names per submenu"), Description("Amount of names to be showed per submenu when showing lists of names (of timers, values, or named objects)."), DefaultValue(15)]
        public int NamesPerSubmenu { get { return _namesPerSubmenu; } set { _namesPerSubmenu = value; } }
        public int _namesPerSubmenu;

		/// <summary>
		/// Shall the spacemap display names consisting of whitespaces decorated with angle brackets
		/// </summary>
        [DisplayName("Mark whitespace names on space map"), Description("Mark object names consisting only of whitespaces with <> brackets."), DefaultValue(false)]
        public bool MarkWhitespaceNamesOnSpaceMap { get { return _markWhitespaceNamesOnSpaceMap; } set { _markWhitespaceNamesOnSpaceMap = value; } }
        public bool _markWhitespaceNamesOnSpaceMap;

		/// <summary>
		/// Focus statement window when pasting into selected node
		/// </summary>
        [DisplayName("Focus statements when pasting into node"), Description("When pasting a statement while focus is on a node, focus the newly inserted statement."), DefaultValue(false)]
        public bool FocusOnStatementPaste { get { return _focusOnStatementPaste; } set { _focusOnStatementPaste = value; } }
        public bool _focusOnStatementPaste;

		/// <summary>
		/// Show start node's statements in background when editing on space map
		/// </summary>
        [DisplayName("Show background statements on space map"), Description("Whether to show statements from the start block (or chosen event) in the background when editing objects on the space map."), DefaultValue(true)]
        public bool ShowStartStatementsInBackground { get { return _showStartStatementsInBackground; } set { _showStartStatementsInBackground = value; } }
        public bool _showStartStatementsInBackground;

		/// <summary>
		/// Whether top use generic mesh's color when drawing it on space map
		/// </summary>
		[DisplayName("Use generic mesh color"), Description("Use generic mesh color when drawing generic mesh on space map."), DefaultValue(true)]
        public bool UseGenericMeshColor { get { return _useGenericMeshColor; } set { _useGenericMeshColor = value; } }
        public bool _useGenericMeshColor;

		/// <summary>
		/// If UseGenericMeshColor is true, this is the minimal luminance, beyond which generic mesh's color will be used.
		/// </summary>
		[DisplayName("Minimal luminance"), Description("If use generic mesh color is true then what is the minimal luminance to use generic mesh color instead of basic color (set this higher to prevent very dark generic meshes from being invisible).\r\nValid values from 0 to 1."), DefaultValue(0.05)]
        public double MinimalLuminance { get { return _minimalLuminance; } set { _minimalLuminance = value; } }
        public double _minimalLuminance;

		/// <summary>
		/// Minimum possible space map size, in pixels (cannot zoom out further than this).
		/// </summary>
		[DisplayName("Space map min size"), Description("Minimal possible space map size, in pixels"), DefaultValue(100)]
        public int MinimalSpaceMapSize { get { return _minimalSpaceMapSize; } set { _minimalSpaceMapSize = value; } }
        public int _minimalSpaceMapSize;

		/// <summary>
		/// Maximum possible space map size, in pixels (cannot zoom in further than this).
		/// </summary>
		[DisplayName("Space map max size"), Description("Maximal possible space map size, in pixels.\r\nIf you see a lot of blinking when zoomed to the max, but not when zooming to a lesser degree, and moving map around, you can reduce this value to prevent zooming that far."), DefaultValue(15000)]
        public int MaximalSpaceMapSize { get { return _maximalSpaceMapSize; } set { _maximalSpaceMapSize = value; } }
        public int _maximalSpaceMapSize;

		/// <summary>
        /// When using plus/minus keys to edit statement values, while focus is on the statement tree view, should the label that was changed be selected or not.
		/// </summary>
		[DisplayName("Select label when using plus/minus"), Description("When using plus/minus keys to edit statement values, while focus is on the statement tree view, should the label that was changed be selected or not."), DefaultValue(false)]
        public bool SelectLabelWhenUsingPlusMinus { get { return _selectLabelWhenUsingPlusMinus; } set { _selectLabelWhenUsingPlusMinus = value; } }
        public bool _selectLabelWhenUsingPlusMinus;

		/// <summary>
		/// Clear start node contents when delete command is called on it. If false, nothing happens when delete is called on start node.
		/// </summary>
		[DisplayName("Clear start node's contents on delete"), Description("Clear start node contents when delete command is called on it. If true, attempt to delete start node (alone or  If false, start node is unaffected by delete."), DefaultValue(true)]
        public bool ClearStartNodeOnDelete { get { return _сlearStartNodeOnDelete; } set { _сlearStartNodeOnDelete = value; } }
        public bool _сlearStartNodeOnDelete;

		/// <summary>
        /// Defines interval (in minutes) between autosaves. 0 means autosave is disabled.
        /// </summary>
        [DisplayName("Autosave interval"), Description("Defines interval (in minutes) between autosaves. Autosave saves current mission state to a file in program folder (%APPDATA%\\ArtemisMissionEditor\\). You can manually load mission file from that folder if you need, like a program crashes or computer reboots unexpectedly. 0 means autosave is disabled."), DefaultValue(5)]
        public int AutoSaveInterval
        {
            get { return _autoSaveInterval; }
            set { _autoSaveInterval = Math.Max(0, value); if (this == Current) OnSettingAutoSaveIntervalChanged(); }
        }
        private int _autoSaveInterval;
        
		/// <summary>
        /// Defines amount of autosave files that would be kept.
		/// </summary>
		[DisplayName("Autosave files count"), Description("Defines amount of autosave files that would be kept. If more than this amount of autosave files are present in the program folder, oldest file will be deleted."), DefaultValue(10)]
        public int AutoSaveFilesCount
        {
            get { return _autoSaveFilesCount; }
            set { _autoSaveFilesCount = Math.Max(1, value); }
        }
        private int _autoSaveFilesCount;
		
        /// <summary>
        /// Removes flickering when redrawing tree view controls, however, it can introduce artifacts when scrolling control while dragging nodes.
        /// </summary>
        [DisplayName("Tree View flickering fix"), Description("[Requires Restart] Removes flickering when redrawing tree view controls, however, it can introduce artifacts when scrolling control while dragging nodes."), DefaultValue(true)]
        public bool TreeViewFlickeringFix { get { return _treeViewFlickeringFix; } set { _treeViewFlickeringFix = value; } }
        private bool _treeViewFlickeringFix;

        /// <summary>
        /// How many undo entries do you want to keep when you save your mission.
        /// </summary>
        [DisplayName("Amount of undo entries to keep upon saving"), Description("How many undo entries do you want to keep when you save your mission."), DefaultValue(0)]
        public int AmountOfUndoEntriesToKeep { get { return _amountOfUndoEntriesToKeep; } set { _amountOfUndoEntriesToKeep = Math.Max(0, value); } }
        private int _amountOfUndoEntriesToKeep;

        /// <summary>
        /// The font used to display currently selected statement (in the bottom right of the main form).
        /// </summary>
        [DisplayName("Font for current statement"), Description("The font used to display currently selected statement (in the bottom right of the main form)."), DefaultValue(typeof(Font), DefaultLabelFont)]
        public Font LabelFont { get { return _labelFont; } set { _labelFont = value; } }
        private Font _labelFont;
        private const string DefaultLabelFont = "Segoe UI, 16pt";

		#endregion

        #region Special Settings (edited separately)

        /// <summary>
        /// Xml that gets added into a Start block when new mission is created.
        /// </summary>
		[Browsable(false)]
        public string NewMissionStartBlock { get { return _newMissionStartBlock; } set { _newMissionStartBlock = value; } }
        public string _newMissionStartBlock;

        /// <summary>
        /// List of default player ship names.
        /// </summary>
        [Browsable(false)]
        public string[] PlayerShipNames { get { return _playerShipNames; } set { _playerShipNames = value; } }
        public string[] _playerShipNames;

        /// <summary>
        /// Path to vesselData.xml (loaded from this path at startup).
        /// </summary>
        [Browsable(false)]
        public string DefaultVesselDataPath { get { return _defaultVesselDataPath; } set { _defaultVesselDataPath = value; } }
        public string _defaultVesselDataPath;

        #endregion

        #region Bindings (some are edited in special PropertyGrids, others are readonly)

        //Bindings
        private Dictionary<Keys, KeyCommands> _bindsKeyboardKeys;
        private List<KeyCommands> _bindsKeyboardCommandsGlobal;
        private Dictionary<MapColors, Color> _bindsBrushColor;
        private Dictionary<MapColors, HatchStyle> _bindsBrushStyle;
        private Dictionary<MapFonts, Font> _bindsFont;

        public void AssignDictionariesFromLegacySettings(
            Dictionary<Keys, KeyCommands> bindsKeyboardKeys,
            List<KeyCommands> bindsKeyboardCommandsGlobal,
            Dictionary<MapColors, Color> bindsBrushColor,
            Dictionary<MapColors, HatchStyle> bindsBrushStyle,
            Dictionary<MapFonts, Font> bindsFont)
        {
            _bindsKeyboardKeys = bindsKeyboardKeys;
            _bindsKeyboardCommandsGlobal = bindsKeyboardCommandsGlobal;
            _bindsBrushColor = bindsBrushColor;
            _bindsBrushStyle = bindsBrushStyle;
            _bindsFont = bindsFont;
        }

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

        public DictionaryPropertyGridAdapter GetAdaptedBrushColorDictionary()
        {
            return new DictionaryPropertyGridAdapter(_bindsBrushColor);
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
            return _defaultFont;
        }
        private static Font _defaultFont = new Font(FontFamily.GenericMonospace, 24);

        #endregion

        public static void EnsureProgramDataFolderExists()
        {
            if (!Directory.Exists(Environment.ExpandEnvironmentVariables(ProgramDataFolder)))
                Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(ProgramDataFolder));
        }

        public static bool Load()
        {
            string fileName = Environment.ExpandEnvironmentVariables(ProgramDataFolder + FileName);
            try
            {
                try
                {
                    BinaryFormatter iser = new BinaryFormatter();
                    using (FileStream istream = new FileStream(fileName, FileMode.Open))
                        Settings.Current = (Settings)(iser.Deserialize(istream));
                }
                catch
                {
                    BinaryFormatter iser = new BinaryFormatter();
                    iser.Binder = new LegacySettingsSerializationBinder();
                    LegacySettings legacySettings;
                    using (FileStream istream = new FileStream(fileName, FileMode.Open))
                        legacySettings = (LegacySettings)(iser.Deserialize(istream));
                    Settings.Current = legacySettings.ToSettings();
                }
                if (Settings.Current._version != Settings.LastVersion)
                {
                    Settings.Current.InitializeSettings(true);
                    Settings.Save();
                }

                Program.UpdateAllFormsOpacity();

                return true;

            }
            catch (Exception ex)
            {
                Log.Add("Loading settings file has failed with the following exception:");
                Log.Add(ex);

                return false;
            }
        }

        public static bool Save()
        {
            string fileName = Environment.ExpandEnvironmentVariables(ProgramDataFolder + FileName);
            FileStream ostream = null;
            try
            {
                BinaryFormatter oser = new BinaryFormatter();
                EnsureProgramDataFolderExists();
                ostream = new FileStream(fileName, FileMode.Create);
                oser.Serialize(ostream, Settings.Current);
                ostream.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (ostream != null)
                    ostream.Close();
                Log.Add("Creating settings file has failed with the following exception:");
                Log.Add(ex);
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

        public Settings()
        {
            _version = -1;
            InitializeSettings();
        }

        public Settings(int version)
        {
            _version = version;
        }

        public void InitializeSettings(bool verbose = false)
        {
			if (_version > LastVersion && verbose)
				Log.Add("Encountered unknown settings version "+_version);
			for (; _version++ < LastVersion; )
            {
                if (verbose) Log.Add("Updating settings to version " + _version.ToString());
                switch(_version)
                {
                    case 0:
                        BindsFont_LoadDefaults();
                        DefaultVesselDataPath = @"..\dat\vesselData.xml";
                        AddFailureComments = false;
                        InsertNewOverElement = true;
                        DragIntoFolderToLastPosition = true;
			            DefaultSpaceMapScalePadded = true;
			            UseYForNameless = true;
			            UseYForNamed = true;
			            YScale = 7.0;
                        ShowLabelNumbers = false;
                        NamesPerSubmenu = 15;
			            MarkWhitespaceNamesOnSpaceMap = false;
			            FocusOnStatementPaste = false;
			            ShowStartStatementsInBackground = true;
                        break;
                    case 1:
                        BindsBrush_LoadDefaults();
                        BindsKeyboard_LoadDefaults();
						UseGenericMeshColor = true;
						MinimalLuminance = 0.05;
						MinimalSpaceMapSize = 100;
						MaximalSpaceMapSize = 15000;
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
                    case 4:
                        FormOpacity = 1.0;
                        PlayerShipNames = new string[]{ 
                            "Artemis",
                            "Intrepid",
                            "Aegis",
                            "Horatio",
                            "Excalibur",
                            "Hera",
                            "Ceres",
                            "Diana",
                                };
                        break;
                    case 5:
                        _treeViewFlickeringFix = true;
                        _amountOfUndoEntriesToKeep = 0;
                        break;
                    case 6:
                        _labelFont = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromInvariantString(DefaultLabelFont);
                        break;
                    case 7:
                        NewMissionStartBlock = "<create type=\"player\" player_slot=\"\" x=\"50000\" y=\"0\" z=\"50000\" name=\"Artemis\" />\r\n<set_difficulty_level value=\"5\" />\r\n<set_skybox_index index=\"10\" />\r\n<set_object_property property=\"nebulaIsOpaque\" value=\"0\" />\r\n<set_object_property property=\"sensorSetting\" value=\"1\" />\r\n<set_object_property property=\"nonPlayerSpeed\" value=\"100\" />\r\n<set_object_property property=\"nonPlayerShield\" value=\"100\" />\r\n<set_object_property property=\"nonPlayerWeapon\" value=\"100\" />\r\n<set_object_property property=\"playerWeapon\" value=\"100\" />\r\n<set_object_property property=\"playerShields\" value=\"100\" />\r\n<big_message title=\"Unnamed mission\" subtitle1=\"by Unknown Author\" subtitle2=\"adventure for Artemis 2.6\" /><set_timer name=\"start_mission_timer_1\" seconds=\"10\" />\r\n<set_variable name=\"chapter_1\" value=\"1\" />";
                        break;
                    case 8:
                        // Transition from old format
                        break;
                }
            }
			_version = LastVersion;
        }
    }
}
