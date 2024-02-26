using Arrow.DeveloperTest.Repository;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Arrow.DeveloperTest.Runner
{
  public static class Startup
  {
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args).ConfigureServices(services => {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPaymentValidationService, PaymentValidationService>();
        services.AddScoped<IAccountDataStoreRepository, AccountDataStore>();
        services.AddScoped<Program>();
        services.AddScoped<IPaymentService, PaymentService>();
    
      });
    }
  }
}