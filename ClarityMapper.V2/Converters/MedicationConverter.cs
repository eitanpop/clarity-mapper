using System;
using System.Collections.Generic;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Medication = ClarityMapper.DomainObjects.FHIR.Medication;

namespace ClarityMapper.V2.Converters
{
    public class MedicationConverter : IConverter<Medication>
    {
        public Medication ConvertToFhirResource(Message message)
        {
            var extractor = new Extractor<Medication>(message);

            extractor.AddExtraction(c => c.Code, m => FirelyHelper.GetCodeableConcept(FirelyHelper.GetDelimitedHL7Values(new[]
            { "RXO.1.1", "RXO.2.1", "RXD.2.1", "RXG.4.1", "RXA.5.1", "RXC.2.1"}, m)));

            extractor.AddExtraction(c => c.Form, m => FirelyHelper.GetCodeableConcept(FirelyHelper.GetDelimitedHL7Values(new[]
                { "RXO.5", "RXE.6", "RXD.6", "RXG.8", "RXA.8"}, m)));

            extractor.AddExtraction(c => c.Ingredient, m =>
            {
                var ingredientComponent = new Hl7.Fhir.Model.Medication.IngredientComponent();
                ingredientComponent.Item = new FhirString(FirelyHelper.GetDelimitedHL7Values(new[] { "RXC.2", "RXO.1", "RXE.2", "RXD.2", "RXG.4", "RXA.5" }, m));
                return ingredientComponent;
            });

            extractor.AddExtraction(c => c.Batch, m =>
              {
                  var batchComponent = new Hl7.Fhir.Model.Medication.BatchComponent();
                  batchComponent.LotNumber = FirelyHelper.GetDelimitedHL7Values(new[] { "RXA.15", "RXG.19" }, m);
                  batchComponent.ExpirationDate = FirelyHelper.GetDelimitedHL7Values(new[] { "RXA.16", "RXG.20" }, m);

                  return batchComponent;
              });

            return extractor.GetFhirResource();
        }

        public string[] SegmentsPertainingToThisConverter()
        {
            return new[] { "RXO.1.1", "RXO.2.1", "RXD.2.1", "RXG.4.1", "RXA.5.1", "RXC.2.1", "RXO.5", "RXE.6", "RXD.6", "RXG.8", "RXA.8", "RXC.2", "RXO.1", "RXA.15", "RXG.19", "RXA.16", "RXG.20" };
        }
    }
}
