using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Win32;

namespace ArtemisMissionEditor
{
    public interface IMRUClient
    {
        void OpenMRUFile(string fileName);
    }

    public sealed class MRUManager
    {
        private static MRUManager _current = new MRUManager();
        public static MRUManager Current => _current;

        public ToolStripMenuItem MRUMenu;
        public ToolStripMenuItem AutosavesMenu;

        private Form _ownerForm;
        private ToolStripMenuItem _menuItemParent; // Recent Files menu item parent

        private ToolStripMenuItem AddMenuItem(ToolStripMenuItem menu, string text, bool latest)
        {
            RemoveMenuItem(menu, text);

            var item = new ToolStripMenuItem(text);
            if (latest)
            {
                menu.DropDownItems.Insert(0, item);
            }
            else
            {
                menu.DropDownItems.Add(item);
            }
            return item;
        }

        public void AddRecentFile(string missionFileName, bool latest = true)
        {
            // Don't add an autosave filename.
            if (missionFileName.Contains("autosave"))
            {
                return;
            }

            ToolStripMenuItem item = AddMenuItem(MRUMenu, missionFileName, latest);

            item.Click += RecentFile_Click;

            if (latest)
            {
                SaveMRU();
            }
        }

        public ToolStripMenuItem AddAutosaveFile(string timestamp, bool latest = true)
        {
            ToolStripMenuItem item = AddMenuItem(AutosavesMenu, timestamp, latest);

            item.Click += AutosaveFile_Click;

            return item;
        }

        private void AutosaveFile_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            string timestamp = item.Text;

            // Find file with the specified timestamp.
            string path = Environment.ExpandEnvironmentVariables("%APPDATA%\\ArtemisMissionEditor");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            var fileInfos = dirInfo.EnumerateFiles("autosave*.xml").Where(i => i.LastWriteTime.ToString() == timestamp);
            FileInfo info = fileInfos.FirstOrDefault();
            if (info != null)
            {
                ((IMRUClient)_ownerForm).OpenMRUFile(info.FullName);
            }
            else
            {
                MessageBox.Show("File no longer exists, removing it from recent files...");
                RemoveAutosaveFile(timestamp);
            }
        }

        private void RecentFile_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            string missionFileName = item.Text;
            if (File.Exists(missionFileName))
            {
                ((IMRUClient)_ownerForm).OpenMRUFile(missionFileName);
            } else
            {
                MessageBox.Show("File no longer exists, removing it from recent files...");
                RemoveRecentFile(missionFileName);
            }
        }

        private void RemoveMenuItem(ToolStripMenuItem menu, string text)
        {
            foreach (var item in menu.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem == null)
                    continue;
                if (menuItem.Text == text)
                {
                    menu.DropDownItems.Remove(menuItem);
                    return;
                }
            }
        }

        public void RemoveRecentFile(string missionFileName)
        {
            RemoveMenuItem(MRUMenu, missionFileName);
        }

        public void RemoveAutosaveFile(string timestamp)
        {
            RemoveMenuItem(AutosavesMenu, timestamp);
        }

        public void Initialize(Form owner, ToolStripMenuItem parent, ToolStripMenuItem mruMenuItem, ToolStripMenuItem autosavesMenuItem)
        {
            _ownerForm = owner;

            // check if owner form implements IMRUClient interface
            if (!(owner is IMRUClient))
            {
                throw new Exception("MRUManager: Owner form doesn't implement IMRUClient interface");
            }

            MRUMenu = mruMenuItem;
            AutosavesMenu = autosavesMenuItem;

            // keep reference to MRU menu item parent
            _menuItemParent = parent;
            if (_menuItemParent == null)
            {
                throw new Exception("MRUManager: Cannot find parent of MRU menu item");
            }

            // subscribe to MRU parent Popup event
            _menuItemParent.DropDownOpening += new EventHandler(OnFileMenuOpening);

            // subscribe to owner form Closing event
            _ownerForm.Closing += new System.ComponentModel.CancelEventHandler(OnFormClosing);

            LoadMRU();
            ToolStripMenuItem autosaveMenuItem = LoadAutosavesList();
            if (autosaveMenuItem != null)
            {
                // Looks like the autosave timestamp is newer than the most recent file, so as the user if they want to recover it.
                DialogResult result = MessageBox.Show(
                    "Do you want to recover where you left off at " + autosaveMenuItem.Text + "?",
                    "Recover using autosaved copy",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    AutosaveFile_Click(autosaveMenuItem, null);
                }
            }
        }

        private static RegistryKey GetRegistryKey()
        {
            RegistryKey key = Settings.GetRegistryKey();
            RegistryKey mru = key.OpenSubKey("Recent Files List", true);
            if (mru == null)
                mru = key.CreateSubKey("Recent Files List");
            return mru;
        }

        /// <summary>
        /// Read the autosaves list from disk. 
        /// </summary>
        private ToolStripMenuItem LoadAutosavesList()
        {
            var itemToRecoverTimestamp = new DateTime();
            ToolStripMenuItem itemToRecover = null;

            AutosavesMenu.DropDownItems.Clear();

            string path = Environment.ExpandEnvironmentVariables("%APPDATA%\\ArtemisMissionEditor");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            var fileInfos = dirInfo.EnumerateFiles("autosave*.xml");
            var sortedFileInfos = fileInfos.OrderBy(i => i.LastWriteTime).ToList();
            foreach (FileInfo info in sortedFileInfos)
            {
                itemToRecoverTimestamp = info.LastWriteTime;
                itemToRecover = AddAutosaveFile(itemToRecoverTimestamp.ToString());
            }

            // See if itemToRecover is newer than most recent file timestamp.
            if (itemToRecover != null && MRUMenu.DropDownItems.Count > 0)
            {
                var mostRecentFileItem = MRUMenu.DropDownItems[0] as ToolStripMenuItem;
                string missionFileName = mostRecentFileItem.Text;
                if (File.Exists(missionFileName))
                {
                    FileInfo info = new FileInfo(missionFileName);
                    if (info.LastWriteTime > itemToRecoverTimestamp)
                    {
                        // Looks like we saved successfully.
                        return null;
                    }
                }
            }

            return itemToRecover;
        }

        /// <summary>
        /// Read the MRU list from registry. 
        /// </summary>
        private void LoadMRU()
        {
            MRUMenu.DropDownItems.Clear();

            var key = GetRegistryKey();
            foreach (string valueName in key.GetValueNames())
            {
                string missionFileName = key.GetValue(valueName) as string;
                AddRecentFile(missionFileName, false);
            }
        }

        /// <summary>
        /// Save MRU list to registry.
        /// </summary>
        private void SaveMRU()
        {
            var key = GetRegistryKey();

            for (int index = 0; index < MRUMenu.DropDownItems.Count; index++)
            {
                var mruItem = MRUMenu.DropDownItems[index];

                var name = String.Format("File{0}", index + 1);
                var value = mruItem.Text;

                key.SetValue(name, value);
            }
        }

        // The OnMRUParentClosing method is called when the owner form is closed and saves the MRU list to the registry. 
        private void OnFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveMRU();
        }

        private void OnFileMenuOpening(object sender, EventArgs e)
        {
            MRUMenu.Enabled = (MRUMenu.DropDownItems.Count > 0);
            AutosavesMenu.Enabled = (AutosavesMenu.DropDownItems.Count > 0);
        }
    }
}
