using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace PTG_Builder {
	[Serializable]
	public class PTGTex {
		public string name;

		public List<Layer> reliefLayers = new List<Layer>();
		public List<Layer> textureLayers = new List<Layer>();

		public PTGTex(string name) {
			this.name = name;
		}

		public string getLayerYFunctionFullName(string layerName) {
			return "_ptg_" + name + "_" + layerName + "_y";
		}

		public string getLayerColorFunctionFullName(string layerName) {
			return "_ptg_" + name + "_" + layerName + "_color";
		}

		public string getUVariableFullName(string varName, string layerName) {
			return "_ptg_u_" + name + "_" + layerName + "_" + varName;
		}

		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long currentTimeMillis() {
			return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}

		public void updateUniforms(int program, bool all = false) {
			GL.Uniform1(GL.GetUniformLocation(program, "t"), (float)(currentTimeMillis() % 10000) / 10000.0f);
			
			foreach (Layer layer in reliefLayers)
				layer.updateUniforms(this, program, all);
			foreach (Layer layer in textureLayers)
				layer.updateUniforms(this, program, all);

		}

		string[] overlayFunctions = {
			"PTG_ADD", "PTG_SUBTR", "PTG_MUL", "PTG_MIN", "PTG_MAX"
		};

		public string getCode() {
			StringBuilder sb = new StringBuilder();

			sb.Append("//Visiblity vars\n");

			foreach(Layer layer in reliefLayers) {
				sb.Append("uniform int " + getUVariableFullName("ptg_visiblity", layer.name) + ";\n");
			}

			foreach(Layer layer in textureLayers) {
				sb.Append("uniform int " + getUVariableFullName("ptg_visiblity", layer.name) + ";\n");
			}

			foreach(Layer layer in reliefLayers) {
				sb.Append(layer.getCode(this));
			}

			foreach(Layer layer in textureLayers) {
				sb.Append(layer.getCode(this));
			}

			//Y Function
			sb.Append("float _ptg_" + name + "_y(float x, float z) {\n");
			//sb.Append("vec3 result = vec3(0.0, 1.0, 0.0);");
			sb.Append("float result = 0.0;\n");
			//Добавляем слои
			for(int index = 0; index < reliefLayers.Count; index++) {
				sb.Append("if(" + getUVariableFullName("ptg_visiblity", reliefLayers[index].name) + " != 0)");
				sb.Append("result = " + overlayFunctions[reliefLayers[index].overlayType] + "(result, " + getLayerYFunctionFullName(reliefLayers[index].name) + "(x, z));\n");
			}

			sb.Append("return result;\n");
			sb.Append("}\n");

			//Color Function
			sb.Append("vec4 _ptg_" + name + "_color(float x, float z) {\n");
			//sb.Append("vec3 result = vec3(0.0, 1.0, 0.0);");
			sb.Append("vec4 result = vec4(0.0, 0.0, 0.0, 0.0);\n");
			//Добавляем слои
			for(int index = 0; index < textureLayers.Count; index++) {
				sb.Append("if(" + getUVariableFullName("ptg_visiblity", textureLayers[index].name) + " != 0)\n");
				sb.Append("result = " + overlayFunctions[textureLayers[index].overlayType] + "(result, " + getLayerColorFunctionFullName(textureLayers[index].name) + "(x, z));\n");
			}

			sb.Append("return result;\n");
			sb.Append("}\n");

			return sb.ToString();
		}

	}
}
