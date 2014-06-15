namespace ArtemisMissionEditor
{
    partial class _FormSettings
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
			this.label2 = new System.Windows.Forms.Label();
			this._FM_tb_VD = new System.Windows.Forms.TextBox();
			this._FM_b_OpenVesselData = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pgMisc = new System.Windows.Forms.PropertyGrid();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this._FM_tb_SN = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.panel3 = new System.Windows.Forms.Panel();
			this.pgColor = new System.Windows.Forms.PropertyGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this._FS_b_LoadSettings = new System.Windows.Forms.Button();
			this._FM_b_OK = new System.Windows.Forms.Button();
			this._FS_b_SaveSettings = new System.Windows.Forms.Button();
			this._FS_b_ResetDefaults = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(6, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(166, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "Default path to vesselData.xml";
			// 
			// _FM_tb_VD
			// 
			this._FM_tb_VD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FM_tb_VD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FM_tb_VD.Location = new System.Drawing.Point(9, 31);
			this._FM_tb_VD.Name = "_FM_tb_VD";
			this._FM_tb_VD.Size = new System.Drawing.Size(710, 23);
			this._FM_tb_VD.TabIndex = 5;
			this._FM_tb_VD.TextChanged += new System.EventHandler(this._E_FM_tb_VD_TextChanged);
			// 
			// _FM_b_OpenVesselData
			// 
			this._FM_b_OpenVesselData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._FM_b_OpenVesselData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FM_b_OpenVesselData.Location = new System.Drawing.Point(725, 31);
			this._FM_b_OpenVesselData.Name = "_FM_b_OpenVesselData";
			this._FM_b_OpenVesselData.Size = new System.Drawing.Size(31, 23);
			this._FM_b_OpenVesselData.TabIndex = 7;
			this._FM_b_OpenVesselData.Text = "...";
			this._FM_b_OpenVesselData.UseVisualStyleBackColor = true;
			this._FM_b_OpenVesselData.Click += new System.EventHandler(this._E_FM_b_OpenVesselData_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(770, 557);
			this.tabControl1.TabIndex = 10;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(762, 529);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Common misc.";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.pgMisc);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(756, 523);
			this.panel2.TabIndex = 2;
			// 
			// pgMisc
			// 
			this.pgMisc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgMisc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.pgMisc.Location = new System.Drawing.Point(0, 0);
			this.pgMisc.Name = "pgMisc";
			this.pgMisc.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.pgMisc.Size = new System.Drawing.Size(754, 521);
			this.pgMisc.TabIndex = 1;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this._FM_tb_SN);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this._FM_tb_VD);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this._FM_b_OpenVesselData);
			this.tabPage3.Location = new System.Drawing.Point(4, 24);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(762, 529);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Common strings";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// _FM_tb_SN
			// 
			this._FM_tb_SN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FM_tb_SN.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FM_tb_SN.Location = new System.Drawing.Point(9, 84);
			this._FM_tb_SN.Multiline = true;
			this._FM_tb_SN.Name = "_FM_tb_SN";
			this._FM_tb_SN.Size = new System.Drawing.Size(747, 439);
			this._FM_tb_SN.TabIndex = 9;
			this._FM_tb_SN.TextChanged += new System.EventHandler(this._E_FM_tb_SN_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(6, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(123, 15);
			this.label1.TabIndex = 8;
			this.label1.Text = "Default start node text";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.panel3);
			this.tabPage4.Location = new System.Drawing.Point(4, 24);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(762, 529);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Colors";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.pgColor);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(756, 523);
			this.panel3.TabIndex = 3;
			// 
			// pgColor
			// 
			this.pgColor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgColor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.pgColor.Location = new System.Drawing.Point(0, 0);
			this.pgColor.Name = "pgColor";
			this.pgColor.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.pgColor.Size = new System.Drawing.Size(754, 521);
			this.pgColor.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._FS_b_LoadSettings);
			this.panel1.Controls.Add(this._FM_b_OK);
			this.panel1.Controls.Add(this._FS_b_SaveSettings);
			this.panel1.Controls.Add(this._FS_b_ResetDefaults);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 566);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(770, 38);
			this.panel1.TabIndex = 11;
			// 
			// _FS_b_LoadSettings
			// 
			this._FS_b_LoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FS_b_LoadSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FS_b_LoadSettings.Image = global::ArtemisMissionEditor.Properties.Resources.action_save;
			this._FS_b_LoadSettings.Location = new System.Drawing.Point(448, 6);
			this._FS_b_LoadSettings.Name = "_FS_b_LoadSettings";
			this._FS_b_LoadSettings.Size = new System.Drawing.Size(113, 23);
			this._FS_b_LoadSettings.TabIndex = 11;
			this._FS_b_LoadSettings.Text = "Load from disk";
			this._FS_b_LoadSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._FS_b_LoadSettings.UseVisualStyleBackColor = true;
			this._FS_b_LoadSettings.Click += new System.EventHandler(this._E_FS_b_LoadSettings_Click);
			// 
			// _FM_b_OK
			// 
			this._FM_b_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FM_b_OK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FM_b_OK.Location = new System.Drawing.Point(686, 6);
			this._FM_b_OK.Name = "_FM_b_OK";
			this._FM_b_OK.Size = new System.Drawing.Size(75, 23);
			this._FM_b_OK.TabIndex = 10;
			this._FM_b_OK.Text = "OK";
			this._FM_b_OK.UseVisualStyleBackColor = true;
			this._FM_b_OK.Click += new System.EventHandler(this._E_FM_b_OK_Click);
			// 
			// _FS_b_SaveSettings
			// 
			this._FS_b_SaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FS_b_SaveSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FS_b_SaveSettings.Image = global::ArtemisMissionEditor.Properties.Resources.action_save;
			this._FS_b_SaveSettings.Location = new System.Drawing.Point(567, 6);
			this._FS_b_SaveSettings.Name = "_FS_b_SaveSettings";
			this._FS_b_SaveSettings.Size = new System.Drawing.Size(113, 23);
			this._FS_b_SaveSettings.TabIndex = 2;
			this._FS_b_SaveSettings.Text = "Save to disk";
			this._FS_b_SaveSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._FS_b_SaveSettings.UseVisualStyleBackColor = true;
			this._FS_b_SaveSettings.Click += new System.EventHandler(this._E_FS_b_SaveSettings_Click);
			// 
			// _FS_b_ResetDefaults
			// 
			this._FS_b_ResetDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._FS_b_ResetDefaults.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FS_b_ResetDefaults.Image = global::ArtemisMissionEditor.Properties.Resources.action_refresh_blue;
			this._FS_b_ResetDefaults.Location = new System.Drawing.Point(337, 6);
			this._FS_b_ResetDefaults.Name = "_FS_b_ResetDefaults";
			this._FS_b_ResetDefaults.Size = new System.Drawing.Size(105, 23);
			this._FS_b_ResetDefaults.TabIndex = 9;
			this._FS_b_ResetDefaults.Text = "Reset defaults";
			this._FS_b_ResetDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._FS_b_ResetDefaults.UseVisualStyleBackColor = true;
			this._FS_b_ResetDefaults.Click += new System.EventHandler(this._E_FS_b_ResetDefaults_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(776, 607);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// _FormSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(776, 607);
			this.Controls.Add(this.tableLayoutPanel1);
			this.KeyPreview = true;
			this.Name = "_FormSettings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FS_FormClosing);
			this.Load += new System.EventHandler(this._E_FL_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._FormSettings_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button _FS_b_SaveSettings;
		private System.Windows.Forms.TextBox _FM_tb_VD;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _FM_b_OpenVesselData;
		private System.Windows.Forms.Button _FS_b_ResetDefaults;
        private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.PropertyGrid pgMisc;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button _FM_b_OK;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox _FM_tb_SN;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button _FS_b_LoadSettings;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Panel panel3;
		public System.Windows.Forms.PropertyGrid pgColor;
    }
}