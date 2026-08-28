using CBRE.Common.Mediator;
using CBRE.DataStructures.Geometric;
using CBRE.Editor.Documents;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.UI;
using System;
using System.Windows.Forms;

namespace CBRE.Editor.UI
{
    public sealed class ViewportContextMenu : ContextMenuStrip
    {
        internal static ViewportContextMenu Instance { get; private set; }

        static ViewportContextMenu()
        {
            Instance = new ViewportContextMenu();
        }

        public void AddNonSelectionItems(Document doc, ViewportBase viewport)
        {
            Items.Clear();
            Add("Paste", HotkeysMediator.OperationsPaste, Clipboard.ClipboardManager.CanPaste());
            Add("Paste Special", HotkeysMediator.OperationsPasteSpecial, Clipboard.ClipboardManager.CanPaste());
            Items.Add(new ToolStripSeparator());
            Add(doc.History.GetUndoString(), HotkeysMediator.HistoryUndo, doc.History.CanUndo());
            Add(doc.History.GetRedoString(), HotkeysMediator.HistoryRedo, doc.History.CanRedo());
        }

        public void AddSelectionItems(Document doc, ViewportBase viewport)
        {
            Items.Clear();
            Add(Local.LocalString("menu.edit.cut"), HotkeysMediator.OperationsCut);
            Add(Local.LocalString("menu.edit.copy"), HotkeysMediator.OperationsCopy);
            Add(Local.LocalString("menu.edit.delete"), HotkeysMediator.OperationsDelete);
            Add(Local.LocalString("menu.edit.paste_special"), HotkeysMediator.OperationsPasteSpecial, Clipboard.ClipboardManager.CanPaste());
            Items.Add(new ToolStripSeparator());
            Add(Local.LocalString("menu.tools.transform"), HotkeysMediator.Transform);
            Items.Add(new ToolStripSeparator());
            Add(doc.History.GetUndoString(), HotkeysMediator.HistoryUndo, doc.History.CanUndo());
            Add(doc.History.GetRedoString(), HotkeysMediator.HistoryRedo, doc.History.CanRedo());
            Items.Add(new ToolStripSeparator());
            Add(Local.LocalString("menu.tools.carve"), HotkeysMediator.Carve);
            Add(Local.LocalString("menu.tools.make_hollow"), HotkeysMediator.MakeHollow);
            Items.Add(new ToolStripSeparator());
            Add(Local.LocalString("menu.tools.group"), HotkeysMediator.GroupingGroup);
            Add(Local.LocalString("menu.tools.ungroup"), HotkeysMediator.GroupingUngroup);
            Items.Add(new ToolStripSeparator());
            Add(Local.LocalString("menu.tools.tie_to_entity"), HotkeysMediator.TieToEntity);
            Add(Local.LocalString("menu.tools.move_to_world"), HotkeysMediator.TieToWorld);
            Items.Add(new ToolStripSeparator());
            Viewport2D vp = viewport as Viewport2D;
            if (vp != null)
            {
                Coordinate flat = vp.Flatten(new Coordinate(1, 2, 3));
                HotkeysMediator left = flat.X == 1 ? HotkeysMediator.AlignXMin : (flat.X == 2 ? HotkeysMediator.AlignYMin : HotkeysMediator.AlignZMin);
                HotkeysMediator right = flat.X == 1 ? HotkeysMediator.AlignXMax : (flat.X == 2 ? HotkeysMediator.AlignYMax : HotkeysMediator.AlignZMax);
                HotkeysMediator bottom = flat.Y == 1 ? HotkeysMediator.AlignXMin : (flat.Y == 2 ? HotkeysMediator.AlignYMin : HotkeysMediator.AlignZMin);
                HotkeysMediator top = flat.Y == 1 ? HotkeysMediator.AlignXMax : (flat.Y == 2 ? HotkeysMediator.AlignYMax : HotkeysMediator.AlignZMax);
                Items.Add(new ToolStripMenuItem(Local.LocalString("menu.align"), null,
                                                CreateMenuItem(Local.LocalString("menu.align.top"), top),
                                                CreateMenuItem(Local.LocalString("menu.align.left"), left),
                                                CreateMenuItem(Local.LocalString("menu.align.right"), right),
                                                CreateMenuItem(Local.LocalString("menu.align.bottom"), bottom)));
            }
            Add(Local.LocalString("menu.properties"), HotkeysMediator.ObjectProperties);
        }

        private void Add(string name, Enum onclick, bool enabled = true)
        {
            ToolStripItem mi = CreateMenuItem(name, onclick);
            mi.Enabled = enabled;
            Items.Add(mi);
        }

        private static ToolStripItem CreateMenuItem(string name, Enum onclick)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Click += (sender, args) => Mediator.Publish(onclick);
            return item;
        }
    }
}