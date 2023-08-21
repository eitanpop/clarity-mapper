using System.Collections.Generic;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.Adapters
{
    public class PassThroughParser : IHL7Parser
    {
        private readonly string _input;
        public PassThroughParser(string input)
        {
            _input = input;
        }
        public IList<IFhirResource> ToFhirResources()
        {
            throw new System.NotImplementedException();
        }
    }
}
