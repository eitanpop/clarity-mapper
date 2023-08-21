using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Coverage = ClarityMapper.DomainObjects.FHIR.Coverage;

namespace ClarityMapper.V2.Converters
{
    public class CoverageConverter : IConverter<Coverage>
    {
        public Coverage ConvertToFhirResource(Message message)
        {
            var groupNumber = message.GetValueOrNull("IN1.8");
            var extractor = new Extractor<Coverage>(message);
            extractor.AddExtraction(c => c.Identifier, m => FirelyHelper.GetIdentifierList(m.GetValueOrNull("IN1.2")));
            extractor.AddExtraction(c => c.Period, m => new Period
            {
                Start = m.GetValueOrNull(m.GetValueOrNull("IN1.12")),
                End = m.GetValueOrNull("IN1.13")
            });
            extractor.AddExtraction(c => c.Payor, m => FirelyHelper.GetResourceReference("IN1.3"));
            extractor.AddExtraction(c => c.Class, m =>
              {
                  var classComponent = new Hl7.Fhir.Model.Coverage.ClassComponent
                  {
                      Value = groupNumber,
                      Name = groupNumber
                  };
                  return classComponent;
              });
            extractor.AddExtraction(c => c.CostToBeneficiary, m =>
              {
                  var ctb = new Hl7.Fhir.Model.Coverage.CostToBeneficiaryComponent
                  {
                      Value = new FhirString(groupNumber)
                  };
                  return new List<Hl7.Fhir.Model.Coverage.CostToBeneficiaryComponent> { ctb };
              });
            return extractor.GetFhirResource();
        }

        public string[] SegmentsPertainingToThisConverter() =>
            new[] { "IN1.2" };
    }
}
