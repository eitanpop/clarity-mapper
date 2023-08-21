using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Encounter = ClarityMapper.DomainObjects.FHIR.Encounter;

namespace ClarityMapper.V2.Converters
{
    public class EncounterConverter : IConverter<Encounter>
    {
        public Encounter ConvertToFhirResource(Message message)
        {
            var extractor = new Extractor<Encounter>(message);

            extractor.AddExtraction(e => e.Identifier, m => FirelyHelper.GetIdentifierList(m.GetValueOrNull("PV1.19")));

            extractor.AddExtraction(e => e.Class, m => new Coding("", m.GetValueOrNull("PV1.2")));

            extractor.AddExtraction(e => e.Type,
                m =>
                    new List<CodeableConcept>
                        {new CodeableConcept("", null, m.GetValueOrNull("PV1.4") ?? m.GetValueOrNull("PV1.18"))});

            extractor.AddExtraction(e => e.ServiceType, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PV1.10")));

            extractor.AddExtraction(e => e.Priority, m => FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PV2.25")));

            extractor.AddExtraction(e => e.Subject, m => FirelyHelper.GetResourceReference(m.GetValueOrNull("PID.3")));

            extractor.AddExtraction(e => e.EpisodeOfCare,
                m =>
                {
                    IList<ResourceReference> references = new List<ResourceReference>();
                    string firstReference = m.GetValueOrNull("PV1.54");
                    string secondReference = m.GetValueOrNull("PV1.53");
                    if (!String.IsNullOrEmpty(firstReference))
                        references.Add(new ResourceReference(firstReference));
                    if (!String.IsNullOrEmpty(secondReference))
                        references.Add(new ResourceReference(secondReference));

                    return references;
                });

            extractor.AddExtraction(e => e.Participant, m =>
            {
                var participant = new Hl7.Fhir.Model.Encounter.ParticipantComponent();
                string participantType = m.GetValueOrNull("ROL.3") ?? m.GetValueOrNull("PRT.4");
                participant.Type = FirelyHelper.GetCodeableConceptList(participantType);
                participant.Individual = FirelyHelper.GetResourceReference(m.GetValueOrNull("ROL.4"));
                return new List<Hl7.Fhir.Model.Encounter.ParticipantComponent> { participant };
            });

            extractor.AddExtraction(e => e.Appointment,
                m => FirelyHelper.GetResourceReference(m.GetValueOrNull("SCH.1") ?? m.GetValueOrNull("SCH.2")));

            extractor.AddExtraction(e => e.Period,
                m =>
                {
                    var begin = m.GetValueOrNull("PV1.44");
                    var end = m.GetValueOrNull("PV1.45");

                    if (String.IsNullOrEmpty(begin) || String.IsNullOrEmpty(end))
                        return null;

                    return new Period(new FhirDateTime(FirelyHelper.GetDate(begin).Value), new FhirDateTime(FirelyHelper.GetDate(end).Value));
                });

            extractor.AddExtraction(e => e.Length, m =>
            {
                string beginString = m.GetValueOrNull("PV1.45");
                string endString = m.GetValueOrNull("PV1.44");

                if (int.TryParse(beginString, out int begin) && int.TryParse(endString, out int end))
                {
                    int duration = end - begin;
                    return new Duration { Unit = duration.ToString() };
                }
                return null;
            });

            extractor.AddExtraction(e => e.ReasonCode, m =>
               FirelyHelper.GetCodeableConceptList($"{m.GetValueOrNull("EVN.4")} {m.GetValueOrNull("PV2.3")}"));

            extractor.AddExtraction(e => e.ReasonReference, m => FirelyHelper.GetResourceReference($"{m.GetValueOrNull("EVN.4")} {m.GetValueOrNull("PV2.3")}"));

            extractor.AddExtraction(e => e.Hospitalization, m =>
              {
                  var hospitalization = new Hl7.Fhir.Model.Encounter.HospitalizationComponent();

                  hospitalization.PreAdmissionIdentifier = FirelyHelper.GetIdentifier(m.GetValueOrNull("PV1.5"));
                  hospitalization.AdmitSource = FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PV1.14"));
                  hospitalization.ReAdmission = FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PV1.13"));
                  hospitalization.DietPreference = FirelyHelper.GetCodeableConceptList(m.GetValueOrNull("PV1.38"));
                  hospitalization.SpecialCourtesy = FirelyHelper.GetCodeableConceptList(m.GetValueOrNull("PV1.16"));
                  hospitalization.SpecialArrangement = FirelyHelper.GetCodeableConceptList(m.GetValueOrNull("PV1.15"));
                  hospitalization.Destination = FirelyHelper.GetResourceReference(m.GetValueOrNull("PV1.37"));
                  hospitalization.DischargeDisposition = FirelyHelper.GetCodeableConcept(m.GetValueOrNull("PV1.36"));

                  return hospitalization;
              });

            extractor.AddExtraction(e => e.Location, m =>
              {
                  var location = new Hl7.Fhir.Model.Encounter.LocationComponent();
                  location.Location = FirelyHelper.GetResourceReference(m.GetValueOrNull("PV1.3"));

                  return new List<Hl7.Fhir.Model.Encounter.LocationComponent> { location };
              });

            return extractor.GetFhirResource();
        }

        public string[] SegmentsPertainingToThisConverter() =>
            new[]
            {
                "PV1.19", "PV1.2", "PV1.4", "PV1.18", "PV.10", "PV2.25", "PV1.54",
                "PV1.53", "ROL.3", "PRT.4", "ROL.4", "SCH.1", "SCH.2", "PV1.44", "PV1.45", "EVN.4", "PV2.3",
                "PV1.5", "PV1.14", "PV1.13", "PV1.38", "PV1.16", "PV1.15", "PV1.37", "PV1.36"
            };
    }
}
