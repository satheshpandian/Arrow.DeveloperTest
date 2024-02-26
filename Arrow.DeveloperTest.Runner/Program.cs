using System;

using Arrow.DeveloperTest.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Arrow.DeveloperTest.Runner
{
  class Program
    {
      private readonly IPaymentService _paymentService;
      private readonly IAccountService _accountService;
      private readonly Random _random = new Random();
    public Program(IPaymentService paymentService, IAccountService accountService)
      {
        _paymentService = paymentService;
        _accountService = accountService;
      }
      static void Main(string[] args)
      {
        var host = Startup.CreateHostBuilder(args).Build();
        try
        {
          host.Services.GetRequiredService<Program>().Run();

          Console.ReadKey();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }

      public void Run()
      {
      //Preparing Sample data
      Console.WriteLine("Preparing Sample data.");
      TestHelper testHelper = new TestHelper(_paymentService,_accountService);
      testHelper.DisplaySelectedMenu(1);
      
      }
    }
}
