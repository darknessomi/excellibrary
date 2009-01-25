using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QiHe.CodeLib
{
    /// <summary>
    /// XmlFile load and save object from and to xml file;
    /// </summary>	
    public class XmlData<DataType>
    {
        /// <summary>
        /// Loads the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <returns></returns>
        public static DataType Load(string xmlfile)
        {
            DataType data;
            Type datatype = typeof(DataType);
            XmlSerializer mySerializer = new XmlSerializer(datatype);
            if (File.Exists(xmlfile))
            {
                using (XmlTextReader reader = new XmlTextReader(xmlfile))
                {
                    data = (DataType)mySerializer.Deserialize(reader);
                }
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return default(DataType);
            }
            return data;
        }
        /// <summary>
        /// Loads the specified xmldata.
        /// </summary>
        /// <param name="xmldata">The xmldata.</param>
        /// <returns></returns>
        public static DataType Load(Stream xmldata)
        {
            using (XmlTextReader reader = new XmlTextReader(xmldata))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(DataType));
                return (DataType)mySerializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// Loads the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public static DataType Load(string xmlfile, string root)
        {
            DataType data;
            Type datatype = typeof(DataType);
            XmlRootAttribute rootattr = new XmlRootAttribute(root);
            XmlSerializer mySerializer = new XmlSerializer(datatype, rootattr);
            if (File.Exists(xmlfile))
            {
                XmlTextReader reader = new XmlTextReader(xmlfile);
                data = (DataType)mySerializer.Deserialize(reader);
                reader.Close();
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return default(DataType);
            }
            return data;
        }
        /// <summary>
        /// Saves the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <param name="data">The data.</param>
        public static void Save(string xmlfile, DataType data)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
    }
    /// <summary>
    /// XmlFile load and save object from and to xml file;
    /// </summary>	
    public class XmlFile
    {
        /// <summary>
        /// Loads the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <param name="datatype">The datatype.</param>
        /// <returns></returns>
        public static object Load(string xmlfile, Type datatype)
        {
            object data = null;
            XmlSerializer mySerializer = new XmlSerializer(datatype);
            if (File.Exists(xmlfile))
            {
                XmlTextReader reader = new XmlTextReader(xmlfile);
                data = mySerializer.Deserialize(reader);
                reader.Close();
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return null;
            }
            return data;
        }
        /// <summary>
        /// Saves the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <param name="data">The data.</param>
        public static void Save(string xmlfile, object data)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
        /// <summary>
        /// Saves the specified xmlfile.
        /// </summary>
        /// <param name="xmlfile">The xmlfile.</param>
        /// <param name="root">The root.</param>
        /// <param name="data">The data.</param>
        public static void Save(string xmlfile, string root, object data)
        {
            XmlRootAttribute rootattr = new XmlRootAttribute(root);
            XmlSerializer mySerializer = new XmlSerializer(data.GetType(), rootattr);
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
    }
    /// <summary>
    /// TxtFile
    /// </summary>
    public class TxtFile
    {
        /// <summary>
        /// Creates the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        public static void Create(string file)
        {
            StreamWriter sw = File.CreateText(file);
            sw.Close();
        }
        /// <summary>
        /// Reads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static string Read(string file)
        {
            StreamReader sr = File.OpenText(file);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
        //Encodings: "ASCII","UTF-8","Unicode","GB2312","GB18030"
        /// <summary>
        /// Reads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string Read(string file, string encoding)
        {
            return Read(file, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// Reads the lines.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static List<string> ReadLines(string file, string encoding)
        {
            return ReadLines(file, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// Reads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string Read(string file, Encoding encoding)
        {
            StreamReader sr = new StreamReader(file, encoding);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
        /// <summary>
        /// Reads the lines.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static List<string> ReadLines(string file, Encoding encoding)
        {
            StreamReader reader = new StreamReader(file, encoding);
            List<string> lines = new List<string>();
            string line = reader.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = reader.ReadLine();
            }
            reader.Close();
            return lines;
        }
        /// <summary>
        /// Writes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        public static void Write(string file, string text)
        {
            StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8);
            sw.Write(text);
            sw.Close();
        }
        /// <summary>
        /// Writes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        public static void Write(string file, string text, Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(file, false, encoding);
            sw.Write(text);
            sw.Close();
        }
        /// <summary>
        /// Appends the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        public static void Append(string file, string text)
        {
            StreamWriter sw = File.AppendText(file);
            sw.WriteLine(text);
            sw.Close();
        }
        /// <summary>
        /// Appends the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        public static void Append(string file, string text, Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(file, true, encoding);
            sw.Write(text);
            sw.Close();
        }

        /// <summary>
        /// Gets the text from current position of reader to specified tag.
        /// tag should occupy just one line.
        /// if tag is null then read to the end of file.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>the text from current position of reader to specified tag, tag is not included.</returns>
        public static string GetText(TextReader reader, string tag)
        {
            if (tag == null)
            {
                return reader.ReadToEnd();
            }
            StringBuilder text = new StringBuilder();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line == tag)
                {
                    int n = Environment.NewLine.Length;
                    if (text.Length > n)
                    {
                        text.Remove(text.Length - n, n);
                    }
                    return text.ToString();
                }
                text.Append(line + Environment.NewLine);
                line = reader.ReadLine();
            }
            return null;
        }


        /// <summary>
        /// Gets the text between two tags in file.
        /// each tag should occupy just one line.
        /// </summary>
        /// <param name="file">The file in UTF-8.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <returns>the text between two tags in file, or null if startTag is not found.</returns>
        public static string GetText(string file, string startTag, string endTag)
        {
            StreamReader reader = File.OpenText(file);
            StringBuilder text = new StringBuilder();
            bool found = false;
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line == endTag) { break; }
                if (found) { text.AppendLine(line); }
                if (line == startTag) { found = true; }
                line = reader.ReadLine();
            }
            reader.Close();
            if (found) return text.ToString(); return null;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <param name="lineNum">The line num.</param>
        /// <returns></returns>
        public static string GetText(string file, string startTag, string endTag, out int lineNum)
        {
            StreamReader reader = File.OpenText(file);
            StringBuilder text = new StringBuilder();
            bool found = false;
            int linecounter = 0; lineNum = -1;
            string line = reader.ReadLine();
            while (line != null)
            {
                linecounter++;
                if (line == endTag) { break; }
                if (found) { text.AppendLine(line); }
                if (line == startTag) { found = true; lineNum = linecounter + 1; }
                line = reader.ReadLine();
            }
            reader.Close();
            if (found) return text.ToString(); return null;
        }

        /// <summary>
        /// Counts the lines and chars.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="lines">The lines.</param>
        /// <param name="chars">The chars.</param>
        public static void CountLinesAndChars(string file, out int lines, out int chars)
        {
            StreamReader reader = File.OpenText(file);
            lines = 0;
            chars = 0;
            string line = reader.ReadLine();
            while (line != null)
            {
                lines++;
                chars += line.Length;
                line = reader.ReadLine();
            }
            reader.Close();
        }
    }
    /// <summary>
    /// BinFile
    /// </summary>
    public class BinFile
    {
        /// <summary>
        /// Reads the specified datafile.
        /// </summary>
        /// <param name="datafile">The datafile.</param>
        /// <returns></returns>
        public static object Read(string datafile)
        {
            if (File.Exists(datafile))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(datafile, FileMode.Open, FileAccess.Read, FileShare.Read);
                object obj = formatter.Deserialize(stream);
                stream.Close();
                return obj;
            }
            else
            {
                throw new FileNotFoundException(datafile);
            }
        }
        /// <summary>
        /// Writes the specified datafile.
        /// </summary>
        /// <param name="datafile">The datafile.</param>
        /// <param name="obj">The obj.</param>
        public static void Write(string datafile, object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(datafile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
    }
}
