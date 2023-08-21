using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClarityAPI.Class;
using ClarityMapper.Adapters;
using ClarityMapper.Contracts;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.DomainObjects.FHIR;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task = Hl7.Fhir.Model.Task;

namespace ClarityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LegacyController : ControllerBase
    {
        private readonly ILogger<LegacyController> _logger;
        private readonly IHL7Adapter _adapter;
        private readonly IObjectMapperWrapper _mapper;
        private readonly IOptions<AppSettingsOptions> _settings;

        public LegacyController(ILogger<LegacyController> logger, IHL7Adapter adapter, IObjectMapperWrapper mapper,
            IOptions<AppSettingsOptions> settings)
        {
            _logger = logger;
            _adapter = adapter;
            _mapper = mapper;
            _settings = settings;
        }

        [HttpGet]
        public String Get()
        {
            return "Works!!";
        }

        [HttpPost]
        [Route("hl7v2")]
        public async Task<string> HL7V2()
        {
            var content = await GetRequestContent();
            var fhirResources = _adapter.FromV2(content).ToFhirResources();
            return await SaveFhirResources(fhirResources);
        }

        [HttpPost]
        [Route("cda")]
        public async Task<string> Cda()
        {
            var content = await GetRequestContent();
            var fhirResources = _adapter.FromCDA(content).ToFhirResources();
            return await SaveFhirResources(fhirResources);
        }

        private async Task<string> GetRequestContent()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
              return await reader.ReadToEndAsync();
            }
        }

        private async Task<string> SaveFhirResources(IList<IFhirResource> fhirResources)
        {
            FhirClient client = new FhirClient(_settings.Value.FhirApiUrl);
            IList<string> resourcesCreated = new List<string>();
            var firelyResources = _mapper.Map<List<DomainResource>>(fhirResources);
            try
            {
                foreach (var resource in firelyResources)
                {
                    var domainResource = await client.CreateAsync(resource);
                    resourcesCreated.Add(domainResource.TypeName);
                }

                return $"The following resources were created: {String.Join(", ", resourcesCreated)}";
            }
            catch (Exception ex)
            {
                return
                    $"There was an error \"{ex.Message}\". The following resources were created: {String.Join(", ", resourcesCreated)}";
            }
        }

    }
}
