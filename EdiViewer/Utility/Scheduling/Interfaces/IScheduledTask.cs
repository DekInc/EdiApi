using System.Threading;
using System.Threading.Tasks;

namespace EdiViewer.Utility.Scheduling.Interfaces
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}