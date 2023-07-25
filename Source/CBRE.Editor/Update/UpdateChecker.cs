using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.Common.Scheduling;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;
using CBRE.Shell;

namespace CBRE.Editor.Update
{
    [Export(typeof(IInitialiseHook))]
    [Export(typeof(ISettingsContainer))]
    [AutoTranslate]
    public class UpdateChecker : IInitialiseHook, ISettingsContainer
    {
        private readonly Form _shell;

        private string _updateFileToInstall;

        [Setting("CheckForUpdates")] private bool _checkForUpdates = true;

        public string UpdateDownloadedTitle { get; set; }
        public string UpdateDownloadedMessage { get; set; }

        [ImportingConstructor]
        public UpdateChecker([Import("Shell")] Form shell)
        {
            _shell = shell;
        }

        public Task OnInitialise()
        {
#if DEBUG
            _checkForUpdates = false;
#endif
            if (_checkForUpdates)
            {
                Scheduler.Schedule(this, CheckForUpdates, TimeSpan.FromSeconds(5));
            }

            Oy.Subscribe<string>("CBRE:Editor:UpdateDownloaded", OnUpdateDownloaded);

            Application.ApplicationExit += OnApplicationExit;

            return Task.CompletedTask;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= OnApplicationExit;

            if (_updateFileToInstall == null || !File.Exists(_updateFileToInstall)) return;

            string loc = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            string arguments = "/S" + (loc != null ? " /D=" + loc : "");
#if DEBUG
            arguments = "";
#endif
            Process.Start(_updateFileToInstall, arguments);
        }

        private Task OnUpdateDownloaded(string file)
        {
            _shell.InvokeLater(() =>
            {
                if (!File.Exists(file)) return;

                DialogResult res = MessageBox.Show(UpdateDownloadedMessage, UpdateDownloadedTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Cancel) return;

                _updateFileToInstall = file;

                if (res == DialogResult.Yes) Oy.Publish("Command:Run", new CommandMessage("File:Exit"));
            });
            return Task.CompletedTask;
        }

        private void CheckForUpdates()
        {
            Oy.Publish("Command:Run", new CommandMessage("CBRE:Editor:CheckForUpdates", new {Silent = true}));
        }

        public string Name => "CBRE.Editor.UpdateChecker";

        public IEnumerable<SettingKey> GetKeys()
        {
            yield return new SettingKey("Interface", "CheckForUpdates", typeof(bool));
        }

        public void LoadValues(ISettingsStore store)
        {
            store.LoadInstance(this);
        }

        public void StoreValues(ISettingsStore store)
        {
            store.StoreInstance(this);
        }
    }
}