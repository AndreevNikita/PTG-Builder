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
	public partial class PanelListbox : UserControl
	{
		private Control selection = null;
		public Control Selection {
			get {
				return selection;
			}

			set {
				if (selection != null)
					selection.BackColor = Color.Transparent;
				if (value != null) {
					selection = (Control)value;
					selection.BackColor = Color.LightSkyBlue;
				} else
					selection = null;
			}
		}

		public PanelListbox()
		{
			InitializeComponent();
		}

		public void update()
		{
			if(panelsContainerPanel.Controls.Count == 0) {
				panelsContainerPanel.Height = 0;
				return;
			}
			int sumHeight = 0;
			for (int index = 0; index < panelsContainerPanel.Controls.Count; index++)
				sumHeight += panelsContainerPanel.Controls[index].Height;

			panelsContainerPanel.Height = sumHeight;

			panelsContainerPanel.Controls[panelsContainerPanel.Controls.Count - 1].Location = new Point(0, 0);
			for (int index = panelsContainerPanel.Controls.Count - 2; index >= 0; index--)
			{
				Control panel = panelsContainerPanel.Controls[index];
				//
				panel.Location = new Point(0, panelsContainerPanel.Controls[index + 1].Bounds.Bottom + 1);
				panel.Width = panelsContainerPanel.Width;
			}
			/*
			if (panelsContainerPanel.Controls.Count == 0)
				panelsContainerPanel.Height = 0;
			else
				panelsContainerPanel.Height = panelsContainerPanel.Controls[panelsContainerPanel.Controls.Count - 1].Bounds.Bottom;
				*/
		
	 
			
			if (viewPanel.Height > panelsContainerPanel.Height)
				vScrollBar.Enabled = false;
			else {
				vScrollBar.Enabled = true;
				vScrollBar.Maximum = panelsContainerPanel.Height - viewPanel.Height;
			}
		}

		public void addPanel(Control panel) {
			panel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			panel.Click += childPanel_Click;
			panelsContainerPanel.Controls.Add(panel);
			update();
			/*
			int yPos = 0;
			if(panelsContainerPanel.Controls.Count > 0) {
				yPos = panelsContainerPanel.Controls[panelsContainerPanel.Controls.Count - 1].Bounds.Bottom;
			}
			panel.Location = new Point(0, yPos);
			panel.Width = panelsContainerPanel.Width;
			panel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			panelsContainerPanel.Controls.Add(panel);*/
		}

		public void removePanel(Control panel) {
			if (panel == selection)
				select(null);
			panelsContainerPanel.Controls.Remove(panel);
			panel.Click -= childPanel_Click;
			update();
		}

		public void clear() {
			panelsContainerPanel.Controls.Clear();
			update();
		}

		private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			panelsContainerPanel.Location = new Point(0, -vScrollBar.Value);
		}

		public void select(Control control) {
			Selection = control;
		}

		private void childPanel_Click(object sender, EventArgs e) {
			select((Control)sender);
		}
	}
}
