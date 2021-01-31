using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebAPI.ServiceExtensions
{
    internal static class SwaggerExtensions
    {
        private const string WebApiTitle = "Tweet Analyzer WebAPI";
        private const string WebApiVersion = "v1";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(WebApiVersion,
                    new OpenApiInfo
                    {
                        Title = WebApiTitle,
                        Version = WebApiVersion
                    });
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
                $"/swagger/{WebApiVersion}/swagger.json",
                $"{WebApiTitle} {WebApiVersion}"
            ));
            return app;
        }
    }
}