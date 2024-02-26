using System.Collections.Generic;

using Arrow.DeveloperTest.Models;

namespace Arrow.DeveloperTest.Services.Interfaces
{
  public interface IAccountService
  {
    List<Account> GetAllAccounts();

    void UpdateAccounts(List<Account> accounts);
  }
}
