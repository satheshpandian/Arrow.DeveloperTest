using System;
using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Repository;
using Arrow.DeveloperTest.Services.Interfaces;

namespace Arrow.DeveloperTest.Services
{
  public class PaymentService : IPaymentService
  {
    private readonly IAccountDataStoreRepository _accountDataStore;

    private readonly IPaymentValidationService _paymentValidationService;
    /// <summary>
    /// Constructor of Payment Service
    /// </summary>
    /// <param name="accountDataStore"></param>
    /// <param name="paymentValidationService"></param>
    public PaymentService(IAccountDataStoreRepository accountDataStore, IPaymentValidationService paymentValidationService)
    {
      _accountDataStore = accountDataStore ?? throw new System.ArgumentNullException(nameof(accountDataStore));
      _paymentValidationService = paymentValidationService ?? throw new System.ArgumentNullException(nameof(paymentValidationService));
    }

    /// <summary>
    /// Make Payment request
    /// </summary>
    /// <param name="request"></param>
    /// <returns>MakePaymentResult Object</returns>
    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
      if (string.IsNullOrEmpty(request.DebtorAccountNumber)) 
        throw new ArgumentNullException(nameof(request.DebtorAccountNumber));
      if (string.IsNullOrEmpty(request.CreditorAccountNumber)) 
        throw new ArgumentNullException(nameof(request.CreditorAccountNumber));
      if (request.Amount <= Decimal.Zero) 
        throw new ArgumentNullException(nameof(request.Amount)); 
      if (!request.PaymentDate.HasValue ) 
        throw new ArgumentNullException(nameof(request.PaymentDate));
      Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
      MakePaymentResult result = new MakePaymentResult();

      if (account == null)
      {
        result.Success = false;
        return result;
      }

      switch (request.PaymentScheme)
      {
        case PaymentScheme.Bacs:
          result.Success = _paymentValidationService.IsBACSPaymentAllowed(account);
          break;
        case PaymentScheme.FasterPayments:
          result.Success = _paymentValidationService.IsFasterPaymentAllowed(account, request.Amount);
          break;
        case PaymentScheme.Chaps:
          result.Success = _paymentValidationService.IsCHAPSPaymentAllowed(account);
          break;
      }

      if (!result.Success) return result;
      UpdateAccounts(request, account);
      return result;
    }

    /// <summary>
    /// Update both creditor and debtor accounts
    /// </summary>
    /// <param name="request"></param>
    /// <param name="account"></param>
    private void UpdateAccounts(MakePaymentRequest request, Account account)
    {
      account.Balance -= request.Amount;
      Account creditorAccount = _accountDataStore.GetAccount(request.CreditorAccountNumber);
      creditorAccount.Balance += request.Amount;
      _accountDataStore.UpdateAccount(account);
      _accountDataStore.UpdateAccount(creditorAccount);
    }
  }
}
