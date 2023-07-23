using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Compile;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Providers;
using CBRE.Common;
using CBRE.Common.Shell.Commands;
using CBRE.DataStructures.GameData;
using CBRE.DataStructures.Geometric;
using CBRE.FileSystem;
using CBRE.Providers.GameData;
using CBRE.Providers.Texture;
using Path = System.IO.Path;

namespace CBRE.BspEditor.Environment.ContainmentBreach
{
    public class ContainmentBreachEnvironment : IEnvironment
    {
        private readonly ITexturePackageProvider _genericProvider;
        private readonly ITexturePackageProvider _spriteProvider;
        private readonly IGameDataProvider _jsonProvider;

        private readonly Lazy<Task<TextureCollection>> _textureCollection;
        private readonly List<IEnvironmentData> _data;
        private readonly Lazy<Task<GameData>> _gameData;

        public string Engine => "Blitz3D";
        public string ID { get; set; }
        public string Name { get; set; }

        public List<string> FgdFiles { get; set; }

        public string GraphicsDirectory { get; set; }
        public string GameDirectory { get; set; }
        
        public string DefaultPointEntity { get; set; }
        public string DefaultBrushEntity { get; set; }
        public bool OverrideMapSize { get; set; }
        public decimal MapSizeLow { get; set; }
        public decimal MapSizeHigh { get; set; }

        public decimal DefaultTextureScale { get; set; } = 1;

        private IFile _root;

        public IFile Root
        {
            get
            {
                if (_root == null)
                {
                    var dirs = Directories.Where(Directory.Exists).ToList();
                    if (dirs.Any()) _root = new RootFile(Name, dirs.Select(x => new NativeFile(x)));
                    else _root = new VirtualFile(null, "");
                }
                return _root;
            }
        }

        public IEnumerable<string> Directories
        {
            get
            {
                yield return Path.Combine(GameDirectory, GraphicsDirectory);

                // Editor location to the path, for sprites and the like
                yield return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        public ContainmentBreachEnvironment()
        {
            _genericProvider = Container.Get<ITexturePackageProvider>("Generic");
            _spriteProvider = Container.Get<ITexturePackageProvider>("Spr");
            _jsonProvider = Container.Get<IGameDataProvider>("Fgd");

            _textureCollection = new Lazy<Task<TextureCollection>>(MakeTextureCollectionAsync);
            _gameData = new Lazy<Task<GameData>>(MakeGameDataAsync);
            _data = new List<IEnvironmentData>();

            FgdFiles = new List<string>();
        }

        private async Task<TextureCollection> MakeTextureCollectionAsync()
        {
            IFile graphicsDirectory = Root.GetChild(GraphicsDirectory);

            var genericRefs = _genericProvider.GetPackagesInFile(Name, graphicsDirectory);
            var generics = await _genericProvider.GetTexturePackages(Name, genericRefs);

            // TODO: Remove spr provider once entity sprites are able to be loaded with the generic provider.
            var spriteRefs = _spriteProvider.GetPackagesInFile(null, Root);
            var sprites = await _spriteProvider.GetTexturePackages(null, spriteRefs);

            return new ContainmentBreachTextureCollection(generics.Union(sprites));
        }

        private Task<GameData> MakeGameDataAsync()
        {
            return Task.FromResult(_jsonProvider.GetGameDataFromFiles(FgdFiles));
        }

        public Task<TextureCollection> GetTextureCollection()
        {
            return _textureCollection.Value;
        }

        public Task<GameData> GetGameData()
        {
            return _gameData.Value;
        }

        public async Task UpdateDocumentData(MapDocument document)
        {
            // Ensure that worldspawn has the correct entity data
            var ed = document.Map.Root.Data.GetOne<EntityData>();

            if (ed == null)
            {
                ed = new EntityData();
                document.Map.Root.Data.Add(ed);
            }

            ed.Name = "root";
        }

        private IEnumerable<string> GetUsedTextures(MapDocument document)
        {
            return document.Map.Root.FindAll().SelectMany(x => x.Data.OfType<ITextured>()).Select(x => x.Texture.Name).Distinct();
        }

        private IEnumerable<TexturePackage> GetUsedTexturePackages(MapDocument document, TextureCollection collection)
        {
            var used = GetUsedTextures(document).ToList();
            return collection.Packages.Where(x => used.Any(x.HasTexture));
        }

        public void AddData(IEnvironmentData data)
        {
            if (!_data.Contains(data)) _data.Add(data);
        }

        public IEnumerable<T> GetData<T>() where T : IEnvironmentData
        {
            return _data.OfType<T>();
        }

        private async Task ExportDocumentForBatch(MapDocument doc, string path, Box cordonBounds)
        {
            var cordonTextureName = "BLACK"; // todo make this configurable

            if (cordonBounds != null && !cordonBounds.IsEmpty())
            {
                doc = doc.CloneWithCordon(cordonBounds, cordonTextureName);
            }

            await Oy.Publish("Command:Run", new CommandMessage("Internal:ExportDocument", new
            {
                Document = doc,
                Path = path,
                LoaderHint = nameof(MapBspSourceProvider)
            }));
        }

        public Task<Batch> CreateBatch(IEnumerable<BatchArgument> arguments, BatchOptions options)
        {
            //var args = arguments.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.First().Arguments);

            //var batch = new Batch();

            //// Create the working directory
            //batch.Steps.Add(new BatchCallback(BatchStepType.CreateWorkingDirectory, async (b, d) =>
            //{
            //    var workingDir = options.WorkingDirectory ?? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            //    if (!Directory.Exists(workingDir)) Directory.CreateDirectory(workingDir);
            //    b.Variables["WorkingDirectory"] = workingDir;

            //    await Oy.Publish("Compile:Debug", $"Working directory is: {workingDir}\r\n");
            //}));

            //// Save the file to the working directory
            //batch.Steps.Add(new BatchCallback(BatchStepType.ExportDocument, async (b, d) =>
            //{
            //    var fn = options.MapFileName ?? d.FileName;
            //    var ext = options.MapFileExtension ?? ".map";
            //    if (String.IsNullOrWhiteSpace(fn) || fn.IndexOf('.') < 0) fn = Path.GetRandomFileName();
            //    var mapFile = Path.GetFileNameWithoutExtension(fn) + ext;
            //    b.Variables["MapFileName"] = mapFile;

            //    var path = Path.Combine(b.Variables["WorkingDirectory"], mapFile);
            //    b.Variables["MapFile"] = path;

            //    if (options.ExportDocument != null)
            //    {
            //        await options.ExportDocument(d, path);
            //    }
            //    else
            //    {
            //        var useCordon = options.UseCordonBounds.GetValueOrDefault(true);
            //        Box bounds = null;
            //        if (useCordon && options.CordonBounds != null)
            //        {
            //            bounds = options.CordonBounds;
            //        }
            //        else if (useCordon)
            //        {
            //            var cb = d.Map.Data.GetOne<CordonBounds>();
            //            if (cb != null && cb.Enabled && !cb.Box.IsEmpty()) bounds = cb.Box;
            //        }
            //        await ExportDocumentForBatch(d, path, bounds);
            //    }

            //    await Oy.Publish("Compile:Debug", $"Map file is: {path}\r\n");
            //}));
            
            //// Run the compile tools
            //if (args.ContainsKey("CSG")) batch.Steps.Add(new BatchProcess(BatchStepType.RunBuildExecutable, Path.Combine(ToolsDirectory, CsgExe), args["CSG"] + " \"{MapFile}\""));
            //if (args.ContainsKey("BSP")) batch.Steps.Add(new BatchProcess(BatchStepType.RunBuildExecutable, Path.Combine(ToolsDirectory, BspExe), args["BSP"] + " \"{MapFile}\""));
            //if (args.ContainsKey("VIS")) batch.Steps.Add(new BatchProcess(BatchStepType.RunBuildExecutable, Path.Combine(ToolsDirectory, VisExe), args["VIS"] + " \"{MapFile}\""));
            //if (args.ContainsKey("RAD")) batch.Steps.Add(new BatchProcess(BatchStepType.RunBuildExecutable, Path.Combine(ToolsDirectory, RadExe), args["RAD"] + " \"{MapFile}\""));

            //// Check for errors
            //batch.Steps.Add(new BatchCallback(BatchStepType.CheckIfSuccessful, async (b, d) =>
            //{
            //    var errFile = Path.ChangeExtension(b.Variables["MapFile"], "err");
            //    if (errFile != null && File.Exists(errFile))
            //    {
            //        var errors = File.ReadAllText(errFile);
            //        b.Successful = false;
            //        await Oy.Publish("Compile:Error", errors);
            //    }

            //    var bspFile = Path.ChangeExtension(b.Variables["MapFile"], "bsp");
            //    if (bspFile != null && !File.Exists(bspFile))
            //    {
            //        b.Successful = false;
            //    }
            //}));

            //// Copy resulting files around
            //batch.Steps.Add(new BatchCallback(BatchStepType.ProcessBuildResults, (b, d) =>
            //{
            //    var mapDir = Path.GetDirectoryName(d.FileName);
            //    var gameMapDir = Path.Combine(BaseDirectory, ModDirectory, "maps");

            //    // Copy configured files to the map directory
            //    CopyFile(MapCopyBsp, "bsp", mapDir);
            //    CopyFile(MapCopyMap, "map", mapDir);
            //    CopyFile(MapCopyRes, "res", mapDir);
            //    CopyFile(MapCopyErr, "err", mapDir);
            //    CopyFile(MapCopyLog, "log", mapDir);

            //    // Always copy pointfiles if they exist
            //    CopyFile(true, "lin", mapDir);
            //    CopyFile(true, "pts", mapDir);

            //    // Copy the BSP/RES to the game dir if configured
            //    CopyFile(b.Successful && GameCopyBsp, "bsp", gameMapDir);
            //    CopyFile(b.Successful && GameCopyBsp, "res", gameMapDir);

            //    void CopyFile(bool test, string extension, string directory)
            //    {
            //        if (!test || directory == null || !Directory.Exists(directory)) return;

            //        var file = Path.ChangeExtension(b.Variables["MapFile"], extension);
            //        if (file == null || !File.Exists(file)) return;

            //        File.Copy(file, Path.Combine(directory, Path.GetFileName(file)), true);
            //    }

            //    return Task.CompletedTask;
            //}));

            //// Delete temp directory
            //batch.Steps.Add(new BatchCallback(BatchStepType.DeleteWorkingDirectory, (b, d) =>
            //{
            //    var workingDir = b.Variables["WorkingDirectory"];
            //    if (Directory.Exists(workingDir)) Directory.Delete(workingDir, true);
            //    return Task.CompletedTask;
            //}));

            //if (options.RunGame ?? GameRun)
            //{
            //    batch.Steps.Add(new BatchCallback(BatchStepType.RunGame, (b, d) =>
            //    {
            //        if (!b.Successful) return Task.CompletedTask;
                    
            //        var silent = options.AllowUserInterruption.GetValueOrDefault(false);

            //        if (options.AskRunGame ?? GameAsk)
            //        {
            //            // We can't ask to run the game if interruption isn't allowed
            //            if (silent) return Task.CompletedTask;

            //            var ask = MessageBox.Show(
            //                $"The compile of {d.Name} completed successfully.\nWould you like to run the game now?",
            //                "Compile Successful!",
            //                MessageBoxButtons.YesNo,
            //                MessageBoxIcon.Question
            //            );
            //            if (ask != DialogResult.Yes) return Task.CompletedTask;
            //        }

            //        var exe = Path.Combine(BaseDirectory, GameExe);
            //        if (!File.Exists(exe))
            //        {
            //            if (!silent)
            //            {
            //                MessageBox.Show(
            //                    "The location of the game executable is incorrect. Please ensure that the game configuration has been set up correctly.",
            //                    "Failed to launch!",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error
            //                );
            //            }

            //            return Task.CompletedTask;
            //        }

            //        var gameArg = ModDirectory == "valve" ? "" : $"-game {ModDirectory} ";
            //        var mapName = Path.GetFileNameWithoutExtension(b.Variables["MapFileName"]);

            //        var flags = $"{gameArg}-dev -console +map \"{mapName}\"";
            //        try
            //        {
            //            Process.Start(exe, flags);
            //        }
            //        catch (Exception ex)
            //        {
            //            if (!silent)
            //            {
            //                MessageBox.Show(
            //                    "Launching game failed: " + ex.Message,
            //                    "Failed to launch!",
            //                    MessageBoxButtons.OK, MessageBoxIcon.Error
            //                );
            //            }
            //        }

            //        return Task.CompletedTask;
            //    }));
            //}

            //if (options.BatchSteps != null)
            //{
            //    batch.Steps.RemoveAll(x => !options.BatchSteps.Contains(x.StepType));
            //}

            //return Task.FromResult(batch);

            return Task.FromResult(new Batch());
        }

        private static readonly string AutoVisgroupPrefix = typeof(ContainmentBreachEnvironment).Namespace + ".AutomaticVisgroups";

        public IEnumerable<AutomaticVisgroup> GetAutomaticVisgroups()
        {
            // Entities
            yield return new AutomaticVisgroup(x => x is Entity && x.Hierarchy.HasChildren)
            {
                Path = $"{AutoVisgroupPrefix}.Entities",
                Key = $"{AutoVisgroupPrefix}.BrushEntities"
            };
            yield return new AutomaticVisgroup(x => x is Entity && !x.Hierarchy.HasChildren)
            {
                Path = $"{AutoVisgroupPrefix}.Entities",
                Key = $"{AutoVisgroupPrefix}.PointEntities"
            };

            // World geometry
            yield return new AutomaticVisgroup(x => x is Solid s && s.FindClosestParent(p => p is Entity) == null)
            {
                Path = $"{AutoVisgroupPrefix}.WorldGeometry",
                Key = $"{AutoVisgroupPrefix}.Brushes"
            };
        }
    }
}