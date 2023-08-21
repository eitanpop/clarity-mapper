using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace ClarityAPI.Dtos
{
    public class PatientDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
}
