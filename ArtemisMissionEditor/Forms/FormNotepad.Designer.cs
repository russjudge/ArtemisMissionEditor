namespace ArtemisMissionEditor
{
    partial class FormNotepad
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
            this.doubleBufferedTextBox1 = new ArtemisMissionEditor.DoubleBufferedTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // doubleBufferedTextBox1
            // 
            this.doubleBufferedTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doubleBufferedTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.doubleBufferedTextBox1.Location = new System.Drawing.Point(3, 3);
            this.doubleBufferedTextBox1.Multiline = true;
            this.doubleBufferedTextBox1.Name = "doubleBufferedTextBox1";
            this.doubleBufferedTextBox1.ReadOnly = true;
            this.doubleBufferedTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.doubleBufferedTextBox1.Size = new System.Drawing.Size(540, 546);
            this.doubleBufferedTextBox1.TabIndex = 0;
            this.doubleBufferedTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.doubleBufferedTextBox1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Location = new System.Drawing.Point(0, 556);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(546, 42);
            this.panel1.TabIndex = 13;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOK.Location = new System.Drawing.Point(462, 10);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // FormNotepad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 598);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.doubleBufferedTextBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormNotepad";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Notepad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotepad_FormClosing);
            this.Load += new System.EventHandler(this.FormNotepad_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormNotepad_KeyDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedTextBox doubleBufferedTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
    }
}