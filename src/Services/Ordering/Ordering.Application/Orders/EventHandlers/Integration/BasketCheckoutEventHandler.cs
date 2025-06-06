using BuildingBlock.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler (ISender sender, ILogger<BasketCheckoutEventHandler> logger): IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            //TODO: Create new order and start order fullfillment process
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

            var command = MapToCreateOrderCommand(context.Message);
            await sender.Send(command);

        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            //Create full order with incoming event data
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(Id: orderId, CustomerId: message.CustomerId, OrderName: message.UserName, ShippingAddress: addressDto, BillingAddress: addressDto, Payment: paymentDto, Status: Ordering.Domain.Enums.OrderStatus.Pending, OrderItems: [
                new OrderItemDto(orderId, new Guid("1DAFD0B6-36A7-4D52-B6EA-EA8FEF5A8A48"), 2, 500),
                new OrderItemDto(orderId, new Guid("A3029FE5-901D-4031-BDBB-F3165DA204F9"), 1, 400)
                ]);

            return new CreateOrderCommand(orderDto);
        }
    }
}
