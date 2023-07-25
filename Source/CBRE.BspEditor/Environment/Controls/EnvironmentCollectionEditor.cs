using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Environment.Controls
{
    public partial class EnvironmentCollectionEditor : UserControl, ISettingEditor, IManualTranslate
    {
        private readonly List<IEnvironmentFactory> _factories;
        private EnvironmentCollection _value;
        public event EventHandler<SettingKey> OnValueChanged;

        public string Label { get; set; }

        public object Value
        {
            get => _value;
            set
            {
                _value = value as EnvironmentCollection;
                UpdateTreeNodes();
            }
        }

        public object Control => this;

        public SettingKey Key { get; set; }

        private Label _nameLabel;
        private TextBox _nameBox;

        public EnvironmentCollectionEditor(IEnumerable<IEnvironmentFactory> factories)
        {
            _factories = factories.ToList();
            InitializeComponent();
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom;

            _nameLabel = new Label {Text = "Name", Padding = new Padding(0, 6, 0, 0), AutoSize = true};
            _nameBox = new TextBox{Width = 250};
            _nameBox.TextChanged += UpdateEnvironment;

            if (_factories.Any())
            {
                ctxEnvironmentMenu.Items.Clear();
                foreach (IEnvironmentFactory ef in _factories)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem(ef.Description) { Tag = ef };
                    mi.Click += AddEnvironment;
                    ctxEnvironmentMenu.Items.Add(mi);
                }
            }

            ITranslationStringProvider translate = Common.Container.Get<ITranslationStringProvider>();
            translate.Translate(this);
        }

        public void Translate(ITranslationStringProvider strings)
        {
            string prefix = GetType().FullName;
            btnAdd.Text = strings.GetString(prefix, "Add");
            btnRemove.Text = strings.GetString(prefix, "Remove");
            _nameLabel.Text = strings.GetString(prefix, "Name");
        }

        private void UpdateTreeNodes()
        {
            treEnvironments.Nodes.Clear();
            if (_value == null) return;

            foreach (IGrouping<string, SerialisedEnvironment> g in _value.GroupBy(x => x.Type))
            {
                string ef = _factories.FirstOrDefault(x => x.TypeName == g.Key)?.Description ?? g.Key;
                TreeNode groupNode = new TreeNode(ef);
                foreach (SerialisedEnvironment se in g)
                {
                    TreeNode envNode = new TreeNode(se.Name) { Tag = se };
                    groupNode.Nodes.Add(envNode);
                }
                treEnvironments.Nodes.Add(groupNode);
            }
            treEnvironments.ExpandAll();
        }

        private void AddEnvironment(object sender, EventArgs e)
        {
            IEnvironmentFactory factory = (sender as ToolStripItem)?.Tag as IEnvironmentFactory;
            if (factory != null && _value != null)
            {
                SerialisedEnvironment newEnv = new SerialisedEnvironment
                {
                    ID = Guid.NewGuid().ToString("N"),
                    Name = "New Environment",
                    Type = factory.TypeName
                };
                _value.Add(newEnv);
                UpdateTreeNodes();

                TreeNode nodeToSelect = treEnvironments.Nodes.OfType<TreeNode>().SelectMany(x => x.Nodes.OfType<TreeNode>()).FirstOrDefault(x => x.Tag == newEnv);
                if (nodeToSelect != null) treEnvironments.SelectedNode = nodeToSelect;

                OnValueChanged?.Invoke(this, Key);
            }
        }

        private void RemoveEnvironment(object sender, EventArgs e)
        {
            SerialisedEnvironment node = treEnvironments.SelectedNode?.Tag as SerialisedEnvironment;
            if (node != null && _value != null)
            {
                _value.Remove(node);
                UpdateTreeNodes();
                OnValueChanged?.Invoke(this, Key);
                EnvironmentSelected(null, null);
            }
        }

        private IEnvironmentEditor _currentEditor = null;

        private void EnvironmentSelected(object sender, TreeViewEventArgs e)
        {
            if (_currentEditor != null) _currentEditor.EnvironmentChanged -= UpdateEnvironment;

            ITranslationStringProvider translate = Common.Container.Get<ITranslationStringProvider>();

            _currentEditor = null;
            pnlSettings.Controls.Clear();

            SerialisedEnvironment node = e?.Node?.Tag as SerialisedEnvironment;
            if (node != null)
            {
                IEnvironmentFactory factory = _factories.FirstOrDefault(x => x.TypeName == node.Type);
                if (factory != null)
                {
                    FlowLayoutPanel fp = new FlowLayoutPanel
                    {
                        Height = 30,
                        Width = 400,
                        FlowDirection = FlowDirection.LeftToRight,
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                    };
                    fp.Controls.Add(_nameLabel);
                    fp.Controls.Add(_nameBox);
                    pnlSettings.Controls.Add(fp);

                    _nameBox.Text = node.Name;

                    IEnvironment des = factory.Deserialise(node);
                    _currentEditor = factory.CreateEditor();
                    translate.Translate(_currentEditor);
                    pnlSettings.Controls.Add(_currentEditor.Control);
                    _currentEditor.Environment = des;
                    _currentEditor.EnvironmentChanged += UpdateEnvironment;
                }
            }
        }

        private void UpdateEnvironment(object sender, EventArgs e)
        {
            SerialisedEnvironment node = treEnvironments.SelectedNode?.Tag as SerialisedEnvironment;
            if (node != null && _currentEditor != null)
            {
                treEnvironments.SelectedNode.Text = _nameBox.Text;
                IEnvironmentFactory factory = _factories.FirstOrDefault(x => x.TypeName == node.Type);
                if (factory != null)
                {
                    SerialisedEnvironment ser = factory.Serialise(_currentEditor.Environment);
                    node.Name = _nameBox.Text;
                    node.Properties = ser.Properties;
                }
                OnValueChanged?.Invoke(this, Key);
            }
        }
    }
}
