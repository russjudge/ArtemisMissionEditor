namespace EngineeringTemplateMaster
{
	partial class ShipSystemControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.coolantBar = new System.Windows.Forms.Panel();
            this.energy = new System.Windows.Forms.TextBox();
            this.coolant = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.energyBar = new EngineeringTemplateMaster.VerticalProgressBar();
            this.myLabel1 = new EngineeringTemplateMaster.MyLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // coolantBar
            // 
            this.coolantBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.coolantBar.BackColor = System.Drawing.SystemColors.Control;
            this.coolantBar.Location = new System.Drawing.Point(34, 1);
            this.coolantBar.Margin = new System.Windows.Forms.Padding(1);
            this.coolantBar.Name = "coolantBar";
            this.coolantBar.Size = new System.Drawing.Size(14, 62);
            this.coolantBar.TabIndex = 2;
            this.coolantBar.Paint += new System.Windows.Forms.PaintEventHandler(this.coolantBar_Paint);
            this.coolantBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coolantBar_MouseDown);
            this.coolantBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.coolantBar_MouseMove);
            this.coolantBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.coolantBar_MouseUp);
            // 
            // energy
            // 
            this.energy.BackColor = System.Drawing.SystemColors.Control;
            this.energy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.energy.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.energy.Location = new System.Drawing.Point(3, 3);
            this.energy.Name = "energy";
            this.energy.Size = new System.Drawing.Size(36, 23);
            this.energy.TabIndex = 3;
            this.energy.Text = "100";
            this.energy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.energy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.energy_KeyDown);
            this.energy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.energy_MouseDown);
            // 
            // coolant
            // 
            this.coolant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.coolant.BackColor = System.Drawing.SystemColors.Control;
            this.coolant.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coolant.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.coolant.Location = new System.Drawing.Point(3, 7);
            this.coolant.Name = "coolant";
            this.coolant.Size = new System.Drawing.Size(36, 23);
            this.coolant.TabIndex = 5;
            this.coolant.Text = "0";
            this.coolant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.coolant.KeyDown += new System.Windows.Forms.KeyEventHandler(this.coolant_KeyDown);
            this.coolant.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coolant_MouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(49, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(45, 62);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.energy);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(45, 31);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.coolant);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 31);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(45, 31);
            this.panel4.TabIndex = 1;
            // 
            // energyBar
            // 
            this.energyBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.energyBar.Location = new System.Drawing.Point(18, 1);
            this.energyBar.Margin = new System.Windows.Forms.Padding(1);
            this.energyBar.MarqueeAnimationSpeed = 0;
            this.energyBar.Maximum = 300;
            this.energyBar.Name = "energyBar";
            this.energyBar.Size = new System.Drawing.Size(14, 62);
            this.energyBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.energyBar.TabIndex = 8;
            this.energyBar.Value = 100;
            this.energyBar.ValueChanged += new EngineeringTemplateMaster.ValueChangedEventHandler(this.energyBar_ValueChanged);
            // 
            // myLabel1
            // 
            this.myLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.myLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.myLabel1.Location = new System.Drawing.Point(0, 0);
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.NewText = "SHIP SYSTEM";
            this.myLabel1.RotateAngle = -90;
            this.myLabel1.Size = new System.Drawing.Size(14, 64);
            this.myLabel1.TabIndex = 0;
            this.myLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ShipSystemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.energyBar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.coolantBar);
            this.Controls.Add(this.myLabel1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ShipSystemControl";
            this.Size = new System.Drawing.Size(94, 64);
            this.Resize += new System.EventHandler(this.ShipSystemControl_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private MyLabel myLabel1;
		private System.Windows.Forms.Panel coolantBar;
		private System.Windows.Forms.TextBox energy;
		private System.Windows.Forms.TextBox coolant;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private VerticalProgressBar energyBar;


	}
}
