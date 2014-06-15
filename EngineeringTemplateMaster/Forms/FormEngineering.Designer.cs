namespace EngineeringTemplateMaster
{
	partial class FormEngineering
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEngineering));
			this.settingsControl1 = new EngineeringTemplateMaster.SettingsControl();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.fileList = new EngineeringTemplateMaster.RefreshingListBox();
			this.ScrollBarDetector = new System.Windows.Forms.Panel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.storeAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.applyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.applyGoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.reloadListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.resetCurrentPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.applySelectedPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.applyStartArtemisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// settingsControl1
			// 
			this.settingsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.settingsControl1.CurrentSetting = null;
			this.settingsControl1.Location = new System.Drawing.Point(225, 3);
			this.settingsControl1.MinimumSize = new System.Drawing.Size(780, 700);
			this.settingsControl1.Name = "settingsControl1";
			this.settingsControl1.Size = new System.Drawing.Size(783, 700);
			this.settingsControl1.TabIndex = 2;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AutoScroll = true;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.settingsControl1);
			this.toolStripContainer1.ContentPanel.Controls.Add(this.ScrollBarDetector);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1008, 706);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(1008, 730);
			this.toolStripContainer1.TabIndex = 1;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(221, 700);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.fileList);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(213, 672);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Stored presets";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// fileList
			// 
			this.fileList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileList.IntegralHeight = false;
			this.fileList.ItemHeight = 15;
			this.fileList.Location = new System.Drawing.Point(3, 3);
			this.fileList.Name = "fileList";
			this.fileList.Size = new System.Drawing.Size(207, 666);
			this.fileList.TabIndex = 0;
			this.fileList.SelectedValueChanged += new System.EventHandler(this.fileList_SelectedValueChanged);
			// 
			// ScrollBarDetector
			// 
			this.ScrollBarDetector.BackColor = System.Drawing.Color.Transparent;
			this.ScrollBarDetector.Location = new System.Drawing.Point(1007, 705);
			this.ScrollBarDetector.Name = "ScrollBarDetector";
			this.ScrollBarDetector.Size = new System.Drawing.Size(1, 1);
			this.ScrollBarDetector.TabIndex = 3;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.reloadListToolStripMenuItem,
            this.toolStripMenuItem2,
            this.resetCurrentPresetToolStripMenuItem,
            this.toolStripMenuItem3,
            this.applySelectedPresetToolStripMenuItem,
            this.toolStripMenuItem4,
            this.applyStartArtemisToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.storeAsToolStripMenuItem,
            this.applyToolStripMenuItem,
            this.applyGoToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Enabled = false;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.openToolStripMenuItem.Text = "Open...";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Enabled = false;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = "";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			// 
			// saveAllToolStripMenuItem
			// 
			this.saveAllToolStripMenuItem.Enabled = false;
			this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
			this.saveAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+S";
			this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.saveAllToolStripMenuItem.Text = "Save All";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(229, 6);
			// 
			// storeAsToolStripMenuItem
			// 
			this.storeAsToolStripMenuItem.Name = "storeAsToolStripMenuItem";
			this.storeAsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Alt+S";
			this.storeAsToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.storeAsToolStripMenuItem.Text = "Store As...";
			this.storeAsToolStripMenuItem.Click += new System.EventHandler(this.storeAsToolStripMenuItem_Click);
			// 
			// applyToolStripMenuItem
			// 
			this.applyToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.icon_download;
			this.applyToolStripMenuItem.Name = "applyToolStripMenuItem";
			this.applyToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Enter";
			this.applyToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.applyToolStripMenuItem.Text = "Apply";
			this.applyToolStripMenuItem.Click += new System.EventHandler(this.applyToolStripMenuItem_Click);
			// 
			// applyGoToolStripMenuItem
			// 
			this.applyGoToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.icon_get_world;
			this.applyGoToolStripMenuItem.Name = "applyGoToolStripMenuItem";
			this.applyGoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+Enter";
			this.applyGoToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.applyGoToolStripMenuItem.Text = "Apply && Go!";
			this.applyGoToolStripMenuItem.Click += new System.EventHandler(this.applyGoToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Enabled = false;
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(22, 20);
			this.toolStripMenuItem1.Text = " ";
			// 
			// reloadListToolStripMenuItem
			// 
			this.reloadListToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.action_refresh_blue;
			this.reloadListToolStripMenuItem.Name = "reloadListToolStripMenuItem";
			this.reloadListToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
			this.reloadListToolStripMenuItem.Text = "Reload List";
			this.reloadListToolStripMenuItem.Click += new System.EventHandler(this.reloadListToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(22, 20);
			this.toolStripMenuItem2.Text = " ";
			// 
			// resetCurrentPresetToolStripMenuItem
			// 
			this.resetCurrentPresetToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.icon_history;
			this.resetCurrentPresetToolStripMenuItem.Name = "resetCurrentPresetToolStripMenuItem";
			this.resetCurrentPresetToolStripMenuItem.Size = new System.Drawing.Size(144, 20);
			this.resetCurrentPresetToolStripMenuItem.Text = "Reset selected preset";
			this.resetCurrentPresetToolStripMenuItem.Click += new System.EventHandler(this.resetCurrentPresetToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(22, 20);
			this.toolStripMenuItem3.Text = " ";
			// 
			// applySelectedPresetToolStripMenuItem
			// 
			this.applySelectedPresetToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.icon_download;
			this.applySelectedPresetToolStripMenuItem.Name = "applySelectedPresetToolStripMenuItem";
			this.applySelectedPresetToolStripMenuItem.Size = new System.Drawing.Size(147, 20);
			this.applySelectedPresetToolStripMenuItem.Text = "Apply selected preset";
			this.applySelectedPresetToolStripMenuItem.Click += new System.EventHandler(this.applySelectedPresetToolStripMenuItem_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(22, 20);
			this.toolStripMenuItem4.Text = " ";
			// 
			// applyStartArtemisToolStripMenuItem
			// 
			this.applyStartArtemisToolStripMenuItem.Image = global::EngineeringTemplateMaster.Properties.Resources.icon_get_world;
			this.applyStartArtemisToolStripMenuItem.Name = "applyStartArtemisToolStripMenuItem";
			this.applyStartArtemisToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
			this.applyStartArtemisToolStripMenuItem.Text = "Apply && Go!";
			this.applyStartArtemisToolStripMenuItem.Click += new System.EventHandler(this.applyStartArtemisToolStripMenuItem_Click);
			// 
			// FormEngineering
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 730);
			this.Controls.Add(this.toolStripContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormEngineering";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEngineering_KeyDown);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem storeAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem applyToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reloadListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.ToolStripMenuItem applyStartArtemisToolStripMenuItem;
		private SettingsControl settingsControl1;
		private System.Windows.Forms.Panel ScrollBarDetector;
		private System.Windows.Forms.ToolStripMenuItem resetCurrentPresetToolStripMenuItem;
		private RefreshingListBox fileList;
		private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem applySelectedPresetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem applyGoToolStripMenuItem;

	}
}

