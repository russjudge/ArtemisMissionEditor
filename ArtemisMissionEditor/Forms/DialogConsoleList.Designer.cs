namespace ArtemisMissionEditor.Forms
{
	partial class DialogConsoleList
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
            this.cbM = new System.Windows.Forms.CheckBox();
            this.cbH = new System.Windows.Forms.CheckBox();
            this.cbW = new System.Windows.Forms.CheckBox();
            this.cbE = new System.Windows.Forms.CheckBox();
            this.cbS = new System.Windows.Forms.CheckBox();
            this.cbC = new System.Windows.Forms.CheckBox();
            this.cbO = new System.Windows.Forms.CheckBox();
            this.cbF = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(49, 249);
            this.okButton.Margin = new System.Windows.Forms.Padding(4);
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
            this.cancelButton.Location = new System.Drawing.Point(121, 249);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(76, 31);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cbM
            // 
            this.cbM.AutoSize = true;
            this.cbM.Location = new System.Drawing.Point(16, 15);
            this.cbM.Margin = new System.Windows.Forms.Padding(4);
            this.cbM.Name = "cbM";
            this.cbM.Size = new System.Drawing.Size(123, 21);
            this.cbM.TabIndex = 2;
            this.cbM.Text = "MAIN SCREEN";
            this.cbM.UseVisualStyleBackColor = true;
            // 
            // cbH
            // 
            this.cbH.AutoSize = true;
            this.cbH.Location = new System.Drawing.Point(16, 43);
            this.cbH.Margin = new System.Windows.Forms.Padding(4);
            this.cbH.Name = "cbH";
            this.cbH.Size = new System.Drawing.Size(77, 21);
            this.cbH.TabIndex = 3;
            this.cbH.Text = "HELMS";
            this.cbH.UseVisualStyleBackColor = true;
            // 
            // cbW
            // 
            this.cbW.AutoSize = true;
            this.cbW.Location = new System.Drawing.Point(16, 71);
            this.cbW.Margin = new System.Windows.Forms.Padding(4);
            this.cbW.Name = "cbW";
            this.cbW.Size = new System.Drawing.Size(100, 21);
            this.cbW.TabIndex = 4;
            this.cbW.Text = "WEAPONS";
            this.cbW.UseVisualStyleBackColor = true;
            // 
            // cbE
            // 
            this.cbE.AutoSize = true;
            this.cbE.Location = new System.Drawing.Point(16, 100);
            this.cbE.Margin = new System.Windows.Forms.Padding(4);
            this.cbE.Name = "cbE";
            this.cbE.Size = new System.Drawing.Size(125, 21);
            this.cbE.TabIndex = 5;
            this.cbE.Text = "ENGINEERING";
            this.cbE.UseVisualStyleBackColor = true;
            // 
            // cbS
            // 
            this.cbS.AutoSize = true;
            this.cbS.Location = new System.Drawing.Point(16, 128);
            this.cbS.Margin = new System.Windows.Forms.Padding(4);
            this.cbS.Name = "cbS";
            this.cbS.Size = new System.Drawing.Size(88, 21);
            this.cbS.TabIndex = 6;
            this.cbS.Text = "SCIENCE";
            this.cbS.UseVisualStyleBackColor = true;
            // 
            // cbC
            // 
            this.cbC.AutoSize = true;
            this.cbC.Location = new System.Drawing.Point(16, 156);
            this.cbC.Margin = new System.Windows.Forms.Padding(4);
            this.cbC.Name = "cbC";
            this.cbC.Size = new System.Drawing.Size(155, 21);
            this.cbC.TabIndex = 7;
            this.cbC.Text = "COMMUNICATIONS";
            this.cbC.UseVisualStyleBackColor = true;
            // 
            // cbO
            // 
            this.cbO.AutoSize = true;
            this.cbO.Location = new System.Drawing.Point(16, 185);
            this.cbO.Margin = new System.Windows.Forms.Padding(4);
            this.cbO.Name = "cbO";
            this.cbO.Size = new System.Drawing.Size(106, 21);
            this.cbO.TabIndex = 8;
            this.cbO.Text = "OBSERVER";
            this.cbO.UseVisualStyleBackColor = true;
            // 
            // cbF
            // 
            this.cbF.AutoSize = true;
            this.cbF.Location = new System.Drawing.Point(16, 214);
            this.cbF.Margin = new System.Windows.Forms.Padding(4);
            this.cbF.Name = "cbF";
            this.cbF.Size = new System.Drawing.Size(90, 21);
            this.cbF.TabIndex = 9;
            this.cbF.Text = "FIGHTER";
            this.cbF.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(13, 249);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 31);
            this.button1.TabIndex = 10;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DialogConsoleList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(220, 325);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbF);
            this.Controls.Add(this.cbO);
            this.Controls.Add(this.cbC);
            this.Controls.Add(this.cbS);
            this.Controls.Add(this.cbE);
            this.Controls.Add(this.cbW);
            this.Controls.Add(this.cbH);
            this.Controls.Add(this.cbM);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogConsoleList";
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
        public System.Windows.Forms.CheckBox cbM;
        public System.Windows.Forms.CheckBox cbH;
        public System.Windows.Forms.CheckBox cbW;
        public System.Windows.Forms.CheckBox cbE;
        public System.Windows.Forms.CheckBox cbS;
        public System.Windows.Forms.CheckBox cbC;
        public System.Windows.Forms.CheckBox cbO;
        public System.Windows.Forms.CheckBox cbF;
        private System.Windows.Forms.Button button1;
    }
}
