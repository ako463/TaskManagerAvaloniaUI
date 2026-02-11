using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Domain.Exceptions;
using TaskManager.Desktop.Infrastructure;

namespace TaskManager.Desktop.Services;

public class TaskRepository : ITaskRepository, ITaskNamingService
{
    private const string _initialTaskTitle = "Task ";
    private readonly string taskTitlePattern = @$"{_initialTaskTitle}(\d+)";

    private readonly TaskItemContext _context;

    public TaskRepository(TaskItemContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync()
    {
        return await _context.TasksItems.ToListAsync();
    }


    public async Task<IEnumerable<TaskItem>> GetTaskItemsAsync()
    {
        return await _context.TasksItems
            .Where(t => !t.IsDeleted)
            .ToListAsync();
    }

    public async Task<TaskItem> GetByIdAsync(Guid id)
    {
        var taskItem = await _context.TasksItems
                .FirstOrDefaultAsync(t => t.Id == id);

        return taskItem ?? throw new NotFoundException($"Task {id} not found");
    }

    public async Task<TaskItem> InsertAsync(TaskItem taskItem)
    {
        await _context.TasksItems.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> SoftDeleteAsync(TaskItem taskItem)
    {
        var task = await FindNotDeletedById(taskItem.Id);

        if (task == null)
            return false;

        task.MarkAsDelete();

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(TaskItem taskItem)
    {
        var task = await FindNotDeletedById(taskItem.Id);

        if (task == null)
            return false;

        // Обновляем только необходимые поля
        _context.Entry(task).CurrentValues.SetValues(taskItem);

        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<TaskItem?> FindNotDeletedById(Guid id)
    {
        return await _context.TasksItems
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task<string> CreateDefaultTitleAsync()
    {
        var tasks = await GetAllTaskItemsAsync();

        var lastNumber = tasks.Select(t => Regex.Match(t.Title ?? "", taskTitlePattern))
            .Where(m => m.Success)
            .Select(m => Convert.ToInt32(m.Groups[1].Value))
            .LastOrDefault();

        return $"{_initialTaskTitle}{lastNumber + 1}";
    }
}
