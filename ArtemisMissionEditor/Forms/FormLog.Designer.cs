namespace ArtemisMissionEditor.Forms
{
	partial class FormLog
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
            this._FL_lb_Log = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _FL_lb_Log
            // 
            this._FL_lb_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._FL_lb_Log.FormattingEnabled = true;
            this._FL_lb_Log.IntegralHeight = false;
            this._FL_lb_Log.Location = new System.Drawing.Point(3, 3);
            this._FL_lb_Log.Name = "_FL_lb_Log";
            this._FL_lb_Log.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._FL_lb_Log.Size = new System.Drawing.Size(729, 348);
            this._FL_lb_Log.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Location = new System.Drawing.Point(3, 355);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(729, 42);
            this.panel1.TabIndex = 14;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(645, 10);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.VisibleChanged += new System.EventHandler(this.buttonOK_VisibleChanged);
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 399);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._FL_lb_Log);
            this.KeyPreview = true;
            this.Name = "FormLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FormLog_FormClosing);
            this.Load += new System.EventHandler(this._E_FL_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._FormLog_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.ListBox _FL_lb_Log;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;

	}
}