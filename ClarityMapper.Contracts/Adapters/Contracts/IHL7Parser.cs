using System.Collections.Generic;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.Contracts.Adapters.Contracts
{
    public interface IHL7Parser
    {
        IList<IFhirResource> ToFhirResources();
    }
}
