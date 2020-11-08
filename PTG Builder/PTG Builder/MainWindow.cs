using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;

namespace PTG_Builder
{
	public partial class MainWindow : Form
	{
		GLHandler glHandler;
		private OpenTK.GLControl glControl;
		public PTGTex ptgTex = new PTGTex("texture1");

		public MainWindow()
		{
			InitializeComponent();
			// 
			// glControl
			//
			glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
			glControl.Dock = DockStyle.Fill;
			glControlPanel.Controls.Add(glControl);
			glControl.BackColor = System.Drawing.Color.Black;
			glControl.Name = "glControl";
			glControl.TabIndex = 0;
			glControl.VSync = false;
			glControl.Load += new System.EventHandler(this.glControl_Load);
			glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
			glControl.Resize += new System.EventHandler(this.glControl_Resize);

			glHandler = new GLHandler();

			viewModeComboBox.SelectedIndex = 0;
		}

		bool loaded = false;

		private void MainWindow_Load(object sender, EventArgs e)
		{
			inputTimer.Start();
		}

		string compileLog = "";

		public void updateShader() {
			string mainVertexShaderCode = File.ReadAllText("shader.vsh");
			string mainFragmentShaderCode = File.ReadAllText("shader.fsh");
			CodePreprocessor preprocessor = new CodePreprocessor(mainVertexShaderCode, mainFragmentShaderCode);
			foreach (string filePath in Directory.GetFiles("codeparts", "*.codepart")) {
				preprocessor.addPart(File.ReadAllText(filePath));
			}
			preprocessor.addPart(ptgTex.getCode(), "texture");
			string[] processResult = preprocessor.process();

			if(processResult == null) {
				Console.WriteLine("Preprocess exception");
				return;
			}


			File.WriteAllText("processed_shader.vsh", processResult[0]);
			File.WriteAllText("processed_shader.fsh", processResult[1]);

			int shader = glHandler.compileShaders(processResult[0], processResult[1], out compileLog);
			Console.WriteLine("Compile result: " + shader);
			if (shader != -1)  {
				compileErrorLabel.Visible = false;
				glHandler.useShader(shader);
				ptgTex.updateUniforms(shader, true);
			} else {
				compileErrorLabel.Visible = true;
			}
		}

		private void glControl_Load(object sender, EventArgs e)
		{
			loaded = true;
			glHandler.glInit();
			updateShader();
			drawTimer.Start();
		}

		private void glControl_Paint(object sender, PaintEventArgs e)
		{
			if (!loaded)
				return;

			glHandler.glDraw();
			glControl.SwapBuffers();
					  
		}

		private void glControl_Resize(object sender, EventArgs e)
		{
			glHandler.glResize(glControl.Width, glControl.Height);
		}

		private void drawTimer_Tick(object sender, EventArgs e)
		{
			glControl.Refresh();
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public bool IsActive() {
			return GetForegroundWindow() == this.Handle;
		}

		const float MOUSE_ROTATION_SPEED = 1.0f;
		const float SCROLL_SCALE_SPEED = -1.0f;
		const float CAM_MIN_DISTANCE = 0.3f;
		MouseState lastMouseState;

		private void inputTimer_Tick(object sender, EventArgs e)
		{
			MouseState mouseState = Mouse.GetCursorState();
			Point glControlScreenPos1 = glControl.PointToScreen(new Point(0, 0));
			Point glControlScreenPos2 = glControl.PointToScreen(new Point(glControl.Width, glControl.Height));

			bool isInControl = IsActive() && ((glControlScreenPos1.X <= mouseState.X && mouseState.X <= glControlScreenPos2.X) &&
				(glControlScreenPos1.Y <= mouseState.Y && mouseState.Y <= glControlScreenPos2.Y));

			bool isDragged = isInControl && mouseState[MouseButton.Left];

			if (isDragged) {
				int dx = mouseState.X - lastMouseState.X;
				int dy = mouseState.Y - lastMouseState.Y;
				glHandler.modelRotation.Y += dx * MOUSE_ROTATION_SPEED;
				float newRotationX = glHandler.modelRotation.X + dy * MOUSE_ROTATION_SPEED;
				if (newRotationX > 135.0f)
					newRotationX = 135.0f;
				if(newRotationX < -90.0f)
					newRotationX = -90.0f;
				glHandler.modelRotation.X = newRotationX;
			}

			float dScrollY = mouseState.Scroll.Y - lastMouseState.Scroll.Y;
			if (isInControl && dScrollY != 0) {
				Vector3 camVec = glHandler.camPosition - glHandler.camLookAt;
				Vector3 newCamPosition = glHandler.camPosition + camVec.Normalized() * dScrollY * SCROLL_SCALE_SPEED;
				Vector3 newCamVec = newCamPosition - glHandler.camLookAt;
				if (newCamVec.Length < CAM_MIN_DISTANCE || (Vector3.Dot(camVec, newCamVec) < 0))
					newCamPosition = glHandler.camLookAt + camVec.Normalized() * CAM_MIN_DISTANCE;
				glHandler.camPosition = newCamPosition;

			}

			lastMouseState = mouseState;
		}

		private void recompileShaderButton_Click(object sender, EventArgs e)
		{
			updateShader();
		}

		private void bordersCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void compileErrorLabel_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Compile error: \n" + compileLog);
		}

		private void openCodeButton_Click(object sender, EventArgs e)
		{
			Process.Start("processed_shader.fsh");
		}

		public void updateLayers() {
			reliefListbox.clear();
			for (int index = 0; index < ptgTex.reliefLayers.Count; index++)
				reliefListbox.addPanel(new LayerElement(ptgTex.reliefLayers[index], this));

			textureListbox.clear();
			for (int index = 0; index < ptgTex.textureLayers.Count; index++)
				textureListbox.addPanel(new LayerElement(ptgTex.textureLayers[index], this));
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			InputDialog dialog = new InputDialog("Введите название слоя");
			
			if (dialog.ShowDialog() != DialogResult.OK)
				return;

			string layerName = dialog.inputString;
			if(layerName.Length == 0) {
				MessageBox.Show("Имя слоя не может быть пустым!");
				return;
			}

			Regex reg = new Regex("[a-zA-Z0-9]+");
			if(reg.Match(layerName).Length != layerName.Length) {
				MessageBox.Show("Имя слоя может содержать только символы a-z A-Z 0-9!");
				return;
			}
			

			foreach(Layer layer in ptgTex.reliefLayers)
				if(layer.name == layerName) {
					MessageBox.Show("Слой с таким именем уже существует!");
					return;
				}
			foreach(Layer layer in ptgTex.textureLayers)
				if(layer.name == layerName) {
					MessageBox.Show("Слой с таким именем уже существует!");
					return;
				}

			switch(layersTabControl.SelectedIndex) {
				case 0:{
					Layer layer = new Layer(layerName, Layer.LayerType.COLOR);
					ptgTex.textureLayers.Add(layer);
					textureListbox.addPanel(new LayerElement(layer, this));
				}
				break;
				case 1: {
					Layer layer = new Layer(layerName, Layer.LayerType.HEIGHT);
					ptgTex.reliefLayers.Add(layer);
					reliefListbox.addPanel(new LayerElement(layer, this));
				}
				break;
			}
			/*
			*/
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			
			switch(layersTabControl.SelectedIndex) {
				case 0:{
					Control selection = textureListbox.Selection;
					if(selection != null) {
						ptgTex.textureLayers.Remove(((LayerElement)selection).layer);
						textureListbox.removePanel(selection);
						updateShader();
					}
				}
				break;
				case 1: {
					Control selection = reliefListbox.Selection;
					if(selection != null) {
						ptgTex.reliefLayers.Remove(((LayerElement)selection).layer);
						reliefListbox.removePanel(selection);
						updateShader();
					}
				}
				break;
			}
		}

		private void viewModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			glHandler.viewMode = viewModeComboBox.SelectedIndex;
		}
	}
}
