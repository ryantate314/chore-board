using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Mapping
{
    public class FamilyMemberMapper : MapperBase<Service.Models.FamilyMember, Data.Models.FamilyMember>
    {
        protected override void Configure(IMapperConfigurationExpression config)
        {
            ConfigureMapper(config);
        }

        internal static void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<Service.Models.FamilyMember, Data.Models.FamilyMember>()
                    .ForMember(x => x.Uuid, opt => opt.MapFrom(x => x.Id))
                    .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap()
                    .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FirstName))
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Uuid));
        }
    }
}
