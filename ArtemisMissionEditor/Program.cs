using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace ArtemisMissionEditor
{
    static class Program
    {
        public static _FormSettings				FS;
		public static _FormLog					FL;
        public static _FormMain					FM;
		public static _FormFindReplace			FFR;
        public static _FormSearchResults		FSR;
        public static _FormDependency			FD;
		public static _FormHelp					FH;
		public static _FormMissionProperties	FMP;

        public static bool IsClosing { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //MY INIT:
            if (!Settings.Load())
            {
                Settings.Current = new Settings();
                Settings.Save();
            }

            string missionFileName = null;
            string customVesselDataFileName = null;

            string[] argList = Environment.GetCommandLineArgs();
            if (argList.Length > 2 && argList[1] == "-v")
                customVesselDataFileName = argList[2];
            else if (argList.Length > 3 && argList[2] == "-v")
                customVesselDataFileName = argList[3];
            if (argList.Length > 1 && argList[1] != "-v")
                missionFileName = argList[1];

            FL  = new   _FormLog();
            FS  = new   _FormSettings();
            FM  = new   _FormMain();
			FFR = new   _FormFindReplace();
            FSR = new   _FormSearchResults();
            FD  = new   _FormDependency();
			FH	= new	_FormHelp();
			FMP	= new	_FormMissionProperties();

            string currentVesselDataPathToLoad = customVesselDataFileName ?? Settings.Current.DefaultVesselDataPath;
            if (File.Exists(currentVesselDataPathToLoad))
                Settings.VesselData.Load(currentVesselDataPathToLoad);
            
            IsClosing = false;

            if (File.Exists(missionFileName))
                Mission.Current.FromFile(missionFileName);

            //Start
            Application.Run(FM);

			//End
			Program.IsClosing = true;
			Program.FL.SaveToRegistry();
			Program.FL.Close();
			Program.FS.SaveToRegistry();
			Program.FS.Close();
			Program.FFR.SaveToRegistry();
			Program.FFR.Close();
			Program.FSR.SaveToRegistry();
			Program.FSR.Close();
            Program.FD.SaveToRegistry();
            Program.FD.Close();
			Program.FH.SaveToRegistry();
			Program.FH.Close();
			Program.FMP.SaveToRegistry();
			Program.FMP.Close();
        }
    }
}
