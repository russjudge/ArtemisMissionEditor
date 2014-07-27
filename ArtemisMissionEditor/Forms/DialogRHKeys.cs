using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
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

        public static KeyValuePair<bool, string> Show(string name, string initialValue, DialogRHKeysMode mode)
		{
            string result = "";
            using (DialogRHKeys form = new DialogRHKeys())
            {
                string caption = name;
                form.Text = caption;
                List<string> keys = initialValue.Split().ToList();

                //Fill and name lists
                switch (mode)
                {
                    case DialogRHKeysMode.HullBroadTypesClassNames:
                        form._DRHK_l_1.Text = "Class Names";
                        foreach (string item in VesselData.Current.VesselClassNames)
                        {
                            form._DRHK_lb_1.Items.Add(item);
                            form._DRHK_lb_1.SetItemChecked(form._DRHK_lb_1.Items.Count - 1, keys.Contains(item));
                            if (keys.Contains(item)) keys.Remove(item);
                        }
                        form._DRHK_l_2.Text = "Broad Types";
                        foreach (string item in VesselData.Current.VesselBroadTypes)
                        {
                            form._DRHK_lb_2.Items.Add(item);
                            form._DRHK_lb_2.SetItemChecked(form._DRHK_lb_2.Items.Count - 1, keys.Contains(item));
                            if (keys.Contains(item)) keys.Remove(item);
                        }
                        break;
                    case DialogRHKeysMode.RaceNamesKeys:
                        form._DRHK_l_1.Text = "Race Names";
                        foreach (string item in VesselData.Current.RaceNames)
                        {
                            form._DRHK_lb_1.Items.Add(item);
                            form._DRHK_lb_1.SetItemChecked(form._DRHK_lb_1.Items.Count - 1, keys.Contains(item));
                            if (keys.Contains(item)) keys.Remove(item);
                        }
                        form._DRHK_l_2.Text = "Race Keys";
                        foreach (string item in VesselData.Current.RaceKeys)
                        {
                            form._DRHK_lb_2.Items.Add(item);
                            form._DRHK_lb_2.SetItemChecked(form._DRHK_lb_2.Items.Count - 1, keys.Contains(item));
                            if (keys.Contains(item)) keys.Remove(item);
                        }
                        break;
                }

                string unrecognised = "";
                foreach (string item in keys)
                    unrecognised += item + " ";

                form.tbUnrecognised.Text = unrecognised.Trim();

                if (form.ShowDialog() == DialogResult.Cancel)
                    return new KeyValuePair<bool, string>(false, null);

                
                foreach (string item in form._DRHK_lb_1.CheckedItems)
                    result += item + " ";
                foreach (string item in form._DRHK_lb_2.CheckedItems)
                    result += item + " ";
                foreach (string item in form.tbUnrecognised.Text.Split())
                    result += item + " ";
                result = result.Trim();
            }
			return new KeyValuePair<bool, string>(true, result.ToString());
		}

		private void ckButton_Click(object sender, EventArgs e)
		{
			Close_OK();
		}

		private void Close_OK()
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < _DRHK_lb_1.Items.Count; ++i)
				_DRHK_lb_1.SetItemChecked(i, false);
			for (int i = 0; i < _DRHK_lb_2.Items.Count; ++i)
				_DRHK_lb_2.SetItemChecked(i, false);
		}



	}
}
