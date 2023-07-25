using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBRE.DataStructures.GameData
{
    public class Option
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public bool On { get; set; }

        public string DisplayText()
        {
            return string.IsNullOrWhiteSpace(Description) ? Key : Description;
        }
    }
}
