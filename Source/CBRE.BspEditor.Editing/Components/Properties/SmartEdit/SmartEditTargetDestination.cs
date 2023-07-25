using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.DataStructures.GameData;

namespace CBRE.BspEditor.Editing.Components.Properties.SmartEdit
{
    [Export(typeof(IObjectPropertyEditor))]
    public class SmartEditTargetDestination : SmartEditControl
    {
        private readonly ComboBox _comboBox;
        public SmartEditTargetDestination()
        {
            _comboBox = new ComboBox { Width = 250 };
            _comboBox.TextChanged += (sender, e) => OnValueChanged();
            Controls.Add(_comboBox);
        }

        public override string PriorityHint => "H";

        public override bool SupportsType(VariableType type)
        {
            return type == VariableType.TargetDestination;
        }

        protected override string GetName()
        {
            return OriginalName;
        }

        protected override string GetValue()
        {
            return _comboBox.Text;
        }

        private IEnumerable<string> GetSortedTargetNames(MapDocument document)
        {
            return document.Map.Root.Find(x => x.Data.GetOne<EntityData>() != null)
                .Select(x => x.Data.GetOne<EntityData>().Get<string>("targetname"))
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Distinct()
                .OrderBy(x => x.ToLowerInvariant());
        }

        protected override void OnSetProperty(MapDocument document)
        {
            _comboBox.Items.Clear();
            if (Property != null)
            {
                List<string> options = GetSortedTargetNames(document).ToList();
                _comboBox.Items.AddRange(options.OfType<object>().ToArray());
                int index = options.FindIndex(x => String.Equals(x, PropertyValue, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    _comboBox.SelectedIndex = index;
                    return;
                }
            }
            _comboBox.Text = PropertyValue;
        }
    }
}