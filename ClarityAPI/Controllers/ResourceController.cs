using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ClarityAPI.Class;
using ClarityAPI.Dtos;
using ClarityMapper.DomainObjects.FHIR;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;

namespace ClarityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController
    {
        private readonly ILogger<ResourceController> _logger;
        private readonly IOptions<AppSettingsOptions> _settings;

        public ResourceController(ILogger<ResourceController> logger, IOptions<AppSettingsOptions> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet]
        [Route("patient")]
        public async Task<HttpResponseMessage> Get()
        {
            return new HttpResponseMessage{StatusCode = HttpStatusCode.OK};
        }

        [HttpPost]
        [Route("patient")]
        public async Task<HttpResponseMessage> Patient([FromBody]PatientDto patientDto)
        {
            FhirClient client = new FhirClient(_settings.Value.FhirApiUrl);
            var patient = new Hl7.Fhir.Model.Patient
            {
                Active = true,
                Name = new List<HumanName>{new HumanName
                {
                    Given = new List<string>{patientDto.firstName},
                    Family = patientDto.lastName
                }}
            };

            var response = await client.CreateAsync(patient);

            return new HttpResponseMessage {StatusCode = HttpStatusCode.Created};
        }
    }
}
