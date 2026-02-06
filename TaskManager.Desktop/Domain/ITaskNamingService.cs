using System.Threading.Tasks;

namespace TaskManager.Desktop.Domain;

public interface ITaskNamingService
{
    Task<string> CreateDefaultTitleAsync();
}
