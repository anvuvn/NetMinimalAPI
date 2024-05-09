using DailyTask.Persistance;
namespace DailyTask;
using DailyTask.Data;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<TaskDb>(opt => opt.UseInMemoryDatabase("TaskDatabase"));

        var app = builder.Build();

        // Minimal APIs code here 
        app.MapPost("/tasks", async (MTask task, TaskDb db) =>
        {                               
            db.MTasks.Add(task);
            await db.SaveChangesAsync(); 

            return Results.Created($"/tasks/{task.Id}", task);
        });

        app.MapGet("/tasks", async (TaskDb db) =>
        await db.MTasks.ToListAsync());

        app.MapGet("/tasks/complete", async (TaskDb db) =>
            await db.MTasks.Where(t => t.IsComplete).ToListAsync());

        app.MapGet("/tasks/{id}", async (int id, TaskDb db) =>
            await db.MTasks.FindAsync(id)
                is MTask task
                    ? Results.Ok(task)
                    : Results.NotFound());
        // Delete
        app.MapDelete("/tasks/{id}", async (int id, TaskDb db) =>
        {
            if (await db.MTasks.FindAsync(id) is MTask task)
            {
                db.MTasks.Remove(task);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        // Minimal APIs code end

        app.Run();
    }
}
