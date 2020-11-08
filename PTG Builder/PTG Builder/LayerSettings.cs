using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTG_Builder
{
	public partial class LayerSettings : Form
	{
		Layer layer;
		ValuePickForm valuePickForm;

		public LayerSettings(Layer layer) {
			this.layer = layer;
			InitializeComponent();
			Text = layer.name;
			//layerNameTextBox.Text = layer.name;
			functionTextBox.Text = layer.function;
			updateVariables();
			valuePickForm = new ValuePickForm(valueTextBox);
			overlayComboBox.SelectedIndex = layer.overlayType;
		}

		void updateVariables() {
			variablesListBox.Items.Clear();
			foreach(var pair in layer.variables) {
				variablesListBox.Items.Add(pair.Key);
			}
		}

		private void showVarsMessageBox_Click(object sender, EventArgs e) {
			MessageBox.Show(
				"x - координата x\n" +
				"z - координата z\n" +
				"t - время (мс)\n" +
				"\n" +
				"rand(x, z) - рандомный вектор");
		}

		private void addVariableButton_Click(object sender, EventArgs e) {
			//Interaction.InputBox("Question?","Title", "Default Text");
			//variablesListBox.Items.Add();
			InputDialog inputDialog = new InputDialog("Введите название переменной");
			if (inputDialog.ShowDialog() == DialogResult.Cancel) {
				return;
			}

			if (inputDialog.inputString.Length == 0)
				return;

			layer.variables.Add(inputDialog.inputString, 0.0f);
			updateVariables();
		}

		private void removeVariableButton_Click(object sender, EventArgs e) {
			if (variablesListBox.SelectedItem == null)
				return;
			layer.variables.Remove(variablesListBox.SelectedItem.ToString());
			updateVariables();
		}

		private void okButton_Click(object sender, EventArgs e) {
			layer.function = functionTextBox.Text;
			DialogResult = DialogResult.OK;
			Close();
			Program.mainWindow.updateShader();
		}

		private void variablesListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (variablesListBox.SelectedItem == null) {
				valueTextBox.Text = "";
				return;
			}

			valueTextBox.Text = layer.variables[variablesListBox.SelectedItem.ToString()].ToString();

		}

		private void showFunctionsDialog_Click(object sender, EventArgs e) {

		}

		private void valueTextBox_TextChanged(object sender, EventArgs e) {
			if (variablesListBox.SelectedItem == null)
				return;
			float newValue;
			if(float.TryParse(valueTextBox.Text, out newValue)) {
				layer.variables[variablesListBox.SelectedItem.ToString()] = newValue;
				layer.changedVars.Add(variablesListBox.SelectedItem.ToString());
			}
		}

		private void pickButton_Click(object sender, EventArgs e)
		{
			valuePickForm = new ValuePickForm(valueTextBox);
			float value;
			if(float.TryParse(valueTextBox.Text, out value))
				valuePickForm.setValue(value);
			valuePickForm.Show();
		}

		private void LayerSettings_FormClosing(object sender, FormClosingEventArgs e)
		{
			valuePickForm.Close();
		}

		private void functionTextBox_TextChanged(object sender, EventArgs e)
		{
			layer.function = functionTextBox.Text;
			float value;
			if(float.TryParse(valueTextBox.Text, out value))
				valuePickForm.setValue(value);
		}

		private void overlayComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (overlayComboBox.SelectedIndex == -1 || layer.overlayType == overlayComboBox.SelectedIndex)
				return;
			layer.overlayType = overlayComboBox.SelectedIndex;
			Program.mainWindow.updateShader();
		}
	}
}
