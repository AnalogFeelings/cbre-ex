using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LogicAndTrick.Oy;
using CBRE.Common.Scheduling;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Settings;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Components
{
    [Export(typeof(ISettingsContainer))]
    public class Autosaver : ISettingsContainer
    {
        private readonly DocumentRegister _documentRegister;

        [Setting] private bool Enabled { get; set; } = true;
        [Setting] private int IntervalMinutes { get; set; } = 5;
        [Setting] private int RetainNumber { get; set; } = 5;
        [Setting] private bool SaveToAlternateDirectory { get; set; } = false;
        [Setting] private string AutosaveDirectory { get; set; } = "";
        [Setting] private bool OnlySaveIfChanged { get; set; } = true;
        [Setting] private bool SaveDocumentOnAutosave { get; set; } = true;

        [ImportingConstructor]
        public Autosaver(
            [Import] Lazy<DocumentRegister> documentRegister
        )
        {
            _documentRegister = documentRegister.Value;
        }

        public string Name => "CBRE.Shell.Autosaver";

        public IEnumerable<SettingKey> GetKeys()
        {
            yield return new SettingKey("Autosaving", "Enabled", typeof(bool));
            yield return new SettingKey("Autosaving", "IntervalMinutes", typeof(int)) { EditorHint = "1,60" };
            yield return new SettingKey("Autosaving", "RetainNumber", typeof(int)) { EditorHint = "0,100" };
            yield return new SettingKey("Autosaving", "SaveToAlternateDirectory", typeof(bool));
            yield return new SettingKey("Autosaving", "AutosaveDirectory", typeof(string)) { EditorHint = "Directory" };
            yield return new SettingKey("Autosaving", "OnlySaveIfChanged", typeof(bool));
            yield return new SettingKey("Autosaving", "SaveDocumentOnAutosave", typeof(bool));
        }

        public void LoadValues(ISettingsStore store)
        {
            store.LoadInstance(this);
            Reschedule();
        }

        public void StoreValues(ISettingsStore store)
        {
            store.StoreInstance(this);
        }

        private void Reschedule()
        {
            Scheduler.RemoveContext<Autosaver>(x => x == this);
            if (!Enabled) return;

            int at = Math.Max(1, IntervalMinutes);
            Scheduler.Schedule(this, Autosave, Schedule.Interval(TimeSpan.FromMinutes(at)));
        }

        private void Autosave()
        {
            foreach (IDocument doc in _documentRegister.OpenDocuments)
            {
                if (OnlySaveIfChanged && !doc.HasUnsavedChanges) continue;
                try
                {
                    Autosave(doc);
                    if (SaveDocumentOnAutosave) Save(doc);
                }
                catch
                {
                    // 
                }
            }
        }

        private void Autosave(IDocument document)
        {
            string fs = GetAutosaveFormatString(document);
            string directory = GetAutosaveFolder(document);
            if (fs == null || directory == null) return;

            // Get the filename and ensure it doesn't exist
            string date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd-hh-mm-ss");
            string filename = Path.Combine(directory, string.Format(fs, date));
            if (File.Exists(filename)) File.Delete(filename);

            // Delete excessive autosaves
            if (RetainNumber > 0)
            {
                Dictionary<string, DateTime> asFiles = GetExistingAutosaveFiles(directory, fs);
                foreach (KeyValuePair<string, DateTime> file in asFiles.OrderByDescending(x => x.Value).Skip(RetainNumber))
                {
                    if (File.Exists(file.Key)) File.Delete(file.Key);
                }
            }

            // Save the file
            _documentRegister.ExportDocument(document, filename);
        }

        private void Save(IDocument document)
        {
            string filename = document.FileName;
            if (filename == null || !Directory.Exists(Path.GetDirectoryName(filename))) return;

            _documentRegister.SaveDocument(document, filename);
        }

        private string GetAutosaveFormatString(IDocument document)
        {
            string path = document.FileName;
            if (path == null) return null;

            string we = Path.GetFileNameWithoutExtension(path);
            string ex = Path.GetExtension(path);
            return we + ".auto.{0}" + ex;
        }

        private string GetAutosaveFolder(IDocument document)
        {
            if (SaveToAlternateDirectory && Directory.Exists(AutosaveDirectory)) return AutosaveDirectory;
            string dir = Path.GetDirectoryName(document.FileName);
            return Directory.Exists(dir) ? dir : null;
        }

        public Dictionary<string, DateTime> GetExistingAutosaveFiles(string directory, string formatString)
        {
            Dictionary<string, DateTime> ret = new Dictionary<string, DateTime>();
            if (formatString == null || directory == null) return ret;

            // Search for matching files
            string[] files = Directory.GetFiles(directory, string.Format(formatString, "*"));
            foreach (string file in files)
            {
                // Match the date portion with a regex
                string re = Regex.Escape(formatString.Replace("{0}", ":")).Replace(":", "{0}");
                string regex = string.Format(re, "(\\d{4})-(\\d{2})-(\\d{2})-(\\d{2})-(\\d{2})-(\\d{2})");
                Match match = Regex.Match(Path.GetFileName(file), regex, RegexOptions.IgnoreCase);
                if (!match.Success) continue;

                // Parse the date and add it if it is valid
                bool result = DateTime.TryParse(
                    string.Format("{0}-{1}-{2}T{3}:{4}:{5}Z",
                        match.Groups[1].Value, match.Groups[2].Value,
                        match.Groups[3].Value, match.Groups[4].Value,
                        match.Groups[5].Value, match.Groups[6].Value
                    ),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out DateTime date
                );
                if (result) ret.Add(file, date);
            }
            return ret;
        }
    }
}
