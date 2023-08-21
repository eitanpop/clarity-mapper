using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.CDA
{
    public interface ICdaReader
    {
        IList<IFhirResource> Read(string xml);
    }
}
