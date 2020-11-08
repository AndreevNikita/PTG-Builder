using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PTG_Builder
{
	static class Program
	{
		public static MainWindow mainWindow;
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			mainWindow = new MainWindow();
			BinaryFormatter formatter = new BinaryFormatter();

			if(File.Exists("texture.ptg")) {
				using (FileStream fs = new FileStream("texture.ptg", FileMode.Open)) {
					mainWindow.ptgTex = (PTGTex)formatter.Deserialize(fs);
					mainWindow.updateLayers();
				}
			} else {
				mainWindow.ptgTex = new PTGTex("texture");
			}

			Application.Run(mainWindow);
		
			using (FileStream fs = new FileStream("texture.ptg", FileMode.OpenOrCreate)) {
				formatter.Serialize(fs, mainWindow.ptgTex);
				fs.Close();
			}
		}
	}
}
