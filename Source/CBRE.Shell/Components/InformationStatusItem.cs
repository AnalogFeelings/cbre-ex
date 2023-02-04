using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;

namespace CBRE.Shell.Components
{
    [Export(typeof(IStatusItem))]
    [OrderHint("B")]
    public class InformationStatusItem : IStatusItem
    {
        public string ID => "CBRE.Shell.InformationStatusItem";
        public int Width => -1;
        public bool HasBorder => false;
        public string Text { get; set; } = "";

        public event EventHandler<string> TextChanged;

        public InformationStatusItem()
        {
            Oy.Subscribe<string>("Status:Information", Post);
        }

        private Task Post(string message)
        {
            Text = message;
            TextChanged?.Invoke(this, message);
            return Task.FromResult(0);
        }

        public bool IsInContext(IContext context)
        {
            return true;
        }
    }
}
