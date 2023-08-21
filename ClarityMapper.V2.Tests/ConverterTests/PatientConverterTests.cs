using System;
using System.Linq;
using ClarityMapper.V2.Converters;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using NUnit.Framework;

namespace ClarityMapper.V2.Tests.ConverterTests
{
    public class PatientConverterTests
    {
        private string _validHL7;

        [SetUp]
        public void Setup()
        {
            _validHL7 = TestUtility.GetHL7FakeData("AllScriptsExample.txt");
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesName()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);
            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Name);
            Assert.IsNotEmpty(patient.Name);
            Assert.AreEqual("WASHINGTON EMAJEAN L", patient.Name.First().Given?.First());
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesTelecom()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);
            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Telecom);
            Assert.IsNotEmpty(patient.Telecom);
            Assert.AreEqual("(614)900-8865", patient.Telecom.First().Value);
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesGenderFemale()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);
            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Gender);
            Assert.AreEqual(AdministrativeGender.Female, patient.Gender);
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesGenderMale()
        {
            string hl7 = _validHL7.Replace("|F|", "|M|");
            var message = TestUtility.GetMessageFromHL7V2(hl7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Gender);
            Assert.AreEqual(AdministrativeGender.Male, patient.Gender);
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesBirthdate()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.BirthDate);
            Assert.AreEqual("19631019115959", patient.BirthDate);
        }

        [Test]
        public void PatientConverter_PassValidHL7_ParsesDeceased()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Deceased);
            Assert.IsFalse(((FhirBoolean)patient.Deceased).Value);
        }


        [Test]
        public void PatientConverter_PassValidHL7_ParsesAddress()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Address.First());
            Assert.AreEqual("1233 ARKWOOD AVE  COLUMBUS OH 43227", patient.Address.First().Text);
        }

        [Test]
        public void PatientConverter_PassValidHL7_MaritalStatus()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.MaritalStatus);
            Assert.AreEqual("Single", patient.MaritalStatus.Coding[0].Code);
        }

        [Test]
        public void PatientConverter_PassValidHL7_Contacts()
        {
            var message = TestUtility.GetMessageFromHL7V2(_validHL7);

            var patient = new PatientConverter().ConvertToFhirResource(message);

            Assert.IsNotNull(patient.Contact);
            Assert.AreEqual("WASHINGTON EMMITT(BROTHER/HCPOA) ", patient.Contact.First().Name.Text);


        }
    }
}
