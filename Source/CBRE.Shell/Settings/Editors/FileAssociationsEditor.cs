using System;
using System.Windows.Forms;
using CBRE.Common.Shell.Settings;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Settings.Editors
{
    public partial class FileAssociationsEditor : UserControl, ISettingEditor
    {
        public event EventHandler<SettingKey> OnValueChanged;

        public string Label { get; set; }
        
        private DocumentRegister.FileAssociations _bindings;

        public object Value
        {
            get => _bindings;
            set
            {
                _bindings = ((DocumentRegister.FileAssociations) value).Clone();
                UpdateAssociationsList();
            }
        }
        
        public object Control => this;
        public SettingKey Key { get; set; }

        public FileAssociationsEditor()
        {
            InitializeComponent();
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
        }

        private void UpdateAssociationsList()
        {
            CheckboxPanel.Controls.Clear();
            
            if (_bindings == null) return;

            foreach (System.Collections.Generic.KeyValuePair<string, bool> b in _bindings)
            {
                CheckBox checkbox = new CheckBox
                {
                    Text = b.Key,
                    Checked = b.Value,
                    Tag = b.Key,
                    Margin = new Padding(2)
                };
                checkbox.CheckedChanged += SetAssociation;
                CheckboxPanel.Controls.Add(checkbox);
            }
        }

        private void SetAssociation(object sender, EventArgs e)
        {
            bool assoc = (sender as CheckBox)?.Checked ?? false;
            _bindings[(sender as CheckBox)?.Tag as string ?? ""] = assoc;
            OnValueChanged?.Invoke(this, Key);
        }
    }
}
