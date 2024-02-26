
using System;

using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Repository;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Services.Interfaces;

using Moq;

using NUnit.Framework;

namespace Arrow.DeveloperTest.Tests
{
  public class PaymentServiceTests
  {
    private readonly Mock<IAccountDataStoreRepository> _mockAccountDataStore = new Mock<IAccountDataStoreRepository>();
    private readonly Mock<IPaymentValidationService> _mockPaymentValidationService = new Mock<IPaymentValidationService>();
    private PaymentService _paymentService;

    [Test]
    public void MakePayment_WithInvalidDebtorAccount_ShouldFail()
    {
      // Arrange
      var request = new MakePaymentRequest { DebtorAccountNumber = "", PaymentScheme = PaymentScheme.Bacs, Amount = 50 };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act + Assert
      var result = Assert.Throws<ArgumentNullException>(() => _paymentService.MakePayment(request));
      Assert.NotNull(result);
      Assert.That(result.ParamName, Is.EqualTo("DebtorAccountNumber"));
    }
    [Test]
    public void MakePayment_WithInvalidCreditorAccount_ShouldFail()
    {
      // Arrange
      var request = new MakePaymentRequest { DebtorAccountNumber = "123", PaymentScheme = PaymentScheme.Bacs, Amount = 50, CreditorAccountNumber = "" };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act + Assert
      var result = Assert.Throws<ArgumentNullException>(() => _paymentService.MakePayment(request));
      Assert.NotNull(result);
      Assert.That(result.ParamName, Is.EqualTo("CreditorAccountNumber"));
    }

    [Test]
    public void MakePayment_WithInvalidAmount_ShouldFail()
    {
      // Arrange
      var request = new MakePaymentRequest
      {
        DebtorAccountNumber = "123",
        PaymentScheme = PaymentScheme.Bacs,
        Amount = 0,
        CreditorAccountNumber = "x123"
      };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act + Assert
      var result = Assert.Throws<ArgumentNullException>(() => _paymentService.MakePayment(request));
      Assert.NotNull(result);
      Assert.That(result.ParamName, Is.EqualTo("Amount"));
    }

    [Test]
    public void MakePayment_WithInvalidDate_ShouldFail()
    {
      // Arrange
      var request = new MakePaymentRequest {
        DebtorAccountNumber = "123",
        PaymentScheme = PaymentScheme.Bacs,
        Amount = 100,
        CreditorAccountNumber = "x123"
      };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act + Assert
      var result = Assert.Throws<ArgumentNullException>(() => _paymentService.MakePayment(request));
      Assert.NotNull(result);
      Assert.That(result.ParamName, Is.EqualTo("PaymentDate"));
    }

    [TestCase("123", 10000, AllowedPaymentSchemes.Bacs, false)]
    public void MakePayment_WithInvalidAccount_ShouldFail(string accountNumber, decimal balance,
      AllowedPaymentSchemes allowedPaymentSchemes, bool expectedResult)
    {
      // Arrange
      _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns((Account)null);
      _mockPaymentValidationService.Setup(x => x.IsBACSPaymentAllowed(It.IsAny<Account>())).Returns(AllowedPaymentSchemes.Bacs == allowedPaymentSchemes);
      var request = new MakePaymentRequest
      {
        DebtorAccountNumber = accountNumber,
        PaymentScheme = PaymentScheme.Bacs,
        Amount = 50,
        CreditorAccountNumber = "x123",
        PaymentDate = DateTime.Today
      };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act
      var result = _paymentService.MakePayment(request);

      //Assert
      Assert.That(result.Success, Is.EqualTo(expectedResult));
    }


    [TestCase("123", 10000, AllowedPaymentSchemes.Bacs, true)]
    [TestCase("123", 0, AllowedPaymentSchemes.Bacs,
                true)]// Overdraft or Negative balance is allowed
    [TestCase("123", 1000, AllowedPaymentSchemes.Chaps,
      false)]// Only Chaps allowed so it should fail
    [TestCase("123", 1000, AllowedPaymentSchemes.FasterPayments,
      false)]// Only faster payments allowed so it should fail
    public void MakePayment_WithValidBACSPayment_SucceedsAndInvalid_Failed(string accountNumber, decimal balance,
      AllowedPaymentSchemes allowedPaymentSchemes, bool expectedResult)
    {
      // Arrange
      var account = new Account { AccountNumber = accountNumber, Balance = balance, AllowedPaymentSchemes = allowedPaymentSchemes };
      _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
      _mockPaymentValidationService.Setup(x => x.IsBACSPaymentAllowed(It.IsAny<Account>())).Returns(AllowedPaymentSchemes.Bacs == allowedPaymentSchemes);
      var request = new MakePaymentRequest
      {
        DebtorAccountNumber = accountNumber,
        PaymentScheme = PaymentScheme.Bacs,
        Amount = 50,
        CreditorAccountNumber = "x123",
        PaymentDate = DateTime.Today
      };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act
      var result = _paymentService.MakePayment(request);

      //Assert
      Assert.That(result.Success, Is.EqualTo(expectedResult));
    }


    [TestCase("123", 10000, AllowedPaymentSchemes.Chaps, true)]
    [TestCase("123", 0, AllowedPaymentSchemes.Chaps,
      true)]// Overdraft or Negative balance is allowed
    [TestCase("123", 1000, AllowedPaymentSchemes.Bacs,
      false)]// Only bacs allowed so it should fail
    [TestCase("123", 1000, AllowedPaymentSchemes.FasterPayments,
      false)]// Only faster payments allowed so it should fail
    public void MakePayment_WithValidCHAPSPayment_SucceedsAndInvalid_Failed(string accountNumber, decimal balance,
      AllowedPaymentSchemes allowedPaymentSchemes, bool expectedResult)
    {
      // Arrange
      var account = new Account { AccountNumber = accountNumber, Balance = balance, AllowedPaymentSchemes = allowedPaymentSchemes };
      _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
      _mockPaymentValidationService.Setup(x => x.IsCHAPSPaymentAllowed(It.IsAny<Account>())).Returns(AllowedPaymentSchemes.Chaps == allowedPaymentSchemes);
      var request = new MakePaymentRequest
      {
        DebtorAccountNumber = accountNumber,
        PaymentScheme = PaymentScheme.Chaps,
        Amount = 50,
        CreditorAccountNumber = "x123",
        PaymentDate = DateTime.Today
      };
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act
      var result = _paymentService.MakePayment(request);

      //Assert
      Assert.That(result.Success, Is.EqualTo(expectedResult));
    }


    [TestCase("123", 10000, AllowedPaymentSchemes.FasterPayments, true)]
    [TestCase("123", 0, AllowedPaymentSchemes.FasterPayments,
      true)]// Overdraft or Negative balance not allowed
    [TestCase("123", 1000, AllowedPaymentSchemes.Bacs,
      false)]// Only bacs allowed so it should fail
    [TestCase("123", 1000, AllowedPaymentSchemes.Chaps,
      false)]// Only chaps allowed so it should fail
    public void MakePayment_WithValidFasterPaymentsPayment_SucceedsAndInvalid_Failed(string accountNumber, decimal balance,
      AllowedPaymentSchemes allowedPaymentSchemes, bool expectedResult)
    {
      // Arrange
      var account = new Account { AccountNumber = accountNumber, Balance = balance, AllowedPaymentSchemes = allowedPaymentSchemes };
      _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
      _mockPaymentValidationService.Setup(x => x.IsFasterPaymentAllowed(It.IsAny<Account>(), It.IsAny<decimal>())).Returns(AllowedPaymentSchemes.FasterPayments == allowedPaymentSchemes);
      var request = new MakePaymentRequest { DebtorAccountNumber = accountNumber, PaymentScheme = PaymentScheme.FasterPayments, Amount = 50, CreditorAccountNumber = "x123", PaymentDate = DateTime.Today};
      _paymentService = new PaymentService(_mockAccountDataStore.Object,
        _mockPaymentValidationService.Object);
      //Act
      var result = _paymentService.MakePayment(request);

      //Assert
      Assert.That(result.Success, Is.EqualTo(expectedResult));
    }
  }
}
