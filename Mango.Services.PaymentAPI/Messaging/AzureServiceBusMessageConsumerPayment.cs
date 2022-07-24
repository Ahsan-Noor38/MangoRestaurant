using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusMessageConsumerPayment : IAzureServiceBusMessageConsumerPayment
    {
        private readonly string serviceBusConnectionString;
        private readonly string paymentSubscription;
        private readonly string orderPaymentProcessorTopic;
        private readonly string orderPaymentUpdateResultTopic;

        private ServiceBusProcessor _orderPaymentProcessor;

        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        private readonly IProcessPayment _processPayment;

        public AzureServiceBusMessageConsumerPayment(IConfiguration configuration, IMessageBus messageBus, IProcessPayment processPayment)
        {
            _messageBus = messageBus;
            _configuration = configuration;
            _processPayment = processPayment;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            paymentSubscription = _configuration.GetValue<string>("OrderPaymentProcessorSubscription");
            orderPaymentProcessorTopic = _configuration.GetValue<string>("OrderPaymentProcessorTopic");
            orderPaymentUpdateResultTopic = _configuration.GetValue<string>("OrderPaymentUpdateResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessorTopic, paymentSubscription);
        }

        public async Task Start()
        {
            _orderPaymentProcessor.ProcessMessageAsync += ProcessPayments;
            _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderPaymentProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _orderPaymentProcessor.StartProcessingAsync();
            await _orderPaymentProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentMessageRequest = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                PaymentStatus = result,
                OrderId = paymentMessageRequest.OrderId,
            };

            try
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, orderPaymentUpdateResultTopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
