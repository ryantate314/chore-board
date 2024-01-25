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
            config.CreateMap<Service.Models.TaskDefinition, Data.Models.TaskDefinition>()
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.TaskSchedules, opt => opt.MapFrom(x => x.Schedules))
                .ReverseMap()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Uuid));

            config.CreateMap<Service.Models.TaskSchedule, Data.Models.TaskSchedule>()
                    .ForMember(x => x.RRule, opt => opt.MapFrom(x => x.Pattern.ToString()))
                    .ForMember(x => x.TaskDefinitionId, opt => opt.Ignore())
                    .ForMember(x => x.TaskDefinition, opt => opt.MapFrom((source, dest) =>
                    {
                        return new Data.Models.TaskDefinition()
                        {
                            Uuid = source.TaskDefinitionId
                        };
                    }))
                .ReverseMap()
                    .ForMember(x => x.Pattern, opt => opt.MapFrom(x => new RecurrencePattern(x.RRule)))
                    .ForMember(x => x.TaskDefinitionId, opt => opt.MapFrom(y => y.TaskDefinition.Uuid));
        }
    }
}
