using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
	//TODO: Make another class that is focusable, so we have focusable and unfocusable labels <-- WTF does it mean? 
	public class NormalLabel : DoubleBufferedPanel
	{
		/// <summary>
        /// This is useful for active label and means its keyboard activation number.
        /// </summary>
        public int Number;

        /// <summary>
        /// Whether this label was selected by keyboard (number press) or other means.
        /// </summary>
        public bool SelectedByKeyboard;

        /// <summary>
        /// Whether linebreak is required after this label.
        /// </summary>
        public bool RequiresLinebreak;

		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value;}
		}

		/// <summary>
		/// THIS MEMBER IS MEANINGFUL!
		/// </summary>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
            TextRenderer.DrawText(e.Graphics, Text.TrimEnd(), Font, new Point(-5, 0), ForeColor);
		}

        public void UpdateSize()
        {
            Width = TextRenderer.MeasureText(CreateGraphics(), Text, Font, Size.Empty, TextFormatFlags.NoPadding).Width + 1; // +1 because default is too small

            // If the width is too big (label wont fit into control) we need to adjust the text
            int padding = 4; //TODO: Remember why it's 23 here and maybe fix this
            if (Width > Parent.Width - padding && !RequiresLinebreak)
            {
                // If the text length its required to have in order to fit is bigger than zero - then refit the string
                int newLength;
                /* We're actually cheating here. 
                 * 
                 * We're dividing target width by current width to get a modifier that means how much % of the string would VISUALLY fit.
                 * Then we're just multiplying our string length (in characters) by that and subtracting 4 (three dots and a space)
                 * This would work if the font we're using would be monospace, but it can as well be not monospace! 
                 * In which case what we should do is actually search for a number of letters that's as close to the max as possible.
                 * But that would probably take too much processing power for too little actual benefit 
                 * (I mean, we have to make an ellipsis so rarely that it hardly ever matters we don't get it perfectly!)
                 */
                if ((newLength = Text.Length * (Parent.Width - padding) / Width - 4) > 0) 
                    Text = Text.Substring(0, newLength) + "... ";
                // If we have no chance to fit the label - so shrink the string to one character (absolute minimum we can afford)
                else
                    Text = Text.Substring(0, 1) + "... ";
                Width = Parent.Width - padding;
                if (Width < 0) Width = 0;
            }
            Height = (int)Math.Round(TextRenderer.MeasureText(Text, Font).Height + 0.5);
            Invalidate();
        }

		public NormalLabel()
		{
			Padding = Padding.Empty;
			Margin = Padding.Empty;

            Font = Settings.Current.LabelFont;
			ForeColor = Color.Black;

            Number = -1;
            SelectedByKeyboard = false;
		}
	}

	public class NormalSelectableLabel : NormalLabel
	{
        /// <summary>
        /// Draw label with special color, used to denote expression members with invalid data.
        /// </summary>
        public bool SpecialMode;
		
        protected Brush SelectionBrush;
		protected Brush SpecialBrush;

        protected Pen NumberPen;
        protected Font NumberFont;
        protected Brush NumberBrush;
        
		private Color _selectionColor;
		public Color SelectionColor
		{
			get { return _selectionColor; }
			set { _selectionColor = value; SelectionBrush = new SolidBrush(SelectionColor); }
		}

		private Color _specialColor;
		public Color SpecialColor
		{
			get { return _specialColor; }
			set { _specialColor = value; SpecialBrush = new SolidBrush(SpecialColor); }
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Focus();
			base.OnMouseDown(e);
		}

        protected override void OnLostFocus(EventArgs e)
		{
			SelectedByKeyboard = false;
            if (FindForm().CanFocus)
				this.Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			this.Invalidate();
			base.OnGotFocus(e); 
		}

		protected override void OnPaint(PaintEventArgs e)
		{
            TextRenderer.DrawText(e.Graphics, Text.TrimEnd(), Font, new Point(-5, 0), SpecialMode ? SpecialColor : Focused ? SelectionColor: ForeColor);
            if (Settings.Current.ShowLabelNumbers && Number<=10) // Apparently, GDI cannot handle transparency? Therefore using GDI+
                e.Graphics.DrawString(Number == 10 ? "0" : Number.ToString(), NumberFont, NumberBrush, new Point(e.ClipRectangle.Left + 1 + e.ClipRectangle.Width - 12, e.ClipRectangle.Top -1 + 1));
		}

		public NormalSelectableLabel() : base()
		{
			//Makes our labels receive focus
			this.SetStyle(ControlStyles.Selectable, true);
			//Makes tab key stop at our labels
			this.TabStop = true;
			SpecialMode = false;

			Font = new Font(Font, FontStyle.Underline); 
			ForeColor = Color.Blue;
			SelectionColor = Color.FromArgb(0,160,0);
			SpecialColor = Color.FromArgb(240, 0, 0);

            NumberPen = new Pen(Color.FromArgb(128, 0, 0, 0));
            NumberBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            NumberFont= new Font("Segoe UI", 8);
		}
	}
}
