using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public class TaskRepository : ITaskRepository
{
    public async Task<IEnumerable<TaskItem>> GetTaskItems()
    {
        using ApplicationContext db = new ApplicationContext();
        
        var tasks = db.TasksItems.ToList();
 
        return await Task.FromResult(tasks);
    }

    public async Task<bool> Add(TaskItem taskItem)
    {
        using ApplicationContext db = new ApplicationContext();

        db.TasksItems.Add(taskItem);
        db.SaveChanges();

        return await Task.FromResult(true);
    }

    public async Task<bool> SoftDelete(TaskItem taskItem)
    {
        using ApplicationContext db = new ApplicationContext();

        taskItem.IsDeleted = true;

        db.TasksItems.Update(taskItem);
        db.SaveChanges();

        return await Task.FromResult(true);
    }

    public async Task<bool> Update(TaskItem taskItem)
    {
        using ApplicationContext db = new ApplicationContext();

        db.TasksItems.Update(taskItem);
        db.SaveChanges();

        return await Task.FromResult(true);
    }
}
