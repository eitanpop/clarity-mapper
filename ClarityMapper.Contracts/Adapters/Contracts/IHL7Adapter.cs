namespace ClarityMapper.Contracts.Adapters.Contracts
{
    public interface IHL7Adapter
    {
        IHL7Parser FromV2(string hl7v2);
        public IHL7Parser FromCDA(string cda);
        IHL7Parser FromFhir(string fhir);
    }
}
