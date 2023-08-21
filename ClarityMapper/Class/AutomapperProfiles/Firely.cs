using AutoMapper;
using ClarityMapper.DomainObjects.FHIR;
using Hl7.Fhir.Model;

namespace ClarityMapper.Class.AutomapperProfiles
{
    public class FirelyProfile : Profile
    {
        public FirelyProfile()
        {
            CreateMap<IFhirResource, DomainResource>(MemberList.None)
                .Include<ClarityMapper.DomainObjects.FHIR.Patient, Hl7.Fhir.Model.Patient>()
                .Include<ClarityMapper.DomainObjects.FHIR.AllergyIntolerance, Hl7.Fhir.Model.AllergyIntolerance>();


            CreateMap<ClarityMapper.DomainObjects.FHIR.Patient, Hl7.Fhir.Model.Patient>(MemberList.None);
            CreateMap<ClarityMapper.DomainObjects.FHIR.AllergyIntolerance, Hl7.Fhir.Model.AllergyIntolerance>(MemberList.None);

            //  CreateMap<ClarityMapper.DomainObjects.FHIR.Patient, Hl7.Fhir.Model.Patient>();
        }
    }
}
