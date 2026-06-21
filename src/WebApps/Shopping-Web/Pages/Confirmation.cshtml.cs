using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping_Web.Pages
{
    public class ConfirmationModelModel : PageModel
    {

        public string Message { get; set; } = default!;
        public void OnGetContact()
        {
            Message = "Your email was sent.";
        }

        public void OnGetOrderSubmitted()
        {
            Message = "Your order was submitted successfully.";

        }
    }
}
