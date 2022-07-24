using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusMessageConsumer : IAzureServiceBusMessageConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderPaymentUpdateResultTopic;
        private readonly string emailSubscription;

        private readonly EmailRepository _emailRepository;

        private ServiceBusProcessor _serviceBusEmailProcessor;

        private readonly IConfiguration _configuration;
        //private readonly IMessageBus _messageBus;

        public AzureServiceBusMessageConsumer(EmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;

            emailSubscription = _configuration.GetValue<string>("SubscriptionName");
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderPaymentUpdateResultTopic = _configuration.GetValue<string>("OrderPaymentUpdateResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _serviceBusEmailProcessor = client.CreateProcessor(orderPaymentUpdateResultTopic, emailSubscription);
            //_messageBus = messageBus;
        }

        public async Task Start()
        {
            _serviceBusEmailProcessor.ProcessMessageAsync += OnUpdatePaymentMessageReceived;
            _serviceBusEmailProcessor.ProcessErrorAsync += ErrorHandler;
            await _serviceBusEmailProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _serviceBusEmailProcessor.StartProcessingAsync();
            await _serviceBusEmailProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnUpdatePaymentMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage emailMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            try
            {
                await _emailRepository.SendAndLogEmail(emailMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
