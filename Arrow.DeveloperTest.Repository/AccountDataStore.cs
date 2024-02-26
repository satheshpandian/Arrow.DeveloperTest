using System;
using System.Collections.Generic;
using System.Linq;
using Arrow.DeveloperTest.Models;

namespace Arrow.DeveloperTest.Repository
{
  public class AccountDataStore : IAccountDataStoreRepository
  {
    /// <summary>
    /// To Keep the accounts in memory
    /// </summary>
    public List<Account> Accounts { get; set; } = new List<Account>();

    /// <summary>
    /// Get an account by account number from database
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns>account object</returns>
    public Account GetAccount(string accountNumber)
    {
      if (string.IsNullOrEmpty(accountNumber)) throw new ArgumentNullException(nameof(accountNumber));
      return Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
    }

    /// <summary>
    /// Get All Accounts for testing
    /// </summary>
    /// <returns>All accounts</returns>
    public List<Account> GetAllAccounts()
    {
      return Accounts;
    }

    /// <summary>
    /// Update the account object in the Database
    /// </summary>
    /// <param name="account"></param>
    public void UpdateAccount(Account account)
    {
      var oldAccount=Accounts.FirstOrDefault(x => x.AccountNumber == account.AccountNumber);
      Accounts.Remove(oldAccount);
      Accounts.Add(account);
    }
    /// <summary>
    /// Update accounts if empty
    /// </summary>
    /// <param name="accounts"></param>
    public void UpdateAccounts(List<Account> accounts)
    {
      Accounts.AddRange(accounts);
    }
  }
}
