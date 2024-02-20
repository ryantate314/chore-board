using ChoreBoard.Data.Models;
using ChoreBoard.Data.Repositories;
using ChoreBoard.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data
{
    public static class Configuration
    {
        public static void Configure(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITaskDefinitionRepo, TaskDefinitionRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IFamilyMemberRepository, FamilyRepository>();

            services.AddDbContext<ChoreBoardContext>(
                options => options.UseSqlServer(config.GetConnectionString("ChoreBoard")));
        }
    }
}
