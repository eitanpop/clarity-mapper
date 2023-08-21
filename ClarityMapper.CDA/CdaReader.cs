using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ClarityMapper.DomainObjects.FHIR;

namespace ClarityMapper.CDA
{
    public class CdaReader : ICdaReader
    {
        private readonly ICdaFhirMap _map;
        private readonly IFhirResourceStore _store;
        public CdaReader(ICdaFhirMap map, IFhirResourceStore store)
        {
            _map = map;
            _store = store;
        }
        public IList<IFhirResource> Read(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList resourceList = doc.ChildNodes[1].ChildNodes[0].ChildNodes;
            var mapItems = _map.Generate();
            foreach (XmlNode node in resourceList)
            {
                var mapItem = mapItems.FirstOrDefault(x => x.Name == node.Name);
                mapItem?.Action(_store, node);
            }
            return _store.GetResources();
        }
    }
}
