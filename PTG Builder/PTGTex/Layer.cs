using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace PTG_Builder
{
	[Serializable]
	public class Layer {

		public string name;
		public string function = "";
		public enum LayerType { COLOR, HEIGHT };
		LayerType layerType;
		public int overlayType = 0;

		public Dictionary<string, float> variables = new Dictionary<string, float>();
		public List<string> changedVars = new List<string>();

		public bool isVisible = true;

		public Layer(string name, LayerType layerType) {
			this.name = name;
			this.layerType = layerType;
		}

		public string getCode(PTGTex owner) {
			StringBuilder code = new StringBuilder();
			foreach(var pair in variables) {
				code.Append("uniform float " + owner.getUVariableFullName(pair.Key, name) + ";\n");
			}
			string declaration = "";
			switch(layerType) { 
				case LayerType.COLOR: declaration = "vec4 " + owner.getLayerColorFunctionFullName(name) + "(float x, float z)";  break;
				case LayerType.HEIGHT: declaration = "float " + owner.getLayerYFunctionFullName(name) + "(float x, float z)"; break;
			}

			code.Append(declaration + " {\n");
			foreach(string variable in variables.Keys) {
				code.Append("float " + variable + " = " + owner.getUVariableFullName(variable, name) + ";\n");
			}
			if(function.Length > 0) {
				code.Append(function);
			} else {
				switch(layerType) { 
					case LayerType.COLOR: code.Append("return vec4(0.0, 0.0, 0.0, 1.0);");  break;
					case LayerType.HEIGHT: code.Append("return 0.0;"); break;
				}
			}
			code.Append("\n}\n");
			
			return code.ToString();
		}

		public void updateUniforms(PTGTex owner, int program, bool all = false) {
			GL.UseProgram(program);
			if(!all) {
				foreach(string name in changedVars) {
					if(variables.ContainsKey(name)) {
						GL.Uniform1(GL.GetUniformLocation(program, owner.getUVariableFullName(name, this.name)), variables[name]);
					}
				}
				changedVars.Clear();
			} else {
				foreach(var pair in variables) {
					 GL.Uniform1(GL.GetUniformLocation(program, owner.getUVariableFullName(pair.Key, this.name)), pair.Value);
				}
			}
		}
	}
}
