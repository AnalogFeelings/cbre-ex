using CBRE.Localization;
using CBRE.Settings.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Settings
{
    public static class Hotkeys
    {
        private static readonly List<HotkeyDefinition> Definitions;
        private static readonly List<HotkeyImplementation> Implementations;

        static Hotkeys()
        {
            Definitions = new List<HotkeyDefinition>
                               {
                                    new HotkeyDefinition(Local.LocalString("hotkey.autosize_views"), Local.LocalString("hotkey.autosize_views.description"), HotkeysMediator.ViewportAutosize, ""),
                                    new HotkeyDefinition(Local.LocalString("hotkey.focus_view_top_left"), Local.LocalString("hotkey.focus_view_top_left.description"), HotkeysMediator.FourViewFocusTopLeft, "F5"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.focus_view_top_right"), Local.LocalString("hotkey.focus_view_top_right.description"), HotkeysMediator.FourViewFocusTopRight, "F2"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.focus_view_bottom_left"), Local.LocalString("hotkey.focus_view_bottom_left.description"), HotkeysMediator.FourViewFocusBottomLeft, "F4"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.focus_view_bottom_right"), Local.LocalString("hotkey.focus_view_bottom_right.description"), HotkeysMediator.FourViewFocusBottomRight, "F3"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.focus_current_view"), Local.LocalString("hotkey.focus_current_view.description"), HotkeysMediator.FourViewFocusCurrent, "Shift+Z"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.new_file"), Local.LocalString("hotkey.new_file.description"), HotkeysMediator.FileNew, "Ctrl+N"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.open_file"), Local.LocalString("hotkey.open_file.description"), HotkeysMediator.FileOpen, "Ctrl+O"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.save_file"), Local.LocalString("hotkey.save_file.description"), HotkeysMediator.FileSave, "Ctrl+S"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.save_file_as"), Local.LocalString("hotkey.save_file.description"), HotkeysMediator.FileSaveAs, "Ctrl+Alt+S"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.export_map"), Local.LocalString("hotkey.export_map.description"), HotkeysMediator.FileCompile, "F9"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.increase_grid_size"), Local.LocalString("hotkey.increase_grid_size.description"), HotkeysMediator.GridIncrease, "]"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.decrease_grid_size"), Local.LocalString("hotkey.decrease_grid_size.description"), HotkeysMediator.GridDecrease, "["),
                                    new HotkeyDefinition(Local.LocalString("hotkey.toggle_2d_grid"), Local.LocalString("hotkey.toggle_2d_grid.description"), HotkeysMediator.ToggleShow2DGrid, "Shift+R"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.toggle_3d_grid"), Local.LocalString("hotkey.toggle_3d_grid.description"), HotkeysMediator.ToggleShow3DGrid, "P"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.toggle_snap_to_grid"), Local.LocalString("hotkey.toggle_snap_to_grid.description"), HotkeysMediator.ToggleSnapToGrid, "Shift+W"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.snap_selection_to_grid"), Local.LocalString("hotkey.snap_selection_to_grid.description"), HotkeysMediator.SnapSelectionToGrid, "Ctrl+B"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.snap_selection_to_grid_individually"), Local.LocalString("hotkey.snap_selection_to_grid_individually.description"), HotkeysMediator.SnapSelectionToGridIndividually, "Ctrl+Shift+B"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.undo"), Local.LocalString("hotkey.undo.description"), HotkeysMediator.HistoryUndo, "Ctrl+Z"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.redo"), Local.LocalString("hotkey.redo.description"), HotkeysMediator.HistoryRedo, "Ctrl+Y"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.show_object_properties"), Local.LocalString("hotkey.show_object_properties.description"), HotkeysMediator.ObjectProperties, "Alt+Enter"),

                                    new HotkeyDefinition(Local.LocalString("copy"), Local.LocalString("hotkey.copy.description"), HotkeysMediator.OperationsCopy, "Ctrl+C", "Ctrl+Ins"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.cut"), Local.LocalString("hotkey.cut.description"), HotkeysMediator.OperationsCut, "Ctrl+X", "Shift+Del"),
                                    new HotkeyDefinition(Local.LocalString("paste"), Local.LocalString("hotkey.paste.description"), HotkeysMediator.OperationsPaste, "Ctrl+V", "Shift+Ins"),
                                    new HotkeyDefinition(Local.LocalString("paste_special"), Local.LocalString("hotkey.paste_special.description"), HotkeysMediator.OperationsPasteSpecial, "Ctrl+Shift+V"),
                                    new HotkeyDefinition(Local.LocalString("delete"), Local.LocalString("hotkey.delete.description"), HotkeysMediator.OperationsDelete, "Del"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.group"), Local.LocalString("hotkey.group.description"), HotkeysMediator.GroupingGroup, "Ctrl+G"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.ungroup"), Local.LocalString("hotkey.ungroup.description"), HotkeysMediator.GroupingUngroup, "Ctrl+U"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.toggle_ignore_grouping"), Local.LocalString("hotkey.toggle_ignore_grouping.description"), HotkeysMediator.ToggleIgnoreGrouping, "Ctrl+W"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.hide_selected"), Local.LocalString("hotkey.hide_selected.description"), HotkeysMediator.QuickHideSelected, "H"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.hide_unselected"), Local.LocalString("hotkey.hide_unselected.description"), HotkeysMediator.QuickHideUnselected, "Ctrl+H"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.unhide_all"), Local.LocalString("hotkey.unhide_all.description"), HotkeysMediator.QuickHideShowAll, "U"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.rotate_selection_clockwise"), Local.LocalString("hotkey.rotate_selection_clockwise.description"), HotkeysMediator.RotateClockwise, "N"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.rotate_selection_counter_clockwise"), Local.LocalString("hotkey.rotate_selection_counter_clockwise.description"), HotkeysMediator.RotateCounterClockwise, "M"),

                                    new HotkeyDefinition(Local.LocalString("tool.create_new_visgroup"), Local.LocalString("hotkey.create_new_visgroup.description"), HotkeysMediator.VisgroupCreateNew, "Alt+V"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.center_2d_view_on_selection"), Local.LocalString("hotkey.center_2d_view_on_selection.description"), HotkeysMediator.Center2DViewsOnSelection, "Ctrl+E"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.center_3d_view_on_selection"), Local.LocalString("hotkey.center_3d_view_on_selection.description"), HotkeysMediator.Center3DViewsOnSelection, "Ctrl+Shift+E"),

                                    new HotkeyDefinition(Local.LocalString("document.select_all"), Local.LocalString("hotkey.select_all.description"), HotkeysMediator.SelectAll, "Ctrl+A"),
                                    new HotkeyDefinition(Local.LocalString("deselect_all"), Local.LocalString("hotkey.deselect_all.description"), HotkeysMediator.SelectionClear, "Shift+Q", "Escape"),

                                    new HotkeyDefinition(Local.LocalString("document.tie_to_entity"), Local.LocalString("hotkey.tie_to_entity.description"), HotkeysMediator.TieToEntity, "Ctrl+T"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.move_to_world"), Local.LocalString("hotkey.move_to_world.description"), HotkeysMediator.TieToWorld, "Ctrl+Shift+W"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.carve"), Local.LocalString("hotkey.carve.description"), HotkeysMediator.Carve, "Ctrl+Shift+C"),
                                    new HotkeyDefinition(Local.LocalString("brush.make_hollow"), Local.LocalString("hotkey.make_hollow.description"), HotkeysMediator.MakeHollow, "Ctrl+Shift+H"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.toggle_texture_lock"), Local.LocalString("hotkey.toggle_texture_lock.description"), HotkeysMediator.ToggleTextureLock, "Shift+L"),
                                    new HotkeyDefinition(Local.LocalString("transform"), Local.LocalString("hotkey.transform.description"), HotkeysMediator.Transform, "Ctrl+M"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.go_to_brush_id"), Local.LocalString("hotkey.go_to_brush_id.description"), HotkeysMediator.GoToBrushID, "Ctrl+Shift+G"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.flip_x"), Local.LocalString("hotkey.flip_x.description"), HotkeysMediator.FlipX, "Ctrl+L"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.flip_y"), Local.LocalString("hotkey.flip_y.description"), HotkeysMediator.FlipY, "Ctrl+I"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.flip_z"), Local.LocalString("hotkey.flip_z.description"), HotkeysMediator.FlipZ, "Ctrl+K"),

                                    new HotkeyDefinition(Local.LocalString("tool.select"), Local.LocalString("hotkey.selection_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Selection, "Shift+S"),
                                    new HotkeyDefinition(Local.LocalString("tool.camera"), Local.LocalString("hotkey.camera_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Camera, "Shift+C"),
                                    new HotkeyDefinition(Local.LocalString("tool.entity"), Local.LocalString("hotkey.entity_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Entity, "Shift+E"),
                                    new HotkeyDefinition(Local.LocalString("tool.brush"), Local.LocalString("hotkey.brush_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Brush, "Shift+B"),
                                    new HotkeyDefinition(Local.LocalString("tool.texture.tool"), Local.LocalString("hotkey.texture_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Texture, "Shift+A"),
                                    new HotkeyDefinition(Local.LocalString("tool.clip"), Local.LocalString("hotkey.clip_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.Clip, "Shift+X"),
                                    new HotkeyDefinition(Local.LocalString("tool.vertex_manipulation"), Local.LocalString("hotkey.vertex_manipulation_tool.description"), HotkeysMediator.SwitchTool, HotkeyTool.VM, "Shift+V"),

                                    new HotkeyDefinition(Local.LocalString("document.apply_texture"), Local.LocalString("hotkey.apply_texture.description"), HotkeysMediator.ApplyCurrentTextureToSelection, "Shift+T"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.vertex_manipulation.standard"), Local.LocalString("hotkey.vertex_manipulation.standard.description"), HotkeysMediator.VMStandardMode, "Alt+W"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.vertex_manipulation.scaling"), Local.LocalString("hotkey.vertex_manipulation.scaling.description"), HotkeysMediator.VMScalingMode, "Alt+E"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.vertex_manipulation.face_edit"), Local.LocalString("hotkey.vertex_manipulation.face_edit.description"), HotkeysMediator.VMFaceEditMode, "Alt+R"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.vertex_manipulation.split_face"), Local.LocalString("hotkey.vertex_manipulation.split_face.description"), HotkeysMediator.VMSplitFace, "Ctrl+F"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.next_camera"), Local.LocalString("hotkey.next_camera.description"), HotkeysMediator.CameraNext, "Tab", "PgDn"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.previous_camera"), Local.LocalString("hotkey.previous_camera.description"), HotkeysMediator.CameraPrevious, "PgUp"),

                                    new HotkeyDefinition(Local.LocalString("hotkey.previous_tab"), Local.LocalString("hotkey.previous_tab.description"), HotkeysMediator.PreviousTab, "Ctrl+Shift+Tab"),
                                    new HotkeyDefinition(Local.LocalString("hotkey.next_tab"), Local.LocalString("hotkey.next_tab.description"), HotkeysMediator.NextTab, "Ctrl+Tab"),
                               };
            Implementations = new List<HotkeyImplementation>();
            SetupHotkeys(new List<Hotkey>());
        }

        public static void SetupHotkeys(List<Hotkey> overrides)
        {
            Implementations.Clear();
            foreach (HotkeyDefinition def in Definitions)
            {
                bool overridden = false;
                foreach (Hotkey hk in overrides.Where(x => x.ID == def.ID).ToList())
                {
                    overridden = true;
                    if (!String.IsNullOrWhiteSpace(hk.HotkeyString))
                    {
                        Implementations.Add(new HotkeyImplementation(def, hk.HotkeyString));
                    }
                }
                if (!overridden)
                {
                    foreach (string hk in def.DefaultHotkeys)
                    {
                        Implementations.Add(new HotkeyImplementation(def, hk));
                    }
                }
            }
        }

        public static IEnumerable<Hotkey> GetHotkeys()
        {
            foreach (HotkeyDefinition def in Definitions)
            {
                List<HotkeyImplementation> impls = Implementations.Where(x => x.Definition.ID == def.ID).ToList();
                if (!impls.Any())
                {
                    yield return new Hotkey { ID = def.ID, HotkeyString = "" };
                }
                else
                {
                    foreach (HotkeyImplementation impl in impls)
                    {
                        yield return new Hotkey { ID = def.ID, HotkeyString = impl.Hotkey };
                    }
                }
            }
        }

        public static HotkeyImplementation GetHotkeyForMessage(object message, object parameter)
        {
            HotkeyDefinition def = Definitions.FirstOrDefault(x => x.Action.ToString() == message.ToString() && Equals(x.Parameter, parameter));
            if (def == null) return null;
            return Implementations.FirstOrDefault(x => x.Definition.ID == def.ID);
        }

        public static HotkeyImplementation GetHotkeyFor(string keyCombination)
        {
            return Implementations.FirstOrDefault(x => x.Hotkey == keyCombination);
        }

        public static HotkeyDefinition GetHotkeyDefinition(string id)
        {
            return Definitions.FirstOrDefault(x => x.ID == id);
        }

        public static IEnumerable<HotkeyDefinition> GetHotkeyDefinitions()
        {
            return Definitions;
        }
    }
}
