using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor.SpaceMap
{
    public sealed class Space
    {
        #region Constants

        //Coordinates and angle in space
        public static readonly int MinX = 0;
        public static readonly int MaxX = 100000;
        public static readonly int MinY = -100000;
        public static readonly int MaxY = 100000;
        public static readonly int MinZ = 0;
        public static readonly int MaxZ = 100000;
        public static readonly int PaddingZ = 10000;
        public static readonly int PaddingX = 10000;
        public static readonly int NebulaWidth = 4000;
        public static readonly int AsteroidWidth = 300;
        public static readonly int MineWidth = 250;

        #endregion

        #region Helper Functions

        public  int PointXToScreen(int surface_Width, int space_map_x)
		{
			return (int)(((long)space_map_x + (long)Space.PaddingX) * (long)surface_Width / ((long)Space.MaxX + (long)Space.PaddingX * 2));
		}
                
		public  int PointZToScreen(int surface_Height, int space_map_z)
		{
			return (int)(((long)space_map_z + (long)Space.PaddingZ) * (long)surface_Height / ((long)Space.MaxZ + (long)Space.PaddingZ * 2));
		}
                
		public  double PointXToScreen(double surface_Width, double space_map_x)
		{
			return (space_map_x + Space.PaddingX) * surface_Width / (Space.MaxX + Space.PaddingX * 2);
		}
                
		public  double PointZToScreen(double surface_Height, double space_map_z)
		{
			return (space_map_z + Space.PaddingZ) * surface_Height / (Space.MaxZ + Space.PaddingZ * 2);
		}
                
		public  int PointXToSpaceMap(int surface_Width, int screen_x)
		{
			return (int)(((long)screen_x) * ((long)Space.MaxX + (long)Space.PaddingX * 2) / (long)surface_Width - (long)Space.PaddingX);
		}
                
		public  int PointZToSpaceMap(int surface_Height, int screen_z)
		{
			return (int)(((long)screen_z) * ((long)Space.MaxZ + (long)Space.PaddingZ * 2) / (long)surface_Height - (long)Space.PaddingZ);
		}
                
		public  double PointXToSpaceMap(double surface_Width, double screen_x)
		{
			return (screen_x) * (Space.MaxX + Space.PaddingX * 2) / surface_Width - Space.PaddingX;
		}
                
		public  double PointZToSpaceMap(double surface_Height, double screen_z)
		{
			return (screen_z) * (Space.MaxZ + Space.PaddingZ * 2) / surface_Height - Space.PaddingZ;
		}

		#endregion

		public bool ChangesPending { get; set; }

		/// <summary> List of ID's of named create statements that were imported </summary>
        public  List<int> NamedIdList;
        /// <summary> List of ID's of nameless create statements that were imported </summary>
        public  List<int> NamelessIdList;
                
        public  List<MapObjectNamed> NamedObjects;
        public  List<MapObjectNameless> NamelessObjects;
                
		public  List<MapObjectNamed> BacgkroundNamedObjects;
		public  List<MapObjectNameless> BackgroundNamelessObjects;
                
        public  int Selection_Count
        {
            get
            {
                int value = 0;
                foreach (MapObjectNamed item in NamedObjects)
                    value += (item.Selected) ? 1 : 0;
                return value;
            }
        }
                
        public MapObjectNamed[] GetSelectionNamed()
        {
            List<MapObjectNamed> value = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
                if (item.Selected)
                    value.Add(item); 
            return value.ToArray();
        }

        public  MapObjectNameless SelectionNameless
        {
            get { return _selectionNameless; }
            set { if (value != null) Select_None(); if(value!=null && !NamelessObjects.Contains(value)) throw new ArgumentOutOfRangeException("Attempting to select a nonexistant nameless object"); _selectionNameless = value; }
        }
        private MapObjectNameless _selectionNameless;
        
        public MapObjectSpecial SelectionSpecial { get; set; }

        public  object AddObject(string type, int posX = 0, int posY = 0, int posZ = 0, int angle = 0, bool makeSelected = false)
        {
            MapObjectNamed mo = null;
            MapObjectNameless nmo = null;

            switch (type)
            {
                case "station":
                    mo = new MapObjectNamed_station(posX, posY, posZ, makeSelected, angle);
                    break;
                case "neutral":
                    mo = new MapObjectNamed_neutral(posX, posY, posZ, makeSelected, angle);
                    break;
                case "player":
                    mo = new MapObjectNamed_player(posX, posY, posZ, makeSelected, angle);
                    break;
                case "monster":
                    mo = new MapObjectNamed_monster(posX, posY, posZ, makeSelected, angle);
                    break;
                case "Anomaly":
                    mo = new MapObjectNamed_Anomaly(posX, posY, posZ, "", makeSelected);
                    break;
                case "blackHole":
                    mo = new MapObjectNamed_blackHole(posX, posY, posZ, "", makeSelected);
                    break;
                case "enemy":
                    mo = new MapObjectNamed_enemy(posX, posY, posZ, makeSelected, angle);
                    break;
                case "genericMesh":
                    mo = new MapObjectNamed_genericMesh(posX, posY, posZ, makeSelected, angle);
                    break;
				case "whale":
					mo = new MapObjectNamed_whale(posX, posY, posZ, makeSelected, angle);
					break;
				case "nameless":
                    nmo = new MapObjectNameless(posX, posY, posZ, "");
                    break;
                case "nebulas":
                    nmo = new MapObjectNameless(posX, posY, posZ, "nebulas");
                    break;
                case "asteroids":
                    nmo = new MapObjectNameless(posX, posY, posZ, "asteroids");
                    break;
                case "mines":
                    nmo = new MapObjectNameless(posX, posY, posZ, "mines");
                    break;
                default:
                    break;
            }

            if (mo == null && nmo==null)
                throw new NotImplementedException("Attempting to add object of unknown type " + type);

            if (mo != null)
            {
                NamedObjects.Add(mo);
                return mo;
            }
            else
            {
                NamelessObjects.Add(nmo);
                if (makeSelected)
                {
                    Select_None();
                    SelectionNameless = nmo;
                }
                return nmo;
            }
        }
                
        public  bool Select_All()
        {
            bool changed = false;
            if (_selectionNameless != null)
            {
                changed = true;
                _selectionNameless = null;
            }

            foreach (MapObjectNamed item in NamedObjects)
            {
                changed = changed || !item.Selected;
                item.Selected = true;
            }
            return changed;
        }
                
        public  bool Select_AtPoint(int x, int z, int pixelsInOne, double y_scale = 0.0, bool additive = false, bool removative = false)
        {
            List<MapObjectNamed> objectsSelectedUnderCursor = new List<MapObjectNamed>();
            List<MapObjectNamed> objectsUnderCursor = new List<MapObjectNamed>();
            List<MapObjectNamed> objectsSelectedNotUnderCursor = new List<MapObjectNamed>();
            int i;
            bool changed = false;
            if (_selectionNameless != null)
            {
                changed = true;
                _selectionNameless = null;
            }

            foreach (MapObjectNamed item in NamedObjects)
            {
                if ((Math.Sqrt(Math.Pow(item.Coordinates.X_Scr - x, 2) + Math.Pow(item.Coordinates.Z_Scr - z, 2)) < item._selectionSize * (item._selectionAbsolute ? 1 : pixelsInOne))
                    || (Math.Sqrt(Math.Pow(item.Coordinates.X_Scr - x, 2) + Math.Pow(item.Coordinates.Z_Scr- item.Coordinates.Y_Scr* y_scale - z, 2)) < item._selectionSize * (item._selectionAbsolute ? 1 : pixelsInOne)))
                {
                    objectsUnderCursor.Add(item);
                    if (item.Selected)
                        objectsSelectedUnderCursor.Add(item);

                }
                else
                {
                    if (item.Selected)
                        objectsSelectedNotUnderCursor.Add(item);
                }
            }

            if (additive)
            {
                //if there are objects under cursor that are not selected and those that are => select them in usual order
                if (objectsUnderCursor.Count > objectsSelectedUnderCursor.Count && objectsSelectedUnderCursor.Count > 0)
                {
                    MapObjectNamed toBeSelected = null, cur = null;
                    i = 0;
                    bool foundSelected = false;
                    while (toBeSelected == null)
                    {
                        cur = objectsUnderCursor[i];
                        if (!cur.Selected && foundSelected)
                            toBeSelected = cur;
                        if (objectsSelectedUnderCursor.Contains(cur))
                            foundSelected = true;
                        i = (i + 1) % objectsUnderCursor.Count;
                    }
                    toBeSelected.Selected = true;
                    return true;
                }

                //if there are objects under cursor that are not selected and none under cursor are selected => select first from the list
                if (objectsUnderCursor.Count > 0 && objectsSelectedUnderCursor.Count == 0)
                {
                    objectsUnderCursor[0].Selected = true;
                    return true;
                }

                return changed;
            }

            if (removative)
            {
                if (objectsSelectedUnderCursor.Count > 0)
                {
                    objectsSelectedUnderCursor[0].Selected = false;
                    return true;
                }
                
                return changed;
            }

            //If more than one selected total and at least one selected under cursor => deselect everything and try again
            if (objectsSelectedUnderCursor.Count + objectsSelectedNotUnderCursor.Count > 1 && objectsSelectedUnderCursor.Count > 0)
            {
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                return Select_AtPoint(x, z, pixelsInOne);
            }

            //If there is something to select under cursor and nothing is selected there => just select first (and deselect everything else)
            if (objectsUnderCursor.Count > 0 && objectsSelectedUnderCursor.Count == 0)
            {
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                objectsUnderCursor[0].Selected = true;
                return true;
            }

            //If only one object under cursor and its already selected => do nothing, let it remain selected
            if (objectsUnderCursor.Count == 1 && objectsSelectedUnderCursor.Count == 1)
            {
                return changed;
            }

            //If there is something to select under cursor but something is already selected there = >  we will select the next unselected after a selected one
            if (objectsUnderCursor.Count > 0 && objectsSelectedUnderCursor.Count > 0)
            {
                MapObjectNamed toBeSelected = null, cur = null;
                i = 0;
                bool foundSelected= false; 
                while (toBeSelected == null)
                {
                    cur = objectsUnderCursor[i];
                    if (!cur.Selected && foundSelected)
                        toBeSelected = cur;
                    if (objectsSelectedUnderCursor.Contains(cur))
                        foundSelected = true;
                    i = (i + 1) % objectsUnderCursor.Count;
                }
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                toBeSelected.Selected = true;
                return true;
            }

            //If there is nothing to select under cursor but something selected elsewhere - clear it
            if (objectsSelectedNotUnderCursor.Count > 0)
            {
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                return true;
            }

            //If nothing is or was selected
            if (objectsSelectedNotUnderCursor.Count == 0 && objectsUnderCursor.Count == 0)
            {
                return changed;
            }
            
            throw new Exception("Select_AtPoint reached the end in normal mode! This means an error in function's logic.");
            //return false;
        }
                
        public  bool Select_InRectangle(int x1, int z1, int x2, int z2, int pixelsInOne, double y_scale = 0.0, bool additive = false, bool removative = false)
        {
            List<MapObjectNamed> objectsSelectedOutside = new List<MapObjectNamed>();
            List<MapObjectNamed> objectsInside = new List<MapObjectNamed>();
            List<MapObjectNamed> objectsSelectedInside = new List<MapObjectNamed>();
            int tmp;
            bool changed = false;
            if (_selectionNameless != null)
            {
                changed = true;
                _selectionNameless = null;
            }

            if (x1 > x2)
            {
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }

            if (z1 > z2)
            {
                tmp = z1;
                z1 = z2;
                z2 = tmp;
            }

            foreach (MapObjectNamed item in NamedObjects)
            {
                if ((item.Coordinates.X_Scr > x1 - pixelsInOne && item.Coordinates.X_Scr < x2 + pixelsInOne && item.Coordinates.Z_Scr > z1 - pixelsInOne && item.Coordinates.Z_Scr < z2 + pixelsInOne)
                    || (item.Coordinates.X_Scr > x1 - pixelsInOne && item.Coordinates.X_Scr < x2 + pixelsInOne && item.Coordinates.Z_Scr - item.Coordinates.Y_Scr * y_scale > z1 - pixelsInOne && item.Coordinates.Z_Scr - item.Coordinates.Y_Scr * y_scale < z2 + pixelsInOne))
                {
                    objectsInside.Add(item);
                    if (item.Selected)
                        objectsSelectedInside.Add(item);
                }
                else
                    if (item.Selected)
                        objectsSelectedOutside.Add(item);
            }

            if (additive)
            {
                //if there are unselected objects inside => select them
                if (objectsInside.Count > objectsSelectedInside.Count)
                {
                    foreach (MapObjectNamed item in objectsInside)
                        item.Selected = true;
                    return true;
                }

                return changed;
            }

            if (removative)
            {
                //if there are selected objects inside => unselect them
                if (objectsSelectedInside.Count > 0)
                {
                    foreach (MapObjectNamed item in objectsSelectedInside)
                        item.Selected = false;
                    return true;
                }

                return changed;
            }

            //if there are objects selected outside but no objects inside unselect everything
            if (objectsSelectedOutside.Count > 0 && objectsInside.Count == 0)
            {
                foreach (MapObjectNamed item in objectsSelectedOutside)
                    item.Selected = false;
                return true;
            }

            
            //If we have nothing to select inside or deselect outside
            if (objectsInside.Count == objectsSelectedInside.Count && objectsSelectedOutside.Count == 0)
                return changed;
               
            //if there are objects to be selected inside - select them, after unselecting everything
            if (objectsInside.Count > objectsSelectedInside.Count)
            {
                foreach (MapObjectNamed item in objectsSelectedOutside)
                    item.Selected = false;

                foreach (MapObjectNamed item in objectsInside)
                    item.Selected = true;

                return true;
            }

            return changed;
        }
                
        public  bool Select_None()
        {
            bool foundOne = false;
            bool changed = false;
            if (_selectionNameless != null)
            {
                changed = true;
                _selectionNameless = null;
            }

            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                {
                    foundOne = true;
                    item.Selected = false;
                }
            }
            return foundOne || changed;
        }
                
        public  bool Select_NextNamed()
        {
            bool changed = false;
            MapObjectNamed toBeSelected = null, cur = null;
            MapObjectNamed[] selectedObjects = GetSelectionNamed();
            int i;

            if (SelectionNameless != null)
            {
                changed = true;
                SelectionNameless = null;
            }

            if (selectedObjects.Count() == 0)
            {
                if (NamedObjects.Count == 0)
                    return changed;
                NamedObjects[0].Selected = true;   
                return true;
            }

            if (selectedObjects.Count() == NamedObjects.Count)
            {
                if (NamedObjects.Count == 1)
                    return changed;
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                return Select_NextNamed();
            }

            i = 0;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                cur = NamedObjects[i];
                if (!cur.Selected && foundSelected)
                    toBeSelected = cur;
                if (selectedObjects.Contains(cur))
                    foundSelected = true;
                i = (i + 1) % NamedObjects.Count;
            }
            for (i = NamedObjects.Count - 1; i >= 0; i--)
                NamedObjects[i].Selected = false;
            toBeSelected.Selected = true;
            return true;
        }
                
        public  bool Select_PreviousNamed()
        {
            bool changed = false;
            MapObjectNamed toBeSelected = null, cur = null;
            MapObjectNamed[] selectedObjects = GetSelectionNamed();
            int i;

            if (SelectionNameless != null)
            {
                changed = true;
                SelectionNameless = null;
            }

            if (selectedObjects.Count() == 0)
            {
                if (NamedObjects.Count == 0)
                    return changed;
                NamedObjects[NamedObjects.Count-1].Selected = true;
                    return true;
            }

            if (selectedObjects.Count() == NamedObjects.Count)
            {
                if (NamedObjects.Count == 1)
                    return changed; 
                for (i = NamedObjects.Count - 1; i >= 0; i--)
                    NamedObjects[i].Selected = false;
                return Select_PreviousNamed();
            }

            i = NamedObjects.Count - 1;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                cur = NamedObjects[i];
                if (!cur.Selected && foundSelected)
                    toBeSelected = cur;
                if (selectedObjects.Contains(cur))
                    foundSelected = true;
                i--;
                i = i < 0 ? NamedObjects.Count - 1 : i;
            }
            for (i = NamedObjects.Count - 1; i >= 0; i--)
                NamedObjects[i].Selected = false;
            toBeSelected.Selected = true;
            return true;
        }
                
        public  bool Select_NextNameless()
        {
            bool changed = false;
            MapObjectNameless toBeSelected = null;
            int i;

            for (i = NamedObjects.Count - 1; i >= 0; i--)
            {
                changed = changed || NamedObjects[i].Selected;
                NamedObjects[i].Selected = false;
            }

            if (SelectionNameless==null)
            {
                if (NamelessObjects.Count == 0)
                    return changed;
                SelectionNameless =  NamelessObjects[0];
                return true;
            }

            if (NamelessObjects.Count==1)
                return changed;

            i = 0;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                if (foundSelected)
                    toBeSelected = NamelessObjects[i];
                if (SelectionNameless==NamelessObjects[i])
                    foundSelected = true;
                i = (i + 1) % NamelessObjects.Count;
            }
            SelectionNameless = toBeSelected;
            return true;
        }
                
        public  bool Select_PreviousNameless()
        {
            bool changed = false; 
            MapObjectNameless toBeSelected = null;
            int i;

            for (i = NamedObjects.Count - 1; i >= 0; i--)
            {
                changed = changed || NamedObjects[i].Selected;
                NamedObjects[i].Selected = false;
            }

            if (SelectionNameless == null)
            {
                if (NamelessObjects.Count == 0)
                    return changed;
                SelectionNameless = NamelessObjects[NamelessObjects.Count-1];
                return true;
            }

            if (NamelessObjects.Count == 1)
                return changed;

            i = NamelessObjects.Count - 1;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                if (foundSelected)
                    toBeSelected = NamelessObjects[i];
                if (SelectionNameless == NamelessObjects[i])
                    foundSelected = true;
                i--;
                i = i < 0 ? NamelessObjects.Count - 1 : i;
            }
            SelectionNameless = toBeSelected;
            return true;
        }
                
        public  int Delete_Selected()
        {
            int somethingDeleted = 0;

            if (SelectionNameless != null)
            {
                RemoveNamelessObject(SelectionNameless);
                _selectionNameless = null;
                return 1;
            }

            for (int i = NamedObjects.Count - 1; i >= 0; i--)
            {
                if (NamedObjects[i].Selected)
                {
                    RemoveNamedObject(NamedObjects[i]);
                    somethingDeleted++;
                }
            }
            return somethingDeleted;
        }
                
        public  bool Clear()
        {
            int tmp = NamelessObjects.Count + NamedObjects.Count;
			Select_None();
			NamedObjects.Clear();
            NamelessObjects.Clear();
            NamedIdList.Clear();
            NamelessIdList.Clear();
			BacgkroundNamedObjects.Clear();
			BackgroundNamelessObjects.Clear();
            return tmp > 0;
        }
                
        public bool Selection_Turn_or_SetRadius(int x, int z, bool shiftPressed=false, bool controlPressed = false)
        {
            if (SelectionNameless != null)
            {
                if (shiftPressed)
                    SelectionNameless.radius = SelectionNameless.radius == 0 ? 1 : 0;
                if (controlPressed)
                {
                    SelectionNameless.randomRange = (int)Math.Round(Math.Sqrt(Math.Pow(SelectionNameless._coordinatesStart.X_Scr - x, 2) + Math.Pow(SelectionNameless._coordinatesStart.Z_Scr - z, 2)));
                    if (SelectionNameless.randomRange > SelectionNameless.radius)
                        SelectionNameless.randomRange = SelectionNameless.randomRange - SelectionNameless.radius;
                    else
                        SelectionNameless.randomRange = SelectionNameless.radius - SelectionNameless.randomRange;
                        return true;
                }
                
                if (SelectionNameless.radius == 0)
                {
                    SelectionNameless._coordinatesEnd.X_Scr = x;
                    SelectionNameless._coordinatesEnd.Z_Scr = z;
                }
                else
                {
                    SelectionNameless.radius = (int)Math.Round(Math.Sqrt(Math.Pow(SelectionNameless._coordinatesStart.X_Scr-x,2) + Math.Pow(SelectionNameless._coordinatesStart.Z_Scr - z,2)));
                    
                }
                return true;
            }
            
            List<MapObjectNamed> selectedObjects = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                    selectedObjects.Add(item);
            }

            if (selectedObjects.Count == 0)
                return false;

            if (shiftPressed)
            {
                int vecx, vecy, vecz;
                Selection_GetCenterOfMass(out vecx, out vecy, out vecz);

                double angle = Helper.CalcaulateAngle(vecx, vecz, x, z);
                
                foreach (MapObjectNamed item in selectedObjects)
                {
                    if (!item.IsPropertyAvailable("angle"))
                        continue;
                    ((MapObjectNamedA)item).A_rad = angle + (controlPressed ? -Math.PI / 2 : Math.PI / 2);
                }
                return true;
            }

           foreach (MapObjectNamed item in selectedObjects)
            {
                if (!item.IsPropertyAvailable("angle"))
                    continue;

                double angle = Helper.CalcaulateAngle(item.Coordinates.X_Scr, item.Coordinates.Z_Scr, x, z);
                ((MapObjectNamedA)item).A_rad = angle + (controlPressed ? -Math.PI / 2 : Math.PI / 2);
            }
            return true;
        }
                
        public  bool Selection_GetCenterOfMass(out int vecx, out int vecy, out int vecz)
        {
            MapObjectNamed[] selectedObjects = GetSelectionNamed();
            int count = selectedObjects.Count();
            double dvecx = 0.0, dvecy = 0.0, dvecz = 0.0;

            foreach (MapObjectNamed item in selectedObjects)
            {
                dvecx += item.Coordinates.X_Scr / count;
                dvecy += item.Coordinates.Y_Scr / count;
                dvecz += item.Coordinates.Z_Scr / count;
            }

			vecx = (int)Math.Round(dvecx);
			vecy = (int)Math.Round(dvecy);
			vecz = (int)Math.Round(dvecz);

            return count > 1;
        }

        private bool Selection_ToFrontBack(bool toFront)
        {
            if (SelectionNameless != null)
            {
                if (toFront && NamelessObjects[0] == SelectionNameless)
                    return false;
                if (!toFront && NamelessObjects[NamelessObjects.Count - 1] == SelectionNameless)
                    return false;

                List<MapObjectNameless> unselected = new List<MapObjectNameless>();
                foreach (MapObjectNameless item in NamelessObjects)
                    if (item != SelectionNameless)
                        unselected.Add(item);

                NamelessObjects.Clear();
                if (toFront)
                {
                    NamelessObjects.Add(SelectionNameless);
                    foreach (MapObjectNameless item in unselected)
                        NamelessObjects.Add(item);
                }
                else
                {
                    foreach (MapObjectNameless item in unselected)
                        NamelessObjects.Add(item);
                    NamelessObjects.Add(SelectionNameless);
                }
                return true;
            }
            
            bool foundDeselected = false;
            bool foundSelectedAfterDeselected = false;
            bool foundSelected = false;
            bool foundDeselectedAfterSelected = false;

            List<MapObjectNamed> selected = new List<MapObjectNamed>();
            List<MapObjectNamed> deselected = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                {
                    selected.Add(item);
                    foundSelected = true;
                    foundSelectedAfterDeselected = foundSelectedAfterDeselected || foundDeselected;
                }
                else
                {
                    deselected.Add(item);
                    foundDeselected = true;
                    foundDeselectedAfterSelected = foundDeselectedAfterSelected || foundSelected;
                }
            }

            if (!((foundSelectedAfterDeselected&&toFront)||(foundDeselectedAfterSelected&&!toFront)))
                return false;

            NamedObjects.Clear();
            if (toFront)
            {
                foreach (MapObjectNamed item in selected) NamedObjects.Add(item);
                foreach (MapObjectNamed item in deselected) NamedObjects.Add(item);
            }
            else
            {
                foreach (MapObjectNamed item in deselected) NamedObjects.Add(item);
                foreach (MapObjectNamed item in selected) NamedObjects.Add(item);
            }

            return true;
        }

        public  bool Selection_ToFront() { return Selection_ToFrontBack(true); }
                
        public  bool Selection_ToBack() { return Selection_ToFrontBack(false); }
                
        public  bool Selection_MoveTo(int x, int y, int z)
        {
            List<MapObjectNamed> selectedObjects = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                    selectedObjects.Add(item);
            }

            if (selectedObjects.Count == 0)
                return false;

            int vecx, vecy, vecz;
            Selection_GetCenterOfMass(out vecx, out vecy, out vecz);

            //calculate movement vector
            vecx -= x;
            vecy -= y;
            vecz -= z;

            foreach (MapObjectNamed item in selectedObjects)
            {
                item._coordinates.X_Scr -= vecx;
                item._coordinates.Y_Scr -= vecy;
                item._coordinates.Z_Scr -= vecz;
            }
            return true;
        }
                
        public  bool Selection_MoveTo(int x, int z)
        {
            if (SelectionNameless != null)
            {
                if (SelectionNameless.CoordinatesStart == SelectionNameless.CoordinatesEnd)
                {
                    SelectionNameless._coordinatesEnd.X_Scr = x;
                    SelectionNameless._coordinatesEnd.Z_Scr = z;
                }
                SelectionNameless._coordinatesStart.X_Scr = x;
                SelectionNameless._coordinatesStart.Z_Scr = z;
                return true;
            }

            List<MapObjectNamed> selectedObjects = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                    selectedObjects.Add(item);
            }

            if (selectedObjects.Count == 0)
                return false;

            int vecx, vecy, vecz;
            Selection_GetCenterOfMass(out vecx, out vecy, out vecz);
            
            //calculate movement vector
            vecx -= x;
            vecz -= z;

            foreach (MapObjectNamed item in selectedObjects)
            {
                item._coordinates.X_Scr -= vecx;
                item._coordinates.Z_Scr -= vecz;
            }
            return true;
        }
                
        public  bool Selection_ChangeCountBy(int by)
        {
            if (SelectionNameless == null)
                return false;
            if (SelectionNameless.count == 0 && by < 0)
                return false;
            SelectionNameless.count += by;
            return true;
        }
                
        public  bool Selection_MoveBy(int x, int y, int z)
        {
            if (SelectionNameless != null)
            {
                SelectionNameless._coordinatesStart.X_Scr += x;
                SelectionNameless._coordinatesStart.Y_Scr += y;
                SelectionNameless._coordinatesStart.Z_Scr += z;
                SelectionNameless._coordinatesEnd.X_Scr += x;
                SelectionNameless._coordinatesEnd.Y_Scr += y;
                SelectionNameless._coordinatesEnd.Z_Scr += z;
                return true;
            }
                        
            List<MapObjectNamed> selectedObjects = new List<MapObjectNamed>();
            foreach (MapObjectNamed item in NamedObjects)
            {
                if (item.Selected)
                    selectedObjects.Add(item);
            }

            if (selectedObjects.Count == 0)
                return false;

            foreach (MapObjectNamed item in selectedObjects)
            {
                item._coordinates.X_Scr += x;
                item._coordinates.Y_Scr += y;
                item._coordinates.Z_Scr += z;
            }
            return true;
        }
                
        public  bool Selection_SetStartEndAngleTo(int x, int z, bool start, bool? negative)
        {
            if (SelectionNameless != null)
            {
                if (negative == null)
                {
                    if (start)
                        SelectionNameless.A_Start_rad = null;
                    else
                        SelectionNameless.A_End_rad = null;
                    return true;
                }

                double angle = Helper.CalcaulateAngle(SelectionNameless.CoordinatesStart.X_Scr, SelectionNameless.CoordinatesStart.Z_Scr, x, z);
                angle = angle + Math.PI / 2;
                if (angle > Math.PI * 2.0)
                    angle = angle - Math.PI * 2.0;

                if (start)
                    SelectionNameless.A_Start_rad = (negative.Value ? -Math.PI * 2.0 : 0) + (angle);
                else
                    SelectionNameless.A_End_rad = (negative.Value ? -Math.PI * 2.0 : 0) + (angle);

                return true;
            }
            return true;
        }

        public bool ExecuteWheelAction(int delta, PanelSpaceMapMode mode, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (mode == PanelSpaceMapMode.SingleSpecial)
                return SelectionSpecial.ExecuteWheelAction(delta, modifierKeys, x, y, pixelsInOne);

            return false;
        }
        
        public bool ExecuteCommand(KeyCommands command, PanelSpaceMapMode mode, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (mode == PanelSpaceMapMode.SingleSpecial)
                return SelectionSpecial.ExecuteCommand(command, modifierKeys, x, y, pixelsInOne);
            
            return false;
        }

        public  bool ExecuteMouseAction(MouseButtons buttons, PanelSpaceMapMode mode, Keys modifierKeys, int x, int y, int pixelsInOne)
        {
            if (mode == PanelSpaceMapMode.SingleSpecial)
                return SelectionSpecial.ExecuteMouseAction(buttons, modifierKeys, x, y, pixelsInOne);

            return false;
        }
        public  void FromXml(string text)
        {
            XmlDocument xDoc = new XmlDocument();
            
            try
            {
                xDoc.LoadXml(text);
            }
            catch (Exception ex)
            {
                Log.Add("Failed to load space map from xml for the following reasons:");
                Log.Add(ex);
                return;
            }

            XmlNode root = null;
            foreach (XmlNode node in xDoc.ChildNodes)
                if (node.Name == "createInput" 
                    || node.Name == "statementInput" 
                    || node.Name == "bgInput"
					|| node.Name == "output")
                    root = node;

            if (root == null)
            {
                Log.Add("Failed to load space map from xml for the following reasons:");
                Log.Add("Root node \"createInput\" or \"statementInput\" or \"bgInput\" or \"output\"not found!");
                return;
            }

            if (root.Name == "createInput" || root.Name == "bgInput" || root.Name == "output")
            {
                bool bg = root.Name == "bgInput";

                if (!bg)
                    Clear();
                foreach (XmlNode item in root.ChildNodes)
                {
                    MapObjectNamed mo =  MapObjectNamed.NewFromXml(item);
                    MapObjectNameless nmo = MapObjectNameless.NewFromXml(item);

                    if (bg)
                    {
                        if (mo != null)
                            BacgkroundNamedObjects.Add(mo);
                        if (nmo != null)
                            BackgroundNamelessObjects.Add(nmo);
                    }
                    else
                    {
                        if (mo != null)
                            NamedObjects.Add(mo);
                        if (nmo != null)
                            NamelessObjects.Add(nmo);
                    }
                }
            }

            if (root.Name == "statementInput")
            {
                Clear();

                SelectionSpecial = MapObjectSpecial.NewFromXml(root.ChildNodes[0]);
            }
            
        }
        
        public  string ToXml(bool bg = false)
        {
            XmlDocument xDoc;
            XmlElement root, create;
            XmlComment comment;
            List<string> missingProperties;

            xDoc = new XmlDocument();
            missingProperties = new List<string>();
            root = xDoc.CreateElement(bg ? "bgInput": "output");
            xDoc.AppendChild(root);

            //Out objects
            foreach (MapObjectNameless item in bg ? BackgroundNamelessObjects : NamelessObjects)
            {
                missingProperties.Clear();

                create = item.ToXml(xDoc, missingProperties);

                if (missingProperties.Count > 0)
                {
                    comment = xDoc.CreateComment("will fail because it lacks "+missingProperties.Aggregate((i,j)=>i+", "+j)+" ");
                    root.AppendChild(comment);
                }
                
                root.AppendChild(create);
            }

            foreach (MapObjectNamed item in bg ? BacgkroundNamedObjects : NamedObjects)
            {
                missingProperties.Clear();

                create = item.ToXml(xDoc, missingProperties);

                if (missingProperties.Count > 0)
                {
                    comment = xDoc.CreateComment("will fail because it lacks " + missingProperties.Aggregate((i, j) => i + ", " + j) + " ");
                    root.AppendChild(comment);
                }

                root.AppendChild(create);
            }

            //I make this look GOOD!
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                xDoc.Save(writer);
            }

            return sb.ToString();
        }

        /// <summary> Function called once when initialising edit mode to mark all objects as imported </summary>
        public  void MarkAsImported()
        {
            foreach (MapObjectNamed item in NamedObjects)
                item.Imported = true;
            foreach (MapObjectNameless item in NamelessObjects)
                item.Imported = true;
        }

        private void RemoveNamedObject(MapObjectNamed item)
        {
            if (item.Imported)
            {
                int j = 0;
                for (int i = 0; i < NamedObjects.Count && NamedObjects[i] != item; i++)
                    j += NamedObjects[i].Imported ? 1 : 0;
                NamedIdList.RemoveAt(j);
            }
            NamedObjects.Remove(item);
        }

        private void RemoveNamelessObject(MapObjectNameless item)
        {
            if (item.Imported)
            {
                int j = 0;
                for (int i = 0; i < NamedObjects.Count && NamelessObjects[i] != item; i++)
                    j += NamelessObjects[i].Imported ? 1 : 0;
                NamelessIdList.RemoveAt(j);
            }
            NamelessObjects.Remove(item);
        }

		#region Undo / Redo / Change registration

		public Stack<SpaceSavedState> UndoStack;
		public Stack<SpaceSavedState> RedoStack;

		public string UndoDescription { get { if (UndoStack.Count < 2) return null; return UndoStack.Peek().Description; } }
		public string RedoDescription { get { if (RedoStack.Count == 0) return null; return RedoStack.Peek().Description; } }

		public bool Undo(SpaceSavedState state = null)
		{
			if (UndoStack.Count < 2)
				return false;

			state = state ?? UndoStack.Peek();
			while (RedoStack.Count == 0 || RedoStack.Peek() != state)
				RedoStack.Push(UndoStack.Pop());
			FromState(UndoStack.Peek());

			return true;
		}

		public bool Redo(SpaceSavedState state = null)
		{
			if (RedoStack.Count == 0)
				return false;

			state = state ?? RedoStack.Peek();
			while (UndoStack.Count == 0 || UndoStack.Peek() != state)
				UndoStack.Push(RedoStack.Pop());
			FromState(UndoStack.Peek());

			return true;
		}
		
		public void RegisterChange(string shortDescription)
		{
			UndoStack.Push(ToState(shortDescription));
			RedoStack.Clear();
		}

		private SpaceSavedState ToState(string shortDescription)
		{
			SpaceSavedState state = new SpaceSavedState();

			state.StatementXml = SelectionSpecial == null ? null : "<statementInput>" + SelectionSpecial.ToXml(new XmlDocument()).OuterXml + "</statementInput>";
			state.Xml = ToXml();
			state.BgXml = ToXml(true);
			state.ChangesPending = ChangesPending;
			state.Description = shortDescription;
			state.NamedIdList = NamedIdList.ToList();
			state.NamelessIdList = NamelessIdList.ToList();
			state.NamedImported = new List<int>();
			state.NamelessImported = new List<int>();
			for (int i = 0; i < NamedObjects.Count; i++)
				if (NamedObjects[i].Imported)
					state.NamedImported.Add(i);
			for (int i = 0; i < NamelessObjects.Count; i++)
				if (NamelessObjects[i].Imported)
					state.NamelessImported.Add(i);
			state.NamedSelected = new List<int>();
			for (int i = 0; i < NamedObjects.Count; i++)
				if (NamedObjects[i].Selected)
					state.NamedSelected.Add(i);
			if (SelectionNameless == null)
				state.NamelessSelected = null;
			else
				state.NamelessSelected = NamelessObjects.IndexOf(SelectionNameless);
			
			return state;
		}

		private void FromState(SpaceSavedState state)
		{
			if (!string.IsNullOrWhiteSpace(state.StatementXml)) FromXml(state.StatementXml);
			FromXml(state.Xml);
			FromXml(state.BgXml);
			ChangesPending = state.ChangesPending;
			NamedIdList = state.NamedIdList.ToList();
			NamelessIdList = state.NamelessIdList.ToList();
			foreach (int i in state.NamedImported)
				NamedObjects[i].Imported = true;
			foreach (int i in state.NamelessImported)
				NamelessObjects[i].Imported = true;
			foreach (int i in state.NamedSelected)
				NamedObjects[i].Selected = true;
			if (state.NamelessSelected != null)
				SelectionNameless = NamelessObjects[state.NamelessSelected.Value];
		}

		#endregion

        public Space()
        {
            NamedObjects = new List<MapObjectNamed>();
            NamelessObjects = new List<MapObjectNameless>();

            BacgkroundNamedObjects = new List<MapObjectNamed>();
            BackgroundNamelessObjects = new List<MapObjectNameless>();

            NamedIdList = new List<int>();
            NamelessIdList = new List<int>();

            UndoStack = new Stack<SpaceSavedState>();
            RedoStack = new Stack<SpaceSavedState>();
        }
    }
}
