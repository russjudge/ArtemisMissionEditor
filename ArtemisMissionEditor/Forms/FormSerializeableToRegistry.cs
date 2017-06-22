using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;

namespace ArtemisMissionEditor.Forms
{
	public class FormSerializeableToRegistry : Form
	{
		protected SplitContainer _mainSC;
		protected SplitContainer _rightSC;
		protected string ID;

		public void SaveToRegistry()
		{
			RegistryKey key;

			key = Registry.CurrentUser.OpenSubKey("Software\\ArtemisMissionEditor\\" + ID, true);
			key.SetValue("WindowState", this.WindowState);
            key.SetValue("X", Location.X < 0 ? 0 : Location.X);
            key.SetValue("Y", Location.Y < 0 ? 0 : Location.Y);
			key.SetValue("W", Width);
			key.SetValue("H", Height);
			if (_mainSC != null)
				key.SetValue("M", _mainSC.SplitterDistance);
			if (_rightSC != null)
				key.SetValue("R", _rightSC.SplitterDistance);  
		}  

		public void LoadFromRegistry()
		{
			RegistryKey key;

			key = Registry.CurrentUser.OpenSubKey("Software\\ArtemisMissionEditor\\" + ID);
			if (key == null)
			{
				key = Registry.CurrentUser.CreateSubKey("Software\\ArtemisMissionEditor\\" + ID);
			}

			//WindowState 
			if (key.GetValue("WindowState") != null)
			{
				string strWindowsState = (string)key.GetValue("WindowState");
				if (strWindowsState != null && strWindowsState.CompareTo("Maximized") == 0)
				{
					WindowState = System.Windows.Forms.FormWindowState.Maximized;
				}
				//else if (strWindowsState != null && strWindowsState.CompareTo("Minimized") == 0)
				//{
				//    WindowState = System.Windows.Forms.FormWindowState.Minimized;
				//}
				else
				{
					WindowState = FormWindowState.Normal;
				}
			}

			//Location of Windows 
			if (key.GetValue("X") != null)
			{
                int x = (int)key.GetValue("X");
                x = x < 0 ? 0 : x;
                x = x > Screen.PrimaryScreen.Bounds.Width ? 0 : x;
                int y = (int)key.GetValue("Y");
                y = y < 0 ? 0 : y;
                y = y > Screen.PrimaryScreen.Bounds.Height ? 0 : y;
                Location = new Point(x, y);
			}

			//Size of windows 6
			if (key.GetValue("W") != null)
			{
				Size = new Size((int)key.GetValue("W"), (int)key.GetValue("H"));
			}
			
			//Positions of splitters
			if (key.GetValue("M") != null && _mainSC != null)
			{
				_mainSC.SplitterDistance = (int)key.GetValue("M");
			}
			if (key.GetValue("R") != null && _rightSC != null)
			{
				_rightSC.SplitterDistance = (int)key.GetValue("R");
			}  
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormSerializeableToRegistry
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "FormSerializeableToRegistry";
            this.ResumeLayout(false);

        }

	}
}
