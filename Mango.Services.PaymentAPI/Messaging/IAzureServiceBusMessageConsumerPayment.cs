using System.Threading.Tasks;

namespace Mango.Services.PaymentAPI.Messaging
{
    public interface IAzureServiceBusMessageConsumerPayment
    {
        Task Start();
        Task Stop();
    }
}
