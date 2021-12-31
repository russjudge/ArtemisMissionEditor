namespace ArtemisMissionEditor.Forms
{
    partial class DialogSpaceMap
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
            ArtemisMissionEditor.SpaceMap.Space spaceMap1 = new ArtemisMissionEditor.SpaceMap.Space();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSpaceMap));
            this._DSM_sc_Main = new System.Windows.Forms.SplitContainer();
            this.pSpaceMap = new ArtemisMissionEditor._PanelSpaceMap();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._DSM_pg_Properties = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this._DSM_b_Cancel = new System.Windows.Forms.Button();
            this._DSM_b_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._DSM_sc_Main)).BeginInit();
            this._DSM_sc_Main.Panel1.SuspendLayout();
            this._DSM_sc_Main.Panel2.SuspendLayout();
            this._DSM_sc_Main.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _DSM_sc_Main
            // 
            this._DSM_sc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DSM_sc_Main.Location = new System.Drawing.Point(0, 0);
            this._DSM_sc_Main.Name = "_DSM_sc_Main";
            // 
            // _DSM_sc_Main.Panel1
            // 
            this._DSM_sc_Main.Panel1.Controls.Add(this.pSpaceMap);
            // 
            // _DSM_sc_Main.Panel2
            // 
            this._DSM_sc_Main.Panel2.Controls.Add(this.tableLayoutPanel1);
            this._DSM_sc_Main.Size = new System.Drawing.Size(812, 534);
            this._DSM_sc_Main.SplitterDistance = 581;
            this._DSM_sc_Main.TabIndex = 1;
            // 
            // pSpaceMap
            // 
            this.pSpaceMap.BackColor = System.Drawing.Color.DarkRed;
            this.pSpaceMap.ChangesPending = false;
            this.pSpaceMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSpaceMap.Location = new System.Drawing.Point(0, 0);
            this.pSpaceMap.Margin = new System.Windows.Forms.Padding(0);
            this.pSpaceMap.Name = "pSpaceMap";
            this.pSpaceMap.Padding = new System.Windows.Forms.Padding(3);
            this.pSpaceMap.ReadOnly = false;
            this.pSpaceMap.Size = new System.Drawing.Size(581, 534);
            spaceMap1.ChangesPending = false;
            spaceMap1.SelectionNameless = null;
            spaceMap1.SelectionSpecial = null;
            this.pSpaceMap.SpaceMap = spaceMap1;
            this.pSpaceMap.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._DSM_pg_Properties, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(227, 534);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _DSM_pg_Properties
            // 
            this._DSM_pg_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DSM_pg_Properties.Location = new System.Drawing.Point(3, 3);
            this._DSM_pg_Properties.Name = "_DSM_pg_Properties";
            this._DSM_pg_Properties.Size = new System.Drawing.Size(221, 484);
            this._DSM_pg_Properties.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._DSM_b_Cancel);
            this.panel1.Controls.Add(this._DSM_b_OK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 493);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 38);
            this.panel1.TabIndex = 1;
            // 
            // _DSM_b_Cancel
            // 
            this._DSM_b_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._DSM_b_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._DSM_b_Cancel.Location = new System.Drawing.Point(56, 6);
            this._DSM_b_Cancel.Name = "_DSM_b_Cancel";
            this._DSM_b_Cancel.Size = new System.Drawing.Size(75, 23);
            this._DSM_b_Cancel.TabIndex = 1;
            this._DSM_b_Cancel.Text = "Cancel";
            this._DSM_b_Cancel.UseVisualStyleBackColor = true;
            this._DSM_b_Cancel.Click += new System.EventHandler(this._DSM_b_Cancel_Click);
            // 
            // _DSM_b_OK
            // 
            this._DSM_b_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._DSM_b_OK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._DSM_b_OK.Location = new System.Drawing.Point(137, 6);
            this._DSM_b_OK.Name = "_DSM_b_OK";
            this._DSM_b_OK.Size = new System.Drawing.Size(75, 23);
            this._DSM_b_OK.TabIndex = 0;
            this._DSM_b_OK.Text = "OK";
            this._DSM_b_OK.UseVisualStyleBackColor = true;
            this._DSM_b_OK.Click += new System.EventHandler(this._DSM_b_OK_Click);
            // 
            // DialogSpaceMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 534);
            this.Controls.Add(this._DSM_sc_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "DialogSpaceMap";
            this.ShowInTaskbar = false;
            this.Text = "DialogSpaceMap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogSpaceMap_FormClosing);
            this.Load += new System.EventHandler(this.DialogSpaceMap_Load);
            this.VisibleChanged += new System.EventHandler(this.DialogSpaceMap_VisibleChanged);
            this._DSM_sc_Main.Panel1.ResumeLayout(false);
            this._DSM_sc_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._DSM_sc_Main)).EndInit();
            this._DSM_sc_Main.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private _PanelSpaceMap pSpaceMap;
        private System.Windows.Forms.SplitContainer _DSM_sc_Main;
        private System.Windows.Forms.PropertyGrid _DSM_pg_Properties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _DSM_b_OK;
        private System.Windows.Forms.Button _DSM_b_Cancel;
    }
}