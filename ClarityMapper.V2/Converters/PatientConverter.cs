using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Patient = ClarityMapper.DomainObjects.FHIR.Patient;

namespace ClarityMapper.V2.Converters
{
    public class PatientConverter : IConverter<Patient>
    {
        public Patient ConvertToFhirResource(Message message)
        {
            var extractor = new Extractor<Patient>(message);

            extractor.AddExtraction(p => p.Identifier,
                m => FirelyHelper.GetIdentifierList(m.GetValueOrNull("PID.3")));

            extractor.AddExtraction(p => p.Name,
                m => new List<HumanName>
                {
                        new HumanName().WithGiven(message.GetValueOrNull("PID.5"))
                });

            extractor.AddExtraction(p => p.Telecom, m => new List<ContactPoint>
                {
                    new ContactPoint(ContactPoint.ContactPointSystem.Phone, null, m.GetValue("PID.13"))
                });

            extractor.AddExtraction(p => p.Gender, m =>
            {
                string gender = m.GetValue("PID.8")?.ToUpper();
                return FirelyHelper.GetFhirGender(gender);
            });

            extractor.AddExtraction(p => p.BirthDate, m => m.GetValueOrNull("PID.7"));

            extractor.AddExtraction(m => m.Deceased, m =>
            {

                bool.TryParse(m.GetValueOrNull("PID.30"), out var isDeceased);
                return new FhirBoolean(isDeceased);
            });


            extractor.AddExtraction(p => p.Address, m =>
                {
                    var address = new Address();
                    address.Text = m.GetValueOrNull("PID.11");
                    return new List<Address> { address };
                });

            extractor.AddExtraction(p => p.MaritalStatus, m =>
                {
                    return new CodeableConcept("http://hl7.org/fhir/v2/0131", m.GetValueOrNull("PID.16"));
                });

            extractor.AddExtraction(p => p.Contact, m =>
              {
                  var contact = new Hl7.Fhir.Model.Patient.ContactComponent();
                  string relationship = m.GetValueOrNull("NK1.7") ?? m.GetValueOrNull("NK1.3");
                  string name = m.GetValueOrNull("NK1.2");
                  string telecom = m.GetValueOrNull("NK1.5") ?? m.GetValueOrNull("NK1.6") ?? m.GetValueOrNull("NK1.40");
                  string address = m.GetValueOrNull("NK1.4");
                  string gender = m.GetValueOrNull("NK1.15");


                  if (string.IsNullOrEmpty(relationship) && string.IsNullOrEmpty(name) &&
                      string.IsNullOrEmpty(telecom) && string.IsNullOrEmpty(address) &&
                      string.IsNullOrEmpty(gender))
                      return null;

                  contact.Relationship.Add(new CodeableConcept("http://hl7.org/fhir/v2/0131", relationship));
                  contact.Name = new HumanName { Text = name };
                  contact.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, null, telecom));
                  contact.Address = new Address { Text = address };
                  contact.Gender = FirelyHelper.GetFhirGender(gender);
                  return new List<Hl7.Fhir.Model.Patient.ContactComponent> { contact };
              });
            return extractor.GetFhirResource();

        }

        public string[] SegmentsPertainingToThisConverter() =>
            new[] { "PID.3", "PID.5", "PID.13", "PID.8", "PID.7", "PID.30", "PID.11", "PID.16", "PID.3",
                "NK1.7", "NK1.3", "NK1.2", "NK1.5", "NK1.40", "NK1.4", "NK1.15", "NK1.6" };
    }
}
