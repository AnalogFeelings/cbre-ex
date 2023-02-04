using System.Collections.Generic;

namespace CBRE.DataStructures.GameData
{
    public class AutoVisgroupSection
    {
        public string Name { get; set; }
        public List<AutoVisgroup> Groups { get; private set; }

        public AutoVisgroupSection()
        {
            Groups = new List<AutoVisgroup>();
        }
    }
}