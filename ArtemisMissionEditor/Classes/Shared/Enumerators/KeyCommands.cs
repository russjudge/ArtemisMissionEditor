using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor
{
    /// <summary>
    /// List of actions available to the user via keyboard or mouse commands
    /// </summary>
    public enum KeyCommands
    {
        KeyCommands_Nothing,
        ZoomMinus,
        ZoomPlus,
        MoveRight,
        MoveLeft,
        MoveUp,
        MoveDown,
        MoveSelectionRight,
        MoveSelectionLeft,
        MoveSelectionUp,
        MoveSelectionDown,
        MoveSelectionFastRight,
        MoveSelectionFastLeft,
        MoveSelectionFastUp,
        MoveSelectionFastDown,
        ResetMap,
        AddStation,
        AddPlayer,
        AddEnemy,
        AddNeutral,
        AddAnomaly,
        AddBlackHole,
        AddMonster,
        AddGenericMesh,
		AddWhales,
		AddNebulas,
        AddAsteroids,
        AddMines,
        DeleteSelected,
        MoveSelected,
        SelectAll,
        SelectNone,
        SelectNextNamed,
        SelectPreviousNamed,
        SelectNextNameless,
        SelectPreviousNameless,
        SetStartAnglePositive,
        SetStartAngleNegative,
        SetStartAngleNull,
        SetEndAnglePositive,
        SetEndAngleNegative,
        SetEndAngleNull,
        FinishEditing,
        FinishEditingForceYes,
        FinishEditingForceNo,
        FocusMap,
    }
}
