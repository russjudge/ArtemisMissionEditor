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
            this.cbStealth = new System.Windows.Forms.CheckBox();
            this.cbLowVis = new System.Windows.Forms.CheckBox();
            this.cbCloak = new System.Windows.Forms.CheckBox();
            this.cbHET = new System.Windows.Forms.CheckBox();
            this.cbWarp = new System.Windows.Forms.CheckBox();
            this.cbTeleport = new System.Windows.Forms.CheckBox();
            this.cbTractor = new System.Windows.Forms.CheckBox();
            this.cbDrones = new System.Windows.Forms.CheckBox();
            this.cbAntiMine = new System.Windows.Forms.CheckBox();
            this.cbAntiTorp = new System.Windows.Forms.CheckBox();
            this.cbShldDrain = new System.Windows.Forms.CheckBox();
            this.cbShldVamp = new System.Windows.Forms.CheckBox();
            this.cbTeleBack = new System.Windows.Forms.CheckBox();
            this.cbShldReset = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // okButton
            //
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(235, 369);
            this.okButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(71, 31);
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
            this.cancelButton.Location = new System.Drawing.Point(307, 369);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(76, 31);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            // cbStealth
            //
            this.cbStealth.AutoSize = true;
            this.cbStealth.Location = new System.Drawing.Point(16, 15);
            this.cbStealth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbStealth.Name = "cbStealth";
            this.cbStealth.Size = new System.Drawing.Size(235, 21);
            this.cbStealth.TabIndex = 2;
            this.cbStealth.Text = "Stealth (invisible to main screen)";
            this.cbStealth.UseVisualStyleBackColor = true;
            //
            // cbLowVis
            //
            this.cbLowVis.AutoSize = true;
            this.cbLowVis.Location = new System.Drawing.Point(16, 43);
            this.cbLowVis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLowVis.Name = "cbLowVis";
            this.cbLowVis.Size = new System.Drawing.Size(269, 21);
            this.cbLowVis.TabIndex = 4;
            this.cbLowVis.Text = "LowVis (invisible to LRS/Tactical map)";
            this.cbLowVis.UseVisualStyleBackColor = true;
            //
            // cbCloak
            //
            this.cbCloak.AutoSize = true;
            this.cbCloak.Location = new System.Drawing.Point(16, 71);
            this.cbCloak.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCloak.Name = "cbCloak";
            this.cbCloak.Size = new System.Drawing.Size(65, 21);
            this.cbCloak.TabIndex = 5;
            this.cbCloak.Text = "Cloak";
            this.cbCloak.UseVisualStyleBackColor = true;
            //
            // cbHET
            //
            this.cbHET.AutoSize = true;
            this.cbHET.Location = new System.Drawing.Point(16, 99);
            this.cbHET.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbHET.Name = "cbHET";
            this.cbHET.Size = new System.Drawing.Size(184, 21);
            this.cbHET.TabIndex = 6;
            this.cbHET.Text = "HET (High Energy Turn)";
            this.cbHET.UseVisualStyleBackColor = true;
            //
            // cbWarp
            //
            this.cbWarp.AutoSize = true;
            this.cbWarp.Location = new System.Drawing.Point(16, 127);
            this.cbWarp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbWarp.Name = "cbWarp";
            this.cbWarp.Size = new System.Drawing.Size(64, 21);
            this.cbWarp.TabIndex = 7;
            this.cbWarp.Text = "Warp";
            this.cbWarp.UseVisualStyleBackColor = true;
            //
            // cbTeleport
            //
            this.cbTeleport.AutoSize = true;
            this.cbTeleport.Location = new System.Drawing.Point(16, 155);
            this.cbTeleport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTeleport.Name = "cbTeleport";
            this.cbTeleport.Size = new System.Drawing.Size(83, 21);
            this.cbTeleport.TabIndex = 8;
            this.cbTeleport.Text = "Teleport";
            this.cbTeleport.UseVisualStyleBackColor = true;
            //
            // cbTractor
            //
            this.cbTractor.AutoSize = true;
            this.cbTractor.Location = new System.Drawing.Point(16, 183);
            this.cbTractor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTractor.Name = "cbTractor";
            this.cbTractor.Size = new System.Drawing.Size(170, 21);
            this.cbTractor.TabIndex = 9;
            this.cbTractor.Text = "Tractor (tractor beam)";
            this.cbTractor.UseVisualStyleBackColor = true;
            //
            // cbDrones
            //
            this.cbDrones.AutoSize = true;
            this.cbDrones.Location = new System.Drawing.Point(16, 211);
            this.cbDrones.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDrones.Name = "cbDrones";
            this.cbDrones.Size = new System.Drawing.Size(186, 21);
            this.cbDrones.TabIndex = 10;
            this.cbDrones.Text = "Drones (drone launcher)";
            this.cbDrones.UseVisualStyleBackColor = true;
            //
            // cbAntiMine
            //
            this.cbAntiMine.AutoSize = true;
            this.cbAntiMine.Location = new System.Drawing.Point(16, 239);
            this.cbAntiMine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAntiMine.Name = "cbAntiMine";
            this.cbAntiMine.Size = new System.Drawing.Size(202, 21);
            this.cbAntiMine.TabIndex = 11;
            this.cbAntiMine.Text = "AntiMine (anti-mine beams)";
            this.cbAntiMine.UseVisualStyleBackColor = true;
            //
            // cbAntiTorp
            //
            this.cbAntiTorp.AutoSize = true;
            this.cbAntiTorp.Location = new System.Drawing.Point(16, 267);
            this.cbAntiTorp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAntiTorp.Name = "cbAntiTorp";
            this.cbAntiTorp.Size = new System.Drawing.Size(197, 21);
            this.cbAntiTorp.TabIndex = 12;
            this.cbAntiTorp.Text = "AntiTorp (anti-torp beams)";
            this.cbAntiTorp.UseVisualStyleBackColor = true;
            //
            // cbShldDrain
            //
            this.cbShldDrain.AutoSize = true;
            this.cbShldDrain.Location = new System.Drawing.Point(16, 295);
            this.cbShldDrain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShldDrain.Name = "cbShldDrain";
            this.cbShldDrain.Size = new System.Drawing.Size(171, 21);
            this.cbShldDrain.TabIndex = 13;
            this.cbShldDrain.Text = "ShldDrain (anti-shield)";
            this.cbShldDrain.UseVisualStyleBackColor = true;
            //
            // cbShldVamp
            //
            this.cbShldVamp.AutoSize = true;
            this.cbShldVamp.Location = new System.Drawing.Point(16, 323);
            this.cbShldVamp.Margin = new System.Windows.Forms.Padding(4);
            this.cbShldVamp.Name = "cbShldVamp";
            this.cbShldVamp.Size = new System.Drawing.Size(94, 21);
            this.cbShldVamp.TabIndex = 14;
            this.cbShldVamp.Text = "ShldVamp";
            this.cbShldVamp.UseVisualStyleBackColor = true;
            //
            // cbTeleBack
            //
            this.cbTeleBack.AutoSize = true;
            this.cbTeleBack.Location = new System.Drawing.Point(16, 351);
            this.cbTeleBack.Margin = new System.Windows.Forms.Padding(4);
            this.cbTeleBack.Name = "cbTeleBack";
            this.cbTeleBack.Size = new System.Drawing.Size(94, 21);
            this.cbTeleBack.TabIndex = 15;
            this.cbTeleBack.Text = "TeleBack";
            this.cbTeleBack.UseVisualStyleBackColor = true;
            //
            // cbShldReset
            //
            this.cbShldReset.AutoSize = true;
            this.cbShldReset.Location = new System.Drawing.Point(16, 379);
            this.cbShldReset.Margin = new System.Windows.Forms.Padding(4);
            this.cbShldReset.Name = "cbShldReset";
            this.cbShldReset.Size = new System.Drawing.Size(171, 21);
            this.cbShldReset.TabIndex = 16;
            this.cbShldReset.Text = "ShldReset";
            this.cbShldReset.UseVisualStyleBackColor = true;
            //
            // button1
            //
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(199, 369);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 31);
            this.button1.TabIndex = 17;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // DialogAbilityBits
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(408, 442);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbShldReset);
            this.Controls.Add(this.cbTeleBack);
            this.Controls.Add(this.cbShldVamp);
            this.Controls.Add(this.cbShldDrain);
            this.Controls.Add(this.cbAntiTorp);
            this.Controls.Add(this.cbAntiMine);
            this.Controls.Add(this.cbDrones);
            this.Controls.Add(this.cbTractor);
            this.Controls.Add(this.cbTeleport);
            this.Controls.Add(this.cbWarp);
            this.Controls.Add(this.cbHET);
            this.Controls.Add(this.cbCloak);
            this.Controls.Add(this.cbLowVis);
            this.Controls.Add(this.cbStealth);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        public System.Windows.Forms.CheckBox cbStealth;
        public System.Windows.Forms.CheckBox cbLowVis;
        public System.Windows.Forms.CheckBox cbCloak;
        public System.Windows.Forms.CheckBox cbHET;
        public System.Windows.Forms.CheckBox cbWarp;
        public System.Windows.Forms.CheckBox cbTeleport;
        public System.Windows.Forms.CheckBox cbTractor;
        public System.Windows.Forms.CheckBox cbDrones;
        public System.Windows.Forms.CheckBox cbAntiMine;
        public System.Windows.Forms.CheckBox cbAntiTorp;
        public System.Windows.Forms.CheckBox cbShldDrain;
        public System.Windows.Forms.CheckBox cbShldVamp;
        public System.Windows.Forms.CheckBox cbTeleBack;
        public System.Windows.Forms.CheckBox cbShldReset;
        private System.Windows.Forms.Button button1;
    }
}
