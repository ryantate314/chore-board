using ChoreBoard.Api.Dto;
using ChoreBoard.Data.Models;
using ChoreBoard.Data.Repositories;
using ChoreBoard.Service;
using ChoreBoard.Service.Exceptions;
using ChoreBoard.Service.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ChoreBoard.Api
{
    // New minimal hosting model: https://learn.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-8.0&tabs=visual-studio
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<ITaskDefinitionService, TaskDefinitionService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IFamilyService, FamilyService>();

            Data.Configuration.Configure(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    await HandleError(app.ApplicationServices, ex, context, env);
                }
            });

            app.UseCors(x =>
            {
                // TODO: Restrict to develop
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task HandleError(IServiceProvider services, Exception ex, HttpContext context, IWebHostEnvironment env)
        {
            var logger = services.GetService<ILogger<Startup>>();

            int statusCode = StatusCodes.Status500InternalServerError;
            ErrorDto body = new ErrorDto();

            if (ex is InvalidOperationException || ex is ArgumentException)
            {
                logger?.LogWarning(ex, $"Invalid operation performing {context.Request.Method} request to {context.Request.Path}");
                statusCode = StatusCodes.Status400BadRequest;
            }
            else if (ex is AlreadyExistsException)
            {
                logger?.LogWarning(ex, $"Entity already exists performing {context.Request.Method} request to {context.Request.Path}");
                statusCode = StatusCodes.Status409Conflict;
            }
            else
            {
                logger?.LogError(ex, $"Error performing {context.Request.Method} request to {context.Request.Path}.");
            }

            var error = new ErrorDto()
            {
                Message = ex.Message,
                StackTrace = env.IsDevelopment() ? ex.StackTrace : null
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
