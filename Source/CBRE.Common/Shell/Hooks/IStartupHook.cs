using System.Threading.Tasks;

namespace CBRE.Common.Shell.Hooks
{
    /// <summary>
    /// A hook that runs at startup
    /// </summary>
    public interface IStartupHook
    {
        /// <summary>
        /// Runs on startup
        /// </summary>
        /// <returns></returns>
        Task OnStartup();
    }
}
