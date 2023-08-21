using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.CDA
{
    public class FhirResourceStore : IFhirResourceStore
    {
        private IList<IFhirResource> _fhirResources = new List<IFhirResource>();
        public T Get<T>() where T : IFhirResource, new() =>
            (T)(_fhirResources.FirstOrDefault(x => x is T) ?? new T());

        public void Save<T>(T item) where T : IFhirResource
        {
            _fhirResources = _fhirResources.Where(x => !(x is T)).ToList();
            _fhirResources.Add(item);
        }

        public IList<IFhirResource> GetResources() =>
            _fhirResources;
    }
}
