using Marten.Schema;

namespace CatalogAPI.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                   Id = new Guid("9FEAEF79-EE5B-4376-918B-D9189FEEA910"),
                   Name = "IPhone X",
                   Description = "This phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                   ImageFile = "product-1.png",
                   Price = 959.00M,
                   Category = new List<string> { "Smart Phone" }
                },

                new Product()
                {
                    Id = new Guid("68A8AA29-9D5B-4875-8C66-50545681FBA2"),
                    Name = "Samsung 10",
                    Description = "This Phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    Category = new List<string> { "Smart Phone" }
                },
                new Product()
                {
                    Id = new Guid("DA021EA8-BE44-42BF-AF4B-595D1508BD8A"),
                    Name = "Huawei Plus",
                    Description = "This Phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    Category = new List<string> { "White Appliances" }
                },

                new Product()
                {
                    Id = new Guid("FF4A7DB5-A208-40FD-A5F6-0B6514A02F19"),
                    Name = "Xiaomi Mi 9",
                    Description = "This TV is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-4.png",
                    Price = 470.00M,
                    Category = new List<string> { "White Appliances" }
                },

                new Product()
                {
                    Id = new Guid("E16766BD-C1C0-44B5-A5D4-3A91FC026657"),
                    Name = "HTC U11+ Plus",
                    Description = "This Phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    Category = new List<string> { "Smart Phone" }
                },

                new Product ()
                {
                    Id = new Guid("6BF7E348-22E3-4679-8222-6EA3384ADC9B"),
                    Name = "LG G7 ThinQ",
                    Description = "This Phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    Category = new List<string> { "Home Kitchen" }
                },

                new Product()
                {
                    Id = new Guid("62813C90-4402-476F-9FA4-FEBA7DAF63E9"),
                    Name = "Panasonic Lumix",
                    Description = "This Phone is the company's biggest change to its flagship phone in years, with a new design and a new screen that takes up almost the entire front of the device.",
                    ImageFile = "product-7.png",
                    Price = 240.00M,
                    Category = new List<string> { "Camera" }
                },

            };
        }
    }
}
