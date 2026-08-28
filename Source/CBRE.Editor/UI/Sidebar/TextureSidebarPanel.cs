using CBRE.Common.Mediator;
using CBRE.Editor.Documents;
using CBRE.Localization;
using CBRE.Providers.Texture;
using CBRE.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI.Sidebar
{
    public partial class TextureSidebarPanel : UserControl, IMediatorListener
    {
        public TextureSidebarPanel()
        {
            InitializeComponent();
            SizeLabel.Text = "";

            Mediator.Subscribe(EditorMediator.DocumentActivated, this);
            Mediator.Subscribe(EditorMediator.DocumentAllClosed, this);
            Mediator.Subscribe(EditorMediator.TextureSelected, this);
        }

        public TextureItem GetSelectedTexture()
        {
            return TextureComboBox.GetSelectedTexture();
        }

        private void ApplyButtonClicked(object sender, EventArgs e)
        {
            Mediator.Publish(HotkeysMediator.ApplyCurrentTextureToSelection);
        }

        private void BrowseButtonClicked(object sender, EventArgs e)
        {
            if (DocumentManager.CurrentDocument == null) return;
            using (TextureBrowser tb = new TextureBrowser())
            {
                tb.SetTextureList(DocumentManager.CurrentDocument.TextureCollection.GetAllBrowsableItems());
                tb.ShowDialog();
                if (tb.SelectedTexture != null)
                {
                    Mediator.Publish(EditorMediator.TextureSelected, tb.SelectedTexture);
                }
            }
        }

        private void ReplaceButtonClicked(object sender, EventArgs e)
        {
            Mediator.Publish(HotkeysMediator.ReplaceTextures);
        }

        private void GroupChanged(object sender, EventArgs e)
        {
            TexturePackage tp = GroupComboBox.SelectedItem as TexturePackage;
            TextureComboBox.Update(tp == null ? null : tp.ToString());
        }

        private void TextureSelectionChanged(object sender, EventArgs e)
        {
            Mediator.Publish(EditorMediator.TextureSelected, TextureComboBox.GetSelectedTexture());
        }

        public void Notify(string message, object data)
        {
            Mediator.ExecuteDefault(this, message, data);
        }

        private void DocumentActivated(Document doc)
        {
            // Textures
            int index = GroupComboBox.SelectedIndex;
            GroupComboBox.Items.Clear();
            GroupComboBox.Items.Add(Local.LocalString("category.texture.all"));
            HashSet<string> takenNames = new HashSet<string>();
            foreach (TexturePackage package in doc.TextureCollection.Packages)
            {
                if (takenNames.Contains(package.PackageRelativePath)) { continue; }
                takenNames.Add(package.PackageRelativePath);
                GroupComboBox.Items.Add(package);
            }
            if (index < 0 || index >= GroupComboBox.Items.Count) index = 0;
            GroupComboBox.SelectedIndex = index;
            TextureSelected(doc.TextureCollection.GetRecentTextures().FirstOrDefault());
        }

        private void DocumentAllClosed()
        {
            GroupComboBox.Items.Clear();
            TextureComboBox.Items.Clear();
            TextureSelected(null);
        }

        private void TextureSelected(TextureItem selection)
        {
            System.Drawing.Image dis = SelectionPictureBox.Image;
            SelectionPictureBox.Image = null;
            //if (dis != null) dis.Dispose();
            SizeLabel.Text = "";
            if (selection == null || DocumentManager.CurrentDocument == null) return;
            TextureComboBox.SetSelectedTexture(selection);
            using (ITextureStreamSource tp = DocumentManager.CurrentDocument.TextureCollection.GetStreamSource(128, 128))
            {
                BitmapRef bmp = tp.GetImage(selection);
                if (bmp != null)
                {
                    if (bmp.Bitmap.Width > SelectionPictureBox.Width || bmp.Bitmap.Height > SelectionPictureBox.Height)
                    {
                        SelectionPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        SelectionPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                }
                SelectionPictureBox.Image = bmp.Bitmap;
            }
            SizeLabel.Text = string.Format("{0} x {1}", selection.Width, selection.Height);
        }
    }
}
