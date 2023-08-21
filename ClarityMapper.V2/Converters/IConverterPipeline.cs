using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Converters
{
    public interface IConverterPipeline
    {
        void Add(IConverter<IFhirResource> converter);
        void Remove(IConverter<IFhirResource> converter);
        IList<IFhirResource> RunMessageThroughPipeline(Message message);
    }
}
