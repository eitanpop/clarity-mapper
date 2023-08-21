using AutoMapper;
using ClarityMapper.Adapters;
using ClarityMapper.CDA;
using ClarityMapper.Class;
using ClarityMapper.Contracts;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.V2;
using ClarityMapper.V2.Converters;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ClarityFunction.Startup))]

namespace ClarityFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IObjectMapperWrapper, AutomapperObjectWrapper>();
            services.AddTransient<ICdaReader, CdaReader>();
            services.AddTransient<ICdaFhirMap, CdaFhirMap>();
            services.AddTransient<IFhirResourceStore, FhirResourceStore>();
            services.AddTransient<IParserFactory, ParserFactory>();
            services.AddTransient<IConverterPipeline>(x =>
            {
                var pipeline = new ConverterPipeline();
                pipeline.Add(new PatientConverter());
                pipeline.Add(new EncounterConverter());
                return pipeline;
            });
            services.AddTransient<HL7v2ToFhirResourcesService, HL7v2ToFhirResourcesService>();
            services.AddTransient<IHL7Adapter, HL7Adapter>();
            services.AddOptions<AppSettingsOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {  
                    configuration.Bind(settings);
                });
        }
    }
}
