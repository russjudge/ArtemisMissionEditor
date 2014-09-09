using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using ArtemisMissionEditor.Forms;

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
        public static FormNotepad               FormNotepadInstance;
        public static List<Form>                AllOwnedForms;

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

            FormLogInstance = new FormLog();
            FormMainInstance = new FormMain();
            FormSettingsInstance = new FormSettings();
            FormFindReplaceInstance = new   FormFindReplace();
            FormSearchResultsInstance = new   FormSearchResults();
            FormDependencyInstance  = new   FormDependency();
            FormHelpInstance	= new	FormHelp();
            FormMissionPropertiesInstance	= new	FormMissionProperties();
            FormNotepadInstance = new FormNotepad();

            AllOwnedForms = new List<Form>();
            AllOwnedForms.Add(FormSettingsInstance);
            AllOwnedForms.Add(FormSettingsInstance);
            AllOwnedForms.Add(FormFindReplaceInstance);
            AllOwnedForms.Add(FormSearchResultsInstance);
            AllOwnedForms.Add(FormDependencyInstance);
            AllOwnedForms.Add(FormHelpInstance);
            AllOwnedForms.Add(FormMissionPropertiesInstance);
            AllOwnedForms.Add(FormNotepadInstance);
            AllOwnedForms.Add(FormLogInstance);

            foreach (Form form in AllOwnedForms)
            {
                form.Owner = FormMainInstance;
                form.Deactivate +=  new EventHandler((object sender, EventArgs e) => { ((Form)sender).Opacity = Settings.Current.FormOpacity; });
                form.Activated += new EventHandler((object sender, EventArgs e) => { ((Form)sender).Opacity = 1.0; });
            }

            string currentVesselDataPathToLoad = customVesselDataFileName ?? Settings.Current.DefaultVesselDataPath;
            if (File.Exists(currentVesselDataPathToLoad))
                VesselData.Current.Load(currentVesselDataPathToLoad);
            
            if (File.Exists(missionFileName))
                Mission.Current.FromFile(missionFileName);

            //Start
            Application.Run(FormMainInstance);
        }

        public static void UpdateAllFormsOpacity()
        {
            if (AllOwnedForms != null)
                foreach (Form form in AllOwnedForms)
                    if (form != Form.ActiveForm)
                        form.Opacity = Settings.Current.FormOpacity;   
        }

        /// <summary>
        /// Shows Main form if it is the last form remaining (called upon hiding another form)
        /// </summary>
        /// <remarks>
        /// WTF MICROSOFT, WHY DOES IT GO OUT OF FOCUS!?
        /// EDIT: Apparently, if you show a form from a form, it will happen, so all forms that open forms should have this line
        /// Stupid Miscrosoft...
        /// </remarks>
        public static void ShowMainFormIfRequired()
        {
            int countOpenforms = 0;
            foreach (Form form in AllOwnedForms)
                if (form.Visible)
                    countOpenforms++;
            if (countOpenforms == 0)
                FormMainInstance.BringToFront();
        }
    }
}
