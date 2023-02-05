using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Translations;
using DiscordRPC;
using LogicAndTrick.Oy;

namespace CBRE.Shell.Components
{
    [Export(typeof(ISettingsContainer))]
    [Export(typeof(IInitialiseHook))]
    [AutoTranslate]
    public class DiscordManager : IInitialiseHook, ISettingsContainer
    {
        [Setting("EnableDiscordPresence")] 
        private bool Enable = false;

        [Import] 
        private IContext Context;

        private DiscordRpcClient Client { get; set; }

        private readonly RichPresence BasicPresence = new RichPresence()
        {
            Assets = new Assets()
            {
                LargeImageKey = "logo",
                LargeImageText = "Version " + typeof(DiscordManager).Assembly.GetName().Version.ToString(3)
            },
            Timestamps = Timestamps.Now,
            Buttons = new Button[]
            {
                new Button()
                {
                    Label = "GitHub",
                    Url = "https://github.com/AestheticalZ/cbre-ex"
                }
            }
        };

        ~DiscordManager()
        {
            DestroyClient();
        }

        public async Task OnInitialise()
        {
            Oy.Subscribe<IDocument>("Document:Activated", DocumentActivated);
        }

        private void DocumentActivated(IDocument document)
        {
            if (Client == null) return;

            if (document == null || document is NoDocument)
            {
                SetNothingOpen();
            }
            else
            {
                SetCurrentDocument(document);
            }
        }

        private void InitClient()
        {
            DestroyClient();
            if (!Enable) return;

            Client = new DiscordRpcClient("1036415011742032013");
            Client.Initialize();

            Client.SetPresence(BasicPresence);

            IDocument currentDocument = Context.Get<IDocument>("ActiveDocument");
            if (currentDocument != null)
            {
                SetCurrentDocument(currentDocument);
            }
            else
            {
                SetNothingOpen();
            }
        }

        private void DestroyClient()
        {
            if (Client != null)
            {
                Client.SetPresence(null);
                Client.Dispose();

                Client = null;
            }
        }

        private void SetCurrentDocument(IDocument document)
        {
            Client.UpdateDetails("Editing a room");
            Client.UpdateState(document.Name);
            Client.UpdateStartTime();
        }

        private void SetNothingOpen()
        {
            Client.UpdateDetails("No rooms opened");
            Client.UpdateState(string.Empty);
            Client.UpdateStartTime();
        }

        public string Name => "CBRE.Shell.DiscordManager";

        public IEnumerable<SettingKey> GetKeys()
        {
            yield return new SettingKey("Interface", "EnableDiscordPresence", typeof(bool));
        }

        public void LoadValues(ISettingsStore store)
        {
            store.LoadInstance(this);
            InitClient();
        }

        public void StoreValues(ISettingsStore store)
        {
            store.StoreInstance(this);
        }
    }
}
