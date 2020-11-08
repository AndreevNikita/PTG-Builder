using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTG_Builder
{
	class CodePreprocessor {
		string[] mainCodes;

		Dictionary<string, string> codeParts = new Dictionary<string, string>();

		public CodePreprocessor(params string[] mainCodes) {
			this.mainCodes = mainCodes;
		}

		public void addPart(string code, string name) {
			codeParts.Add(name, code);
		}

		const string defPartPattern = @"<\s*part\s+def\s+"".+""\s*>";
		const string endPartPattern = @"<\s*part\s+end\s*>";
		const string putPartPattern = @"<\s*part\s+put\s+"".+""\s*>";

		public void addPart(string code) {
			try {
				Regex defRegex = new Regex(defPartPattern);
				Regex endRegex = new Regex(endPartPattern);
				foreach (Match startMatch in defRegex.Matches(code)) {
					string name = new Regex(@"[^""].+[^""]").Matches(new Regex(@""".+""").Matches(startMatch.Value)[0].Value)[0].Value;
					Console.WriteLine("Found part " + name);
					MatchCollection endMatches = endRegex.Matches(code, startMatch.Index + startMatch.Length);
					int partStartPos = startMatch.Index + startMatch.Length;
					string part = endMatches.Count > 0 ? code.Substring(partStartPos, endMatches[0].Index - partStartPos) : code.Substring(partStartPos);
					Console.WriteLine(part);
					addPart(part, name);
				}
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		public string[] process() {
			try {
				string[] result = new string[mainCodes.Length];
				for (int index = 0; index < result.Length; index++) {
					string code = mainCodes[index];
					Regex putRegex = new Regex(putPartPattern);
					MatchCollection matches;
					while ((matches = putRegex.Matches(code)).Count != 0) {
						Match putMatch = matches[0];
						int startIndex = putMatch.Index;
						string name = new Regex(@"[^""].+[^""]").Matches(new Regex(@""".+""").Matches(putMatch.Value)[0].Value)[0].Value;
						Console.WriteLine("Put part " + name);
						code = code.Remove(startIndex, putMatch.Length);
						code =  code.Insert(startIndex, "//code part \"" + name + "\"" + (codeParts.ContainsKey(name) ? "\n" + codeParts[name] : " - not found\n"));
					}
					result[index] = code;
				}
				return result;
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
				return null;
			}
		}
		
	}
}
