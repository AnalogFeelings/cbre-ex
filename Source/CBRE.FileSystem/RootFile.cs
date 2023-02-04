using System.Collections.Generic;

namespace CBRE.FileSystem
{
    public class RootFile : CompositeFile
    {
        public string RootName { get; private set; }
        public override string FullPathName { get { return RootName + ":" + base.FullPathName; } }

        public RootFile(string rootName, IEnumerable<IFile> files) : base(null, files)
        {
            RootName = rootName;
        }
    }
}