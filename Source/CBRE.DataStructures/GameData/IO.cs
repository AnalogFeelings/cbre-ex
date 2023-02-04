using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBRE.DataStructures.GameData
{
    public class IO
    {
        public IOType IOType { get; set; }
        public VariableType VariableType { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
