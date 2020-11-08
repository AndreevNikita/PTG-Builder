using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTG_Builder
{
	public partial class ValuePickForm : Form
	{
		TextBox pickTextBox;

		public ValuePickForm(TextBox pickTextBox)
		{
			InitializeComponent();
			this.pickTextBox = pickTextBox;
		}

		public void setValue(float value) {
			trackBar.Value = Math.Max(Math.Min((int)(value * 100), 100), 0);
		}

		private void trackBar_Scroll(object sender, EventArgs e) {
			pickTextBox.Text = ((float)trackBar.Value / 100.0f).ToString();
		}
	}
}
