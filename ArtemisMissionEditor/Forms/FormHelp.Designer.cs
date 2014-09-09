namespace ArtemisMissionEditor.Forms
{
	partial class FormHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHelp));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMiscellaneous = new System.Windows.Forms.TabPage();
            this.tabSpaceMapHotkeys = new System.Windows.Forms.TabPage();
            this.tabAnalysis = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.doubleBufferedPanel1 = new ArtemisMissionEditor.DoubleBufferedPanel();
            this.textBox1 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.doubleBufferedPanel2 = new ArtemisMissionEditor.DoubleBufferedPanel();
            this.textBox2 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.doubleBufferedPanel3 = new ArtemisMissionEditor.DoubleBufferedPanel();
            this.textBox3 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.tabControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.doubleBufferedPanel1.SuspendLayout();
            this.doubleBufferedPanel2.SuspendLayout();
            this.doubleBufferedPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabMiscellaneous);
            this.tabControl1.Controls.Add(this.tabSpaceMapHotkeys);
            this.tabControl1.Controls.Add(this.tabAnalysis);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(844, 22);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabMiscellaneous
            // 
            this.tabMiscellaneous.Location = new System.Drawing.Point(4, 22);
            this.tabMiscellaneous.Name = "tabMiscellaneous";
            this.tabMiscellaneous.Padding = new System.Windows.Forms.Padding(3);
            this.tabMiscellaneous.Size = new System.Drawing.Size(836, 0);
            this.tabMiscellaneous.TabIndex = 0;
            this.tabMiscellaneous.Text = "Miscellaneous info";
            this.tabMiscellaneous.UseVisualStyleBackColor = true;
            // 
            // tabSpaceMapHotkeys
            // 
            this.tabSpaceMapHotkeys.Location = new System.Drawing.Point(4, 22);
            this.tabSpaceMapHotkeys.Name = "tabSpaceMapHotkeys";
            this.tabSpaceMapHotkeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpaceMapHotkeys.Size = new System.Drawing.Size(836, 0);
            this.tabSpaceMapHotkeys.TabIndex = 1;
            this.tabSpaceMapHotkeys.Text = "Space Map Hotkeys";
            this.tabSpaceMapHotkeys.UseVisualStyleBackColor = true;
            // 
            // tabAnalysis
            // 
            this.tabAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabAnalysis.Name = "tabAnalysis";
            this.tabAnalysis.Size = new System.Drawing.Size(836, 0);
            this.tabAnalysis.TabIndex = 2;
            this.tabAnalysis.Text = "Script Analysis";
            this.tabAnalysis.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Location = new System.Drawing.Point(0, 544);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(844, 42);
            this.panel1.TabIndex = 12;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(760, 10);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this._FM_b_OK_Click);
            // 
            // doubleBufferedPanel1
            // 
            this.doubleBufferedPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doubleBufferedPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.doubleBufferedPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.doubleBufferedPanel1.Controls.Add(this.textBox1);
            this.doubleBufferedPanel1.Location = new System.Drawing.Point(0, 20);
            this.doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            this.doubleBufferedPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.doubleBufferedPanel1.Size = new System.Drawing.Size(843, 521);
            this.doubleBufferedPanel1.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(833, 511);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // doubleBufferedPanel2
            // 
            this.doubleBufferedPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doubleBufferedPanel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.doubleBufferedPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.doubleBufferedPanel2.Controls.Add(this.textBox2);
            this.doubleBufferedPanel2.Location = new System.Drawing.Point(0, 20);
            this.doubleBufferedPanel2.Name = "doubleBufferedPanel2";
            this.doubleBufferedPanel2.Padding = new System.Windows.Forms.Padding(4);
            this.doubleBufferedPanel2.Size = new System.Drawing.Size(843, 521);
            this.doubleBufferedPanel2.TabIndex = 14;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(4, 4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(833, 511);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // doubleBufferedPanel3
            // 
            this.doubleBufferedPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doubleBufferedPanel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.doubleBufferedPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.doubleBufferedPanel3.Controls.Add(this.textBox3);
            this.doubleBufferedPanel3.Location = new System.Drawing.Point(0, 20);
            this.doubleBufferedPanel3.Name = "doubleBufferedPanel3";
            this.doubleBufferedPanel3.Padding = new System.Windows.Forms.Padding(4);
            this.doubleBufferedPanel3.Size = new System.Drawing.Size(843, 521);
            this.doubleBufferedPanel3.TabIndex = 15;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(4, 4);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(833, 511);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // FormHelp
            // 
            this.ClientSize = new System.Drawing.Size(844, 586);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.doubleBufferedPanel1);
            this.Controls.Add(this.doubleBufferedPanel2);
            this.Controls.Add(this.doubleBufferedPanel3);
            this.KeyPreview = true;
            this.Name = "FormHelp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Help window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHelp_FormClosing);
            this.Load += new System.EventHandler(this.FormHelp_Load);
            this.VisibleChanged += new System.EventHandler(this.FormHelp_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._FormHelp_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.doubleBufferedPanel1.ResumeLayout(false);
            this.doubleBufferedPanel1.PerformLayout();
            this.doubleBufferedPanel2.ResumeLayout(false);
            this.doubleBufferedPanel2.PerformLayout();
            this.doubleBufferedPanel3.ResumeLayout(false);
            this.doubleBufferedPanel3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabMiscellaneous;
        private DoubleBufferedTextBox textBox1;
        private System.Windows.Forms.TabPage tabSpaceMapHotkeys;
        private DoubleBufferedTextBox textBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
        private DoubleBufferedPanel doubleBufferedPanel1;
        private DoubleBufferedPanel doubleBufferedPanel2;
        private System.Windows.Forms.TabPage tabAnalysis;
        private DoubleBufferedPanel doubleBufferedPanel3;
        private DoubleBufferedTextBox textBox3;
	}
}
