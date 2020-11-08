using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG_Builder
{

	partial class GLHandler
	{
		public Vector3 camPosition = new Vector3(0.0f, 1.0f, 2.0f);
		public Vector3 camLookAt = new Vector3(0.0f, 0.0f, 0.0f);
		public Vector3 camUp = new Vector3(0.0f, 1.0f, 0.0f);

		public Vector3 modelRotation = new Vector3(0.0f, 0.0f, 0.0f);

		public Vector3 lightDirection = new Vector3(0.0f, -3.0f, 0.0f);

		public int viewMode = 0;

		private int currentProgramId = 0;
		int viewMatrixUniformId;
		int modelMatrixUniformId;
		int projectionMatrixUniformId;
		int lightDirectionUniformId;
		int vertexAttributeId;
		int texCoordAttributeId;
		int dUniformId;
		int viewModeUniformId;

		public void useShader(int id) {
			if (currentProgramId != 0) {
				GL.DeleteProgram(currentProgramId);
			}
			currentProgramId = id;
			viewMatrixUniformId = GL.GetUniformLocation(currentProgramId, "u_viewMatrix");
			modelMatrixUniformId = GL.GetUniformLocation(currentProgramId, "u_modelMatrix");
			lightDirectionUniformId = GL.GetUniformLocation(currentProgramId, "u_lightDirection");
			projectionMatrixUniformId = GL.GetUniformLocation(currentProgramId, "u_projectionMatrix");
			dUniformId = GL.GetUniformLocation(currentProgramId, "u_d");
			viewModeUniformId = GL.GetUniformLocation(currentProgramId, "u_viewMode");
			vertexAttributeId = GL.GetAttribLocation(currentProgramId, "a_vertexCoord");
			texCoordAttributeId = GL.GetAttribLocation(currentProgramId, "a_texCoord");
			Console.WriteLine("Use shader: " + lightDirectionUniformId + "; " + vertexAttributeId);
		}

		//Компиляция шейдера

		public int compileShaders(string vsh, string fsh, out string log) {
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vsh);
			GL.CompileShader(vertexShader);
			int status;
			GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status);
			if (status == 0) {
				log = shaderLog("vsh:", vertexShader);
				GL.DeleteShader(vertexShader);
				return -1;
			}
			Console.WriteLine("vsh: Compiled");

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fsh);
			GL.CompileShader(fragmentShader);
			GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status);
			if (status == 0) {
				log = shaderLog("fsh:", fragmentShader);
				GL.DeleteShader(vertexShader);
				GL.DeleteShader(fragmentShader);
				return -1;
			}
			Console.WriteLine("fsh: Compiled");


			var program = GL.CreateProgram();
			GL.AttachShader(program, vertexShader);
			GL.AttachShader(program, fragmentShader);
			GL.LinkProgram(program);
			GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
			if (status == 0) {
				Console.WriteLine("Program link error");
				GL.DeleteProgram(program);
				GL.DeleteShader(vertexShader);
				GL.DeleteShader(fragmentShader);
				log = "Link error";
				return -1;
			}
			Console.WriteLine("program: Linked");
			log = "Compile ok";

			return program;
		}

		static string shaderLog(string tag, int shader) {
			string infoLog;
			GL.GetShaderInfoLog(shader, out infoLog);
			string outLog = tag + " InfoLog: " + infoLog + "\n\n\n";
			Console.WriteLine(outLog);
			return outLog;
		}

		float[] borderVertices;
		float[] borderTexCoords;
		int[] borderIndices;

		float[] planeVertices = {
			 1.0f,  0.0f, -1.0f,
			-1.0f,  0.0f, -1.0f,
			-1.0f,  0.0f,  1.0f,
			 1.0f,  0.0f,  1.0f,
		};

		float[] planeTexCoords = {
			  1.0f,  -1.0f,
			 -1.0f,  -1.0f,
			 -1.0f,   1.0f,
			  1.0f,   1.0f
		};
		int[] planeIndices = {0, 1, 2, 0, 2, 3};

		int planeVBO;
		int planeTBO;
		int planeEBO;

		public void genBorders(int pointsPerBorder) {
			pointsPerBorder = Math.Max(pointsPerBorder, 2);
			borderVertices = new float[pointsPerBorder * 6 * 4];
		}

		public void glInit() {
			Console.WriteLine("Init");
			GL.GenBuffers(1, out planeVBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, planeVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(planeVertices.Length * sizeof(float)), planeVertices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.GenBuffers(1, out planeTBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, planeTBO);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(planeTexCoords.Length * sizeof(float)), planeTexCoords, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			GL.GenBuffers(1, out planeEBO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, planeEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(planeIndices.Length * sizeof(int)), planeIndices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
		float dx = 0.0001f;

		public void glDraw() {
			if (currentProgramId <= 0)
				return;

			GL.UseProgram(currentProgramId);

			//Set texture uniforms
			GL.Uniform1(viewModeUniformId, viewMode);
			Program.mainWindow.ptgTex.updateUniforms(currentProgramId);

			foreach (Layer layer in Program.mainWindow.ptgTex.reliefLayers)
				GL.Uniform1(GL.GetUniformLocation(currentProgramId, Program.mainWindow.ptgTex.getUVariableFullName("ptg_visiblity", layer.name)), layer.isVisible ? 1 : 0);
			foreach (Layer layer in Program.mainWindow.ptgTex.textureLayers)
				GL.Uniform1(GL.GetUniformLocation(currentProgramId, Program.mainWindow.ptgTex.getUVariableFullName("ptg_visiblity", layer.name)), layer.isVisible ? 1 : 0);


			GL.ClearColor(0.4f, 0.4f, 0.6f, 1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			
			Matrix4 viewMatrix = Matrix4.LookAt(camPosition, camLookAt, camUp);
			Matrix4 modelMatrix = Matrix4.Identity;
			modelMatrix = Matrix4.Mult(modelMatrix, Matrix4.CreateRotationY((float)(modelRotation.Y / 180.0 * Math.PI)));
			modelMatrix = Matrix4.Mult(modelMatrix, Matrix4.CreateRotationX((float)(modelRotation.X / 180.0 * Math.PI)));
			
			
			//GL.MatrixMode(MatrixMode.Modelview);

			GL.UniformMatrix4(viewMatrixUniformId, false, ref viewMatrix);
			GL.UniformMatrix4(modelMatrixUniformId, false, ref modelMatrix);
			GL.UniformMatrix4(projectionMatrixUniformId, false, ref projectionMatrix);
			GL.Uniform3(lightDirectionUniformId, lightDirection.X, lightDirection.Y, lightDirection.Z);
			if (Keyboard.GetState().IsKeyDown(Key.Up)) {
				dx += 0.0001f;
			}
			if (Keyboard.GetState().IsKeyDown(Key.Down))
			{
				dx -= 0.0001f;
			}
			if (Keyboard.GetState().IsKeyDown(Key.Left))
			{
				dx /= 1.05f;
			}
			if (Keyboard.GetState().IsKeyDown(Key.Right))
			{
				dx *= 1.05f;
			}
			GL.Uniform1(dUniformId, dx);
			//Вершины

			// GL.Enable(EnableCap.VertexArray);
			GL.EnableVertexAttribArray(vertexAttributeId);
			GL.EnableVertexAttribArray(texCoordAttributeId);

			GL.BindBuffer(BufferTarget.ArrayBuffer, planeVBO);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexAttribPointer(vertexAttributeId, 3, VertexAttribPointerType.Float, false, 12, 0); //Говорим компьютеру использовать буфер вершин

			GL.BindBuffer(BufferTarget.ArrayBuffer, planeTBO);
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.VertexAttribPointer(texCoordAttributeId, 2, VertexAttribPointerType.Float, false, 8, 0); //Говорим компьютеру использовать буфер вершин

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, planeEBO);
			GL.DrawElements(BeginMode.Triangles, planeIndices.Length, DrawElementsType.UnsignedInt, 0); //Рисуем элементы в порядке, заданном Indices

			//Говорим компьютеру, что наши буферы дальше использовать не надо
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);



			GL.DisableVertexAttribArray(vertexAttributeId);
			GL.DisableVertexAttribArray(texCoordAttributeId);
			//
		}

		Matrix4 projectionMatrix;

		public void glResize(int width, int height) {
			GL.Viewport(0, 0, width, height); //Устанавливаем новые координаты, где нужно отрисовывать картинку

			//Матрица проекции тоже меняется
			projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)width / (float)height, 0.1f, 6000);
		}


	}
}
