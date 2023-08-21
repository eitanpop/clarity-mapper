using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClarityMapper.Class;
using ClarityMapper.Contracts;
using ClarityMapper.Contracts.Adapters.Contracts;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ClarityFunction
{
    public class FhirFunction
    {
        private readonly IOptions<AppSettingsOptions> _settings;
        public FhirFunction(IOptions<AppSettingsOptions> settings)
        {
            _settings = settings;
        }

        [FunctionName("Fhir")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", "put", "delete", Route = "{resource}")] HttpRequest req, string resource, ILogger log)
        {
            return await ProcessFhirRequestAsync(req, resource, log);
        }

        private async Task<IActionResult> ProcessFhirRequestAsync(HttpRequest req, string resource, ILogger log)
        {
            try
            {
                log.LogInformation("Processing FHIR request");

                string fhir = await new StreamReader(req.Body).ReadToEndAsync();
                HttpMethod verb = new HttpMethod(req.Method);
                var httpResponseMessage = await SendFhirAsync(resource, verb, fhir);
                string responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                return new ObjectResult(responseMessage) { StatusCode = (int)httpResponseMessage.StatusCode };
            }
            catch (Exception ex)
            {
                log.LogCritical(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        private async Task<HttpResponseMessage> SendFhirAsync(string resource, HttpMethod verb, string fhir)
        {
            //Todo: Add HttpClient factory in DI 
            using var client = new HttpClient();

            using var requestMessage =
                new HttpRequestMessage(verb, $"{_settings.Value.FhirApiUrl}{resource}")
                {
                    Content = new StringContent(fhir, Encoding.UTF8, "application/fhir+json")
                };

            var httpResponseMessage = await client.SendAsync(requestMessage);
            return httpResponseMessage;
        }
    }
}
