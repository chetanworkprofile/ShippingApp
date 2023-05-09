
using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    public interface IPaymentService
    {
        public Response CreateOrder(int amount, string userId, string token, out int code);
        public Response VerifyPayment(string paymentId, string orderId, string signature, string userId, string token, out int code);
    }
}
