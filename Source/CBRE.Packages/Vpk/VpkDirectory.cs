using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBRE.Packages.Vpk
{
    // https://developer.valvesoftware.com/wiki/VPK_File_Format
    public class VpkDirectory : IPackage
    {
        private const uint Signature = 0x55aa1234;
        private const string DirString = "_dir";
        internal const ushort DirectoryIndex = 0x7fff;

        public FileInfo PackageFile { get; private set; }
        public uint Version { get; private set; }
        internal uint TreeLength { get; private set; }
        internal uint HeaderLength { get; private set; }

        internal Dictionary<string, VpkEntry> Entries { get; private set; }
        internal Dictionary<ushort, FileInfo> Chunks { get; private set; }

        public VpkDirectory(FileInfo packageFile)
        {
            PackageFile = packageFile;
            Entries = new Dictionary<string, VpkEntry>();
            Chunks = new Dictionary<ushort, FileInfo>();

            string nameWithoutExt = Path.GetFileNameWithoutExtension(packageFile.Name);
            string ext = Path.GetExtension(packageFile.Name);
            if (!nameWithoutExt.EndsWith(DirString)) throw new PackageException(Local.LocalString("exception.invalid_vpk"));

            string baseName = nameWithoutExt.Substring(0, nameWithoutExt.Length - DirString.Length);

            // Scan and find all chunk files that match this vpk directory
            FileInfo[] matchingFiles = packageFile.Directory.GetFiles(baseName + "_???" + ext);
            foreach (FileInfo mf in matchingFiles)
            {
                string index = mf.Name.Substring(baseName.Length + 1, 3);
                ushort num;
                if (ushort.TryParse(index, out num))
                {
                    Chunks.Add(num, mf);
                }
            }
            Chunks[DirectoryIndex] = PackageFile;

            // Read the data from the vpk
            using (BinaryReader br = new BinaryReader(OpenFile(packageFile)))
            {
                uint sig = br.ReadUInt32();
                if (sig != Signature) throw new PackageException(Local.LocalString("exception.unknown_package_signature.hex", Signature.ToString("x8"), sig.ToString("x8")));

                Version = br.ReadUInt32();
                TreeLength = br.ReadUInt32();
                switch (Version)
                {
                    case 1:
                        HeaderLength = 12;
                        break;
                    case 2:
                        HeaderLength = 28;
                        br.ReadInt32(); // Unknown1
                        uint footerLength = br.ReadUInt32();
                        br.ReadInt32(); // Unknown3
                        br.ReadInt32(); // Unknown4
                        break;
                    default:
                        throw new PackageException(Local.LocalString("exception.unknown_version", Version));
                }

                // Read all the entries from the vpk
                ReadDirectoryEntries(br);
                BuildDirectories();
            }
        }

        internal Stream OpenFile(FileInfo file)
        {
            return Stream.Synchronized(new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.RandomAccess));
        }

        private void ReadDirectoryEntries(BinaryReader br)
        {
            String extension, path, filename;
            while ((extension = br.ReadNullTerminatedString()).Length > 0)
            {
                while ((path = br.ReadNullTerminatedString()).Length > 0)
                {
                    // Single space = root directory
                    path = path == " " ? "" : path.Replace('\\', '/').Trim('/');
                    while ((filename = br.ReadNullTerminatedString()).Length > 0)
                    {
                        // get me some file information
                        VpkEntry entry = ReadEntry(br, path + "/" + filename + "." + extension);
                        Entries.Add(entry.FullName, entry);
                    }
                }
            }
        }

        private VpkEntry ReadEntry(BinaryReader br, string path)
        {
            uint crc = br.ReadUInt32();
            ushort preloadBytes = br.ReadUInt16();
            ushort archiveIndex = br.ReadUInt16(); // 0x7fff = directory archive
            uint entryOffset = br.ReadUInt32(); // If archive directory, relative to END of directory structure
            uint entryLength = br.ReadUInt32(); // If 0, preload data contains the entire file
            ushort terminator = br.ReadUInt16();
            if (terminator != VpkEntry.EntryTerminator) throw new PackageException(Local.LocalString("exception.invalid_terminator", VpkEntry.EntryTerminator.ToString("x8"), terminator.ToString("x8")));

            byte[] preloadData = br.ReadBytes(preloadBytes);
            return new VpkEntry(this, path.ToLowerInvariant(), crc, preloadData, archiveIndex, entryOffset, entryLength);
        }

        public IEnumerable<IPackageEntry> GetEntries()
        {
            return Entries.Values;
        }

        public IPackageEntry GetEntry(string path)
        {
            path = path.ToLowerInvariant();
            return Entries.ContainsKey(path) ? Entries[path] : null;
        }

        public byte[] ExtractEntry(IPackageEntry entry)
        {
            using (BinaryReader sr = new BinaryReader(OpenStream(entry)))
            {
                return sr.ReadBytes((int)sr.BaseStream.Length);
            }
        }

        public Stream OpenStream(IPackageEntry entry)
        {
            VpkEntry pe = entry as VpkEntry;
            if (pe == null) throw new ArgumentException(Local.LocalString("exception.only_compatible_vpkentry"));
            return new BufferedStream(new VpkEntryStream(pe, this));
        }

        internal Stream OpenChunk(VpkEntry entry)
        {
            FileInfo file = Chunks[entry.ArchiveIndex];
            Stream stream = OpenFile(file);
            uint offset = entry.ArchiveIndex == DirectoryIndex ? HeaderLength + TreeLength + entry.EntryOffset : entry.EntryOffset;
            stream.Position = offset;
            return stream;
        }

        public IPackageStreamSource GetStreamSource()
        {
            return new VpkPackageStreamSource(this);
        }

        public void Dispose()
        {
            Entries.Clear();
            Chunks.Clear();
        }

        private Dictionary<string, HashSet<string>> _folders;
        private Dictionary<string, HashSet<string>> _files;

        private void BuildDirectories()
        {
            _folders = new Dictionary<string, HashSet<string>>();
            _files = new Dictionary<string, HashSet<string>>();
            foreach (IPackageEntry entry in GetEntries())
            {
                string[] split = entry.FullName.Split('/');
                string joined = "";
                for (int i = 0; i < split.Length; i++)
                {
                    string sub = split[i];
                    string name = joined.Length == 0 ? sub : joined + '/' + sub;
                    if (i == split.Length - 1)
                    {
                        // File name
                        if (!_files.ContainsKey(joined)) _files.Add(joined, new HashSet<string>());
                        _files[joined].Add(name);
                    }
                    else
                    {
                        // Folder name
                        if (!_folders.ContainsKey(joined)) _folders.Add(joined, new HashSet<string>());
                        if (!_folders[joined].Contains(sub)) _folders[joined].Add(name);
                    }
                    joined = joined.Length == 0 ? sub : joined + '/' + sub;
                }
            }
        }

        public bool HasDirectory(string path)
        {
            return _folders.ContainsKey(path);
        }

        public bool HasFile(string path)
        {
            return _files.ContainsKey(path.ToLowerInvariant());
        }

        public IEnumerable<string> GetDirectories()
        {
            return _files.Keys;
        }

        public IEnumerable<string> GetFiles()
        {
            return _files.Values.SelectMany(x => x);
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            if (!_folders.ContainsKey(path)) return new string[0];
            return _folders[path].Where(x => x.Length > 0);
        }

        public IEnumerable<string> GetFiles(string path)
        {
            if (!_files.ContainsKey(path)) return new string[0];
            return _files[path];
        }

        public IEnumerable<string> SearchDirectories(string path, string regex, bool recursive)
        {
            IEnumerable<string> files = recursive ? CollectDirectories(path) : GetDirectories(path);
            return files.Where(x => Regex.IsMatch(GetName(x), regex, RegexOptions.IgnoreCase));
        }

        public IEnumerable<string> SearchFiles(string path, string regex, bool recursive)
        {
            IEnumerable<string> files = recursive ? CollectFiles(path) : GetFiles(path);
            return files.Where(x => Regex.IsMatch(GetName(x), regex, RegexOptions.IgnoreCase));
        }

        private string GetName(string path)
        {
            int idx = path.LastIndexOf('/');
            if (idx < 0) return path;
            return path.Substring(idx + 1);
        }

        private IEnumerable<string> CollectDirectories(string path)
        {
            List<string> files = new List<string>();
            if (_folders.ContainsKey(path))
            {
                files.AddRange(_folders[path].Where(x => x.Length > 0));
                files.AddRange(_folders[path].SelectMany(CollectDirectories));
            }
            return files;
        }

        private IEnumerable<string> CollectFiles(string path)
        {
            List<string> files = new List<string>();
            if (_folders.ContainsKey(path))
            {
                files.AddRange(_folders[path].SelectMany(CollectFiles));
            }
            if (_files.ContainsKey(path))
            {
                files.AddRange(_files[path]);
            }
            return files;
        }

        public Stream OpenFile(string path)
        {
            IPackageEntry entry = GetEntry(path);
            if (entry == null) throw new FileNotFoundException();
            return OpenStream(entry);
        }
    }
}
