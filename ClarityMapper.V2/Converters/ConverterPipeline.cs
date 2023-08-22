using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Converters
{
    public class ConverterPipeline : IConverterPipeline
    {
        private readonly IList<IConverter<IFhirResource>> _converters;

        public ConverterPipeline()
        {
            _converters = new List<IConverter<IFhirResource>>();
        }
        public void Add(IConverter<IFhirResource> converter)
        {
            _converters.Add(converter);
        }

        public void Remove(IConverter<IFhirResource> converter)
        {
            _converters.Remove(converter);
        }

        public IList<IFhirResource> RunMessageThroughPipeline(Message message)
        {
            return _converters
                .Where(x => DoesConverterContainSegmentsInMessage(x, message))
                .Select(converter => converter.ConvertToFhirResource(message))
                .Where(fhirResource => fhirResource != null).ToList();
        }

        private bool DoesConverterContainSegmentsInMessage<T>(IConverter<T> converter, Message m)
            where T : IFhirResource
            => converter.SegmentsPertainingToThisConverter()
                .Any(segmentLocation => m.GetValueOrNull(segmentLocation) != null);
    }
}
