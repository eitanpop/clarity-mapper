using ClarityMapper.Contracts.Adapters.Contracts;

namespace ClarityMapper.Adapters
{
    /* The adapter exposes a "From" method for all input formats and then uses the factory to produce a parser to return the requested output data.
       Adapters provide an input for external calls and parser provided the output to the requested format. You know....just adapter type stuff.
    */
    public class HL7Adapter : IHL7Adapter
    {
        private readonly IParserFactory _parserFactory;

        public HL7Adapter(IParserFactory parserFactory)
        {
            _parserFactory = parserFactory;
        }
        public IHL7Parser FromV2(string hl7v2)
        {
            return _parserFactory.GetV2Parser(hl7v2);
        }

        public IHL7Parser FromCDA(string cda)
        {
            return _parserFactory.GetCDAParser(cda);
        }

        public IHL7Parser FromFhir(string fhir)
        {
            return _parserFactory.GetFhirParser(fhir);
        }
    }
}
