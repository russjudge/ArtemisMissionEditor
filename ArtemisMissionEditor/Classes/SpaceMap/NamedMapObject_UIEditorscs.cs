using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ArtemisMissionEditor
{
    public sealed class CheckedListBoxUITypeEditor_vesselClassNames : System.Drawing.Design.UITypeEditor, IDisposable
    {
        public CheckedListBox cbx = new CheckedListBox();

        private IWindowsFormsEditorService es;

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable { get { return true; } }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            int i;

            //instantiate the custom property editor service provider
            es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (es != null)
            {
                //load the ListBox items from vesseldata and check them appropriately
                cbx.Items.Clear();
                cbx.Sorted = false; //dont sort while adding

                foreach (string item in VesselData.Current.VesselClassNames)
                {
                    cbx.Items.Add(item);
                    if (value != null)
                        if (((string)value).Split(' ').Contains(item))
                            cbx.SetItemChecked(cbx.Items.Count - 1, true);
                }

                cbx.Sorted = true;
                cbx.CheckOnClick = true;

                //show the control and wait for user input
                es.DropDownControl(cbx);

                //Apply values to a temp list and return it
                List<string> tmp = new List<string>();

                for (i = 0; i < cbx.Items.Count; i++)
                    if (cbx.GetItemChecked(i))
                        tmp.Add(cbx.Items[i].ToString());

                if (tmp.Count == 0)
                    return "";
                else
                    return tmp.Aggregate((alfa, beta) => alfa + " " + beta);
            }
            return null;
        }

        public void Dispose()
        {
            cbx.Dispose();
        }
    }

    public sealed class CheckedListBoxUITypeEditor_vesselBroadTypes : System.Drawing.Design.UITypeEditor, IDisposable
    {
        public CheckedListBox cbx = new CheckedListBox();

        private IWindowsFormsEditorService es;

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable { get { return true; } }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            int i;

            //instantiate the custom property editor service provider
            es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (es != null)
            {
                //load the ListBox items from vesseldata and check them appropriately
                cbx.Items.Clear();
                cbx.Sorted = false; //dont sort while adding

                foreach (string item in VesselData.Current.VesselBroadTypes)
                {
                    cbx.Items.Add(item);
                    if (value != null)
                        if (((string)value).Split(' ').Contains(item))
                            cbx.SetItemChecked(cbx.Items.Count - 1, true);
                }

                cbx.Sorted = true;
                cbx.CheckOnClick = true;

                //show the control and wait for user input
                es.DropDownControl(cbx);

                //Apply values to a temp list and return it
                List<string> tmp = new List<string>();

                for (i = 0; i < cbx.Items.Count; i++)
                    if (cbx.GetItemChecked(i))
                        tmp.Add(cbx.Items[i].ToString());

                if (tmp.Count == 0)
                    return "";
                else
                    return tmp.Aggregate((alfa, beta) => alfa + " " + beta);
            }
            return null;
        }

        public void Dispose()
        {
            cbx.Dispose();
        }
    }

    public sealed class CheckedListBoxUITypeEditor_raceNames : System.Drawing.Design.UITypeEditor, IDisposable
    {
        public CheckedListBox cbx = new CheckedListBox();

        private IWindowsFormsEditorService es;

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable { get { return true; } }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            int i;

            //instantiate the custom property editor service provider
            es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (es != null)
            {
                //load the ListBox items from vesseldata and check them appropriately
                cbx.Items.Clear();
                cbx.Sorted = false; //dont sort while adding

                foreach (string item in VesselData.Current.RaceNames)
                {
                    cbx.Items.Add(item);
                    if (value != null)
                        if (((string)value).Split(' ').Contains(item))
                            cbx.SetItemChecked(cbx.Items.Count - 1, true);
                }

                cbx.Sorted = true;
                cbx.CheckOnClick = true;

                //show the control and wait for user input
                es.DropDownControl(cbx);

                //Apply values to a temp list and return it
                List<string> tmp = new List<string>();

                for (i = 0; i < cbx.Items.Count; i++)
                    if (cbx.GetItemChecked(i))
                        tmp.Add(cbx.Items[i].ToString());

                if (tmp.Count == 0)
                    return "";
                else
                    return tmp.Aggregate((alfa, beta) => alfa + " " + beta);
            }
            return null;
        }

        public void Dispose()
        {
            cbx.Dispose();
        }
    }

    public sealed class CheckedListBoxUITypeEditor_raceKeys : System.Drawing.Design.UITypeEditor, IDisposable
    {
        public CheckedListBox cbx = new CheckedListBox();

        private IWindowsFormsEditorService es;

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable { get { return true; } }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            int i;

            //instantiate the custom property editor service provider
            es = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (es != null)
            {
                //load the ListBox items from vesseldata and check them appropriately
                cbx.Items.Clear();
                cbx.Sorted = false; //dont sort while adding

                foreach (string item in VesselData.Current.RaceKeys)
                {
                    cbx.Items.Add(item);
                    if (value != null)
                        if (((string)value).Split(' ').Contains(item))
                            cbx.SetItemChecked(cbx.Items.Count - 1, true);
                }

                cbx.Sorted = true;
                cbx.CheckOnClick = true;

                //show the control and wait for user input
                es.DropDownControl(cbx);

                //Apply values to a temp list and return it
                List<string> tmp = new List<string>();

                for (i = 0; i < cbx.Items.Count; i++)
                    if (cbx.GetItemChecked(i))
                        tmp.Add(cbx.Items[i].ToString());

                if (tmp.Count == 0)
                    return "";
                else
                    return tmp.Aggregate((alfa, beta) => alfa + " " + beta);
            }
            return null;
        }

        public void Dispose()
        {
            cbx.Dispose();
        }
    }


}
