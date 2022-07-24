using Mango.Services.PaymentAPI.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mango.Services.PaymentAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusMessageConsumerPayment ServiceBusMessageConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusMessageConsumer = app.ApplicationServices.GetService<IAzureServiceBusMessageConsumerPayment>();
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
