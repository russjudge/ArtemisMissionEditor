namespace ArtemisMissionEditor
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
            this.textBox1 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.tabSpaceMapHotkeys = new System.Windows.Forms.TabPage();
            this.textBox2 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabMiscellaneous.SuspendLayout();
            this.tabSpaceMapHotkeys.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabMiscellaneous);
            this.tabControl1.Controls.Add(this.tabSpaceMapHotkeys);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(844, 543);
            this.tabControl1.TabIndex = 0;
            // 
            // tabMiscellaneous
            // 
            this.tabMiscellaneous.Controls.Add(this.textBox1);
            this.tabMiscellaneous.Location = new System.Drawing.Point(4, 22);
            this.tabMiscellaneous.Name = "tabMiscellaneous";
            this.tabMiscellaneous.Padding = new System.Windows.Forms.Padding(3);
            this.tabMiscellaneous.Size = new System.Drawing.Size(836, 517);
            this.tabMiscellaneous.TabIndex = 0;
            this.tabMiscellaneous.Text = "Miscellaneous info";
            this.tabMiscellaneous.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(830, 511);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // tabSpaceMapHotkeys
            // 
            this.tabSpaceMapHotkeys.Controls.Add(this.textBox2);
            this.tabSpaceMapHotkeys.Location = new System.Drawing.Point(4, 22);
            this.tabSpaceMapHotkeys.Name = "tabSpaceMapHotkeys";
            this.tabSpaceMapHotkeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpaceMapHotkeys.Size = new System.Drawing.Size(836, 517);
            this.tabSpaceMapHotkeys.TabIndex = 1;
            this.tabSpaceMapHotkeys.Text = "Space Map Hotkeys";
            this.tabSpaceMapHotkeys.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(3, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(830, 511);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = resources.GetString("textBox2.Text");
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
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(760, 10);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this._FM_b_OK_Click);
            // 
            // FormHelp
            // 
            this.ClientSize = new System.Drawing.Size(844, 586);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "FormHelp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Help window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHelp_FormClosing);
            this.Load += new System.EventHandler(this.FormHelp_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._FormHelp_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabMiscellaneous.ResumeLayout(false);
            this.tabMiscellaneous.PerformLayout();
            this.tabSpaceMapHotkeys.ResumeLayout(false);
            this.tabSpaceMapHotkeys.PerformLayout();
            this.panel1.ResumeLayout(false);
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
	}
}
