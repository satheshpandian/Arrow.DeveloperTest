
using System;

using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Services.Interfaces;

using NUnit.Framework;

namespace Arrow.DeveloperTest.Tests
{
  public class PaymentValidationServiceTests
  {
    private readonly IPaymentValidationService _paymentValidationService =
      new PaymentValidationService();


    [TestCase(AllowedPaymentSchemes.Bacs, true)]

    [TestCase(AllowedPaymentSchemes.Chaps, false)]

    [TestCase(AllowedPaymentSchemes.FasterPayments, false)]
    public void IsBACSPaymentAllowed_ShouldValidate_AllowedPaymentType(AllowedPaymentSchemes allowedPaymentSchemes,bool expectedResult)
    {
      //Arrange
      var account = new Account { AllowedPaymentSchemes = allowedPaymentSchemes };
      //Act
      var result = _paymentValidationService.IsBACSPaymentAllowed(account);
      //Assert
      Assert.NotNull(result);
      Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.Live, true)]
    [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.Disabled, false)]
    [TestCase(AllowedPaymentSchemes.Chaps, AccountStatus.InboundPaymentsOnly, false)]
    [TestCase(AllowedPaymentSchemes.FasterPayments, AccountStatus.Live, false)]
    [TestCase(AllowedPaymentSchemes.Bacs, AccountStatus.Live, false)]
    public void IsCHAPSPaymentAllowed_ShouldValidate_AllowedPaymentType(AllowedPaymentSchemes allowedPaymentSchemes, AccountStatus accountStatus,
      bool expectedResult)
    {
      //Arrange
      var account = new Account { AllowedPaymentSchemes = allowedPaymentSchemes , Status = accountStatus };
      //Act
      var result = _paymentValidationService.IsCHAPSPaymentAllowed(account);
      //Assert
      Assert.NotNull(result);
      Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase(AllowedPaymentSchemes.FasterPayments, 100, true)]
    [TestCase(AllowedPaymentSchemes.FasterPayments, 10000000, false)]
    [TestCase(AllowedPaymentSchemes.Bacs, 100, false)]
    [TestCase(AllowedPaymentSchemes.Chaps, 100, false)]
    public void IsFasterPaymentAllowed_ShouldValidate_AllowedPaymentType(AllowedPaymentSchemes allowedPaymentSchemes, decimal amount,
      bool expectedResult)
    {
      //Arrange
      var account = new Account { AllowedPaymentSchemes = allowedPaymentSchemes, Balance = 100};
      //Act
      var result = _paymentValidationService.IsFasterPaymentAllowed(account,amount);
      //Assert
      Assert.NotNull(result);
      Assert.That(result, Is.EqualTo(expectedResult));
    }
    [Test]
    public void IsFasterPaymentAllowed_ShouldThrowException_IfAmountIsInvalid()
    {
      //Arrange
      var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 100 };
      //Act + Assert
      var result = Assert.Throws<ArgumentException>(() => _paymentValidationService.IsFasterPaymentAllowed(account,0));
      Assert.NotNull(result);
      Assert.That(result.ParamName, Is.EqualTo("amount"));
    }

  }
}
