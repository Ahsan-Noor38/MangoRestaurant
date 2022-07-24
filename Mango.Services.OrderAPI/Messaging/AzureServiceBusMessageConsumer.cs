using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusMessageConsumer : IAzureServiceBusMessageConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string checkoutMessageTopic;
        private readonly string checkoutSubscription;
        private readonly OrderRepository _orderRepository;

        private ServiceBusProcessor _serviceBusProcessor;

        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        public AzureServiceBusMessageConsumer(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            checkoutMessageTopic = _configuration.GetValue<string>("checkoutmessagetopic");
            checkoutSubscription = _configuration.GetValue<string>("CheckoutSubscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _serviceBusProcessor = client.CreateProcessor(checkoutMessageTopic, checkoutSubscription);
            _messageBus = messageBus;
        }

        public async Task Start()
        {
            _serviceBusProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _serviceBusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _serviceBusProcessor.StartProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDTO checkoutHeaderDTO = JsonConvert.DeserializeObject<CheckoutHeaderDTO>(body);

            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDTO.UserId,
                FirstName = checkoutHeaderDTO.FirstName,
                LastName = checkoutHeaderDTO.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDTO.CardNumber,
                CouponCode = checkoutHeaderDTO.CouponCode,
                CVV = checkoutHeaderDTO.CVV,
                DiscountTotal = checkoutHeaderDTO.DiscountTotal,
                Email = checkoutHeaderDTO.Email,
                ExpiryMonthYear = checkoutHeaderDTO.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDTO.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDTO.Phone,
                PickUpDateTime = checkoutHeaderDTO.PickUpDateTime
            };
            foreach (var detail in checkoutHeaderDTO.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    Price = detail.Product.Price,
                    ProductId = detail.ProductId,
                    ProductName = detail.Product.Name,
                    Count = detail.Count
                };
                orderHeader.CartTotalItems += detail.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = orderHeader.FirstName + " " + orderHeader.LastName,
                CardNumber = orderHeader.CardNumber,
                CVV = orderHeader.CVV,
                ExpiryMonthYear = orderHeader.ExpiryMonthYear,
                OrderId = orderHeader.OrderHeaderId,
                OrderTotal = orderHeader.OrderTotal,
            };

            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, "");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
