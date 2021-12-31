namespace ArtemisMissionEditor.Forms
{
	partial class DialogRHKeys
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
            this.components = new System.ComponentModel.Container();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._DRHK_l_1 = new System.Windows.Forms.Label();
            this._DRHK_l_2 = new System.Windows.Forms.Label();
            this._DRHK_lb_1 = new System.Windows.Forms.CheckedListBox();
            this._DRHK_lb_2 = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tbUnrecognised = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._FSM_p_7 = new System.Windows.Forms.Panel();
            this.lbPossibleRaces = new System.Windows.Forms.ListBox();
            this._FSM_l_7 = new System.Windows.Forms.Label();
            this._FSM_p_8 = new System.Windows.Forms.Panel();
            this.lbPossibleVessels = new System.Windows.Forms.ListBox();
            this._FSM_l_8 = new System.Windows.Forms.Label();
            this.timerValidate = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this._FSM_p_7.SuspendLayout();
            this._FSM_p_8.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(507, 641);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(53, 25);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.ckButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cancelButton.Location = new System.Drawing.Point(561, 641);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(57, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(426, 641);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 9;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._DRHK_l_1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._DRHK_l_2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._DRHK_lb_1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._DRHK_lb_2, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 595);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // _DRHK_l_1
            // 
            this._DRHK_l_1.AutoSize = true;
            this._DRHK_l_1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._DRHK_l_1.Location = new System.Drawing.Point(3, 0);
            this._DRHK_l_1.Name = "_DRHK_l_1";
            this._DRHK_l_1.Size = new System.Drawing.Size(16, 15);
            this._DRHK_l_1.TabIndex = 0;
            this._DRHK_l_1.Text = "l1";
            // 
            // _DRHK_l_2
            // 
            this._DRHK_l_2.AutoSize = true;
            this._DRHK_l_2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._DRHK_l_2.Location = new System.Drawing.Point(148, 0);
            this._DRHK_l_2.Name = "_DRHK_l_2";
            this._DRHK_l_2.Size = new System.Drawing.Size(16, 15);
            this._DRHK_l_2.TabIndex = 1;
            this._DRHK_l_2.Text = "l2";
            // 
            // _DRHK_lb_1
            // 
            this._DRHK_lb_1.CheckOnClick = true;
            this._DRHK_lb_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DRHK_lb_1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._DRHK_lb_1.FormattingEnabled = true;
            this._DRHK_lb_1.IntegralHeight = false;
            this._DRHK_lb_1.Location = new System.Drawing.Point(3, 23);
            this._DRHK_lb_1.Name = "_DRHK_lb_1";
            this._DRHK_lb_1.Size = new System.Drawing.Size(139, 569);
            this._DRHK_lb_1.TabIndex = 2;
            this._DRHK_lb_1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._DRHK_lb_1_ItemCheck);
            // 
            // _DRHK_lb_2
            // 
            this._DRHK_lb_2.CheckOnClick = true;
            this._DRHK_lb_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._DRHK_lb_2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._DRHK_lb_2.FormattingEnabled = true;
            this._DRHK_lb_2.IntegralHeight = false;
            this._DRHK_lb_2.Location = new System.Drawing.Point(148, 23);
            this._DRHK_lb_2.Name = "_DRHK_lb_2";
            this._DRHK_lb_2.Size = new System.Drawing.Size(139, 569);
            this._DRHK_lb_2.TabIndex = 3;
            this._DRHK_lb_2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._DRHK_lb_2_ItemCheck);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.AutoSize = true;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(453, 641);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(53, 25);
            this.button2.TabIndex = 11;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbUnrecognised
            // 
            this.tbUnrecognised.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUnrecognised.Location = new System.Drawing.Point(111, 614);
            this.tbUnrecognised.Name = "tbUnrecognised";
            this.tbUnrecognised.Size = new System.Drawing.Size(506, 20);
            this.tbUnrecognised.TabIndex = 12;
            this.tbUnrecognised.TextChanged += new System.EventHandler(this.tbUnrecognised_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 617);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Unrecognised keys:";
            // 
            // _FSM_p_7
            // 
            this._FSM_p_7.Controls.Add(this.lbPossibleRaces);
            this._FSM_p_7.Controls.Add(this._FSM_l_7);
            this._FSM_p_7.Location = new System.Drawing.Point(308, 12);
            this._FSM_p_7.Name = "_FSM_p_7";
            this._FSM_p_7.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this._FSM_p_7.Size = new System.Drawing.Size(309, 202);
            this._FSM_p_7.TabIndex = 14;
            // 
            // lbPossibleRaces
            // 
            this.lbPossibleRaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPossibleRaces.FormattingEnabled = true;
            this.lbPossibleRaces.IntegralHeight = false;
            this.lbPossibleRaces.Location = new System.Drawing.Point(3, 17);
            this.lbPossibleRaces.Name = "lbPossibleRaces";
            this.lbPossibleRaces.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbPossibleRaces.Size = new System.Drawing.Size(303, 185);
            this.lbPossibleRaces.TabIndex = 2;
            this.lbPossibleRaces.Tag = "PossibleRaces";
            // 
            // _FSM_l_7
            // 
            this._FSM_l_7.AutoSize = true;
            this._FSM_l_7.Dock = System.Windows.Forms.DockStyle.Top;
            this._FSM_l_7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._FSM_l_7.Location = new System.Drawing.Point(3, 0);
            this._FSM_l_7.Margin = new System.Windows.Forms.Padding(0);
            this._FSM_l_7.Name = "_FSM_l_7";
            this._FSM_l_7.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this._FSM_l_7.Size = new System.Drawing.Size(86, 17);
            this._FSM_l_7.TabIndex = 1;
            this._FSM_l_7.Text = "Possible Races";
            // 
            // _FSM_p_8
            // 
            this._FSM_p_8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._FSM_p_8.Controls.Add(this.lbPossibleVessels);
            this._FSM_p_8.Controls.Add(this._FSM_l_8);
            this._FSM_p_8.Location = new System.Drawing.Point(308, 220);
            this._FSM_p_8.Name = "_FSM_p_8";
            this._FSM_p_8.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this._FSM_p_8.Size = new System.Drawing.Size(309, 387);
            this._FSM_p_8.TabIndex = 15;
            // 
            // lbPossibleVessels
            // 
            this.lbPossibleVessels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPossibleVessels.FormattingEnabled = true;
            this.lbPossibleVessels.IntegralHeight = false;
            this.lbPossibleVessels.Location = new System.Drawing.Point(3, 17);
            this.lbPossibleVessels.Name = "lbPossibleVessels";
            this.lbPossibleVessels.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbPossibleVessels.Size = new System.Drawing.Size(303, 367);
            this.lbPossibleVessels.TabIndex = 3;
            this.lbPossibleVessels.Tag = "PossibleVessels";
            // 
            // _FSM_l_8
            // 
            this._FSM_l_8.AutoSize = true;
            this._FSM_l_8.Dock = System.Windows.Forms.DockStyle.Top;
            this._FSM_l_8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._FSM_l_8.Location = new System.Drawing.Point(3, 0);
            this._FSM_l_8.Margin = new System.Windows.Forms.Padding(0);
            this._FSM_l_8.Name = "_FSM_l_8";
            this._FSM_l_8.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this._FSM_l_8.Size = new System.Drawing.Size(94, 17);
            this._FSM_l_8.TabIndex = 2;
            this._FSM_l_8.Text = "Possible Vessels";
            // 
            // timerValidate
            // 
            this.timerValidate.Interval = 250;
            this.timerValidate.Tick += new System.EventHandler(this.timerValidate_Tick);
            // 
            // DialogRHKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(629, 676);
            this.ControlBox = false;
            this.Controls.Add(this._FSM_p_8);
            this.Controls.Add(this._FSM_p_7);
            this.Controls.Add(this.tbUnrecognised);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogRHKeys";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.Load += new System.EventHandler(this.DialogRHKeys_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._FSM_p_7.ResumeLayout(false);
            this._FSM_p_7.PerformLayout();
            this._FSM_p_8.ResumeLayout(false);
            this._FSM_p_8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _DRHK_l_1;
		private System.Windows.Forms.Label _DRHK_l_2;
		private System.Windows.Forms.CheckedListBox _DRHK_lb_1;
		private System.Windows.Forms.CheckedListBox _DRHK_lb_2;
		private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbUnrecognised;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _FSM_p_7;
        private System.Windows.Forms.ListBox lbPossibleRaces;
        private System.Windows.Forms.Label _FSM_l_7;
        private System.Windows.Forms.Panel _FSM_p_8;
        private System.Windows.Forms.ListBox lbPossibleVessels;
        private System.Windows.Forms.Label _FSM_l_8;
        private System.Windows.Forms.Timer timerValidate;
	}
}