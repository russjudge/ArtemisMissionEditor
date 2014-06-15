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
        /// ITS TIME FOR BUSTER!!!!
		/// </summary>
		public bool SpecialMode;

		/// <summary>
        /// This is useful for active label and means its keyboard activation number
        /// </summary>
        public int Number;
        public bool SelectedByKeyboard;

		protected Form _form;
        
		protected Brush ForeBrush;

		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; ForeBrush = new SolidBrush(ForeColor); }
		}

		/// <summary>
		/// THIS MEMBER IS FUCKING MEANINGFUL!
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
			e.Graphics.DrawString(Text, Font, ForeBrush, new Point(-5, 0));
		}

		public NormalLabel(Form form)
		{
			_form = form;

			Padding = Padding.Empty;
			Margin = Padding.Empty;

			Font = new Font("Segoe UI", 16);
			ForeColor = Color.Black;

            Number = -1;
            SelectedByKeyboard = false;
		}
	}

	public class NormalSelectableLabel : NormalLabel
	{
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
            if (_form.CanFocus)
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
			e.Graphics.DrawString(Text, Font, this.Focused ? SelectionBrush : (SpecialMode ? SpecialBrush : ForeBrush), new Point(-5, 0));
            if (Settings.Current.ShowLabelNumbers && Number<=10)
                //Circle around number (looks BAD!)
				//e.Graphics.DrawEllipse(NumberPen, new Rectangle(e.ClipRectangle.Left + e.ClipRectangle.Width - 12, e.ClipRectangle.Top + 1, 11, 11));
                e.Graphics.DrawString(Number == 10 ? "0" : Number.ToString(), NumberFont, NumberBrush, new Point(e.ClipRectangle.Left + 1 + e.ClipRectangle.Width - 12, e.ClipRectangle.Top -1 + 1));
		}

		public NormalSelectableLabel(Form form) : base(form)
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
