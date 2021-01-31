using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.ServiceExtensions
{
    internal static class CompressionExtensions
    {
        public static IServiceCollection AddGzipCompression(this IServiceCollection services)
        {
            services
                .Configure<GzipCompressionProviderOptions>(
                    o => o.Level = CompressionLevel.Fastest)
                .AddResponseCompression(o =>
                {
                    o.EnableForHttps = true;
                    o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[]
                        {
                            // General
                            "text/plain",
                            // Static files
                            "text/css",
                            "application/javascript",
                            // MVC
                            "text/html",
                            "application/xml",
                            "text/xml",
                            "application/json",
                            "text/json"
                        });
                    o.Providers.Add<GzipCompressionProvider>();
                });
            return services;
        }
    }
}