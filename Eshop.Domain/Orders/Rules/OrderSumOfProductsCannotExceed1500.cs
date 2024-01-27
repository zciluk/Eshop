using Eshop.Domain.SeedWork;

namespace Eshop.Domain.Orders.Rules
{
  public class OrderSumOfProductsCannotExceed1500 : IBusinessRule
  {
    private readonly List<OrderProduct> _orderProducts;

    public OrderSumOfProductsCannotExceed1500(List<OrderProduct> orderProducts)
    {
      _orderProducts = orderProducts;
    }

    public bool IsBroken()
    {
      return _orderProducts.Sum(product => product.TotalCost) > 1500;
    }

    public string Message => "Sum of all products cost cannot exceed 1500 in one order";
  }
}