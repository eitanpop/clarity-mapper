using System;
using System.Collections.Generic;
using ClarityMapper.Adapters;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2;
using Moq;
using NUnit.Framework;

namespace ClarityMapper.Tests
{
    public class HL7AdapterTests
    {

        [Test]
        public void ParserFactoryImplementation_GetV2Parser_ReturnsV2Parser()
        {
            IParserFactory factory = new ParserFactory(null, null);
            var v2Parser = factory.GetV2Parser(string.Empty);

            Assert.IsInstanceOf(typeof(HL7V2Parser), v2Parser);
        }

        [Test]
        public void ParserFactoryImplementation_GetCDAParser_ReturnsCDAParser()
        {
            IParserFactory factory = new ParserFactory(null, null);
            var cdaParser = factory.GetCDAParser(string.Empty);

            Assert.IsInstanceOf(typeof(CDAParser), cdaParser);
        }

        [Test]
        public void ParserFactoryImplementation_GetFhirParser_ReturnsPassThroughParser()
        {
            IParserFactory factory = new ParserFactory(null, null);
            var passthroughParser = factory.GetFhirParser(string.Empty);

            Assert.IsInstanceOf(typeof(PassThroughParser), passthroughParser);
        }


        [Test]
        public void FromHL7V2_PassHL7v2_ConvertsToMockedFHIR()
        {
            string hl7v2 = @"MSH|^~\&";
            var mockFhirReturnData = new List<IFhirResource>{ new Patient() };
            var hl7ParserMock = new Mock<IHL7Parser>();
            hl7ParserMock.Setup(x => x.ToFhirResources()).Returns(() =>  mockFhirReturnData );

            var hl7ParserFactoryMock = new Mock<IParserFactory>();
            hl7ParserFactoryMock.Setup(x => x.GetV2Parser(hl7v2)).Returns(() => hl7ParserMock.Object);

            var fhir = new HL7Adapter(hl7ParserFactoryMock.Object)
                .FromV2(hl7v2)
                .ToFhirResources();

            Assert.AreSame(mockFhirReturnData, fhir);
        }

        [Test]
        public void FromCDA_PassCDA_ConvertsToMockedFHIR()
        {
            string cda = @"<section>";
            var mockFhirReturnData = new List<IFhirResource> { new Patient() };
            var cdaParserMock = new Mock<IHL7Parser>();
            cdaParserMock.Setup(x => x.ToFhirResources()).Returns(() => mockFhirReturnData);

            var parserFactoryMock = new Mock<IParserFactory>();
            parserFactoryMock.Setup(x => x.GetCDAParser(cda)).Returns(() => cdaParserMock.Object);

            var fhir = new HL7Adapter(parserFactoryMock.Object)
                .FromCDA(cda)
                .ToFhirResources();

            Assert.AreSame(mockFhirReturnData, fhir);
        }

        [Test]
        public void FromFhir_PassFhir_ConvertsToMockedFhir()
        {
            string fhirData = "test fhir";
            var mockFhirReturnData = new List<IFhirResource> { new Patient() };
            var fhirParser = new Mock<IHL7Parser>();
            fhirParser.Setup(x => x.ToFhirResources()).Returns(() => mockFhirReturnData);

            var parserFactoryMock = new Mock<IParserFactory>();
            parserFactoryMock.Setup(x => x.GetFhirParser(fhirData)).Returns(() => fhirParser.Object);

            var fhir = new HL7Adapter(parserFactoryMock.Object)
                .FromFhir(fhirData)
                .ToFhirResources();

            Assert.AreSame(mockFhirReturnData, fhir);
        }

    }
}
