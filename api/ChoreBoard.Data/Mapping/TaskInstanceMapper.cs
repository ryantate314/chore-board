using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Mapping
{
    internal class TaskInstanceMapper : MapperBase<Service.Models.TaskInstance, Data.Models.TaskInstance>
    {
        protected override void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<Service.Models.TaskInstance, Data.Models.TaskInstance>()
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.TaskDefinition, opt => opt.MapFrom(x => x.Definition))
                    // We only have the Uuid available when mapping to the data model
                    .ForMember(x => x.TaskDefinitionId, opt => opt.Ignore())
                .ReverseMap()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Uuid))
                    .ForMember(x => x.Definition, opt => opt.MapFrom(x => x.TaskDefinition));

            TaskDefinitionMapper.ConfigureMapper(config);
        }
    }
}
