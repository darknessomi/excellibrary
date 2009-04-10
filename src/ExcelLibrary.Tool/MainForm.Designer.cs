namespace ExcelLibrary.Tool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserCode = new QiHe.CodeLib.FolderBrowser();
            this.buttonGenCode = new System.Windows.Forms.Button();
            this.folderBrowserData = new QiHe.CodeLib.FolderBrowser();
            this.SuspendLayout();
            // 
            // folderBrowserCode
            // 
            this.folderBrowserCode.ButtonText = "Browse";
            this.folderBrowserCode.Caption = "Code Folder:";
            this.folderBrowserCode.FolderPath = "";
            this.folderBrowserCode.Location = new System.Drawing.Point(22, 97);
            this.folderBrowserCode.Name = "folderBrowserCode";
            this.folderBrowserCode.ReadOnly = false;
            this.folderBrowserCode.Size = new System.Drawing.Size(513, 33);
            this.folderBrowserCode.TabIndex = 0;
            // 
            // buttonGenCode
            // 
            this.buttonGenCode.Location = new System.Drawing.Point(225, 166);
            this.buttonGenCode.Name = "buttonGenCode";
            this.buttonGenCode.Size = new System.Drawing.Size(75, 23);
            this.buttonGenCode.TabIndex = 1;
            this.buttonGenCode.Text = "GenCode";
            this.buttonGenCode.UseVisualStyleBackColor = true;
            this.buttonGenCode.Click += new System.EventHandler(this.buttonGenCode_Click);
            // 
            // folderBrowserData
            // 
            this.folderBrowserData.ButtonText = "Browse";
            this.folderBrowserData.Caption = "Date Folder:";
            this.folderBrowserData.FolderPath = "";
            this.folderBrowserData.Location = new System.Drawing.Point(22, 36);
            this.folderBrowserData.Name = "folderBrowserData";
            this.folderBrowserData.ReadOnly = false;
            this.folderBrowserData.Size = new System.Drawing.Size(513, 33);
            this.folderBrowserData.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 247);
            this.Controls.Add(this.buttonGenCode);
            this.Controls.Add(this.folderBrowserData);
            this.Controls.Add(this.folderBrowserCode);
            this.Name = "MainForm";
            this.Text = "Record Code Generate Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private QiHe.CodeLib.FolderBrowser folderBrowserCode;
        private System.Windows.Forms.Button buttonGenCode;
        private QiHe.CodeLib.FolderBrowser folderBrowserData;
    }
}

