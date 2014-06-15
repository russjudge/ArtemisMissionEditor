namespace ArtemisMissionEditor
{
	partial class _FormLog
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
			this.SuspendLayout();
			// 
			// _FL_lb_Log
			// 
			this._FL_lb_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FL_lb_Log.FormattingEnabled = true;
			this._FL_lb_Log.IntegralHeight = false;
			this._FL_lb_Log.Location = new System.Drawing.Point(12, 12);
			this._FL_lb_Log.Name = "_FL_lb_Log";
			this._FL_lb_Log.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this._FL_lb_Log.Size = new System.Drawing.Size(711, 375);
			this._FL_lb_Log.TabIndex = 0;
			// 
			// _FormLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(735, 399);
			this.Controls.Add(this._FL_lb_Log);
			this.KeyPreview = true;
			this.Name = "_FormLog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Log";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._E_FormLog_FormClosing);
			this.Load += new System.EventHandler(this._E_FL_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this._FormLog_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.ListBox _FL_lb_Log;

	}
}