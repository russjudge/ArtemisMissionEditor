using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace EngineeringTemplateMaster
{
	static class Program
	{
		public static bool StartArtemis;
        public static string ArtemisPath;

        public static bool IsArtemisPath(string path)
        {
            return File.Exists(path + "Artemis.exe");
        }

        public static string GetArtemisPath()
		{
            //return @"C:\Program Files (x86)\Artemis\";
			List<string> possiblePaths = new List<string>();

			if (File.Exists("ArtemisPath.cfg"))
			{
				StreamReader sr = new StreamReader("ArtemisPath.cfg");
				possiblePaths.AddRange(sr.ReadToEnd().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
			}

            possiblePaths.Add(Directory.GetCurrentDirectory() + "\\" + @"");
            possiblePaths.Add(Directory.GetParent(Directory.GetCurrentDirectory()) + "\\");
            possiblePaths.Add(Directory.GetCurrentDirectory() + "\\" + @"Artemis\");
            possiblePaths.Add(Directory.GetParent(Directory.GetCurrentDirectory()) + "\\" + @"Artemis\");

			foreach (DriveInfo item in DriveInfo.GetDrives())
			{
				if (!item.IsReady)
					continue;
				possiblePaths.Add(item.RootDirectory + @"Program Files\Artemis\");
				possiblePaths.Add(item.RootDirectory + @"Program Files (x86)\Artemis\");
				possiblePaths.Add(item.RootDirectory + @"Games\Artemis\");
			}


			foreach (string item in possiblePaths)
				if (IsArtemisPath(item))
					return item;

			return null;
		}

		public static bool FindArtemisPath()
		{
			ArtemisPath = GetArtemisPath();

			//Prompt for artemis location
			if (ArtemisPath == null)
			{
				//TODO: Actually prompt for location
				ArtemisPath = "";
			}

			return true;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            StartArtemis = false;

			if (!FindArtemisPath())
			{
				MessageBox.Show("Artemis not found!");
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormEngineering());

			if (StartArtemis)
			{
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.UseShellExecute = true;
                startInfo.FileName = ArtemisPath + @"Artemis.exe";
				startInfo.WorkingDirectory = ArtemisPath;
				Process.Start(startInfo);
				
			}
		}
	}
}
