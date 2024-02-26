
using System;
using System.Collections.Generic;

using Arrow.DeveloperTest.Common.Enums;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Repository;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Services.Interfaces;

using Moq;

using NUnit.Framework;

namespace Arrow.DeveloperTest.Tests
{
  public class AccountServiceTests
  {
    private readonly Mock<IAccountDataStoreRepository> _mockAccountDataStore = new Mock<IAccountDataStoreRepository>();
    private IAccountService _accountService;

    [Test]
    public void GetAllAccounts_ShouldReturnAllAccounts()
    {
      //Arrange
      _mockAccountDataStore.Setup(x => x.GetAllAccounts())
        .Returns(new List<Account>() { new Account() {AccountNumber = "xyz"}});
      _accountService = new AccountService(_mockAccountDataStore.Object);
      //Act
      var result = _accountService.GetAllAccounts();
      //Assert
      Assert.NotNull(result);
      Assert.That(result.Count, Is.EqualTo(1));
      Assert.That(result[0].AccountNumber, Is.EqualTo("xyz"));
    }

    [Test]
    public void UpdateAccounts_ShouldUpdateAccounts()
    {
      //Arrange
      _mockAccountDataStore.Setup(x => x.UpdateAccounts(It.IsAny<List<Account>>())).Verifiable();
      _accountService = new AccountService(_mockAccountDataStore.Object);
      //Act 
      _accountService.UpdateAccounts(new List<Account>());

      //Assert
      _mockAccountDataStore.Verify(x => x.UpdateAccounts(It.IsAny<List<Account>>()), Times.AtLeastOnce);
    }
  }
}
