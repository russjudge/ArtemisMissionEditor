namespace ArtemisMissionEditor
{
    partial class _FormSearchResults
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
			this._FSR_tsc_Main = new System.Windows.Forms.ToolStripContainer();
			this._FSR_p_Main = new System.Windows.Forms.Panel();
			this._FSR_lb_Main = new System.Windows.Forms.ListBox();
			this._FSR_ss_Main = new System.Windows.Forms.StatusStrip();
			this._FSR_ss_Main_l_Main = new System.Windows.Forms.ToolStripStatusLabel();
			this._FSR_ss_Main_tsb_Clear = new System.Windows.Forms.ToolStripSplitButton();
			this._FSR_tsc_Main.ContentPanel.SuspendLayout();
			this._FSR_tsc_Main.SuspendLayout();
			this._FSR_p_Main.SuspendLayout();
			this._FSR_ss_Main.SuspendLayout();
			this.SuspendLayout();
			// 
			// _FSR_tsc_Main
			// 
			// 
			// _FSR_tsc_Main.ContentPanel
			// 
			this._FSR_tsc_Main.ContentPanel.Controls.Add(this._FSR_p_Main);
			this._FSR_tsc_Main.ContentPanel.Size = new System.Drawing.Size(783, 100);
			this._FSR_tsc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FSR_tsc_Main.Location = new System.Drawing.Point(0, 0);
			this._FSR_tsc_Main.Name = "_FSR_tsc_Main";
			this._FSR_tsc_Main.Size = new System.Drawing.Size(783, 125);
			this._FSR_tsc_Main.TabIndex = 2;
			this._FSR_tsc_Main.Text = "toolStripContainer1";
			// 
			// _FSR_p_Main
			// 
			this._FSR_p_Main.BackColor = System.Drawing.SystemColors.Window;
			this._FSR_p_Main.Controls.Add(this._FSR_lb_Main);
			this._FSR_p_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FSR_p_Main.Location = new System.Drawing.Point(0, 0);
			this._FSR_p_Main.Name = "_FSR_p_Main";
			this._FSR_p_Main.Padding = new System.Windows.Forms.Padding(3);
			this._FSR_p_Main.Size = new System.Drawing.Size(783, 100);
			this._FSR_p_Main.TabIndex = 1;
			// 
			// _FSR_lb_Main
			// 
			this._FSR_lb_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FSR_lb_Main.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._FSR_lb_Main.FormattingEnabled = true;
			this._FSR_lb_Main.IntegralHeight = false;
			this._FSR_lb_Main.ItemHeight = 14;
			this._FSR_lb_Main.Location = new System.Drawing.Point(3, 3);
			this._FSR_lb_Main.Name = "_FSR_lb_Main";
			this._FSR_lb_Main.ScrollAlwaysVisible = true;
			this._FSR_lb_Main.Size = new System.Drawing.Size(777, 94);
			this._FSR_lb_Main.TabIndex = 0;
			this._FSR_lb_Main.DoubleClick += new System.EventHandler(this._E_FSR_lb_Main_DoubleClick);
			this._FSR_lb_Main.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FSR_lb_Main_KeyDown);
			// 
			// _FSR_ss_Main
			// 
			this._FSR_ss_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._FSR_ss_Main_l_Main,
            this._FSR_ss_Main_tsb_Clear});
			this._FSR_ss_Main.Location = new System.Drawing.Point(0, 125);
			this._FSR_ss_Main.Name = "_FSR_ss_Main";
			this._FSR_ss_Main.Size = new System.Drawing.Size(783, 22);
			this._FSR_ss_Main.TabIndex = 1;
			this._FSR_ss_Main.Text = "statusStrip1";
			// 
			// _FSR_ss_Main_l_Main
			// 
			this._FSR_ss_Main_l_Main.Name = "_FSR_ss_Main_l_Main";
			this._FSR_ss_Main_l_Main.Size = new System.Drawing.Size(679, 17);
			this._FSR_ss_Main_l_Main.Spring = true;
			this._FSR_ss_Main_l_Main.Text = "Looking for \"Warsaw\" in [Xml attribute value], [Xml attribute name], [Mission nod" +
    "e name], [Commentary] (Matching case)";
			this._FSR_ss_Main_l_Main.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _FSR_ss_Main_tsb_Clear
			// 
			this._FSR_ss_Main_tsb_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._FSR_ss_Main_tsb_Clear.DropDownButtonWidth = 0;
			this._FSR_ss_Main_tsb_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._FSR_ss_Main_tsb_Clear.Name = "_FSR_ss_Main_tsb_Clear";
			this._FSR_ss_Main_tsb_Clear.Size = new System.Drawing.Size(89, 20);
			this._FSR_ss_Main_tsb_Clear.Text = "Clear result list";
			this._FSR_ss_Main_tsb_Clear.ButtonClick += new System.EventHandler(this._E_FSR_ss_Main_tsb_Clear_ButtonClick);
			// 
			// _FormSearchResults
			// 
			this.ClientSize = new System.Drawing.Size(783, 147);
			this.Controls.Add(this._FSR_tsc_Main);
			this.Controls.Add(this._FSR_ss_Main);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "_FormSearchResults";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Find Results";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FSR_FormClosing);
			this.Load += new System.EventHandler(this._E_FSR_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FSR_KeyDown);
			this._FSR_tsc_Main.ContentPanel.ResumeLayout(false);
			this._FSR_tsc_Main.ResumeLayout(false);
			this._FSR_tsc_Main.PerformLayout();
			this._FSR_p_Main.ResumeLayout(false);
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
		private System.Windows.Forms.ListBox _FSR_lb_Main;
    }
}
