using System.Threading.Tasks;

namespace CBRE.Common.Shell.Hooks
{
    /// <summary>
    /// A hook that runs when shutting down
    /// </summary>
    public interface IShutdownHook
    {
        /// <summary>
        /// Runs on shutdown
        /// </summary>
        /// <returns></returns>
        Task OnShutdown();
    }
}