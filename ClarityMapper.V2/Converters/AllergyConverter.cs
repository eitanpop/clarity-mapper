using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using AllergyIntolerance = ClarityMapper.DomainObjects.FHIR.AllergyIntolerance;

namespace ClarityMapper.V2.Converters
{
    public class AllergyConverter : IConverter<AllergyIntolerance>
    {
        public AllergyIntolerance ConvertToFhirResource(Message message)
        {
            var extractor = new Extractor<AllergyIntolerance>(message);

            extractor.AddExtraction(a => a.Identifier,
                m => new List<Identifier> { new Identifier("", m.GetValueOrNull("IAM.7")) });
            return extractor.GetFhirResource();
        }

        public string[] SegmentsPertainingToThisConverter() =>
            new[] { "" };
    }
}
