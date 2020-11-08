using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using PTG_Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Refraction
{
    public partial class MainWindow : Form
    {
        public MainWindow() {
            InitializeComponent();
        }

        //Состояние камеры
        float camYaw = 0.0f; //Вращение
        float camPitch = 0.0f; //Наклон
        Vector4 camDirection = new Vector4(0.0f, 0.0f, 0.0f, 1); //Направление камеры
        Vector3 camPosition = new Vector3(0.0f, 20.0f, 50.0f); //Позиция в пространстве

        //Вспомогательные структуры хранения данных
        public struct Vertex {
            public float X, Y, Z;
            public Vertex(float x = 0.0f, float y = 0.0f, float z = 0.0f) {
                X = x; Y = y; Z = z;
            }

            public Vertex(Vector3 vec) {
                X = vec.X;
                Y = vec.Y;
                Z = vec.Z;
            }

            public const int Stride = 12;

            public Vertex subtract(Vertex v) {
                return new Vertex(X - v.X, Y - v.Y, Z - v.Z);
            }

            public Vector3 tov3() {
                return new Vector3(X, Y, Z);
            }

            public YPDirection toYP() {
                Vector3 vec = tov3().Normalized();
                
                double yaw = Math.Acos(vec.X);
                if (vec.Z < 0.0)
                    yaw = Math.PI * 2.0 - yaw;

                double pitch = Math.Asin(vec.Y);

                return new YPDirection((float)yaw, (float)pitch);
            }
        }

        public struct YPDirection {
            public float yaw;
            public float pitch;

            public YPDirection(float yaw, float pitch) {
                this.yaw = yaw;
                this.pitch = pitch;
            }

            public Vertex toVertex() {
                double X, Y, Z;
                Vector3 axisX = new Vector3(1.0f, 0.0f, 0.0f);
                X = axisX.X * Math.Cos(pitch) - axisX.Y * Math.Sin(pitch);
                Y = axisX.X * Math.Sin(pitch) + axisX.Y * Math.Cos(pitch);
                Z = axisX.Z;
                axisX.X = (float)X; axisX.Y = (float)Y; axisX.Z = (float)Z;
                X = axisX.X * Math.Cos(yaw) + axisX.Z * Math.Sin(yaw);
                Y = axisX.Y;
                Z = -axisX.X * Math.Sin(yaw) + axisX.Z * Math.Cos(yaw);
                axisX.X = (float)X; axisX.Y = (float)Y; axisX.Z = (float)Z;
                return new Vertex(axisX.X, axisX.Y, axisX.Z);
            }
        }

        public YPDirection interpolate(YPDirection yp1, YPDirection yp2) {
            return new YPDirection((yp1.yaw + yp2.yaw) / 2.0f, (yp1.pitch + yp2.pitch) / 2.0f);
        }

        public struct Vertex2 {
            public float X, Y;
            public Vertex2(float x, float y) {
                X = x;
                Y = y;
            }

            public const int Stride = 8;

            public override string ToString() {
                return X + "; " + Y;
            }
        }

        //Метод, умножающий матрицу на вектор (не нашёл такого в OpenTK)
        private Vector4 mul(Matrix4 mat, Vector4 vec) {
            return new Vector4(
                mat.M11 * vec.X + mat.M12 * vec.Y + mat.M13 * vec.Z + mat.M14 * vec.W,
                mat.M21 * vec.X + mat.M22 * vec.Y + mat.M23 * vec.Z + mat.M24 * vec.W,
                mat.M31 * vec.X + mat.M32 * vec.Y + mat.M33 * vec.Z + mat.M34 * vec.W,
                mat.M41 * vec.X + mat.M42 * vec.Y + mat.M43 * vec.Z + mat.M44 * vec.W
            );
        }

        const float SCENE_SIDE = 60.0f; //Сторона сцены
        const float WATER_Y = 1.0f; //Высота водной поверхности
        int[] waterIndices = { 0, 1, 2, 0, 2, 3 };
        Vertex[] waterMesh = new Vertex[4];
        Vertex2[] waterTexVertices = new Vertex2[4];
        int waterVBO;
        int waterEBO;
        int waterTBO;

        const float HEIGHT = 0.75f;

        int waterProgram;
        int refractionTextureUniformId;
        int camPositionUniformId;
        int texCoordAttributeId;
        int normalAttributeId;
        int vertexAttributeId;

        bool loaded = false;


        const float sensivity = 0.005f; //Чуствительность мышки 
        const float speed = 10.0f; //Скорость перемещения
        Point lastPos = new Point();
        bool lastKeyDown = false;

        int texture;
        int poolTexture;
        float poolDepth = 60.0f;

        //Метод, следящий за мышкой. Каждый раз считает разницу позиций курсора и высчитывает углы поворота камеры (camYaw и camPitch)
        private void MouseTimer_Tick(object sender, EventArgs e) {
            //Получаем объекты мыши и клавиатуры
            MouseState ms = Mouse.GetState();
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Key.Space)) { //Если нажат пробел, то ничего не делаем (в том числе, не возвращаем курсор в центр окна)
                lastKeyDown = true;
                return;
            }

            if (lastKeyDown) {
                lastPos = new Point(ms.X, ms.Y); //Устанавливаем прошлую позицию мышки как только отпустили пробел
                lastKeyDown = false;
            }

            //Находим изменение в координатах
            int dx = ms.X - lastPos.X;
            int dy = ms.Y - lastPos.Y;
            //Добавляем к уже имеющимся углам
            float dYaw = dx * sensivity;
            float dPitch = dy * sensivity;
            camYaw += dYaw;
            camPitch += dPitch;
            if (camPitch > Math.PI / 2 - 0.001f)
                camPitch = (float)Math.PI / 2 - 0.001f;
            if (camPitch < -Math.PI / 2 - 0.001f)
                camPitch = (float)-Math.PI / 2 - 0.001f;

            Vector4 direction = new Vector4(0.0f, 0.0f, -1.0f, 1.0f);
			 Matrix4 rotationXMatrix;
			Matrix4.CreateRotationX(camPitch, out rotationXMatrix);
			Matrix4 rotationYMatrix;
			Matrix4.CreateRotationY(camYaw, out rotationYMatrix);
            direction = mul(rotationXMatrix, direction);
            direction = mul(rotationYMatrix, direction);
            camDirection = direction;

            Point glAreaPosition = new Point();
            glAreaPosition = glControl.PointToScreen(new Point(0, 0));

            Mouse.SetPosition(glAreaPosition.X + glControl.Width / 2, glAreaPosition.Y + glControl.Height / 2); //Смещаем курсор обратно в центр окна

            lastPos = new Point(ms.X, ms.Y);
        }

        //Раз в 10 миллисекунд кнопки клавиатуры проверяются на нажатие. Если кнопка нажата, камера сдвигается в соответствующем направлении
        private void keyboardTick(object sender, EventArgs e) {
            Vector2 dir = new Vector2(camDirection.X, camDirection.Z);
            dir.Normalize();

            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Key.W))
                camPosition += new Vector3(camDirection.X, camDirection.Y, camDirection.Z);
            if (kbs.IsKeyDown(Key.S))
                camPosition -= new Vector3(camDirection.X, camDirection.Y, camDirection.Z);
            if (kbs.IsKeyDown(Key.A))
                camPosition -= new Vector3(-dir.Y, 0, dir.X);
            if (kbs.IsKeyDown(Key.D))
                camPosition += new Vector3(-dir.Y, 0, dir.X);
        }

        //Загрузка текстуры
        static int loadTexture(string filename) {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture(); //Создаём новую текстуру в OpenGL
            GL.BindTexture(TextureTarget.Texture2D, id); //Говорим, что сейчас будем работать с ней

            Bitmap bmp = new Bitmap(filename); //Загружаем изображение текстуры
            //Получаем пиксели изображения
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //Передаём пиксели загруженного изображеняия изображению в OpenGL
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            //Параметры фильтрации текстур (при масштабировании)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            return id;
        }

        private void MainWindow_Load(object sender, EventArgs e) {
            Point glAreaPosition = new Point();
            glAreaPosition = glControl.PointToScreen(new Point(0, 0));
            Mouse.SetPosition(glAreaPosition.X + glControl.Width / 2, glAreaPosition.Y + glControl.Height / 2); //Смещаем курсор обратно в центр окна

            Timer drawTimer = new Timer();
            drawTimer.Tick += drawTick;
            drawTimer.Interval = 10;
            drawTimer.Start();

            Timer keyboardTimer = new Timer();
            keyboardTimer.Tick += keyboardTick;
            keyboardTimer.Interval = 10;
            keyboardTimer.Start();

            Timer mouseTimer = new Timer();
            mouseTimer.Tick += MouseTimer_Tick;
            mouseTimer.Interval = 10;
            mouseTimer.Start();
        }

        private int compileShaders(string vsh, string fsh) {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vsh);
            GL.CompileShader(vertexShader);
            shaderLog("vsh" + ":", vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fsh);
            GL.CompileShader(fragmentShader);
            shaderLog("fsh" + ":", fragmentShader);

            var program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            return program;
        }

        static void shaderLog(string tag, int shader) {
            string infoLog;
            GL.GetShaderInfoLog(shader, out infoLog);
            Console.WriteLine(tag + " InfoLog: " + infoLog + "\n\n\n");
        }

        private void drawTick(object sender, EventArgs e) { //Метод исполняется каждые 10 мс и перерисовывает изображение
            glControl.Refresh();
        }

        FBO fbo;
		PTGTex ptgTex;

        private void glControl_Load(object sender, EventArgs e) {
			BinaryFormatter formatter = new BinaryFormatter();

			if(File.Exists("texture.ptg")) {
				using (FileStream fs = new FileStream("texture.ptg", FileMode.Open)) {
					ptgTex = (PTGTex)formatter.Deserialize(fs);
				}
			} else {
			}

			waterMesh[0] = new Vertex(SCENE_SIDE / 2.0f,  0.0f, -SCENE_SIDE / 2.0f);
			waterMesh[1] = new Vertex(-SCENE_SIDE / 2.0f,  0.0f, -SCENE_SIDE / 2.0f);
			waterMesh[2] = new Vertex(-SCENE_SIDE / 2.0f,  0.0f,  SCENE_SIDE / 2.0f);
			waterMesh[3] = new Vertex(SCENE_SIDE / 2.0f,  0.0f,  SCENE_SIDE / 2.0f);

            //Получившийся массив вершин записывается в буфер OpenGL
            GL.GenBuffers(1, out waterVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, waterVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(waterMesh.Length * Vertex.Stride), waterMesh, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			waterTexVertices[0] = new Vertex2(1.0f,  0.0f);
			waterTexVertices[1] = new Vertex2(0.0f,  0.0f);
			waterTexVertices[2] = new Vertex2(0.0f,   1.0f);
			waterTexVertices[3] = new Vertex2(1.0f,   1.0f);
			  
            //Пишем их в буфер

            GL.GenBuffers(1, out waterTBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, waterTBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(waterTexVertices.Length * Vertex2.Stride), waterTexVertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.GenBuffers(1, out waterEBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, waterEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(waterIndices.Length * sizeof(int)), waterIndices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            texture = loadTexture("texture.jpg");
            poolTexture = loadTexture("Стенка.jpg");

            fbo = new FBO(glControl);

			string mainVertexShaderCode = File.ReadAllText("WaterShader.vsh");
			string mainFragmentShaderCode = File.ReadAllText("WaterShader.fsh");
			CodePreprocessor preprocessor = new CodePreprocessor(mainVertexShaderCode, mainFragmentShaderCode);
			foreach (string filePath in Directory.GetFiles("codeparts", "*.codepart")) {
				preprocessor.addPart(File.ReadAllText(filePath));
			}

			preprocessor.addPart(ptgTex.getCode(), "texture");
			string[] processResult = preprocessor.process();
			
			File.WriteAllText("processed_shader.vsh", processResult[0]);
			File.WriteAllText("processed_shader.fsh", processResult[1]);
			if(processResult == null) {
				Console.WriteLine("Preprocess exception");
				return;
			}

            waterProgram = compileShaders(processResult[0], processResult[1]);
            refractionTextureUniformId = GL.GetUniformLocation(waterProgram, "u_refrationTexture");
            camPositionUniformId = GL.GetUniformLocation(waterProgram, "u_camPosition");
            vertexAttributeId = GL.GetAttribLocation(waterProgram, "a_vertexCoord");
            normalAttributeId = GL.GetAttribLocation(waterProgram, "a_normal");
            texCoordAttributeId = GL.GetAttribLocation(waterProgram, "a_texCoord");
            Console.WriteLine(vertexAttributeId);
            Console.WriteLine(texCoordAttributeId);

            loaded = true;
        }

        long time = 0;

        const long DROP_TIME = 100;
        const long GROW_TIME = 10;
        Vertex2 dropCoord = new Vertex2(0.0f, 0.0f);
        bool wasWaves = false;
        long startTime = 100;

        Random rand = new Random();

        private void glControl_Paint(object sender, PaintEventArgs e) {
            if (!loaded)
                return;

            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //Очищаем экран и буфер глубины
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Enable(EnableCap.DepthTest); //Включаем тест глубины, чтобы соблюдался порядок по дальности 
            //Говорим, какие источники света будут задействованы

            //Обновляем матрицу вида
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 viewMatrix = Matrix4.LookAt(camPosition.X, camPosition.Y, camPosition.Z, camPosition.X + camDirection.X, camPosition.Y + camDirection.Y, camPosition.Z + camDirection.Z, 0, 1, 0);
            GL.LoadMatrix(ref viewMatrix);

            //Рендерим сцену в буфер
            fbo.bindRefractionFrameBuffer();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            renderScene();
            fbo.unbindCurrentFrameBuffer(glControl);

            //Рендерим сцену на экран
            renderScene();


            //Рисуем воду
            GL.UseProgram(waterProgram);
			ptgTex.updateUniforms(waterProgram, true);
			foreach (Layer layer in ptgTex.reliefLayers)
				GL.Uniform1(GL.GetUniformLocation(waterProgram, ptgTex.getUVariableFullName("ptg_visiblity", layer.name)), layer.isVisible ? 1 : 0);
			foreach (Layer layer in ptgTex.textureLayers)
				GL.Uniform1(GL.GetUniformLocation(waterProgram, ptgTex.getUVariableFullName("ptg_visiblity", layer.name)), layer.isVisible ? 1 : 0);

            GL.EnableVertexAttribArray(vertexAttributeId);
            //GL.EnableVertexAttribArray(normalAttributeId);
            GL.EnableVertexAttribArray(texCoordAttributeId);
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.Uniform4(camPositionUniformId, camPosition.X, camPosition.Y, camPosition.Z, 1.0f);

            //Вершины
            GL.EnableClientState(EnableCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, waterVBO);
            GL.VertexAttribPointer(vertexAttributeId, 3, VertexAttribPointerType.Float, false, Vertex.Stride, 0); //Говорим компьютеру использовать буфер вершин
			
            //Текстура
            GL.BindTexture(TextureTarget.Texture2D, fbo.refractionTexture);
            GL.Uniform1(refractionTextureUniformId, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, waterTBO);
            GL.VertexAttribPointer(texCoordAttributeId, 3, VertexAttribPointerType.Float, false, Vertex2.Stride, 0); //Говорим компьютеру использовать буфер вершин

            //Отрисовка
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, waterEBO);
            GL.DrawElements(BeginMode.Triangles, waterIndices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero); //Рисуем элементы в порядке, заданном Indices

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(vertexAttributeId);
            GL.DisableVertexAttribArray(normalAttributeId);
            GL.DisableVertexAttribArray(texCoordAttributeId);

            GL.UseProgram(0);
              
            //viewTexture(fbo.refractionDepthTexture, fbo.REFRACTION_WIDTH, fbo.REFRACTION_HEIGHT);

            //glControl выводит всё что мы нарисовали ранее
            glControl.SwapBuffers();
        }

        //Рендер сцены
        private void renderScene() {
            //Треугольник
			time++;
            GL.PushMatrix();
            
            GL.Translate(0.0f, -10.0f, 0.0f);
            GL.Rotate(time * 1.0f, 0.0f, 1.0f, 0.0f);
            GL.Rotate(90, 1.0f, 0.0f, 0.0f);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 15.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(15.0f, 0.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(-15.0f, 0.0f, 0.0f);
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.End();

            GL.PopMatrix();


            //Бассейн
            GL.PushMatrix();

            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, poolTexture);

            GL.Translate(-SCENE_SIDE / 2.0f, 3.0f, -SCENE_SIDE / 2.0f);

            GL.Begin(BeginMode.Quads);

            float T_UP = 1.0f;

            GL.TexCoord2(0.0f, T_UP);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, -poolDepth, 0.0f);
            GL.TexCoord2(T_UP, 0.0f);
            GL.Vertex3(SCENE_SIDE, -poolDepth, 0.0f);
            GL.TexCoord2(T_UP, T_UP);
            GL.Vertex3(SCENE_SIDE, 0.0f, 0.0f);

            GL.TexCoord2(0.0f, T_UP);
            GL.Vertex3(SCENE_SIDE, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(SCENE_SIDE, -poolDepth, 0.0f);
            GL.TexCoord2(T_UP, 0.0f);
            GL.Vertex3(SCENE_SIDE, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, T_UP);
            GL.Vertex3(SCENE_SIDE, 0.0f, SCENE_SIDE);

            GL.TexCoord2(0.0f, T_UP);
            GL.Vertex3(SCENE_SIDE, 0.0f, SCENE_SIDE);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(SCENE_SIDE, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, 0.0f);
            GL.Vertex3(0.0f, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, T_UP);
            GL.Vertex3(0.0f, 0.0f, SCENE_SIDE);

            GL.TexCoord2(0.0f, T_UP);
            GL.Vertex3(0.0f, 0.0f, SCENE_SIDE);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, 0.0f);
            GL.Vertex3(0.0f, -poolDepth, 0.0f);
            GL.TexCoord2(T_UP, T_UP);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            
            GL.TexCoord2(0.0f, T_UP);
            GL.Vertex3(0.0f, -poolDepth, 0.0f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, 0.0f);
            GL.Vertex3(SCENE_SIDE, -poolDepth, SCENE_SIDE);
            GL.TexCoord2(T_UP, T_UP);
            GL.Vertex3(SCENE_SIDE, -poolDepth, 0.0f);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
        }
        

        //Чтобы показывать, что в буфере преломлений
        private void viewTexture(int texture, int width, int height) {
            GL.Disable(EnableCap.DepthTest);
            GL.PushMatrix();
            GL.LoadIdentity();
            float ratio = (float) width / (float)height;
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Scale(0.25f, 0.25f, 0.25f);
            GL.Translate(-1.0f, 0.0f, 0.0f);
            //GL.Rotate(180, 0, 0, 1);
            
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, -1.0f);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, -1.0f);  

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(ratio, 0.0f, -1.0f);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(ratio, 1.0f, -1.0f);
            
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
            GL.Enable(EnableCap.DepthTest);
        }

        //Метод, вызываемый при изменении размера окна
        private void glControl_Resize(object sender, EventArgs e) {  
            GL.Viewport(0, 0, glControl.Width, glControl.Height); //Устанавливаем новые координаты, где нужно отрисовывать картинку

            //Матрица проекции тоже меняется
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl.Width / (float)glControl.Height, 0.1f, 6000);
            //Устанавливаем матрицу проекции, полученную выше
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
        } 
    }
}
