using ChoreBoard.Data;
using ChoreBoard.Data.Repositories;
using ChoreBoard.Service;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChoreBoard.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<ITaskDefinitionService, TaskDefinitionService>();
            builder.Services.AddTransient<ITaskDefinitionRepo, TaskDefinitionRepository>();
            builder.Services.AddDbContext<TaskContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("ChoreBoard")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}