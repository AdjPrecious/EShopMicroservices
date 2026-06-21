using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping_Web.Pages
{
    public class CheckoutModelModel(IBasketService basketService, ILogger<CheckoutModelModel> logger) : PageModel
    {
        [BindProperty]
        public BasketCheckoutModel Order { get; set; } = default!;

        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.LoadUserBasket();

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            logger.LogInformation("Checkout button clicked");

            Cart = await basketService.LoadUserBasket();

            if(!ModelState.IsValid)
            {
                return Page();
            }

            // assumption customerId is passed in from the UI authenticated user swn
            Order.CustomerId = new Guid("708bd17d-fade-4f8a-87a5-95b306d41011");
            Order.UserName = Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;


            await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
