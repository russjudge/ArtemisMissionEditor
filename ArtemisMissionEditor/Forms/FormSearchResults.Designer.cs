namespace ArtemisMissionEditor.Forms
{
    partial class FormSearchResults
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._FSR_tsc_Main = new System.Windows.Forms.ToolStripContainer();
            this._FSR_p_Main = new System.Windows.Forms.Panel();
            this._FSR_lv_Main = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._FSR_ss_Main = new System.Windows.Forms.StatusStrip();
            this._FSR_ss_Main_l_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this._FSR_ss_Main_tsb_Update = new System.Windows.Forms.ToolStripSplitButton();
            this._FSR_ss_Main_tsb_Clear = new System.Windows.Forms.ToolStripSplitButton();
            this._FSR_tsc_Main.ContentPanel.SuspendLayout();
            this._FSR_tsc_Main.SuspendLayout();
            this._FSR_p_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._FSR_lv_Main)).BeginInit();
            this._FSR_ss_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // _FSR_tsc_Main
            // 
            // 
            // _FSR_tsc_Main.ContentPanel
            // 
            this._FSR_tsc_Main.ContentPanel.Controls.Add(this._FSR_p_Main);
            this._FSR_tsc_Main.ContentPanel.Size = new System.Drawing.Size(863, 183);
            this._FSR_tsc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._FSR_tsc_Main.Location = new System.Drawing.Point(0, 0);
            this._FSR_tsc_Main.Name = "_FSR_tsc_Main";
            this._FSR_tsc_Main.Size = new System.Drawing.Size(863, 208);
            this._FSR_tsc_Main.TabIndex = 2;
            this._FSR_tsc_Main.Text = "toolStripContainer1";
            // 
            // _FSR_p_Main
            // 
            this._FSR_p_Main.BackColor = System.Drawing.SystemColors.Window;
            this._FSR_p_Main.Controls.Add(this._FSR_lv_Main);
            this._FSR_p_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._FSR_p_Main.Location = new System.Drawing.Point(0, 0);
            this._FSR_p_Main.Name = "_FSR_p_Main";
            this._FSR_p_Main.Padding = new System.Windows.Forms.Padding(3);
            this._FSR_p_Main.Size = new System.Drawing.Size(863, 183);
            this._FSR_p_Main.TabIndex = 1;
            // 
            // _FSR_lv_Main
            // 
            this._FSR_lv_Main.AllowUserToAddRows = false;
            this._FSR_lv_Main.AllowUserToDeleteRows = false;
            this._FSR_lv_Main.AllowUserToResizeColumns = false;
            this._FSR_lv_Main.AllowUserToResizeRows = false;
            this._FSR_lv_Main.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this._FSR_lv_Main.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this._FSR_lv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._FSR_lv_Main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._FSR_lv_Main.DefaultCellStyle = dataGridViewCellStyle3;
            this._FSR_lv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._FSR_lv_Main.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._FSR_lv_Main.Location = new System.Drawing.Point(3, 3);
            this._FSR_lv_Main.MultiSelect = false;
            this._FSR_lv_Main.Name = "_FSR_lv_Main";
            this._FSR_lv_Main.ReadOnly = true;
            this._FSR_lv_Main.RowHeadersVisible = false;
            this._FSR_lv_Main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._FSR_lv_Main.Size = new System.Drawing.Size(857, 177);
            this._FSR_lv_Main.TabIndex = 1;
            this._FSR_lv_Main.DoubleClick += new System.EventHandler(this._E_FSR_lb_Main_DoubleClick);
            this._FSR_lv_Main.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FSR_lb_Main_KeyDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 19;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 19;
            // 
            // Column4
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column4.HeaderText = "Node Title";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column3.HeaderText = "Description";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // _FSR_ss_Main
            // 
            this._FSR_ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._FSR_ss_Main_l_Main,
            this._FSR_ss_Main_tsb_Update,
            this._FSR_ss_Main_tsb_Clear});
            this._FSR_ss_Main.Location = new System.Drawing.Point(0, 208);
            this._FSR_ss_Main.Name = "_FSR_ss_Main";
            this._FSR_ss_Main.Size = new System.Drawing.Size(863, 22);
            this._FSR_ss_Main.TabIndex = 1;
            this._FSR_ss_Main.Text = "statusStrip1";
            // 
            // _FSR_ss_Main_l_Main
            // 
            this._FSR_ss_Main_l_Main.Name = "_FSR_ss_Main_l_Main";
            this._FSR_ss_Main_l_Main.Size = new System.Drawing.Size(675, 17);
            this._FSR_ss_Main_l_Main.Spring = true;
            this._FSR_ss_Main_l_Main.Text = "Looking for \"Warsaw\" in [Xml attribute value], [Xml attribute name], [Mission nod" +
    "e name], [Commentary] (Matching case)";
            this._FSR_ss_Main_l_Main.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _FSR_ss_Main_tsb_Update
            // 
            this._FSR_ss_Main_tsb_Update.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._FSR_ss_Main_tsb_Update.DropDownButtonWidth = 0;
            this._FSR_ss_Main_tsb_Update.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._FSR_ss_Main_tsb_Update.Name = "_FSR_ss_Main_tsb_Update";
            this._FSR_ss_Main_tsb_Update.Size = new System.Drawing.Size(93, 20);
            this._FSR_ss_Main_tsb_Update.Text = "Update results";
            this._FSR_ss_Main_tsb_Update.ButtonClick += new System.EventHandler(this._FSR_ss_Main_tsb_Update_ButtonClick);
            // 
            // _FSR_ss_Main_tsb_Clear
            // 
            this._FSR_ss_Main_tsb_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._FSR_ss_Main_tsb_Clear.DropDownButtonWidth = 0;
            this._FSR_ss_Main_tsb_Clear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._FSR_ss_Main_tsb_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._FSR_ss_Main_tsb_Clear.Name = "_FSR_ss_Main_tsb_Clear";
            this._FSR_ss_Main_tsb_Clear.Size = new System.Drawing.Size(80, 20);
            this._FSR_ss_Main_tsb_Clear.Text = "Clear results";
            this._FSR_ss_Main_tsb_Clear.ButtonClick += new System.EventHandler(this._E_FSR_ss_Main_tsb_Clear_ButtonClick);
            // 
            // FormSearchResults
            // 
            this.ClientSize = new System.Drawing.Size(863, 230);
            this.Controls.Add(this._FSR_tsc_Main);
            this.Controls.Add(this._FSR_ss_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSearchResults";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FSR_FormClosing);
            this.Load += new System.EventHandler(this._E_FSR_Load);
            this.VisibleChanged += new System.EventHandler(this.FormSearchResults_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FSR_KeyDown);
            this._FSR_tsc_Main.ContentPanel.ResumeLayout(false);
            this._FSR_tsc_Main.ResumeLayout(false);
            this._FSR_tsc_Main.PerformLayout();
            this._FSR_p_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._FSR_lv_Main)).EndInit();
            this._FSR_ss_Main.ResumeLayout(false);
            this._FSR_ss_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip _FSR_ss_Main;
        private System.Windows.Forms.ToolStripStatusLabel _FSR_ss_Main_l_Main;
        private System.Windows.Forms.ToolStripContainer _FSR_tsc_Main;
		private System.Windows.Forms.ToolStripSplitButton _FSR_ss_Main_tsb_Clear;
        private System.Windows.Forms.Panel _FSR_p_Main;
        private System.Windows.Forms.DataGridView _FSR_lv_Main;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.ToolStripSplitButton _FSR_ss_Main_tsb_Update;
    }
}
