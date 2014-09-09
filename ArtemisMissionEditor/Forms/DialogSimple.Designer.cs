namespace ArtemisMissionEditor.Forms
{
	partial class DialogSimple
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
            BrushError.Dispose();
            BrushWarning.Dispose();
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
            this.input = new System.Windows.Forms.TextBox();
            this.nullButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.warningLabel = new System.Windows.Forms.LinkLabel();
            this.panelHighlighter = new System.Windows.Forms.Panel();
            this.openFileButton = new System.Windows.Forms.Button();
            this.timerValidate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.okButton.Location = new System.Drawing.Point(447, 44);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(53, 25);
            this.okButton.TabIndex = 1;
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
            this.cancelButton.Location = new System.Drawing.Point(501, 44);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(57, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.input.Location = new System.Drawing.Point(12, 13);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(545, 21);
            this.input.TabIndex = 0;
            this.input.TextChanged += new System.EventHandler(this.input_TextChanged);
            this.input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            // 
            // nullButton
            // 
            this.nullButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nullButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nullButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nullButton.Location = new System.Drawing.Point(393, 44);
            this.nullButton.Name = "nullButton";
            this.nullButton.Size = new System.Drawing.Size(53, 25);
            this.nullButton.TabIndex = 3;
            this.nullButton.Text = "Default";
            this.nullButton.UseVisualStyleBackColor = true;
            this.nullButton.Click += new System.EventHandler(this.nullButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(366, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 25);
            this.button1.TabIndex = 10;
            this.button1.TabStop = false;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // warningLabel
            // 
            this.warningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.warningLabel.AutoEllipsis = true;
            this.warningLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.warningLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.warningLabel.LinkColor = System.Drawing.Color.Black;
            this.warningLabel.Location = new System.Drawing.Point(12, 49);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(270, 18);
            this.warningLabel.TabIndex = 12;
            this.warningLabel.VisitedLinkColor = System.Drawing.Color.Black;
            this.warningLabel.Click += new System.EventHandler(this.warningLabel_Click);
            // 
            // panelHighlighter
            // 
            this.panelHighlighter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHighlighter.Location = new System.Drawing.Point(12, 34);
            this.panelHighlighter.Name = "panelHighlighter";
            this.panelHighlighter.Size = new System.Drawing.Size(545, 9);
            this.panelHighlighter.TabIndex = 13;
            this.panelHighlighter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHighlighter_Paint);
            // 
            // openFileButton
            // 
            this.openFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openFileButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.openFileButton.Image = global::ArtemisMissionEditor.Properties.Resources.action_save;
            this.openFileButton.Location = new System.Drawing.Point(283, 44);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(82, 25);
            this.openFileButton.TabIndex = 11;
            this.openFileButton.Text = "Open File";
            this.openFileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Visible = false;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // timerValidate
            // 
            this.timerValidate.Interval = 500;
            this.timerValidate.Tick += new System.EventHandler(this.timerValidate_Tick);
            // 
            // DialogSimple
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(569, 79);
            this.ControlBox = false;
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.panelHighlighter);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nullButton);
            this.Controls.Add(this.input);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogSimple";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.Shown += new System.EventHandler(this.DialogSimple_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox input;
		private System.Windows.Forms.Button nullButton;
		private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.LinkLabel warningLabel;
        private System.Windows.Forms.Panel panelHighlighter;
        private System.Windows.Forms.Timer timerValidate;
	}
}