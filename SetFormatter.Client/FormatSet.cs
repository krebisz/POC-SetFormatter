using System;
using System.Collections.Generic;
using System.Linq;

namespace SetFormatterWebClient
{
    public class FormatSet
    {
        private List<Element> Elements = new List<Element>();

        public string OrderSet(string setIn, char separator)
        {
            string setOut = string.Empty;

            string[] setInArray = setIn.Split(separator);
            List<string> setInList = new List<string>();

            foreach (string element in setInArray)
            {
                setInList.Add(element.ToLower().Trim());
            }

            setInList.RemoveAll(s => s == string.Empty);

            //IEnumerable<string> setOutList = setInList;
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

        #region Functions

        public string ReadFilePortion(string fileContent, string tag, char delimiterStart, char delimiterEnd, bool unbounded)
        {
            string filePortion = string.Empty;

            string[] fileContentArray = fileContent.Split(new string[] { tag }, StringSplitOptions.None);

            if (fileContentArray.Length > 1)
            {
                filePortion = fileContentArray[1];
            }
            else
            {
                filePortion = fileContent;
            }

            int start = Math.Max(filePortion.IndexOf(delimiterStart), 0);
            int end = filePortion.LastIndexOf(delimiterEnd);

            if (end < 0)
            {
                end = unbounded ? filePortion.Length - 1 : 0;
            }

            return filePortion.Substring(start, end);
        }

        public string[] PartitionText(string content, char[] tag)
        {
            content = content.Replace("\r\n", "");

            string[] partitionedText = content.Split(tag);

            List<string> collections = new List<string>();

            //for (int i = 0; i < partitionedText.Length - 1; i = i + 2)
            //{
            //    collections.Add(partitionedText[i] + "{" + partitionedText[i + 1] + "}");
            //}

            for (int i = 0; i < partitionedText.Length - 1; i++)
            {
                if (i % 2 == 0)
                {
                    collections.Add(partitionedText[i]);
                    collections.Add(tag[0].ToString());
                }
                else
                {
                    collections.Add(partitionedText[i]);
                    collections.Add(tag[1].ToString());
                }
            }

            return collections.ToArray();
        }

        public string[] ReadCollections(string stringContent, char[] tags)
        {
            string[] fileContentArray = stringContent.Split(tags);
            return fileContentArray;
        }

        public List<Element> CreateTree(string stringContent, char[] delimiters)
        {
            List<Element> elements = new List<Element>();
            int Level = 0;
            int Id = 0;
            int Parent = -1;

            while (stringContent.Length > 0)
            {
                int content_length = stringContent.Length - 1;
                string currentContent = string.Empty;
                string nextContent = string.Empty;

                int open_index = stringContent.IndexOf(delimiters[0]);
                int close_index = stringContent.IndexOf(delimiters[1]);

                int index_min = 0;
                int index_max = 0;

                if (content_length > 0)
                {
                    if (open_index == close_index)
                    {
                        index_min = Math.Min(open_index, content_length);
                        index_max = Math.Max(index_min + 1, content_length - index_min);

                        currentContent = stringContent.Substring(0, index_min).Trim();
                        elements.Add(new Element(Id, currentContent, Level, Parent));
                        Id = elements.Count;
                    }
                    if (open_index < close_index)
                    {
                        if (open_index < 0)
                        {
                            index_min = Math.Min(close_index, content_length);
                            index_max = Math.Max(index_min + 1, content_length - index_min);

                            currentContent = stringContent.Substring(0, index_min).Trim();

                            elements.Add(new Element(Id, currentContent, Level, Parent));
                            Id = Parent;
                            Parent = elements.Find(s => s.Id == Id).Parent;
                            Level--;
                        }
                        else
                        {
                            index_min = Math.Min(open_index, content_length);
                            index_max = Math.Max(index_min + 1, content_length - index_min);

                            currentContent = stringContent.Substring(0, index_min).Trim();
                            elements.Add(new Element(Id, currentContent, Level, Parent));
                            Parent = Id;
                            Id = elements.Count;
                            Level++;
                        }
                    }
                    if (open_index > close_index)
                    {
                        if (close_index < 0)
                        {
                            index_min = Math.Min(open_index, content_length);
                            index_max = Math.Max(index_min + 1, content_length - index_min);

                            currentContent = stringContent.Substring(0, index_min).Trim();
                            elements.Add(new Element(Id, currentContent, Level, Parent));
                            Parent = Id;
                            Id = elements.Count;
                            Level++;
                        }
                        else
                        {
                            index_min = Math.Min(close_index, content_length);
                            index_max = Math.Max(index_min + 1, content_length - index_min);

                            currentContent = stringContent.Substring(0, index_min).Trim();
                            elements.Add(new Element(Id, currentContent, Level, Parent));
                            Id = Parent;
                            Parent = elements.Find(s => s.Id == Id).Parent;
                            Level--;
                        }
                    }
                }

                if (index_min + 1 > content_length)
                {
                    nextContent = string.Empty;
                }
                else
                {
                    nextContent = stringContent.Substring(index_min + 1, index_max).Trim();
                }

                stringContent = nextContent;
            }

            return elements;
        }

        #endregion Functions

        public List<Element> CreateTree(string full_content, char[] delimiter, char separator)
        {
            full_content = full_content.Replace("\r\n", "");

            Elements = new List<Element>();
            int Level = 0;
            int Id = 0;
            int Parent = -1;

            while (full_content.Length > 0)
            {
                string current_content = string.Empty;
                string next_content = string.Empty;

                int open_index = full_content.IndexOf(delimiter[0]);
                int close_index = full_content.IndexOf(delimiter[1]);

                int first_index = 0;
                int last_index = full_content.Length - 1;

                if (last_index > 0)
                {
                    if (open_index == close_index)
                    {
                        first_index = 0;
                        AddNode(full_content, first_index, Id, Parent, Level, separator);
                    }
                    if (open_index < close_index)
                    {
                        if (open_index < 0)
                        {
                            first_index = Math.Min(close_index, last_index);
                            AddNode(full_content, first_index, Id, Parent, Level, separator);

                            Id = Parent;
                            Parent = Elements.Find(s => s.Id == Id).Parent;
                            Level--;
                        }
                        else
                        {
                            first_index = Math.Min(open_index, last_index);
                            AddNode(full_content, first_index, Id, Parent, Level, separator);

                            Parent = Elements.FindLast(s => s.Parent == Parent).Id;
                            Id = Elements.Count;
                            Level++;
                        }
                    }
                    if (open_index > close_index)
                    {
                        if (close_index < 0)
                        {
                            first_index = Math.Min(open_index, last_index - 1);
                            AddNode(full_content, first_index, Id, Parent, Level, separator);

                            Parent = Elements.FindLast(s => s.Parent == Parent).Id;
                            Id = Elements.Count;
                            Level++;
                        }
                        else
                        {
                            first_index = Math.Min(close_index, last_index - 1);
                            AddNode(full_content, first_index, Id, Parent, Level, separator);

                            Id = Parent;
                            Parent = Elements.Find(s => s.Id == Id).Parent;
                            Level--;
                        }
                    }
                }

                full_content = FindNextContent(full_content, first_index, last_index);
            }

            return Elements;
        }

        public void AddNode(string content, int index, int Id, int Parent, int Level, char separator)
        {
            string current_content = content.Substring(0, index).Trim();
            string[] currentContextArray = current_content.Split(separator);

            foreach (string s in currentContextArray)
            {
                Elements.Add(new Element(Id, s.Trim(), Level, Parent));
            }
        }

        public string FindNextContent(string full_content, int first_index, int last_index)
        {
            string next_content = string.Empty;

            if (first_index + 1 <= last_index)
            {
                int next_index = Math.Max(first_index + 1, last_index - first_index);

                if (first_index + next_index > last_index)
                {
                    next_index = 0;
                }

                next_content = full_content.Substring(first_index + 1, next_index).Trim();
            }

            return next_content;
        }

        public void OptimizeIds(List<int> idList = null)
        {
            int current_id = -1;
            int new_id = -1;

            if (idList == null)
            {
                idList = new List<int>();
                idList.AddRange(Elements.Select(e => e.Id));
            }
            else
            {
                idList = ValidateIds(idList);
            }

            List<int> validatedIdList = (idList).GroupBy(p => new { p }).Select(g => g.First()).OrderBy(e => (e)).ToList();

            foreach (int id in validatedIdList)
            {
                if (current_id + 1 < id)
                {
                    current_id = current_id + 1;
                    OptimizeId(id, current_id);
                }
                else
                {
                    current_id = id;
                }
            }
        }

        public List<int> ValidateIds(List<int> idList)
        {
            List<int> validatedIdList = new List<int>();

            foreach (int id in idList)
            {
                if (Elements.Select(e => e.Id = id) != null)
                {
                    validatedIdList.Add(id);
                }
            }

            return validatedIdList;
        }

        public void OptimizeId(int Id, int newId)
        {
            foreach (Element element in Elements)
            {
                if (element.Parent == Id)
                {
                    element.Parent = newId;
                }
                if (element.Id == Id)
                {
                    element.Id = newId;
                }
            }
        }

        public List<Element> OptimizeIds(List<Element> elements)
        {
            List<Element> newElements = SortElements(elements);

            int i = 0;

            foreach (Element e in newElements)
            {
                e.Id = i;
                i++;
            }


            return newElements;
        }

        public List<Element> SortElements(List<Element> elements)
        {
            List<Element> newElements = new List<Element>();

            newElements = elements.OrderBy(e => e.Level).ThenBy(e => e.Id).ToList();

            return newElements;
        }
    }
}