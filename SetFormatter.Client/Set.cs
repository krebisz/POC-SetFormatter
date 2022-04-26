using System;
using System.Collections.Generic;
using System.Linq;

namespace SetFormatterWebClient
{
    public class Set
    {
        public List<Element> CreatSet(string stringData, char[] delimiters)
        {
            stringData = stringData.Replace("\r\n", "");
            int parent = -1;
            int level = 0;
            int id = 0;

            List<Element> set = new List<Element>();

            while (stringData.Length > 0)
            {
                int stringLength = stringData.Length - 1;
                string stringCurrent = string.Empty;
                string stringNext = string.Empty;

                int indexOpen = stringData.IndexOf(delimiters[0]);
                int indexClose = stringData.IndexOf(delimiters[1]);

                int indexMin = 0;
                int indexMax = 0;

                if (stringLength > 0)
                {
                    if (indexOpen == indexClose)
                    {
                        indexMin = Math.Min(indexOpen, stringLength);
                        indexMax = Math.Max(indexMin + 1, stringLength - indexMin);

                        stringCurrent = stringData.Substring(0, indexMin).Trim();
                        set.Add(new Element(id, stringCurrent, level, parent));
                        id = set.Count;
                    }
                    if (indexOpen < indexClose)
                    {
                        if (indexOpen < 0)
                        {
                            indexMin = Math.Min(indexClose, stringLength);
                            indexMax = Math.Max(indexMin + 1, stringLength - indexMin);

                            stringCurrent = stringData.Substring(0, indexMin).Trim();

                            set.Add(new Element(id, stringCurrent, level, parent));
                            id = parent;
                            parent = set.Find(s => s.Id == id).Parent;
                            level--;
                        }
                        else
                        {
                            indexMin = Math.Min(indexOpen, stringLength);
                            indexMax = Math.Max(indexMin + 1, stringLength - indexMin);

                            stringCurrent = stringData.Substring(0, indexMin).Trim();
                            set.Add(new Element(id, stringCurrent, level, parent));
                            parent = id;
                            id = set.Count;
                            level++;
                        }
                    }
                    if (indexOpen > indexClose)
                    {
                        if (indexClose < 0)
                        {
                            indexMin = Math.Min(indexOpen, stringLength);
                            indexMax = Math.Max(indexMin + 1, stringLength - indexMin);

                            stringCurrent = stringData.Substring(0, indexMin).Trim();
                            set.Add(new Element(id, stringCurrent, level, parent));
                            parent = id;
                            id = set.Count;
                            level++;
                        }
                        else
                        {
                            indexMin = Math.Min(indexClose, stringLength);
                            indexMax = Math.Max(indexMin + 1, stringLength - indexMin);

                            stringCurrent = stringData.Substring(0, indexMin).Trim();
                            set.Add(new Element(id, stringCurrent, level, parent));
                            id = parent;
                            parent = set.Find(s => s.Id == id).Parent;
                            level--;
                        }
                    }
                }

                if (indexMin + 1 > stringLength)
                {
                    stringNext = string.Empty;
                }
                else
                {
                    stringNext = stringData.Substring(indexMin + 1, indexMax).Trim();
                }

                stringData = stringNext;
            }

            return set;
        }

        public List<Element> CreateSet(string stringData, char[] delimiters, char separator)
        {
            stringData = stringData.Replace("\r\n", "");
            int parent = -1;
            int level = 0;
            int id = 0;

            List<Element> set = new List<Element>();

            while (stringData.Length > 0)
            {
                string stringCurrent = string.Empty;
                string stringNext = string.Empty;

                int indexOpen = stringData.IndexOf(delimiters[0]);
                int indexClose = stringData.IndexOf(delimiters[1]);

                int indexStart = 0;
                int indexEnd = stringData.Length - 1;

                if (indexEnd > 0)
                {
                    if (indexOpen == indexClose)
                    {
                        indexStart = 0;
                        AddElement(ref set, stringData, indexStart, id, parent, level, separator);
                    }
                    if (indexOpen < indexClose)
                    {
                        if (indexOpen < 0)
                        {
                            indexStart = Math.Min(indexClose, indexEnd);
                            AddElement(ref set, stringData, indexStart, id, parent, level, separator);

                            id = parent;
                            parent = set.Find(s => s.Id == id).Parent;
                            level--;
                        }
                        else
                        {
                            indexStart = Math.Min(indexOpen, indexEnd);
                            AddElement(ref set, stringData, indexStart, id, parent, level, separator);

                            parent = set.FindLast(s => s.Parent == parent).Id;
                            id = set.Count;
                            level++;
                        }
                    }
                    if (indexOpen > indexClose)
                    {
                        if (indexClose < 0)
                        {
                            indexStart = Math.Min(indexOpen, indexEnd - 1);
                            AddElement(ref set, stringData, indexStart, id, parent, level, separator);

                            parent = set.FindLast(s => s.Parent == parent).Id;
                            id = set.Count;
                            level++;
                        }
                        else
                        {
                            indexStart = Math.Min(indexClose, indexEnd - 1);
                            AddElement(ref set, stringData, indexStart, id, parent, level, separator);

                            id = parent;
                            parent = set.Find(s => s.Id == id).Parent;
                            level--;
                        }
                    }
                }

                stringData = FindNextString(stringData, indexStart, indexEnd);
            }

            return set;
        }

        public List<Element> SortSet(List<Element> set)
        {
            List<Element> setSorted = new List<Element>();

            setSorted = set.OrderBy(e => e.Level).ThenBy(e => e.Id).ToList();

            return setSorted;
        }

        public List<Element> OrderSet(List<Element> set)
        {
            List<Element> setOrdered = set.GroupBy(p => new { p.Id, p.Parent }).Select(g => g.First()).OrderBy(e => (e.Parent, e.Id)).ToList();
            OptimizeIds(set);
            return setOrdered;
        }

        public void AddElement(ref List<Element> set, string stringData, int index, int id, int parent, int level, char separator)
        {
            string subStringData = stringData.Substring(0, index).Trim();
            string[] subStringArray = subStringData.Split(separator);

            foreach (string s in subStringArray)
            {
                set.Add(new Element(id, s.Trim(), level, parent));
            }
        }

        public List<Element> GetChildElements(int id, List<Element> setNew, List<Element> set)
        {
            List<Element> setSub = new List<Element>();
            setSub = set.FindAll(e => e.Parent == id);

            foreach (Element element in setSub)
            {
                if (set.Any(e => e.Parent == element.Id))
                {
                    GetChildElements(element.Id, setNew, set);
                }
                else
                {
                    setNew.Add(element);
                }
            }

            setNew.Add(set.FirstOrDefault(e => e.Id == id));

            return setNew;
        }



        public List<Element> OptimizeIds(List<Element> set)
        {
            List<Element> setSorted = SortSet(set);

            int i = 0;

            foreach (Element e in setSorted)
            {
                e.Id = i;
                i++;
            }

            return setSorted;
        }

        public void OrderIds(List<Element> set)
        {
            int idNew = -1;
            List<int> idList = new List<int>();
            idList.AddRange(set.Select(e => e.Id));

            //idList = ValidateIds(idList);

            List<int> idListOrdered = (idList).GroupBy(p => new { p }).Select(g => g.First()).OrderBy(e => (e)).ToList();

            foreach (int id in idListOrdered)
            {
                if (idNew + 1 < id)
                {
                    idNew++;
                    OptimizeId(set, id, idNew);
                }
                else
                {
                    idNew = id;
                }
            }
        }

        public void OptimizeId(List<Element> set, int id, int idNew)
        {
            foreach (Element element in set)
            {
                if (element.Parent == id)
                {
                    element.Parent = idNew;
                }
                if (element.Id == id)
                {
                    element.Id = idNew;
                }
            }
        }

        public List<int> GetIdsFromSet(List<Element> set)
        {
            List<int> idList = new List<int>();

            foreach (Element e in set)
            {
                idList.Add(e.Id);

            }

            return idList;
        }



        public string[] SplitStringToArray(string stringData, char[] tags, bool trimString, bool reinsertTags)
        {
            if (trimString)
            {
                stringData = stringData.Replace("\r\n", "");
            }

            string[] stringArray = stringData.Split(tags);

            if (reinsertTags)
            {
                List<string> stringList = new List<string>();

                for (int i = 0; i < stringArray.Length - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        stringList.Add(stringArray[i]);
                        stringList.Add(tags[0].ToString());
                    }
                    else
                    {
                        stringList.Add(stringArray[i]);
                        stringList.Add(tags[1].ToString());
                    }
                }

                return stringList.ToArray();
            }
            else
            {
                return stringArray;
            }
        }

        public string FindNextString(string stringData, int indexFirst, int indexLast)
        {
            string substringNext = string.Empty;

            if (indexFirst + 1 <= indexLast)
            {
                int indexNext = Math.Max(indexFirst + 1, indexLast - indexFirst);

                if (indexFirst + indexNext > indexLast)
                {
                    indexNext = 0;
                }

                substringNext = stringData.Substring(indexFirst + 1, indexNext).Trim();
            }

            return substringNext;
        }


        public string ReadStringPortion(string stringData, string tag, char delimiterStart, char delimiterEnd, bool unbounded)
        {
            string stringPortion = string.Empty;

            string[] stringArray = stringData.Split(new string[] { tag }, StringSplitOptions.None);

            if (stringArray.Length > 1)
            {
                stringPortion = stringArray[1];
            }
            else
            {
                stringPortion = stringData;
            }

            int start = Math.Max(stringPortion.IndexOf(delimiterStart), 0);
            int end = stringPortion.LastIndexOf(delimiterEnd);

            if (end < 0)
            {
                end = unbounded ? stringPortion.Length - 1 : 0;
            }

            return stringPortion.Substring(start, end);
        }

        public string OrderStringSet(string setIn, char separator)
        {
            string setOut = string.Empty;

            string[] setInArray = setIn.Split(separator);
            List<string> setInList = new List<string>();

            foreach (string element in setInArray)
            {
                setInList.Add(element.ToLower().Trim());
            }

            setInList.RemoveAll(s => s == string.Empty);

            IEnumerable<string> setOutList = setInList.Distinct();
            setOutList = setOutList.OrderBy(e => e.First()).ToList();

            foreach (string element in setOutList)
            {
                setOut += element;

                if (setOutList.Last() != element)
                {
                    setOut += separator + " ";
                }
            }

            return setOut;
        }

        public string GetStringDataFromSet(List<Element> set)
        {
            string stringData = string.Empty;

            foreach (Element e in set)
            {
                stringData = stringData + "Id:" + e.Id + " Parent:" + e.Parent + " Level:" + e.Level + " |NAME: " + e.Name + "\r\n";
            }

            return stringData;
        }



        public void GetSetFromFile(ref List<Element> set, string fileName, char[] delimiters, char separator)
        {
            try
            {
                string stringData = IOHelper.ReadFileAsString(fileName);
                set = CreateSet(stringData, delimiters, separator);
                set = OrderSet(set);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string WriteSetToFile(ref List<Element> set, string fileName = null)
        {
            try
            {
                string stringData = GetStringDataFromSet(set);
                string fileNameNew = IOHelper.WriteFile(stringData, fileName);

                return fileNameNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}