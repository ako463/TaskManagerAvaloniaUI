using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public class TaskService : ITaskService
{
    public async Task<IEnumerable<TaskModel>> GetTasks()
    {
        return await Task.FromResult(
            new TaskModel[]
            {
                new TaskModel()
                {
                    Id = 1,
                    Title = "Задача А",
                    CreatedAt = DateTime.Now,
                    IsCompleted = false,
                    IsDeleted = false,
                },
                new TaskModel()
                {
                    Id = 2,
                    Title = "Задача Б",
                    CreatedAt = DateTime.Now,
                    IsCompleted = true,
                    IsDeleted = false,
                },
                new TaskModel()
                {
                    Id = 3,
                    Title = "Задача В",
                    CreatedAt = DateTime.Now,
                    IsCompleted = false,
                    IsDeleted = true,
                },
                new TaskModel()
                {
                    Id = 4,
                    Title = "Задача Г",
                    CreatedAt = DateTime.Now,
                    IsCompleted = false,
                    IsDeleted = false,
                }
            });
    }
}
