using CBRE.Common.Mediator;
using CBRE.Editor.Documents;
using CBRE.Editor.Properties;
using CBRE.Localization;
using CBRE.Settings;
using CBRE.Settings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.Menu
{
    public static class MenuManager
    {
        private static MenuStrip _menu;
        private static ToolStripContainer _container;
        private static readonly Dictionary<string, List<IMenuBuilder>> MenuItems;

        public static List<RecentFile> RecentFiles { get; private set; }

        static MenuManager()
        {
            RecentFiles = new List<RecentFile>();
            MenuItems = new Dictionary<string, List<IMenuBuilder>>();
        }

        public static void Init(MenuStrip menu, ToolStripContainer toolStripContainer)
        {
            _menu = menu;
            _container = toolStripContainer;
            AddDefault();
        }

        public static void Add(string menuName, IMenuBuilder builder)
        {
            if (!MenuItems.ContainsKey(menuName)) MenuItems.Add(menuName, new List<IMenuBuilder>());
            MenuItems[menuName].Add(builder);
        }

        public static void Insert(string menuName, int index, IMenuBuilder builder)
        {
            if (!MenuItems.ContainsKey(menuName)) MenuItems.Add(menuName, new List<IMenuBuilder>());
            if (index < 0) index = 0;
            if (index > MenuItems[menuName].Count) index = MenuItems[menuName].Count;
            MenuItems[menuName].Insert(index, builder);
        }

        private static void AddDefault()
        {
            Func<bool> mapOpen = () => DocumentManager.CurrentDocument != null;
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.new"), HotkeysMediator.FileNew) { Image = Resources.Menu_New, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.open"), HotkeysMediator.FileOpen) { Image = Resources.Menu_Open, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.close"), HotkeysMediator.FileClose) { IsVisible = mapOpen, Image = Resources.Menu_Close, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.save"), HotkeysMediator.FileSave) { IsVisible = mapOpen, Image = Resources.Menu_Save, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.save_as"), HotkeysMediator.FileSaveAs) { Image = Resources.Menu_SaveAs, IsVisible = mapOpen });
            Add(Local.LocalString("menu.file"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.export"), HotkeysMediator.FileCompile) { Image = Resources.Menu_ExportRmesh, IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.file"), new RecentFilesMenu());
            Add(Local.LocalString("menu.file"), new MenuSplitter());
            Add(Local.LocalString("menu.file"), new SimpleMenuBuilder(Local.LocalString("menu.file.exit"), EditorMediator.Exit));

            Func<bool> canUndo = () => mapOpen() && DocumentManager.CurrentDocument.History.CanUndo();
            Func<bool> canRedo = () => mapOpen() && DocumentManager.CurrentDocument.History.CanRedo();
            Func<string> undoText = () => mapOpen() ? DocumentManager.CurrentDocument.History.GetUndoString() : Local.LocalString("menu.edit.undo");
            Func<string> redoText = () => mapOpen() ? DocumentManager.CurrentDocument.History.GetRedoString() : Local.LocalString("menu.edit.redo");
            Func<bool> itemsSelected = () => mapOpen() && DocumentManager.CurrentDocument.Selection.GetSelectedObjects().Any();
            Func<bool> canPaste = Clipboard.ClipboardManager.CanPaste;
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.undo"), HotkeysMediator.HistoryUndo) { Image = Resources.Menu_Undo, IsVisible = mapOpen, IsActive = canUndo, Text = undoText, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.redo"), HotkeysMediator.HistoryRedo) { Image = Resources.Menu_Redo, IsVisible = mapOpen, IsActive = canRedo, Text = redoText, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.cut"), HotkeysMediator.OperationsCut) { Image = Resources.Menu_Cut, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.copy"), HotkeysMediator.OperationsCopy) { Image = Resources.Menu_Copy, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.paste"), HotkeysMediator.OperationsPaste) { Image = Resources.Menu_Paste, IsVisible = mapOpen, IsActive = canPaste, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.paste_special"), HotkeysMediator.OperationsPasteSpecial) { Image = Resources.Menu_PasteSpecial, IsVisible = mapOpen, IsActive = canPaste, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.delete"), HotkeysMediator.OperationsDelete) { Image = Resources.Menu_Delete, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.clear_selection"), HotkeysMediator.SelectionClear) { Image = Resources.Menu_ClearSelection, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.select_all"), HotkeysMediator.SelectAll) { Image = Resources.Menu_SelectAll, IsVisible = mapOpen });
            Add(Local.LocalString("menu.edit"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.edit"), new SimpleMenuBuilder(Local.LocalString("menu.edit.object_properties"), HotkeysMediator.ObjectProperties) { Image = Resources.Menu_ObjectProperties, IsVisible = mapOpen, ShowInToolStrip = true });

            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.snap_to_grid"), HotkeysMediator.ToggleSnapToGrid) { Image = Resources.Menu_SnapToGrid, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.SnapToGrid, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.show_2d_grid"), HotkeysMediator.ToggleShow2DGrid) { Image = Resources.Menu_Show2DGrid, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.Show2DGrid, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.show_3d_grid"), HotkeysMediator.ToggleShow3DGrid) { Image = Resources.Menu_Show3DGrid, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.Show3DGrid, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new GroupedMenuBuilder(Local.LocalString("menu.map.grid_settings"),
                                              new SimpleMenuBuilder(Local.LocalString("menu.map.grid_settings.smaller"), HotkeysMediator.GridDecrease) { Image = Resources.Menu_SmallerGrid, IsVisible = mapOpen, ShowInToolStrip = true },
                                              new SimpleMenuBuilder(Local.LocalString("menu.map.grid_settings.bigger"), HotkeysMediator.GridIncrease) { Image = Resources.Menu_LargerGrid, IsVisible = mapOpen, ShowInToolStrip = true }
                           )
            { IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.grid_settings.smaller"), HotkeysMediator.GridDecrease) { Image = Resources.Menu_SmallerGrid, IsVisible = mapOpen, ShowInToolStrip = true, ShowInMenu = false });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.grid_settings.bigger"), HotkeysMediator.GridIncrease) { Image = Resources.Menu_LargerGrid, IsVisible = mapOpen, ShowInToolStrip = true, ShowInMenu = false });
            Add(Local.LocalString("menu.map"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.ignore_grouping"), HotkeysMediator.ToggleIgnoreGrouping) { Image = Resources.Menu_IgnoreGrouping, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.IgnoreGrouping, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.texture_lock"), HotkeysMediator.ToggleTextureLock) { Image = Resources.Menu_TextureLock, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.TextureLock, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.texture_scaling_lock"), HotkeysMediator.ToggleTextureScalingLock) { Image = Resources.Menu_TextureScalingLock, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.TextureScalingLock, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.hide_tool_textures"), HotkeysMediator.ToggleHideToolTextures) { Image = Resources.Menu_HideToolTextures, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.HideToolTextures, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.hide_entity_sprites"), HotkeysMediator.ToggleHideEntitySprites) { Image = Resources.Menu_HideEntitySprites, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.HideEntitySprites, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.hide_map_origin"), HotkeysMediator.ToggleHideMapOrigin) { Image = Resources.Menu_HideMapOrigin, IsVisible = mapOpen, IsChecked = () => mapOpen() && DocumentManager.CurrentDocument.Map.HideMapOrigin, ShowInToolStrip = true });
            Add(Local.LocalString("menu.map"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.show_information"), HotkeysMediator.ShowMapInformation) { Image = Resources.Menu_ShowInformation, IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.show_selected_brush_id"), HotkeysMediator.ShowSelectedBrushID) { Image = Resources.Menu_ShowBrushID, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.entity_report"), HotkeysMediator.ShowEntityReport) { Image = Resources.Menu_EntityReport, IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.show_logical_tree"), HotkeysMediator.ShowLogicalTree) { Image = Resources.Menu_ShowLogicalTree, IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.map"), new SimpleMenuBuilder(Local.LocalString("menu.map.map_properties"), EditorMediator.WorldspawnProperties) { Image = Resources.Menu_MapProperties, IsVisible = mapOpen });

            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.autosize_views"), HotkeysMediator.ViewportAutosize) { Image = Resources.Menu_AutosizeViews, IsVisible = mapOpen });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.center_all_views"), HotkeysMediator.CenterAllViewsOnSelection) { Image = Resources.Menu_CenterSelectionAll, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.center_2d_views"), HotkeysMediator.Center2DViewsOnSelection) { Image = Resources.Menu_CenterSelection2D, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.center_3d_views"), HotkeysMediator.Center3DViewsOnSelection) { Image = Resources.Menu_CenterSelection3D, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.view"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.go_to_brush_id"), HotkeysMediator.GoToBrushID) { Image = Resources.Menu_GoToBrushID, IsVisible = mapOpen });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.go_to_coordinates"), HotkeysMediator.GoToCoordinates) { Image = Resources.Menu_GoToCoordinates, IsVisible = mapOpen });
            Add(Local.LocalString("menu.view"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.hide_selected_objects"), HotkeysMediator.QuickHideSelected) { Image = Resources.Menu_HideSelected, IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.hide_unselected_objects"), HotkeysMediator.QuickHideUnselected) { Image = Resources.Menu_HideUnselected, IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.view"), new SimpleMenuBuilder(Local.LocalString("menu.view.show_hidden_objects"), HotkeysMediator.QuickHideShowAll) { Image = Resources.Menu_ShowHidden, IsVisible = mapOpen, ShowInToolStrip = true });

            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.carve"), HotkeysMediator.Carve) { Image = Resources.Menu_Carve, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.make_hollow"), HotkeysMediator.MakeHollow) { Image = Resources.Menu_Hollow, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.tools"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.group"), HotkeysMediator.GroupingGroup) { Image = Resources.Menu_Group, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.ungroup"), HotkeysMediator.GroupingUngroup) { Image = Resources.Menu_Ungroup, IsVisible = mapOpen, IsActive = itemsSelected, ShowInToolStrip = true });
            Add(Local.LocalString("menu.tools"), new MenuSplitter { IsVisible = mapOpen, ShowInToolStrip = true });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.tie_to_entity"), HotkeysMediator.TieToEntity) { Image = Resources.Menu_TieToEntity, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.move_to_world"), HotkeysMediator.TieToWorld) { Image = Resources.Menu_TieToWorld, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.tools"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.replace_textures"), HotkeysMediator.ReplaceTextures) { Image = Resources.Menu_ReplaceTextures, IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.transform"), HotkeysMediator.Transform) { Image = Resources.Menu_Transform, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.snap_to_grid"), HotkeysMediator.SnapSelectionToGrid) { Image = Resources.Menu_SnapSelection, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.snap_to_grid_individually"), HotkeysMediator.SnapSelectionToGridIndividually) { Image = Resources.Menu_SnapSelectionIndividual, IsVisible = mapOpen, IsActive = itemsSelected });
            Add(Local.LocalString("menu.tools"), new GroupedMenuBuilder(Local.LocalString("menu.tools.align_objects"),
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.to_x_min"), HotkeysMediator.AlignXMin) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.to_x_max"), HotkeysMediator.AlignXMax) { IsVisible = mapOpen, IsActive = itemsSelected },                                                new SimpleMenuBuilder("To Y Axis Min", HotkeysMediator.AlignZMin) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.to_z_max"), HotkeysMediator.AlignZMax) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.to_y_min"), HotkeysMediator.AlignYMin) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.to_y_max"), HotkeysMediator.AlignYMax) { IsVisible = mapOpen, IsActive = itemsSelected }
                             )
            { Image = Resources.Menu_Align, IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new GroupedMenuBuilder(Local.LocalString("menu.tools.flip_objects"),
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.flip_objects.x"), HotkeysMediator.FlipX) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.flip_objects.y"), HotkeysMediator.FlipZ) { IsVisible = mapOpen, IsActive = itemsSelected },
                                                new SimpleMenuBuilder(Local.LocalString("menu.tools.flip_objects.z"), HotkeysMediator.FlipY) { IsVisible = mapOpen, IsActive = itemsSelected }
                             )
            { Image = Resources.Menu_Flip, IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.tools"), new SimpleMenuBuilder(Local.LocalString("menu.tools.options"), EditorMediator.OpenSettings) { Image = Resources.Menu_Options, ShowInToolStrip = true });

            Add(Local.LocalString("menu.help"), new SimpleMenuBuilder(Local.LocalString("menu.help.updates"), EditorMediator.CheckForUpdates) { Image = Resources.Menu_Update });
            Add(Local.LocalString("menu.help"), new SimpleMenuBuilder(Local.LocalString("menu.help.report"), EditorMediator.OpenWebsite, Editor.GITHUB_REPORT_BUG_URL) { Image = Resources.Menu_GitHub });
            Add(Local.LocalString("menu.help"), new MenuSplitter { IsVisible = mapOpen });
            Add(Local.LocalString("menu.help"), new SimpleMenuBuilder(Local.LocalString("menu.help.about"), EditorMediator.About) { Image = Resources.Menu_ShowInformation });
        }

        public static void UpdateRecentFilesMenu()
        {
            RebuildPartial(Local.LocalString("menu.file"));
        }

        public static void RebuildPartial(string name)
        {
            if (!MenuItems.ContainsKey(name)) return;
            List<IMenuBuilder> mi = MenuItems[name];

            foreach (ToolStripMenuItem menu in _menu.Items)
            {
                if (menu.Text != name) continue;
                menu.DropDownItems.Clear();
                menu.DropDownItems.AddRange(mi.Where(x => x.ShowInMenu).SelectMany(x => x.Build()).ToArray());
            }
        }

        public static void Rebuild()
        {
            if (_menu == null || _container == null) return;
            foreach (ToolStripMenuItem mi in _menu.Items)
            {
                mi.DropDownOpening -= DropDownOpening;
            }
            _menu.Items.Clear();
            List<ToolStripMenuItem> removeMenu = _menu.Items.OfType<ToolStripMenuItem>().ToList();
            foreach (KeyValuePair<string, List<IMenuBuilder>> kv in MenuItems)
            {
                ToolStripMenuItem mi = removeMenu.FirstOrDefault(x => x.Text == kv.Key) ?? new ToolStripMenuItem(kv.Key);
                mi.DropDownItems.Clear();
                mi.DropDownItems.AddRange(kv.Value.Where(x => x.ShowInMenu).SelectMany(x => x.Build()).ToArray());
                if (mi.DropDownItems.Count <= 0) continue;
                removeMenu.Remove(mi);
                mi.DropDownOpening += DropDownOpening;
                if (!_menu.Items.Contains(mi)) _menu.Items.Add(mi);
            }
            foreach (ToolStripMenuItem rem in removeMenu)
            {
                _menu.Items.Remove(rem);
            }
            // Need to remove and re-add tool strips because the ordering is incorrect otherwise
            List<ToolStrip> removeToolbar = _container.Controls.OfType<ToolStripPanel>()
                .SelectMany(x => x.Controls.OfType<ToolStrip>())
                .Where(control => MenuItems.Any(x => x.Key == control.Name))
                .ToList();
            foreach (KeyValuePair<string, List<IMenuBuilder>> kv in MenuItems.Reverse())
            {
                ToolStrip ts = removeToolbar.FirstOrDefault(x => x.Name == kv.Key) ?? new ToolStrip { Name = kv.Key };
                // TODO Match by name, only remove items that don't match
                ts.Items.Clear();
                ts.Items.AddRange(kv.Value.Where(x => x.ShowInToolStrip).SelectMany(x => x.BuildToolStrip()).ToArray());
                if (ts.Items.Count > 0)
                {
                    if (!removeToolbar.Contains(ts)) _container.TopToolStripPanel.Join(ts);
                    removeToolbar.Remove(ts);
                }
            }
            foreach (ToolStrip control in removeToolbar)
            {
                control.Parent.Controls.Remove(control);
                control.Dispose();
            }
        }

        private static void DropDownOpening(object sender, EventArgs e)
        {
            Mediator.Publish(EditorMediator.UpdateMenu);
        }
    }
}
