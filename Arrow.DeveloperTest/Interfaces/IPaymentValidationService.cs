
using Arrow.DeveloperTest.Models;

namespace Arrow.DeveloperTest.Services.Interfaces
{
  public interface IPaymentValidationService
  {
    bool IsBACSPaymentAllowed(Account account);
    bool IsFasterPaymentAllowed(Account account, decimal amount);
    bool IsCHAPSPaymentAllowed(Account account);
  }
}
