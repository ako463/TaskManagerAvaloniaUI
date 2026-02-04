using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationContext _context;

    public TaskRepository(ApplicationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<TaskItem>> GetTaskItems()
    {
        return await _context.TasksItems
            .Where(t => !t.IsDeleted)
            .ToListAsync();
    }

    public async Task<TaskItem> Add(TaskItem taskItem)
    {
        if (taskItem == null)
            throw new ArgumentNullException(nameof(taskItem));

        await _context.TasksItems.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> SoftDelete(TaskItem taskItem)
    {
        var task = await _context.TasksItems
            .FirstOrDefaultAsync(t => t.Id == taskItem.Id && !t.IsDeleted);

        if (task == null)
            return false;

        task.IsDeleted = true;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(TaskItem taskItem)
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
