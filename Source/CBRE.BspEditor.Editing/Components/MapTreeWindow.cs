using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;
using CBRE.Shell;

namespace CBRE.BspEditor.Editing.Components
{
    [Export(typeof(IDialog))]
    [AutoTranslate]
    public partial class MapTreeWindow : Form, IDialog, IManualTranslate
    {
        [Import("Shell", typeof(Form))] private Lazy<Form> _parent;
        [Import] private IContext _context;

        private List<Subscription> _subscriptions;

        public MapTreeWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Oy.Publish("Context:Remove", new ContextInfo("BspEditor:MapTree"));
        }

        protected override void OnMouseEnter(EventArgs e)
		{
            Focus();
            base.OnMouseEnter(e);
        }

        public bool IsInContext(IContext context)
        {
            return context.HasAny("BspEditor:MapTree");
        }

        public void SetVisible(IContext context, bool visible)
        {
            this.InvokeLater(() =>
            {
                if (visible)
                {
                    if (!Visible) Show(_parent.Value);
                    Subscribe();
                    RefreshNodes();
                }
                else
                {
                    Hide();
                    Unsubscribe();
                }
            });
        }

        public void Translate(ITranslationStringProvider strings)
        {
            CreateHandle();
            string prefix = GetType().FullName;
            this.InvokeLater(() =>
            {
                Text = strings.GetString(prefix, "Title");
            });
        }

        private void Subscribe()
        {
            if (_subscriptions != null) return;
            _subscriptions = new List<Subscription>
            {
                Oy.Subscribe<Change>("MapDocument:Changed", DocumentChanged),
                Oy.Subscribe<MapDocument>("Document:Activated", DocumentActivated),
                Oy.Subscribe<MapDocument>("MapDocument:SelectionChanged", SelectionChanged)
            };
        }

        private void Unsubscribe()
        {
            if (_subscriptions == null) return;
            _subscriptions.ForEach(x => x.Dispose());
            _subscriptions = null;
        }

        private async Task SelectionChanged(MapDocument doc)
        {
            this.InvokeLater(() =>
            {
                if (doc == null || doc.Selection.IsEmpty) return;
                IMapObject first = doc.Selection.GetSelectedParents().First();
                TreeNode node = FindNodeWithTag(MapTree.Nodes.OfType<TreeNode>(), first);
                if (node != null) MapTree.SelectedNode = node;
            });
        }

        public async Task DocumentActivated(MapDocument document)
        {
            this.InvokeLater(() =>
            {
                RefreshNodes(document);
            });
        }

        private async Task DocumentChanged(Change change)
        {
            this.InvokeLater(() =>
            {
                RefreshNodes(change.Document);
            });
        }

        private TreeNode FindNodeWithTag(IEnumerable<TreeNode> nodes, object tag)
        {
            foreach (TreeNode tn in nodes)
            {
                if (tn.Tag == tag) return tn;
                TreeNode recurse = FindNodeWithTag(tn.Nodes.OfType<TreeNode>(), tag);
                if (recurse != null) return recurse;
            }
            return null;
        }

        private void RefreshNodes()
        {
            MapDocument doc = _context.Get<MapDocument>("ActiveDocument");
            RefreshNodes(doc);
        }

        private void RefreshNodes(MapDocument doc)
        {
            MapTree.BeginUpdate();
            MapTree.Nodes.Clear();
            if (doc != null)
            {
                LoadMapNode(null, doc.Map.Root);
            }
            MapTree.EndUpdate();
        }

        private void LoadMapNode(TreeNode parent, IMapObject obj)
        {
            string text = GetNodeText(obj);
            TreeNode node = new TreeNode(obj.GetType().Name + text) { Tag = obj };
            if (obj is Root w)
            {
                node.Nodes.AddRange(GetEntityNodes(w.Data.GetOne<EntityData>()).ToArray());
            }
            else if (obj is Entity e)
            {
                node.Nodes.AddRange(GetEntityNodes(e.EntityData).ToArray());
            }
            else if (obj is Solid s)
            {
                node.Nodes.AddRange(GetFaceNodes(s.Faces).ToArray());
            }
            foreach (IMapObject mo in obj.Hierarchy)
            {
                LoadMapNode(node, mo);
            }
            if (parent == null) MapTree.Nodes.Add(node);
            else parent.Nodes.Add(node);
        }

        private string GetNodeText(IMapObject mo)
        {
            if (mo is Solid solid)
            {
                return " (" + solid.Faces.Count() + " faces)";
            }
            if (mo is Group)
            {
                return " (" + mo.Hierarchy.HasChildren + " children)";
            }
            EntityData ed = mo.Data.GetOne<EntityData>();
            if (ed != null)
            {
                string targetName = ed.Get("targetname", "");
                return ": " + ed.Name + (string.IsNullOrWhiteSpace(targetName) ? "" : " (" + targetName + ")");
            }
            return "";
        }

        private IEnumerable<TreeNode> GetEntityNodes(EntityData data)
        {
            if (data == null) yield break;

            yield return new TreeNode("Flags: " + data.Flags);
        }

        private IEnumerable<TreeNode> GetFaceNodes(IEnumerable<Face> faces)
        {
            int c = 0;
            foreach (Face face in faces)
            {
                TreeNode fnode = new TreeNode("Face " + c);
                c++;
                TreeNode pnode = fnode.Nodes.Add($"Plane: {face.Plane.Normal} * {face.Plane.DistanceFromOrigin}");
                pnode.Nodes.Add($"Normal: {face.Plane.Normal}");
                pnode.Nodes.Add($"Distance: {face.Plane.DistanceFromOrigin}");
                pnode.Nodes.Add($"A: {face.Plane.A}");
                pnode.Nodes.Add($"B: {face.Plane.B}");
                pnode.Nodes.Add($"C: {face.Plane.C}");
                pnode.Nodes.Add($"D: {face.Plane.D}");
                TreeNode tnode = fnode.Nodes.Add("Texture: " + face.Texture.Name);
                tnode.Nodes.Add($"U Axis: {face.Texture.UAxis}");
                tnode.Nodes.Add($"V Axis: {face.Texture.VAxis}");
                tnode.Nodes.Add($"Scale: X = {face.Texture.XScale}, Y = {face.Texture.YScale}");
                tnode.Nodes.Add($"Offset: X = {face.Texture.XShift}, Y = {face.Texture.YShift}");
                tnode.Nodes.Add("Rotation: " + face.Texture.Rotation);
                TreeNode vnode = fnode.Nodes.Add($"Vertices: {face.Vertices.Count}");
                int d = 0;
                foreach (System.Numerics.Vector3 vertex in face.Vertices)
                {
                    TreeNode cnode = vnode.Nodes.Add("Vertex " + d + ": " + vertex);
                    d++;
                }
                yield return fnode;
            }
        }

        private async void TreeSelectionChanged(object sender, TreeViewEventArgs e)
        {
            await RefreshSelectionProperties();
            // if (MapTree.SelectedNode != null && MapTree.SelectedNode.Tag is MapObject && !(MapTree.SelectedNode.Tag is World) && MapDocument != null && !MapDocument.Selection.InFaceSelection)
            // {
            //     MapDocument.PerformAction("Select object", new ChangeSelection(((MapObject)MapTree.SelectedNode.Tag).FindAll(), MapDocument.Selection.GetSelectedObjects()));
            // }
        }

        private async Task RefreshSelectionProperties()
        {
            Properties.Items.Clear();
            if (MapTree.SelectedNode != null && MapTree.SelectedNode.Tag != null)
            {
                IEnumerable<Tuple<string, string>> list = await GetTagProperties(MapTree.SelectedNode.Tag);
                foreach (Tuple<string, string> kv in list)
                {
                    Properties.Items.Add(new ListViewItem(new[] {kv.Item1, kv.Item2}));
                }
                Properties.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private async Task<IEnumerable<Tuple<string, string>>> GetTagProperties(object tag)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            if (!(tag is long id)) return list;

            MapDocument doc = _context.Get<MapDocument>("ActiveDocument");
            if (doc == null) return list;

            IMapObject mo = doc.Map.Root.FindByID(id);
            if (mo == null) return list;

            EntityData ed = mo.Data.GetOne<EntityData>();
            if (ed == null) return list;

            DataStructures.GameData.GameData gameData = await doc.Environment.GetGameData();

            DataStructures.GameData.GameDataObject gd = gameData.GetClass(ed.Name);
            foreach (KeyValuePair<string, string> prop in ed.Properties)
            {
                DataStructures.GameData.Property gdp = gd?.Properties.FirstOrDefault(x => string.Equals(x.Name, prop.Key, StringComparison.InvariantCultureIgnoreCase));
                string key = gdp != null && !string.IsNullOrWhiteSpace(gdp.ShortDescription) ? gdp.ShortDescription : prop.Key;
                list.Add(Tuple.Create(key, prop.Value));
            }
            return list;
        }
    }
}
