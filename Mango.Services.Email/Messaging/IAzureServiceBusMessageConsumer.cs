using System.Threading.Tasks;

namespace Mango.Services.Email.Messaging
{
    public interface IAzureServiceBusMessageConsumer
    {
        Task Start();
        Task Stop();
    }
}
