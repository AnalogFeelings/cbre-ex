using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;
using CBRE.DataStructures.GameData;
using CBRE.Shell;

namespace CBRE.BspEditor.Editing.Components.Properties.Tabs
{
    /// <summary>
    /// The class info tab allows entity classes and keyvalues to be edited.
    /// 
    /// SmartEdit is functionality which detects the type of keyvalues and shows
    /// an editor that should allow easier editing. Turning it off exposes all
    /// keyvalues as simple editable text boxes.
    /// 
    /// Editing keyvalues on multiple selections will work. If the keyvalue doesn't
    /// exist when applied, it will be added.
    /// </summary>
    [AutoTranslate]
    [Export(typeof(IObjectPropertyEditorTab))]
    public partial class ClassInfoTab : UserControl, IObjectPropertyEditorTab
    {
        private readonly IEnumerable<Lazy<IObjectPropertyEditor>> _smartEditControls;
        private readonly IObjectPropertyEditor _defaultControl;

        private ClassValues _tableValues;
        private WeakReference<MapDocument> _document;
        private GameData _gameData;
        private IObjectPropertyEditor _currentEditor;

        public string OrderHint => "D";
        public Control Control => this;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Translations

        public string ClassLabel
        {
            get => lblClass.Text;
            set => this.InvokeLater(() => lblClass.Text = value);
        }

        public string AnglesLabel
        {
            get => angAngles.LabelText;
            set => this.InvokeLater(() => angAngles.LabelText = value);
        }

        public string KeyValuesLabel
        {
            get => lblKeyValues.Text;
            set => this.InvokeLater(() => lblKeyValues.Text = value);
        }

        public string PropertyNameLabel
        {
            get => colPropertyName.Text;
            set => this.InvokeLater(() => colPropertyName.Text = value);
        }

        public string PropertyValueLabel
        {
            get => colPropertyValue.Text;
            set => this.InvokeLater(() => colPropertyValue.Text = value);
        }

        public string HelpLabel
        {
            get => lblHelp.Text;
            set => this.InvokeLater(() => lblHelp.Text = value);
        }

        public string SmartEditButton
        {
            get => btnSmartEdit.Text;
            set => this.InvokeLater(() => btnSmartEdit.Text = value);
        }

        public string CopyButton
        {
            get => btnCopy.Text;
            set => this.InvokeLater(() => btnCopy.Text = value);
        }

        public string PasteButton
        {
            get => btnPaste.Text;
            set => this.InvokeLater(() => btnPaste.Text = value);
        }

        public string AddButton
        {
            get => btnAdd.Text;
            set => this.InvokeLater(() => btnAdd.Text = value);
        }

        public string DeleteButton
        {
            get => btnDelete.Text;
            set => this.InvokeLater(() => btnDelete.Text = value);
        }

        public string MultipleClassesText { get; set; }
        public string MultipleValuesText { get; set; }

        #endregion
        
        /// <inheritdoc />
        public bool HasChanges => _tableValues.ClassChanged || _tableValues.Any(x => x.IsAdded || x.IsModified || x.IsRemoved);

        [ImportingConstructor]
        public ClassInfoTab(
            [ImportMany] IEnumerable<Lazy<IObjectPropertyEditor>> smartEditControls,
            [Import("Default")] Lazy<IObjectPropertyEditor> defaultControl
        )
        {
            _smartEditControls = smartEditControls;
            _defaultControl = defaultControl.Value;

            InitializeComponent();
            CreateHandle();

            _tableValues = new ClassValues();
            _document = new WeakReference<MapDocument>(null);
        }

        public bool IsInContext(IContext context, List<IMapObject> objects)
        {
            return context.TryGet("ActiveDocument", out MapDocument _) &&
                   objects.Any(x => x.Data.GetOne<EntityData>() != null);
        }

        public async Task SetObjects(MapDocument document, List<IMapObject> objects)
        {
            _document = new WeakReference<MapDocument>(document);
            _gameData = null;
            if (document != null) _gameData = await document.Environment.GetGameData();
            if (_gameData == null) _gameData = new GameData();
            if (objects == null) objects = new List<IMapObject>();
            this.InvokeLater(() =>
            {
                UpdateObjects(objects);
            });
        }

        public IEnumerable<IOperation> GetChanges(MapDocument document, List<IMapObject> objects)
        {
            // Update the class if it has changed (this doesn't touch keyvalues)
            if (_tableValues.ClassChanged)
            {
                foreach (IMapObject o in objects)
                {
                    yield return new EditEntityDataName(o.ID, _tableValues.NewClass.Name);
                }
            }

            // Update the keyvalues if they have changed
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (TableValue tv in _tableValues)
            {
                // Remove original key if it's changed
                if (tv.Key != tv.OriginalKey) values[tv.OriginalKey] = null;

                // Removed -> delete key, updated -> set key
                if (tv.IsRemoved) values[tv.Key] = null;
                else if (tv.IsAdded || tv.IsModified) values[tv.Key] = tv.Value;
            }

            // No changes - carry on
            if (values.Count == 0) yield break;

            // Only modify objects that already have entity data
            foreach (IMapObject obj in objects.Where(x => x.Data.GetOne<EntityData>() != null))
            {
                yield return new EditEntityDataProperties(obj.ID, values);
            }
        }

        private void UpdateObjects(List<IMapObject> objects)
        {
            SuspendLayout();

            pnlSmartEdit.Controls.Clear();

            // Update the keyvalues
            _tableValues = new ClassValues(_gameData, objects);

            bool solidTypes = objects.FindAll(x => x is Entity && x.Hierarchy.HasChildren).Any();
            bool pointTypes = objects.FindAll(x => x is Entity && !x.Hierarchy.HasChildren).Any();

            object[] gameDataClasses = _gameData.Classes
                .Where(x => x.ClassType != ClassType.Base)
                .Where(x => solidTypes || x.ClassType != ClassType.Solid)
                .Where(x => pointTypes || x.ClassType == ClassType.Solid)
                .OrderBy(x => x.Name, StringComparer.InvariantCultureIgnoreCase)
                .Select(x => x.Name)
                .OfType<object>()
                .ToArray();

            // Update the class list
            cmbClass.BeginUpdate();
            cmbClass.Items.Clear();
            cmbClass.Items.AddRange(gameDataClasses);

            HashSet<GameDataObject> classes = _tableValues.OriginalClasses.ToHashSet();
            if (classes.Count == 0) cmbClass.Text = "";
            else if (classes.Count > 1) cmbClass.Text = MultipleClassesText + @" " + string.Join("; ", classes.Select(x => x.Name));
            else if (classes.Count == 1) cmbClass.Text = classes.First().Name;

            cmbClass.EndUpdate();

            cmbClass.Enabled = !objects.Any(x => x is Root);

            UpdateTable();

            ResumeLayout();
        }

        /// <summary>
        /// When the list of table values has been altered, we need to update all the items in the table.
        /// </summary>
        private void UpdateTable()
        {
            bool smartEdit = btnSmartEdit.Checked;

            lstKeyValues.BeginUpdate();

            lstKeyValues.Items.Clear();

            angAngles.Enabled = false;
            angAngles.Angle = 0;

            foreach (TableValue tv in _tableValues.OrderBy(x => x.IsAdded ? 1 : x.IsRemoved ? 2 : 0))
            {
                string keyText = tv.Key;
                string valText = tv.Value;

                if (smartEdit)
                {
                    keyText = tv.DisplayText;
                    valText = tv.DisplayValue;
                }

                if (tv.IsAdded) keyText += " [+]";
                else if (tv.IsRemoved) keyText += " [-]";
                else if (tv.IsModified) keyText += " [*]";

                lstKeyValues.Items.Add(new ListViewItem(keyText)
                {
                    Tag = tv,
                    BackColor = tv.Colour
                }).SubItems.Add(valText);

                if (tv.Key == "angles")
                {
                    angAngles.Enabled = true;
                    angAngles.AnglePropertyString = tv.Value;
                }
            }

            lstKeyValues.EndUpdate();
        }

        /// <summary>
        /// When only the properties of the table values have changed, we just need to update the display properties of the table.
        /// </summary>
        private void RefreshTable()
        {
            bool smartEdit = btnSmartEdit.Checked;

            foreach (ListViewItem lv in lstKeyValues.Items.OfType<ListViewItem>())
            {
                TableValue tv = (TableValue)lv.Tag;

                string keyText = tv.Key;
                string valText = tv.Value;

                if (smartEdit)
                {
                    keyText = tv.DisplayText;
                    valText = tv.DisplayValue;
                }

                if (tv.IsAdded) keyText += " [+]";
                else if (tv.IsRemoved) keyText += " [-]";
                else if (tv.IsModified) keyText += " [*]";

                lv.BackColor = tv.Colour;
                lv.SubItems[0].Text = keyText;
                lv.SubItems[1].Text = valText;

                if (tv.Key == "angles")
                {
                    angAngles.AnglePropertyString = tv.Value;
                }
            }
        }

        /// <summary>
        /// A keyvalue record has been selected, need to show the value editor
        /// </summary>
        private void SelectedPropertyChanged(object sender, EventArgs e)
        {
            ListViewItem sel = lstKeyValues.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            TableValue tv = sel?.Tag as TableValue;

            if (_currentEditor != null)
            {
                _currentEditor.NameChanged -= NameChanged;
                _currentEditor.ValueChanged -= ValueChanged;
                _currentEditor = null;
            }

            txtHelp.Text = "";
            pnlSmartEdit.Controls.Clear();

            if (tv != null)
            {
                Property prop = btnSmartEdit.Checked ? tv.GameDataProperty : null;
                VariableType type = prop?.VariableType ?? VariableType.Void;
                _currentEditor = _smartEditControls
                                     .Select(x => x.Value)
                                     .OrderBy(x => x.PriorityHint)
                                     .FirstOrDefault(x => x.SupportsType(type)) ?? _defaultControl;

                _document.TryGetTarget(out MapDocument doc);
                _currentEditor.SetProperty(doc, tv.OriginalKey, tv.NewKey, tv.Value, prop);
                pnlSmartEdit.Controls.Add(_currentEditor.Control);

                _currentEditor.NameChanged += NameChanged;
                _currentEditor.ValueChanged += ValueChanged;

                if (prop != null && !string.IsNullOrWhiteSpace(prop.Description))
                {
                    txtHelp.Text = prop.Description.Trim();
                }
            }
        }

        /// <summary>
        /// User has changed the key of the selected property
        /// </summary>
        private void NameChanged(object sender, string key)
        {
            ListViewItem sel = lstKeyValues.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            TableValue tv = sel?.Tag as TableValue;
            if (tv == null) return;

            tv.NewKey = key;

            // The key of this item has changed, we need to update the gamedata as well
            tv.GameDataProperty = _tableValues.OriginalClasses.SelectMany(x => x.Properties)
                                      .FirstOrDefault(x => string.Equals(x.Name, key, StringComparison.InvariantCultureIgnoreCase))
                                  ?? new Property(key, VariableType.String);

            RefreshTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
        }

        /// <summary>
        /// User has changed the value of the selected property
        /// </summary>
        private void ValueChanged(object sender, string value)
        {
            ListViewItem sel = lstKeyValues.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            TableValue tv = sel?.Tag as TableValue;
            if (tv == null) return;

            tv.NewValue = value;
            RefreshTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
        }

        /// <summary>
        /// User has changed the angles via the circle widget
        /// </summary>
        private void SetAngleValue(object sender, EventArgs e)
        {
            TableValue angles = _tableValues.FirstOrDefault(x => x.Key == "angles");
            if (angles == null) return;

            string ps = angAngles.AnglePropertyString;
            if (ps == angles.NewValue) return;

            angles.NewValue = ps;

            RefreshTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
        }

        /// <summary>
        /// User has modified the class name, recalculate the keyvalues table.
        /// </summary>
        private void ClassChanged(object sender, EventArgs e)
        {
            string txt = (cmbClass.Text ?? "").ToLower();
            if (_tableValues.NewClass == null && string.Equals(txt, _tableValues.OriginalClass.ToLower(), StringComparison.InvariantCultureIgnoreCase)) return;

            GameDataObject newClass = _gameData.Classes.FirstOrDefault(x => x.ClassType != ClassType.Base && (x.Name ?? "").ToLower() == txt) ?? new GameDataObject(txt, "", ClassType.Any);
            _tableValues.NewClass = newClass;

            List<string> keys = _tableValues.Select(x => x.NewKey.ToLower()).Union(newClass.Properties.Select(x => (x.Name ?? "").ToLower())).ToList();
            foreach (string key in keys)
            {
                // Never include spawnflags
                if (key == "spawnflags") continue;

                TableValue origKey = _tableValues.FirstOrDefault(x => x.NewKey.ToLower() == key);
                Property newKey = newClass.Properties.FirstOrDefault(x => (x.Name ?? "").ToLower() == key);
                
                if (origKey != null && newKey != null)
                {
                    // Key was present originally, so if it's marked as removed we should undo that.
                    origKey.IsRemoved = false;
                }
                else if (origKey != null)
                {
                    // Key was present but isn't anymore. If it's a new key, remove it entirely, otherwise, mark it as removed.
                    if (origKey.IsAdded) _tableValues.Remove(origKey);
                    else origKey.IsRemoved = true;
                }
                else if (newKey != null)
                {
                    // Brand new key, mark it as added and add it to the list.
                    _tableValues.Add(new TableValue(newKey, key, new [] { newKey.DefaultValue }) { IsAdded = true });
                }
            }

            UpdateTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
        }

        /// <summary>
        /// User has toggled smart edit, update the table and the current value editor
        /// </summary>
        private void SmartEditToggled(object sender, EventArgs e)
        {
            RefreshTable();
            SelectedPropertyChanged(this, e);
        }

        /// <summary>
        /// User clicks the button to add a new key
        /// </summary>
        private void AddKeyClicked(object sender, EventArgs e)
        {
            // Add a new key with an automatically created name
            string key = "new";
            TableValue tv = new TableValue(new Property(key, VariableType.String), key, new string[0])
            {
                IsAdded = true,
                NewValue = ""
            };
            _tableValues.Add(tv);

            UpdateTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));

            // Select the new key
            lstKeyValues.SelectedIndices.Clear();
            ListViewItem sel = lstKeyValues.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Tag == tv);
            int idx = lstKeyValues.Items.IndexOf(sel);
            if (idx >= 0) lstKeyValues.SelectedIndices.Add(idx);
        }

        /// <summary>
        /// User clicks the button to remove the current key
        /// </summary>
        private void DeleteKeyClicked(object sender, EventArgs e)
        {
            ListViewItem sel = lstKeyValues.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            TableValue tv = sel?.Tag as TableValue;
            if (tv == null) return;

            if (tv.IsAdded) _tableValues.Remove(tv);
            else tv.IsRemoved = true;

            UpdateTable();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasChanges)));
        }

        /// <summary>
        /// Stores the state of the selection before and after modifications have been made
        /// </summary>
        private class ClassValues : List<TableValue>
        {
            public List<GameDataObject> OriginalClasses { get; }
            public GameDataObject NewClass { get; set; }

            public string OriginalClass => string.Join(", ", OriginalClasses.Select(x => x.Name));
            public bool ClassChanged => NewClass != null && !string.Equals(NewClass.Name, OriginalClass, StringComparison.InvariantCultureIgnoreCase);

            public ClassValues()
            {
                OriginalClasses = new List<GameDataObject>();
                NewClass = null;
            }

            public ClassValues(GameData gameData, List<IMapObject> objects)
            {
                List<EntityData> datas = objects.Select(x => x.Data.GetOne<EntityData>()).Where(x => x != null).ToList();

                Dictionary<string, GameDataObject> gameDataClasses = gameData.Classes
                    .Where(x => x.ClassType != ClassType.Base)
                    .GroupBy(x => (x.Name ?? "").ToLower())
                    .ToDictionary(x => x.Key, x => x.First());

                // Gather information about the classes of the selection
                OriginalClasses = new List<GameDataObject>();
                NewClass = null;

                foreach (EntityData d in datas)
                {
                    // For unknown classes, create a dummy object to represent them
                    string n = (d.Name ?? "").ToLower();
                    GameDataObject cls = gameDataClasses.ContainsKey(n) ? gameDataClasses[n] : new GameDataObject(n, "", ClassType.Any);
                    OriginalClasses.Add(cls);
                }

                List<string> keys = OriginalClasses.SelectMany(x => x.Properties)
                    .Select(x => (x.Name ?? "").ToLower())
                    .Union(datas.SelectMany(x => x.Properties.Keys).Select(x => x.ToLower()))
                    .ToList();
                foreach (string key in keys)
                {
                    // Spawnflags are handled by the flags tab
                    if (key == "spawnflags") continue;

                    // Get the (first) property for this key
                    Property prop = OriginalClasses.SelectMany(x => x.Properties).FirstOrDefault(x => string.Equals(x.Name, key, StringComparison.InvariantCultureIgnoreCase))
                               ?? new Property(key, VariableType.String);
                    TableValue val = new TableValue(prop, key, datas.Select(x => x.Get<string>(key)));
                    Add(val);
                }
            }
        }

        /// <summary>
        /// An individual value in the keyvalues table. May represent multiple values.
        /// </summary>
        private class TableValue
        {
            public Property GameDataProperty { get; set; }

            public string OriginalKey { get; }
            public string NewKey { get; set; }
            public string Key => NewKey ?? OriginalKey;

            public List<string> OriginalValues { get; }
            public string NewValue { get; set; }

            public bool IsModified => NewValue != null && !string.Equals(OriginalValue, NewValue)
                                   || NewKey != null && !string.Equals(OriginalKey, NewKey);

            public bool IsAdded { get; set; }
            public bool IsRemoved { get; set; }

            public string Value => NewValue ?? OriginalValue;
            public string OriginalValue => string.Join(", ", OriginalValues.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());

            public string DisplayText => GameDataProperty?.DisplayText() ?? NewKey;

            public string DisplayValue => string.IsNullOrWhiteSpace(Value)
                ? GameDataProperty?.DefaultValue
                : GameDataProperty?.Options.FirstOrDefault(x => x.Key == Value)?.Description ?? Value;

            public Color Colour
            {
                get
                {
                    if (IsAdded) return Color.LightBlue;
                    if (IsRemoved) return Color.LightPink;
                    if (IsModified) return Color.LightGreen;
                    return Color.Transparent;
                }
            }

            public TableValue(Property gameDataProperty, string key, IEnumerable<string> values)
            {
                GameDataProperty = gameDataProperty;
                OriginalKey = key;
                NewKey = key;
                OriginalValues = values.ToList();
                IsAdded = IsRemoved = false;
            }
        }
    }
}
