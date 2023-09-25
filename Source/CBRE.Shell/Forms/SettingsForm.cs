using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;
using CBRE.Shell.Properties;
using CBRE.Shell.Settings.Editors;

namespace CBRE.Shell.Forms
{
    [Export(typeof(IDialog))]
    [AutoTranslate]
    public partial class SettingsForm : Form, IDialog
    {
        private readonly IEnumerable<Lazy<ISettingEditorFactory>> _editorFactories;
        private readonly IEnumerable<Lazy<ISettingsContainer>> _settingsContainers;
        private readonly Lazy<ITranslationStringProvider> _translations;
        private readonly Lazy<Form> _parent;

        private Dictionary<ISettingsContainer, List<SettingKey>> _keys;
        private Dictionary<ISettingsContainer, JsonSettingsStore> _values;

        public string Title
        {
            get => Text;
            set => this.InvokeLater(() => Text = value);
        }

        public string OK
        {
            get => OKButton.Text;
            set => this.InvokeLater(() => OKButton.Text = value);
        }

        public string Cancel
        {
            get => CancelButton.Text;
            set => this.InvokeLater(() => CancelButton.Text = value);
        }

        [ImportingConstructor]
        public SettingsForm(
            [ImportMany] IEnumerable<Lazy<ISettingEditorFactory>> editorFactories, 
            [ImportMany] IEnumerable<Lazy<ISettingsContainer>> settingsContainers, 
            [Import] Lazy<ITranslationStringProvider> translations, 
            [Import("Shell")] Lazy<Form> parent
        )
        {
            _editorFactories = editorFactories;
            _settingsContainers = settingsContainers;
            _translations = translations;
            _parent = parent;

            InitializeComponent();
            Icon = Icon.FromHandle(Resources.Menu_Options.GetHicon());
            CreateHandle();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                _keys = _settingsContainers.ToDictionary(x => x.Value, x =>
                {
                    List<SettingKey> keys = x.Value.GetKeys().ToList();
                    keys.ForEach(k => k.Container = x.Value.Name);
                    return keys;
                });
                _values = _settingsContainers.ToDictionary(x => x.Value, x =>
                {
                    JsonSettingsStore fss = new JsonSettingsStore();
                    x.Value.StoreValues(fss);
                    return fss;
                });
                LoadGroupList();

                GroupList.SelectedNode = GroupList.TopNode;
            }
            base.OnVisibleChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Oy.Publish("Context:Remove", new ContextInfo("SettingsForm"));
        }

        private void LoadGroupList()
        {
            GroupList.BeginUpdate();

            Dictionary<string, TreeNode> nodes = new Dictionary<string, TreeNode>();

            GroupList.Nodes.Clear();
            foreach (IGrouping<string, SettingKey> k in _keys.SelectMany(x => x.Value).GroupBy(x => x.Group).OrderBy(x => x.Key))
            {
                GroupHolder gh = new GroupHolder(k.Key, _translations.Value.GetSetting("@Group." + k.Key) ?? k.Key);

                TreeNode parentNode = null;
                int par = k.Key.LastIndexOf('/');
                if (par > 0)
                {
                    string sub = k.Key.Substring(0, par);
                    if (nodes.ContainsKey(sub))
                    {
                        parentNode = nodes[sub];
                    }
                    else
                    {
                        GroupHolder pgh = new GroupHolder(sub, _translations.Value.GetSetting("@Group." + sub) ?? sub);
                        parentNode = new TreeNode(pgh.Label) {Tag = pgh};
                        GroupList.Nodes.Add(parentNode);
                        nodes.Add(sub, parentNode);
                    }
                }
                TreeNode node = new TreeNode(gh.Label) { Tag = gh };
                if (parentNode != null) parentNode.Nodes.Add(node);
                else GroupList.Nodes.Add(node);
                nodes.Add(k.Key, node);
            }

            GroupList.ExpandAll();

            GroupList.EndUpdate();
        }
        
        protected override void OnMouseEnter(EventArgs e)
        {
            Focus();
            base.OnMouseEnter(e);
        }

        private readonly List<ISettingEditor> _editors = new List<ISettingEditor>();

        private void LoadEditorList()
        {
            _editors.ForEach(x => x.OnValueChanged -= OnValueChanged);
            _editors.Clear();

            SettingsPanel.SuspendLayout();
            SettingsPanel.Controls.Clear();
            
            SettingsPanel.RowStyles.Clear();

            if (GroupList?.SelectedNode?.Tag is GroupHolder gh)
            {
                string group = gh.Key;
                foreach (KeyValuePair<ISettingsContainer, List<SettingKey>> kv in _keys)
                {
                    ISettingsContainer container = kv.Key;
                    IEnumerable<SettingKey> keys = kv.Value.Where(x => x.Group == group);
                    JsonSettingsStore values = _values[container];
                    foreach (SettingKey key in keys)
                    {
                        ISettingEditor editor = GetEditor(key);
                        editor.Key = key;
                        editor.Label = _translations.Value.GetSetting($"{kv.Key.Name}.{key.Key}") ?? key.Key;
                        editor.Value = values.Get(key.Type, key.Key);

                        if (SettingsPanel.Controls.Count > 0)
                        {
                            // Add a separator
                            Label line = new Label
                            {
                                Height = 2,
                                BorderStyle = BorderStyle.Fixed3D,
                                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                            };
                            SettingsPanel.Controls.Add(line);
                        }

                        Control ctrl = (Control) editor.Control;
                        ctrl.Anchor |= AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        SettingsPanel.Controls.Add(ctrl);

                        if (ctrl.Anchor.HasFlag(AnchorStyles.Bottom))
                        {
                            SettingsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                        }

                        _editors.Add(editor);
                    }
                }
            }

            SettingsPanel.ResumeLayout();

            _editors.ForEach(x => x.OnValueChanged += OnValueChanged);
        }

        private void OnValueChanged(object sender, SettingKey key)
        {
            ISettingEditor se = sender as ISettingEditor;
            JsonSettingsStore store = _values.Where(x => x.Key.Name == key.Container).Select(x => x.Value).FirstOrDefault();
            if (store != null && se != null)
            {
                store.Set(key.Key, se.Value);
            }
        }

        private ISettingEditor GetEditor(SettingKey key)
        {
            foreach (Lazy<ISettingEditorFactory> ef in _editorFactories.OrderBy(x => x.Value.OrderHint))
            {
                if (ef.Value.Supports(key)) return ef.Value.CreateEditorFor(key);
            }
            return new DefaultSettingEditor();
        }

        public bool IsInContext(IContext context)
        {
            return context.HasAny("SettingsForm");
        }

        public void SetVisible(IContext context, bool visible)
        {
            this.InvokeLater(() =>
            {
                if (visible)
                {
                    if (!Visible) Show(_parent.Value);
                }
                else
                {
                    if (Visible) Hide();
                }
            });
        }

        private void GroupListSelectionChanged(object sender, TreeViewEventArgs e)
        {
            LoadEditorList();
        }

        private void OkClicked(object sender, EventArgs e)
        {
            foreach (KeyValuePair<ISettingsContainer, JsonSettingsStore> kv in _values)
            {
                kv.Key.LoadValues(kv.Value);
            }

            Oy.Publish("Settings:Save");
            Oy.Publish("SettingsChanged", new object());
            Close();
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private class GroupHolder
        {
            public string Key { get; set; }
            public string Label { get; set; }

            public GroupHolder(string key, string label)
            {
                Key = key;
                Label = label;
            }

            public override string ToString()
            {
                return Label;
            }
        }
    }
}
