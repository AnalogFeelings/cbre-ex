using System;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.DataStructures.GameData;

namespace CBRE.BspEditor.Editing.Components.Properties
{
    public interface IObjectPropertyEditor
    {
        event EventHandler<string> ValueChanged;
        event EventHandler<string> NameChanged;

        string PriorityHint { get; }
        Control Control { get; }

        bool SupportsType(VariableType type);
        void SetProperty(MapDocument document, string originalName, string newName, string currentValue, Property property);
    }
}