using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace ExcelLibrary.Tool
{
	public class Settings : AppSettings
	{
		public static Settings Default = WinApp.LoadConfig<Settings>();

		public static void LoadToDefault(string file)
		{
			Settings settings = WinApp.LoadConfig<Settings>(file);
			Default.DataFolder = settings.DataFolder;
			Default.CodeFolder = settings.CodeFolder;
			Default.UpdateControls();
		}

		private string datafolder;

		public string DataFolder
		{
			get
			{
				return datafolder;
			}
			set
			{
				datafolder = value;
			}
		}

		private string codefolder;

		public string CodeFolder
		{
			get
			{
				return codefolder;
			}
			set
			{
				codefolder = value;
			}
		}

	}
}
