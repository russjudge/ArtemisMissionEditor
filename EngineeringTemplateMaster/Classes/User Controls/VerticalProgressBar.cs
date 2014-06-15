using System.Windows.Forms;

namespace EngineeringTemplateMaster
{
	public delegate void ValueChangedEventHandler();

	public class VerticalProgressBar : ProgressBar
	{
		public VerticalProgressBar()
		{
			_leftMouseDown = false;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.Style |= 0x04;
				return cp;
			}
		}

		private bool _leftMouseDown;

		public event ValueChangedEventHandler ValueChanged;

		public void OnValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged();
		}

		private void UpdateValueBasedOnMousePosition(double position)
		{
			int result = (int)(300.0 * position);
			result = result < 0 ? 0 : result > 300 ? 300 : result;
			if (Value != result)
			{
				Value = result;
				OnValueChanged();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				_leftMouseDown = true;

			if (_leftMouseDown)
				UpdateValueBasedOnMousePosition(1.0 - (double)e.Location.Y / this.Height);

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (_leftMouseDown)
				UpdateValueBasedOnMousePosition(1.0 - (double)e.Location.Y / this.Height);
			
			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_leftMouseDown = false;

			base.OnMouseUp(e);
		}
	}
}