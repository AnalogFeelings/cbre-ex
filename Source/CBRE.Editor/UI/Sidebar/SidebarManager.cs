using CBRE.Common.Mediator;
using CBRE.Editor.Tools;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI.Sidebar
{
    public class SidebarManager : IMediatorListener
    {
        private static SidebarManager _instance;

        private SidebarManager()
        {
            Mediator.Subscribe(EditorMediator.ToolSelected, this);
        }

        private static SidebarContainer _container;
        private static List<SidebarPanel> _toolPanels;

        public static void Init(DockedPanel container)
        {
            _instance = new SidebarManager();

            _container = new SidebarContainer { Dock = DockStyle.Fill };

            if (CBRE.Settings.Layout.SidebarWidth <= 10) container.Hidden = true;
            else container.Width = CBRE.Settings.Layout.SidebarWidth;
            container.Resize += (s, e) => CBRE.Settings.Layout.SidebarWidth = container.Hidden ? 0 : container.Width;

            container.Controls.Add(_container);

            CreatePanel(Local.LocalString("setting.textures"), new TextureSidebarPanel());
            CreatePanel(Local.LocalString("sidebar.visgroups"), new VisgroupSidebarPanel());
            CreatePanel(Local.LocalString("sidebar.help"), new HelpSidebarPanel());
            //CreatePanel("Entities", new EntitySidebarPanel());
            //CreatePanel("Brushes", new BrushSidebarPanel());
        }

        private static SidebarPanel CreatePanel(string text, Control contents, bool insert = false)
        {
            SidebarPanel panel = new SidebarPanel { Text = text, Name = text, Dock = DockStyle.Fill, Hidden = !Expanded(text) };
            panel.AddControl(contents);
            if (insert) _container.Insert(panel, _container.Count() - 1);
            else _container.Add(panel);
            return panel;
        }

        private static void RemovePanel(SidebarPanel panel)
        {
            _container.Remove(panel);
            panel.Controls.Clear();
            panel.Dispose();
        }

        public static void SaveLayout()
        {
            if (_container == null) return;
            string serialised = String.Join(";", _container.GetControls()
                .OfType<SidebarPanel>()
                .Select(x => x.Text + ":" + (x.Hidden ? '0' : '1')));
            CBRE.Settings.Layout.SidebarLayout = serialised;
        }

        private static bool Expanded(string text)
        {
            return CBRE.Settings.Layout.SidebarLayout.Split(';')
                .Select(x => x.Split(':'))
                .Where(x => x.Length == 2 && x[0] == text)
                .Select(x => x[1])
                .FirstOrDefault() != "0";
        }

        private static void ToolSelected()
        {
            if (_toolPanels != null) _toolPanels.ForEach(RemovePanel);
            _toolPanels = null;
            if (ToolManager.ActiveTool == null) return;

            List<KeyValuePair<string, Control>> controls = ToolManager.ActiveTool.GetSidebarControls().ToList();
            if (!controls.Any()) return;

            _toolPanels = controls.Select(x => CreatePanel(x.Key, x.Value, true)).ToList();
        }

        public void Notify(string message, object data)
        {
            Mediator.ExecuteDefault(this, message, data);
        }
    }
}
