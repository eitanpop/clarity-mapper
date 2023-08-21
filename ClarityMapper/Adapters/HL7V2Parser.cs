using System;
using System.Collections.Generic;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2;


namespace ClarityMapper.Adapters
{
    public class HL7V2Parser : IHL7Parser
    {
        private readonly HL7v2ToFhirResourcesService _serializer;

        private readonly string _hl7v2;
        public HL7V2Parser(string hl7v2, HL7v2ToFhirResourcesService serializer)
        {
            _hl7v2 = hl7v2;
            _serializer = serializer;
        }

        public IList<IFhirResource> ToFhirResources()
        {
            return _serializer.GetFhirResourcesFromHL7V2(_hl7v2);
        }

    }
}
