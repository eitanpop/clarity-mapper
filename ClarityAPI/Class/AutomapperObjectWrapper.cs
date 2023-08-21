using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClarityMapper.Contracts;

namespace ClarityAPI.Class
{
    public class AutomapperObjectWrapper : IObjectMapperWrapper
    {
        private readonly IMapper _mapper;

        public AutomapperObjectWrapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TTarget Map<TTarget>(object source)
        {
            return _mapper.Map<TTarget>(source);
        }
    }
}
