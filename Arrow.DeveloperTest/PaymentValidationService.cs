
using System;

using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Services.Interfaces;

namespace Arrow.DeveloperTest.Services
{
  public class PaymentValidationService : IPaymentValidationService
  {
    /// <summary>
    /// Validate if Bacs is allowed in an account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns>true if BACS allowed else false</returns>
    public bool IsBACSPaymentAllowed(Account account)
    {
      return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
    }

    /// <summary>
    /// Validate if Faster payments is allowed in an account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="amount"></param>
    /// <returns>true if Faster payments allowed else false</returns>
    public bool IsFasterPaymentAllowed(Account account, decimal amount)
    {
      if (Math.Sign(amount) <= 0) throw new ArgumentException("Invalid amount",nameof(amount));
      return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) && account.Balance >= amount;
    }
    /// <summary>
    /// Validate if Chaps payment is allowed in an account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns>true if Chaps allowed else false</returns>
    public bool IsCHAPSPaymentAllowed(Account account)
    {
      return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) && account.Status == AccountStatus.Live;
    }
  }
}
