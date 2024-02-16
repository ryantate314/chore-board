using AutoMapper;
using Ical.Net.DataTypes;

namespace ChoreBoard.Data.Mapping
{
    internal class TaskScheduleMapper : MapperBase<Service.Models.TaskSchedule, Data.Models.TaskSchedule>
    {
        public TaskScheduleMapper()
        {
        }

        protected override void Configure(IMapperConfigurationExpression config)
        {
            ConfigureMapper(config);
        }

        public static void ConfigureMapper(IMapperConfigurationExpression config)
        {
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