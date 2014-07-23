namespace ArtemisMissionEditor
{
    partial class FormDependency
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
			this._FD_tlp_Main = new System.Windows.Forms.TableLayoutPanel();
			this._FD_tc_Main = new System.Windows.Forms.TabControl();
			this._FD_tc_Main_tp_1 = new System.Windows.Forms.TabPage();
			this._FD_lb_Left = new System.Windows.Forms.ListBox();
			this._FD_tc_Main_tp_2 = new System.Windows.Forms.TabPage();
			this._FD_lb_Right = new System.Windows.Forms.ListBox();
			this._FD_b_Event = new System.Windows.Forms.Button();
			this._FD_tlp_Main.SuspendLayout();
			this._FD_tc_Main.SuspendLayout();
			this._FD_tc_Main_tp_1.SuspendLayout();
			this._FD_tc_Main_tp_2.SuspendLayout();
			this.SuspendLayout();
			// 
			// _FD_tlp_Main
			// 
			this._FD_tlp_Main.ColumnCount = 1;
			this._FD_tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._FD_tlp_Main.Controls.Add(this._FD_tc_Main, 0, 1);
			this._FD_tlp_Main.Controls.Add(this._FD_b_Event, 0, 0);
			this._FD_tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FD_tlp_Main.Location = new System.Drawing.Point(0, 0);
			this._FD_tlp_Main.Name = "_FD_tlp_Main";
			this._FD_tlp_Main.RowCount = 2;
			this._FD_tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this._FD_tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._FD_tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._FD_tlp_Main.Size = new System.Drawing.Size(832, 270);
			this._FD_tlp_Main.TabIndex = 0;
			// 
			// _FD_tc_Main
			// 
			this._FD_tc_Main.Controls.Add(this._FD_tc_Main_tp_1);
			this._FD_tc_Main.Controls.Add(this._FD_tc_Main_tp_2);
			this._FD_tc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FD_tc_Main.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._FD_tc_Main.Location = new System.Drawing.Point(3, 53);
			this._FD_tc_Main.Name = "_FD_tc_Main";
			this._FD_tc_Main.SelectedIndex = 0;
			this._FD_tc_Main.Size = new System.Drawing.Size(826, 214);
			this._FD_tc_Main.TabIndex = 1;
			// 
			// _FD_tc_Main_tp_1
			// 
			this._FD_tc_Main_tp_1.Controls.Add(this._FD_lb_Left);
			this._FD_tc_Main_tp_1.Location = new System.Drawing.Point(4, 24);
			this._FD_tc_Main_tp_1.Name = "_FD_tc_Main_tp_1";
			this._FD_tc_Main_tp_1.Padding = new System.Windows.Forms.Padding(3);
			this._FD_tc_Main_tp_1.Size = new System.Drawing.Size(818, 186);
			this._FD_tc_Main_tp_1.TabIndex = 0;
			this._FD_tc_Main_tp_1.Text = "Preceding";
			this._FD_tc_Main_tp_1.UseVisualStyleBackColor = true;
			// 
			// _FD_lb_Left
			// 
			this._FD_lb_Left.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FD_lb_Left.Font = new System.Drawing.Font("Consolas", 9F);
			this._FD_lb_Left.FormattingEnabled = true;
			this._FD_lb_Left.IntegralHeight = false;
			this._FD_lb_Left.ItemHeight = 14;
			this._FD_lb_Left.Location = new System.Drawing.Point(3, 3);
			this._FD_lb_Left.Name = "_FD_lb_Left";
			this._FD_lb_Left.ScrollAlwaysVisible = true;
			this._FD_lb_Left.Size = new System.Drawing.Size(812, 180);
			this._FD_lb_Left.TabIndex = 1;
			this._FD_lb_Left.Click += new System.EventHandler(this._E_FD_lb_Left_Click);
			this._FD_lb_Left.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FD_lb_Left_KeyDown);
			// 
			// _FD_tc_Main_tp_2
			// 
			this._FD_tc_Main_tp_2.Controls.Add(this._FD_lb_Right);
			this._FD_tc_Main_tp_2.Location = new System.Drawing.Point(4, 24);
			this._FD_tc_Main_tp_2.Name = "_FD_tc_Main_tp_2";
			this._FD_tc_Main_tp_2.Padding = new System.Windows.Forms.Padding(3);
			this._FD_tc_Main_tp_2.Size = new System.Drawing.Size(818, 186);
			this._FD_tc_Main_tp_2.TabIndex = 1;
			this._FD_tc_Main_tp_2.Text = "Succeeding";
			this._FD_tc_Main_tp_2.UseVisualStyleBackColor = true;
			// 
			// _FD_lb_Right
			// 
			this._FD_lb_Right.Dock = System.Windows.Forms.DockStyle.Fill;
			this._FD_lb_Right.Font = new System.Drawing.Font("Consolas", 9F);
			this._FD_lb_Right.FormattingEnabled = true;
			this._FD_lb_Right.IntegralHeight = false;
			this._FD_lb_Right.ItemHeight = 14;
			this._FD_lb_Right.Location = new System.Drawing.Point(3, 3);
			this._FD_lb_Right.Name = "_FD_lb_Right";
			this._FD_lb_Right.ScrollAlwaysVisible = true;
			this._FD_lb_Right.Size = new System.Drawing.Size(812, 180);
			this._FD_lb_Right.TabIndex = 0;
			this._FD_lb_Right.Click += new System.EventHandler(this._E_FD_lb_Right_Click);
			this._FD_lb_Right.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FD_lb_Right_KeyDown);
			// 
			// _FD_b_Event
			// 
			this._FD_b_Event.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FD_b_Event.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._FD_b_Event.Location = new System.Drawing.Point(6, 6);
			this._FD_b_Event.Margin = new System.Windows.Forms.Padding(6);
			this._FD_b_Event.Name = "_FD_b_Event";
			this._FD_b_Event.Size = new System.Drawing.Size(820, 38);
			this._FD_b_Event.TabIndex = 0;
			this._FD_b_Event.UseVisualStyleBackColor = true;
			this._FD_b_Event.Click += new System.EventHandler(this._E_FD_b_Event_Click);
			// 
			// _FormDependency
			// 
			this.ClientSize = new System.Drawing.Size(832, 270);
			this.Controls.Add(this._FD_tlp_Main);
			this.HelpButton = true;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "_FormDependency";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Dependencies";
			this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this._FormDependency_HelpButtonClicked);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FD_FormClosing);
			this.Load += new System.EventHandler(this._E_FD_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._E_FD_KeyDown);
			this._FD_tlp_Main.ResumeLayout(false);
			this._FD_tc_Main.ResumeLayout(false);
			this._FD_tc_Main_tp_1.ResumeLayout(false);
			this._FD_tc_Main_tp_2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _FD_b_Event;
		private System.Windows.Forms.TableLayoutPanel _FD_tlp_Main;
        private System.Windows.Forms.TabControl _FD_tc_Main;
        private System.Windows.Forms.TabPage _FD_tc_Main_tp_1;
		private System.Windows.Forms.ListBox _FD_lb_Left;
		private System.Windows.Forms.ListBox _FD_lb_Right;
		private System.Windows.Forms.TabPage _FD_tc_Main_tp_2;

    }
}
