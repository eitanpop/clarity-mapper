using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Converters
{
    public interface IConverter<out T> where T: IFhirResource
    {
        T ConvertToFhirResource(Message message);

        string[] SegmentsPertainingToThisConverter();
    }
}
