using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;

namespace CBRE.Shell.Settings.Editors
{
    public partial class LanguageSelectionEditor : UserControl, ISettingEditor
    {
        public event EventHandler<SettingKey> OnValueChanged;

        string ISettingEditor.Label
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public object Value
        {
            get => (Combobox.SelectedItem as LanguageValue)?.Value;
            set => Combobox.SelectedItem = Combobox.Items.OfType<LanguageValue>().FirstOrDefault(x => x.Value == Convert.ToString(value));
        }

        public object Control => this;
        public SettingKey Key { get; set; }

        public LanguageSelectionEditor(TranslationStringsCatalog catalog)
        {
            InitializeComponent();

            Combobox.DisplayMember = "Label";
            Combobox.ValueMember = "Value";

            System.Collections.Generic.Dictionary<string, Language>.ValueCollection values = catalog.Languages.Values;
            Combobox.Items.AddRange(values.Select(x => new LanguageValue(x)).OfType<object>().ToArray());

            Combobox.SelectedIndexChanged += (o, e) => OnValueChanged?.Invoke(this, Key);
        }

        private class LanguageValue
        {
            public string Label { get; set; }
            public string Value { get; set; }
            public LanguageValue(Language lang)
            {
                Label = string.IsNullOrWhiteSpace(lang.Description) ? lang.Code : lang.Description;
                Value = lang.Code;
            }
        }
    }
}
