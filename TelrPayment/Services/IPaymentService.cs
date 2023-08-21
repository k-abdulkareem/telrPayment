using TelrPayment.Helpers;
using TelrPayment.Models.Telr.SendRequest;

namespace TelrPayment.Services
{
    public interface IPaymentService
    {
        Task<Response<string>> SendPaymentRequest(int orderId);
        Task<Response<string>> CheckPaymentRequest(int orderId);
    }
}
