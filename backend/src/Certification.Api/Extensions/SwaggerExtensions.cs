using System.IO;
using System.Reflection;
using Asp.Versioning;
using Microsoft.OpenApi;

namespace Certification.Api.Extensions;

public static class SwaggerExtensions
{
    private const string DocumentName = "v1";

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(DocumentName, new OpenApiInfo
            {
                Title = "Certification Assessment Platform API",
                Version = DocumentName,
                Description = "Enterprise Certification Assessment Platform REST API",
                Contact = new OpenApiContact
                {
                    Name = "ISSQUARED Engineering",
                },
                License = new OpenApiLicense
                {
                    Name = "Internal Use",
                },
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter a valid JWT bearer token.",
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (environment.IsProduction())
        {
            return app;
        }

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{DocumentName}/swagger.json", "Certification Assessment Platform API v1");
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnablePersistAuthorization();
            options.DefaultModelsExpandDepth(0);
        });

        return app;
    }
}
