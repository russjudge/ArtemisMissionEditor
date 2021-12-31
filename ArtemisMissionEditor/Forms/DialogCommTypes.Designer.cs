namespace ArtemisMissionEditor.Forms
{
    partial class DialogCommTypes
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
            this.cbAlert = new System.Windows.Forms.CheckBox();
            this.cbSide = new System.Windows.Forms.CheckBox();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.cbPlayer = new System.Windows.Forms.CheckBox();
            this.cbStation = new System.Windows.Forms.CheckBox();
            this.cbEnemy = new System.Windows.Forms.CheckBox();
            this.cbFriend = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(37, 174);
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
            this.cancelButton.Location = new System.Drawing.Point(91, 174);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(57, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cbAlert
            // 
            this.cbAlert.AutoSize = true;
            this.cbAlert.Location = new System.Drawing.Point(12, 12);
            this.cbAlert.Name = "cbAlert";
            this.cbAlert.Size = new System.Drawing.Size(61, 17);
            this.cbAlert.TabIndex = 2;
            this.cbAlert.Text = "ALERT";
            this.cbAlert.UseVisualStyleBackColor = true;
            // 
            // cbSide
            // 
            this.cbSide.AutoSize = true;
            this.cbSide.Location = new System.Drawing.Point(12, 35);
            this.cbSide.Name = "cbSide";
            this.cbSide.Size = new System.Drawing.Size(51, 17);
            this.cbSide.TabIndex = 3;
            this.cbSide.Text = "SIDE";
            this.cbSide.UseVisualStyleBackColor = true;
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(12, 58);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(69, 17);
            this.cbStatus.TabIndex = 4;
            this.cbStatus.Text = "STATUS";
            this.cbStatus.UseVisualStyleBackColor = true;
            // 
            // cbPlayer
            // 
            this.cbPlayer.AutoSize = true;
            this.cbPlayer.Location = new System.Drawing.Point(12, 81);
            this.cbPlayer.Name = "cbPlayer";
            this.cbPlayer.Size = new System.Drawing.Size(68, 17);
            this.cbPlayer.TabIndex = 5;
            this.cbPlayer.Text = "PLAYER";
            this.cbPlayer.UseVisualStyleBackColor = true;
            // 
            // cbStation
            // 
            this.cbStation.AutoSize = true;
            this.cbStation.Location = new System.Drawing.Point(12, 104);
            this.cbStation.Name = "cbStation";
            this.cbStation.Size = new System.Drawing.Size(73, 17);
            this.cbStation.TabIndex = 6;
            this.cbStation.Text = "STATION";
            this.cbStation.UseVisualStyleBackColor = true;
            // 
            // cbEnemy
            // 
            this.cbEnemy.AutoSize = true;
            this.cbEnemy.Location = new System.Drawing.Point(12, 127);
            this.cbEnemy.Name = "cbEnemy";
            this.cbEnemy.Size = new System.Drawing.Size(64, 17);
            this.cbEnemy.TabIndex = 7;
            this.cbEnemy.Text = "ENEMY";
            this.cbEnemy.UseVisualStyleBackColor = true;
            // 
            // cbFriend
            // 
            this.cbFriend.AutoSize = true;
            this.cbFriend.Location = new System.Drawing.Point(12, 150);
            this.cbFriend.Name = "cbFriend";
            this.cbFriend.Size = new System.Drawing.Size(66, 17);
            this.cbFriend.TabIndex = 8;
            this.cbFriend.Text = "FRIEND";
            this.cbFriend.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(10, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 9;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DialogCommTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(159, 209);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbFriend);
            this.Controls.Add(this.cbEnemy);
            this.Controls.Add(this.cbStation);
            this.Controls.Add(this.cbPlayer);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.cbSide);
            this.Controls.Add(this.cbAlert);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCommTypes";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DialogCommTypes_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.CheckBox cbAlert;
        public System.Windows.Forms.CheckBox cbSide;
        public System.Windows.Forms.CheckBox cbStatus;
        public System.Windows.Forms.CheckBox cbPlayer;
        public System.Windows.Forms.CheckBox cbStation;
        public System.Windows.Forms.CheckBox cbEnemy;
        public System.Windows.Forms.CheckBox cbFriend;
        private System.Windows.Forms.Button button1;
    }
}
