using System;
using System.Collections;
using System.Collections.Generic;

namespace SetFormatterWebClient
{
    public class Element : IEnumerable
    {
        public Element(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public Element(int id, string name, int level, int parent)
        {
            Id = id;
            Name = name;
            Level = level;
            Parent = parent;
        }

        private List<Element> Elements = new List<Element>();

        public int Id { get; set; }
        public String Name { get; set; }
        public int Level { get; set; }
        public int Parent { get; set; }

        public IEnumerator GetEnumerator()
        {
            foreach (object o in Elements)
            {
                if (o == null)
                {
                    break;
                }

                yield return o;
            }
        }

        public void Add(Element element)
        {
            Elements.Add(element);
        }
    }
}