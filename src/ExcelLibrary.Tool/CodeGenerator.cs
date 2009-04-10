using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeGen;

namespace ExcelLibrary.Tool
{
    public class CodeGenerator
    {
        public string NamespaceName;
        public Dictionary<string, Record> AllRecords;

        public CodeGenerator(Dictionary<string, Record> allRecords, string @namespace)
        {
            NamespaceName = @namespace;
            AllRecords = allRecords;
        }

        public void GenCode(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            foreach (Record record in AllRecords.Values)
            {
                GenRecordCode(directory, record);
            }
        }

        public void GenRecordCode(string directory, Record record)
        {
            string subdir = directory;
            if (record.Category != null)
            {
                subdir = Path.Combine(directory, record.Category);
                if (!Directory.Exists(subdir))
                {
                    Directory.CreateDirectory(subdir);
                }
            }
            string nsName = NamespaceName + "." + Path.GetDirectoryName(record.Category);
            Namespace ns = new Namespace(nsName);
            Class cs = ExcelRecord.BuildClass(ns, record, AllRecords);
            if (cs != null)
            {
                string file = Path.Combine(subdir, record.Name + ".cs");
                CodeWriter writer = new CodeWriter(file);
                ns.AddClass(cs);
                ns.Output(writer);
                writer.Close();
            }
        }
    }
}
