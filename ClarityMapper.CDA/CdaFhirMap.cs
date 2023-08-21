using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClarityMapper.DomainObjects.FHIR;
using Hl7.Fhir.Model;
using Patient = ClarityMapper.DomainObjects.FHIR.Patient;


namespace ClarityMapper.CDA
{
    public class CdaFhirMap : ICdaFhirMap
    {
        public IList<CdaFhirMapItem> Generate()
        {
            IList<CdaFhirMapItem> items = new List<CdaFhirMapItem>();

            items.Add(GetMapItemUsingDatastore<Patient>("FirstName", (patient, node) =>
            {
                var name = patient.Name?.FirstOrDefault() ?? new HumanName();
                name.Given = new[] { node.InnerText };
                patient.Name = new List<HumanName> { name };
            }));

            items.Add(GetMapItemUsingDatastore<Patient>("LastName", (patient, node) =>
            {
                var name = patient.Name?.FirstOrDefault() ?? new HumanName();
                name.Family = node.InnerText;
                patient.Name = new List<HumanName> { name };
            }));

            items.Add(GetMapItemUsingDatastore<Patient>("DateOfBirth", (patient, node) =>
                patient.BirthDate = node.FirstChild.FirstChild.InnerText
                ));

            return items;
        }

        private CdaFhirMapItem GetMapItemUsingDatastore<T>(string xmlPropertyName, Action<T, XmlNode> action) where T : IFhirResource, new() => new CdaFhirMapItem
        {
            Name = xmlPropertyName,
            Action = (dataStore, node) =>
            {
                var entity = dataStore.Get<T>();
                action(entity, node);
                dataStore.Save(entity);
            }
        };
    }

    public class CdaFhirMapItem
    {
        public string Name { get; set; }
        public Action<IFhirResourceStore, XmlNode> Action { get; set; }

    }
}
