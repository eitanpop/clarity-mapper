using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ClarityMapper.V2.Converters;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Moq;
using NUnit.Framework;

namespace ClarityMapper.V2.Tests
{
    public class HL7v2ToFhirResourcesServiceTests
    {
        // Shouldn't really test 3rd party functionality but wanted to anyways
        [Test]
        public void FhirSerializer_ConvertToSegments_CorrectNumberOfSegments()
        {
            string hl7 = GetHL7FakeData("AllScriptsExample.txt");
            var serializer = new HL7v2ToFhirResourcesService(null);

            var message = serializer.ConvertToMessage(hl7);
            Assert.AreEqual(message.SegmentCount, 11);
        }

        private string GetHL7FakeData(string fileName)
        {
            var path = Path.GetDirectoryName(this.GetType().GetTypeInfo().Assembly.Location) + "/";
            return File.ReadAllText(path + "MockHL7Data/" + fileName);
        }

        [Test]
        public void FhirSerializer_GetFhirResourcesFromMessage_CallsPipeline()
        {
            var mockConverterPipeline = new Mock<IConverterPipeline>();
            var mockMessage = new Mock<Message>();
            var serializer = new HL7v2ToFhirResourcesService(mockConverterPipeline.Object);
            serializer.GetFhirResourcesFromSegments(mockMessage.Object);
            mockConverterPipeline.Verify(x => x.RunMessageThroughPipeline(mockMessage.Object), Times.Once);
        }
    }
}