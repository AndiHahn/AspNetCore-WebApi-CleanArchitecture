using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CleanArchitecture.Web.Extensions;
using CleanArchitecture.Web.Middleware;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string allowSpecificOriginsPolicy = "MyAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = Configuration.GetSection("CorsConfiguration")
                .GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    allowSpecificOriginsPolicy,
                    builder =>
                    {
                        builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            services.ConfigureProblemDetails(Configuration);

            services.ConfigureJwtAuthentication(Configuration);

            services.AddApplicationServices(Configuration);

            // Add Swagger including configuration to support authenticated calls
            // using a Bearer token.
            services.AddSwaggerGen(c =>
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Budget Manager API", Version = version });
                var scheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Authentication header using the Bearer scheme. Example: \"Bearer << token >>\"",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                };
                c.AddSecurityDefinition("Bearer", scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            },
                        }, new List<string>()
                    },
                });

                // Set the comments path for the Swagger JSON and UI (name and path configured in project settings)
                var basePath = AppContext.BaseDirectory;
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(basePath, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            // Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budget Manager API");
            });

            app.UseRouting();

            app.UseCors(allowSpecificOriginsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<CurrentUserMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
