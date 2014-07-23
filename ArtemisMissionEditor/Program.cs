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
        public static FormSettings				FormSettingsInstance;
		public static FormLog					FormLogInstance;
        public static FormMain					FormMainInstance;
		public static FormFindReplace			FormFindReplaceInstance;
        public static FormSearchResults		    FormSearchResultsInstance;
        public static FormDependency			FormDependencyInstance;
		public static FormHelp					FormHelpInstance;
		public static FormMissionProperties	    FormMissionPropertiesInstance;

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

            FormLogInstance  = new   FormLog();
            FormSettingsInstance  = new   FormSettings();
            FormMainInstance  = new   FormMain();
			FormFindReplaceInstance = new   FormFindReplace();
            FormSearchResultsInstance = new   FormSearchResults();
            FormDependencyInstance  = new   FormDependency();
			FormHelpInstance	= new	FormHelp();
			FormMissionPropertiesInstance	= new	FormMissionProperties();

            string currentVesselDataPathToLoad = customVesselDataFileName ?? Settings.Current.DefaultVesselDataPath;
            if (File.Exists(currentVesselDataPathToLoad))
                Settings.VesselData.Load(currentVesselDataPathToLoad);
            
            IsClosing = false;

            if (File.Exists(missionFileName))
                Mission.Current.FromFile(missionFileName);

            //Start
            Application.Run(FormMainInstance);

			//End
			Program.IsClosing = true;
			Program.FormLogInstance.SaveToRegistry();
			Program.FormLogInstance.Close();
			Program.FormSettingsInstance.SaveToRegistry();
			Program.FormSettingsInstance.Close();
			Program.FormFindReplaceInstance.SaveToRegistry();
			Program.FormFindReplaceInstance.Close();
			Program.FormSearchResultsInstance.SaveToRegistry();
			Program.FormSearchResultsInstance.Close();
            Program.FormDependencyInstance.SaveToRegistry();
            Program.FormDependencyInstance.Close();
			Program.FormHelpInstance.SaveToRegistry();
			Program.FormHelpInstance.Close();
			Program.FormMissionPropertiesInstance.SaveToRegistry();
			Program.FormMissionPropertiesInstance.Close();
        }
    }
}
