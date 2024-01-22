using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Data.Mapping
{
    public class MapperBase<TDomain, TData>
    {
        protected IMapper Mapper { get; private set; }

        public MapperBase()
        {
            Mapper = new MapperConfiguration(x => Configure(x))
                .CreateMapper();
        }

        protected virtual void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<TDomain, TData>()
                .ReverseMap();
        }

        public TDomain Map(TData model) => Mapper.Map<TDomain>(model);
        public TData Map(TDomain model) => Mapper.Map<TData>(model);
    }
}
