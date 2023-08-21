using System.Collections.Generic;
using ClarityMapper.CDA;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.V2;

namespace ClarityMapper.Adapters
{
    // Factory because the input healthcare data is injected into the constructor at runtime + seam for possible formatting before handing off to the parser
    public class ParserFactory : IParserFactory
    {
        private readonly HL7v2ToFhirResourcesService _serializer;
        private readonly ICdaReader _cdaReader;
        public ParserFactory(HL7v2ToFhirResourcesService serializer, ICdaReader cdaReader)
        {
            _serializer = serializer;
            _cdaReader = cdaReader;
        }
        public IHL7Parser GetV2Parser(string hl7)
        {
            return new HL7V2Parser(hl7, _serializer);
        }

        public IHL7Parser GetCDAParser(string cda)
        {
            return new CDAParser(cda, _cdaReader);
        }

        public IHL7Parser GetFhirParser(string fhir)
        {
            return new PassThroughParser(fhir);
        }
    }
}
