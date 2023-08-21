using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.Contracts;
using Hl7.Fhir.Model;

namespace ClarityMapper.DomainObjects.FHIR
{
    public class AllergyIntolerance : Hl7.Fhir.Model.AllergyIntolerance, IFhirResource
    {
        public DomainResource GetFirelyObject(IObjectMapperWrapper mapper)
        {
            throw new NotImplementedException();
        }
    }
}
