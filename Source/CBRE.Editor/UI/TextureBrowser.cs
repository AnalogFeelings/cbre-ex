using CBRE.Common.Mediator;
using CBRE.Editor.Documents;
using CBRE.Localization;
using CBRE.Providers.Texture;
using CBRE.QuickForms;
using CBRE.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI
{
    public partial class TextureBrowser : Form
    {
        public TextureBrowser()
        {
            int sz = GetMemory("SizeMode", 2);
            int so = GetMemory("SortBy", 0);

            InitializeComponent();
            TextureList.TextureSelected += TextureSelected;
            TextureList.SelectionChanged += SelectionChanged;
            SizeCombo.SelectedIndex = 2;
            _textures = new List<TextureItem>();
            _packages = new List<TexturePackage>();
            SelectedTexture = null;

            SortOrderCombo.Items.Clear();
            foreach (object tso in Enum.GetValues(typeof(TextureListPanel.TextureSortOrder)))
            {
                SortOrderCombo.Items.Add(tso);
            }
            SortOrderCombo.SelectedIndex = 0;

            FilterTextbox.Text = GetMemory("Filter", "");
            UsedTexturesOnlyBox.Checked = GetMemory("UsedTexturesOnly", false);
            SizeCombo.SelectedIndex = sz;
            SortOrderCombo.SelectedIndex = so;
            SortDescendingCheckbox.Checked = GetMemory("SortDescending", false);

            SelectionChanged(null, TextureList.GetSelectedTextures());
        }

        protected override void OnLoad(EventArgs e)
        {
            FilterTextbox.SelectAll();
            base.OnLoad(e);
        }

        private void SelectionChanged(object sender, IEnumerable<TextureItem> selection)
        {
            List<TextureItem> list = selection.ToList();
            if (!list.Any())
            {
                TextureNameLabel.Text = Local.LocalString("paste_special.name", Local.LocalString("filetype.none"));
                TextureSizeLabel.Text = Local.LocalString("paste_special.size", Local.LocalString("filetype.none"));
            }
            else if (list.Count == 1)
            {
                TextureItem t = list[0];
                TextureNameLabel.Text = Local.LocalString("paste_special.name", t.Name);
                TextureSizeLabel.Text = Local.LocalString("paste_special.size", t.Width + " x " + t.Height);
            }
            else
            {
                TextureNameLabel.Text = Local.LocalString("paste_special.name.many", list.Count);
                TextureSizeLabel.Text = Local.LocalString("paste_special.size", Local.LocalString("paste_special.size.unable_display"));
            }
        }

        public TextureItem SelectedTexture { get; set; }
        private readonly List<TextureItem> _textures;
        private readonly List<TexturePackage> _packages;

        private void SetMemory<T>(string name, T value)
        {
            name = GetType().Name + '.' + name;
            if (DocumentManager.CurrentDocument != null) DocumentManager.CurrentDocument.SetMemory(name, value);
        }

        private T GetMemory<T>(string name, T def = default(T))
        {
            name = GetType().Name + '.' + name;
            return DocumentManager.CurrentDocument != null ? DocumentManager.CurrentDocument.GetMemory(name, def) : def;
        }

        private void TextureSelected(object sender, TextureItem item)
        {
            SelectedTexture = item;
            Close();
        }

        public void SetTextureList(IEnumerable<TextureItem> items)
        {
            _textures.Clear();
            _textures.AddRange(items);
            _packages.Clear();
            _packages.AddRange(_textures.Select(x => x.Package).Distinct());
            UpdatePackageList();
            UpdateFavouritesList();
            UpdateTextureList();
        }

        public void SetSelectedTextures(IEnumerable<TextureItem> items)
        {
            TextureList.SetSelectedTextures(items);
        }

        public void SetFilterText(string text)
        {
            if (text != null) FilterTextbox.Text = text;
        }

        private void FilterTextboxKeyUp(object sender, KeyEventArgs e)
        {
            SetMemory("Filter", FilterTextbox.Text);
            UpdateTextureList();
        }

        private void SelectedPackageChanged(object sender, TreeViewEventArgs e)
        {
            FavouritesTree.SelectedNode = null;
            TreeNode package = PackageTree.SelectedNode;
            string key = package == null ? null : package.Name;
            if (String.IsNullOrWhiteSpace(key)) key = null;
            SetMemory("SelectedPackage", key);
            SetMemory("SelectedFavourite", (string)null);

            UpdateTextureList();
        }

        private void SelectedFavouriteChanged(object sender, TreeViewEventArgs e)
        {
            PackageTree.SelectedNode = null;
            TreeNode favourite = FavouritesTree.SelectedNode;
            string key = favourite == null ? null : favourite.Name;
            if (String.IsNullOrWhiteSpace(key)) key = null;
            SetMemory("SelectedFavourite", key);
            SetMemory("SelectedPackage", (string)null);

            UpdateTextureList();
        }

        private void UsedTexturesOnlyChanged(object sender, EventArgs e)
        {
            SetMemory("UsedTexturesOnly", UsedTexturesOnlyBox.Checked);
            UpdateTextureList();
        }

        private void UpdatePackageList()
        {
            TreeNode selected = PackageTree.SelectedNode;
            string selectedKey = selected == null ? GetMemory<string>("SelectedPackage") : selected.Name;
            IEnumerable<TexturePackage> packages = _textures.Select(x => x.Package).Distinct();
            PackageTree.Nodes.Clear();
            TreeNode parent = PackageTree.Nodes.Add("", Local.LocalString("paste_special.all_packages"));
            TreeNode reselect = null;
            foreach (TexturePackage tp in packages.OrderBy(x => x.ToString()))
            {
                TreeNode node = parent.Nodes.Add(tp.ToString(), tp + " (" + tp.Items.Count + ")");
                if (selectedKey == node.Name) reselect = node;
            }
            PackageTree.SelectedNode = reselect;
            PackageTree.ExpandAll();
        }

        private void UpdateFavouritesList()
        {
            TreeNode selected = FavouritesTree.SelectedNode;
            string selectedKey = selected == null ? GetMemory<string>("SelectedFavourite") : selected.Name;
            List<FavouriteTextureFolder> favourites = SettingsManager.FavouriteTextureFolders;
            FavouritesTree.Nodes.Clear();
            TreeNode parent = FavouritesTree.Nodes.Add("", Local.LocalString("paste_special.all_packages"));
            TreeNode reselect;
            AddFavouriteTextureFolders(parent, favourites, selectedKey, out reselect);
            FavouritesTree.SelectedNode = reselect;
            FavouritesTree.ExpandAll();
        }

        private void AddFavouriteTextureFolders(TreeNode parent, IEnumerable<FavouriteTextureFolder> folders, string selectedKey, out TreeNode reselect)
        {
            reselect = null;
            foreach (FavouriteTextureFolder fav in folders)
            {
                List<TextureItem> items = GetTexturesInFavourite(fav);
                TreeNode node = parent.Nodes.Add(parent.Tag + "/" + fav.Name, fav.Name + " (" + items.Count + ")");
                AddFavouriteTextureFolders(node, fav.Children, selectedKey, out reselect);
                if (selectedKey == node.Name) reselect = node;
                node.Tag = fav;
            }
        }

        private List<TextureItem> GetTexturesInFavourite(FavouriteTextureFolder fav)
        {
            return _textures.Where(x => InFavouriteList(fav.Items, x)).ToList();
        }

        private IEnumerable<TextureItem> GetPackageTextures()
        {
            TreeNode package = PackageTree.SelectedNode;
            string key = package == null ? null : package.Name;
            if (String.IsNullOrWhiteSpace(key)) key = null;
            TexturePackage p = _packages.FirstOrDefault(x => x.ToString() == key);
            if (p != null) return p.Items.Values;
            return _textures.Where(x => key == null || key == x.Package.ToString());
        }

        private IEnumerable<TextureItem> GetFavouriteFolderTextures()
        {
            TreeNode folder = FavouritesTree.SelectedNode;
            FavouriteTextureFolder node = folder == null ? null : folder.Tag as FavouriteTextureFolder;
            List<FavouriteTextureFolder> nodes = new List<FavouriteTextureFolder>();
            CollectNodes(nodes, node == null ? SettingsManager.FavouriteTextureFolders : node.Children);
            if (node != null) nodes.Add(node);
            List<string> favs = nodes.SelectMany(x => x.Items).ToList();
            return _textures.Where(x => InFavouriteList(favs, x));
        }

        private bool InFavouriteList(IEnumerable<string> favs, TextureItem ti)
        {
            return favs.Contains(ti.Name, StringComparer.OrdinalIgnoreCase);
        }

        private void CollectNodes(List<FavouriteTextureFolder> favs, IEnumerable<FavouriteTextureFolder> folders)
        {
            foreach (FavouriteTextureFolder f in folders)
            {
                favs.Add(f);
                CollectNodes(favs, f.Children);
            }
        }

        private void UpdateTextureList()
        {
            IEnumerable<TextureItem> list = FavouritesTree.SelectedNode != null ? GetFavouriteFolderTextures() : GetPackageTextures();
            if (!String.IsNullOrEmpty(FilterTextbox.Text))
            {
                list = list.Where(x => x.Name.ToLower().Contains(FilterTextbox.Text.ToLower()));
            }
            if (UsedTexturesOnlyBox.Checked && DocumentManager.CurrentDocument != null)
            {
                List<string> used = DocumentManager.CurrentDocument.GetUsedTextures().ToList();
                list = list.Where(x => used.Any(y => String.Equals(x.Name, y, StringComparison.OrdinalIgnoreCase)));
            }
            List<TextureItem> l = list.ToList();
            TextureList.SetTextureList(l);

            TextureItem sel = DocumentManager.CurrentDocument == null ? null : DocumentManager.CurrentDocument.TextureCollection.SelectedTexture;
            if (sel != null)
            {
                TextureList.SetSelectedTextures(new[] { sel });
                TextureList.ScrollToItem(sel);
            }
        }

        private void SizeValueChanged(object sender, EventArgs e)
        {
            SetMemory("SizeMode", SizeCombo.SelectedIndex);
            TextureList.ImageSize = SizeCombo.SelectedIndex == 0 ? 0 : Convert.ToInt32(SizeCombo.SelectedItem);
        }

        private static readonly char[] AllowedSpecialChars = "!@#$%^&*()-_=+<>,.?/'\"\\;:[]{}`~".ToCharArray();

        private void TextureBrowserKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!TextureList.Focused) return;

            if (e.KeyChar == 8 && FilterTextbox.Text.Length > 0)
            {
                if (FilterTextbox.SelectionLength > 0)
                {
                    FilterTextbox.Text = FilterTextbox.Text.Substring(0, FilterTextbox.SelectionStart) +
                                         FilterTextbox.Text.Substring(FilterTextbox.SelectionStart + FilterTextbox.SelectionLength);
                }
                else
                {
                    FilterTextbox.Text = FilterTextbox.Text.Substring(0, FilterTextbox.Text.Length - 1);
                }
                FilterTextboxKeyUp(null, null);
            }
            else if ((e.KeyChar >= 'a' && e.KeyChar <= 'z')
                  || (e.KeyChar >= '0' && e.KeyChar <= '9')
                  || AllowedSpecialChars.Contains(e.KeyChar))
            {
                if (FilterTextbox.SelectionLength > 0)
                {

                    FilterTextbox.Text = FilterTextbox.Text.Substring(0, FilterTextbox.SelectionStart) +
                                         e.KeyChar +
                                         FilterTextbox.Text.Substring(FilterTextbox.SelectionStart + FilterTextbox.SelectionLength);
                }
                else
                {
                    FilterTextbox.Text += e.KeyChar;
                }
                FilterTextboxKeyUp(null, null);
            }
        }

        private void TextureBrowserKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && FilterTextbox.SelectionLength > 0)
            {
                FilterTextbox.Text = FilterTextbox.Text.Substring(0, FilterTextbox.SelectionStart) +
                                         FilterTextbox.Text.Substring(FilterTextbox.SelectionStart + FilterTextbox.SelectionLength);
                FilterTextboxKeyUp(null, null);
            }
        }

        private void SortOrderComboIndexChanged(object sender, EventArgs e)
        {
            SetMemory("SortBy", SortOrderCombo.SelectedIndex);
            TextureList.SortOrder = (TextureListPanel.TextureSortOrder)SortOrderCombo.SelectedItem;
        }

        private void SortDescendingCheckboxChanged(object sender, EventArgs e)
        {
            SetMemory("SortDescending", SortDescendingCheckbox.Checked);
            TextureList.SortDescending = SortDescendingCheckbox.Checked;
        }

        private void DeleteFavouriteFolderButtonClicked(object sender, EventArgs e)
        {
            FavouriteTextureFolder parent = null;
            TreeNode selected = FavouritesTree.SelectedNode;
            if (selected != null && selected.Parent != null)
            {
                parent = selected.Parent.Tag as FavouriteTextureFolder;
                List<FavouriteTextureFolder> siblings = parent != null ? parent.Children : SettingsManager.FavouriteTextureFolders;
                siblings.Remove(selected.Tag as FavouriteTextureFolder);
                UpdateFavouritesList();
                UpdateTextureList();
            }
        }

        private void AddFavouriteFolderButtonClicked(object sender, EventArgs e)
        {
            FavouriteTextureFolder parent = null;
            TreeNode selected = FavouritesTree.SelectedNode;
            if (selected != null) parent = selected.Tag as FavouriteTextureFolder;
            List<FavouriteTextureFolder> siblings = parent != null ? parent.Children : SettingsManager.FavouriteTextureFolders;
            using (QuickForm qf = new QuickForm(Local.LocalString("texture_browser.folder_name")) { UseShortcutKeys = true }.TextBox(Local.LocalString("entity_report.name")).OkCancel())
            {
                if (qf.ShowDialog() != DialogResult.OK) return;

                string name = qf.String(Local.LocalString("entity_report.name"));
                string uniqName = name;
                if (String.IsNullOrWhiteSpace(name)) return;

                int counter = 1;
                while (siblings.Any(x => x.Name == uniqName))
                {
                    uniqName = name + "_" + counter;
                    counter++;
                }

                siblings.Add(new FavouriteTextureFolder { Name = uniqName });
                UpdateFavouritesList();
            }
        }

        private TreeNode _highlightedNode;

        private void FavouritesTreeDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TextureItem)) && !e.Data.GetDataPresent(typeof(List<TextureItem>))) return;

            Point pt = FavouritesTree.PointToClient(new Point(e.X, e.Y));
            TreeNode highlightedNode = FavouritesTree.GetNodeAt(pt);
            if (highlightedNode == null || !(highlightedNode.Tag is FavouriteTextureFolder)) return;

            _highlightedNode = highlightedNode;
            _highlightedNode.BackColor = Color.LightSkyBlue;
            e.Effect = DragDropEffects.Copy;
        }

        private void FavouritesTreeDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TextureItem)) || e.Data.GetDataPresent(typeof(List<TextureItem>)))
            {
                Point pt = FavouritesTree.PointToClient(new Point(e.X, e.Y));
                TreeNode dest = FavouritesTree.GetNodeAt(pt);
                if (dest != null && dest.Tag is FavouriteTextureFolder)
                {
                    List<TextureItem> data = e.Data.GetDataPresent(typeof(TextureItem))
                        ? new List<TextureItem> { (TextureItem)e.Data.GetData(typeof(TextureItem)) }
                        : (List<TextureItem>)e.Data.GetData(typeof(List<TextureItem>));
                    FavouriteTextureFolder folder = (FavouriteTextureFolder)dest.Tag;
                    foreach (TextureItem ti in data)
                    {
                        if (!folder.Items.Contains(ti.Name, StringComparer.OrdinalIgnoreCase)) folder.Items.Add(ti.Name);
                    }
                    UpdateFavouritesList();
                }
            }
            if (_highlightedNode != null) _highlightedNode.BackColor = Color.Transparent;
            _highlightedNode = null;
        }

        private void FavouritesTreeDragLeave(object sender, EventArgs e)
        {
            if (_highlightedNode != null) _highlightedNode.BackColor = Color.Transparent;
            _highlightedNode = null;
        }

        private void FavouritesTreeDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TextureItem)) && !e.Data.GetDataPresent(typeof(List<TextureItem>))) return;

            Point pt = FavouritesTree.PointToClient(new Point(e.X, e.Y));
            TreeNode highlightedNode = FavouritesTree.GetNodeAt(pt);
            if (highlightedNode == null || !(highlightedNode.Tag is FavouriteTextureFolder))
            {
                if (_highlightedNode != null) _highlightedNode.BackColor = Color.Transparent;
                _highlightedNode = null;
                e.Effect = DragDropEffects.None;
                return;
            }

            if (_highlightedNode != null) _highlightedNode.BackColor = Color.Transparent;
            _highlightedNode = highlightedNode;
            _highlightedNode.BackColor = Color.LightSkyBlue;
            e.Effect = DragDropEffects.Copy;
        }

        private void RemoveFavouriteItemButtonClicked(object sender, EventArgs e)
        {
            IEnumerable<string> selection = TextureList.GetSelectedTextures().Select(x => x.Name);

            TreeNode folder = FavouritesTree.SelectedNode;
            FavouriteTextureFolder node = folder == null ? null : folder.Tag as FavouriteTextureFolder;
            List<FavouriteTextureFolder> nodes = new List<FavouriteTextureFolder>();
            CollectNodes(nodes, node == null ? SettingsManager.FavouriteTextureFolders : node.Children);
            if (node != null) nodes.Add(node);

            nodes.ForEach(x => x.Items.RemoveAll(selection.Contains));
            UpdateFavouritesList();
            UpdateTextureList();
        }

        private void SelectButtonClicked(object sender, EventArgs e)
        {
            List<TextureItem> sel = TextureList.GetSelectedTextures().ToList();
            if (!sel.Any()) return;
            Mediator.Publish(EditorMediator.SelectMatchingTextures, sel.Select(x => x.Name).ToList());
            Close();
        }
    }
}
