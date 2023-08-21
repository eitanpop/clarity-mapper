using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ClarityMapper.Common;
using ClarityMapper.DomainObjects.FHIR;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;
using Patient = Hl7.Fhir.Model.Patient;

namespace ClarityMapper.V2
{
    public class Extractor<T> where T : IFhirResource, new()
    {
        private readonly Message _message;

        private readonly IList<(Expression<Func<T, Object>>, Func<Message, Object>)> _extractions;

        public Extractor(Message message)
        {
            _message = message;
            _extractions = new List<(Expression<Func<T, Object>>, Func<Message, Object>)>();
        }

        public void AddExtraction(Expression<Func<T, Object>> property, Func<Message, Object> messageFunc)
        {
            _extractions.Add((property, messageFunc));
        }

        public T GetFhirResource()
        {
            var fhirResource = new T();
            foreach (var (property, messageFunc) in _extractions)
            {
                var value = messageFunc(_message);
                ExpressionHelper.SetPropertyValue(fhirResource, property, value);
            }
            return fhirResource;
        }
    }
}
