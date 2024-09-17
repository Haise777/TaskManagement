﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Data;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services
{
    public class TaskService
    {
        private readonly MyDbContext _db;
        private readonly UserManager<User> _userManager;

        public TaskService(UserManager<User> userManager, MyDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IEnumerable<ReadableTask>> GetAllAssignedTasksAsync(string userId)
        {
            var userAssignedTasks = await _db.Tasks
                .Where(t => t.UserTasks
                .Any(t => t.UserId == userId))
                .ToListAsync();

            return await TranslateTasksAsync(userAssignedTasks);
        }

        private async Task<IEnumerable<ReadableTask>> TranslateTasksAsync(IEnumerable<MyTask> tasks)
        {
            var translatedTasks = new List<ReadableTask>();

            var userIds = tasks.SelectMany(t => t.UserTasks).Select(t => t.UserId).Distinct().ToList();
            var userNames = await _db.Users.Where(x => userIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.UserName);

            foreach (var task in tasks)
            {
                translatedTasks.Add(new ReadableTask
                {
                    Id = task.Id,
                    Author = userNames.Single(x => x.Key == task.AuthorId),
                    Title = task.Title,
                    Description = task.Description,
                    Created = task.Created,
                    LastUpdated = task.LastUpdated,
                    AssignedUsers = userNames.Where(un => task.UserTasks.Any(ut => ut.UserId == un.Key)).ToDictionary(x => x.Key, x => x.Value)
                });
            }

            return translatedTasks;
        }
    }
}
