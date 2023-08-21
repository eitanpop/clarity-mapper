namespace ClarityMapper.Contracts.Adapters.Contracts
{
    public interface IParserFactory
    {
        IHL7Parser GetV2Parser(string hl7);
        IHL7Parser GetCDAParser(string cda);
        IHL7Parser GetFhirParser(string fhir);
    }
}
