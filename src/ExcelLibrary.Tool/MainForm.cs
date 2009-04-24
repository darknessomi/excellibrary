using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QiHe.CodeLib;

namespace ExcelLibrary.Tool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Settings.Default.Set(folderBrowserData, "FolderPath", "DataFolder");
            Settings.Default.Set(folderBrowserCode, "FolderPath", "CodeFolder");
            FormClosing += delegate { Settings.Default.Save(); };
        }

        private void buttonGenCode_Click(object sender, EventArgs e)
        {
            string dataFolder = folderBrowserData.FolderPath;
            Record BiffRecord = XmlData<Record>.Load(Path.Combine(dataFolder, "Record.xml"));
            Record SubRecord = XmlData<Record>.Load(Path.Combine(dataFolder, "SubRecord.xml"));
            Record EscherRecord = XmlData<Record>.Load(Path.Combine(dataFolder, "EscherRecord.xml"));

            Dictionary<string, Record> allRecords = new Dictionary<string, Record>();
            AddRecords(allRecords, BiffRecord);
            AddRecords(allRecords, SubRecord);
            AddRecords(allRecords, EscherRecord);

            CodeGenerator generator = new CodeGenerator(allRecords, "ExcelLibrary");
            generator.GenCode(folderBrowserCode.FolderPath);

            MessageBox.Show(this, "Finished.");
        }

        private static void AddRecords(Dictionary<string, Record> allRecords, Record baseRecord)
        {
            allRecords.Add(baseRecord.Name, baseRecord);
            foreach (Record childRecord in baseRecord.ChildRecords)
            {
                if (childRecord.ChildRecords.Count > 0)
                {
                    AddRecords(allRecords, childRecord);
                }
                else
                {
                    allRecords.Add(childRecord.Name, childRecord);
                }
            }
        }
    }
}
