using System.Collections.Generic;
using Arrow.DeveloperTest.Models;

namespace Arrow.DeveloperTest.Repository
{
  public interface IAccountDataStoreRepository
  {
    List<Account> GetAllAccounts();
    Account GetAccount(string accountNumber);
    void UpdateAccount(Account account);
    void UpdateAccounts(List<Account> account);
  }
}