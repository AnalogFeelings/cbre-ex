using System.Collections.Generic;

namespace CBRE.DataStructures.GameData
{
    public class AutoVisgroup
    {
        public string Name { get; set; }
        public List<string> EntityNames { get; private set; }

        public AutoVisgroup()
        {
            EntityNames = new List<string>();
        }
    }
}