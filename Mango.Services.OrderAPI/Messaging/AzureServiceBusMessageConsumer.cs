﻿using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusMessageConsumer
    {
        private readonly OrderRepository _orderRepository;

        public AzureServiceBusMessageConsumer(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
        }
    }
}
