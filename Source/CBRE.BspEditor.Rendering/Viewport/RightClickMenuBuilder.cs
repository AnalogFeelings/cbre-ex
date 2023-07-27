using CBRE.BspEditor.Rendering.Properties;
using CBRE.Common.Shell.Commands;
using LogicAndTrick.Oy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CBRE.BspEditor.Rendering.Viewport
{
    public class RightClickMenuBuilder
    {
        public ViewportEvent Event { get; }
        public MapViewport Viewport { get; }
        public bool Intercepted { get; set; }
        private List<ToolStripItem> Items { get; }
        public bool IsEmpty => Items.Count == 0;

        public RightClickMenuBuilder(MapViewport viewport, ViewportEvent viewportEvent)
        {
            Event = viewportEvent;
            Viewport = viewport;
            Items = new List<ToolStripItem>
            {
                new CommandItem("BspEditor:Edit:Paste", Resources.Menu_Paste),
                new CommandItem("BspEditor:Edit:PasteSpecial", Resources.Menu_PasteSpecial),
                new ToolStripSeparator(),
                new CommandItem("BspEditor:Edit:Undo", Resources.Menu_Undo),
                new CommandItem("BspEditor:Edit:Redo", Resources.Menu_Redo)
            };
        }

        public ToolStripMenuItem CreateCommandItem(string commandId, Bitmap iconBitmap = null, object parameters = null)
        {
            return new CommandItem(commandId, iconBitmap, parameters);
        }

        public ToolStripMenuItem AddCommand(string commandId, Bitmap iconBitmap = null, object parameters = null)
        {
            ToolStripMenuItem mi = CreateCommandItem(commandId, iconBitmap, parameters);
            Items.Add(mi);
            return mi;
        }

        public ToolStripMenuItem AddCallback(string description, Action callback)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem(description);
            mi.Click += (s, e) => callback();
            Items.Add(mi);
            return mi;
        }

        public ToolStripSeparator AddSeparator()
        {
            ToolStripSeparator mi = new ToolStripSeparator();
            Items.Add(mi);
            return mi;
        }

        public ToolStripMenuItem AddGroup(string description)
        {
            ToolStripMenuItem g = new ToolStripMenuItem(description);
            Items.Add(g);
            return g;
        }

        public void Add(params ToolStripItem[] items)
        {
            Items.AddRange(items);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public void Populate(ContextMenuStrip menu)
        {
            menu.Items.Clear();
            foreach (ToolStripItem command in Items)
            {
                menu.Items.Add(command);
            }
        }

        private class CommandItem : ToolStripMenuItem
        {
            private readonly string _commandID;
            private readonly object _parameters;

            public CommandItem(string commandID, Bitmap iconBitmap = null, object parameters = null)
            {
                _commandID = commandID;
                _parameters = parameters;
                Click += RunCommand;

                Shell.Registers.CommandRegister register = Common.Container.Get<Shell.Registers.CommandRegister>();
                ICommand cmd = register.Get(_commandID);
                Text = cmd == null ? _commandID : cmd.Name;

                base.Image = iconBitmap == null ? null : iconBitmap as Image;
            }

            private void RunCommand(object sender, EventArgs e)
            {
                Oy.Publish("Command:Run", new CommandMessage(_commandID, _parameters));
            }
        }
    }
}