using System;
using System.Collections.Generic;
using System.Linq;

using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Services.Interfaces;

namespace Arrow.DeveloperTest.Runner
{
  public class TestHelper
  {
    private readonly IAccountService _accountService;
    private readonly IPaymentService _paymentService;
    private readonly Random _random = new Random();
    public TestHelper(IPaymentService paymentService, IAccountService accountService)
    {
      _paymentService = paymentService;
      _accountService = accountService;
    }
    public void DisplayMenu()
    {
      Console.WriteLine("\n");
      Console.WriteLine("Select one of the menu");
      Console.WriteLine(string.Format("|{0,5}|{1,5}|", 1, ":Display Test Data"));
      Console.WriteLine(String.Format("|{0,5}|{1,5}|", 2, ":Request for Payment"));
      Console.WriteLine(String.Format("|{0,5}|{1,5}|", 3, ":Exit Console Application"));
      Console.WriteLine(String.Format("|{0,5}|{1,5}|", 4, ":Recreate Test Data"));
      Console.WriteLine("\n");
    }

    public void DisplaySelectedMenu(int menuNumber)
    {
      switch (menuNumber)
      {
        case 1:
          DisplayMenu();
          DisplayTestData(PrepareTestData());
          break;
        case 2:
          var paymentResult = _paymentService.MakePayment(GetUserInputs());
          if (!paymentResult.Success)
            Console.WriteLine("Payment not done successfully.");
          else
            Console.WriteLine("Payment done.");
          DisplayMenu();
          break;
        case 3:
          Environment.Exit(0);
          break;
        case 4:
          ClearTestData();
          DisplayTestData(PrepareTestData());
          DisplayMenu();
          break;
      }

      DisplaySelectedMenu(int.Parse(Console.ReadLine()));
    }

    public void ClearTestData()
    {
      _accountService.UpdateAccounts(new List<Account>());
    }

    public List<Account> PrepareTestData()
    {
      var accounts = _accountService.GetAllAccounts();
      if (accounts.Count > 0) return accounts;
      for (int i = 0; i < _random.Next(2, 10); i++)
      {
        if (accounts.All(x => x.AccountNumber != $"xyz{i}"))
        {
          accounts.Add(new Account()
          {
            AccountNumber = $"xyz{i}",
            AllowedPaymentSchemes = (AllowedPaymentSchemes)_random.Next(1, 3),
            Balance = _random.Next(1000, 5000),
            Status = (AccountStatus)_random.Next(0, 3)
          });
        }
      }
      accounts?.OrderBy(x => x.AccountNumber);
      return accounts;
    }

    public void DisplayTestData(List<Account> accounts)
    {
      Console.WriteLine("\n");
      foreach (Account account in accounts)
      {
        Console.WriteLine(string.Format("|{0,5}|{1,5}|{2,5}|{3,5}|",
          account.AccountNumber, account.AllowedPaymentSchemes, account.Balance, account.Status));
      }
      Console.WriteLine("\n");
    }

    public MakePaymentRequest GetUserInputs()
    {
      MakePaymentRequest paymentRequest = new MakePaymentRequest
      {
        PaymentDate = DateTime.Today
      };
      Console.WriteLine("Please enter your account number");
      paymentRequest.DebtorAccountNumber = Console.ReadLine();

      Console.WriteLine("Please enter creditor account number");
      paymentRequest.CreditorAccountNumber = Console.ReadLine();

      Console.WriteLine("Please enter amount to transfer");
      paymentRequest.Amount = decimal.Parse(Console.ReadLine());

      Console.WriteLine("Please enter payment scheme");
      foreach (var paymentScheme in Enum.GetValues(typeof(AllowedPaymentSchemes)).Cast<AllowedPaymentSchemes>().ToList())
      {
        Console.WriteLine(paymentScheme);
      }
      Console.WriteLine("\n");
      Enum.TryParse(Console.ReadLine(), out PaymentScheme myStatus);
      paymentRequest.PaymentScheme = (PaymentScheme) myStatus;
      return paymentRequest;
    }
  }
}
