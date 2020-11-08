using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTG_Builder
{
	public partial class LayerElement : UserControl
	{
		private Form ownerForm;
		public Layer layer;

		public LayerElement(Layer layer, Form ownerForm)
		{
			this.ownerForm = ownerForm;
			this.layer = layer;
			InitializeComponent();
			visiblityCheckBox.Checked = layer.isVisible;
			nameLabel.Text = layer.name;
		}

		private void visiblityCheckBox_CheckedChanged(object sender, EventArgs e) {
			layer.isVisible = visiblityCheckBox.Checked;
		}

		private void nameLabel_Click(object sender, EventArgs e) {
		}

		private void LayerElement_DoubleClick(object sender, EventArgs e) {
			LayerSettings layerSettings = new LayerSettings(layer);
			layerSettings.Show(ownerForm);
		}
	}
}
