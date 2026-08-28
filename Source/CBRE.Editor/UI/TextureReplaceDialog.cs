using CBRE.Common;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Editor.Actions.MapObjects.Selection;
using CBRE.Editor.Documents;
using CBRE.Localization;
using CBRE.Providers.Texture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI
{
    public partial class TextureReplaceDialog : Form
    {
        private Document _document;

        public TextureReplaceDialog(Document document)
        {
            _document = document;
            InitializeComponent();
            BindTextureControls(Find, FindImage, FindBrowse, FindInfo);
            BindTextureControls(Replace, ReplaceImage, ReplaceBrowse, ReplaceInfo);

            ReplaceSelection.Checked = true;
            ActionExact.Checked = true;

            if (document.Selection.IsEmpty())
            {
                ReplaceSelection.Enabled = false;
                ReplaceVisible.Checked = true;
            }

            if (_document.TextureCollection.SelectedTexture != null)
            {
                TextureItem tex = _document.TextureCollection.SelectedTexture;
                Find.Text = tex.Name;
            }
        }

        private IEnumerable<MapObject> GetObjects()
        {
            if (ReplaceSelection.Checked) return _document.Selection.GetSelectedObjects();
            if (ReplaceVisible.Checked) return _document.Map.WorldSpawn.Find(x => !x.IsVisgroupHidden);
            return _document.Map.WorldSpawn.FindAll();
        }

        private bool MatchTextureName(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return false;

            string match = Find.Text;
            if (!ActionExact.Checked)
            {
                return name.ToLowerInvariant().Contains(match.ToLowerInvariant());
            }
            return String.Equals(name, match, StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<Tuple<string, TextureItem, ITexture>> GetReplacements(IEnumerable<string> names)
        {
            List<Tuple<string, TextureItem, ITexture>> list = new List<Tuple<string, TextureItem, ITexture>>();
            bool substitute = ActionSubstitute.Checked;
            string find = Find.Text.ToLowerInvariant();
            string replace = Replace.Text.ToLowerInvariant();

            foreach (string name in names.Select(x => x.ToLowerInvariant()).Distinct())
            {
                string n = substitute ? name.Replace(find, replace) : replace;

                TextureItem item = _document.TextureCollection.GetItem(n);
                if (item == null) continue;

                list.Add(Tuple.Create(name, item, item.GetTexture()));
            }
            return list;
        }

        public IAction GetAction()
        {
            List<Face> faces = GetObjects().OfType<Solid>().SelectMany(x => x.Faces).Where(x => MatchTextureName(x.Texture.Name)).ToList();
            if (ActionSelect.Checked)
            {
                return new ChangeSelection(faces.Select(x => x.Parent).Distinct(), _document.Selection.GetSelectedObjects());
            }
            bool rescale = RescaleTextures.Checked;
            IEnumerable<Tuple<string, TextureItem, ITexture>> replacements = GetReplacements(faces.Select(x => x.Texture.Name));
            Action<Document, Face> action = (doc, face) =>
            {
                Tuple<string, TextureItem, ITexture> repl = replacements.FirstOrDefault(x => x.Item1 == face.Texture.Name.ToLowerInvariant());
                if (repl == null) return;
                if (rescale)
                {
                    TextureItem item = _document.TextureCollection.GetItem(face.Texture.Name);
                    if (item != null)
                    {
                        face.Texture.XScale *= item.Width / (decimal)repl.Item2.Width;
                        face.Texture.YScale *= item.Height / (decimal)repl.Item2.Height;
                    }
                }
                face.Texture.Name = repl.Item2.Name;
                face.Texture.Texture = repl.Item3;
                face.CalculateTextureCoordinates(true);
            };
            return new EditFace(faces, action, true);
        }

        private void BindTextureControls(TextBox box, PictureBox image, Button browse, Label info)
        {
            box.TextChanged += (sender, e) => UpdateTexture(box.Text, image, info);
            browse.Click += (sender, e) => BrowseTexture(box);
            UpdateTexture(box.Text, image, info);
        }

        private void BrowseTexture(TextBox box)
        {
            using (TextureBrowser tb = new TextureBrowser())
            {
                tb.SetTextureList(_document.TextureCollection.GetAllBrowsableItems());
                tb.ShowDialog();
                if (tb.SelectedTexture != null)
                {
                    box.Text = tb.SelectedTexture.Name;
                }
            }
        }

        private void UpdateTexture(string text, PictureBox image, Label info)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                image.Image = null;
                info.Text = Local.LocalString("texture.no_image");
                return;
            }

            TextureItem item = _document.TextureCollection.GetItem(text)
                       ?? new TextureItem(null, text, TextureFlags.Missing, 64, 64);

            using (ITextureStreamSource tp = _document.TextureCollection.GetStreamSource(128, 128))
            {
                BitmapRef bmp = tp.GetImage(item);
                image.SizeMode = bmp.Bitmap.Width > image.Width || bmp.Bitmap.Height > image.Height
                                     ? PictureBoxSizeMode.Zoom
                                     : PictureBoxSizeMode.CenterImage;
                image.Image = bmp.Bitmap;
            }

            string format = item.Flags.HasFlag(TextureFlags.Missing) ? Local.LocalString("texture.invalid_texture") : "{0} x {1}";
            info.Text = string.Format(format, item.Width, item.Height);
        }
    }
}
