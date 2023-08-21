using System;
using System.Collections.Generic;
using ClarityMapper.CDA;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.Adapters
{
    public class CDAParser : IHL7Parser
    {
        private readonly string _cda;
        private readonly ICdaReader _cdaReader;
        public CDAParser(string cda, ICdaReader cdaReader) 
        {
            _cda = cda;
            _cdaReader = cdaReader;
        }
        public IList<IFhirResource> ToFhirResources()
        {
            return _cdaReader.Read(_cda);
        }
    }
}
