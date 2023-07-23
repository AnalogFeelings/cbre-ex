using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CBRE.Common.Translations;
using CBRE.DataStructures.GameData;
using CBRE.FileSystem;
using CBRE.Providers.GameData;
using CBRE.Providers.Texture;
using CBRE.Shell;

namespace CBRE.BspEditor.Environment.ContainmentBreach
{
    public partial class BlitzEnvironmentEditor : UserControl, IEnvironmentEditor
    {
        public event EventHandler EnvironmentChanged;
        public Control Control => this;

        private readonly IGameDataProvider _fgdProvider = Common.Container.Get<IGameDataProvider>("Fgd");
        private readonly ITexturePackageProvider _wadProvider = Common.Container.Get<ITexturePackageProvider>("Generic");

        public IEnvironment Environment
        {
            get => GetEnvironment();
            set => SetEnvironment(value as BlitzEnvironment);
        }

        public BlitzEnvironmentEditor()
        {
            InitializeComponent();

            txtGameDir.TextChanged += OnEnvironmentChanged;
            cmbBaseGame.SelectedIndexChanged += OnEnvironmentChanged;

            cmbDefaultPointEntity.SelectedIndexChanged += OnEnvironmentChanged;
            cmbDefaultBrushEntity.SelectedIndexChanged += OnEnvironmentChanged;
            chkOverrideMapSize.CheckedChanged += OnEnvironmentChanged;
            cmbMapSizeOverrideLow.SelectedIndexChanged += OnEnvironmentChanged;
            cmbMapSizeOverrideHigh.SelectedIndexChanged += OnEnvironmentChanged;

            nudDefaultTextureScale.ValueChanged += OnEnvironmentChanged;

            cklTexturePackages.ItemCheck += (s, e) => this.InvokeLater(() => OnEnvironmentChanged(s, e)); // So it happens after the checkstate has changed, not before
        }

        public void Translate(ITranslationStringProvider strings)
        {
            CreateHandle();
            var prefix = GetType().FullName;

            grpDirectories.Text = strings.GetString(prefix, "Directories");
            grpFgds.Text = strings.GetString(prefix, "GameDataFiles");
            grpTextures.Text = strings.GetString(prefix, "Textures");
            
            btnAddFgd.Text = btnAddTextures.Text = strings.GetString(prefix, "Add");
            btnRemoveFgd.Text = btnRemoveTextures.Text = strings.GetString(prefix, "Remove");

            colFgdName.Text = colWadName.Text = strings.GetString(prefix, "Name");
            colFgdPath.Text = colWadPath.Text = strings.GetString(prefix, "Path");

            lblGameDir.Text = strings.GetString(prefix, "GameDirectory");
            lblBaseGame.Text = strings.GetString(prefix, "BaseDirectory");
            
            lblDefaultPointEntity.Text = strings.GetString(prefix, "DefaultPointEntity");
            lblDefaultBrushEntity.Text = strings.GetString(prefix, "DefaultBrushEntity");
            lblMapSizeOverrideLow.Text = strings.GetString(prefix, "Low");
            lblMapSizeOverrideHigh.Text = strings.GetString(prefix, "High");
            chkOverrideMapSize.Text = strings.GetString(prefix, "OverrideMapSize");

            lblDefaultTextureScale.Text = strings.GetString(prefix, "DefaultTextureScale");
            lblTexturePackageExclusions.Text = strings.GetString(prefix, "TexturePackagesToInclude");
            chkToggleAllTextures.Text = strings.GetString(prefix, "ToggleAll");
            lblAdditionalTexturePackages.Text = strings.GetString(prefix, "AdditionalTexturePackages");
        }

        private void OnEnvironmentChanged(object sender, EventArgs e)
        {
            EnvironmentChanged?.Invoke(this, e);
        }

        public void SetEnvironment(BlitzEnvironment env)
        {
            if (env == null) env = new BlitzEnvironment();

            txtGameDir.Text = env.GameDirectory;
            cmbBaseGame.SelectedItem = env.GraphicsDirectory;

            lstFgds.Items.Clear();
            foreach (var fileName in env.FgdFiles)
            {
                lstFgds.Items.Add(new ListViewItem(new[] { Path.GetFileName(fileName), fileName }) {ToolTipText = fileName});
            }
            UpdateFgdList();

            cmbDefaultPointEntity.SelectedItem = env.DefaultPointEntity;
            cmbDefaultBrushEntity.SelectedItem = env.DefaultBrushEntity;
            chkOverrideMapSize.Checked = env.OverrideMapSize;
            cmbMapSizeOverrideLow.SelectedItem = Convert.ToString(env.MapSizeLow, CultureInfo.InvariantCulture);
            cmbMapSizeOverrideHigh.SelectedItem = Convert.ToString(env.MapSizeHigh, CultureInfo.InvariantCulture);

            nudDefaultTextureScale.Value = env.DefaultTextureScale;

            cklTexturePackages.Items.Clear();
            UpdateTexturePackages();

            lstAdditionalTextures.Items.Clear();
            UpdateWadList();
        }

        public BlitzEnvironment GetEnvironment()
        {
            return new BlitzEnvironment()
            {
                GameDirectory = txtGameDir.Text,
                GraphicsDirectory = Convert.ToString(cmbBaseGame.SelectedItem, CultureInfo.InvariantCulture),

                FgdFiles = lstFgds.Items.OfType<ListViewItem>().Select(x => x.SubItems[1].Text).Where(File.Exists).ToList(),
                DefaultPointEntity = Convert.ToString(cmbDefaultPointEntity.SelectedItem, CultureInfo.InvariantCulture),
                DefaultBrushEntity = Convert.ToString(cmbDefaultBrushEntity.SelectedItem, CultureInfo.InvariantCulture),
                OverrideMapSize = chkOverrideMapSize.Checked,
                MapSizeLow = decimal.TryParse(Convert.ToString(cmbMapSizeOverrideLow.SelectedItem, CultureInfo.InvariantCulture), out decimal l) ? l : 0,
                MapSizeHigh = decimal.TryParse(Convert.ToString(cmbMapSizeOverrideHigh.SelectedItem, CultureInfo.InvariantCulture), out decimal h) ? h : 0,

                DefaultTextureScale = nudDefaultTextureScale.Value
            };
        }

        // Directories

        private void BrowseGameDirectory(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(txtGameDir.Text)) fbd.SelectedPath = txtGameDir.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtGameDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void GameDirectoryTextChanged(object sender, EventArgs e)
        {
            UpdateGameDirectory();
            UpdateTexturePackages();
        }

        private void UpdateGameDirectory()
        {
            var dir = txtGameDir.Text;
            if (!Directory.Exists(dir)) return;

            // Set game/mod directories
            var bse = cmbBaseGame.SelectedItem ?? "";
            
            cmbBaseGame.Items.Clear();

            var mods = Directory.GetDirectories(dir).Select(Path.GetFileName);
            var ignored = new[] { "gldrv", "logos", "logs", "errorlogs", "platform", "config" };

            var range = mods.Where(x => !ignored.Contains(x.ToLowerInvariant())).OfType<object>().ToArray();
            cmbBaseGame.Items.AddRange(range);

            if (cmbBaseGame.Items.Contains(bse)) cmbBaseGame.SelectedItem = bse;
            else if (cmbBaseGame.Items.Count > 0) cmbBaseGame.SelectedIndex = 0;
        }

        // Game data files

        public string FgdFilesLabel { get; set; } = "Forge Game Data files";

        private void BrowseFgd(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = FgdFilesLabel + @" (*.fgd)|*.fgd", Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fileName in ofd.FileNames)
                    {
                        lstFgds.Items.Add(new ListViewItem(new[]
                        {
                            Path.GetFileName(fileName),
                            fileName
                        }) {ToolTipText = fileName});
                    }
                    UpdateFgdList();
                    OnEnvironmentChanged(this, EventArgs.Empty);
                }
            }
        }
        
        private void RemoveFgd(object sender, EventArgs e)
        {
            if (lstFgds.SelectedItems.Count > 0)
            {
                foreach (var i in lstFgds.SelectedItems.OfType<ListViewItem>().ToList())
                {
                    lstFgds.Items.Remove(i);
                }
                UpdateFgdList();
                OnEnvironmentChanged(this, EventArgs.Empty);
            }
        }

        private void UpdateFgdList()
        {
            lstFgds.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            var entities = new List<GameDataObject>();
            if (_fgdProvider != null)
            {
                var files = lstFgds.Items.OfType<ListViewItem>().Select(x => x.SubItems[1].Text).Where(File.Exists).Where(_fgdProvider.IsValidForFile);
                try
                {
                    var gd = _fgdProvider.GetGameDataFromFiles(files);
                    entities.AddRange(gd.Classes);
                }
                catch
                {
                    //
                }
            }

            var selPoint = cmbDefaultPointEntity.SelectedItem as string;
            var selBrush = cmbDefaultBrushEntity.SelectedItem as string;

            cmbDefaultPointEntity.BeginUpdate();
            cmbDefaultBrushEntity.BeginUpdate();

            cmbDefaultPointEntity.Items.Clear();
            cmbDefaultBrushEntity.Items.Clear();

            cmbDefaultPointEntity.Items.Add("");
            cmbDefaultBrushEntity.Items.Add("");

            foreach (var gdo in entities.OrderBy(x => x.Name, StringComparer.InvariantCultureIgnoreCase))
            {
                if (gdo.ClassType == ClassType.Solid) cmbDefaultBrushEntity.Items.Add(gdo.Name);
                else if (gdo.ClassType != ClassType.Base) cmbDefaultPointEntity.Items.Add(gdo.Name);
            }

            var idx = cmbDefaultBrushEntity.Items.IndexOf(selBrush ?? "");
            if (idx >= 0) cmbDefaultBrushEntity.SelectedIndex = idx;
            idx = cmbDefaultPointEntity.Items.IndexOf(selPoint ?? "");
            if (idx >= 0) cmbDefaultPointEntity.SelectedIndex = idx;

            cmbDefaultPointEntity.EndUpdate();
            cmbDefaultBrushEntity.EndUpdate();
        }

        public Dictionary<string, bool> GetTexturePackageSelection()
        {
            var d = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

            var packages = cklTexturePackages.Items.OfType<string>().ToList();
            for (var i = 0; i < packages.Count; i++)
            {
                var name = packages[i];
                var state = cklTexturePackages.GetItemCheckState(i);
                if (state == CheckState.Indeterminate) continue;
                d[name] = state == CheckState.Checked;
            }

            return d;
        }

        private void BaseGameDirectoryChanged(object sender, EventArgs e)
        {
            UpdateTexturePackages();
        }

        private void ModDirectoryChanged(object sender, EventArgs e)
        {
            UpdateTexturePackages();
        }

        private void IncludeBuildToolsChanged(object sender, EventArgs e)
        {
            UpdateTexturePackages();
        }

        private void UpdateTexturePackages()
        {
            var state = GetTexturePackageSelection();

            var directories = new List<string>();
            if (cmbBaseGame.SelectedItem is string sbg)
            {
                directories.AddRange(new[]
                {
                    Path.Combine(txtGameDir.Text, sbg),
                    Path.Combine(txtGameDir.Text, sbg + "_hd"),
                    Path.Combine(txtGameDir.Text, sbg + "_downloads"),
                    Path.Combine(txtGameDir.Text, sbg + "_addon"),
                });
            }

            directories = directories.Distinct().Where(Directory.Exists).ToList();

            if (directories.Any())
            {
                try
                {
                    //var packages = _wadProvider.GetPackagesInFile(null, new CompositeFile(
                    //    new NativeFile(txtGameDir.Text),
                    //    directories.Select(x => new NativeFile(x))
                    //)).ToList();

                    //// Exclude game-internal packages that can not be used
                    //string[] _internalWads = new[] { "cached.wad", "fonts.wad", "gfx.wad", "tempdecal.wad" };
                    //foreach (var pr in packages)
                    //{
                    //    if (!state.ContainsKey(pr.Name) && !_internalWads.Contains(pr.Name)) 
                    //        state[pr.Name] = true;
                    //}

                    //foreach (var key in state.Keys.ToList())
                    //{
                    //    if (packages.All(x => !string.Equals(x.Name, key, StringComparison.InvariantCultureIgnoreCase))) state.Remove(key);
                    //}
                }
                catch
                {
                    //
                }
            }
            cklTexturePackages.BeginUpdate();

            cklTexturePackages.Items.Clear();
            foreach (var kv in state.OrderBy(x => x.Key, StringComparer.InvariantCultureIgnoreCase))
            {
                cklTexturePackages.Items.Add(kv.Key, kv.Value);
            }

            cklTexturePackages.EndUpdate();
        }

        private void ToggleAllTextures(object sender, EventArgs e)
        {
            var on = chkToggleAllTextures.Checked;
            for (var i = 0; i < cklTexturePackages.Items.Count; i++)
            {
                cklTexturePackages.SetItemChecked(i, on);
            }
        }

        public string WadFilesLabel { get; set; } = "WAD texture packages";

        private void BrowseWad(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = WadFilesLabel + @" (*.wad)|*.wad", Multiselect = true })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                foreach (var fileName in ofd.FileNames)
                {
                    lstAdditionalTextures.Items.Add(new ListViewItem(new[] { Path.GetFileName(fileName), fileName }) { ToolTipText = fileName });
                }

                UpdateWadList();
                OnEnvironmentChanged(this, EventArgs.Empty);
            }
        }

        private void RemoveWad(object sender, EventArgs e)
        {
            if (lstAdditionalTextures.SelectedItems.Count > 0)
            {
                foreach (var i in lstAdditionalTextures.SelectedItems.OfType<ListViewItem>().ToList())
                {
                    lstAdditionalTextures.Items.Remove(i);
                }
                UpdateWadList();
                OnEnvironmentChanged(this, EventArgs.Empty);
            }
        }

        private void UpdateWadList()
        {
            lstAdditionalTextures.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
