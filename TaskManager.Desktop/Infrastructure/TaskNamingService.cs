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
        var tasks = await _taskRepository.GetTaskItemsAsync();

        var lastDefaultTitle = tasks.Where(t => Regex.IsMatch(t.Title ?? "", taskTitlePattern))
            .Select(t => t.Title)
            .Order()
            .LastOrDefault();

        if (lastDefaultTitle != null)
        {
            var match = Regex.Match(lastDefaultTitle, taskTitlePattern);
            if (match.Success)
            {
                int nextTitleId = Convert.ToInt32(match.Groups[1].Value) + 1;
                return $"{_initialTaskTitle}{nextTitleId}";
            }
        }
        
        return $"{_initialTaskTitle}1";
    }
}
