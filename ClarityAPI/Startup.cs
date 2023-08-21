using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClarityAPI.Class;
using ClarityMapper.Adapters;
using ClarityMapper.CDA;
using ClarityMapper.Contracts;
using ClarityMapper.Contracts.Adapters.Contracts;
using ClarityMapper.V2;
using ClarityMapper.V2.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClarityAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

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

            IConfigurationSection sec = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsOptions>(sec);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();

            app.UseHttpsRedirection();



            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
