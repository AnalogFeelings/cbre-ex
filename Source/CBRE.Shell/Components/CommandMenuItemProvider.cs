using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Menu;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Components
{
    [Export(typeof(IMenuItemProvider))]
    public class CommandMenuItemProvider : IMenuItemProvider
    {
        private readonly IEnumerable<Lazy<ICommand>> _commands;
        
        // Store the hotkey register so we know what the hotkey for each command is
        private readonly HotkeyRegister _hotkeys;

        [ImportingConstructor]
        internal CommandMenuItemProvider([ImportMany] IEnumerable<Lazy<ICommand>> commands, [Import] Lazy<HotkeyRegister> hotkeys)
        {
            _commands = commands;
            _hotkeys = hotkeys.Value;
        }

        public event EventHandler MenuItemsChanged;

        public IEnumerable<IMenuItem> GetMenuItems()
        {
            foreach (Lazy<ICommand> export in _commands)
            {
                Type ty = export.Value.GetType();
                MenuItemAttribute mia = ty.GetCustomAttributes(typeof(MenuItemAttribute), false).OfType<MenuItemAttribute>().FirstOrDefault();
                if (mia == null) continue;
                MenuImageAttribute icon = ty.GetCustomAttributes(typeof(MenuImageAttribute), false).OfType<MenuImageAttribute>().FirstOrDefault();

                Common.Shell.Hotkeys.IHotkey hotkey = _hotkeys.GetHotkey("Command:" + export.Value.GetID());
                string shortcut = _hotkeys.GetHotkeyString(hotkey);

                AllowToolbarAttribute allow = ty.GetCustomAttributes(typeof(AllowToolbarAttribute), false).OfType<AllowToolbarAttribute>().FirstOrDefault();

                yield return new CommandMenuItem(export.Value, mia.Section, mia.Path, mia.Group, mia.OrderHint, icon?.Image, shortcut ?? "", allow?.Allowed != false);
            }
        }
    }
}
