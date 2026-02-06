using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Domain.Exceptions;
using TaskManager.Desktop.Infrastructure;

namespace TaskManager.Desktop.Services;

public class TaskRepository : ITaskRepository
{
    private readonly TaskItemContext _context;

    public TaskRepository(TaskItemContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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

        return taskItem ?? throw new NotFoundException($"Task with id {id} not found");
    }

    public async Task<TaskItem> InsertAsync(TaskItem taskItem)
    {
        if (taskItem == null)
            throw new ArgumentNullException(nameof(taskItem));

        await _context.TasksItems.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> SoftDeleteAsync(TaskItem taskItem)
    {
        var task = await _context.TasksItems
            .FirstOrDefaultAsync(t => t.Id == taskItem.Id && !t.IsDeleted);

        if (task == null)
            return false;

        task.MarkAsDelete();

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(TaskItem taskItem)
    {
        if (taskItem == null)
            throw new ArgumentNullException(nameof(taskItem));

        var existingTask = await _context.TasksItems
            .FirstOrDefaultAsync(t => t.Id == taskItem.Id && !t.IsDeleted);

        if (existingTask == null)
            return false;

        // Обновляем только необходимые поля
        _context.Entry(existingTask).CurrentValues.SetValues(taskItem);

        return await _context.SaveChangesAsync() > 0;
    }
}
