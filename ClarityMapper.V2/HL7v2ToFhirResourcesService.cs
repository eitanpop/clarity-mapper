using System;
using System.Collections;
using System.Collections.Generic;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Converters;
using HL7.Dotnetcore;

namespace ClarityMapper.V2
{
    public class HL7v2ToFhirResourcesService
    {
        private readonly IConverterPipeline _converterPipeline;
        public HL7v2ToFhirResourcesService(IConverterPipeline converterPipeline)
        {
            _converterPipeline = converterPipeline;
        }

        public Message ConvertToMessage(string hl7v2)
        {
            Message message = new Message(hl7v2);
            message.ParseMessage();
            return message;
        }

        public IList<IFhirResource> GetFhirResourcesFromSegments(Message message) =>
            _converterPipeline.RunMessageThroughPipeline(message);


        public IList<IFhirResource>  GetFhirResourcesFromHL7V2(string hl7v2)
        {
            var message = ConvertToMessage(hl7v2);
            return GetFhirResourcesFromSegments(message);
        }
    }
}
