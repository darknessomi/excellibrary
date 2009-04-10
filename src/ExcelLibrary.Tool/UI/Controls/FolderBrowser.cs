using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QiHe.CodeLib
{
    public partial class FolderBrowser : UserControl
    {
        public FolderBrowser()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return labelCaption.Text; }
            set { labelCaption.Text = value; }
        }

        public string ButtonText
        {
            get { return buttonBrowse.Text; }
            set { buttonBrowse.Text = value; }
        }

        public bool ReadOnly
        {
            get { return textBoxFolderPath.ReadOnly; }
            set
            {
                textBoxFolderPath.ReadOnly = value;
                folderBrowserDialog.ShowNewFolderButton = !value;
            }
        }

        public string FolderPath
        {
            get { return textBoxFolderPath.Text; }
            set
            {
                textBoxFolderPath.Text = value;
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(FolderPath))
            {
                folderBrowserDialog.SelectedPath = FolderPath;
            }
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void FolderBrowser_Layout(object sender, LayoutEventArgs e)
        {
            textBoxFolderPath.Left = labelCaption.Right;
            buttonBrowse.Left = this.Width - buttonBrowse.Width - 2;
            int width = buttonBrowse.Left - labelCaption.Right - 2;
            textBoxFolderPath.Width = width > 0 ? width : 0;
        }

        public event EventHandler FolderPathChanged;

        private void textBoxFolderPath_TextChanged(object sender, EventArgs e)
        {
            if (FolderPathChanged != null)
            {
                FolderPathChanged(sender, e);
            }
        }
    }
}
