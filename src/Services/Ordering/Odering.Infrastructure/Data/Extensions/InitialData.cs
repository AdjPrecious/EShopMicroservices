

namespace Ordering.Infrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<Customer> Customers => new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("708BD17D-FADE-4F8A-87A5-95B306D41011")), "mehmet", "mehmet@gmail.com" ),
            Customer.Create(CustomerId.Of(new Guid("38552F46-9280-4B75-AE90-97AEA94809CD")), "john","john@gmail.com")
        };

        public static IEnumerable<Product> Products => new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("1DAFD0B6-36A7-4D52-B6EA-EA8FEF5A8A48")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("A3029FE5-901D-4031-BDBB-F3165DA204F9")), "Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid("08DB8048-B4F6-4A2A-84C9-F99D14F73302")), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("5ED7C47D-4FE1-46FD-9F18-603AB4363A00")), "Xiaomi Mi", 450),

        };

        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                var address1 = Address.Of("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
                var address2 = Address.Of("john", "doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

                var payment1 = Payment.Of("mehmet", "5555555555554444", "12/28", "355", 1);
                var payment2 = Payment.Of("john", "8885555555554444", "06/30", "222", 2);

                var order1 = Order.Create(OrderId.Of(Guid.NewGuid()), CustomerId.Of(new Guid("708BD17D-FADE-4F8A-87A5-95B306D41011")), OrderName.Of("ORD_1"), shippingAddress: address1, billingAddress: address1, payment1);

                order1.Add(ProductId.Of(new Guid("1DAFD0B6-36A7-4D52-B6EA-EA8FEF5A8A48")), 2, 500);
                order1.Add(ProductId.Of(new Guid("A3029FE5-901D-4031-BDBB-F3165DA204F9")), 1, 400);

                var order2 = Order.Create(OrderId.Of(Guid.NewGuid()), CustomerId.Of(new Guid("38552F46-9280-4B75-AE90-97AEA94809CD")), OrderName.Of("ORD_2"), shippingAddress: address2, billingAddress: address2, payment2);

                order2.Add(ProductId.Of(new Guid("08DB8048-B4F6-4A2A-84C9-F99D14F73302")), 1, 650);
                order2.Add(ProductId.Of(new Guid("5ED7C47D-4FE1-46FD-9F18-603AB4363A00")), 2, 450);

                return new List<Order> { order1, order2 };
            }
        }
    }
}