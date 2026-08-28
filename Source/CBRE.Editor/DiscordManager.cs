using System;
using CBRE.Common.Mediator;
using CBRE.Editor.Documents;
using DiscordRPC;

namespace CBRE.Editor
{
    public class DiscordManager : IMediatorListener, IDisposable
    {
        private DiscordRpcClient _Client;

        private readonly RichPresence _BasicPresence = new RichPresence()
        {
            Assets = new Assets()
            {
                LargeImageKey = "logo",
                LargeImageText = "Version " + typeof(Editor).Assembly.GetName().Version.ToString(3)
            },
            Timestamps = Timestamps.Now,
            Buttons = new Button[]
            {
                new Button()
                {
                    Label = "GitHub",
                    Url = "https://github.com/SCP-CB-Community-Preservation-Project/cbre-ex"
                }
            }
        };

        public DiscordManager()
        {
            _Client = new DiscordRpcClient("1036415011742032013");
            _Client.Initialize();
            
            Mediator.Subscribe(EditorMediator.DocumentActivated, this);
            Mediator.Subscribe(EditorMediator.DocumentAllClosed, this);
            
            _Client.SetPresence(_BasicPresence);

            Document currentDocument = DocumentManager.CurrentDocument;
            if (currentDocument == null)
            {
                Mediator.Publish(EditorMediator.DocumentAllClosed);
            }
            else
            {
                Mediator.Publish(EditorMediator.DocumentActivated, currentDocument);
            }
        }

        ~DiscordManager()
        {
            Dispose();
        }

        public void Notify(string Message, object Data)
        {
            Mediator.ExecuteDefault(this, Message, Data);
        }

        public void DocumentActivated(Document Document)
        {
            _Client.UpdateDetails("Editing a room");
            _Client.UpdateState(Document.MapFileName);
            _Client.UpdateStartTime();
        }

        public void DocumentAllClosed()
        {
            _Client.UpdateDetails("No rooms opened");
            _Client.UpdateState(string.Empty);
            _Client.UpdateStartTime();
        }

        public void Dispose()
        {
            if (_Client != null)
            {
                Mediator.UnsubscribeAll(this);
                
                _Client.SetPresence(null);
                _Client.Dispose();

                _Client = null;
            }
        }
    }
}