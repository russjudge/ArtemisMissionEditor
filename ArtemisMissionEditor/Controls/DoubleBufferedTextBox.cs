using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtemisMissionEditor
{
    public class DoubleBufferedTextBox : TextBox
    {
        public DoubleBufferedTextBox()
        {
            DoubleBuffered = true;
        }
    }
}
