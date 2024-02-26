using System.Collections.Generic;
using Arrow.DeveloperTest.Models;
using Arrow.DeveloperTest.Repository;
using Arrow.DeveloperTest.Services.Interfaces;

namespace Arrow.DeveloperTest.Services
{
  public class AccountService : IAccountService
  {
    private readonly IAccountDataStoreRepository _accountDataStore;

    /// <summary>
    /// Constructor of the account service
    /// </summary>
    /// <param name="accountDataStore"></param>
    public AccountService(IAccountDataStoreRepository accountDataStore)
    {
      _accountDataStore = accountDataStore ?? throw new System.ArgumentNullException(nameof(accountDataStore));
    }
    /// <summary>
    /// Get All accounts
    /// </summary>
    /// <returns></returns>
    public List<Account> GetAllAccounts()
    {
      return _accountDataStore.GetAllAccounts();
    }

    /// <summary>
    /// Update accounts 
    /// </summary>
    /// <param name="accounts"></param>
    public void UpdateAccounts(List<Account> accounts)
    {
      _accountDataStore.UpdateAccounts(accounts);
    }
  }
}
