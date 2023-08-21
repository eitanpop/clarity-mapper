using System;
using System.Collections.Generic;
using System.Text;

namespace ClarityMapper.Contracts
{
    public interface IObjectMapperWrapper
    {
        public TTarget Map<TTarget>(object source);
    }
}
