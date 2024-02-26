using Arrow.DeveloperTest.Models;

namespace Arrow.DeveloperTest.Services.Interfaces
{
  public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
