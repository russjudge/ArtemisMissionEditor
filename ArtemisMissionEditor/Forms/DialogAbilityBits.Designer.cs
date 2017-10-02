namespace ArtemisMissionEditor.Forms
{
	partial class DialogAbilityBits
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.cbITMS = new System.Windows.Forms.CheckBox();
            this.cbITLT = new System.Windows.Forms.CheckBox();
            this.cbC = new System.Windows.Forms.CheckBox();
            this.cbHET = new System.Windows.Forms.CheckBox();
            this.cbW = new System.Windows.Forms.CheckBox();
            this.cbT = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbTB = new System.Windows.Forms.CheckBox();
            this.cbDL = new System.Windows.Forms.CheckBox();
            this.cbAMB = new System.Windows.Forms.CheckBox();
            this.cbATB = new System.Windows.Forms.CheckBox();
            this.cbAS = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(70, 266);
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
            this.cancelButton.Location = new System.Drawing.Point(124, 266);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(57, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cbITMS
            // 
            this.cbITMS.AutoSize = true;
            this.cbITMS.Location = new System.Drawing.Point(12, 12);
            this.cbITMS.Name = "cbITMS";
            this.cbITMS.Size = new System.Drawing.Size(172, 17);
            this.cbITMS.TabIndex = 2;
            this.cbITMS.Text = "INVISIBLE TO MAIN SCREEN";
            this.cbITMS.UseVisualStyleBackColor = true;
            this.cbITMS.CheckedChanged += new System.EventHandler(this.cbITMS_CheckedChanged);
            // 
            // cbITLT
            // 
            this.cbITLT.AutoSize = true;
            this.cbITLT.Location = new System.Drawing.Point(12, 35);
            this.cbITLT.Name = "cbITLT";
            this.cbITLT.Size = new System.Drawing.Size(175, 17);
            this.cbITLT.TabIndex = 4;
            this.cbITLT.Text = "INVISIBLE TO LRS/TACTICAL";
            this.cbITLT.UseVisualStyleBackColor = true;
            // 
            // cbC
            // 
            this.cbC.AutoSize = true;
            this.cbC.Location = new System.Drawing.Point(12, 58);
            this.cbC.Name = "cbC";
            this.cbC.Size = new System.Drawing.Size(80, 17);
            this.cbC.TabIndex = 5;
            this.cbC.Text = "CLOAKING";
            this.cbC.UseVisualStyleBackColor = true;
            // 
            // cbHET
            // 
            this.cbHET.AutoSize = true;
            this.cbHET.Location = new System.Drawing.Point(12, 81);
            this.cbHET.Name = "cbHET";
            this.cbHET.Size = new System.Drawing.Size(135, 17);
            this.cbHET.TabIndex = 6;
            this.cbHET.Text = "HIGH ENERGY TURN";
            this.cbHET.UseVisualStyleBackColor = true;
            // 
            // cbW
            // 
            this.cbW.AutoSize = true;
            this.cbW.Location = new System.Drawing.Point(12, 104);
            this.cbW.Name = "cbW";
            this.cbW.Size = new System.Drawing.Size(59, 17);
            this.cbW.TabIndex = 7;
            this.cbW.Text = "WARP";
            this.cbW.UseVisualStyleBackColor = true;
            // 
            // cbT
            // 
            this.cbT.AutoSize = true;
            this.cbT.Location = new System.Drawing.Point(12, 127);
            this.cbT.Name = "cbT";
            this.cbT.Size = new System.Drawing.Size(83, 17);
            this.cbT.TabIndex = 8;
            this.cbT.Text = "TELEPORT";
            this.cbT.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(43, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 9;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbTB
            // 
            this.cbTB.AutoSize = true;
            this.cbTB.Location = new System.Drawing.Point(12, 150);
            this.cbTB.Name = "cbTB";
            this.cbTB.Size = new System.Drawing.Size(111, 17);
            this.cbTB.TabIndex = 10;
            this.cbTB.Text = "TRACTOR BEAM";
            this.cbTB.UseVisualStyleBackColor = true;
            // 
            // cbDL
            // 
            this.cbDL.AutoSize = true;
            this.cbDL.Location = new System.Drawing.Point(12, 173);
            this.cbDL.Name = "cbDL";
            this.cbDL.Size = new System.Drawing.Size(127, 17);
            this.cbDL.TabIndex = 11;
            this.cbDL.Text = "DRONE LAUNCHER";
            this.cbDL.UseVisualStyleBackColor = true;
            // 
            // cbAMB
            // 
            this.cbAMB.AutoSize = true;
            this.cbAMB.Location = new System.Drawing.Point(12, 196);
            this.cbAMB.Name = "cbAMB";
            this.cbAMB.Size = new System.Drawing.Size(121, 17);
            this.cbAMB.TabIndex = 12;
            this.cbAMB.Text = "ANTI-MINE BEAMS";
            this.cbAMB.UseVisualStyleBackColor = true;
            // 
            // cbATB
            // 
            this.cbATB.AutoSize = true;
            this.cbATB.Location = new System.Drawing.Point(12, 219);
            this.cbATB.Name = "cbATB";
            this.cbATB.Size = new System.Drawing.Size(124, 17);
            this.cbATB.TabIndex = 13;
            this.cbATB.Text = "ANTI-TORP BEAMS";
            this.cbATB.UseVisualStyleBackColor = true;
            // 
            // cbAS
            // 
            this.cbAS.AutoSize = true;
            this.cbAS.Location = new System.Drawing.Point(12, 242);
            this.cbAS.Name = "cbAS";
            this.cbAS.Size = new System.Drawing.Size(93, 17);
            this.cbAS.TabIndex = 14;
            this.cbAS.Text = "ANTI-SHIELD";
            this.cbAS.UseVisualStyleBackColor = true;
            // 
            // DialogAbilityBits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(192, 301);
            this.ControlBox = false;
            this.Controls.Add(this.cbAS);
            this.Controls.Add(this.cbATB);
            this.Controls.Add(this.cbAMB);
            this.Controls.Add(this.cbDL);
            this.Controls.Add(this.cbTB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbT);
            this.Controls.Add(this.cbW);
            this.Controls.Add(this.cbHET);
            this.Controls.Add(this.cbC);
            this.Controls.Add(this.cbITLT);
            this.Controls.Add(this.cbITMS);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogAbilityBits";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DialogConsoleList_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.CheckBox cbITMS;
        public System.Windows.Forms.CheckBox cbITLT;
        public System.Windows.Forms.CheckBox cbC;
        public System.Windows.Forms.CheckBox cbHET;
        public System.Windows.Forms.CheckBox cbW;
        public System.Windows.Forms.CheckBox cbT;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.CheckBox cbTB;
        public System.Windows.Forms.CheckBox cbDL;
        public System.Windows.Forms.CheckBox cbAMB;
        public System.Windows.Forms.CheckBox cbATB;
        public System.Windows.Forms.CheckBox cbAS;
	}
}