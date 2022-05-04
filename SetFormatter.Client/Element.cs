using System;
using System.Collections;
using System.Collections.Generic;

namespace SetFormatter
{
    public class Element
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

        public int Id { get; set; }
        public String Name { get; set; }
        public int Level { get; set; }
        public int Parent { get; set; }
    }
}