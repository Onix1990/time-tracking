using System;
using System.Globalization;
using Api.Common.Extensions;
using Api.Common.Settings;
using Api.IoC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services) {
            var databaseSettingsSection = Configuration.GetSection("Database");
            services.Configure<DatabaseSettings>(databaseSettingsSection);

            ValidateAndThrowConfigurationSections(
                databaseSettingsSection
            );

            services.AddSwaggerDocument(config => {
                config.Title = "TimeTracking";
                config.Version = "v1";
                config.DocumentName = "docs";
            });
            services.AddRouting(options => options.LowercaseUrls = true);
            services
                .AddMvc(options => {
                    options.Filters.Add(
                        new ProducesAttribute("application/json"));
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterModule(new AppModule());
            AutofacContainer = builder.Build();

            return new AutofacServiceProvider(AutofacContainer);
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(
                name: "ru"
            );

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        private static void ValidateAndThrowConfigurationSections(
            IConfigurationSection databaseSettingsSection
        ) {
            databaseSettingsSection
                .CheckExistPropertiesAndThrow<DatabaseSettings>();
        }
    }
}