using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClarityMapper.V2.Extensions;
using HL7.Dotnetcore;
using Hl7.Fhir.Model;

namespace ClarityMapper.V2
{
    public static class FirelyHelper
    {
        private static string system = "";

        public static CodeableConcept GetCodeableConcept(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new CodeableConcept(system, null, value);
        }

        public static List<CodeableConcept> GetCodeableConceptList(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new List<CodeableConcept> { GetCodeableConcept(value) };
        }

        public static Identifier GetIdentifier(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new Identifier(system, value);
        }

        public static IList<Identifier> GetIdentifierList(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return null;
            return new List<Identifier> { GetIdentifier(identifier) };
        }

        public static ResourceReference GetResourceReference(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new ResourceReference(value);
        }

        public static IList<ResourceReference> GetResourceReferenceList(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new List<ResourceReference> { new ResourceReference(value) };
        }

        public static AdministrativeGender GetFhirGender(string gender)
        {
            switch (gender)
            {
                case "F":
                    return AdministrativeGender.Female;
                case "M":
                    return AdministrativeGender.Male;
                case "O":
                    return AdministrativeGender.Other;
                default:
                    return AdministrativeGender.Unknown;
            }
        }

        public static DateTime? GetDate(string hl7DateString)
        {
            if (String.IsNullOrEmpty(hl7DateString))
                return null;
            string year = hl7DateString.Substring(0, 4);
            string month = hl7DateString.Substring(4, 2);
            string day = hl7DateString.Substring(6, 2);

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        }

        public static string GetDelimitedHL7Values(string[] values, Message message)
        {
            string valueString = values.Aggregate(string.Empty, (current, value) => current + $"{message.GetValueOrNull(value)} | ");

            return valueString.Substring(0, valueString.Length - 2);
        }
    }
}
