using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_palette
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            setColor();
        }

        private void setColor()
        {
            colorBox.BackColor = 
                Color.FromArgb(redTrackBar.Value, greenTrackBar.Value, blueTrackBar.Value);

            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(colorBox.BackColor.ToArgb()));
            toolTip1.SetToolTip(colorBox, hexColor);

            Clipboard.SetText(hexColor);
        }

        private void redTrackBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }

        private void greenTrackBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }

        private void blueTrackBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }
    }
}
