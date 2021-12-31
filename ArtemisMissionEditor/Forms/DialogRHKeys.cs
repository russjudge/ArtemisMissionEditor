using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor.Forms
{
	public enum DialogRHKeysMode
	{
		RaceNamesKeys,
		HullBroadTypesClassNames
	}

	public partial class DialogRHKeys : Form
	{
		private DialogRHKeys()
		{
			InitializeComponent();
		}

        private DialogRHKeysMode Mode;
        private string InitialRaceKeys;
        private string InitialHullKeys;

        public static KeyValuePair<bool, string> Show(string name, string raceKeys, string hullKeys, DialogRHKeysMode mode)
		{
            string result = "";
            using (DialogRHKeys form = new DialogRHKeys())
            {
                string caption = name;
                form.Text = caption;
                form.Mode = mode;
                form.InitialRaceKeys = raceKeys;
                form.InitialHullKeys = hullKeys;
                List<string> keys = raceKeys.Split(' ').ToList();

                //Fill and name lists
                switch (mode)
                {
                    case DialogRHKeysMode.HullBroadTypesClassNames:
                        {
                            Tuple<List<string>, List<string>, List<string>> parsedKeys = VesselData.Current.ParseHullKeys(hullKeys);
                            // Left Listbox
                            form._DRHK_l_1.Text = "Class Names";
                            foreach (string item in VesselData.Current.VesselClassNames)
                            {
                                form._DRHK_lb_1.Items.Add(item);
                                form._DRHK_lb_1.SetItemChecked(form._DRHK_lb_1.Items.Count - 1, parsedKeys.Item1.Contains(item));
                            }
                            // Right Listbox
                            form._DRHK_l_2.Text = "Broad Types";
                            foreach (string item in VesselData.Current.VesselBroadTypes)
                            {
                                form._DRHK_lb_2.Items.Add(item);
                                form._DRHK_lb_2.SetItemChecked(form._DRHK_lb_2.Items.Count - 1, parsedKeys.Item2.Contains(item));
                            }
                            // Unrecognized
                            form.tbUnrecognised.Text = parsedKeys.Item3.Count == 0 ? "" : parsedKeys.Item3.Aggregate((i, j) => i + " " + j);
                        }
                        break;
                    case DialogRHKeysMode.RaceNamesKeys:
                        {
                            Tuple<List<string>, List<string>, List<string>> parsedKeys = VesselData.Current.ParseRaceKeys(raceKeys);
                            // Left Listbox
                            form._DRHK_l_1.Text = "Race Names";
                            foreach (string item in VesselData.Current.RaceNames)
                            {
                                form._DRHK_lb_1.Items.Add(item);
                                form._DRHK_lb_1.SetItemChecked(form._DRHK_lb_1.Items.Count - 1, parsedKeys.Item1.Contains(item));
                            }
                            // Right Listbox
                            form._DRHK_l_2.Text = "Race Keys";
                            foreach (string item in VesselData.Current.RaceKeys)
                            {
                                form._DRHK_lb_2.Items.Add(item);
                                form._DRHK_lb_2.SetItemChecked(form._DRHK_lb_2.Items.Count - 1, parsedKeys.Item2.Contains(item));
                            }
                            // Unrecognized
                            form.tbUnrecognised.Text = parsedKeys.Item3.Count == 0 ? "" : parsedKeys.Item3.Aggregate((i, j) => i + " " + j);
                        }
                        break;
                }

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                result = form.GetResult();
            }
			return new KeyValuePair<bool, string>(true, result);
		}

        private string GetResult()
        {
            string result = "";
            foreach (string item in _DRHK_lb_1.CheckedItems)
                result += item + " ";
            foreach (string item in _DRHK_lb_2.CheckedItems)
                result += item + " ";
            foreach (string item in tbUnrecognised.Text.Split())
                result += item + " ";
            return result.Trim();
        }

		private void ckButton_Click(object sender, EventArgs e)
		{
			Close_OK();
		}

		private void Close_OK()
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < _DRHK_lb_1.Items.Count; ++i)
				_DRHK_lb_1.SetItemChecked(i, false);
			for (int i = 0; i < _DRHK_lb_2.Items.Count; ++i)
				_DRHK_lb_2.SetItemChecked(i, false);
		}

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Race and hull keys work in a very peculiar manner, therefore the result you get may be not quite what you normally expect. You can read detalied information on the relevant help page.",
                "Notice about race/hull keys.");
        }

        private void _DRHK_lb_1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                this.BeginInvoke((MethodInvoker)(() => RestartUpdateTimer()));
            }
            catch
            { 

            }
        }

        private void _DRHK_lb_2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                this.BeginInvoke((MethodInvoker)(() => RestartUpdateTimer()));
            }
            catch
            {

            }
        }

        private void tbUnrecognised_TextChanged(object sender, EventArgs e)
        {
            RestartUpdateTimer();
        }

        private void RestartUpdateTimer()
        {
            timerValidate.Stop();
            timerValidate.Start();
        }

        private void timerValidate_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdatePossibleLists();

                timerValidate.Stop();
            }
            catch
            { 

            }
        }

        private void UpdatePossibleLists()
        {
            lbPossibleRaces.Items.Clear();
            lbPossibleVessels.Items.Clear();

            string raceKeys = "";
            string hullKeys = "";

            switch (Mode)
            { 
                case DialogRHKeysMode.HullBroadTypesClassNames:
                    raceKeys = InitialRaceKeys;
                    hullKeys = GetResult();
                    break;
                case DialogRHKeysMode.RaceNamesKeys:
                    raceKeys = GetResult();
                    hullKeys = InitialHullKeys;
                    break;
            }

            //Prepare races list
            List<string> possibleRaceIDs = VesselData.Current.GetPossibleRaceIDs(raceKeys);

            //Output races list
            foreach (string item in possibleRaceIDs)
                lbPossibleRaces.Items.Add(VesselData.Current.RaceToString(item));

            //Prepare vessel list
            List<string> possibleVesselIDs = VesselData.Current.GetPossibleVesselIDs(possibleRaceIDs, hullKeys);

            //Finally output the vessel list
            foreach (string id in possibleVesselIDs)
                lbPossibleVessels.Items.Add(VesselData.Current.VesselToString(id));

            if (possibleVesselIDs.Count == 0)
            {
                if (String.IsNullOrWhiteSpace(hullKeys) )
                {
                    lbPossibleVessels.Items.Add("Attribute \"hullKeys\" is blank.");
                    lbPossibleVessels.Items.Add("Server will crash when executing this statement."); 
                }
                else
                {
                    lbPossibleVessels.Items.Add("No possible vessels found.");
                    lbPossibleVessels.Items.Add("Check if you have vesselData.xml loaded.");
                    lbPossibleVessels.Items.Add("Otherwise please report this as a bug.");
                }
            }
        }

        private void DialogRHKeys_Load(object sender, EventArgs e)
        {
            UpdatePossibleLists();
        }


	}
}
