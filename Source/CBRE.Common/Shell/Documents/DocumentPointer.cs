using System.Runtime.Serialization;
using CBRE.Common.Transport;

namespace CBRE.Common.Shell.Documents
{
    public class DocumentPointer : SerialisedObject
    {
        public string FileName
        {
            get => this.Get<string>("FileName");
            set => this.Set("FileName", value);
        }

        public DocumentPointer(string name) : base(name)
        {
        }

        protected DocumentPointer(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
