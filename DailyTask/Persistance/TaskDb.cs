using DailyTask.Data;
using Microsoft.EntityFrameworkCore;

namespace DailyTask.Persistance
{
    public class TaskDb : DbContext
    {
        public TaskDb(DbContextOptions<TaskDb> options)
        : base(options) { }

        public DbSet<MTask> MTasks => Set<MTask>();

    }
}
