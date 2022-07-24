using Mango.Services.Email.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mango.Services.Email.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusMessageConsumer ServiceBusMessageConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusMessageConsumer = app.ApplicationServices.GetService<IAzureServiceBusMessageConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopped.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            ServiceBusMessageConsumer.Start();
        }

        private static void OnStop()
        {
            ServiceBusMessageConsumer.Stop();
        }
    }
}
