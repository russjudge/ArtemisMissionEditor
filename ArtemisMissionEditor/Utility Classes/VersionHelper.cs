using System;
using System.Diagnostics;

namespace ArtemisMissionEditor
{
    public sealed class VersionHelper
    {
        const string UpdaterName = "ArtemisMissionEditorUpdater.exe";
        const string UpdaterArgs = "/silent";

        public static void CheckVersion()
        {
            try
            {
                Process.Start(UpdaterName, UpdaterArgs);
            }
            catch (Exception e)
            {
                // Silently ignore failures.
                string message = e.Message;
            }
        }
    }
}
