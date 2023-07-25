using System.Linq;

namespace CBRE.Common.Shell.Commands
{
    /// <summary>
    /// Common command extension methods
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Get the ID of a command. This can be overridden by a <see cref="CommandIDAttribute"/>.
        /// </summary>
        /// <param name="command">The command</param>
        /// <returns>The command's ID</returns>
        public static string GetID(this ICommand command)
        {
            System.Type ty = command.GetType();
            CommandIDAttribute mt = ty.GetCustomAttributes(typeof(CommandIDAttribute), false).OfType<CommandIDAttribute>().FirstOrDefault();
            return mt?.ID ?? ty.FullName;
        }
    }
}