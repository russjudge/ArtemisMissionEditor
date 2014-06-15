using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtemisMissionEditor
{
    public enum PanelSpaceMapMode
    {
        /// <summary>
        /// Normal mode - add create statement objects, named or nameless, or edit them
        /// </summary>
        Normal,
        /// <summary>
        /// Single special mode - edit single special object, specified before map is opened, used when editing object locations, rectangles, spheres etc.
        /// </summary>
        SingleSpecial
    }
}