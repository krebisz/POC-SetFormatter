namespace SetFormatterWebClient
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
            this.FilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btMessageFileBrowse = new System.Windows.Forms.Button();
            this.SetFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.NotificationList = new System.Windows.Forms.ListBox();
            this.SendMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FilePath
            // 
            this.FilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilePath.Location = new System.Drawing.Point(39, 19);
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Size = new System.Drawing.Size(417, 20);
            this.FilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File";
            // 
            // btMessageFileBrowse
            // 
            this.btMessageFileBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMessageFileBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btMessageFileBrowse.Location = new System.Drawing.Point(467, 20);
            this.btMessageFileBrowse.Name = "btMessageFileBrowse";
            this.btMessageFileBrowse.Size = new System.Drawing.Size(67, 20);
            this.btMessageFileBrowse.TabIndex = 2;
            this.btMessageFileBrowse.Text = "Browse";
            this.btMessageFileBrowse.UseVisualStyleBackColor = true;
            this.btMessageFileBrowse.Click += new System.EventHandler(this.BrowseFile_Click);
            // 
            // SetFileDialog
            // 
            this.SetFileDialog.AddExtension = false;
            this.SetFileDialog.DefaultExt = "xml";
            this.SetFileDialog.Filter = "XmlInterchange files|*.xml";
            this.SetFileDialog.RestoreDirectory = true;
            // 
            // NotificationList
            // 
            this.NotificationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotificationList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.NotificationList.FormattingEnabled = true;
            this.NotificationList.HorizontalScrollbar = true;
            this.NotificationList.Location = new System.Drawing.Point(13, 80);
            this.NotificationList.Name = "NotificationList";
            this.NotificationList.Size = new System.Drawing.Size(596, 251);
            this.NotificationList.TabIndex = 7;
            this.NotificationList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.NotificationList_DrawItem);
            // 
            // SendMessage
            // 
            this.SendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.SendMessage.Location = new System.Drawing.Point(541, 20);
            this.SendMessage.Name = "SendMessage";
            this.SendMessage.Size = new System.Drawing.Size(67, 20);
            this.SendMessage.TabIndex = 6;
            this.SendMessage.Text = "Format";
            this.SendMessage.UseVisualStyleBackColor = true;
            this.SendMessage.Click += new System.EventHandler(this.FormatSets_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.SendMessage);
            this.Controls.Add(this.NotificationList);
            this.Controls.Add(this.btMessageFileBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilePath);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.MinimumSize = new System.Drawing.Size(640, 450);
            this.Name = "MainForm";
            this.Text = "SetFormatter Client";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btMessageFileBrowse;
        private System.Windows.Forms.OpenFileDialog SetFileDialog;
        private System.Windows.Forms.ListBox NotificationList;
        private System.Windows.Forms.Button SendMessage;
    }
}

