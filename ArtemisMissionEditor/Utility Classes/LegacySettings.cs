using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Windows.Forms;
using ArtemisMissionEditor.SpaceMap;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ArtemisMissionEditor
{
    public sealed class LegacySettingsSerializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            typeName = typeName.Replace("ArtemisMissionEditor.Settings", "ArtemisMissionEditor.LegacySettings");
            typeName = typeName.Replace("ArtemisMissionEditor.SpaceMapKeyCommands", "ArtemisMissionEditor.SpaceMap.KeyCommands");
            typeName = typeName.Replace("ArtemisMissionEditor.SpaceMapColors", "ArtemisMissionEditor.SpaceMap.MapColors");
            typeName = typeName.Replace("ArtemisMissionEditor.SpaceMapFonts", "ArtemisMissionEditor.SpaceMap.MapFonts");
            //assemblyName = assemblyName.Replace("MyNamespace", "MyNamespace.Class");
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }

    /// <summary>
    /// Class used to deserialize settings from 2.1.12 or older versions (prior to code rework)
    /// </summary>
    [Serializable]
    class LegacySettings
    {
        public static readonly int LastVersion = 6;

        public int _version = -1;

        #region Miscellaneous Settings (edited in PropertyGrid)

        public double FormOpacity { get { return _formOpacity; } set { _formOpacity = Math.Max(0.1, Math.Min(1.0, value)); } }
        private double _formOpacity;

        public bool AddFailureComments { get; set; }

        public bool InsertNewOverElement { get; set; }

        public bool DragIntoFolderToLastPosition { get; set; }

        public bool DefaultSpaceMapScalePadded { get; set; }

        public bool UseYForNameless { get; set; }

        public bool UseYForNamed { get; set; }

        public bool UseYForSelection { get; set; }

        public double YScale { get; set; }

        public bool ShowLabelNumbers { get; set; }

        public int NamesPerSubmenu { get; set; }

        public bool MarkWhitespaceNamesOnSpaceMap { get; set; }

        public bool FocusOnStatementPaste { get; set; }

        public bool ShowStartStatementsInBackground { get; set; }

        public bool UseGenericMeshColor { get; set; }

        public double MinimalLuminance { get; set; }

        public int MinimalSpaceMapSize { get; set; }

        public int MaximalSpaceMapSize { get; set; }

        public bool SelectLabelWhenUsingPlusMinus { get; set; }

        public bool ClearStartNodeOnDelete { get; set; }

        public int AutoSaveInterval { get { return _autoSaveInterval; } set { _autoSaveInterval = Math.Max(0, value); } }
        private int _autoSaveInterval;

        public int AutoSaveFilesCount { get { return _autoSaveFilesCount; } set { _autoSaveFilesCount = Math.Max(1, value); } }
        private int _autoSaveFilesCount;

        public bool TreeViewFlickeringFix { get { return _treeViewFlickeringFix; } set { _treeViewFlickeringFix = value; } }
        private bool _treeViewFlickeringFix;

        public int AmountOfUndoEntriesToKeep { get { return _amountOfUndoEntriesToKeep; } set { _amountOfUndoEntriesToKeep = Math.Max(0, value); } }
        private int _amountOfUndoEntriesToKeep;

        public Font LabelFont { get { return _labelFont; } set { _labelFont = value; } }
        private Font _labelFont;

        #endregion

        #region Special Settings (edited separately)

        public string NewMissionStartBlock { get; set; }

        public string[] PlayerShipNames { get; set; }

        public string DefaultVesselDataPath { get; set; }

        #endregion

        #region Bindings (some are edited in special PropertyGrids, others are readonly)

        //Bindings
        private Dictionary<Keys, KeyCommands> _bindsKeyboardKeys = null;
        private List<KeyCommands> _bindsKeyboardCommandsGlobal = null;
        private Dictionary<MapColors, Color> _bindsBrushColor = null;
        private Dictionary<MapColors, HatchStyle> _bindsBrushStyle = null;
        private Dictionary<MapFonts, Font> _bindsFont = null;

        #endregion

        public Settings ToSettings()
        {
            Settings result = new Settings(_version);

            result.AddFailureComments = AddFailureComments;
            result.AmountOfUndoEntriesToKeep = AmountOfUndoEntriesToKeep;
            result.AutoSaveFilesCount = AutoSaveFilesCount;
            result.AutoSaveInterval = AutoSaveInterval;
            result.ClearStartNodeOnDelete = ClearStartNodeOnDelete;
            result.DefaultSpaceMapScalePadded = DefaultSpaceMapScalePadded;
            result.DefaultVesselDataPath = DefaultVesselDataPath;
            result.DragIntoFolderToLastPosition = DragIntoFolderToLastPosition;
            result.FocusOnStatementPaste = FocusOnStatementPaste;
            result.FormOpacity = FormOpacity;
            result.InsertNewOverElement = InsertNewOverElement;
            result.LabelFont = LabelFont;
            result.MarkWhitespaceNamesOnSpaceMap = MarkWhitespaceNamesOnSpaceMap;
            result.MaximalSpaceMapSize = MaximalSpaceMapSize;
            result.MinimalLuminance = MinimalLuminance;
            result.MinimalSpaceMapSize = MinimalSpaceMapSize;
            result.NamesPerSubmenu = NamesPerSubmenu;
            result.NewMissionStartBlock = NewMissionStartBlock;
            result.PlayerShipNames = PlayerShipNames;
            result.SelectLabelWhenUsingPlusMinus = SelectLabelWhenUsingPlusMinus;
            result.ShowLabelNumbers = ShowLabelNumbers;
            result.ShowStartStatementsInBackground = ShowStartStatementsInBackground;
            result.TreeViewFlickeringFix = TreeViewFlickeringFix;
            result.UseGenericMeshColor = UseGenericMeshColor;
            result.UseYForNamed = UseYForNamed;
            result.UseYForNameless = UseYForNameless;
            result.UseYForSelection = UseYForSelection;
            result.YScale = YScale;

            result.AssignDictionariesFromLegacySettings(
                _bindsKeyboardKeys, 
                _bindsKeyboardCommandsGlobal,
                _bindsBrushColor, 
                _bindsBrushStyle, 
                _bindsFont);

            return result;     
        }
    }
}
