using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Converters
{
    public class ConditionConverter : IConverter<Condition>
    {
        public Condition ConvertToFhirResource(Message message)
        {
            var extractor = new Extractor<Condition>(message);

            extractor.AddExtraction(c => c.ClinicalStatus, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PRB.14")));
            extractor.AddExtraction(c => c.VerificationStatus, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PRB.13")));
            extractor.AddExtraction(c => c.Category, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PRB.3")));
            extractor.AddExtraction(c => c.Severity,
                m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PRB.26") + " | " + m.GetValueOrNull("ABS.3")));
            extractor.AddExtraction(c => c.Code, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PRB.3")));
            extractor.AddExtraction(c => c.Subject, m => FirelyHelper.GetResourceReference(m.GetValueOrNull("PID.3")));
            extractor.AddExtraction(c => c.Encounter, m => FirelyHelper.GetResourceReference(m.GetValueOrNull("PV1.19")));
            extractor.AddExtraction(c => c.RecordedDate, m => m.GetValueOrNull("REL.11"));
            extractor.AddExtraction(c => c.Asserter, m => FirelyHelper.GetResourceReference(m.GetValueOrNull("REL.7")));
            extractor.AddExtraction(c => c.Stage, m => new Hl7.Fhir.Model.Condition.StageComponent
            {
                Summary = FirelyHelper.GetCodeableConcept("PRB.14")
            });
            return extractor.GetFhirResource();
        }

        public string[] SegmentsPertainingToThisConverter() =>
            new[] { "PRB.18", "PRB.13", "PRB.26", "PRB.3", "PRB.16", "REL.11", "REL.7", "PRB.14" };
    }
}
