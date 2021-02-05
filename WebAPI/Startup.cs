using Core.Data;
using Core.Data.Interfaces;
using Core.Processing;
using Core.Processing.Interfaces;
using Core.Processing.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Helpers;
using WebAPI.Hubs;
using WebAPI.ServiceExtensions;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        private bool EnableDetailedErrors => Configuration
            .GetSection("WebSockets")
            .GetValue<bool>("EnableDetailedErrors");
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ITweetRepository, TweetRepository>()
                .AddTransient<IEmojiParser, EmojiParser>()
                .AddTransient<ITweetAnalysisStrategy, TweetAnalysisStrategy>()
                .AddTransient<IHostedServiceAccessor<TweetAnalysisService>, HostedServiceAccessor<TweetAnalysisService>>()
                .AddTransient<IHostedServiceAccessor<TweetProcessingService>, HostedServiceAccessor<TweetProcessingService>>()
                .AddHostedService<TweetProcessingService>()
                .AddHostedService<TweetAnalysisService>()
                .AddControllers();

            services
                .AddSwagger()
                .AddGzipCompression()
                .AddSignalR()
                .AddJsonProtocol()
                .AddHubOptions<MessagingHub>(options =>
                {
                    options.EnableDetailedErrors = EnableDetailedErrors;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerWithUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}