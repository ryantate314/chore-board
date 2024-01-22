using ChoreBoard.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChoreBoard.Data
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskDefinition> TaskDefinitions { get; set; }
        public DbSet<TaskInstance> TaskInstances { get; set; }
        public DbSet<TaskSchedule> TaskSchedules { get; set; }
        public DbSet<Models.TaskStatus> TaskStatuses { get; set; }

        private readonly string _connectionString;

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.TaskStatus>()
                .HasData(new List<Models.TaskStatus>()
                {
                    new Models.TaskStatus() { Id = 1, Name = "Upcoming" },
                    new Models.TaskStatus() { Id = 2, Name = "ToDo" },
                    new Models.TaskStatus() { Id = 3, Name = "InProgress" },
                    new Models.TaskStatus() { Id = 4, Name = "Done" },
                    new Models.TaskStatus() { Id = 5, Name = "Excluded" }
                });
        }
    }
}
