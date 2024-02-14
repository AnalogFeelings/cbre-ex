using CBRE.Common;
using CBRE.Common.Mediator;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Editing;
using CBRE.Editor.Environment;
using CBRE.Editor.Extensions;
using CBRE.Editor.History;
using CBRE.Editor.Rendering;
using CBRE.Editor.Rendering.Helpers;
using CBRE.Editor.Settings;
using CBRE.Editor.Tools;
using CBRE.Editor.UI;
using CBRE.Graphics.Helpers;
using CBRE.Providers.Map;
using CBRE.Providers.Texture;
using CBRE.Settings;
using CBRE.Settings.Models;
using CBRE.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CBRE.Editor.Logging;
using Path = System.IO.Path;
using CBRE.Localization;

namespace CBRE.Editor.Documents
{
    public class Document
    {
        public string MapFile { get; set; }
        public string MapFileName { get; set; }
        public Map Map { get; set; }

        public Game Game { get; set; }
        public GameEnvironment Environment { get; private set; }
        public GameData GameData { get; set; }

        public RenderManager Renderer { get; private set; }

        public SelectionManager Selection { get; private set; }
        public HistoryManager History { get; private set; }
        public HelperManager HelperManager { get; set; }
        public TextureCollection TextureCollection { get; set; }

        private readonly DocumentSubscriptions _subscriptions;
        private readonly DocumentMemory _memory;

        private Document()
        {
            Map = new Map();
            Selection = new SelectionManager(this);
            History = new HistoryManager(this);
            HelperManager = new HelperManager(this);
            TextureCollection = new TextureCollection(new List<TexturePackage>());
        }

        public Document(string mapFile, Map map, Game game)
        {
            MapFile = mapFile;
            Map = map;
            Game = game;
            Environment = new GameEnvironment(game);
            MapFileName = mapFile == null
                              ? DocumentManager.GetUntitledDocumentName()
                              : Path.GetFileName(mapFile);
            SelectListTransform = Matrix4.Identity;

            _subscriptions = new DocumentSubscriptions(this);

            _memory = new DocumentMemory();

            Camera cam = Map.GetActiveCamera();
            if (cam != null) _memory.SetCamera(cam.EyePosition, cam.LookPosition);

            Selection = new SelectionManager(this);
            History = new HistoryManager(this);
            if (Map.GridSpacing <= 0)
            {
                Map.GridSpacing = Grid.DefaultSize;
            }

            GameData = new GameData();
            if (GameData.CustomEntityErrors.Count > 0 && Editor.Instance.ShowEntityErrorForm)
            {
                EntityErrorWindow ErrorForm = new EntityErrorWindow(GameData.CustomEntityErrors);
                ErrorForm.ShowDialog();
            }

            if (game.OverrideMapSize)
            {
                GameData.MapSizeLow = game.OverrideMapSizeLow;
                GameData.MapSizeHigh = game.OverrideMapSizeHigh;
            }

            TextureCollection = TextureProvider.CreateCollection(Environment.GetTextureCategories());
            /* .Union(GameData.MaterialExclusions) */ // todo material exclusions

            IEnumerable<string> texList = Map.GetAllTextures();
            IEnumerable<TextureItem> items = TextureCollection.GetItems(texList);
            TextureProvider.LoadTextureItems(items);

            Map.PostLoadProcess(GameData, GetTexture, SettingsManager.GetSpecialTextureOpacity);
            Map.UpdateDecals(this);
            Map.UpdateModels(this);
            Map.UpdateSprites(this);
            
            IEnumerable<Entity> allEntities = map.WorldSpawn.Find(x => x.ClassName != null).OfType<Entity>();

            if (allEntities.Any(x => x.GameData == null))
            {
                MessageBox.Show(Local.LocalString("warning.document.unknown_entity"), Local.LocalString("warning.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            HelperManager = new HelperManager(this);
            Renderer = new RenderManager(this);

            if (MapFile != null) Mediator.Publish(EditorMediator.FileOpened, MapFile);

            // Autosaving
            if (Game.Autosave)
            {
                int at = Math.Max(1, Game.AutosaveTime);
                Scheduler.Schedule(this, Autosave, TimeSpan.FromMinutes(at));
            }
        }

        public void SetMemory<T>(string name, T obj)
        {
            _memory.Set(name, obj);
        }

        public T GetMemory<T>(string name, T def = default(T))
        {
            return _memory.Get(name, def);
        }

        public void SetActive()
        {
            if (!CBRE.Settings.View.KeepSelectedTool) ToolManager.Activate(_memory.SelectedTool);
            if (!CBRE.Settings.View.KeepCameraPositions) _memory.RestoreViewports(ViewportManager.Viewports);

            ViewportManager.AddContext3D(new WidgetLinesRenderable());
            Renderer.Register(ViewportManager.Viewports);
            ViewportManager.AddContextAll(new ToolRenderable());
            ViewportManager.AddContextAll(new HelperRenderable(this));

            _subscriptions.Subscribe();

            RenderAll();
        }

        public void SetInactive()
        {
            if (!CBRE.Settings.View.KeepSelectedTool && ToolManager.ActiveTool != null) _memory.SelectedTool = ToolManager.ActiveTool.GetType();
            if (!CBRE.Settings.View.KeepCameraPositions) _memory.RememberViewports(ViewportManager.Viewports);

            ViewportManager.ClearContexts();
            HelperManager.ClearCache();

            _subscriptions.Unsubscribe();
        }

        public void Close()
        {
            Scheduler.Clear(this);
            TextureProvider.DeleteCollection(TextureCollection);
            Renderer.Dispose();
        }

        public bool SaveFile(string path = null, bool forceOverride = false, bool switchPath = true)
        {
            path = forceOverride ? path : path ?? MapFile;

            if (path != null)
            {
                IEnumerable<string> noSaveExtensions = FileTypeRegistration.GetSupportedExtensions().Where(x => !x.CanSave).Select(x => x.Extension);
                foreach (string ext in noSaveExtensions)
                {
                    if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        path = null;
                        break;
                    }
                }
            }

            if (path == null)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string filter = String.Join("|", FileTypeRegistration.GetSupportedExtensions()
                        .Where(x => x.CanSave).Select(x => x.Description + " (*" + x.Extension + ")|*" + x.Extension));
                    string[] all = FileTypeRegistration.GetSupportedExtensions().Where(x => x.CanSave).Select(x => "*" + x.Extension).ToArray();
                    sfd.Filter = Local.LocalString("filetype.all") + " (" + String.Join(", ", all) + ")|" + String.Join(";", all) + "|" + filter;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        path = sfd.FileName;
                    }
                }
            }
            if (path == null) return false;

            // Save the 3D camera position
            Graphics.Camera cam = ViewportManager.Viewports.OfType<Viewport3D>().Select(x => x.Camera).FirstOrDefault();
            if (cam != null)
            {
                if (Map.ActiveCamera == null)
                {
                    Map.ActiveCamera = !Map.Cameras.Any() ? new Camera { LookPosition = Coordinate.UnitX * Map.GridSpacing * 1.5m } : Map.Cameras.First();
                    if (!Map.Cameras.Contains(Map.ActiveCamera)) Map.Cameras.Add(Map.ActiveCamera);
                }
                decimal dist = (Map.ActiveCamera.LookPosition - Map.ActiveCamera.EyePosition).VectorMagnitude();
                Vector3 loc = cam.Location;
                Vector3 look = cam.LookAt - cam.Location;
                look.Normalize();
                look = loc + look * (float)dist;
                Map.ActiveCamera.EyePosition = new Coordinate((decimal)loc.X, (decimal)loc.Y, (decimal)loc.Z);
                Map.ActiveCamera.LookPosition = new Coordinate((decimal)look.X, (decimal)look.Y, (decimal)look.Z);
            }
            Map.WorldSpawn.EntityData.SetPropertyValue("wad", string.Join(";", GetUsedTexturePackages().Select(x => x.PackageRoot).Where(x => x.EndsWith(".wad"))));
            MapProvider.SaveMapToFile(path, Map, GameData, TextureCollection);
            if (switchPath)
            {
                MapFile = path;
                MapFileName = Path.GetFileName(MapFile);
                History.TotalActionsSinceLastSave = 0;
                Mediator.Publish(EditorMediator.DocumentSaved, this);
            }
            return true;
        }

        private string GetAutosaveFormatString()
        {
            if (MapFile == null || Path.GetFileNameWithoutExtension(MapFile) == null) return null;
            string we = Path.GetFileNameWithoutExtension(MapFile);
            string ex = Path.GetExtension(MapFile);
            return we + ".auto.{0}" + ex;
        }

        private string GetAutosaveFolder()
        {
            if (Game.UseCustomAutosaveDir && System.IO.Directory.Exists(Game.AutosaveDir)) return Game.AutosaveDir;
            if (MapFile == null || Path.GetDirectoryName(MapFile) == null) return null;
            return Path.GetDirectoryName(MapFile);
        }

        public void Autosave()
        {
            if (!Game.Autosave) return;
            string dir = GetAutosaveFolder();
            string fmt = GetAutosaveFormatString();

            // Only save on change if the game is configured to do so
            if (dir != null && fmt != null && (History.TotalActionsSinceLastAutoSave != 0 || !Game.AutosaveOnlyOnChanged))
            {
                string date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd-hh-mm-ss");
                string filename = String.Format(fmt, date);
                if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);

                // Save the file
                MapProvider.SaveMapToFile(Path.Combine(dir, filename), Map, GameData);

                // Delete extra autosaves if there is a limit
                if (Game.AutosaveLimit > 0)
                {
                    Dictionary<string, DateTime> asFiles = GetAutosaveFiles(dir);
                    foreach (KeyValuePair<string, DateTime> file in asFiles.OrderByDescending(x => x.Value).Skip(Game.AutosaveLimit))
                    {
                        if (System.IO.File.Exists(file.Key)) System.IO.File.Delete(file.Key);
                    }
                }

                // Publish event
                Mediator.Publish(EditorMediator.FileAutosaved, this);
                History.TotalActionsSinceLastAutoSave = 0;

                if (Game.AutosaveTriggerFileSave && MapFile != null)
                {
                    SaveFile();
                }
            }

            // Reschedule autosave
            int at = Math.Max(1, Game.AutosaveTime);
            Scheduler.Schedule(this, Autosave, TimeSpan.FromMinutes(at));
        }

        public Dictionary<string, DateTime> GetAutosaveFiles(string dir)
        {
            Dictionary<string, DateTime> ret = new Dictionary<string, DateTime>();
            string fs = GetAutosaveFormatString();
            if (fs == null || dir == null) return ret;
            // Search for matching files
            string[] files = System.IO.Directory.GetFiles(dir, String.Format(fs, "*"));
            foreach (string file in files)
            {
                // Match the date portion with a regex
                string re = Regex.Escape(fs.Replace("{0}", ":")).Replace(":", "{0}");
                string regex = String.Format(re, "(\\d{4})-(\\d{2})-(\\d{2})-(\\d{2})-(\\d{2})-(\\d{2})");
                Match match = Regex.Match(Path.GetFileName(file), regex, RegexOptions.IgnoreCase);
                if (!match.Success) continue;

                // Parse the date and add it if it is valid
                DateTime date;
                bool result = DateTime.TryParse(String.Format("{0}-{1}-{2}T{3}:{4}:{5}Z",
                                                             match.Groups[1].Value, match.Groups[2].Value,
                                                             match.Groups[3].Value, match.Groups[4].Value,
                                                             match.Groups[5].Value, match.Groups[6].Value),
                                                             CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                                                             out date);
                if (result)
                {
                    ret.Add(file, date);
                }
            }
            return ret;
        }

        public Coordinate Snap(Coordinate c, decimal spacing = 0)
        {
            if (!Map.SnapToGrid) return c;

            bool snap = (Select.SnapStyle == SnapStyle.SnapOnAlt && KeyboardState.Alt) ||
                       (Select.SnapStyle == SnapStyle.SnapOffAlt && !KeyboardState.Alt);

            return snap ? c.Snap(spacing == 0 ? Map.GridSpacing : spacing) : c;
        }

        /// <summary>
        /// Performs the action, adds it to the history stack, and optionally updates the display lists
        /// </summary>
        /// <param name="name">The name of the action, for history purposes</param>
        /// <param name="action">The action to perform</param>
        public void PerformAction(string name, IAction action)
        {
            try
            {
                action.Perform(this);
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace();
                StackFrame[] frames = st.GetFrames() ?? new StackFrame[0];
                string msg = Local.LocalString("exception.document.action", name, action);
                foreach (StackFrame frame in frames)
                {
                    System.Reflection.MethodBase method = frame.GetMethod();
                    msg += "\r\n    " + method.ReflectedType.FullName + "." + method.Name;
                }
                Logger.ShowException(new Exception(msg, ex), Local.LocalString("error.document.perform_action"));
            }

            HistoryAction history = new HistoryAction(name, action);
            History.AddHistoryItem(history);
        }

        public Matrix4 SelectListTransform { get; private set; }

        public void SetSelectListTransform(Matrix4 matrix)
        {
            SelectListTransform = matrix;
            Renderer.SetSelectionTransform(matrix);
        }

        public void EndSelectionTransform()
        {
            SelectListTransform = Matrix4.Identity;
            Renderer.SetSelectionTransform(Matrix4.Identity);
        }

        public ITexture GetTexture(string name)
        {
            if (!TextureHelper.Exists(name))
            {
                TextureItem ti = TextureCollection.GetItem(name);
                if (ti != null)
                {
                    TextureProvider.LoadTextureItem(ti);
                }
            }
            return TextureHelper.Get(name);
        }

        public void RenderAll()
        {
            Map.PartialPostLoadProcess(GameData, GetTexture, SettingsManager.GetSpecialTextureOpacity);

            bool decalsUpdated = Map.UpdateDecals(this);
            bool modelsUpdated = Map.UpdateModels(this);
            bool spritesUpdated = Map.UpdateSprites(this);
            if (decalsUpdated || modelsUpdated || spritesUpdated) Mediator.Publish(EditorMediator.SelectionChanged);

            HelperManager.UpdateCache();
            Renderer.Update();
            ViewportManager.Viewports.ForEach(vp => vp.UpdateNextFrame());
        }

        public void RenderSelection(IEnumerable<MapObject> objects)
        {
            Renderer.UpdateSelection(objects);
            ViewportManager.Viewports.ForEach(vp => vp.UpdateNextFrame());
        }

        public void RenderObjects(IEnumerable<MapObject> objects)
        {
            List<MapObject> objs = objects.ToList();
            Map.PartialPostLoadProcess(GameData, GetTexture, SettingsManager.GetSpecialTextureOpacity);

            bool decalsUpdated = Map.UpdateDecals(this, objs);
            bool modelsUpdated = Map.UpdateModels(this, objs);
            bool spritesUpdated = Map.UpdateSprites(this, objs);
            if (decalsUpdated || modelsUpdated || spritesUpdated) Mediator.Publish(EditorMediator.SelectionChanged);

            HelperManager.UpdateCache();

            // If the models/decals changed, we need to do a full update
            if (modelsUpdated || decalsUpdated || spritesUpdated) Renderer.Update();
            else Renderer.UpdatePartial(objs);

            ViewportManager.Viewports.ForEach(vp => vp.UpdateNextFrame());
        }

        public void RenderFaces(IEnumerable<Face> faces)
        {
            Map.PartialPostLoadProcess(GameData, GetTexture, SettingsManager.GetSpecialTextureOpacity);
            // No need to update decals or models here: they can only be changed via entity properties
            HelperManager.UpdateCache();
            Renderer.UpdatePartial(faces);
            ViewportManager.Viewports.ForEach(vp => vp.UpdateNextFrame());
        }

        public void Make3D(ViewportBase viewport, Viewport3D.ViewType type)
        {
            Viewport3D vp = ViewportManager.Make3D(viewport, type);
            vp.RenderContext.Add(new WidgetLinesRenderable());
            Renderer.Register(new[] { vp });
            vp.RenderContext.Add(new ToolRenderable());
            vp.RenderContext.Add(new HelperRenderable(this));
            Renderer.UpdateGrid(Map.GridSpacing, Map.Show2DGrid, Map.Show3DGrid, false);
        }

        public void Make2D(ViewportBase viewport, Viewport2D.ViewDirection direction)
        {
            Viewport2D vp = ViewportManager.Make2D(viewport, direction);
            Renderer.Register(new[] { vp });
            vp.RenderContext.Add(new ToolRenderable());
            vp.RenderContext.Add(new HelperRenderable(this));
            Renderer.UpdateGrid(Map.GridSpacing, Map.Show2DGrid, Map.Show3DGrid, false);
        }

        public IEnumerable<string> GetUsedTextures()
        {
            return Map.WorldSpawn.Find(x => x is Solid).OfType<Solid>().SelectMany(x => x.Faces).Select(x => x.Texture.Name).Distinct();
        }

        public IEnumerable<TexturePackage> GetUsedTexturePackages()
        {
            List<string> used = GetUsedTextures().ToList();
            return TextureCollection.Packages.Where(x => used.Any(x.HasTexture));
        }
    }
}
