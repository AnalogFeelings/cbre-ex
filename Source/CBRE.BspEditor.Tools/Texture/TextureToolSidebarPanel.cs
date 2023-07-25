using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;
using CBRE.DataStructures.Geometric;
using CBRE.Shell;

namespace CBRE.BspEditor.Tools.Texture
{
    [AutoTranslate]
    [Export(typeof(ISidebarComponent))]
    [OrderHint("F")]
    public partial class TextureToolSidebarPanel : UserControl, ISidebarComponent
    {
        [Import] private TextureTool _tool;
        
        public string Title { get; set; } = "Texture Power Tools";
        public object Control => this;

        #region Translations

        public string RandomiseShiftValues
        {
            set => this.InvokeLater(() => RandomiseShiftValuesGroup.Text = value);
        }

        public string Min
        {
            set => this.InvokeLater(() => MinLabel.Text = value);
        }

        public string Max
        {
            set => this.InvokeLater(() => MaxLabel.Text = value);
        }

        public string RandomiseX
        {
            set => this.InvokeLater(() => RandomShiftXButton.Text = value);
        }

        public string RandomiseY
        {
            set => this.InvokeLater(() => RandomShiftYButton.Text = value);
        }

        public string FitToMultipleTiles
        {
            set => this.InvokeLater(() => FitGroup.Text = value);
        }

        public string TimesToTile
        {
            set => this.InvokeLater(() => TimesToTileLabel.Text = value);
        }

        public string Fit
        {
            set => this.InvokeLater(() => TileFitButton.Text = value);
        }

        #endregion

        public TextureToolSidebarPanel()
        {
            InitializeComponent();
            CreateHandle();
        }

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveTool", out TextureTool _);
        }

        private void RandomShiftXButtonClicked(object sender, EventArgs e)
        {
            Documents.MapDocument document = _tool.GetDocument();
            FaceSelection fs = document?.Map.Data.GetOne<FaceSelection>();
            if (fs == null || fs.IsEmpty) return;

            int min = (int) RandomShiftMin.Value;
            int max = (int) RandomShiftMax.Value;

            Random rand = new Random();

            Transaction edit = new Transaction();
            foreach (System.Collections.Generic.KeyValuePair<Primitives.MapObjects.IMapObject, Face> it in fs.GetSelectedFaces())
            {
                Face clone = (Face) it.Value.Clone();
                clone.Texture.XShift = rand.Next(min, max + 1); // Upper bound is exclusive

                edit.Add(new RemoveMapObjectData(it.Key.ID, it.Value));
                edit.Add(new AddMapObjectData(it.Key.ID, clone));
            }

            MapDocumentOperation.Perform(document, edit);
        }

        private void RandomShiftYButtonClicked(object sender, EventArgs e)
        {
            Documents.MapDocument document = _tool.GetDocument();
            FaceSelection fs = document?.Map.Data.GetOne<FaceSelection>();
            if (fs == null || fs.IsEmpty) return;

            int min = (int) RandomShiftMin.Value;
            int max = (int) RandomShiftMax.Value;

            Random rand = new Random();

            Transaction edit = new Transaction();
            foreach (System.Collections.Generic.KeyValuePair<Primitives.MapObjects.IMapObject, Face> it in fs.GetSelectedFaces())
            {
                Face clone = (Face) it.Value.Clone();
                clone.Texture.YShift = rand.Next(min, max + 1); // Upper bound is exclusive

                edit.Add(new RemoveMapObjectData(it.Key.ID, it.Value));
                edit.Add(new AddMapObjectData(it.Key.ID, clone));
            }

            MapDocumentOperation.Perform(document, edit);
        }

        private void TileFitButtonClicked(object sender, EventArgs e)
        {
            ApplyFit();
        }

        private async Task ApplyFit()
        {
            Documents.MapDocument document = _tool.GetDocument();
            FaceSelection fs = document?.Map.Data.GetOne<FaceSelection>();
            if (fs == null || fs.IsEmpty) return;

            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();
            if (tc == null) return;

            int tileX = (int) TileFitX.Value;
            int tileY = (int) TileFitY.Value;

            Transaction edit = new Transaction();
            foreach (System.Collections.Generic.KeyValuePair<Primitives.MapObjects.IMapObject, Face> it in fs.GetSelectedFaces())
            {
                Face clone = (Face) it.Value.Clone();

                CBRE.Providers.Texture.TextureItem tex = await tc.GetTextureItem(clone.Texture.Name);
                if (tex == null) continue;

                clone.Texture.FitToPointCloud(tex.Width, tex.Height, new Cloud(clone.Vertices), tileX, tileY);
                
                edit.Add(new RemoveMapObjectData(it.Key.ID, it.Value));
                edit.Add(new AddMapObjectData(it.Key.ID, clone));
            }

            if (!edit.IsEmpty) await MapDocumentOperation.Perform(document, edit);
        }
    }
}
