using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManager.Desktop.Domain;

namespace TaskManager.Desktop.Infrastructure;

public class TaskNamingService : ITaskNamingService
{
    private const string _initialTaskTitle = "Task ";
    private readonly string taskTitlePattern = @$"{_initialTaskTitle}(\d+)";

    private readonly ITaskRepository _taskRepository;

    public TaskNamingService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<string> CreateDefaultTitleAsync()
    {
        var tasks = await _taskRepository.GetAllTaskItemsAsync();

        var lastNumber = tasks.Select(t => Regex.Match(t.Title ?? "", taskTitlePattern))
            .Where(m => m.Success)
            .Select(m => Convert.ToInt32(m.Groups[1].Value))
            .LastOrDefault();

        return $"{_initialTaskTitle}{lastNumber + 1}";
    }
}
