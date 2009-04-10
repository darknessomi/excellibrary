using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace QiHe.CodeLib
{
    /// <summary>
    /// FileHelper
    /// </summary>
    public class FileHelper
    {
        public static string StripExtension(string name)
        {
            int indexofdot = name.LastIndexOf('.');
            if (indexofdot == -1)
            {
                return name;
            }
            else
            {
                return name.Substring(0, indexofdot);
            }
        }

        public static bool IsAbsolutePath(string path)
        {
            return path != null && path.IndexOf(':') > 0;
        }

        public static string GetDirectory(string file)
        {
            FileInfo fi = new FileInfo(file);
            return fi.Directory.FullName;
        }

        public static void CreateDirectoryIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void DeleteFileIfExists(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public static bool CopyFileIfNewer(string srcPath, string destPath)
        {
            if (File.Exists(srcPath))
            {
                if (!File.Exists(destPath))
                {
                    File.Copy(srcPath, destPath);
                    return true;
                }
                else if (File.GetLastWriteTime(srcPath) > File.GetLastWriteTime(destPath))
                {
                    File.Copy(srcPath, destPath, true);
                    return true;
                }
            }
            return false;
        }

        public static string CompareFolder(string dir1, string dir2, bool includeSubFolders)
        {
            if (includeSubFolders)
            {
                StringBuilder text = new StringBuilder();
                StringWriter writer = new StringWriter(text);
                CompareFolder(dir1, dir2, writer);
                return text.ToString();
            }
            else
            {
                StringBuilder equal = new StringBuilder();
                StringBuilder notequal = new StringBuilder();
                DirectoryInfo di1 = new DirectoryInfo(dir1);
                DirectoryInfo di2 = new DirectoryInfo(dir2);
                foreach (FileInfo fi1 in di1.GetFiles())
                {
                    foreach (FileInfo fi2 in di2.GetFiles())
                    {
                        if (fi1.Name == fi2.Name)
                        {
                            if (CompareFile(fi1.FullName, fi2.FullName))
                            {
                                equal.AppendLine(fi1.Name);
                            }
                            else
                            {
                                notequal.AppendLine(fi1.Name);
                            }
                        }
                    }
                }
                StringBuilder text = new StringBuilder();
                text.AppendLine("Equal Files:");
                text.AppendLine(equal.ToString());
                text.AppendLine("Not Equal Files:");
                text.AppendLine(notequal.ToString());
                return text.ToString();
            }
        }

        /// <summary>
        /// Compare two files by content
        /// </summary>
        /// <param name="firstfile"></param>
        /// <param name="secondfile"></param>
        /// <returns>true if equal, false if not equal</returns>
        public static bool CompareFile(string firstfile, string secondfile)
        {
            using (FileStream input = new FileStream(firstfile, FileMode.Open, FileAccess.Read))
            {
                using (FileStream output = new FileStream(secondfile, FileMode.Open, FileAccess.Read))
                {
                    if (input.Length != output.Length) return false;
                    while (true)
                    {
                        int inbyte = input.ReadByte();
                        int outbyte = output.ReadByte();
                        if (inbyte != outbyte) return false;
                        if (inbyte == -1) break;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Get relative file paths of all files in folder and subfolders.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetFileList(string folder)
        {
            List<string> files = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(folder);
            GetFileList(dir, null, files);
            return files;
        }

        private static void GetFileList(DirectoryInfo dir, string subpath, List<string> files)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                files.Add(subpath + file.Name);
            }
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                GetFileList(subdir, subpath + subdir.Name + "\\", files);
            }
        }

        public static List<string> GetFileList(string folder, char pathsep, bool includeDirectory)
        {
            List<string> files = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(folder);
            GetFileList(dir, null, files, pathsep, includeDirectory);
            return files;
        }

        private static void GetFileList(DirectoryInfo dir, string subpath, List<string> files, char pathsep, bool includeDirectory)
        {
            if (includeDirectory && subpath != null)
            {
                files.Add(subpath);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                files.Add(subpath + file.Name);
            }
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                GetFileList(subdir, subpath + subdir.Name + pathsep, files, pathsep, includeDirectory);
            }
        }

        public static void GenFileList(string folder, StreamWriter writer)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folder);
                writer.WriteLine(dir.FullName);
                GenFileList(dir, writer, new StringBuilder());
            }
            catch (Exception error)
            {
                writer.WriteLine(error.Message);
            }
        }

        private static void GenFileList(DirectoryInfo dir, StreamWriter writer, StringBuilder textbuilder)
        {
            FileInfo[] files = GetSortedFiles(dir);
            DirectoryInfo[] dirs = GetSortedDirectories(dir);

            string leading = textbuilder.ToString();
            string blank = dirs.Length > 0 ? "│  " : "    ";

            foreach (FileInfo file in files)
            {
                writer.Write(leading);
                writer.Write(blank);
                writer.WriteLine(file.Name);
            }
            if (files.Length > 0)
            {
                writer.Write(leading);
                writer.WriteLine(blank);
            }

            for (int i = 0; i < dirs.Length - 1; i++)
            {
                DirectoryInfo subdir = dirs[i];
                writer.Write(leading);
                writer.Write("├─");
                writer.WriteLine(subdir.Name);

                textbuilder.Append("│  ");
                GenFileList(subdir, writer, textbuilder);
                textbuilder.Remove(textbuilder.Length - 3, 3);
            }

            if (dirs.Length > 0)
            {
                DirectoryInfo subdir = dirs[dirs.Length - 1];
                writer.Write(leading);
                writer.Write("└─");
                writer.WriteLine(subdir.Name);

                textbuilder.Append("    ");
                GenFileList(subdir, writer, textbuilder);
                textbuilder.Remove(textbuilder.Length - 4, 4);
            }
        }

        private static DirectoryInfo[] GetSortedDirectories(DirectoryInfo dir)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            Array.Sort<DirectoryInfo>(dirs,
                delegate(DirectoryInfo ja, DirectoryInfo yi)
                {
                    return ja.Name.CompareTo(yi.Name);
                });
            return dirs;
        }

        private static FileInfo[] GetSortedFiles(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();

            Array.Sort<FileInfo>(files,
                delegate(FileInfo ja, FileInfo yi)
                {
                    return ja.Name.CompareTo(yi.Name);
                });
            return files;
        }

        public static void CompareFolder(string dir1, string dir2, TextWriter writer)
        {
            try
            {
                DirectoryInfo di1 = new DirectoryInfo(dir1);
                DirectoryInfo di2 = new DirectoryInfo(dir2);

                CompareDirectoryInfo(di1, di2, writer);

                writer.WriteLine("比较完成");
            }
            catch (Exception error)
            {
                writer.WriteLine(error.Message);
            }
        }

        private static void CompareDirectoryInfo(DirectoryInfo di1, DirectoryInfo di2, TextWriter writer)
        {
            FileInfo[] files1 = GetSortedFiles(di1);
            FileInfo[] files2 = GetSortedFiles(di2);

            CompareFiles(files1, files2, writer);

            DirectoryInfo[] dirs1 = GetSortedDirectories(di1);
            DirectoryInfo[] dirs2 = GetSortedDirectories(di2);

            CompareDirectories(dirs1, dirs2, writer);
        }

        private static void CompareFiles(FileInfo[] files1, FileInfo[] files2, TextWriter writer)
        {
            int pos1 = 0;
            int pos2 = 0;

            while (pos1 < files1.Length || pos2 < files2.Length)
            {
                FileInfo file1 = pos1 < files1.Length ? files1[pos1] : null;
                FileInfo file2 = pos2 < files2.Length ? files2[pos2] : null;

                string name1 = file1 == null ? null : file1.Name;
                string name2 = file2 == null ? null : file2.Name;

                int nameCompare = CompareName(name1, name2);

                if (nameCompare == 0)
                {
                    if (file1.Length == file2.Length)
                    {
                        if (file1.Length > 100000000)
                        {
                            writer.Write(file1.FullName);
                            writer.WriteLine("    跳过内容比较");
                        }
                        else
                        {
                            if (CompareFile(file1.FullName, file2.FullName) == false)
                            {
                                writer.Write(file1.FullName);
                                writer.WriteLine("    内容不同");
                            }
                        }
                    }
                    else
                    {
                        writer.Write(file1.FullName);
                        writer.WriteLine("    长度不同");
                    }
                    pos1++;
                    pos2++;
                }
                else if (nameCompare < 0)
                {
                    writer.Write(file1.FullName);
                    writer.WriteLine("    无对应文件");
                    pos1++;
                }
                else
                {
                    writer.Write(file2.FullName);
                    writer.WriteLine("    无对应文件");
                    pos2++;
                }
            }
        }

        private static void CompareDirectories(DirectoryInfo[] dirs1, DirectoryInfo[] dirs2, TextWriter writer)
        {
            int pos1 = 0;
            int pos2 = 0;

            while (pos1 < dirs1.Length || pos2 < dirs2.Length)
            {
                DirectoryInfo dir1 = pos1 < dirs1.Length ? dirs1[pos1] : null;
                DirectoryInfo dir2 = pos2 < dirs2.Length ? dirs2[pos2] : null;

                string name1 = dir1 == null ? null : dir1.Name;
                string name2 = dir2 == null ? null : dir2.Name;

                int nameCompare = CompareName(name1, name2);

                if (nameCompare == 0)
                {
                    CompareDirectoryInfo(dir1, dir2, writer);
                    pos1++;
                    pos2++;
                }
                else if (nameCompare < 0)
                {
                    writer.Write(dir1.FullName);
                    writer.WriteLine("    无对应目录");
                    pos1++;
                }
                else
                {
                    writer.Write(dir2.FullName);
                    writer.WriteLine("    无对应目录");
                    pos2++;
                }
            }
        }

        static int CompareName(string name1, string name2)
        {
            if (name1 == null)
            {
                if (name2 == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (name2 == null)
                {
                    return -1;
                }
                else
                {
                    return name1.CompareTo(name2);
                }
            }
        }
    }
}
