using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Selection;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.ResourceManagement;
using CBRE.BspEditor.Rendering.Viewport;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common;
using CBRE.Common.Shell.Components;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;
using CBRE.Rendering.Cameras;
using CBRE.Rendering.Pipelines;
using CBRE.Rendering.Primitives;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Tools.Entity
{
    [Export(typeof(ITool))]
    [Export(typeof(ISettingsContainer))]
    [OrderHint("F")]
    [AutoTranslate]
    [DefaultHotkey("Shift+E")]
    public class EntityTool : BaseTool, ISettingsContainer
    {
        private enum EntityState
        {
            None,
            Drawn,
            Moving
        }

        private Vector3 _location;
        private EntityState _state;
        private string _activeEntity;

        public string CreateObject { get; set; } = "Create {0}";

        // Settings

        [Setting("SelectCreatedEntity")] private bool _selectCreatedEntity = true;
        [Setting("SwitchToSelectAfterEntityCreation")] private bool _switchToSelectAfterCreation = false;
        [Setting("ResetEntityTypeOnCreation")] private bool _resetEntityTypeOnCreation = false;

        string ISettingsContainer.Name => "CBRE.BspEditor.Tools.EntityTool";

        IEnumerable<SettingKey> ISettingsContainer.GetKeys()
        {
            yield return new SettingKey("Tools/Entity", "SelectCreatedEntity", typeof(bool));
            yield return new SettingKey("Tools/Entity", "SwitchToSelectAfterEntityCreation", typeof(bool));
            yield return new SettingKey("Tools/Entity", "ResetEntityTypeOnCreation", typeof(bool));
        }

        void ISettingsContainer.LoadValues(ISettingsStore store)
        {
            store.LoadInstance(this);
        }

        void ISettingsContainer.StoreValues(ISettingsStore store)
        {
            store.StoreInstance(this);
        }

        public EntityTool()
        {
            Usage = ToolUsage.Both;
            _location = new Vector3(0, 0, 0);
            _state = EntityState.None;
        }

        protected override void ContextChanged(IContext context)
        {
            _activeEntity = context.Get<string>("EntityTool:ActiveEntity");

            base.ContextChanged(context);
        }

        protected override void DocumentChanged()
        {
            Task.Factory.StartNew(BuildMenu);
            base.DocumentChanged();
        }

        private ToolStripItem[] _menu;

        private async void BuildMenu()
        {
            _menu = null;
            MapDocument document = GetDocument();
            if (document == null) return;

            GameData gd = await document.Environment.GetGameData();
            if (gd == null) return;

            List<ToolStripItem> items = new List<ToolStripItem>();
            List<GameDataObject> classes = gd.Classes.Where(x => x.ClassType != ClassType.Base && x.ClassType != ClassType.Solid).ToList();
            IEnumerable<IGrouping<string, GameDataObject>> groups = classes.GroupBy(x => x.Name.Split('_')[0]);
            foreach (IGrouping<string, GameDataObject> g in groups)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(g.Key);
                List<GameDataObject> l = g.ToList();
                if (l.Count == 1)
                {
                    GameDataObject cls = l[0];
                    mi.Text = cls.Name;
                    mi.Tag = cls;
                    mi.Click += (s, e) => CreateEntity(_location, cls.Name);
                }
                else
                {
                    ToolStripItem[] subs = l.Select(x =>
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(x.Name) { Tag = x };
                        item.Click += (s, e) => CreateEntity(_location, x.Name);
                        return item;
                    }).OfType<ToolStripItem>().ToArray();
                    mi.DropDownItems.AddRange(subs);
                }
                items.Add(mi);
            }
            _menu = items.ToArray();
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Entity;
        }

        public override string GetName()
        {
            return "Entity Tool";
        }

        protected override IEnumerable<Subscription> Subscribe()
        {
            yield return Oy.Subscribe<RightClickMenuBuilder>("MapViewport:RightClick", b =>
            {
                b.Clear();
                b.AddCallback(string.Format(CreateObject, _activeEntity), () => CreateEntity(_location));

                if (_menu == null || _menu.Length <= 0) return;

                b.AddSeparator();
                b.Add(_menu);
            });
        }

        // 3D interaction

        protected override void MouseDown(MapDocument document, MapViewport viewport, PerspectiveCamera camera, ViewportEvent e)
        {
            if (e.Button != MouseButtons.Left) return;

            // Get the ray that is cast from the clicked point along the viewport frustrum
            (Vector3 rs, Vector3 re) = camera.CastRayFromScreen(new Vector3(e.X, e.Y, 0));
            Line ray = new Line(rs, re);

            // Grab all the elements that intersect with the ray
            MapObjectExtensions.MapObjectIntersection hit = document.Map.Root.GetIntersectionsForVisibleObjects(ray).FirstOrDefault();

            if (hit == null) return; // Nothing was clicked

            CreateEntity(document, hit.Intersection);
        }

        // 2D interaction

        protected override void MouseEnter(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            viewport.Control.Cursor = Cursors.Cross;
        }

        protected override void MouseLeave(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            viewport.Control.Cursor = Cursors.Cross;
        }

        protected override void MouseDown(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right) return;

            _state = EntityState.Moving;
            Vector3 loc = SnapIfNeeded(camera.ScreenToWorld(e.X, e.Y));
            _location = camera.GetUnusedCoordinate(_location) + loc;
        }

        protected override void MouseUp(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            if (e.Button != MouseButtons.Left) return;
            _state = EntityState.Drawn;
            Vector3 loc = SnapIfNeeded(camera.ScreenToWorld(e.X, e.Y));
            _location = camera.GetUnusedCoordinate(_location) + loc;
        }

        protected override void MouseMove(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            if (!Control.MouseButtons.HasFlag(MouseButtons.Left)) return;
            if (_state != EntityState.Moving) return;
            Vector3 loc = SnapIfNeeded(camera.ScreenToWorld(e.X, e.Y));
            _location = camera.GetUnusedCoordinate(_location) + loc;
        }

        protected override void KeyDown(MapDocument document, MapViewport viewport, OrthographicCamera camera, ViewportEvent e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CreateEntity(document, _location);
                    _state = EntityState.None;
                    break;
                case Keys.Escape:
                    _state = EntityState.None;
                    break;
            }
        }

        private Task CreateEntity(Vector3 origin, string gd = null)
        {
            MapDocument document = GetDocument();
            return document == null ? Task.CompletedTask : CreateEntity(document, origin, gd);
        }

        private async Task CreateEntity(MapDocument document, Vector3 origin, string gd = null)
        {
            if (gd == null) gd = _activeEntity;
            if (gd == null) return;

            Color colour = Colour.GetDefaultEntityColour();
            GameData data = await document.Environment.GetGameData();
            if (data != null)
            {
                GameDataObject cls = data.Classes.FirstOrDefault(x => string.Equals(x.Name, gd, StringComparison.InvariantCultureIgnoreCase));
                if (cls != null)
                {
                    Behaviour[] col = cls.Behaviours.Where(x => x.Name == "color").ToArray();
                    if (col.Any()) colour = col[0].GetColour(0);
                }
            }

            Primitives.MapObjects.Entity entity = new Primitives.MapObjects.Entity(document.Map.NumberGenerator.Next("MapObject"))
            {
                Data =
                {
                    new EntityData { Name = gd },
                    new ObjectColor(colour),
                    new Origin(origin),
                }
            };

            Transaction transaction = new Transaction();

            transaction.Add(new Attach(document.Map.Root.ID, entity));

            if (_selectCreatedEntity)
            {
                transaction.Add(new Deselect(document.Selection));
                transaction.Add(new Select(entity.FindAll()));
            }

            await MapDocumentOperation.Perform(document, transaction);

            if (_switchToSelectAfterCreation)
            {
                Oy.Publish("ActivateTool", "SelectTool");
            }

            if (_resetEntityTypeOnCreation)
            {
                Oy.Publish("EntityTool:ResetEntityType", this);
            }
        }

        // Rendering

        protected override void Render(MapDocument document, BufferBuilder builder, ResourceCollector resourceCollector)
        {
            if (_state != EntityState.None)
            {
                Vector3 vec = _location;
                float high = 1024f * 1024f;
                float low = -high;

                // Draw a box around the point
                Box c = new Box(vec - Vector3.One * 10, vec + Vector3.One * 10);

                const uint numVertices = 4 * 6 + 6;
                const uint numWireframeIndices = numVertices * 2;

                VertexStandard[] points = new VertexStandard[numVertices];
                uint[] indices = new uint[numWireframeIndices];

                Vector4 colour = new Vector4(0, 1, 0, 1);

                uint vi = 0u;
                uint wi = 0u;
                foreach (Vector3[] face in c.GetBoxFaces())
                {
                    uint offs = vi;

                    foreach (Vector3 v in face)
                    {
                        points[vi++] = new VertexStandard { 
                            Position = v,
                            Colour = colour,
                            Tint = Vector4.One
                        };
                    }

                    // Lines - [0 1] ... [n-1 n] [n 0]
                    for (uint i = 0; i < 4; i++)
                    {
                        indices[wi++] = offs + i;
                        indices[wi++] = offs + (i == 4 - 1 ? 0 : i + 1);
                    }
                }

                // Draw 3 lines pinpointing the point
                uint lineOffset = vi;

                points[vi++] = new VertexStandard { Position = new Vector3(low , vec.Y, vec.Z), Colour = colour, Tint = Vector4.One };
                points[vi++] = new VertexStandard { Position = new Vector3(high, vec.Y, vec.Z), Colour = colour, Tint = Vector4.One };
                points[vi++] = new VertexStandard { Position = new Vector3(vec.X, low , vec.Z), Colour = colour, Tint = Vector4.One };
                points[vi++] = new VertexStandard { Position = new Vector3(vec.X, high, vec.Z), Colour = colour, Tint = Vector4.One };
                points[vi++] = new VertexStandard { Position = new Vector3(vec.X, vec.Y, low ), Colour = colour, Tint = Vector4.One };
                points[vi++] = new VertexStandard { Position = new Vector3(vec.X, vec.Y, high), Colour = colour, Tint = Vector4.One };

                indices[wi++] = lineOffset++;
                indices[wi++] = lineOffset++;
                indices[wi++] = lineOffset++;
                indices[wi++] = lineOffset++;
                indices[wi++] = lineOffset++;
                indices[wi++] = lineOffset++;

                BufferGroup[] groups = new[]
                {
                    new BufferGroup(PipelineType.Wireframe, CameraType.Both, 0, numWireframeIndices)
                };

                builder.Append(points, indices, groups);
            }

            base.Render(document, builder, resourceCollector);
        }
    }
}
