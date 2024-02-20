using AutoMapper;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Mapping
{
    public class TaskDefinitionMapper : MapperBase<Service.Models.TaskDefinition, Data.Models.TaskDefinition>
    {
        protected override void Configure(IMapperConfigurationExpression config)
        {
            ConfigureMapper(config);
        }

        public static void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<Service.Models.TaskDefinition, Data.Models.TaskDefinition>()
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.Uuid, opt => opt.MapFrom(x => x.Id))
                    .ForMember(x => x.TaskSchedules, opt => opt.MapFrom(x => x.Schedules))
                    .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.Category))
                .ReverseMap()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Uuid));

            TaskScheduleMapper.ConfigureMapper(config);
        }
    }
}
