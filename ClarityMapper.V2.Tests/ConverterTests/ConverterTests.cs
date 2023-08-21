using System.Linq;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Converters;
using HL7.Dotnetcore;
using Moq;
using NUnit.Framework;

namespace ClarityMapper.V2.Tests.ConverterTests
{
    public class ConverterTests
    {
        [Test]
        public void ConverterPipeline_RunMessageThroughPipelineWithSegments_ReturnsOnlyPatientConverter()
        {
            var pipeline = new ConverterPipeline();
            pipeline.Add(new PatientConverter());
            pipeline.Add(new EncounterConverter());

            var message = TestUtility.GetMessageFromHL7V2(TestUtility.GetHL7FakeData("OnlyPatientHL7.txt"));

            var resources = pipeline.RunMessageThroughPipeline(message);

            Assert.IsTrue(resources.Count == 1);
            Assert.IsTrue(resources[0] is Patient);
        }


        [Test]
        public void ConverterPipeline_RunMessageThroughPipelineWithSegments_ReturnsPatientAndEncounter()
        {
            var pipeline = new ConverterPipeline();
            pipeline.Add(new PatientConverter());
            pipeline.Add(new EncounterConverter());

            var message = TestUtility.GetMessageFromHL7V2(TestUtility.GetHL7FakeData("AllScriptsExample.txt"));

            var resources = pipeline.RunMessageThroughPipeline(message);

            Assert.IsTrue(resources.Count > 1);
            Assert.IsTrue(resources.Any(x => x is Encounter));
        }
    }
}
