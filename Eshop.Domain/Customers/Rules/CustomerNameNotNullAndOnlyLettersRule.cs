using Eshop.Domain.SeedWork;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Eshop.Domain.Orders.Rules
{
  public class CustomerNameNotNullAndOnlyLettersRule : IBusinessRule
  {
    private dynamic _customerName;
    private static string pattern = "^[a-zA-Z]*$";

    public CustomerNameNotNullAndOnlyLettersRule(string name)
    {
      _customerName = name;
    }
    public bool IsBroken() => !Regex.Match(_customerName, pattern, RegexOptions.IgnoreCase).Success;

    public string Message => "Name should be not empty and contain only letters";
  }


}
