using Eshop.Application.Shared;

namespace Eshop.API.Controllers
{
  public class CustomerRequest
  {

    public string CustomerName { get; set; }

    public CustomerRequest(string customerName)
    {
      CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
    }
  }
}
