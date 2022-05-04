using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SetFormatter
{
    public partial class MainForm : Form
    {
        public static List<Element> Elements;

        public static string fileInPath;
        public static string fileOutPath;
        public static string fileExcludePath;
        public static string fileArchivePath;
        private string Argument = string.Empty;

        public MainForm(string[] args)
        {
            InitializeComponent();

            //fileInPath = ConfigurationManager.AppSettings["fileDirectoryIn"];
            //fileOutPath = ConfigurationManager.AppSettings["fileDirectoryOut"];

            //if (args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            //{
            //    Argument = args[0];
            //}
        }

        #region EVENTS

        private void MainForm_Load(object sender, EventArgs e)
        {
            //if (Argument != string.Empty)
            //{
            //FormatSets();
            //}

            //TreeViewForm treeForm = new TreeViewForm();
            TreeForm treeForm = new TreeForm();
            treeForm.ShowDialog();
        }

        private void FormatSets_Click(object sender, EventArgs e)
        {
            //FormatSets();
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            //if (SetFileDialog.ShowDialog() != DialogResult.OK) return;
            //FilePath.Text = SetFileDialog.FileName;
        }
        private void NotificationList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            var notification = NotificationList.Items[e.Index] as Notification;

            if (notification == null) return;

            var color = Color.DarkGray;

            switch (notification.NotificationType)
            {
                case Notification.MessageType.Error:
                    color = Color.DarkRed;
                    break;

                case Notification.MessageType.Warning:
                    color = Color.DarkOrange;
                    break;

                case Notification.MessageType.Confirmation:
                    color = Color.DarkGreen;
                    break;
            }

            e.Graphics.DrawString(NotificationList.Items[e.Index].ToString(), new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold), new SolidBrush(color), e.Bounds);
        }

        #endregion

        //private void FormatSets()
        //{
        //    AddNotification(new Notification("Started", Notification.MessageType.Information));
        //    string[] Filenames = RetrieveFiles();

        //    if (object.Equals(Filenames, null))
        //    {
        //        AddNotification(new Notification("No Files Found", Notification.MessageType.Warning));
        //    }
        //    else
        //    {
        //        TransformFiles(Filenames);
        //    }

        //    AddNotification(new Notification("Finished", Notification.MessageType.Information));

        //    if (!string.IsNullOrEmpty(Argument))
        //    {
        //        Thread.Sleep(5000);
        //        this.Close();
        //    }
        //}

        //private string[] RetrieveFiles()
        //{
        //    AddNotification(new Notification("Retrieving Files", Notification.MessageType.Information));

        //    string[] Filenames;

        //    //if (!string.IsNullOrEmpty(Argument))
        //    //{
        //    if (!ValidateFileDirectory())
        //    {
        //        Filenames = null;
        //    }

        //    string[] excludeFilePath = (ConfigurationManager.AppSettings["fileDirectoryExclude"]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    for (int i = 0; i < excludeFilePath.Length; i++)
        //    {
        //        excludeFilePath[i] = excludeFilePath[i].Trim();
        //    }

        //    Filenames = IOHelper.GetFileNames(fileInPath, excludeFilePath, SearchOption.AllDirectories).ToArray();
        //    //}
        //    //else
        //    //{
        //    //if (!ValidateFileName())
        //    //{
        //    //    Filenames = null;
        //    //}

        //    //Filenames = new string[1];
        //    //Filenames[0] = SetFileDialog.FileName.Trim();
        //    //}

        //    return Filenames;
        //}

        //private void TransformFiles(string[] Filenames)
        //{
        //    foreach (string fileName in Filenames)
        //    {
        //        AddNotification(new Notification(String.Format("Formatting File {0}", fileName), Notification.MessageType.Information));

        //        try
        //        {
        //            FormatFile(fileName);
        //            //File.Delete(fileName);
        //        }
        //        catch (Exception ex)
        //        {
        //            AddNotification(new Notification(String.Format("Format Error: {0}", ex.Message), Notification.MessageType.Error));
        //        }
        //    }

        //    AddNotification(new Notification("Format Done", Notification.MessageType.Confirmation));
        //}

        //public static string FormatFile(string fileName, int outputMode = 1)
        //{
        //    string fileOutput;
        //    string fileNameOut;
        //    string fileContentOut;

        //    using (Stream fileStream = IOHelper.GetFileStream(fileName))
        //    {
        //        StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
        //        string fileContent = streamReader.ReadToEnd();
        //        Elements = FormatCollection(fileContent);
        //        string formattedContent = FormatContent(Elements);

        //        fileNameOut = IOHelper.WriteFile(formattedContent);
        //        fileContentOut = IOHelper.ReadFileAsString(fileNameOut);
        //    }

        //    if (outputMode == 1)
        //    {
        //        fileOutput = fileNameOut;
        //    }
        //    else
        //    {
        //        fileOutput = fileContentOut;
        //    }

        //    return fileOutput;
        //}

        //public static List<Element> FormatCollection(string fileContent)
        //{
        //    FormatSet formatSet = new FormatSet();
        //    List<Element> set = formatSet.CreateTree(fileContent, new char[] { '{', '}' }, ',');
        //    List<Element> formattedSet = (set).GroupBy(p => new { p.Id, p.Parent }).Select(g => g.First()).OrderBy(e => (e.Parent, e.Id)).ToList();
        //    formatSet.OptimizeIds();
        //    return formattedSet;
        //}

        //public static string FormatContent(List<Element> elements)
        //{
        //    string formattedContent = string.Empty;

        //    foreach (Element e in elements)
        //    {
        //        formattedContent = formattedContent + "Id:" + e.Id + " Parent:" + e.Parent + " Level:" + e.Level + " |" + e.Name + "\r\n";
        //    }

        //    return formattedContent;
        //}

        #region VALIDATION

        //private bool ValidateFileDirectory()
        //{
        //    if (!Directory.Exists(fileInPath))
        //    {
        //        AddNotification(new Notification("Invalid File Path", Notification.MessageType.Warning));
        //        return false;
        //    }

        //    return true;
        //}

        //private bool ValidateFileName()
        //{
        //    if (String.IsNullOrEmpty(SetFileDialog.FileName.Trim()))
        //    {
        //        AddNotification(new Notification("Invaild File Name", Notification.MessageType.Warning));
        //        return false;
        //    }

        //    return true;
        //}

        #endregion VALIDATION

        //public void AddNotification(Notification notification)
        //{
        //    NotificationList.Items.Add(notification);
        //}


    }
}