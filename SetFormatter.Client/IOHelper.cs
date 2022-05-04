using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SetFormatter
{
    public static class IOHelper
    {
        public static string fileDirectoryIn = ConfigurationManager.AppSettings["fileDirectoryIn"];
        public static string fileDirectoryOut = ConfigurationManager.AppSettings["fileDirectoryOut"];
        public static string filePrefix = ConfigurationManager.AppSettings["filePrefix"];
        public static string fileSuffix = ConfigurationManager.AppSettings["fileSuffix"];
        public static string fileExtension = ConfigurationManager.AppSettings["fileExtension"];



        #region SELECT

        public static string GetSubdirectory()
        {
            string otherfileDirectoryOut = ConfigurationManager.AppSettings["OtherfileDirectoryOut"];
            return otherfileDirectoryOut;
        }

        public static string[] GetFileNames(char separator)
        {
            string filePath = ConfigurationManager.AppSettings["fileDirectoryIn"];
            string[] fileNames = GetFileNames(filePath, separator);

            return fileNames;
        }

        public static string[] GetFileNames(string filePath, char separator)
        {
            string[] filePathExcuded = (ConfigurationManager.AppSettings["fileDirectoryExclude"]).Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < filePathExcuded.Length; i++)
            {
                filePathExcuded[i] = filePathExcuded[i].Trim();
            }

            string[] fileNames = GetFileNames(filePath, filePathExcuded).ToArray();

            return fileNames;
        }

        public static IEnumerable<string> GetFileNames(string path, IEnumerable<string> exclude, SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<string> fileNames = new List<string>();

            IEnumerable<FileInfo> files = new DirectoryInfo(path).EnumerateFiles("*.*", searchOption);

            foreach (FileInfo filename in files)
            {
                if (!(exclude.Any(s => filename.DirectoryName.Contains(s))))
                {
                    fileNames.Add(filename.FullName);
                }
            }

            return fileNames;
        }

        #endregion


        #region READ

        public static Object ReadFile(string fileName, int readOption)
        {
            Object o = null;

            switch (readOption)
            {
                case 0:
                    {
                        o = ReadFileAsString(fileName);
                        break;
                    }
                case 1:
                    {
                        o = GetFileStream(fileName);
                        break;
                    }
                default:
                    {

                        break;
                    }
            }

            return o;
        }

        public static string ReadFileAsString(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                return streamReader.ReadToEnd();
            }
        }

        public static string readFileFromStreamToString(string fileName)
        {
            string atringData;

            using (Stream fileStream = GetFileStream(fileName))
            {
                StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
                atringData = streamReader.ReadToEnd();
            }

            return atringData;
        }

        public static Stream GetFileStream(string fileName)
        {
            try
            {
                return new FileStream(fileName, FileMode.Open);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ReadDataPortion(string stringData, string tag, char delimiterStart, char delimiterEnd)
        {
            string dataPortion = string.Empty;

            string[] dataArray = stringData.Split(new string[] { tag }, StringSplitOptions.None);

            dataPortion = dataArray.Length > 1 ? dataArray[0] : stringData;

            int start = dataPortion.IndexOf(delimiterStart);
            int end = dataPortion.LastIndexOf(delimiterEnd);

            return dataPortion.Substring(start, end);
        }

        #endregion


        #region WRITE

        public static string WriteFile(string stringData, string fileName = "SetFile")
        {
            string fileFullName = fileDirectoryOut + filePrefix + fileName + fileSuffix + fileExtension;
            File.WriteAllText(fileFullName, stringData);
            return fileFullName;
        }

        #endregion

    }
}