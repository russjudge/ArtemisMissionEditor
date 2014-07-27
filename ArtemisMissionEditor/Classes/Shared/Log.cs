﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    // Microsoft.Design wants to have object sender, who cares if it's useless in a static class?
    public delegate void NewLogEntryEventHandler();

    public static class Log
    {
        public static string _fileName = "log.txt";

        public static ListBox.ObjectCollection LogLines {get{return Program.FormLogInstance._FL_lb_Log.Items;}}
        private static List<string> _logLines = new List<string>();
        public static List<string> LogDisplayLinesTemp = new List<string>();

        public static event NewLogEntryEventHandler NewLogEntry;

        private static void OnNewLogEntry()
        {
            if (NewLogEntry!=null)
				NewLogEntry();
        }

        public static int UnreadLines { get; set; }

        public static void Save()
        {
            string fileName = Environment.ExpandEnvironmentVariables(Settings.ProgramDataFolder + _fileName);
            StreamWriter ostream = null;
            try
            {
                Settings.EnsureProgramDataFolderExists();
                ostream = new StreamWriter(fileName, true);
                foreach (string item in _logLines)
                    ostream.Write(item + "\r\n");

                ostream.Close();
            }
            catch
            {
                if (ostream != null)
                    ostream.Close();
                throw;
            }
        }

        public static void Add(string text)
        {
            _logLines.Add(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + text);
            if (Program.FormLogInstance==null)
                LogDisplayLinesTemp.Add(DateTime.Now.ToString("[HH:mm:ss] ") + text);
            else
                Program.FormLogInstance._FL_lb_Log.Items.Add(DateTime.Now.ToString("[HH:mm:ss] ") + text);
            UnreadLines++;
            
            OnNewLogEntry();
        }

        public static void Add(Exception e) { Add(e.Message + " "+e.StackTrace); }

        public static void MarkAsRead()
        {
            UnreadLines = 0;
        }
    }
}
