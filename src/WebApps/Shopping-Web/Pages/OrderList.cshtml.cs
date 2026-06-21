namespace Shopping_Web.Pages
{
    public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger) : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // assumption customerId is passed in from the UI authenticated user swn
            var customerId = new Guid("708bd17d-fade-4f8a-87a5-95b306d41011");

            var response = await orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;

            return Page();
        }
    }
}
