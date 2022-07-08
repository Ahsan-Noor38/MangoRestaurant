using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Messaging
{
    public interface IAzureServiceBusMessageConsumer
    {
        Task Start();
        Task Stop();
    }
}
