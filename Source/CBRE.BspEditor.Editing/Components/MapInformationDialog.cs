using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;
using CBRE.Shell;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Editing.Components
{
    [Export(typeof(IDialog))]
    [AutoTranslate]
    public partial class MapInformationDialog : Form, IDialog
    {
        [Import("Shell", typeof(Form))] private Lazy<Form> _parent;
        [Import] private IContext _context;
        
        private List<Subscription> _subscriptions;

        #region Translations

        public string Title { set => this.InvokeLater(() => Text = value); }
        public string Solids { set => this.InvokeLater(() => SolidsLabel.Text = value); }
        public string Faces { set => this.InvokeLater(() => FacesLabel.Text = value); }
        public string PointEntities { set => this.InvokeLater(() => PointEntitiesLabel.Text = value); }
        public string SolidEntities { set => this.InvokeLater(() => SolidEntitiesLabel.Text = value); }
        public string UniqueTextures { set => this.InvokeLater(() => UniqueTexturesLabel.Text = value); }
        public string TextureMemory { set => this.InvokeLater(() => TextureMemoryLabel.Text = value); }
        public string TexturePackagesUsed { set => this.InvokeLater(() => TexturePackagesUsedLabel.Text = value); }
        public string CloseButton { set => this.InvokeLater(() => CloseDialogButton.Text = value); }
        public string CalculatingTextureMemoryUsage { get; set; }

        #endregion

        public MapInformationDialog()
        {
            InitializeComponent();
            CreateHandle();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Oy.Publish("Context:Remove", new ContextInfo("BspEditor:MapInformation"));
        }

        protected override void OnMouseEnter(EventArgs e)
		{
            Focus();
            base.OnMouseEnter(e);
        }

        public bool IsInContext(IContext context)
        {
            return context.HasAny("BspEditor:MapInformation");
        }

        public void SetVisible(IContext context, bool visible)
        {
            this.InvokeLater(() =>
            {
                if (visible)
                {
                    if (!Visible) Show(_parent.Value);
                    Subscribe();
                    CalculateStats();
                }
                else
                {
                    Hide();
                    Unsubscribe();
                }
            });
        }

        private void Subscribe()
        {
            if (_subscriptions != null) return;
            _subscriptions = new List<Subscription>
            {
                Oy.Subscribe<Change>("MapDocument:Changed", _ => CalculateStats()),
                Oy.Subscribe<MapDocument>("Document:Activated", _ => CalculateStats())
            };
        }

        private void Unsubscribe()
        {
            if (_subscriptions == null) return;
            _subscriptions.ForEach(x => x.Dispose());
            _subscriptions = null;
        }

        private Task CalculateStats()
        {
            MapDocument doc = _context.Get<MapDocument>("ActiveDocument");

            if (doc == null)
            {
                return this.InvokeLaterAsync(() =>
                {
                    NumSolids.Text = "\u2014";
                    NumFaces.Text = "\u2014";
                    NumPointEntities.Text = "\u2014";
                    NumSolidEntities.Text = "\u2014";
                    NumUniqueTextures.Text = "\u2014";
                    TextureMemoryValue.Text = "\u2014";
                    TexturePackages.Items.Clear();
                });
            }

            List<IMapObject> all = doc.Map.Root.FindAll();
            List<Solid> solids = all.OfType<Solid>().ToList();
            List<Primitives.MapObjectData.Face> faces = solids.SelectMany(x => x.Faces).ToList();
            List<Entity> entities = all.OfType<Entity>().ToList();
            int numSolids = solids.Count;
            int numFaces = faces.Count;
            int numPointEnts = entities.Count(x => !x.Hierarchy.HasChildren);
            int numSolidEnts = entities.Count(x => x.Hierarchy.HasChildren);
            HashSet<string> uniqueTextures = new HashSet<string>(faces.Select(x => x.Texture.Name));
            int numUniqueTextures = uniqueTextures.Count;

            return this.InvokeLaterAsync(() =>
            {
                NumSolids.Text = numSolids.ToString(CultureInfo.CurrentCulture);
                NumFaces.Text = numFaces.ToString(CultureInfo.CurrentCulture);
                NumPointEntities.Text = numPointEnts.ToString(CultureInfo.CurrentCulture);
                NumSolidEntities.Text = numSolidEnts.ToString(CultureInfo.CurrentCulture);
                NumUniqueTextures.Text = numUniqueTextures.ToString(CultureInfo.CurrentCulture);
                TextureMemoryValue.Text = CalculatingTextureMemoryUsage;
            }).ContinueWith(async _ =>
            {
                Environment.TextureCollection tc = await doc.Environment.GetTextureCollection();
                IEnumerable<CBRE.Providers.Texture.TexturePackage> usedPackages = tc.Packages.Where(x => x.Textures.Overlaps(uniqueTextures));

                this.InvokeLater(() =>
                {
                    TexturePackages.Items.Clear();
                    foreach (CBRE.Providers.Texture.TexturePackage tp in usedPackages)
                    {
                        TexturePackages.Items.Add(tp);
                    }
                });

                long texUsage = 0;
                foreach (string ut in uniqueTextures)
                {
                    CBRE.Providers.Texture.TextureItem tex = await tc.GetTextureItem(ut);

                    if (tex == null) continue;

                }
                decimal textureMemoryMb = texUsage / (1024m * 1024m);
                this.InvokeLater(() =>
                {
                    TextureMemoryValue.Text = $@"{textureMemoryMb:0.00} MB";
                });
            });
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void ComputeTextureUsage(object sender, EventArgs e)
        {
            TextureMemoryValue.Text = CalculatingTextureMemoryUsage;
            // ...
        }
    }
}
