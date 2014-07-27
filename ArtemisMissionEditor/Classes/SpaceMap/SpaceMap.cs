using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public sealed class SpaceMap
    {
        //Coordinates and angle in space
        public static readonly int _minX = 0;
        public static readonly int _maxX = 100000;
        public static readonly int _minY = -100000;
        public static readonly int _maxY = 100000;
        public static readonly int _minZ = 0;
        public static readonly int _maxZ = 100000;
        public static readonly int _paddingZ = 10000;
        public static readonly int _paddingX = 10000;
        public static readonly int _nebulaWidth = 4000;
        public static readonly int _asteroidWidth = 300;
        public static readonly int _mineWidth = 250;

		#region Helper Functions

		public  int PointXToScreen(int surface_Width, int space_map_x)
		{
			return (int)(((long)space_map_x + (long)SpaceMap._paddingX) * (long)surface_Width / ((long)SpaceMap._maxX + (long)SpaceMap._paddingX * 2));
		}
                
		public  int PointZToScreen(int surface_Height, int space_map_z)
		{
			return (int)(((long)space_map_z + (long)SpaceMap._paddingZ) * (long)surface_Height / ((long)SpaceMap._maxZ + (long)SpaceMap._paddingZ * 2));
		}
                
		public  double PointXToScreen(double surface_Width, double space_map_x)
		{
			return (space_map_x + SpaceMap._paddingX) * surface_Width / (SpaceMap._maxX + SpaceMap._paddingX * 2);
		}
                
		public  double PointZToScreen(double surface_Height, double space_map_z)
		{
			return (space_map_z + SpaceMap._paddingZ) * surface_Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2);
		}
                
		public  int PointXToSpaceMap(int surface_Width, int screen_x)
		{
			return (int)(((long)screen_x) * ((long)SpaceMap._maxX + (long)SpaceMap._paddingX * 2) / (long)surface_Width - (long)SpaceMap._paddingX);
		}
                
		public  int PointZToSpaceMap(int surface_Height, int screen_z)
		{
			return (int)(((long)screen_z) * ((long)SpaceMap._maxZ + (long)SpaceMap._paddingZ * 2) / (long)surface_Height - (long)SpaceMap._paddingZ);
		}
                
		public  double PointXToSpaceMap(double surface_Width, double screen_x)
		{
			return (screen_x) * (SpaceMap._maxX + SpaceMap._paddingX * 2) / surface_Width - SpaceMap._paddingX;
		}
                
		public  double PointZToSpaceMap(double surface_Height, double screen_z)
		{
			return (screen_z) * (SpaceMap._maxZ + SpaceMap._paddingZ * 2) / surface_Height - SpaceMap._paddingZ;
		}

		#endregion

		public bool ChangesPending { get; set; }

		/// <summary> List of ID's of named create statements that were imported </summary>
        public  List<int> namedIdList;
        /// <summary> List of ID's of nameless create statements that were imported </summary>
        public  List<int> namelessIdList;
                
        public  List<NamedMapObject> namedObjects;
        public  List<NamelessMapObject> namelessObjects;
                
		public  List<NamedMapObject> bgNamedObjects;
		public  List<NamelessMapObject> bgNamelessObjects;
                
        public  int Selection_Count
        {
            get
            {
                int value = 0;
                foreach (NamedMapObject item in namedObjects)
                    value += (item.Selected) ? 1 : 0;
                return value;
            }
        }
                
        public NamedMapObject[] GetSelectionNamed()
        {
            List<NamedMapObject> value = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
                if (item.Selected)
                    value.Add(item);
            return value.ToArray();
        }

        private NamelessMapObject _selectionNameless;
        public  NamelessMapObject SelectionNameless
        {
            get { return _selectionNameless; }
            set { if (value != null) Select_None(); if(value!=null && !namelessObjects.Contains(value)) throw new ArgumentOutOfRangeException("Attempting to select a nonexistant nameless object"); _selectionNameless = value; }
        }

        public SpecialMapObject SelectionSpecial { get; set; }

        public  object AddObject(string type, int posX = 0, int posY = 0, int posZ = 0, int angle = 0, bool makeSelected = false)
        {
            NamedMapObject mo = null;
            NamelessMapObject nmo = null;

            switch (type)
            {
                case "station":
                    mo = new NamedMapObject_station(posX, posY, posZ, makeSelected, angle);
                    break;
                case "neutral":
                    mo = new NamedMapObject_neutral(posX, posY, posZ, makeSelected, angle);
                    break;
                case "player":
                    mo = new NamedMapObject_player(posX, posY, posZ, makeSelected, angle);
                    break;
                case "monster":
                    mo = new NamedMapObject_monster(posX, posY, posZ, makeSelected, angle);
                    break;
                case "anomaly":
                    mo = new NamedMapObject_anomaly(posX, posY, posZ, "", makeSelected);
                    break;
                case "blackHole":
                    mo = new NamedMapObject_blackHole(posX, posY, posZ, "", makeSelected);
                    break;
                case "enemy":
                    mo = new NamedMapObject_enemy(posX, posY, posZ, makeSelected, angle);
                    break;
                case "genericMesh":
                    mo = new NamedMapObject_genericMesh(posX, posY, posZ, makeSelected, angle);
                    break;
				case "whale":
					mo = new NamedMapObject_whale(posX, posY, posZ, makeSelected, angle);
					break;
				case "nameless":
                    nmo = new NamelessMapObject(posX, posY, posZ, "");
                    break;
                case "nebulas":
                    nmo = new NamelessMapObject(posX, posY, posZ, "nebulas");
                    break;
                case "asteroids":
                    nmo = new NamelessMapObject(posX, posY, posZ, "asteroids");
                    break;
                case "mines":
                    nmo = new NamelessMapObject(posX, posY, posZ, "mines");
                    break;
                default:
                    break;
            }

            if (mo == null && nmo==null)
                throw new NotImplementedException("Attempting to add object of unknown type " + type);

            if (mo != null)
            {
                namedObjects.Add(mo);
                return mo;
            }
            else
            {
                namelessObjects.Add(nmo);
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

            foreach (NamedMapObject item in namedObjects)
            {
                changed = changed || !item.Selected;
                item.Selected = true;
            }
            return changed;
        }
                
        public  bool Select_AtPoint(int x, int z, int pixelsInOne, double y_scale = 0.0, bool additive = false, bool removative = false)
        {
            List<NamedMapObject> objectsSelectedUnderCursor = new List<NamedMapObject>();
            List<NamedMapObject> objectsUnderCursor = new List<NamedMapObject>();
            List<NamedMapObject> objectsSelectedNotUnderCursor = new List<NamedMapObject>();
            int i;
            bool changed = false;
            if (_selectionNameless != null)
            {
                changed = true;
                _selectionNameless = null;
            }

            foreach (NamedMapObject item in namedObjects)
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
                    NamedMapObject toBeSelected = null, cur = null;
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
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
                return Select_AtPoint(x, z, pixelsInOne);
            }

            //If there is something to select under cursor and nothing is selected there => just select first (and deselect everything else)
            if (objectsUnderCursor.Count > 0 && objectsSelectedUnderCursor.Count == 0)
            {
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
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
                NamedMapObject toBeSelected = null, cur = null;
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
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
                toBeSelected.Selected = true;
                return true;
            }

            //If there is nothing to select under cursor but something selected elsewhere - clear it
            if (objectsSelectedNotUnderCursor.Count > 0)
            {
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
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
            List<NamedMapObject> objectsSelectedOutside = new List<NamedMapObject>();
            List<NamedMapObject> objectsInside = new List<NamedMapObject>();
            List<NamedMapObject> objectsSelectedInside = new List<NamedMapObject>();
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

            foreach (NamedMapObject item in namedObjects)
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
                    foreach (NamedMapObject item in objectsInside)
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
                    foreach (NamedMapObject item in objectsSelectedInside)
                        item.Selected = false;
                    return true;
                }

                return changed;
            }

            //if there are objects selected outside but no objects inside unselect everything
            if (objectsSelectedOutside.Count > 0 && objectsInside.Count == 0)
            {
                foreach (NamedMapObject item in objectsSelectedOutside)
                    item.Selected = false;
                return true;
            }

            
            //If we have nothing to select inside or deselect outside
            if (objectsInside.Count == objectsSelectedInside.Count && objectsSelectedOutside.Count == 0)
                return changed;
               
            //if there are objects to be selected inside - select them, after unselecting everything
            if (objectsInside.Count > objectsSelectedInside.Count)
            {
                foreach (NamedMapObject item in objectsSelectedOutside)
                    item.Selected = false;

                foreach (NamedMapObject item in objectsInside)
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

            foreach (NamedMapObject item in namedObjects)
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
            NamedMapObject toBeSelected = null, cur = null;
            NamedMapObject[] selectedObjects = GetSelectionNamed();
            int i;

            if (SelectionNameless != null)
            {
                changed = true;
                SelectionNameless = null;
            }

            if (selectedObjects.Count() == 0)
            {
                if (namedObjects.Count == 0)
                    return changed;
                namedObjects[0].Selected = true;   
                return true;
            }

            if (selectedObjects.Count() == namedObjects.Count)
            {
                if (namedObjects.Count == 1)
                    return changed;
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
                return Select_NextNamed();
            }

            i = 0;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                cur = namedObjects[i];
                if (!cur.Selected && foundSelected)
                    toBeSelected = cur;
                if (selectedObjects.Contains(cur))
                    foundSelected = true;
                i = (i + 1) % namedObjects.Count;
            }
            for (i = namedObjects.Count - 1; i >= 0; i--)
                namedObjects[i].Selected = false;
            toBeSelected.Selected = true;
            return true;
        }
                
        public  bool Select_PreviousNamed()
        {
            bool changed = false;
            NamedMapObject toBeSelected = null, cur = null;
            NamedMapObject[] selectedObjects = GetSelectionNamed();
            int i;

            if (SelectionNameless != null)
            {
                changed = true;
                SelectionNameless = null;
            }

            if (selectedObjects.Count() == 0)
            {
                if (namedObjects.Count == 0)
                    return changed;
                namedObjects[namedObjects.Count-1].Selected = true;
                    return true;
            }

            if (selectedObjects.Count() == namedObjects.Count)
            {
                if (namedObjects.Count == 1)
                    return changed; 
                for (i = namedObjects.Count - 1; i >= 0; i--)
                    namedObjects[i].Selected = false;
                return Select_PreviousNamed();
            }

            i = namedObjects.Count - 1;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                cur = namedObjects[i];
                if (!cur.Selected && foundSelected)
                    toBeSelected = cur;
                if (selectedObjects.Contains(cur))
                    foundSelected = true;
                i--;
                i = i < 0 ? namedObjects.Count - 1 : i;
            }
            for (i = namedObjects.Count - 1; i >= 0; i--)
                namedObjects[i].Selected = false;
            toBeSelected.Selected = true;
            return true;
        }
                
        public  bool Select_NextNameless()
        {
            bool changed = false;
            NamelessMapObject toBeSelected = null;
            int i;

            for (i = namedObjects.Count - 1; i >= 0; i--)
            {
                changed = changed || namedObjects[i].Selected;
                namedObjects[i].Selected = false;
            }

            if (SelectionNameless==null)
            {
                if (namelessObjects.Count == 0)
                    return changed;
                SelectionNameless =  namelessObjects[0];
                return true;
            }

            if (namelessObjects.Count==1)
                return changed;

            i = 0;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                if (foundSelected)
                    toBeSelected = namelessObjects[i];
                if (SelectionNameless==namelessObjects[i])
                    foundSelected = true;
                i = (i + 1) % namelessObjects.Count;
            }
            SelectionNameless = toBeSelected;
            return true;
        }
                
        public  bool Select_PreviousNameless()
        {
            bool changed = false; 
            NamelessMapObject toBeSelected = null;
            int i;

            for (i = namedObjects.Count - 1; i >= 0; i--)
            {
                changed = changed || namedObjects[i].Selected;
                namedObjects[i].Selected = false;
            }

            if (SelectionNameless == null)
            {
                if (namelessObjects.Count == 0)
                    return changed;
                SelectionNameless = namelessObjects[namelessObjects.Count-1];
                return true;
            }

            if (namelessObjects.Count == 1)
                return changed;

            i = namelessObjects.Count - 1;
            bool foundSelected = false;
            while (toBeSelected == null)
            {
                if (foundSelected)
                    toBeSelected = namelessObjects[i];
                if (SelectionNameless == namelessObjects[i])
                    foundSelected = true;
                i--;
                i = i < 0 ? namelessObjects.Count - 1 : i;
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

            for (int i = namedObjects.Count - 1; i >= 0; i--)
            {
                if (namedObjects[i].Selected)
                {
                    RemoveNamedObject(namedObjects[i]);
                    somethingDeleted++;
                }
            }
            return somethingDeleted;
        }
                
        public  bool Clear()
        {
            int tmp = namelessObjects.Count + namedObjects.Count;
			Select_None();
			namedObjects.Clear();
            namelessObjects.Clear();
            namedIdList.Clear();
            namelessIdList.Clear();
			bgNamedObjects.Clear();
			bgNamelessObjects.Clear();
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
            
            List<NamedMapObject> selectedObjects = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
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
                
                foreach (NamedMapObject item in selectedObjects)
                {
                    if (!item.IsPropertyAvailable("angle"))
                        continue;
                    ((NamedMapObjectA)item).A_rad = angle + (controlPressed ? -Math.PI / 2 : Math.PI / 2);
                }
                return true;
            }

           foreach (NamedMapObject item in selectedObjects)
            {
                if (!item.IsPropertyAvailable("angle"))
                    continue;

                double angle = Helper.CalcaulateAngle(item.Coordinates.X_Scr, item.Coordinates.Z_Scr, x, z);
                ((NamedMapObjectA)item).A_rad = angle + (controlPressed ? -Math.PI / 2 : Math.PI / 2);
            }
            return true;
        }
                
        public  bool Selection_GetCenterOfMass(out int vecx, out int vecy, out int vecz)
        {
            NamedMapObject[] selectedObjects = GetSelectionNamed();
            int count = selectedObjects.Count();
            double dvecx = 0.0, dvecy = 0.0, dvecz = 0.0;

            foreach (NamedMapObject item in selectedObjects)
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
                if (toFront && namelessObjects[0] == SelectionNameless)
                    return false;
                if (!toFront && namelessObjects[namelessObjects.Count - 1] == SelectionNameless)
                    return false;

                List<NamelessMapObject> unselected = new List<NamelessMapObject>();
                foreach (NamelessMapObject item in namelessObjects)
                    if (item != SelectionNameless)
                        unselected.Add(item);

                namelessObjects.Clear();
                if (toFront)
                {
                    namelessObjects.Add(SelectionNameless);
                    foreach (NamelessMapObject item in unselected)
                        namelessObjects.Add(item);
                }
                else
                {
                    foreach (NamelessMapObject item in unselected)
                        namelessObjects.Add(item);
                    namelessObjects.Add(SelectionNameless);
                }
                return true;
            }
            
            bool foundDeselected = false;
            bool foundSelectedAfterDeselected = false;
            bool foundSelected = false;
            bool foundDeselectedAfterSelected = false;

            List<NamedMapObject> selected = new List<NamedMapObject>();
            List<NamedMapObject> deselected = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
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

            namedObjects.Clear();
            if (toFront)
            {
                foreach (NamedMapObject item in selected) namedObjects.Add(item);
                foreach (NamedMapObject item in deselected) namedObjects.Add(item);
            }
            else
            {
                foreach (NamedMapObject item in deselected) namedObjects.Add(item);
                foreach (NamedMapObject item in selected) namedObjects.Add(item);
            }

            return true;
        }

        public  bool Selection_ToFront() { return Selection_ToFrontBack(true); }
                
        public  bool Selection_ToBack() { return Selection_ToFrontBack(false); }
                
        public  bool Selection_MoveTo(int x, int y, int z)
        {
            List<NamedMapObject> selectedObjects = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
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

            foreach (NamedMapObject item in selectedObjects)
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

            List<NamedMapObject> selectedObjects = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
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

            foreach (NamedMapObject item in selectedObjects)
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
                        
            List<NamedMapObject> selectedObjects = new List<NamedMapObject>();
            foreach (NamedMapObject item in namedObjects)
            {
                if (item.Selected)
                    selectedObjects.Add(item);
            }

            if (selectedObjects.Count == 0)
                return false;

            foreach (NamedMapObject item in selectedObjects)
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
            catch (Exception e)
            {
                Log.Add("Failed to load space map from xml for the following reasons:");
                Log.Add(e);
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
                    NamedMapObject mo =  NamedMapObject.NewFromXml(item);
                    NamelessMapObject nmo = NamelessMapObject.NewFromXml(item);

                    if (bg)
                    {
                        if (mo != null)
                            bgNamedObjects.Add(mo);
                        if (nmo != null)
                            bgNamelessObjects.Add(nmo);
                    }
                    else
                    {
                        if (mo != null)
                            namedObjects.Add(mo);
                        if (nmo != null)
                            namelessObjects.Add(nmo);
                    }
                }
            }

            if (root.Name == "statementInput")
            {
                Clear();

                SelectionSpecial = SpecialMapObject.NewFromXml(root.ChildNodes[0]);
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
            foreach (NamelessMapObject item in bg ? bgNamelessObjects : namelessObjects)
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

            foreach (NamedMapObject item in bg ? bgNamedObjects : namedObjects)
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
            foreach (NamedMapObject item in namedObjects)
                item.Imported = true;
            foreach (NamelessMapObject item in namelessObjects)
                item.Imported = true;
        }

        private void RemoveNamedObject(NamedMapObject item)
        {
            if (item.Imported)
            {
                int j = 0;
                for (int i = 0; i < namedObjects.Count && namedObjects[i] != item; i++)
                    j += namedObjects[i].Imported ? 1 : 0;
                namedIdList.RemoveAt(j);
            }
            namedObjects.Remove(item);
        }

        private void RemoveNamelessObject(NamelessMapObject item)
        {
            if (item.Imported)
            {
                int j = 0;
                for (int i = 0; i < namedObjects.Count && namelessObjects[i] != item; i++)
                    j += namelessObjects[i].Imported ? 1 : 0;
                namelessIdList.RemoveAt(j);
            }
            namelessObjects.Remove(item);
        }

		#region Undo / Redo / Change registration

		public Stack<SpaceMapSavedState> _undoStack;
		public Stack<SpaceMapSavedState> _redoStack;

		public string UndoDescription { get { if (_undoStack.Count < 2) return null; return _undoStack.Peek().Description; } }
		public string RedoDescription { get { if (_redoStack.Count == 0) return null; return _redoStack.Peek().Description; } }

		public bool Undo(SpaceMapSavedState state = null)
		{
			if (_undoStack.Count < 2)
				return false;

			state = state ?? _undoStack.Peek();
			while (_redoStack.Count == 0 || _redoStack.Peek() != state)
				_redoStack.Push(_undoStack.Pop());
			FromState(_undoStack.Peek());

			return true;
		}

		public bool Redo(SpaceMapSavedState state = null)
		{
			if (_redoStack.Count == 0)
				return false;

			state = state ?? _redoStack.Peek();
			while (_undoStack.Count == 0 || _undoStack.Peek() != state)
				_undoStack.Push(_redoStack.Pop());
			FromState(_undoStack.Peek());

			return true;
		}
		
		public void RegisterChange(string shortDescription)
		{
			_undoStack.Push(ToState(shortDescription));
			_redoStack.Clear();
		}

		private SpaceMapSavedState ToState(string shortDescription)
		{
			SpaceMapSavedState state = new SpaceMapSavedState();

			state.StatementXml = SelectionSpecial == null ? null : "<statementInput>" + SelectionSpecial.ToXml(new XmlDocument()).OuterXml + "</statementInput>";
			state.Xml = ToXml();
			state.BgXml = ToXml(true);
			state.ChangesPending = ChangesPending;
			state.Description = shortDescription;
			state.NamedIdList = namedIdList.ToList();
			state.NamelessIdList = namelessIdList.ToList();
			state.NamedImported = new List<int>();
			state.NamelessImported = new List<int>();
			for (int i = 0; i < namedObjects.Count; i++)
				if (namedObjects[i].Imported)
					state.NamedImported.Add(i);
			for (int i = 0; i < namelessObjects.Count; i++)
				if (namelessObjects[i].Imported)
					state.NamelessImported.Add(i);
			state.NamedSelected = new List<int>();
			for (int i = 0; i < namedObjects.Count; i++)
				if (namedObjects[i].Selected)
					state.NamedSelected.Add(i);
			if (SelectionNameless == null)
				state.NamelessSelected = null;
			else
				state.NamelessSelected = namelessObjects.IndexOf(SelectionNameless);
			
			return state;
		}

		private void FromState(SpaceMapSavedState state)
		{
			if (!string.IsNullOrWhiteSpace(state.StatementXml)) FromXml(state.StatementXml);
			FromXml(state.Xml);
			FromXml(state.BgXml);
			ChangesPending = state.ChangesPending;
			namedIdList = state.NamedIdList.ToList();
			namelessIdList = state.NamelessIdList.ToList();
			foreach (int i in state.NamedImported)
				namedObjects[i].Imported = true;
			foreach (int i in state.NamelessImported)
				namelessObjects[i].Imported = true;
			foreach (int i in state.NamedSelected)
				namedObjects[i].Selected = true;
			if (state.NamelessSelected != null)
				SelectionNameless = namelessObjects[state.NamelessSelected.Value];
		}

		#endregion

        public SpaceMap()
        {
            namedObjects = new List<NamedMapObject>();
            namelessObjects = new List<NamelessMapObject>();

            bgNamedObjects = new List<NamedMapObject>();
            bgNamelessObjects = new List<NamelessMapObject>();

            namedIdList = new List<int>();
            namelessIdList = new List<int>();

            _undoStack = new Stack<SpaceMapSavedState>();
            _redoStack = new Stack<SpaceMapSavedState>();
        }
    }
}
