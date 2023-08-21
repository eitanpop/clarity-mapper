using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.CDA
{
    public interface IFhirResourceStore
    {
        T Get<T>() where T : IFhirResource, new();
        void Save<T>(T item) where T : IFhirResource;
        IList<IFhirResource> GetResources();
    }
}
