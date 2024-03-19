using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Mapping
{
    public class TaskInstanceMapper : MapperBase<Service.Models.TaskInstance, Data.Models.TaskInstance>
    {
        protected override void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<Service.Models.TaskInstance, Data.Models.TaskInstance>()
                    .ForMember(x => x.Id, opt => opt.Ignore())
                    .ForMember(x => x.TaskDefinition, opt => opt.MapFrom(x => x.Definition))
                    // We only have the Uuid available when mapping to the data model
                    .ForMember(x => x.TaskDefinitionId, opt => opt.Ignore())
                    .ForMember(x => x.CompletedById, opt => opt.Ignore());
            // Must use a separate mapper here due to bug in ReverseMap() causing nullable classes to behave like structs.
            config.CreateMap<Data.Models.TaskInstance, Service.Models.TaskInstance>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Uuid))
                    .ForMember(x => x.Definition, opt => opt.MapFrom(x => x.TaskDefinition));
                    //.ForMember(x => x.CompletedBy, opt => opt.Condition(x => x.CompletedBy != null));

            TaskDefinitionMapper.ConfigureMapper(config);
            FamilyMemberMapper.ConfigureMapper(config);
        }
    }
}
