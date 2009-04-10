using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace QiHe.CodeLib
{
    /// <summary>
    /// Windows Application Library
    /// </summary>
    public class WinApp
    {
        public static string AppPath
        {
            get
            {
                return Application.StartupPath + Path.DirectorySeparatorChar;
            }
        }
        public static string ConfigFile
        {
            get
            {
                return Path.ChangeExtension(Application.ExecutablePath, ".settings");
            }
        }

        public static T LoadConfig<T>()
        {
            return LoadConfig<T>(ConfigFile);
        }

        public static T LoadConfig<T>(string xmlFile)
        {
            try
            {
                T data = XmlData<T>.Load(xmlFile);
                if (data != null)
                {
                    return data;
                }
            }
            catch { }
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch { }
            return default(T);
        }
        public static void SaveConfig<T>(T data)
        {
            XmlData<T>.Save(ConfigFile, data);
        }

        public static void SaveConfig(object data)
        {
            XmlFile.Save(ConfigFile, data);
        }
    }
}
