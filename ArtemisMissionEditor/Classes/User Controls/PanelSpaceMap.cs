using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Diagnostics;
	
namespace ArtemisMissionEditor
{
    class _PanelSpaceMap : UserControl
	{
		[DisplayName("Mode"), Description("Mode used by this instance of space map panel."), DefaultValue(PanelSpaceMapMode.Normal)]
        public PanelSpaceMapMode Mode { get; set; }

		//Assigned controls
		private PropertyGrid            _propertyEditorPG;
		private SplitContainer          _propertyContainer;
		private string                  _propertyContainerPossiblesPanel;
		private ListBox                 _possibleRacesLB;
		private ListBox                 _possibleVesselsLB;
		private ListBox                 _namedObjectsLB;
		private ListBox                 _namelessObjectsLB;
		private StatusStrip             _statusTS;
		private ToolStripStatusLabel    _objectsSelectedTSSL;
		private ToolStripStatusLabel    _namedObjectsTotalTSSL;
		private ToolStripStatusLabel    _namelessObjectsTotalTSSL;
        private ToolStripStatusLabel    _cursorCoordinates;
		private TabPage					_namedObjectsPage;
		private TabPage					_namelessObjectsPage;
		private TabControl				_objectsTabControl;

		//Other stuff
		private bool readOnly;
		public bool ReadOnly { get { return readOnly; } set { readOnly = value; if (_propertyEditorPG != null) _propertyEditorPG.Enabled = !value; } }
		public bool ChangesPending { get { return SpaceMap.ChangesPending; } set { SpaceMap.ChangesPending = value; UpdateFormText(); } }

		//For dragging the panel with space map around
		private bool _beingDragged;
		private int _grabPointX;
		private int _grabPointY;
		private int _selectPointX;
		private int _selectPointY;
		private bool _mouseMovedSinceMouseDown;

		//for multiple selection
		private bool _beingSelected;

		//For scaling the panel with space map
		public int _curPointX;
		public int _curPointY;

		//For foolish handle behavior and for stupid C# not allowing statics in methods
		bool ___STATIC_E_FSM_SuppressSelectionEvents;//flag to supress selection events while processing one!                         

		//Created controls
		private Form _form;
		private Panel _PSM_p_mapContainer;
		private DoubleBufferedPanel _PSM_p_mapSurface;
		private TextBox _PSM_tb_scapeGoat;
		private ContextMenuStrip _PSM_cms_Main;
		private IContainer components;
		private ToolStripMenuItem _PSM_cms_Main_New;
		private ToolStripMenuItem _PSM_cms_Main_New_Anomaly;
		private ToolStripMenuItem _PSM_cms_Main_New_BlackHole;
		private ToolStripMenuItem _PSM_cms_Main_New_Enemy;
		private ToolStripMenuItem _PSM_cms_Main_New_GenericMesh;
		private ToolStripMenuItem _PSM_cms_Main_New_Monster;
		private ToolStripMenuItem _PSM_cms_Main_New_Neutral;
		private ToolStripMenuItem _PSM_cms_Main_New_Player;
		private ToolStripMenuItem _PSM_cms_Main_New_Whale;
		private ToolStripSeparator _PSM_cms_Main_New_s_1;
		private ToolStripMenuItem _PSM_cms_Main_New_Nebulas;
		private ToolStripMenuItem _PSM_cms_Main_New_Asteroids;
		private ToolStripMenuItem _PSM_cms_Main_New_Mines;
		private ToolStripMenuItem _PSM_cms_Main_Selection;
		private ToolStripMenuItem _PSM_cms_Main_Selection_MoveToCursor;
		private ToolStripMenuItem _PSM_cms_Main_Selection_TurnToCursor;
		private ToolStripMenuItem _PSM_cms_Main_Selection_ToFront;
		private ToolStripMenuItem _PSM_cms_Main_Selection_ToBack;
		private ToolStripSeparator _PSM_cms_Main_Selection_s_1;
		private ToolStripMenuItem _PSM_cms_Main_Selection_All;
		private ToolStripMenuItem _PSM_cms_Main_Selection_None;
		private ToolStripSeparator _PSM_cms_Main_Selection_s_2;
		private ToolStripMenuItem _PSM_cms_Main_New_Station;
		private ContextMenuStrip _PSM_cms_NamedObjectList;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_Delete;
		private ToolStripSeparator _PSM_cms_NamedObjectList_s_2;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_MoveToFront;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_MoveToBack;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_Select;
		private ToolStripSeparator _PSM_cms_NamedObjectList_s_1;
		private ToolStripMenuItem _PSM_cms_Main_Select;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectPreviousObject;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectNextObject;
		private ToolStripSeparator _PSM_cms_NamedObjectList_Select_s_1;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_AddPreviousObject;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_AddNextObject;
		private ToolStripSeparator _PSM_cms_NamedObjectList_Select_s_2;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectAll;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectNone;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectTopObject;
		private ToolStripMenuItem _PSM_cms_NamedObjectList_SelectBottomObject;
		private ToolStripSeparator _PSM_cms_NamedObjectList_Select_s_3;
		private ContextMenuStrip _PSM_cms_NamelessObjectList;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_Select;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectTop;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectBottom;
		private ToolStripSeparator _PSM_cms_NamelessObjectList_Select_s_1;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectPrevious;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectNext;
		private ToolStripSeparator _PSM_cms_NamelessObjectList_Select_s_2;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_AddPrevious;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_AddNext;
		private ToolStripSeparator _PSM_cms_NamelessObjectList_Select_s_3;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectAll;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_SelectNone;
		private ToolStripSeparator _PSM_cms_NamelessObjectList_s_1;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_Delete;
		private ToolStripSeparator _PSM_cms_NamelessObjectList_s_2;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_MoveToFront;
		private ToolStripMenuItem _PSM_cms_NamelessObjectList_MoveToBack;
		private ToolStripMenuItem _PSM_cms_Main_Selection_Delete;

		//The map itself
		public SpaceMap SpaceMap { get; set; }

		#region Helper functions

		//Return rounded value (to nearest odd) //Doesnt fucking work
		public static int RoundToNearestOddInt(double value)
		{
			//int rv = (int)Math.Floor(value + 0.5);
			//return (rv % 2 == 0) ? rv - 1 : rv;
            return (int)Math.Round(value);
		}

		//Return rounded value but not 2
		public static int FloorToIntNot2(double value)
		{
			int rv = (int)Math.Floor(value + 0.5);
			return (rv == 2) ? 1 : rv;
		}

		public double GetMapScale()
		{
            return (double)_PSM_p_mapSurface.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2.0);
		}

		public double GetMapScaleMin()
		{
			return (double)_PSM_p_mapSurface.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2.0) < (double)_PSM_p_mapSurface.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2.0) 
                ? (double)_PSM_p_mapSurface.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2.0) 
                : (double)_PSM_p_mapSurface.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2.0);
		}

		public double GetMapScaleMax()
		{
            return (double)_PSM_p_mapSurface.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2.0) > (double)_PSM_p_mapSurface.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2.0)
                ? (double)_PSM_p_mapSurface.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2.0)
                : (double)_PSM_p_mapSurface.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2.0);
		}

		public int PointXToScreen(int space_map_x)
		{
			return SpaceMap.PointXToScreen(_PSM_p_mapSurface.Width, space_map_x);
		}

		public int PointZToScreen(int space_map_z)
		{
			return SpaceMap.PointZToScreen(_PSM_p_mapSurface.Height, space_map_z);
		}

		public double PointXToScreen(double space_map_x)
		{
			return SpaceMap.PointXToScreen(_PSM_p_mapSurface.Width, space_map_x);
		}

		public double PointZToScreen(double space_map_z)
		{
			return SpaceMap.PointZToScreen(_PSM_p_mapSurface.Height, space_map_z);
		}

		public int PointXToSpaceMap(int screen_x)
		{
			return SpaceMap.PointXToSpaceMap(_PSM_p_mapSurface.Width, screen_x);
		}

		public int PointZToSpaceMap(int screen_z)
		{
			return SpaceMap.PointZToSpaceMap(_PSM_p_mapSurface.Height, screen_z);
		}

		public double PointXToSpaceMap(double screen_x)
		{
			return SpaceMap.PointXToSpaceMap(_PSM_p_mapSurface.Width, screen_x);
		}

		public double PointZToSpaceMap(double screen_z)
		{
			return SpaceMap.PointZToSpaceMap(_PSM_p_mapSurface.Height, screen_z);
		}

		#endregion

		public void AssignForm(Form value = null)
		{
			if (_form != null)
			{
				_form.KeyDown -= _E_form_KeyDown;
			}
			_form = null;

			if (value == null)
				return;

			_form = value;
			_form.KeyDown += _E_form_KeyDown;
		}
		private void AssignPropertyGrid_private_RecursivelyFindControls(Control c, string panelName)
		{
			foreach (Control item in c.Controls)
			{
				if (item.GetType() == typeof(ListBox) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "possibleraces")
				{
					_possibleRacesLB = (ListBox)item;
					_propertyContainerPossiblesPanel = panelName;
				}
				if (item.GetType() == typeof(ListBox) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "possiblevessels")
				{
					_possibleVesselsLB = (ListBox)item;
					_propertyContainerPossiblesPanel = panelName;
				}
				AssignPropertyGrid_private_RecursivelyFindControls(item, panelName);
			}

		}
		public void AssignPropertyGrid(PropertyGrid value = null)
		{
			//Clear everything associated with PG
			if (_propertyContainer != null)
			{
				if (_propertyContainerPossiblesPanel == "Panel1")
					_propertyContainer.Panel1Collapsed = false;
				if (_propertyContainerPossiblesPanel == "Panel2")
					_propertyContainer.Panel2Collapsed = false;
			}
			_propertyContainer = null;
			_propertyContainerPossiblesPanel = "";
			if (_possibleRacesLB != null)
			{
				_possibleRacesLB.Items.Clear();
				_possibleRacesLB.SelectionMode = SelectionMode.None;
			}
			if (_possibleVesselsLB != null)
			{
				_possibleVesselsLB.Items.Clear();
				_possibleVesselsLB.SelectionMode = SelectionMode.None;
			}
			if (_propertyEditorPG != null)
			{
				_propertyEditorPG.Enabled = true;
				_propertyEditorPG.PropertyValueChanged -= _E_propertyGrid_PropertyValueChanged;
			}
			_possibleRacesLB = null;
			_possibleVesselsLB = null;

			if (value == null)
				return;

			_propertyEditorPG = value;
			_propertyEditorPG.PropertyValueChanged += _E_propertyGrid_PropertyValueChanged;
			//Look for container (for collapse action) and possibles (for output)
			if (_propertyEditorPG.Parent != null && _propertyEditorPG.Parent.Parent != null && _propertyEditorPG.Parent.Parent.GetType() == typeof(SplitContainer))
			{
				_propertyContainer = (SplitContainer)_propertyEditorPG.Parent.Parent;

				AssignPropertyGrid_private_RecursivelyFindControls(_propertyContainer.Panel2, "Panel2");
				if (_propertyContainerPossiblesPanel == "") AssignPropertyGrid_private_RecursivelyFindControls(_propertyContainer.Panel1, "Panel1");
			}
		}
		public void AssignNamedObjectsListBox(ListBox value = null)
		{
			if (_namedObjectsLB != null)
			{
				_namedObjectsLB.Items.Clear();
				_namedObjectsLB.SelectionMode = SelectionMode.None;
				_namedObjectsLB.SelectedIndexChanged -= _E_namedObjectsListBox_SelectedIndexChanged;
				_namedObjectsLB.KeyDown -= _E_namedObjectsLB_KeyDown;
				_namedObjectsLB.ContextMenuStrip = null;
			}
			if (_namedObjectsPage != null)
			{
				_namedObjectsPage.Text = "Named objects";
			}
			_namedObjectsLB = null;
			_namedObjectsPage = null;
			_objectsTabControl = null;

			if (value == null)
				return;

			_namedObjectsLB = value;
			_namedObjectsLB.SelectedIndexChanged += _E_namedObjectsListBox_SelectedIndexChanged;
			_namedObjectsLB.KeyDown += _E_namedObjectsLB_KeyDown;
			_namedObjectsLB.ContextMenuStrip =  _PSM_cms_NamedObjectList;
			if (_namedObjectsLB.Parent is TabPage)
			{
				_namedObjectsPage = (TabPage)_namedObjectsLB.Parent;
				_objectsTabControl = (TabControl)_namedObjectsPage.Parent;
			}
		}
		public void AssignNamelessObjectsListBox(ListBox value = null)
		{
			if (_namelessObjectsLB != null)
			{
				_namelessObjectsLB.Items.Clear();
				_namelessObjectsLB.SelectionMode = SelectionMode.None;
				_namelessObjectsLB.SelectedIndexChanged -= _E_namelessObjectsListBox_SelectedIndexChanged;
				_namelessObjectsLB.KeyDown -= _E_namelessObjectsLB_KeyDown;
				_namelessObjectsLB.ContextMenuStrip = null;
			}
			if (_namelessObjectsPage != null)
			{
				_namelessObjectsPage.Text = "Nameless objects";
			}
			_namelessObjectsLB = null;
			_namelessObjectsPage = null;
			_objectsTabControl = null;

			if (value == null)
				return;

			_namelessObjectsLB = value;
			_namelessObjectsLB.SelectedIndexChanged += _E_namelessObjectsListBox_SelectedIndexChanged;
			_namelessObjectsLB.KeyDown += _E_namelessObjectsLB_KeyDown;
			_namelessObjectsLB.ContextMenuStrip = _PSM_cms_NamelessObjectList;
			if (_namelessObjectsLB.Parent is TabPage)
			{
				_namelessObjectsPage = (TabPage)_namelessObjectsLB.Parent;
				_objectsTabControl = (TabControl)_namelessObjectsPage.Parent;
			}
		}
		public void AssignStatusToolStrip(StatusStrip value = null)
		{
			if (_objectsSelectedTSSL != null)
				_objectsSelectedTSSL.Text = "";
			if (_namedObjectsTotalTSSL != null)
				_namedObjectsTotalTSSL.Text = "";
			if (_namelessObjectsTotalTSSL != null)
				_namelessObjectsTotalTSSL.Text = "";
            if (_cursorCoordinates != null)
                _cursorCoordinates.Text = "0, 0";

			_statusTS = null;
			_objectsSelectedTSSL = null;
			_namedObjectsTotalTSSL = null;
			_namelessObjectsTotalTSSL = null;
            _cursorCoordinates = null;

			if (value == null)
				return;

			_statusTS = value;

			foreach (ToolStripItem item in _statusTS.Items)
			{
				if (item.GetType() == typeof(ToolStripStatusLabel) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "objectsselected")
					_objectsSelectedTSSL = (ToolStripStatusLabel)item;
				if (item.GetType() == typeof(ToolStripStatusLabel) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "namedobjectstotal")
					_namedObjectsTotalTSSL = (ToolStripStatusLabel)item;
				if (item.GetType() == typeof(ToolStripStatusLabel) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "namelessobjectstotal")
					_namelessObjectsTotalTSSL = (ToolStripStatusLabel)item;
                if (item.GetType() == typeof(ToolStripStatusLabel) && item.Tag != null && item.Tag.GetType() == typeof(string) && ((string)item.Tag).ToLower() == "cursorcoordinates")
                    _cursorCoordinates = (ToolStripStatusLabel)item;
			}
		}

		public Point DrawSpaceMap_RotateScalePoint(Point p1, double x, double y, double angle, double scale100)
		{
			return new Point((int)Math.Round(x + (Math.Cos(angle) * (p1.X) - Math.Sin(angle) * (p1.Y)) * scale100 / 100.0),
                             (int)Math.Round(y + (Math.Sin(angle) * (p1.X) + Math.Cos(angle) * (p1.Y)) * scale100 / 100.0));
		}

		private int HowManyAsteroidsFitOnArc(double startRadius, double angle, int orbitID, double rem, int max, int skip)
		{
			return HowManyAsteroidsFitOnArc(startRadius, angle, orbitID, ref rem, ref rem, max, skip);
		}

		private int HowManyAsteroidsFitOnArc(double startRadius, double angle, int orbitID, ref double rem, ref double remLast, int max, int skip)
		{
			double radius = GetOrbitRadius(startRadius, orbitID);

			if (radius <= 0) return 0;

			//Arc length in units
			double arcLength = radius * angle;

            int result = (int)Math.Truncate(arcLength / SpaceMap._asteroidWidth / 2.0);

			//Subtract as needed
            result -= (int)Math.Truncate((double)(result) * skip / (max + skip) + rem);
			remLast = rem;
            rem = ((double)(result) * skip / (max + skip) + rem) - Math.Truncate((double)(result) * skip / (max + skip) + rem);

			//Arc smaller than an asteroid can still hold one
			return result >= 1 ? result : 1;
		}

		private int GetOrbitRadius(double startRadius, int orbitID)
		{
            return (int)Math.Round(startRadius + ((orbitID / 2) * Math.Pow(-1, orbitID % 2)) * SpaceMap._asteroidWidth * 2.0);
		}

		private int GetAsteroidReserveOnLastArc(double startRadius, double angle, int maxAsteroids)
		{
			int i = 1;

			while ((maxAsteroids -= HowManyAsteroidsFitOnArc(startRadius, angle, i, 0.0, maxAsteroids, 0)) >= 0)
				i++;
			maxAsteroids += HowManyAsteroidsFitOnArc(startRadius, angle, i, 0.0, maxAsteroids, 0);

			return HowManyAsteroidsFitOnArc(startRadius, angle, i, 0.0, maxAsteroids, 0) - maxAsteroids;
		}

		private KeyValuePair<double, double> GetAsteroidPositionOnArc(double startRadius, double angle, int currentAsteroid, int maxAsteroids)
		{
			int i = 1;
			int reserve = GetAsteroidReserveOnLastArc(startRadius, angle, maxAsteroids) * 93 / 100;

			//This variable will be required to determine if we are on the last orbit or not
			int remAsteroids = maxAsteroids - currentAsteroid;
			//This variable will hold the reminder, so that we evenly remove asteroids from each orbit
			double rem = 0;
			double remLastValue = 0.0;

			while ((currentAsteroid -= HowManyAsteroidsFitOnArc(startRadius, angle, i, ref rem, ref remLastValue, maxAsteroids, reserve)) >= 0)
				i++;
			rem = remLastValue;
			currentAsteroid += HowManyAsteroidsFitOnArc(startRadius, angle, i, rem, maxAsteroids, reserve);

			double res;
			//If we are not on the last orbit we distribute according to the information
			if (currentAsteroid + remAsteroids >= HowManyAsteroidsFitOnArc(startRadius, angle, i, rem, maxAsteroids, reserve)) // Minus two here prevents occasional one or two asteroids from remaining on the next orbit
				res = (currentAsteroid + 0.5) / (HowManyAsteroidsFitOnArc(startRadius, angle, i, rem, maxAsteroids, reserve));
			else //If we are on the last orbit then we evently distribute asteroids on the orbit
                res = (currentAsteroid + 0.5) / (currentAsteroid + remAsteroids);

			return new KeyValuePair<double, double>
				(GetOrbitRadius(startRadius, i), res);
		}

		public bool DrawSpaceMap_GetPoint(double screen_y_scale, Random rnd, NamelessMapObject item, int i, double angleStart, double angleEnd, out double x, out double zy, out double z)
        {
            int j;
            double a, length;
            Coordinates3D c = new Coordinates3D();
            c._allow_out_of_range = true;

            if (item.radius > 0)
            {
                //point on a circle
                switch (item._type)
                {
                    case NamelessMapObjectType.asteroids:
                        KeyValuePair<double, double> result = GetAsteroidPositionOnArc(item.radius, angleEnd - angleStart, i, item.count);
                        if (result.Value < 0)
                            MessageBox.Show("FAIL!");
                        a = 3 * Math.PI / 2 - //Convert from map coords to screen coords
                            (angleStart + ((angleEnd - angleStart) * result.Value)); //start angle plus position inside arc from start to end angle
                        length = result.Key;
                        break;
                    default:
                        a = 3 * Math.PI / 2 - //Convert from map coords to screen coords
                            (angleStart + ((angleEnd - angleStart) * ((i + 0.5) / item.count))); //start angle plus position inside arc from start to end angle
                        length = item.radius;
                        break;
                }

                c.X = item.CoordinatesStart.X + (int)Math.Round(length * Math.Cos(a)) + item.randomRange - rnd.Next(2 * item.randomRange);
                c.Y = item._coordinatesStart.Y;
                c.Z = item.CoordinatesStart.Z + (int)Math.Round(length * Math.Sin(a)) + item.randomRange - rnd.Next(2 * item.randomRange);

            }
            else
            {
				a = item.count == 1 ? 0.5 : (double)i /  (double)(item.count - 1.0);
                c.X = (int)Math.Round(a * item.CoordinatesStart.X + (1 - a) * item.CoordinatesEnd.X + item.randomRange - rnd.Next(2 * (item.randomRange)));
                c.Y = (int)Math.Round(a * item.CoordinatesStart.Y + (1 - a) * item.CoordinatesEnd.Y);
                c.Z = (int)Math.Round(a * item.CoordinatesStart.Z + (1 - a) * item.CoordinatesEnd.Z + item.randomRange - rnd.Next(2 * item.randomRange));
            }

            switch (item._type)
            {
                case NamelessMapObjectType.asteroids:
                    j = SpaceMap._asteroidWidth * 19 / 64;
                    c.X = c.X + j - rnd.Next(2 * j);
                    c.Y = c.Y + j - rnd.Next(2 * j);
                    c.Z = c.Z + j - rnd.Next(2 * j);
                    break;
                default:
                    break;
            }
            //WTF IS THIS?
            //if (c.X < SpaceMap._minX || c.Y < SpaceMap._minY || c.Z < SpaceMap._minZ || c.X > SpaceMap._maxX || c.Y > SpaceMap._maxY || c.Z > SpaceMap._maxZ)
            //{ x = -1; z = -1; zy = -1; return false; } else { }
            x = PointXToScreen(c.X_Scr);
            zy = PointZToScreen(c.Z_Scr) - c.Y_Scr * screen_y_scale;
            z = PointZToScreen(c.Z_Scr);
            return true;
        }

        private void DrawSpaceMap_DrawHeightLine(Graphics g, Pen pen, Brush brush, double x, double zy, double z)
        {
            g.DrawLineD(pen, x, zy, x, z);
            g.FillCircle(brush, x, z, 6.0);
        }

        public void DrawSpaceMap(Graphics g)
		{
            List<IDisposable> ListObjectsToDispose = new List<IDisposable>();

			#region INIT everything and DRAW Background

			int i, j, fleetNumber, podNumber, randomThreshold;
			double x, zy, z, xe, zye, ze, angle, radius1, radius2, radius3, angleStart, angleEnd;
			double sizeAnomaly, sizeVessel, sizeSelection, 
                sizeNamelessBlackholeOuter, sizeNamelessBlackholeInner, sizeNamelessNebula, sizeNamelessAsteroid, sizeNamelessMineBright, sizeNamelessMineDark;
			string name;
			double screen_y_scale = Settings.Current.YScale * GetMapScale();

			//objects for every1
			Random rnd = new Random();

			//DRAW BACKGROUND
			g.Clear(Settings.Current.GetColor(MapColors.MapBackground));

			#endregion

			#region DRAW Nameless Objects

			// calculate sizes
			sizeNamelessNebula = SpaceMap._nebulaWidth * GetMapScale();
            sizeNamelessAsteroid = SpaceMap._asteroidWidth * GetMapScale(); 
            sizeNamelessAsteroid = (sizeNamelessAsteroid < 7.0 ? 7.0 : sizeNamelessAsteroid);
			sizeNamelessMineBright = 9.0;
			sizeNamelessMineDark = 3.0;
            
            // nebulas
			Brush brushNebula = Settings.Current.GetBrush(MapColors.Nebula);
            ListObjectsToDispose.Add(brushNebula);
            Brush brushNebulaBG = Settings.Current.GetBrush(MapColors.NebulaBG);
            ListObjectsToDispose.Add(brushNebulaBG); 
			Pen penNebula = new Pen(brushNebula, 1);
            ListObjectsToDispose.Add(penNebula);
			
            // astreoids
			Brush brushAsteroid = (sizeNamelessAsteroid > 12.5) ? Settings.Current.GetBrush(MapColors.AsteroidBright) : Settings.Current.GetBrush(MapColors.Asteroid);
            ListObjectsToDispose.Add(brushAsteroid);
            Brush brushAsteroidBG = Settings.Current.GetBrush(MapColors.AsteroidBG);
            ListObjectsToDispose.Add(brushAsteroidBG);
            Pen penAsteroid = new Pen(brushAsteroid, (int)Math.Round(sizeNamelessAsteroid / 5));
            ListObjectsToDispose.Add(penAsteroid);
            Pen penAsteroidBG = new Pen(brushAsteroidBG, (int)Math.Round(sizeNamelessAsteroid / 5));
            ListObjectsToDispose.Add(penAsteroidBG);

            // mines
			Brush brushMine = Settings.Current.GetBrush(MapColors.Mine);
            ListObjectsToDispose.Add(brushMine);
            Brush brushMineDark = Settings.Current.GetBrush(MapColors.MineDark);
            ListObjectsToDispose.Add(brushMineDark);
            Brush brushMineBG = Settings.Current.GetBrush(MapColors.MineBG);
            ListObjectsToDispose.Add(brushMineBG);
            Pen penMine = new Pen(brushMine, 1);
            ListObjectsToDispose.Add(penMine);
            Pen penMineDark = new Pen(brushMineDark, 1);
            ListObjectsToDispose.Add(penMineDark);
            Pen penMineBG = new Pen(brushMineBG, 1);
            ListObjectsToDispose.Add(penMineBG);
            
            //height
			Brush brushDarkHeight = Settings.Current.GetBrush(MapColors.MapNamelessHeightMarker);
            ListObjectsToDispose.Add(brushDarkHeight);
            Pen penDarkHeight = new Pen(brushDarkHeight, 1);
            ListObjectsToDispose.Add(penDarkHeight);
            
            // Background objects
			foreach (NamelessMapObject item in SpaceMap.bgNamelessObjects)
			{
				randomThreshold = item._type == NamelessMapObjectType.asteroids ? 6 : 2; //random_threshold
				rnd = new Random(item.randomSeed);
				angleStart = item.A_Start_rad ?? 0.0;
				angleEnd = (item.A_End_rad ?? Math.PI * 2.0);
				if (angleStart > angleEnd)
				{
					double tmp = angleEnd;
					angleEnd = angleStart;
					angleStart = tmp;
				}

				for (i = 0; i < item.count; i++)
				{
					if (!DrawSpaceMap_GetPoint(screen_y_scale, rnd, item, i, angleStart, angleEnd, out x, out zy, out z))
						continue;

					if (!Settings.Current.UseYForNameless)
						zy = z;

                    switch (item._type)
                    {
                        case NamelessMapObjectType.asteroids:
                            g.DrawCircle(penAsteroidBG, x, zy, sizeNamelessAsteroid);
                            break;
                        case NamelessMapObjectType.nebulas:
                            g.DrawCircle(penNebula, x, zy, sizeNamelessNebula);
                            g.FillCircle(brushNebulaBG, x, zy, sizeNamelessNebula);
                            break;
						case NamelessMapObjectType.mines:
                            g.DrawCircle(penMineBG, x, zy, sizeNamelessMineBright);
							break;
					}
				}
			}

			foreach (NamelessMapObject item in SpaceMap.namelessObjects)
			{
				randomThreshold = item._type == NamelessMapObjectType.asteroids ? 6 : 2; //random_threshold
				rnd = new Random(item.randomSeed);
				angleStart = item.A_Start_rad ?? 0.0;
				angleEnd = (item.A_End_rad ?? Math.PI * 2.0);
				if (angleStart > angleEnd)
				{
					double tmp = angleEnd;
					angleEnd = angleStart;
					angleStart = tmp;
				}

				for (i = 0; i < item.count; i++)
				{
					if (!DrawSpaceMap_GetPoint(screen_y_scale, rnd, item, i, angleStart, angleEnd, out x, out zy, out z))
						continue;

					if (!Settings.Current.UseYForNameless)
						zy = z;

                    if (zy < z - randomThreshold)
                        DrawSpaceMap_DrawHeightLine(g, penDarkHeight, brushDarkHeight, x, zy, z);
                    
					switch (item._type)
					{
						case NamelessMapObjectType.asteroids:
                            g.DrawCircle(penAsteroid, x, zy, sizeNamelessAsteroid);
							break;
						case NamelessMapObjectType.nebulas:
                            g.FillCircle(brushNebula, x, zy, sizeNamelessNebula);
							break;
						case NamelessMapObjectType.mines:
                            g.DrawCircle(penMine, x, zy, sizeNamelessMineBright);
                            g.DrawCircle(penMineDark, x, zy, sizeNamelessMineDark);
							break;
					}
					if (zy > z + randomThreshold)
                        DrawSpaceMap_DrawHeightLine(g, penDarkHeight, brushDarkHeight, x, zy, z);
				}
			}


			#endregion

			#region DRAW Grid and Quadrant captions

			Pen penGrid = new Pen(Settings.Current.GetColor(MapColors.MapGridLine), 1);
            ListObjectsToDispose.Add(penGrid);
            for (i = 0; i < 6; i++)
				g.DrawLine(penGrid,
					PointXToScreen(i * 20000),
					PointZToScreen(0),
					PointXToScreen(i * 20000),
					PointZToScreen(SpaceMap._maxZ));
			for (i = 0; i < 6; i++)
				g.DrawLine(penGrid,
					PointXToScreen(0),
					PointZToScreen(i * 20000),
					PointXToScreen(SpaceMap._maxX),
					PointZToScreen(i * 20000));

			//3. DRAW QUANDRANT CAPTIONS
			Brush brushQuadrant = Settings.Current.GetBrush(MapColors.MapGridLine);
            ListObjectsToDispose.Add(brushQuadrant);
            Font fontQuadrant = Settings.Current.GetFont(MapFonts.QuadrantText);
            if (fontQuadrant.Size > 2900.0 * GetMapScale())
            {
                fontQuadrant = new Font(fontQuadrant.FontFamily, (float)(2900.0 * GetMapScale()), fontQuadrant.Style);
                ListObjectsToDispose.Add(fontQuadrant);
            }
			for (i = 0; i < 5; i++)
				for (j = 0; j < 5; j++)
					g.DrawString(Convert.ToChar(Convert.ToInt32('A') + j).ToString() + Convert.ToString(i + 1), fontQuadrant, brushQuadrant,
						PointXToScreen(i * 20000 + 90),
						PointXToScreen(j * 20000 + 20));

			#endregion

			#region INIT Named objects

			Brush brushHeight = Settings.Current.GetBrush(MapColors.MapHeightMarker);
            ListObjectsToDispose.Add(brushHeight);
            Pen penHeight = new Pen(brushHeight, 1);
            ListObjectsToDispose.Add(penHeight);

			//for all fonts that use center output
			StringFormat drawFormat = new StringFormat();
            ListObjectsToDispose.Add(drawFormat);
			drawFormat.Alignment = StringAlignment.Center;
			//for anomalies
			Brush brushAnomaly = Settings.Current.GetBrush(MapColors.Anomaly);
            ListObjectsToDispose.Add(brushAnomaly);
            sizeAnomaly = 1 + 100 * GetMapScale();
			//for vessels
			sizeVessel = 1 + 1500 * GetMapScale();
			sizeVessel = sizeVessel > 30 ? 30 : sizeVessel;
			Point vesselPoint0 = new Point(0, -70);
			Point vesselPoint1 = new Point(50, 50);
			Point vesselPoint2 = new Point(0, 05);
			Point vesselPoint3 = new Point(-50, 50);
			Point[] tp = new Point[4];
			//for enemies
			Brush brushEnemy = Settings.Current.GetBrush(MapColors.Enemy);
            ListObjectsToDispose.Add(brushEnemy);
            Brush brushEnemyBG = Settings.Current.GetBrush(MapColors.EnemyBG);
            ListObjectsToDispose.Add(brushEnemyBG);
            Pen penEnemyBG = new Pen(brushEnemyBG, 1);
            ListObjectsToDispose.Add(penEnemyBG);
			Font fontEnemy = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontEnemy.Size > sizeVessel / 2)
            {
                fontEnemy = new Font(fontEnemy.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f : (float)(sizeVessel / 2.0), fontEnemy.Style);
                ListObjectsToDispose.Add(fontEnemy);
            }
			Font fontFleet = Settings.Current.GetFont(MapFonts.FleetText);
			//for stations
			Brush brushStation = Settings.Current.GetBrush(MapColors.Station);
            ListObjectsToDispose.Add(brushStation);
			Pen penStation = new Pen(brushStation, RoundToNearestOddInt(sizeVessel / 5.0));
            ListObjectsToDispose.Add(penStation);
            Pen penStationBG = new Pen(Settings.Current.GetBrush(MapColors.StationBG), 1);
            ListObjectsToDispose.Add(penStationBG);
			Font fontStation = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontStation.Size > sizeVessel / 2)
            {
                fontStation = new Font(fontStation.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f : (float)(sizeVessel / 2.0), fontStation.Style);
                ListObjectsToDispose.Add(fontStation);
            }
			//for generics
			Brush brushGenericMesh = Settings.Current.GetBrush(MapColors.GenericMesh);
            ListObjectsToDispose.Add(brushGenericMesh);
			Pen penGenericMesh1 = new Pen(brushGenericMesh, RoundToNearestOddInt(sizeVessel / 5.0));
            ListObjectsToDispose.Add(penGenericMesh1);
			Pen penGenericMesh2 = new Pen(brushGenericMesh, FloorToIntNot2(sizeVessel / 10.0));
            ListObjectsToDispose.Add(penGenericMesh2);
			Brush brushGenericMeshSolid = new SolidBrush(Settings.Current.GetColor(MapColors.GenericMesh));
            ListObjectsToDispose.Add(brushGenericMeshSolid);
			Font fontGenericMesh = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontGenericMesh.Size > sizeVessel / 2)
            {
                fontGenericMesh = new Font(fontGenericMesh.FontFamily, sizeVessel / 2.0 < 8.0 ? 8 : (float)(sizeVessel / 2.0), fontGenericMesh.Style);
                ListObjectsToDispose.Add(fontGenericMesh);
            }
            Brush brushGenericMeshBG = Settings.Current.GetBrush(MapColors.GenericMeshBG);
            ListObjectsToDispose.Add(brushGenericMeshBG); 
            Pen penGenericMeshBG = new Pen(brushGenericMeshBG, 1);
            ListObjectsToDispose.Add(penGenericMeshBG);
            //for monsters
            Brush brushMonster = Settings.Current.GetBrush(MapColors.Monster);
            ListObjectsToDispose.Add(brushMonster); 
            Pen penMonster = new Pen(brushMonster, RoundToNearestOddInt(sizeVessel / 10.0));
            ListObjectsToDispose.Add(penMonster); 
            Font fontMonster = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontMonster.Size > sizeVessel / 2)
            {
                fontMonster = new Font(fontMonster.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f : (float)(sizeVessel / 2.0), fontMonster.Style);
                ListObjectsToDispose.Add(fontMonster);
            }
            //for neutrals
            Brush brushNeutral = Settings.Current.GetBrush(MapColors.Neutral);
            ListObjectsToDispose.Add(brushNeutral); 
            Font fontNeutral = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontNeutral.Size > sizeVessel / 2)
            {
                fontNeutral = new Font(fontNeutral.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f: (float)(sizeVessel / 2.0), fontNeutral.Style);
                ListObjectsToDispose.Add(fontNeutral);
            }
            Brush brushNeutralBG = Settings.Current.GetBrush(MapColors.NeutralBG);
            ListObjectsToDispose.Add(brushNeutralBG); 
            Pen penNeutralBG = new Pen(brushNeutralBG, 1);
            ListObjectsToDispose.Add(penNeutralBG);
            //for player(s????)
            Brush brushPlayer = Settings.Current.GetBrush(MapColors.Player);
            ListObjectsToDispose.Add(brushPlayer);
            Font fontPlayer = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontPlayer.Size > sizeVessel / 2)
            {
                fontPlayer = new Font(fontPlayer.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f: (float)(sizeVessel / 2.0), fontPlayer.Style);
                ListObjectsToDispose.Add(fontPlayer);
            }
            Brush brushPlayerBG = Settings.Current.GetBrush(MapColors.PlayerBG);
            ListObjectsToDispose.Add(brushPlayerBG);
            Pen penPlayerBG = new Pen(brushPlayerBG, 1);
            ListObjectsToDispose.Add(penPlayerBG);
            //for whale(s????)
            Brush brushWhale = Settings.Current.GetBrush(MapColors.Whale);
            ListObjectsToDispose.Add(brushWhale);
            Font fontWhale = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontWhale.Size > sizeVessel / 2)
            {
                fontWhale = new Font(fontWhale.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f: (float)(sizeVessel / 2.0), fontWhale.Style);
                ListObjectsToDispose.Add(fontWhale);
            }
            Brush brushWhaleBG = Settings.Current.GetBrush(MapColors.WhaleBG);
            ListObjectsToDispose.Add(brushWhaleBG);
            Pen penWhaleBG = new Pen(brushWhaleBG, 1);
            ListObjectsToDispose.Add(penWhaleBG);
            //for BH
            sizeNamelessBlackholeOuter = 4750 * GetMapScale();
            sizeNamelessBlackholeInner = 550 * GetMapScale();
            Brush brushBlackHole = Settings.Current.GetBrush(MapColors.BlackHole);
            ListObjectsToDispose.Add(brushBlackHole);
            Pen penBlackHoleOuter = new Pen(brushBlackHole, RoundToNearestOddInt(sizeNamelessBlackholeOuter / 8.0));
            ListObjectsToDispose.Add(penBlackHoleOuter);
            Pen penBlackHoleInner = new Pen(brushBlackHole, (sizeNamelessBlackholeOuter < 275) ? RoundToNearestOddInt(sizeNamelessBlackholeOuter / 55.0) : 5);
            ListObjectsToDispose.Add(penBlackHoleInner);
            Font fontBlackHole = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontBlackHole.Size > sizeNamelessBlackholeOuter / 4)
            {
                fontBlackHole = new Font(fontBlackHole.FontFamily, sizeNamelessBlackholeOuter / 4.0 < 8.0 ? 8.0f : (float)(sizeNamelessBlackholeOuter / 4.0), fontBlackHole.Style);
                ListObjectsToDispose.Add(fontBlackHole);
            }
            Pen penBlackHoleBG = new Pen(brushBlackHole);
            ListObjectsToDispose.Add(penBlackHoleBG);
            Font fontAnomaly = Settings.Current.GetFont(MapFonts.ObjectText);
            if (fontAnomaly.Size > sizeVessel / 2)
            {
                fontAnomaly = new Font(fontAnomaly.FontFamily, sizeVessel / 2.0 < 8.0 ? 8.0f : (float)(sizeVessel / 2.0), fontAnomaly.Style);
                ListObjectsToDispose.Add(fontAnomaly);
            }

            #endregion

			#region DRAW Low priority Named Objects (black holes)

			foreach (NamedMapObject mo in SpaceMap.namedObjects)
			{
				x = PointXToScreen(mo.Coordinates.X_Scr);
				zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(mo.Coordinates.Z_Scr);

				if (!Settings.Current.UseYForNamed)
					zy = z;

				switch (mo.TypeToString)
				{
					case "blackHole":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						g.DrawCircle(penBlackHoleOuter, x , zy, sizeNamelessBlackholeOuter * 30.0 / 32);
						g.DrawCircle(penBlackHoleInner, x , zy, sizeNamelessBlackholeInner * 32.0 / 32.0);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontBlackHole, brushBlackHole, x, zy - sizeNamelessBlackholeOuter * 16.0 / 32.0 - fontBlackHole.Size - RoundToNearestOddInt(sizeNamelessBlackholeOuter / 8.0) - 2.0, drawFormat);
						break;
				}
			}

            foreach (NamedMapObject mo in SpaceMap.bgNamedObjects)
            {
                x = PointXToScreen(mo.Coordinates.X_Scr);
                zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
                z = PointZToScreen(mo.Coordinates.Z_Scr);

                if (!Settings.Current.UseYForNamed)
                    zy = z;

                switch (mo.TypeToString)
                {
                    case "blackHole":
                        g.DrawCircle(penBlackHoleBG, x, zy, sizeNamelessBlackholeOuter * 30.0 / 32.0);
                        break;
                }
            }

			#endregion

			#region DRAW BG Named Objects


			foreach (NamedMapObject mo in SpaceMap.bgNamedObjects)
			{
				x = PointXToScreen(mo.Coordinates.X_Scr);
				zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(mo.Coordinates.Z_Scr);

				if (!Settings.Current.UseYForNamed)
					zy = z;

				 switch (mo.TypeToString)
				{
					case "enemy":
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.DrawPolygon(penEnemyBG, tp);
						break;
					case "player":
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.DrawPolygon(penPlayerBG, tp);
						break;
					case "whale":
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel * 0.8);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel * 0.8);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel * 0.8);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel * 0.8);
						g.DrawPolygon(penWhaleBG, tp);
						break;
					case "neutral":
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.DrawPolygon(penNeutralBG, tp);
						break;
					case "station":
						g.DrawCircle(penStationBG, x, zy, sizeVessel * 18.0 / 20.0);
						break;
					case "genericMesh":
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel * 0.8);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel * 0.8);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel * 0.8);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel * 0.8);
						g.DrawCircle(penGenericMeshBG, x, zy, sizeVessel * 14.0 / 20.0);
						break;

				}
			}

			#endregion

			#region DRAW Normal priority objects (all except anomalies and blackholes)

			foreach (NamedMapObject mo in SpaceMap.namedObjects)
			{
				x = PointXToScreen(mo.Coordinates.X_Scr);
				zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(mo.Coordinates.Z_Scr);

				if (!Settings.Current.UseYForNamed)
					zy = z;

				switch (mo.TypeToString)
				{
					case "enemy":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.FillPolygon(brushEnemy, tp);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
                        g.DrawStringD(name, fontEnemy, brushEnemy, x, zy - fontEnemy.Size * 3.0 - 2.0, drawFormat);
						bool fleetNumberNotInt = !Helper.IntTryParse(((NamedMapObject_enemy)mo).fleetnumber, out  fleetNumber);
                        g.DrawStringD(fleetNumberNotInt  ? "x" : fleetNumber == -1 ? "" : fleetNumber >= 0 && fleetNumber <= 99 ? fleetNumber.ToString() : "<!>", fontFleet, brushEnemy, x + sizeVessel * 2.0 / 3.0 + 2.0, zy + sizeVessel * 2.0 / 3.0 + 2.0, StringFormat.GenericDefault);
						break;
					case "player":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.FillPolygon(brushPlayer, tp);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
                        g.DrawStringD(name, fontPlayer, brushPlayer, x, zy - fontPlayer.Size * 3.0 - 2.0, drawFormat);
						break;
					case "whale":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel * 0.80);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel * 0.80);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel * 0.80);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel * 0.80);
						g.FillPolygon(brushWhale, tp);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontWhale, brushWhale, x, zy - fontWhale.Size * 3.0 - 2.0, drawFormat);
                        bool podNumberNotInt = !Helper.IntTryParse(((NamedMapObject_whale)mo).podnumber, out podNumber);
						g.DrawStringD(podNumberNotInt ? "x" : (podNumber >= 0 && podNumber <= 9) ? podNumber.ToString() : "<!>", fontFleet, brushWhale, x + sizeVessel * 2.0 / 3.0 + 2.0, zy + sizeVessel * 2.0 / 3.0 + 2.0, StringFormat.GenericDefault);
						break;
					case "neutral":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel);
						g.FillPolygon(brushNeutral, tp);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontNeutral, brushNeutral, x, zy - fontNeutral.Size * 3.0 - 2.0, drawFormat);
						break;
					case "station":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						g.DrawCircle(penStation, x, zy, sizeVessel * 18.0 / 20.0);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
                        name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontStation, brushStation, x, zy - fontStation.Size * 3.0 - 2.0, drawFormat);
						break;
					case "genericMesh":
						Pen penGenericMeshCurrent1, penGenericMeshCurrent2, penGenericMeshCurrent3;
			            Brush brushGenericMeshCurrent;
			            if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						angle = ((NamedMapObjectA)mo).A_rad;
						tp[0] = DrawSpaceMap_RotateScalePoint(vesselPoint0, x, zy, angle, sizeVessel * 0.80);
						tp[1] = DrawSpaceMap_RotateScalePoint(vesselPoint1, x, zy, angle, sizeVessel * 0.80);
						tp[2] = DrawSpaceMap_RotateScalePoint(vesselPoint2, x, zy, angle, sizeVessel * 0.80);
						tp[3] = DrawSpaceMap_RotateScalePoint(vesselPoint3, x, zy, angle, sizeVessel * 0.80);
						    bool tooDark = false;
						if (!Settings.Current.UseGenericMeshColor)
						{
							brushGenericMeshCurrent = brushGenericMeshSolid;
							penGenericMeshCurrent1 = penGenericMesh1;
							penGenericMeshCurrent2 = penGenericMesh2;
							penGenericMeshCurrent3 = null;
						}
						else if (((NamedMapObject_genericMesh)mo).Color_Blue * 0.0722 + ((NamedMapObject_genericMesh)mo).Color_Green * 0.7152 + ((NamedMapObject_genericMesh)mo).Color_Red * 0.2126 <= Settings.Current.MinimalLuminance)
						{
							//Code to mix the colors
							//genericMeshBrushCurrent = new SolidBrush(((NamedMapObject_genericMesh)mo).Color);
							//genericMeshPenCurrent1 = new Pen(genericMeshBrushCurrent, __ro((db)s_v / 5));
							//genericMeshPenCurrent2 = new Pen(genericMeshBrushCurrent, __rn2((db)s_v / 10));
							//genericMeshPenCurrent3 = genericMeshPen1;
							//genericMeshBrushCurrent = genericMeshSolidBrush;
							//toodark = true;
							brushGenericMeshCurrent = brushGenericMeshSolid;
							penGenericMeshCurrent1 = penGenericMesh1;
							penGenericMeshCurrent2 = penGenericMesh2;
							penGenericMeshCurrent3 = null;
						}
						else
						{
							brushGenericMeshCurrent = new SolidBrush(((NamedMapObject_genericMesh)mo).Color);
							penGenericMeshCurrent1 = new Pen(brushGenericMeshCurrent, RoundToNearestOddInt(sizeVessel / 5.0));
							penGenericMeshCurrent2 = new Pen(brushGenericMeshCurrent, FloorToIntNot2(sizeVessel / 10.0));
							penGenericMeshCurrent3 = null;
						}
						if (tooDark)
							g.DrawCircle(penGenericMeshCurrent3, x, zy, sizeVessel * 18.0 / 20.0);
						    g.DrawCircle(penGenericMeshCurrent1, x, zy, sizeVessel * 14.0 / 20.0);
						g.DrawPolygon(penGenericMeshCurrent2, tp);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontGenericMesh, brushGenericMeshCurrent, x, zy - fontGenericMesh.Size * 3.0 - 2.0, drawFormat);
                        penGenericMeshCurrent1 = null;
                        penGenericMeshCurrent2 = null;
                        penGenericMeshCurrent3 = null;
                        break;
					case "monster":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						g.DrawCircle(penMonster, x, zy, sizeVessel * 18.0 / 20.0);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontMonster, brushMonster, x, zy - fontMonster.Size * 3.0 - 2.0, drawFormat);
						break;
				}
			}

			#endregion

			#region DRAW High priority named objects (anomalies)

			foreach (NamedMapObject mo in SpaceMap.namedObjects)
			{
				x = PointXToScreen(mo.Coordinates.X_Scr);
				zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(mo.Coordinates.Z_Scr);

				if (!Settings.Current.UseYForNamed)
					zy = z;

				switch (mo.TypeToString)
				{
					case "anomaly":
						if (zy < z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						g.FillSquare(brushAnomaly, x, zy, sizeAnomaly);
						if (zy > z)
                            DrawSpaceMap_DrawHeightLine(g, penHeight, brushHeight, x, zy, z);
						name = string.IsNullOrEmpty(mo.name) ? "" : (string.IsNullOrWhiteSpace(mo.name) && Settings.Current.MarkWhitespaceNamesOnSpaceMap ? "<" + mo.name + ">" : mo.name);
						g.DrawStringD(name, fontAnomaly, brushAnomaly, x, zy - fontAnomaly.Size * 3.0 - 2.0, drawFormat);
						break;
				}
			}

			#endregion

			#region DRAW Selection brackets

			Pen penSelection = new Pen(Settings.Current.GetColor(MapColors.MapSelection));
            ListObjectsToDispose.Add(penSelection);
            Pen penSelectionDark = new Pen(Settings.Current.GetColor(MapColors.MapSelectionDark));
            ListObjectsToDispose.Add(penSelectionDark);
            sizeSelection = 10 + 1500 * GetMapScale();
			sizeSelection = sizeSelection > 40 ? 40 : sizeSelection;

			if (SpaceMap.SelectionNameless != null)
			{
                x = PointXToScreen(SpaceMap.SelectionNameless.CoordinatesStart.X_Scr);
                zy = PointZToScreen(SpaceMap.SelectionNameless.CoordinatesStart.Z_Scr) - SpaceMap.SelectionNameless.CoordinatesStart.Y_Scr * screen_y_scale;
                z = PointZToScreen(SpaceMap.SelectionNameless.CoordinatesStart.Z_Scr);
                xe = PointXToScreen(SpaceMap.SelectionNameless.CoordinatesEnd.X_Scr);
                zye = PointZToScreen(SpaceMap.SelectionNameless.CoordinatesEnd.Z_Scr) - SpaceMap.SelectionNameless.CoordinatesEnd.Y_Scr * screen_y_scale;
                ze = PointZToScreen(SpaceMap.SelectionNameless.CoordinatesEnd.Z_Scr);
				radius1 = (SpaceMap.SelectionNameless.radius + SpaceMap.SelectionNameless.randomRange) * GetMapScale();
				radius2 = SpaceMap.SelectionNameless.radius * GetMapScale();
				radius3 = radius2 * 2 - radius1;

				angleStart = SpaceMap.SelectionNameless.A_Start_deg ?? 0.0;
				angleEnd = SpaceMap.SelectionNameless.A_End_deg ?? 360.0;
				if (angleStart > angleEnd)
				{
					double tmp = angleEnd;
					angleEnd = angleStart;
					angleStart = tmp;
				}
				angleEnd = angleEnd - angleStart;
				angleStart = angleStart - 90;

				//if (!_use_y_for_nameless)
				if (true)
				{
					zy = z;
					zye = ze;
				}

				if (SpaceMap.SelectionNameless.radius > 0)
				{
					if (SpaceMap.SelectionNameless.A_Start_deg == null && SpaceMap.SelectionNameless.A_End_deg == null)
					{
						g.DrawCircle(penSelection, x, zy, radius2 * 2.0);
					}
					else
					{
						//Dark circle
                        g.DrawCircle(penSelectionDark, x, zy, radius2 * 2.0);
						//Bright arc
						g.DrawCircleArc(penSelection, x, zy, radius2 * 2.0, angleStart, angleEnd);
						//Lines that show the arc boundaries
						g.DrawLineD(penSelection, x, zy, x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0), 
                                                        zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0));
                        g.DrawLineD(penSelection, x, zy, x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0),
                                                        zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0));
						//Angles on the end of the arc boundaries
						g.DrawLineD(penSelection,
							 x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0),
							zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0),
							 x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0) + sizeSelection / 4.0 * Math.Sin(Math.PI * 4.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0),
							zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0) + sizeSelection / 4.0 * Math.Cos(Math.PI * 4.0 / 2.0 - angleStart / 360.0 * Math.PI * 2.0));
						g.DrawLineD(penSelection,
							 x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0),
							zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0),
							 x - (radius1 + sizeSelection * 1.5) * Math.Sin(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0) + sizeSelection / 4.0 * Math.Sin(Math.PI * 2.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0),
							zy - (radius1 + sizeSelection * 1.5) * Math.Cos(Math.PI * 3.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0) + sizeSelection / 4.0 * Math.Cos(Math.PI * 2.0 / 2.0 - (angleStart + angleEnd) / 360.0 * Math.PI * 2.0));
					}

					if (SpaceMap.SelectionNameless.randomRange > 0)
					{
						g.DrawCircle(penSelectionDark, x, zy, radius1 * 2.0);
						if (radius3 > 0)
                            g.DrawCircle(penSelectionDark, x, zy, radius3 * 2.0);
					}
					//Crosshair on the rad2 of the circle
                    g.DrawLineD(penSelection, x, zy - radius2, x, zy - radius2 - sizeSelection / 2.0);
                    g.DrawLineD(penSelection, x, zy + radius2, x, zy + radius2 + sizeSelection / 2.0);
                    g.DrawLineD(penSelection, x - radius2, zy, x - radius2 - sizeSelection / 2.0, zy);
                    g.DrawLineD(penSelection, x + radius2, zy, x + radius2 + sizeSelection / 2.0, zy);
				}
				else
				{
					//Get angle from start to end
					// Negate X and Y values
					double pxRes = x - xe;
					double pyRes = zy - zye;
					double angleSpan = 0.0;
					// Calculate the angle
					if (pxRes == 0.0)
					{
						if (pyRes == 0.0) angleSpan = 0.0;
						else
							if (pyRes > 0.0) angleSpan = System.Math.PI / 2.0;
							else angleSpan = System.Math.PI * 3.0 / 2.0;
					}
					else if (pyRes == 0.0)
					{
						if (pxRes > 0.0) angleSpan = 0.0;
						else angleSpan = System.Math.PI;
					}
					else
					{
						if (pxRes < 0.0)
							angleSpan = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
						else
							if (pyRes < 0.0) angleSpan = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);
							else angleSpan = System.Math.Atan(pyRes / pxRes);
					}

                    double angleLeft = (angleSpan - Math.PI / 2.0) * 360.0 / 2.0 / Math.PI;
                    double angleRight = (angleSpan + Math.PI / 2.0) * 360.0 / 2.0 / Math.PI;
					while (angleLeft < 0.0) angleLeft += 360.0; 
                    while (angleRight < 0.0) angleRight += 360.0; 
                    while (angleLeft >= 360.0) angleLeft -= 360.0; 
                    while (angleRight > 360.0) angleRight -= 360.0;

					g.DrawLineD(penSelection, x, zy, xe, zye);
					if (SpaceMap.SelectionNameless.randomRange > 0)
					{
						g.DrawLineD(penSelectionDark,
                              x + radius1 * Math.Cos(angleSpan + Math.PI / 2.0),
                             zy + radius1 * Math.Sin(angleSpan + Math.PI / 2.0),
                             xe + radius1 * Math.Cos(angleSpan + Math.PI / 2.0),
                            zye + radius1 * Math.Sin(angleSpan + Math.PI / 2.0));
						g.DrawLineD(penSelectionDark,
                              x - radius1 * Math.Cos(angleSpan + Math.PI / 2.0),
                             zy - radius1 * Math.Sin(angleSpan + Math.PI / 2.0),
                             xe - radius1 * Math.Cos(angleSpan + Math.PI / 2.0),
                            zye - radius1 * Math.Sin(angleSpan + Math.PI / 2.0));
						if (radius1 >= 1.0)
						{
							g.DrawCircleArc(penSelectionDark, x, zy, radius1 * 2.0, angleLeft, 180.0);
							g.DrawCircleArc(penSelectionDark, xe, zye, radius1 * 2.0, angleRight, 180.0);
						}
					}

					g.DrawLineD(penSelection, x, zy - radius2, x, zy - radius2 - sizeSelection / 2.0);
					g.DrawLineD(penSelection, x, zy + radius2, x, zy + radius2 + sizeSelection / 2.0);
					g.DrawLineD(penSelection, x - radius2, zy, x - radius2 - sizeSelection / 2.0, zy);
					g.DrawLineD(penSelection, x + radius2, zy, x + radius2 + sizeSelection / 2.0, zy);
					g.DrawLineD(penSelectionDark, xe, zye - radius2, xe, zye - radius2 - sizeSelection / 2.0);
					g.DrawLineD(penSelectionDark, xe, zye + radius2, xe, zye + radius2 + sizeSelection / 2.0);
					g.DrawLineD(penSelectionDark, xe - radius2, zye, xe - radius2 - sizeSelection / 2.0, zye);
					g.DrawLineD(penSelectionDark, xe + radius2, zye, xe + radius2 + sizeSelection / 2.0, zye);
				}
			}
			else
			{
				foreach (NamedMapObject mo in SpaceMap.SelectionNamed)
				{
					x = PointXToScreen(mo.Coordinates.X_Scr);
					zy = PointZToScreen(mo.Coordinates.Z_Scr) - mo.Coordinates.Y_Scr * screen_y_scale;
					z = PointZToScreen(mo.Coordinates.Z_Scr);

					if (!Settings.Current.UseYForNamed)
						zy = z;

					switch (mo.TypeToString)
					{
						case "anomaly":
							g.DrawSquare(penSelection, x, zy, 20 + sizeAnomaly);
							break;
						case "blackHole":
							g.DrawSquare(penSelection, x, zy, sizeNamelessBlackholeOuter * 36.0 / 32);
							break;
						default:
							g.DrawSquare(penSelection, x, zy, sizeSelection);
							break;
					}
				}
			}

			#endregion

			#region DRAW Selection mean point and selection rectangle

			int vecx, vecy, vecz;
			if (SpaceMap.Selection_GetCenterOfMass(out vecx, out vecy, out vecz))
			{
				x = PointXToScreen(vecx);
				zy = PointZToScreen(vecz) - vecy * screen_y_scale;
				z = PointZToScreen(vecz);

                g.DrawLineD(penSelectionDark, x, z - sizeSelection / 2.0, x, z + sizeSelection / 2.0);
                g.DrawLineD(penSelectionDark, x - sizeSelection / 2.0, z, x + sizeSelection / 2.0, z);
			}

			//7. Draw selection rectangle when mass-selecting
            Brush brushSelectionRectangle = Settings.Current.GetBrush(MapColors.MapSelectionRectangle);
            ListObjectsToDispose.Add(brushSelectionRectangle);
            Pen penSelectionRectangle = new Pen(brushSelectionRectangle);
            ListObjectsToDispose.Add(penSelectionRectangle);

			if (GetFocused() && _beingSelected && (_curPointX != _selectPointX || _curPointY != _selectPointY))
			{
				xe = Math.Max(_curPointX, _selectPointX);
				ze = Math.Max(_curPointY, _selectPointY);
				x = Math.Min(_curPointX, _selectPointX);
				z = Math.Min(_curPointY, _selectPointY);

				g.DrawRectangleD(penSelectionRectangle, x, z, xe - x, ze - z);
			}

			#endregion

            #region DRAW Special Object

            if (SpaceMap.SelectionSpecial is SpecialMapObject_DestroyCircle)
            {
                SpecialMapObject_DestroyCircle item = (SpecialMapObject_DestroyCircle)SpaceMap.SelectionSpecial;
                Brush brushHeightSpecial = new HatchBrush(HatchStyle.WideDownwardDiagonal, penHeight.Color);
                ListObjectsToDispose.Add(brushHeightSpecial);
                Pen penHeightSpecial = new Pen(brushHeightSpecial);
                ListObjectsToDispose.Add(penHeightSpecial);

                x = PointXToScreen(item.Coordinates.X_Scr);
				zy = PointZToScreen(item.Coordinates.Z_Scr) - item.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(item.Coordinates.Z_Scr);
				radius2 = item.Radius * GetMapScale();

				if (!Settings.Current.UseYForSelection)
					zy = z;

				if (zy < z)
				{
					g.DrawLineD(penHeightSpecial, x - radius2, zy, x - radius2, z);
					g.DrawLineD(penHeightSpecial, x + radius2, zy, x + radius2, z);
				}
				//Crosshair at the middle of the bg circle
                g.DrawLineD(penDarkHeight, x, z - sizeSelection / 4.0, x, z + sizeSelection / 4.0);
                g.DrawLineD(penDarkHeight, x - sizeSelection / 4.0, z, x + sizeSelection / 4.0, z);
				//Line from the middle of zy to the middle of the z
				g.DrawLineD(penHeightSpecial, x, zy, x, z);
                //Circle
                g.DrawCircle(penSelection, x, zy, radius2 * 2.0);
                //Crosshair around the radius
                g.DrawLineD(penSelection, x, zy - radius2, x, zy - radius2 - sizeSelection / 2.0);
                g.DrawLineD(penSelection, x, zy + radius2, x, zy + radius2 + sizeSelection / 2.0);
                g.DrawLineD(penSelection, x - radius2, zy, x - radius2 - sizeSelection / 2.0, zy);
                g.DrawLineD(penSelection, x + radius2, zy, x + radius2 + sizeSelection / 2.0, zy);
				//Crosshair at the middle of the circle                                      
                g.DrawLineD(penSelection, x, zy - sizeSelection / 4.0, x, zy + sizeSelection / 4.0);
                g.DrawLineD(penSelection, x - sizeSelection / 4.0, zy, x + sizeSelection / 4.0, zy);
				if (zy > z)
				{
                    g.DrawLineD(penHeightSpecial, x - radius2, zy, x - radius2, z);
                    g.DrawLineD(penHeightSpecial, x + radius2, zy, x + radius2, z);
				}
				//y circle
				if (Settings.Current.UseYForSelection && radius2 >= 1.0)
				{
					int num = 36;
					for (i = 0; i < num; i++)
                        g.DrawCircleArc(penHeight, x, z, radius2 * 2.0, i * 360.0 / num, 360.0 / num / 4.0); 
				}
            }

			if (SpaceMap.SelectionSpecial is SpecialMapObject_PointTarget)
			{
				SpecialMapObject_PointTarget item = (SpecialMapObject_PointTarget)SpaceMap.SelectionSpecial;
				Brush brushHeightSpecial = new HatchBrush(HatchStyle.WideDownwardDiagonal, penHeight.Color);
                ListObjectsToDispose.Add(brushHeightSpecial);
                Pen penHeightSpecial = new Pen(brushHeightSpecial);
                ListObjectsToDispose.Add(penHeightSpecial);

				x = PointXToScreen(item.Coordinates.X_Scr);
				zy = PointZToScreen(item.Coordinates.Z_Scr) - item.Coordinates.Y_Scr * screen_y_scale;
				z = PointZToScreen(item.Coordinates.Z_Scr);
				
				if (!Settings.Current.UseYForSelection)
					zy = z;

				//Crosshair at the middle of the bg circle
                g.DrawLineD(penDarkHeight, x, z - sizeSelection / 4.0, x, z + sizeSelection / 4.0);
                g.DrawLineD(penDarkHeight, x - sizeSelection / 4.0, z, x + sizeSelection / 4.0, z);
				//Line from the middle of zy to the middle of the z
				g.DrawLineD(penHeightSpecial, x, zy, x, z);
				//Crosshair at the middle of the circle
                g.DrawLineD(penSelection, x, zy - sizeSelection / 4.0, x, zy + sizeSelection / 4.0);
                g.DrawLineD(penSelection, x - sizeSelection / 4.0, zy, x + sizeSelection / 4.0, zy);
			}

            if (SpaceMap.SelectionSpecial is SpecialMapObject_BoxSphere)
            {
                SpecialMapObject_BoxSphere item = (SpecialMapObject_BoxSphere)SpaceMap.SelectionSpecial;
                Brush brushHeightSpecial = new HatchBrush(HatchStyle.WideDownwardDiagonal, penHeight.Color);
                ListObjectsToDispose.Add(brushHeightSpecial);
                Pen penHeightSpecial = new Pen(brushHeightSpecial);
                ListObjectsToDispose.Add(penHeightSpecial);

                if (item.SphereBox == SpecialMapObjectSphereBox.Sphere)
                {
                    x = PointXToScreen(item.CoordinatesStart.X_Scr);
                    zy = PointZToScreen(item.CoordinatesStart.Z_Scr) - item.CoordinatesStart.Y_Scr * screen_y_scale;
                    z = PointZToScreen(item.CoordinatesStart.Z_Scr);
                    radius2 = item.Radius * GetMapScale();
                    radius3 = sizeSelection / 2 > radius2 / 2 && item.InOut == SpecialMapObjectInOut.Outside ? radius2 / 2 : sizeSelection / 2;
                    radius3 = item.InOut == SpecialMapObjectInOut.Inside ? radius3 : -radius3;

                    if (!Settings.Current.UseYForSelection)
                        zy = z;

                    if (zy < z)
                    {
                        g.DrawLineD(penHeightSpecial, x - radius2, zy, x - radius2, z);
                        g.DrawLineD(penHeightSpecial, x + radius2, zy, x + radius2, z);
                    }
                    //Crosshair at the middle of the bg circle
                    g.DrawLineD(penDarkHeight, x, z - sizeSelection / 4.0, x, z + sizeSelection / 4.0);
                    g.DrawLineD(penDarkHeight, x - sizeSelection / 4.0, z, x + sizeSelection / 4.0, z);
                    //Line froDm the middle of zy to the middle of the z
                    g.DrawLineD(penHeightSpecial, x, zy, x, z);
                    //Circle
                    g.DrawCircle(penSelection, x, zy, radius2 * 2.0);
                    //Crosshair around the radius
                    g.DrawLineD(penSelection, x, zy - radius2, x, zy - radius2 - radius3);
                    g.DrawLineD(penSelection, x, zy + radius2, x, zy + radius2 + radius3);
                    g.DrawLineD(penSelection, x - radius2, zy, x - radius2 - radius3, zy);
                    g.DrawLineD(penSelection, x + radius2, zy, x + radius2 + radius3, zy);
                    //Crosshair at the middle of the circle
                    g.DrawLineD(penSelection, x, zy - sizeSelection / 4.0, x, zy + sizeSelection / 4.0);
                    g.DrawLineD(penSelection, x - sizeSelection / 4.0, zy, x + sizeSelection / 4.0, zy);
                    if (zy > z)
                    {
                        g.DrawLineD(penHeightSpecial, x - radius2, zy, x - radius2, z);
                        g.DrawLineD(penHeightSpecial, x + radius2, zy, x + radius2, z);
                    }
                    //y circle
                    if (Settings.Current.UseYForSelection && radius2 >= 1.0)
                    {
                        int num = 36;
                        for (i = 0; i < num; i++)
                            g.DrawCircleArc(penHeight, x, z, radius2 * 2.0, i * 360.0 / num, 360.0 / num / 4.0);
                    }
                }
                else
                {
                    x = PointXToScreen(Math.Min(item.CoordinatesStart.X_Scr, item.CoordinatesEnd.X_Scr));
                    z = PointZToScreen(Math.Min(item.CoordinatesStart.Z_Scr, item.CoordinatesEnd.Z_Scr));
                    xe = PointXToScreen(Math.Max(item.CoordinatesStart.X_Scr, item.CoordinatesEnd.X_Scr));
                    ze = PointZToScreen(Math.Max(item.CoordinatesStart.Z_Scr, item.CoordinatesEnd.Z_Scr));
                    radius2 = 3.5;
                    radius3 = sizeSelection / 2 > Math.Min(xe - x, ze - z) / 2 && item.InOut == SpecialMapObjectInOut.Outside ? Math.Min(xe - x, ze - z) / 2 : sizeSelection / 2;
                    radius3 = item.InOut == SpecialMapObjectInOut.Inside ? radius3 : -radius3;

                    g.DrawRectangleD(penSelection, x, z, xe - x, ze - z);

                    g.DrawLineD(penSelection, x, z / 2.0 + ze / 2.0, x - radius3, z / 2.0 + ze / 2.0);
                    g.DrawLineD(penSelection, x / 2.0 + xe / 2.0, z, x / 2.0 + xe / 2.0, z - radius3);
                    g.DrawLineD(penSelection, xe, z / 2 + ze / 2.0, xe + radius3, z / 2.0 + ze / 2.0);
                    g.DrawLineD(penSelection, x / 2.0 + xe / 2.0, ze, x / 2.0 + xe / 2.0, ze + radius3);
                    
                    x = PointXToScreen(item.CoordinatesStart.X_Scr);
                    z = PointZToScreen(item.CoordinatesStart.Z_Scr);
                    xe = PointXToScreen(item.CoordinatesEnd.X_Scr);
                    ze = PointZToScreen(item.CoordinatesEnd.Z_Scr);

                    g.DrawCircle(penSelection, x, z, radius2 * 2.0);
                    g.DrawSquare(penSelection, xe, ze, radius2 * 2.0);
                }
            }
            #endregion

            #region Dispose
            foreach (IDisposable objectToDispose in ListObjectsToDispose)
                objectToDispose.Dispose();
            #endregion
        }

		public void Selection_Update()
		{
			if (_propertyEditorPG != null)
			{
                if (SpaceMap.SelectionSpecial != null)
                {
                    _propertyEditorPG.SelectedObjects = new object[] { SpaceMap.SelectionSpecial };
                    if (_propertyContainerPossiblesPanel == "Panel1") _propertyContainer.Panel1Collapsed = true;
                    if (_propertyContainerPossiblesPanel == "Panel2") _propertyContainer.Panel2Collapsed = true;
                }
                else if (SpaceMap.SelectionNameless != null)
                {
                    _propertyEditorPG.SelectedObjects = new object[] { SpaceMap.SelectionNameless };
                    if (_propertyContainerPossiblesPanel == "Panel1") _propertyContainer.Panel1Collapsed = true;
                    if (_propertyContainerPossiblesPanel == "Panel2") _propertyContainer.Panel2Collapsed = true;
                }
                else
                {
                    _propertyEditorPG.SelectedObjects = SpaceMap.SelectionNamed;
                    if (_propertyContainerPossiblesPanel == "Panel1") _propertyContainer.Panel1Collapsed = !NamedMapObject.IsPropertyPresent("raceKeys", (NamedMapObject[])_propertyEditorPG.SelectedObjects) || _propertyEditorPG.SelectedObjects.Count() == 0;
                    if (_propertyContainerPossiblesPanel == "Panel2") _propertyContainer.Panel2Collapsed = !NamedMapObject.IsPropertyPresent("raceKeys", (NamedMapObject[])_propertyEditorPG.SelectedObjects) || _propertyEditorPG.SelectedObjects.Count() == 0;
                }
				UpdatePossibleLists();
			}
			UpdateObjectsText();

		}

		/// <summary>
		/// Assign current selection from the listbox control
		/// </summary>
		/// <param name="named">Wether it was named or nameless object that was selected</param>
		public void Selection_FromControl(bool named)
		{
			int i;

			if (named && _namedObjectsLB != null)
			{
				if (_namedObjectsLB.SelectionMode == SelectionMode.None)
					return;

				___STATIC_E_FSM_SuppressSelectionEvents = true;

				SpaceMap.SelectionNameless = null;
				for (i = 0; i < _namedObjectsLB.Items.Count; i++)
					SpaceMap.namedObjects[i].Selected = _namedObjectsLB.GetSelected(i);
				if (_namelessObjectsLB != null)
					if (_namelessObjectsLB.SelectionMode != SelectionMode.None)
						for (i = 0; i < _namelessObjectsLB.Items.Count; i++)
							_namelessObjectsLB.SetSelected(i, false);

				___STATIC_E_FSM_SuppressSelectionEvents = false;
			}
			if (!named && _namelessObjectsLB != null)
			{
				if (_namelessObjectsLB.SelectionMode == SelectionMode.None)
					return;

				___STATIC_E_FSM_SuppressSelectionEvents = true;

				SpaceMap.SelectionNameless = null;
				for (i = 0; i < _namelessObjectsLB.Items.Count; i++)
					if (_namelessObjectsLB.GetSelected(i))
						SpaceMap.SelectionNameless = SpaceMap.namelessObjects[i];
				if (_namedObjectsLB != null)
					if (_namedObjectsLB.SelectionMode != SelectionMode.None)
						for (i = 0; i < _namedObjectsLB.Items.Count; i++)
							_namedObjectsLB.SetSelected(i, false);

				___STATIC_E_FSM_SuppressSelectionEvents = false;
			}
		}

		public void UpdateObjectLists()
		{
			int i;
			string max = "";
			if (_namedObjectsLB != null)
			{
				___STATIC_E_FSM_SuppressSelectionEvents = true;

				_namedObjectsLB.Items.Clear();
				_namedObjectsLB.SelectionMode = (SpaceMap.namedObjects.Count == 0) ? SelectionMode.None : SelectionMode.MultiExtended;
				for (i = 0; i < SpaceMap.namedObjects.Count; i++)
				{
					_namedObjectsLB.Items.Add("[" + (i + 1).ToString("000") + "] " + SpaceMap.namedObjects[i].TypeToStringShort
						+ (SpaceMap.namedObjects[i].name != "" ? " - " + SpaceMap.namedObjects[i].name : ""));
					if (SpaceMap.namedObjects[i].Selected)
						_namedObjectsLB.SetSelected(_namedObjectsLB.Items.Count - 1, true);
					if (_namedObjectsLB.Items[_namedObjectsLB.Items.Count - 1].ToString().Length > max.Length)
						max = _namedObjectsLB.Items[_namedObjectsLB.Items.Count - 1].ToString();
				}
				if (SpaceMap.namedObjects.Count == 0)
				{
					//_namedObjectsLB.Items.Add("< nothing >");
				}
                _namedObjectsLB.ColumnWidth = (int)Math.Round(_namedObjectsLB.CreateGraphics().MeasureString(max, _namedObjectsLB.Font).Width);
				
				___STATIC_E_FSM_SuppressSelectionEvents = false;
			}

			if (_namelessObjectsLB != null)
			{
				___STATIC_E_FSM_SuppressSelectionEvents = true;

				_namelessObjectsLB.Items.Clear();
				_namelessObjectsLB.SelectionMode = (SpaceMap.namelessObjects.Count == 0) ? SelectionMode.None : SelectionMode.One;
				for (i = 0; i < SpaceMap.namelessObjects.Count; i++)
				{
					_namelessObjectsLB.Items.Add("[" + (i + 1).ToString("000") + "] " + SpaceMap.namelessObjects[i].TypeToStringShort + " - x" + SpaceMap.namelessObjects[i].count);
					if (SpaceMap.namelessObjects[i] == SpaceMap.SelectionNameless)
						_namelessObjectsLB.SetSelected(_namelessObjectsLB.Items.Count - 1, true);
					if (_namelessObjectsLB.Items[_namelessObjectsLB.Items.Count - 1].ToString().Length > max.Length)
						max = _namelessObjectsLB.Items[_namelessObjectsLB.Items.Count - 1].ToString();
				}
				if (SpaceMap.namelessObjects.Count == 0)
				{
					//_namelessObjectsLB.Items.Add("< nothing >");
				}
                _namelessObjectsLB.ColumnWidth = (int)Math.Round(_namelessObjectsLB.CreateGraphics().MeasureString(max, _namelessObjectsLB.Font).Width);

				___STATIC_E_FSM_SuppressSelectionEvents = false;
			}
		}

		public void UpdatePossibleLists()
		{
			Vessel v;

			List<string> checkedRaceNames;
			List<string> checkedRaceKeys;
			List<string> checkedVesselClassNames;
			List<string> checkedVesselBroadTypes;

			checkedRaceNames = new List<string>();
			checkedRaceKeys = new List<string>();
			checkedVesselClassNames = new List<string>();
			checkedVesselBroadTypes = new List<string>();

			if (_possibleRacesLB != null) _possibleRacesLB.Items.Clear();
			if (_possibleVesselsLB != null) _possibleVesselsLB.Items.Clear();

			NamedMapObjectArhK mo = new NamedMapObjectArhK();
			mo.Copy(true, SpaceMap.SelectionNamed);

			if (!(mo.hullID == "" || mo.hullID == null))
			{
				if (!Settings.VesselData.vesselList.ContainsKey(mo.hullID))
					return;

				v = Settings.VesselData.vesselList[mo.hullID];

				if (_possibleRacesLB != null) _possibleRacesLB.Items.Add(Settings.VesselData.RaceToString(v.side));
				if (_possibleVesselsLB != null) _possibleVesselsLB.Items.Add(Settings.VesselData.VesselToString(v.uniqueID));

				return;
			}

			foreach (string item in mo.RaceNames.Split(' '))
				if (item != "")
					checkedRaceNames.Add(item);
			foreach (string item in mo.RaceKeys.Split(' '))
				if (item != "")
					checkedRaceKeys.Add(item);
			foreach (string item in mo.VesselBroadTypes.Split(' '))
				if (item != "")
					checkedVesselBroadTypes.Add(item);
			foreach (string item in mo.VesselClassNames.Split(' '))
				if (item != "")
					checkedVesselClassNames.Add(item);

			//Prepare races list
			List<string> possibleRaceIDs = Settings.VesselData.GetPossibleRaceIDs(checkedRaceNames, checkedRaceKeys);

			//Output races list
			foreach (string item in possibleRaceIDs)
				if (_possibleRacesLB != null) _possibleRacesLB.Items.Add(Settings.VesselData.RaceToString(item));

			//Prepare vessel list
			List<string> possibleVesselIDs = Settings.VesselData.GetPossibleVesselIDs(possibleRaceIDs, checkedVesselClassNames, checkedVesselBroadTypes);

			//Finally output the vessel list
			foreach (string id in possibleVesselIDs)
				if (_possibleVesselsLB != null) _possibleVesselsLB.Items.Add(Settings.VesselData.VesselToString(id));

		}

		public void UpdateObjectsText()
		{
			int count;

			count = SpaceMap.Selection_Count;
			if (_objectsSelectedTSSL != null)
			{
				switch (count)
				{
					case 0:
						_objectsSelectedTSSL.Text = "Nothing selected";
						break;
					case 1:
						_objectsSelectedTSSL.Text = count.ToString() + " named selected";
						break;
					default:
						_objectsSelectedTSSL.Text = count.ToString() + " named selected";
						break;
				}
				if (SpaceMap.SelectionNameless != null)

					_objectsSelectedTSSL.Text = "Nameless selected";
			}
			count = SpaceMap.namedObjects.Count;

			if (_namedObjectsPage != null)
				_namedObjectsPage.Text = "Named objects [" + count.ToString() + "]";
			if (_namedObjectsTotalTSSL != null)
			{
				switch (count)
				{
					case 1:
						_namedObjectsTotalTSSL.Text = count.ToString() + " named total";
						break;
					default:
						_namedObjectsTotalTSSL.Text = count.ToString() + " named total";
						break;
				}
			}

			count = SpaceMap.namelessObjects.Count;
			if (_namelessObjectsPage != null)
				_namelessObjectsPage.Text = "Nameless objects [" + count.ToString() + "]";
			if (_namelessObjectsTotalTSSL != null)
				_namelessObjectsTotalTSSL.Text = count.ToString() + " nameless total";

            /*if (SpaceMap.UnMappableStorage != null && SpaceMap.UnMappableStorage.Count > 0)
            {
                _unmappable.Text = SpaceMap.UnMappableStorage.Count + " unmappable objects.";
            }
            else
            {
                _unmappable.Text = string.Empty;
            }*/
		}

        public void UpdateCoordinatesText()
        {
            if (_cursorCoordinates != null)
                _cursorCoordinates.Text = ((long)SpaceMap._maxX -PointXToSpaceMap(_curPointX)).ToString() + ", " + PointZToSpaceMap(_curPointY).ToString();
        }

		public void RegisterChange(string shortDescription, bool preserveObjects = false, bool clean=false)
		{
            ChangesPending = !clean;
			UpdateFormText();

			SpaceMap.RegisterChange(shortDescription);
			
			Selection_Update();
			if (!preserveObjects)
				UpdateObjectLists();
			Redraw();
		}

		private void UpdateFormText()
		{
			if (_form == null)
				return;
			if (ChangesPending)
			{
				if (_form.Text[0] != '*')
					_form.Text = "* " + _form.Text;
			}
			else
			{
				if (_form.Text[0] == '*')
					_form.Text =_form.Text.Substring(1,_form.Text.Length-1);
			}
		}

		/// <summary>
		/// Force redraw (invalidate the canvas)
		/// </summary>
		public void Redraw()
		{
			_PSM_p_mapSurface.Invalidate();
		}

		/// <summary>
		/// Resets the map's zoom and position to be in centre and fit the entire canvas
		/// </summary>
		public void Reset()
		{
			//find smaller side of the container
			double coeff;
			if (Settings.Current.DefaultSpaceMapScalePadded)
                coeff = (((double)_PSM_p_mapContainer.Width / SpaceMap._maxX) < ((double)_PSM_p_mapContainer.Height /SpaceMap._maxZ)) 
                    ? ((double)_PSM_p_mapContainer.Width / SpaceMap._maxX) 
                    : ((double)_PSM_p_mapContainer.Height /SpaceMap._maxZ);
			else
                coeff = (((double)_PSM_p_mapContainer.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2)) < ((double)_PSM_p_mapContainer.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2))
                    ? ((double)_PSM_p_mapContainer.Width / (SpaceMap._maxX + SpaceMap._paddingX * 2)) 
                    : ((double)_PSM_p_mapContainer.Height / (SpaceMap._maxZ + SpaceMap._paddingZ * 2)));

			_PSM_p_mapSurface.Width = (int)Math.Round((SpaceMap._maxX + SpaceMap._paddingX * 2.0) * coeff);
            _PSM_p_mapSurface.Height = (int)Math.Round((SpaceMap._maxZ + SpaceMap._paddingZ * 2.0) * coeff);
			_PSM_p_mapSurface.Left = (_PSM_p_mapContainer.Width - _PSM_p_mapSurface.Width) / 2;
			_PSM_p_mapSurface.Top = (_PSM_p_mapContainer.Height - _PSM_p_mapSurface.Height) / 2;
			_beingDragged = false;

			Redraw();
		}

		public void MoveBy(int amountX, int amountY)
		{
			//INIT
			int _Left = _PSM_p_mapSurface.Left;
			int _Top = _PSM_p_mapSurface.Top;
			int _Width = _PSM_p_mapSurface.Width;
			int _Height = _PSM_p_mapSurface.Height;

			//First - move the map's grabpoint to be under the cursor
			_Left = _Left + amountX;
			_Top = _Top + amountY;

			//Second - do not let it go too far off the screen
			bool widthIsDeciding = (SpaceMap._maxX / _PSM_p_mapContainer.Width) > (SpaceMap._maxZ / _PSM_p_mapContainer.Height);
			if (widthIsDeciding)
			{
				//Calculate how far can the top left point go into positive values
				//If width is deciding, then only top can go into positive
				if (_Left > 0)
					_Left = 0;
				if (_Left + _Width < _PSM_p_mapContainer.Width)
					_Left = _PSM_p_mapContainer.Width - _Width;
				if (_Top > 0)
					_Top = 0;
				if (_Top + _Height < _PSM_p_mapContainer.Height)
					_Top = _PSM_p_mapContainer.Height - _Height;
				if (_Height < _PSM_p_mapContainer.Height)
					_Top = (_PSM_p_mapContainer.Height - _Height) / 2;
			}
			else
			{
				//Calculate how far can the corner point go on screen
				//If height is deciding, then only left border can go on screen positive
				if (_Top > 0)
					_Top = 0;
				if (_Top + _Height < _PSM_p_mapContainer.Height)
					_Top = _PSM_p_mapContainer.Height - _Height;
				if (_Left > 0)
					_Left = 0;
				if (_Left + _Width < _PSM_p_mapContainer.Width)
					_Left = _PSM_p_mapContainer.Width - _Width;
				if (_Width < _PSM_p_mapContainer.Width)
					_Left = (_PSM_p_mapContainer.Width - _Width) / 2;
			}

			//FINALLY
			_PSM_p_mapSurface.Left = _Left;
			_PSM_p_mapSurface.Top = _Top;
			//Width = _Width;    //<-- cannot change here
			//Height = _Height;  //<-- cannot change here

			//Require repaint
			//_Redraw();  //<-- shouldnt require repaint here???
		}

		public void ScaleBy(int percent, double relative_x = 0.5, double relative_y = 0.5)
		{
			//INIT
			int _Left = _PSM_p_mapSurface.Left;
			int _Top = _PSM_p_mapSurface.Top;
			int _Width = _PSM_p_mapSurface.Width;
			int _Height = _PSM_p_mapSurface.Height;

			//Width is the deciding factor if its the smaller dimension, otherwise height will decide (as smaller dimension)
			bool widthIsDeciding = (SpaceMap._maxX / _PSM_p_mapContainer.Width) > (SpaceMap._maxZ / _PSM_p_mapContainer.Height);

			if (widthIsDeciding)
			{
				//Calculate amount of pixels to add/substract
				int zoomChangeX = percent * _Width / 100;
				int zoomChangeZ;
				int curMinSizeX;
				int curMaxSizeX;

				//Calculate boundaries for final dimension value
				curMinSizeX = (Settings.Current.MinimalSpaceMapSize < _PSM_p_mapContainer.Width) ? (_PSM_p_mapContainer.Width) : Settings.Current.MinimalSpaceMapSize;
				curMaxSizeX = (Settings.Current.MaximalSpaceMapSize < _PSM_p_mapContainer.Width) ? (_PSM_p_mapContainer.Width) : Settings.Current.MaximalSpaceMapSize;

				//Adjust amount to add/substract if its taking us out of boundaries
				if (_Width + zoomChangeX > curMaxSizeX)
					zoomChangeX -= _Width + zoomChangeX - curMaxSizeX;
				if (_Width + zoomChangeX < curMinSizeX)
					zoomChangeX -= _Width + zoomChangeX - curMinSizeX;

				//Calculate other dimension's value for purpose of adjusting the topleft corner
				zoomChangeZ = (int)Math.Round((double)zoomChangeX / SpaceMap._maxX * SpaceMap._maxZ);

				//Apply changes
				_Width += zoomChangeX;
				_Height = (int)Math.Round((double)_Width / SpaceMap._maxX * SpaceMap._maxZ); //this is safer
				_Left -= (int)Math.Round(zoomChangeX * relative_x);
				_Top -= (int)Math.Round(zoomChangeZ * relative_y);

				//Calculate how far can the top left point go into positive values
				//If width is deciding, then only top can go into positive
				if (_Left > 0)
					_Left = 0;
				if (_Left + _Width < _PSM_p_mapContainer.Width)
					_Left = _PSM_p_mapContainer.Width - _Width;
				if (_Top > 0)
					_Top = 0;
				if (_Top + _Height < _PSM_p_mapContainer.Height)
					_Top = _PSM_p_mapContainer.Height - _Height;
				if (_Height < _PSM_p_mapContainer.Height)
					_Top = (_PSM_p_mapContainer.Height - _Height) / 2;

			}
			else
			{
				//Calculate amount of pixels to add/substract
				int zoomChangeX;
				int zoomChangeZ = percent * _Width / 100;
				int curMinSizeZ;
				int curMaxSizeZ;

				//Calculate boundaries for final dimension value
				curMinSizeZ = (Settings.Current.MinimalSpaceMapSize < _PSM_p_mapContainer.Height) ? (_PSM_p_mapContainer.Height) : Settings.Current.MinimalSpaceMapSize;
				curMaxSizeZ = (Settings.Current.MaximalSpaceMapSize < _PSM_p_mapContainer.Height) ? (_PSM_p_mapContainer.Height) : Settings.Current.MaximalSpaceMapSize;

				//Adjust amount to add/substract if its taking us out of boundaries
				if (_Height + zoomChangeZ > curMaxSizeZ)
					zoomChangeZ -= _Height + zoomChangeZ - curMaxSizeZ;
				if (_Height + zoomChangeZ < curMinSizeZ)
					zoomChangeZ -= _Height + zoomChangeZ - curMinSizeZ;

				//Calculate other dimension's value for purpose of adjusting the topleft corner
				zoomChangeX = (int)Math.Round((double)zoomChangeZ / SpaceMap._maxZ * SpaceMap._maxX);

				//Apply changes
				_Height += zoomChangeZ;
				_Width = (int)Math.Round((double)_Height / SpaceMap._maxZ * SpaceMap._maxX); //this is safer
				_Left -= (int)Math.Round(zoomChangeX * relative_x);
				_Top -= (int)Math.Round(zoomChangeZ * relative_y);

				//Calculate how far can the top left point go into positive values
				//If height is deciding, then only left can go into positive
				if (_Top > 0)
					_Top = 0;
				if (_Top + _Height < _PSM_p_mapContainer.Height)
					_Top = _PSM_p_mapContainer.Height - _Height;
				if (_Left > 0)
					_Left = 0;
				if (_Left + _Width < _PSM_p_mapContainer.Width)
					_Left = _PSM_p_mapContainer.Width - _Width;
				if (_Width < _PSM_p_mapContainer.Width)
					_Left = (_PSM_p_mapContainer.Width - _Width) / 2;
			}

			//FINALLY
			if (_PSM_p_mapSurface.Left != _Left
				|| _PSM_p_mapSurface.Top != _Top
				|| _PSM_p_mapSurface.Width != _Width
				|| _PSM_p_mapSurface.Height != _Height)
			{
                try
                {
                    _PSM_p_mapSurface.SuspendLayout();
                
                    _PSM_p_mapSurface.Left = _Left;
                    _PSM_p_mapSurface.Top = _Top;
                    _PSM_p_mapSurface.Width = _Width;
                    _PSM_p_mapSurface.Height = _Height;

                    //Repaint immediately so we don't see a "blink"
                    DrawSpaceMap(_PSM_p_mapSurface.CreateGraphics());
                }
                finally
                {
                    _PSM_p_mapSurface.ResumeLayout();
                }
                
			}
		}

		public void SetFocus() { _PSM_tb_scapeGoat.Focus(); }

		public bool GetFocused() { return _PSM_tb_scapeGoat.Focused; }

		public void ExecuteCommand_private_Add_Object_To_Map(string type, bool additive)
		{
			object newObject = SpaceMap.AddObject(type, PointXToSpaceMap(_curPointX), 0,
													PointZToSpaceMap(_curPointY), 0, false);
			if (newObject.GetType() != typeof(NamelessMapObject))
			{
				NamedMapObject mo = (NamedMapObject)newObject;
				mo.Copy(true, SpaceMap.SelectionNamed);
				if (!additive)
					SpaceMap.Select_None();
				mo.Selected = true;
				RegisterChange("Added "+mo.TypeToString);
			}
			else
			{
				NamelessMapObject nmo = (NamelessMapObject)newObject;
				if (SpaceMap.SelectionNameless != null)
					nmo.Copy(true, SpaceMap.SelectionNameless);
				else
					nmo.count = 10;
				SpaceMap.Select_None();
				SpaceMap.SelectionNameless = nmo;
				RegisterChange("Added " + nmo.TypeToString);
			}
		}

		public void ExecuteCommand(KeyCommands command)
		{
			bool additive = Control.ModifierKeys == Keys.Shift;
			bool removative = Control.ModifierKeys == Keys.Control;

            //Commands that are same for all modes
            switch (command)
            {
				case KeyCommands.ZoomMinus:
					if (!_beingSelected && !_beingDragged)
						ScaleBy(-10, (double)_curPointX / _PSM_p_mapSurface.Width, (double)_curPointY / _PSM_p_mapSurface.Height);
					break;
				case KeyCommands.ZoomPlus:
					if (!_beingSelected && !_beingDragged)
						ScaleBy(10, (double)_curPointX / _PSM_p_mapSurface.Width, (double)_curPointY / _PSM_p_mapSurface.Height);
					break;
				case KeyCommands.MoveUp:
					MoveBy(0, -10);
					break;
				case KeyCommands.MoveDown:
					MoveBy(0,  10);
					break;
				case KeyCommands.MoveLeft:
					MoveBy(-10, 0);
					break;
				case KeyCommands.MoveRight:
					MoveBy(10,  0);
					break;
                case KeyCommands.ResetMap:
                    Reset();
                    break;
                case KeyCommands.FinishEditing:
                    if (!ChangesPending)
                    {
                        _form.DialogResult = DialogResult.No;
                        return;
                    }
                    DialogResult result = MessageBox.Show("Accept changes?", "Finished editing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (result != DialogResult.Cancel)
                        _form.DialogResult = result;
                    break;
                case KeyCommands.FinishEditingForceYes:
                    _form.DialogResult = DialogResult.Yes;
                    break;
                case KeyCommands.FinishEditingForceNo:
                    _form.DialogResult = DialogResult.No;
                    break;
                case KeyCommands.FocusMap:
                    SetFocus();
                    break;
            }

            if (Mode == PanelSpaceMapMode.SingleSpecial)
            {
                if (SpaceMap.ExecuteCommand(command, Mode, Control.ModifierKeys, PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), (int)Math.Round(1.0 / GetMapScale())))
                    RegisterChange("Executed command "+command);
                return;
            }

			switch (command)
			{
				case KeyCommands.MoveSelectionUp:
					if (SpaceMap.Selection_MoveBy(0, 0, -100))
						RegisterChange("Moved selection up by 100");
					break;
				case KeyCommands.MoveSelectionDown:
					if (SpaceMap.Selection_MoveBy(0, 0, 100))
						RegisterChange("Moved selection down by 100");
					break;
				case KeyCommands.MoveSelectionLeft:
					if (SpaceMap.Selection_MoveBy(-100, 0, 0))
						RegisterChange("Moved selection left by 100");
					break;
				case KeyCommands.MoveSelectionRight:
					if (SpaceMap.Selection_MoveBy(100, 0, 0))
						RegisterChange("Moved selection right by 100");
					break;
				case KeyCommands.MoveSelectionFastUp:
					if (SpaceMap.Selection_MoveBy(0, 0, -1000))
						RegisterChange("Moved selection up by 1000");
					break;
				case KeyCommands.MoveSelectionFastDown:
					if (SpaceMap.Selection_MoveBy(0, 0, 1000))
						RegisterChange("Moved selection down by 100");
					break;
				case KeyCommands.MoveSelectionFastLeft:
					if (SpaceMap.Selection_MoveBy(-1000, 0, 0))
						RegisterChange("Moved selection left by 100");
					break;
				case KeyCommands.MoveSelectionFastRight:
					if (SpaceMap.Selection_MoveBy(1000, 0, 0))
						RegisterChange("Moved selection right by 100");
					break;
				case KeyCommands.AddStation:
					ExecuteCommand_private_Add_Object_To_Map("station", additive);
					break;
				case KeyCommands.AddPlayer:
					ExecuteCommand_private_Add_Object_To_Map("player", additive);
					break;
				case KeyCommands.AddNeutral:
					ExecuteCommand_private_Add_Object_To_Map("neutral", additive);
					break;
				case KeyCommands.AddMonster:
					ExecuteCommand_private_Add_Object_To_Map("monster", additive);
					break;
				case KeyCommands.AddAnomaly:
					ExecuteCommand_private_Add_Object_To_Map("anomaly", additive);
					break;
				case KeyCommands.AddBlackHole:
					ExecuteCommand_private_Add_Object_To_Map("blackHole", additive);
					break;
				case KeyCommands.AddGenericMesh:
					ExecuteCommand_private_Add_Object_To_Map("genericMesh", additive);
					break;
				case KeyCommands.AddEnemy:
					ExecuteCommand_private_Add_Object_To_Map("enemy", additive);
					break;
				case KeyCommands.AddWhales:
					ExecuteCommand_private_Add_Object_To_Map("whale", additive);
					break;
				case KeyCommands.AddNebulas:
					ExecuteCommand_private_Add_Object_To_Map("nebulas", additive);
					break;
				case KeyCommands.AddAsteroids:
					ExecuteCommand_private_Add_Object_To_Map("asteroids", additive);
					break;
				case KeyCommands.AddMines:
					ExecuteCommand_private_Add_Object_To_Map("mines", additive);
					break;
				case KeyCommands.MoveSelected:
					if (SpaceMap.Selection_MoveTo(
						PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY)))
						RegisterChange("Moved selection to cursor");
					break;
				case KeyCommands.DeleteSelected:
					if (SpaceMap.Delete_Selected()>0)
						RegisterChange("Deleted selection");
					break;
				case KeyCommands.SelectAll:
					if (SpaceMap.Select_All())
						RegisterChange("Selected all named objects");
					break;
				case KeyCommands.SelectNone:
					if (SpaceMap.Select_None())
						RegisterChange("Cleared selection");
					break;
				case KeyCommands.SelectNextNamed:
					if (SpaceMap.Select_NextNamed())
						RegisterChange("Selected next named object");
					break;
				case KeyCommands.SelectPreviousNamed:
					if (SpaceMap.Select_PreviousNamed())
						RegisterChange("Selected previous named object");
					break;
				case KeyCommands.SelectPreviousNameless:
					if (SpaceMap.Select_PreviousNameless())
						RegisterChange("Selected previous nameless object");
					break;
				case KeyCommands.SelectNextNameless:
					if (SpaceMap.Select_NextNameless())
						RegisterChange("Selected next nameless object");
					break;
				case KeyCommands.SetStartAngleNegative:
					if (SpaceMap.Selection_SetStartEndAngleTo(
						PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), true, true))
						RegisterChange("Changed start angle of nameless object");
					break;
				case KeyCommands.SetStartAnglePositive:
					if (SpaceMap.Selection_SetStartEndAngleTo(
						PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), true, false))
						RegisterChange("Changed start angle of nameless object");
					break;
				case KeyCommands.SetStartAngleNull:
					if (SpaceMap.Selection_SetStartEndAngleTo(0, 0, true, null))
						RegisterChange("Set start angle of nameless object to null");
					break;
				case KeyCommands.SetEndAngleNegative:
					if (SpaceMap.Selection_SetStartEndAngleTo(
						PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), false, true))
						RegisterChange("Changed end angle of nameless object");
					break;
				case KeyCommands.SetEndAnglePositive:
					if (SpaceMap.Selection_SetStartEndAngleTo(
						PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), false, false))
						RegisterChange("Changed end angle of nameless object");
					break;
				case KeyCommands.SetEndAngleNull:
					if (SpaceMap.Selection_SetStartEndAngleTo(0, 0, false, null))
						RegisterChange("Set end angle of nameless object to null");
					break;
			}
		}

        /// <summary> When form receives a key down event - process global keypress here </summary>
		private void _E_form_KeyDown(object sender, KeyEventArgs e)
		{
			KeyCommands command = Settings.Current.GetKeyCommand(e.KeyData);

			if (command == KeyCommands.KeyCommands_Nothing)
			{
				//Nothing yet
			}
			else
			{
				if (!Settings.Current.KeyCommandRequiresFocus(command))
				{
					e.SuppressKeyPress = true;
					ExecuteCommand(command);
				}
			}
		}

        /// <summary> When space map receives a key down event - process local keypress here </summary>
		private void _E_PSM_tb_ScapeGoat_KeyDown(object sender, KeyEventArgs e)
		{
			KeyCommands command = Settings.Current.GetKeyCommand(e.KeyData);
            
            e.SuppressKeyPress = true;
			if (command == KeyCommands.KeyCommands_Nothing)
			{
				if (e.KeyData == (Keys.Control | Keys.Z))
				{
					e.SuppressKeyPress = true;
					Undo();
				}
				if (e.KeyData == (Keys.Control | Keys.Y))
				{
					e.SuppressKeyPress = true;
					Redo();
				}
				if (e.KeyData == (Keys.Control | Keys.X))
				{
					e.SuppressKeyPress = true;
					Cut();
				}
				if (e.KeyData == (Keys.Control | Keys.C))
				{
					e.SuppressKeyPress = true;
					Copy();
				}
				if (e.KeyData == (Keys.Control | Keys.V))
				{
					e.SuppressKeyPress = true;
					Paste();
				}
			}
			else
			{
				if (Settings.Current.KeyCommandRequiresFocus(command))
					ExecuteCommand(command);
			}
		}

		private void _E_PSM_tb_ScapeGoat_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!_beingSelected && !_beingDragged)
			{
				if (Control.ModifierKeys == Keys.Shift)
                    ScaleBy(10 * Math.Sign(e.Delta), (double)_curPointX / _PSM_p_mapSurface.Width, (double)_curPointY / _PSM_p_mapSurface.Height);
                
			}

			if (Mode == PanelSpaceMapMode.Normal && !_beingSelected && !_beingDragged)
            {
                if (Control.ModifierKeys == Keys.None)
                    if (SpaceMap.Selection_MoveBy(0, 10 * Math.Sign(e.Delta), 0))
                        RegisterChange("Moved selection by 10 on Y axis");
                if (Control.ModifierKeys == Keys.Control)
                    if (SpaceMap.Selection_MoveBy(0, 100 * Math.Sign(e.Delta), 0))
						RegisterChange("Moved selection by 100 on Y axis");
                if (Control.ModifierKeys == Keys.Alt)
                    if (SpaceMap.Selection_ChangeCountBy(1 * Math.Sign(e.Delta)))
						RegisterChange("Changed nameless object count by 1");
                if (Control.ModifierKeys == (Keys.Alt | Keys.Control))
                    if (SpaceMap.Selection_ChangeCountBy(10 * Math.Sign(e.Delta)))
						RegisterChange("Changed nameless object count by 10");
            }

			if (Mode != PanelSpaceMapMode.Normal)
			{
				if (SpaceMap.ExecuteWheelAction(e.Delta, Mode, ModifierKeys, PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), (int)Math.Round(1.0 / GetMapScale())))
					RegisterChange("Executed mouse wheel action");
			}
			
			//_ScaleBy(e.Delta); //bugs, gives 120!
		}

		private void _E_PSM_tb_scapeGoat_ChangedFocus(object sender, EventArgs e)
		{
			if (GetFocused())
				this.BackColor = Settings.Current.GetColor(MapColors.MapBorderActive);
			else
				this.BackColor = Settings.Current.GetColor(MapColors.MapBorderInactive);
		}

		private void _E_PSM_p_mapSurface_Paint(object sender, PaintEventArgs e)
		{
            DrawSpaceMap(e.Graphics);
		}

		private void _E_PSM_p_mapSurface_MouseDown(object sender, MouseEventArgs e)
		{
			_mouseMovedSinceMouseDown = false;
			if (e.Button == MouseButtons.Right)
			{
				//Enable drag mode and remember which point of the panel we "grabbed"
				_beingDragged = true;
				_grabPointX = e.X;
				_grabPointY = e.Y;
			}

			if (e.Button == MouseButtons.Left && Mode==PanelSpaceMapMode.Normal)
			{
				//Enable select mode and remember which point of the panel we started to select from
				_beingSelected = true;
				_selectPointX = e.X;
				_selectPointY = e.Y;
			}
		}

		private void _E_PSM_p_mapSurface_MouseMove(object sender, MouseEventArgs e)
		{
			_mouseMovedSinceMouseDown = true;
			_curPointX = e.X;
			_curPointY = e.Y;
            
            UpdateCoordinatesText();

			if (_beingSelected)
			{
				Redraw();
				if (GetFocused())
					_PSM_p_mapSurface.Cursor = Cursors.Cross;
				else
					_PSM_p_mapSurface.Cursor = Cursors.Default;
			}

			//If dragging try to move the grabbed point to the cursor
			if (_beingDragged)
				MoveBy(e.X - _grabPointX, e.Y - _grabPointY);
		}

		private void _E_PSM_p_mapSurface_AfterMouseUp()
		{
			if (_beingSelected)
			{
				Redraw();
				_PSM_p_mapSurface.Cursor = Cursors.Default;
			}
			_beingDragged = false;
			_beingSelected = false;
		}

		private void _E_PSM_p_mapSurface_MouseUp(object sender, MouseEventArgs e)
		{
			//WHAT TRIGGERS EVEN WITHOUT FOCUS:

			//show context menu
            if (Mode == PanelSpaceMapMode.Normal && (e.Button == MouseButtons.Right) && (!_mouseMovedSinceMouseDown))
                _PSM_cms_Main.Show(Cursor.Position);

			//return focus if we lost it
			if (!GetFocused())
			{
				_E_PSM_p_mapSurface_AfterMouseUp();
				SetFocus();
				return;
			}

			//WHAT TRIGGERS ONLY WHEN HAS FOCUS

			//object selection mechanism
            if (Mode == PanelSpaceMapMode.Normal && (e.Button == MouseButtons.Left) && (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.None))
			{
				bool additive = Control.ModifierKeys == Keys.Shift;
				bool removative = Control.ModifierKeys == Keys.Control;
				if (SpaceMap.Select_AtPoint(PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), (int)Math.Round(1.0 / GetMapScale()), Settings.Current.YScale, additive, removative))
					RegisterChange("Changed selection by single-selection");
			}

			//object rotation mechanism
            if (Mode == PanelSpaceMapMode.Normal && (e.Button == MouseButtons.Left) && ((Control.ModifierKeys == Keys.Alt)) || (Control.ModifierKeys == (Keys.Alt | Keys.Shift)) || (Control.ModifierKeys == (Keys.Alt | Keys.Control)) || (Control.ModifierKeys == (Keys.Alt | Keys.Control | Keys.Shift)))
            {
                bool shift = Control.ModifierKeys == (Keys.Shift | Keys.Alt) || Control.ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt);
                bool control = Control.ModifierKeys == (Keys.Control | Keys.Alt) || Control.ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt);
				if (SpaceMap.Selection_Turn_or_SetRadius(PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), shift, control))
					RegisterChange(SpaceMap.SelectionNameless == null  ? "Rotated selected object(s)" : "Changed radius/end point");
			}

			//mass selection mechanism
            if (Mode == PanelSpaceMapMode.Normal && (_beingSelected && (_selectPointX != _curPointX || _selectPointY != _curPointY))
			&& (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.None))
			{
				bool additive = Control.ModifierKeys == Keys.Shift;
				bool removative = Control.ModifierKeys == Keys.Control;
				if (SpaceMap.Select_InRectangle(
					PointXToSpaceMap(_selectPointX),
					PointZToSpaceMap(_selectPointY),
					PointXToSpaceMap(_curPointX),
					PointZToSpaceMap(_curPointY),
					(int)Math.Round(1.0 / GetMapScale()), Settings.Current.YScale, additive, removative))
					RegisterChange("Changed selection by mass-selection");
			}

            //finish selection mechanism in any case
            if (Mode == PanelSpaceMapMode.Normal && _beingSelected)
			{
				Redraw();
				_PSM_p_mapSurface.Cursor = Cursors.Default;
			}

            //special mode
            if (Mode != PanelSpaceMapMode.Normal)
            {
                if (SpaceMap.ExecuteMouseAction(e.Button, Mode, ModifierKeys, PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY), (int)Math.Round(1.0 / GetMapScale())))
					RegisterChange("Executed mouse action");
            }

			//cancel drag/select in any case
			_E_PSM_p_mapSurface_AfterMouseUp();
		}

		private void _E_PSM_Resize(object sender, EventArgs e)
		{
			ScaleBy(0); //Check if spaceMap went out of bounds
			_PSM_tb_scapeGoat.Left = _PSM_p_mapContainer.Width / 2 - _PSM_tb_scapeGoat.Width / 2;
			_PSM_tb_scapeGoat.Top = _PSM_p_mapContainer.Height / 2 - _PSM_tb_scapeGoat.Height / 2;
		}

		private void _E_PSM_p_mapContainer_MouseUp(object sender, MouseEventArgs e)
		{
			if (!GetFocused())
				SetFocus();
		}

		private void _E_namedObjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (___STATIC_E_FSM_SuppressSelectionEvents)
				return;
			Selection_FromControl(true);
			RegisterChange("Selection changed via list box",true);
		}

		private void _E_namedObjectsLB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.A))
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.Select_All())
					RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == (Keys.Control | Keys.Up))
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.Selection_Count > 0)
					if (SpaceMap.Selection_ToFront())
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == (Keys.Control | Keys.Down))
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.Selection_Count > 0)
					if (SpaceMap.Selection_ToBack())
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == Keys.Delete)
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.Selection_Count > 0) 
					if (SpaceMap.Delete_Selected()>0)
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == Keys.Home || e.KeyData == Keys.End)
			if (_namelessObjectsPage!=null)
			{
				e.SuppressKeyPress = true;
				_objectsTabControl.SelectedTab = _namelessObjectsPage;
				_namelessObjectsLB.Focus();
				if (_namelessObjectsLB.Items.Count > 0)
				{
					if (e.KeyData == Keys.Home)
						_namelessObjectsLB.SelectedIndex = 0;

					if (e.KeyData == Keys.End)
						_namelessObjectsLB.SelectedIndex = _namelessObjectsLB.Items.Count - 1;
				}
			}
		}

		private void _E_namelessObjectsLB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == (Keys.Control | Keys.Up))
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.SelectionNameless !=null)
					if (SpaceMap.Selection_ToFront())
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == (Keys.Control | Keys.Down))
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.SelectionNameless != null)
					if (SpaceMap.Selection_ToBack())
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == Keys.Delete)
			{
				e.SuppressKeyPress = true;
				if (SpaceMap.SelectionNameless != null)
					if (SpaceMap.Delete_Selected()>0)
						RegisterChange("Selection changed via list box");
			}
			if (e.KeyData == Keys.PageUp || e.KeyData == Keys.PageDown)
			if (_namedObjectsPage != null)
			{
				e.SuppressKeyPress = true;
				_objectsTabControl.SelectedTab = _namedObjectsPage;
				_namedObjectsLB.Focus();
				if (_namedObjectsLB.Items.Count > 0)
				{
					if (e.KeyData == Keys.PageUp)
					{
						_namedObjectsLB.SelectedItem = null;
						_namedObjectsLB.SelectedIndex = 0;
					}
					if (e.KeyData == Keys.PageDown)
					{
						_namedObjectsLB.SelectedItem = null;
						_namedObjectsLB.SelectedIndex = _namedObjectsLB.Items.Count - 1;
					}
				}
			}
		}
		
		private void _E_propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			RegisterChange("Changed selection's properties");
		}

		private void _E_namelessObjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (___STATIC_E_FSM_SuppressSelectionEvents)
				return;
			Selection_FromControl(false);
			RegisterChange("Selection changed via list box", true);
		}

		private void _E_PSM_cms_Main_Selection_All_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.SelectAll);
		}

		private void _E_PSM_cms_Main_Selection_None_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.SelectNone);
		}

		private void _E_PSM_cms_Main_Selection_ToFront_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_ToFront())
				RegisterChange("Moved selection to front");
		}

		private void _E_PSM_cms_Main_Selection_ToBack_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_ToBack())
				RegisterChange("Noved selection to back");
		}

		private void _E_PSM_cms_Main_Selection_Delete_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.DeleteSelected);
			SetFocus();
		}

		private void _E_PSM_cms_Main_New_Anomaly_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddAnomaly);
		}

		private void _E_PSM_cms_Main_New_BlackHole_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddBlackHole);
		}

		private void _E_PSM_cms_Main_New_Enemy_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddEnemy);
		}

		private void _E_PSM_cms_Main_New_GenericMesh_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddGenericMesh);
		}

		private void _E_PSM_cms_Main_New_Monster_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddMonster);
		}

		private void _E_PSM_cms_Main_New_Neutral_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddNeutral);
		}

		private void _E_PSM_cms_Main_New_Player_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddPlayer);
		}

		private void _E_PSM_cms_Main_New_Whale_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddWhales);
		}

		private void _E_PSM_cms_Main_New_Station_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddStation);
		}
		
		private void _E_PSM_cms_Main_Selection_MoveToCursor_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.MoveSelected);
		}

		private void _E_PSM_cms_Main_Selection_TurnToCursor_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_Turn_or_SetRadius(PointXToSpaceMap(_curPointX), PointZToSpaceMap(_curPointY)))
				RegisterChange(SpaceMap.SelectionNameless == null ? "Turned selection to cursor" : "Changed radius/end point");
		}

		private void _E_PSM_cms_Main_Opening(object sender, CancelEventArgs e)
		{
			bool somethin = SpaceMap.Selection_Count > 0 || SpaceMap.SelectionNameless != null;

			_PSM_cms_Main_Selection_Delete.Enabled = somethin;
			_PSM_cms_Main_Selection_MoveToCursor.Enabled = somethin;
			_PSM_cms_Main_Selection_ToBack.Enabled = somethin;
			_PSM_cms_Main_Selection_ToFront.Enabled = somethin;
			_PSM_cms_Main_Selection_TurnToCursor.Enabled = somethin;

		}

		private void _E_PSM_cms_Main_New_Nebulas_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddNebulas);
		}

		private void _E_PSM_cms_Main_New_Asteroids_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddAsteroids);
		}

		private void _E_PSM_cms_Main_New_Mines_Click(object sender, EventArgs e)
		{
			ExecuteCommand(KeyCommands.AddMines);
		}

		public _PanelSpaceMap()
		{
			InitializeComponent();

			//Assigned objects
			_form					= null;
			_propertyEditorPG		= null;
			_propertyContainer		= null;
			_possibleRacesLB		= null;
			_possibleVesselsLB		= null;
			_namelessObjectsLB		= null;
			_namedObjectsLB			= null;
			_namelessObjectsPage	= null; 
			_namedObjectsPage		= null;
			_objectsTabControl		= null;

			this.AssignForm();
			this.AssignPropertyGrid();
			this.AssignNamedObjectsListBox();
			this.AssignNamelessObjectsListBox();
			this.AssignStatusToolStrip();

			//Init objects
			_PSM_p_mapContainer.Dock = DockStyle.Fill;

			//init map
			SpaceMap = new SpaceMap();

			//Init dragging vars
			_beingDragged = false;
			_beingSelected = false;
			_grabPointX = 0;
			_grabPointY = 0;
			_selectPointX = 0;
			_selectPointY = 0;
			_curPointX = 0;
			_curPointY = 0;

			//Init event fails
			___STATIC_E_FSM_SuppressSelectionEvents = false;

			//Init events
			_PSM_tb_scapeGoat.GotFocus += new EventHandler(_E_PSM_tb_scapeGoat_ChangedFocus);
			_PSM_tb_scapeGoat.LostFocus += new EventHandler(_E_PSM_tb_scapeGoat_ChangedFocus);
			_PSM_tb_scapeGoat.MouseWheel += new MouseEventHandler(_E_PSM_tb_ScapeGoat_MouseWheel);

			//init other vars
			readOnly = false;
			_mouseMovedSinceMouseDown = false;

            ChangesPending = false;
		}

		~_PanelSpaceMap()
		{
			//AssignPropertyGrid();
			//AssignNamedObjectsListBox();
			//AssignNamelessObjectsListBox();
			//AssignStatusToolStrip();
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this._PSM_p_mapContainer = new System.Windows.Forms.Panel();
            this._PSM_p_mapSurface = new ArtemisMissionEditor.DoubleBufferedPanel();
            this._PSM_tb_scapeGoat = new System.Windows.Forms.TextBox();
            this._PSM_cms_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._PSM_cms_Main_New = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Anomaly = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_BlackHole = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Enemy = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_GenericMesh = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Monster = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Neutral = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Player = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Station = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Whale = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_Main_New_Asteroids = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Mines = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_New_Nebulas = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Select = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_All = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_None = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_MoveToCursor = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_TurnToCursor = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_Main_Selection_ToFront = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_ToBack = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_Main_Selection_s_2 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_Main_Selection_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._PSM_cms_NamedObjectList_Select = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_SelectTopObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_SelectBottomObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_Select_s_3 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamedObjectList_SelectPreviousObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_SelectNextObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_Select_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamedObjectList_AddPreviousObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_AddNextObject = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_Select_s_2 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamedObjectList_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_SelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamedObjectList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_s_2 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamedObjectList_MoveToFront = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamedObjectList_MoveToBack = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._PSM_cms_NamelessObjectList_Select = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_SelectTop = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_SelectBottom = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_Select_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamelessObjectList_SelectPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_SelectNext = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_Select_s_2 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamelessObjectList_AddNext = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_AddPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_Select_s_3 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamelessObjectList_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_SelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_s_1 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamelessObjectList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_s_2 = new System.Windows.Forms.ToolStripSeparator();
            this._PSM_cms_NamelessObjectList_MoveToFront = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_cms_NamelessObjectList_MoveToBack = new System.Windows.Forms.ToolStripMenuItem();
            this._PSM_p_mapContainer.SuspendLayout();
            this._PSM_cms_Main.SuspendLayout();
            this._PSM_cms_NamedObjectList.SuspendLayout();
            this._PSM_cms_NamelessObjectList.SuspendLayout();
            this.SuspendLayout();
            // 
            // _PSM_p_mapContainer
            // 
            this._PSM_p_mapContainer.BackColor = System.Drawing.Color.Black;
            this._PSM_p_mapContainer.Controls.Add(this._PSM_p_mapSurface);
            this._PSM_p_mapContainer.Location = new System.Drawing.Point(10, 10);
            this._PSM_p_mapContainer.Name = "_PSM_p_mapContainer";
            this._PSM_p_mapContainer.Size = new System.Drawing.Size(595, 438);
            this._PSM_p_mapContainer.TabIndex = 0;
            this._PSM_p_mapContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this._E_PSM_p_mapContainer_MouseUp);
            // 
            // _PSM_p_mapSurface
            // 
            this._PSM_p_mapSurface.BackColor = System.Drawing.Color.Transparent;
            this._PSM_p_mapSurface.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._PSM_p_mapSurface.Location = new System.Drawing.Point(14, 15);
            this._PSM_p_mapSurface.Name = "_PSM_p_mapSurface";
            this._PSM_p_mapSurface.Size = new System.Drawing.Size(567, 388);
            this._PSM_p_mapSurface.TabIndex = 0;
            this._PSM_p_mapSurface.Paint += new System.Windows.Forms.PaintEventHandler(this._E_PSM_p_mapSurface_Paint);
            this._PSM_p_mapSurface.MouseDown += new System.Windows.Forms.MouseEventHandler(this._E_PSM_p_mapSurface_MouseDown);
            this._PSM_p_mapSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this._E_PSM_p_mapSurface_MouseMove);
            this._PSM_p_mapSurface.MouseUp += new System.Windows.Forms.MouseEventHandler(this._E_PSM_p_mapSurface_MouseUp);
            // 
            // _PSM_tb_scapeGoat
            // 
            this._PSM_tb_scapeGoat.BackColor = System.Drawing.Color.Lime;
            this._PSM_tb_scapeGoat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._PSM_tb_scapeGoat.ForeColor = System.Drawing.Color.DarkRed;
            this._PSM_tb_scapeGoat.Location = new System.Drawing.Point(297, 437);
            this._PSM_tb_scapeGoat.Name = "_PSM_tb_scapeGoat";
            this._PSM_tb_scapeGoat.Size = new System.Drawing.Size(22, 20);
            this._PSM_tb_scapeGoat.TabIndex = 0;
            this._PSM_tb_scapeGoat.Text = ">:-)";
            this._PSM_tb_scapeGoat.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_PSM_tb_ScapeGoat_KeyDown);
            // 
            // _PSM_cms_Main
            // 
            this._PSM_cms_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_Main_New,
            this._PSM_cms_Main_Select,
            this._PSM_cms_Main_Selection});
            this._PSM_cms_Main.Name = "_PSM_cms_MainSelection";
            this._PSM_cms_Main.Size = new System.Drawing.Size(123, 70);
            // 
            // _PSM_cms_Main_New
            // 
            this._PSM_cms_Main_New.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_Main_New_Anomaly,
            this._PSM_cms_Main_New_BlackHole,
            this._PSM_cms_Main_New_Enemy,
            this._PSM_cms_Main_New_GenericMesh,
            this._PSM_cms_Main_New_Monster,
            this._PSM_cms_Main_New_Neutral,
            this._PSM_cms_Main_New_Player,
            this._PSM_cms_Main_New_Station,
            this._PSM_cms_Main_New_Whale,
            this._PSM_cms_Main_New_s_1,
            this._PSM_cms_Main_New_Asteroids,
            this._PSM_cms_Main_New_Mines,
            this._PSM_cms_Main_New_Nebulas});
            this._PSM_cms_Main_New.Name = "_PSM_cms_Main_New";
            this._PSM_cms_Main_New.Size = new System.Drawing.Size(122, 22);
            this._PSM_cms_Main_New.Text = "&New";
            // 
            // _PSM_cms_Main_New_Anomaly
            // 
            this._PSM_cms_Main_New_Anomaly.Name = "_PSM_cms_Main_New_Anomaly";
            this._PSM_cms_Main_New_Anomaly.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Anomaly.Text = "(&1) Anomaly ";
            this._PSM_cms_Main_New_Anomaly.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Anomaly_Click);
            // 
            // _PSM_cms_Main_New_BlackHole
            // 
            this._PSM_cms_Main_New_BlackHole.Name = "_PSM_cms_Main_New_BlackHole";
            this._PSM_cms_Main_New_BlackHole.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_BlackHole.Text = "(&2) Black hole";
            this._PSM_cms_Main_New_BlackHole.Click += new System.EventHandler(this._E_PSM_cms_Main_New_BlackHole_Click);
            // 
            // _PSM_cms_Main_New_Enemy
            // 
            this._PSM_cms_Main_New_Enemy.Name = "_PSM_cms_Main_New_Enemy";
            this._PSM_cms_Main_New_Enemy.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Enemy.Text = "(&3) Enemy";
            this._PSM_cms_Main_New_Enemy.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Enemy_Click);
            // 
            // _PSM_cms_Main_New_GenericMesh
            // 
            this._PSM_cms_Main_New_GenericMesh.Name = "_PSM_cms_Main_New_GenericMesh";
            this._PSM_cms_Main_New_GenericMesh.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_GenericMesh.Text = "(&4) Generic mesh";
            this._PSM_cms_Main_New_GenericMesh.Click += new System.EventHandler(this._E_PSM_cms_Main_New_GenericMesh_Click);
            // 
            // _PSM_cms_Main_New_Monster
            // 
            this._PSM_cms_Main_New_Monster.Name = "_PSM_cms_Main_New_Monster";
            this._PSM_cms_Main_New_Monster.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Monster.Text = "(&5) Monster";
            this._PSM_cms_Main_New_Monster.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Monster_Click);
            // 
            // _PSM_cms_Main_New_Neutral
            // 
            this._PSM_cms_Main_New_Neutral.Name = "_PSM_cms_Main_New_Neutral";
            this._PSM_cms_Main_New_Neutral.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Neutral.Text = "(&6) Neutral";
            this._PSM_cms_Main_New_Neutral.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Neutral_Click);
            // 
            // _PSM_cms_Main_New_Player
            // 
            this._PSM_cms_Main_New_Player.Name = "_PSM_cms_Main_New_Player";
            this._PSM_cms_Main_New_Player.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Player.Text = "(&7) Player";
            this._PSM_cms_Main_New_Player.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Player_Click);
            // 
            // _PSM_cms_Main_New_Station
            // 
            this._PSM_cms_Main_New_Station.Name = "_PSM_cms_Main_New_Station";
            this._PSM_cms_Main_New_Station.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Station.Text = "(&8) Station";
            this._PSM_cms_Main_New_Station.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Station_Click);
            // 
            // _PSM_cms_Main_New_Whale
            // 
            this._PSM_cms_Main_New_Whale.Name = "_PSM_cms_Main_New_Whale";
            this._PSM_cms_Main_New_Whale.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Whale.Text = "(&9) Whale";
            this._PSM_cms_Main_New_Whale.Click += new EventHandler(_E_PSM_cms_Main_New_Whale_Click);
            // 
            // _PSM_cms_Main_New_s_1
            // 
            this._PSM_cms_Main_New_s_1.Name = "_PSM_cms_Main_New_s_1";
            this._PSM_cms_Main_New_s_1.Size = new System.Drawing.Size(160, 6);
            // 
            // _PSM_cms_Main_New_Asteroids
            // 
            this._PSM_cms_Main_New_Asteroids.Name = "_PSM_cms_Main_New_Asteroids";
            this._PSM_cms_Main_New_Asteroids.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Asteroids.Text = "(&I) Asteroids";
            this._PSM_cms_Main_New_Asteroids.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Asteroids_Click);
            // 
            // _PSM_cms_Main_New_Mines
            // 
            this._PSM_cms_Main_New_Mines.Name = "_PSM_cms_Main_New_Mines";
            this._PSM_cms_Main_New_Mines.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Mines.Text = "(&O) Mines";
            this._PSM_cms_Main_New_Mines.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Mines_Click);
            // 
            // _PSM_cms_Main_New_Nebulas
            // 
            this._PSM_cms_Main_New_Nebulas.Name = "_PSM_cms_Main_New_Nebulas";
            this._PSM_cms_Main_New_Nebulas.Size = new System.Drawing.Size(163, 22);
            this._PSM_cms_Main_New_Nebulas.Text = "(&P) Nebulas";
            this._PSM_cms_Main_New_Nebulas.Click += new System.EventHandler(this._E_PSM_cms_Main_New_Nebulas_Click);
            // 
            // _PSM_cms_Main_Select
            // 
            this._PSM_cms_Main_Select.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_Main_Selection_All,
            this._PSM_cms_Main_Selection_None});
            this._PSM_cms_Main_Select.Name = "_PSM_cms_Main_Select";
            this._PSM_cms_Main_Select.Size = new System.Drawing.Size(122, 22);
            this._PSM_cms_Main_Select.Text = "Select";
            // 
            // _PSM_cms_Main_Selection_All
            // 
            this._PSM_cms_Main_Selection_All.Name = "_PSM_cms_Main_Selection_All";
            this._PSM_cms_Main_Selection_All.ShortcutKeyDisplayString = "Ctrl+A";
            this._PSM_cms_Main_Selection_All.Size = new System.Drawing.Size(164, 22);
            this._PSM_cms_Main_Selection_All.Text = "Select &All";
            this._PSM_cms_Main_Selection_All.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_All_Click);
            // 
            // _PSM_cms_Main_Selection_None
            // 
            this._PSM_cms_Main_Selection_None.Name = "_PSM_cms_Main_Selection_None";
            this._PSM_cms_Main_Selection_None.Size = new System.Drawing.Size(164, 22);
            this._PSM_cms_Main_Selection_None.Text = "Select &None";
            this._PSM_cms_Main_Selection_None.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_None_Click);
            // 
            // _PSM_cms_Main_Selection
            // 
            this._PSM_cms_Main_Selection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_Main_Selection_MoveToCursor,
            this._PSM_cms_Main_Selection_TurnToCursor,
            this._PSM_cms_Main_Selection_s_1,
            this._PSM_cms_Main_Selection_ToFront,
            this._PSM_cms_Main_Selection_ToBack,
            this._PSM_cms_Main_Selection_s_2,
            this._PSM_cms_Main_Selection_Delete});
            this._PSM_cms_Main_Selection.Name = "_PSM_cms_Main_Selection";
            this._PSM_cms_Main_Selection.Size = new System.Drawing.Size(122, 22);
            this._PSM_cms_Main_Selection.Text = "&Selection";
            // 
            // _PSM_cms_Main_Selection_MoveToCursor
            // 
            this._PSM_cms_Main_Selection_MoveToCursor.Name = "_PSM_cms_Main_Selection_MoveToCursor";
            this._PSM_cms_Main_Selection_MoveToCursor.ShortcutKeyDisplayString = "Space";
            this._PSM_cms_Main_Selection_MoveToCursor.Size = new System.Drawing.Size(205, 22);
            this._PSM_cms_Main_Selection_MoveToCursor.Text = "Move to Cursor";
            this._PSM_cms_Main_Selection_MoveToCursor.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_MoveToCursor_Click);
            // 
            // _PSM_cms_Main_Selection_TurnToCursor
            // 
            this._PSM_cms_Main_Selection_TurnToCursor.Name = "_PSM_cms_Main_Selection_TurnToCursor";
            this._PSM_cms_Main_Selection_TurnToCursor.ShortcutKeyDisplayString = "Alt+LMB";
            this._PSM_cms_Main_Selection_TurnToCursor.Size = new System.Drawing.Size(205, 22);
            this._PSM_cms_Main_Selection_TurnToCursor.Text = "Turn to Cursor";
            this._PSM_cms_Main_Selection_TurnToCursor.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_TurnToCursor_Click);
            // 
            // _PSM_cms_Main_Selection_s_1
            // 
            this._PSM_cms_Main_Selection_s_1.Name = "_PSM_cms_Main_Selection_s_1";
            this._PSM_cms_Main_Selection_s_1.Size = new System.Drawing.Size(202, 6);
            // 
            // _PSM_cms_Main_Selection_ToFront
            // 
            this._PSM_cms_Main_Selection_ToFront.Name = "_PSM_cms_Main_Selection_ToFront";
            this._PSM_cms_Main_Selection_ToFront.Size = new System.Drawing.Size(205, 22);
            this._PSM_cms_Main_Selection_ToFront.Text = "Move to &Front";
            this._PSM_cms_Main_Selection_ToFront.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_ToFront_Click);
            // 
            // _PSM_cms_Main_Selection_ToBack
            // 
            this._PSM_cms_Main_Selection_ToBack.Name = "_PSM_cms_Main_Selection_ToBack";
            this._PSM_cms_Main_Selection_ToBack.Size = new System.Drawing.Size(205, 22);
            this._PSM_cms_Main_Selection_ToBack.Text = "Move to &Back";
            this._PSM_cms_Main_Selection_ToBack.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_ToBack_Click);
            // 
            // _PSM_cms_Main_Selection_s_2
            // 
            this._PSM_cms_Main_Selection_s_2.Name = "_PSM_cms_Main_Selection_s_2";
            this._PSM_cms_Main_Selection_s_2.Size = new System.Drawing.Size(202, 6);
            // 
            // _PSM_cms_Main_Selection_Delete
            // 
            this._PSM_cms_Main_Selection_Delete.Name = "_PSM_cms_Main_Selection_Delete";
            this._PSM_cms_Main_Selection_Delete.ShortcutKeyDisplayString = "Del";
            this._PSM_cms_Main_Selection_Delete.Size = new System.Drawing.Size(205, 22);
            this._PSM_cms_Main_Selection_Delete.Text = "(&0) Delete";
            this._PSM_cms_Main_Selection_Delete.Click += new System.EventHandler(this._E_PSM_cms_Main_Selection_Delete_Click);
            // 
            // _PSM_cms_NamedObjectList
            // 
            this._PSM_cms_NamedObjectList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_NamedObjectList_Select,
            this._PSM_cms_NamedObjectList_s_1,
            this._PSM_cms_NamedObjectList_Delete,
            this._PSM_cms_NamedObjectList_s_2,
            this._PSM_cms_NamedObjectList_MoveToFront,
            this._PSM_cms_NamedObjectList_MoveToBack});
            this._PSM_cms_NamedObjectList.Name = "_PSM_cms_NamedObjectList";
            this._PSM_cms_NamedObjectList.Size = new System.Drawing.Size(212, 104);
            // 
            // _PSM_cms_NamedObjectList_Select
            // 
            this._PSM_cms_NamedObjectList_Select.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_NamedObjectList_SelectTopObject,
            this._PSM_cms_NamedObjectList_SelectBottomObject,
            this._PSM_cms_NamedObjectList_Select_s_3,
            this._PSM_cms_NamedObjectList_SelectPreviousObject,
            this._PSM_cms_NamedObjectList_SelectNextObject,
            this._PSM_cms_NamedObjectList_Select_s_1,
            this._PSM_cms_NamedObjectList_AddPreviousObject,
            this._PSM_cms_NamedObjectList_AddNextObject,
            this._PSM_cms_NamedObjectList_Select_s_2,
            this._PSM_cms_NamedObjectList_SelectAll,
            this._PSM_cms_NamedObjectList_SelectNone});
            this._PSM_cms_NamedObjectList_Select.Name = "_PSM_cms_NamedObjectList_Select";
            this._PSM_cms_NamedObjectList_Select.ShortcutKeyDisplayString = "";
            this._PSM_cms_NamedObjectList_Select.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamedObjectList_Select.Tag = "";
            this._PSM_cms_NamedObjectList_Select.Text = "Select";
            // 
            // _PSM_cms_NamedObjectList_SelectTopObject
            // 
            this._PSM_cms_NamedObjectList_SelectTopObject.Name = "_PSM_cms_NamedObjectList_SelectTopObject";
            this._PSM_cms_NamedObjectList_SelectTopObject.ShortcutKeyDisplayString = "PageUp";
            this._PSM_cms_NamedObjectList_SelectTopObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectTopObject.Text = "Select top object";
            this._PSM_cms_NamedObjectList_SelectTopObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectTopObject_Click);
            // 
            // _PSM_cms_NamedObjectList_SelectBottomObject
            // 
            this._PSM_cms_NamedObjectList_SelectBottomObject.Name = "_PSM_cms_NamedObjectList_SelectBottomObject";
            this._PSM_cms_NamedObjectList_SelectBottomObject.ShortcutKeyDisplayString = "PageDown";
            this._PSM_cms_NamedObjectList_SelectBottomObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectBottomObject.Text = "Select bottom object";
            this._PSM_cms_NamedObjectList_SelectBottomObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectBottomObject_Click);
            // 
            // _PSM_cms_NamedObjectList_Select_s_3
            // 
            this._PSM_cms_NamedObjectList_Select_s_3.Name = "_PSM_cms_NamedObjectList_Select_s_3";
            this._PSM_cms_NamedObjectList_Select_s_3.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamedObjectList_SelectPreviousObject
            // 
            this._PSM_cms_NamedObjectList_SelectPreviousObject.Name = "_PSM_cms_NamedObjectList_SelectPreviousObject";
            this._PSM_cms_NamedObjectList_SelectPreviousObject.ShortcutKeyDisplayString = "Up";
            this._PSM_cms_NamedObjectList_SelectPreviousObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectPreviousObject.Text = "Select previous object";
            this._PSM_cms_NamedObjectList_SelectPreviousObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectPreviousObject_Click);
            // 
            // _PSM_cms_NamedObjectList_SelectNextObject
            // 
            this._PSM_cms_NamedObjectList_SelectNextObject.Name = "_PSM_cms_NamedObjectList_SelectNextObject";
            this._PSM_cms_NamedObjectList_SelectNextObject.ShortcutKeyDisplayString = "Down";
            this._PSM_cms_NamedObjectList_SelectNextObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectNextObject.Text = "Select next object";
            this._PSM_cms_NamedObjectList_SelectNextObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectNextObject_Click);
            // 
            // _PSM_cms_NamedObjectList_Select_s_1
            // 
            this._PSM_cms_NamedObjectList_Select_s_1.Name = "_PSM_cms_NamedObjectList_Select_s_1";
            this._PSM_cms_NamedObjectList_Select_s_1.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamedObjectList_AddPreviousObject
            // 
            this._PSM_cms_NamedObjectList_AddPreviousObject.Name = "_PSM_cms_NamedObjectList_AddPreviousObject";
            this._PSM_cms_NamedObjectList_AddPreviousObject.ShortcutKeyDisplayString = "Shift+Up";
            this._PSM_cms_NamedObjectList_AddPreviousObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_AddPreviousObject.Text = "Expand/shrink selection upwards";
            this._PSM_cms_NamedObjectList_AddPreviousObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_AddPreviousObject_Click);
            // 
            // _PSM_cms_NamedObjectList_AddNextObject
            // 
            this._PSM_cms_NamedObjectList_AddNextObject.Name = "_PSM_cms_NamedObjectList_AddNextObject";
            this._PSM_cms_NamedObjectList_AddNextObject.ShortcutKeyDisplayString = "Shift+Down";
            this._PSM_cms_NamedObjectList_AddNextObject.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_AddNextObject.Text = "Expand/shrink selection downwards";
            this._PSM_cms_NamedObjectList_AddNextObject.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_AddNextObject_Click);
            // 
            // _PSM_cms_NamedObjectList_Select_s_2
            // 
            this._PSM_cms_NamedObjectList_Select_s_2.Name = "_PSM_cms_NamedObjectList_Select_s_2";
            this._PSM_cms_NamedObjectList_Select_s_2.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamedObjectList_SelectAll
            // 
            this._PSM_cms_NamedObjectList_SelectAll.Name = "_PSM_cms_NamedObjectList_SelectAll";
            this._PSM_cms_NamedObjectList_SelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this._PSM_cms_NamedObjectList_SelectAll.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectAll.Text = "Select All";
            this._PSM_cms_NamedObjectList_SelectAll.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectAll_Click);
            // 
            // _PSM_cms_NamedObjectList_SelectNone
            // 
            this._PSM_cms_NamedObjectList_SelectNone.Name = "_PSM_cms_NamedObjectList_SelectNone";
            this._PSM_cms_NamedObjectList_SelectNone.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamedObjectList_SelectNone.Text = "Select None";
            this._PSM_cms_NamedObjectList_SelectNone.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_SelectNone_Click);
            // 
            // _PSM_cms_NamedObjectList_s_1
            // 
            this._PSM_cms_NamedObjectList_s_1.Name = "_PSM_cms_NamedObjectList_s_1";
            this._PSM_cms_NamedObjectList_s_1.Size = new System.Drawing.Size(208, 6);
            // 
            // _PSM_cms_NamedObjectList_Delete
            // 
            this._PSM_cms_NamedObjectList_Delete.Name = "_PSM_cms_NamedObjectList_Delete";
            this._PSM_cms_NamedObjectList_Delete.ShortcutKeyDisplayString = "Del";
            this._PSM_cms_NamedObjectList_Delete.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamedObjectList_Delete.Text = "Delete";
            this._PSM_cms_NamedObjectList_Delete.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_Delete_Click);
            // 
            // _PSM_cms_NamedObjectList_s_2
            // 
            this._PSM_cms_NamedObjectList_s_2.Name = "_PSM_cms_NamedObjectList_s_2";
            this._PSM_cms_NamedObjectList_s_2.Size = new System.Drawing.Size(208, 6);
            // 
            // _PSM_cms_NamedObjectList_MoveToFront
            // 
            this._PSM_cms_NamedObjectList_MoveToFront.Name = "_PSM_cms_NamedObjectList_MoveToFront";
            this._PSM_cms_NamedObjectList_MoveToFront.ShortcutKeyDisplayString = "Ctrl+Up";
            this._PSM_cms_NamedObjectList_MoveToFront.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamedObjectList_MoveToFront.Text = "Move to Front";
            this._PSM_cms_NamedObjectList_MoveToFront.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_MoveToFront_Click);
            // 
            // _PSM_cms_NamedObjectList_MoveToBack
            // 
            this._PSM_cms_NamedObjectList_MoveToBack.Name = "_PSM_cms_NamedObjectList_MoveToBack";
            this._PSM_cms_NamedObjectList_MoveToBack.ShortcutKeyDisplayString = "Ctrl+Down";
            this._PSM_cms_NamedObjectList_MoveToBack.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamedObjectList_MoveToBack.Text = "Move to Back";
            this._PSM_cms_NamedObjectList_MoveToBack.Click += new System.EventHandler(this._E_PSM_cms_NamedObjectList_MoveToBack_Click);
            // 
            // _PSM_cms_NamelessObjectList
            // 
            this._PSM_cms_NamelessObjectList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_NamelessObjectList_Select,
            this._PSM_cms_NamelessObjectList_s_1,
            this._PSM_cms_NamelessObjectList_Delete,
            this._PSM_cms_NamelessObjectList_s_2,
            this._PSM_cms_NamelessObjectList_MoveToFront,
            this._PSM_cms_NamelessObjectList_MoveToBack});
            this._PSM_cms_NamelessObjectList.Name = "_PSM_cms_NamedObjectList";
            this._PSM_cms_NamelessObjectList.Size = new System.Drawing.Size(212, 126);
            // 
            // _PSM_cms_NamelessObjectList_Select
            // 
            this._PSM_cms_NamelessObjectList_Select.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._PSM_cms_NamelessObjectList_SelectTop,
            this._PSM_cms_NamelessObjectList_SelectBottom,
            this._PSM_cms_NamelessObjectList_Select_s_1,
            this._PSM_cms_NamelessObjectList_SelectPrevious,
            this._PSM_cms_NamelessObjectList_SelectNext,
            this._PSM_cms_NamelessObjectList_Select_s_2,
            this._PSM_cms_NamelessObjectList_AddNext,
            this._PSM_cms_NamelessObjectList_AddPrevious,
            this._PSM_cms_NamelessObjectList_Select_s_3,
            this._PSM_cms_NamelessObjectList_SelectAll,
            this._PSM_cms_NamelessObjectList_SelectNone});
            this._PSM_cms_NamelessObjectList_Select.Name = "_PSM_cms_NamelessObjectList_Select";
            this._PSM_cms_NamelessObjectList_Select.ShortcutKeyDisplayString = "";
            this._PSM_cms_NamelessObjectList_Select.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamelessObjectList_Select.Tag = "";
            this._PSM_cms_NamelessObjectList_Select.Text = "Select";
            // 
            // _PSM_cms_NamelessObjectList_SelectTop
            // 
            this._PSM_cms_NamelessObjectList_SelectTop.Name = "_PSM_cms_NamelessObjectList_SelectTop";
            this._PSM_cms_NamelessObjectList_SelectTop.ShortcutKeyDisplayString = "Home";
            this._PSM_cms_NamelessObjectList_SelectTop.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectTop.Text = "Select top object";
            this._PSM_cms_NamelessObjectList_SelectTop.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_SelectTop_Click);
            // 
            // _PSM_cms_NamelessObjectList_SelectBottom
            // 
            this._PSM_cms_NamelessObjectList_SelectBottom.Name = "_PSM_cms_NamelessObjectList_SelectBottom";
            this._PSM_cms_NamelessObjectList_SelectBottom.ShortcutKeyDisplayString = "End";
            this._PSM_cms_NamelessObjectList_SelectBottom.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectBottom.Text = "Select bottom object";
            this._PSM_cms_NamelessObjectList_SelectBottom.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_SelectBottom_Click);
            // 
            // _PSM_cms_NamelessObjectList_Select_s_1
            // 
            this._PSM_cms_NamelessObjectList_Select_s_1.Name = "_PSM_cms_NamelessObjectList_Select_s_1";
            this._PSM_cms_NamelessObjectList_Select_s_1.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamelessObjectList_SelectPrevious
            // 
            this._PSM_cms_NamelessObjectList_SelectPrevious.Name = "_PSM_cms_NamelessObjectList_SelectPrevious";
            this._PSM_cms_NamelessObjectList_SelectPrevious.ShortcutKeyDisplayString = "Up";
            this._PSM_cms_NamelessObjectList_SelectPrevious.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectPrevious.Text = "Select previous object";
            this._PSM_cms_NamelessObjectList_SelectPrevious.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_SelectPrevious_Click);
            // 
            // _PSM_cms_NamelessObjectList_SelectNext
            // 
            this._PSM_cms_NamelessObjectList_SelectNext.Name = "_PSM_cms_NamelessObjectList_SelectNext";
            this._PSM_cms_NamelessObjectList_SelectNext.ShortcutKeyDisplayString = "Down";
            this._PSM_cms_NamelessObjectList_SelectNext.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectNext.Text = "Select next object";
            this._PSM_cms_NamelessObjectList_SelectNext.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_SelectNext_Click);
            // 
            // _PSM_cms_NamelessObjectList_Select_s_2
            // 
            this._PSM_cms_NamelessObjectList_Select_s_2.Name = "_PSM_cms_NamelessObjectList_Select_s_2";
            this._PSM_cms_NamelessObjectList_Select_s_2.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamelessObjectList_AddNext
            // 
            this._PSM_cms_NamelessObjectList_AddNext.Enabled = false;
            this._PSM_cms_NamelessObjectList_AddNext.Name = "_PSM_cms_NamelessObjectList_AddNext";
            this._PSM_cms_NamelessObjectList_AddNext.ShortcutKeyDisplayString = "Shift+Down";
            this._PSM_cms_NamelessObjectList_AddNext.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_AddNext.Text = "Expand/shrink selection downwards";
            // 
            // _PSM_cms_NamelessObjectList_AddPrevious
            // 
            this._PSM_cms_NamelessObjectList_AddPrevious.Enabled = false;
            this._PSM_cms_NamelessObjectList_AddPrevious.Name = "_PSM_cms_NamelessObjectList_AddPrevious";
            this._PSM_cms_NamelessObjectList_AddPrevious.ShortcutKeyDisplayString = "Shift+Up";
            this._PSM_cms_NamelessObjectList_AddPrevious.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_AddPrevious.Text = "Expand/shrink selection upwards";
            // 
            // _PSM_cms_NamelessObjectList_Select_s_3
            // 
            this._PSM_cms_NamelessObjectList_Select_s_3.Name = "_PSM_cms_NamelessObjectList_Select_s_3";
            this._PSM_cms_NamelessObjectList_Select_s_3.Size = new System.Drawing.Size(330, 6);
            // 
            // _PSM_cms_NamelessObjectList_SelectAll
            // 
            this._PSM_cms_NamelessObjectList_SelectAll.Enabled = false;
            this._PSM_cms_NamelessObjectList_SelectAll.Name = "_PSM_cms_NamelessObjectList_SelectAll";
            this._PSM_cms_NamelessObjectList_SelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this._PSM_cms_NamelessObjectList_SelectAll.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectAll.Text = "Select All";
            // 
            // _PSM_cms_NamelessObjectList_SelectNone
            // 
            this._PSM_cms_NamelessObjectList_SelectNone.Name = "_PSM_cms_NamelessObjectList_SelectNone";
            this._PSM_cms_NamelessObjectList_SelectNone.Size = new System.Drawing.Size(333, 22);
            this._PSM_cms_NamelessObjectList_SelectNone.Text = "Select None";
            this._PSM_cms_NamelessObjectList_SelectNone.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_SelectNone_Click);
            // 
            // _PSM_cms_NamelessObjectList_s_1
            // 
            this._PSM_cms_NamelessObjectList_s_1.Name = "_PSM_cms_NamelessObjectList_s_1";
            this._PSM_cms_NamelessObjectList_s_1.Size = new System.Drawing.Size(208, 6);
            // 
            // _PSM_cms_NamelessObjectList_Delete
            // 
            this._PSM_cms_NamelessObjectList_Delete.Name = "_PSM_cms_NamelessObjectList_Delete";
            this._PSM_cms_NamelessObjectList_Delete.ShortcutKeyDisplayString = "Del";
            this._PSM_cms_NamelessObjectList_Delete.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamelessObjectList_Delete.Text = "Delete";
            this._PSM_cms_NamelessObjectList_Delete.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_Delete_Click);
            // 
            // _PSM_cms_NamelessObjectList_s_2
            // 
            this._PSM_cms_NamelessObjectList_s_2.Name = "_PSM_cms_NamelessObjectList_s_2";
            this._PSM_cms_NamelessObjectList_s_2.Size = new System.Drawing.Size(208, 6);
            // 
            // _PSM_cms_NamelessObjectList_MoveToFront
            // 
            this._PSM_cms_NamelessObjectList_MoveToFront.Name = "_PSM_cms_NamelessObjectList_MoveToFront";
            this._PSM_cms_NamelessObjectList_MoveToFront.ShortcutKeyDisplayString = "Ctrl+Up";
            this._PSM_cms_NamelessObjectList_MoveToFront.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamelessObjectList_MoveToFront.Text = "Move to Front";
            this._PSM_cms_NamelessObjectList_MoveToFront.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_MoveToFront_Click);
            // 
            // _PSM_cms_NamelessObjectList_MoveToBack
            // 
            this._PSM_cms_NamelessObjectList_MoveToBack.Name = "_PSM_cms_NamelessObjectList_MoveToBack";
            this._PSM_cms_NamelessObjectList_MoveToBack.ShortcutKeyDisplayString = "Ctrl+Down";
            this._PSM_cms_NamelessObjectList_MoveToBack.Size = new System.Drawing.Size(211, 22);
            this._PSM_cms_NamelessObjectList_MoveToBack.Text = "Move to Back";
            this._PSM_cms_NamelessObjectList_MoveToBack.Click += new System.EventHandler(this._E_PSM_cms_NamelessObjectList_MoveToBack_Click);
            // 
            // _PanelSpaceMap
            // 
            this.BackColor = System.Drawing.Color.DarkRed;
            this.Controls.Add(this._PSM_p_mapContainer);
            this.Controls.Add(this._PSM_tb_scapeGoat);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "_PanelSpaceMap";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(615, 483);
            this.Resize += new System.EventHandler(this._E_PSM_Resize);
            this._PSM_p_mapContainer.ResumeLayout(false);
            this._PSM_cms_Main.ResumeLayout(false);
            this._PSM_cms_NamedObjectList.ResumeLayout(false);
            this._PSM_cms_NamelessObjectList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

      
		private void _E_PSM_cms_NamedObjectList_SelectPreviousObject_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_PreviousNamed())
				RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_SelectNextObject_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_NextNamed())
				RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_AddPreviousObject_Click(object sender, EventArgs e)
		{
			_namedObjectsLB.Focus();
			SendKeys.Send("+{UP}");
		}

		private void _E_PSM_cms_NamedObjectList_AddNextObject_Click(object sender, EventArgs e)
		{
			_namedObjectsLB.Focus();
			SendKeys.Send("+{DOWN}");
		}

		private void _E_PSM_cms_NamedObjectList_SelectAll_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_All())
				RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_SelectNone_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_None())
				RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_Delete_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_Count > 0)
				if (SpaceMap.Delete_Selected() > 0)
					RegisterChange("Deleted selection via list box");
		}

		private void _E_PSM_cms_NamedObjectList_MoveToFront_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_Count > 0)
				if (SpaceMap.Selection_ToFront())
					RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_MoveToBack_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Selection_Count > 0)
				if (SpaceMap.Selection_ToBack())
					RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamedObjectList_SelectTopObject_Click(object sender, EventArgs e)
		{
			if (_namedObjectsLB.Items.Count > 0)
			{
				_namedObjectsLB.SelectedItem = null;
				_namedObjectsLB.SelectedIndex = 0;
			}
		}

		private void _E_PSM_cms_NamedObjectList_SelectBottomObject_Click(object sender, EventArgs e)
		{
			if (_namedObjectsLB.Items.Count > 0)
			{
				_namedObjectsLB.SelectedItem = null;
				_namedObjectsLB.SelectedIndex = _namedObjectsLB.Items.Count - 1;
			}
		}

		private void _E_PSM_cms_NamelessObjectList_Delete_Click(object sender, EventArgs e)
		{
			if (SpaceMap.SelectionNameless != null)
				if (SpaceMap.Delete_Selected() > 0)
					RegisterChange("Deleted selection via list box");
		}

		private void _E_PSM_cms_NamelessObjectList_MoveToFront_Click(object sender, EventArgs e)
		{
			if (SpaceMap.SelectionNameless != null)
				if (SpaceMap.Selection_ToFront())
					RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamelessObjectList_MoveToBack_Click(object sender, EventArgs e)
		{
			if (SpaceMap.SelectionNameless != null)
				if (SpaceMap.Selection_ToBack())
					RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamelessObjectList_SelectTop_Click(object sender, EventArgs e)
		{
			if (_namelessObjectsLB.Items.Count > 0)
			{
				_namelessObjectsLB.SelectedItem = null;
				_namelessObjectsLB.SelectedIndex = 0;
			}
		}

		private void _E_PSM_cms_NamelessObjectList_SelectBottom_Click(object sender, EventArgs e)
		{
			if (_namelessObjectsLB.Items.Count > 0)
			{
				_namelessObjectsLB.SelectedItem = null;
				_namelessObjectsLB.SelectedIndex = _namelessObjectsLB.Items.Count - 1;
			}
		}

		private void _E_PSM_cms_NamelessObjectList_SelectPrevious_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_PreviousNameless())
				RegisterChange("Selection changed via list box");
		}

		private void _E_PSM_cms_NamelessObjectList_SelectNext_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_NextNameless())
				RegisterChange("Selection changed via list box");
		}


		private void _E_PSM_cms_NamelessObjectList_SelectNone_Click(object sender, EventArgs e)
		{
			if (SpaceMap.Select_None())
				RegisterChange("Selection changed via list box");
		}

		public void Undo(SpaceMapSavedState state = null)
		{
			if (SpaceMap.Undo(state))
			{
				UpdateFormText();
				Selection_Update();
				UpdateObjectLists();
				Redraw();
			}
		}

		public void Redo(SpaceMapSavedState state = null)
		{
			if (SpaceMap.Redo(state))
			{
				UpdateFormText();
				Selection_Update();
				UpdateObjectLists();
				Redraw();
			}
		}

		private bool CopyDo()
		{
			string Xml = "";
			if (SpaceMap.SelectionNameless != null)
				Xml += SpaceMap.SelectionNameless.ToXml(new XmlDocument(), new List<string>()).OuterXml;
			else
				foreach (NamedMapObject item in SpaceMap.namedObjects)
					if (item.Selected)
						Xml += item.ToXml(new XmlDocument(), new List<string>()).OuterXml;
			if (!string.IsNullOrWhiteSpace(Xml))
				Clipboard.SetText(Xml);

			return !string.IsNullOrWhiteSpace(Xml);
		}

		private int PasteDo()
		{
			XmlDocument xDoc = new XmlDocument();
			try
			{
				xDoc.LoadXml("<root>" + Helper.RemoveNodes(Helper.FixMissionXml(Clipboard.GetText())) + "</root>");
			}
			catch
			{
				return 0;
			}

			XmlNode root = xDoc.ChildNodes[0];

			List<NamedMapObject> namedObjects = new List<NamedMapObject>();
			List<NamelessMapObject> namelessObjects = new List<NamelessMapObject>();

			foreach (XmlNode childNode in root)
			{
				NamedMapObject mo = NamedMapObject.NewFromXml(childNode);
				NamelessMapObject nmo = NamelessMapObject.NewFromXml(childNode);

				if (mo != null)
					namedObjects.Add(mo);
				if (nmo != null)
					namelessObjects.Add(nmo);
			}

			if (namedObjects.Count == 0 && namelessObjects.Count == 0)
				return 0;

			SpaceMap.Select_None();
			
			foreach(NamedMapObject item in namedObjects)
				SpaceMap.namedObjects.Add(item);
			foreach(NamelessMapObject item in namelessObjects)
				SpaceMap.namelessObjects.Add(item);

			if (namedObjects.Count == 0 && namelessObjects.Count > 0)
				SpaceMap.SelectionNameless = namelessObjects[namelessObjects.Count - 1];
			else
				foreach (NamedMapObject item in namedObjects)
					item.Selected = true;

			return namedObjects.Count + namelessObjects.Count;
		}

		public void Cut()
		{
			int result;
			if (CopyDo())
				if ((result = SpaceMap.Delete_Selected()) > 0)
					RegisterChange("Cut " + result + " object(s)");
		}

		public void Copy()
		{
			CopyDo();
		}

		public void Paste()
		{
			int result;
			if ((result = PasteDo()) > 0)
				RegisterChange("Pasted " + result + " object(s)");
		}
	}
}