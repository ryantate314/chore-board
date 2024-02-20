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
            config.CreateMap<Service.Models.FamilyMember, Data.Models.FamilyMember>()
                .ForMember(x => x.Uuid, opt => opt.MapFrom(x => x.Id))
                .ReverseMap()
                    .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FirstName));
        }
    }
}
