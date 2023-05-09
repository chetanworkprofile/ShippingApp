using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models;
using ShippingApp.RabbitMQ;
using Razorpay.Api;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;

namespace ShippingApp.Services
{
    public class PaymentService : IPaymentService
    {
        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();             // model to create token
        object result = new object();
        private readonly ShippingDbContext DbContext;
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _configuration;

        //constructor
        public PaymentService(IConfiguration configuration, ShippingDbContext dbContext, ILogger<PaymentController> logger)
        {
            DbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public Response CreateOrder(int amount, string userId, string token, out int code)
        {
            Guid id = new Guid(userId);             //create guid from string id
            var userLoggedIn = DbContext.Users.Find(id);

            if (token != userLoggedIn.token)        //check if user has valid token
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            Guid transcationId = Guid.NewGuid();
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", ((double)amount * 100)); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", transcationId);

            string key = _configuration.GetSection("RazorPay:key").Value!;
            string secret = _configuration.GetSection("RazorPay:secret").Value!;

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            DbContext.TransactionRecords.Add(new TransactionRecords(order["id"].ToString(), "", transcationId, DateTime.Now, false, amount));
            DbContext.SaveChanges();
            response = new Response(200, "Order id created successfully", order["id"].ToString(), true);
            code = 200;
            return response;
        }

        public Response VerifyPayment(string paymentId, string orderId, string signature, string userId, string token, out int code)
        {
            Guid id = new Guid(userId);             //create guid from string id
            var userLoggedIn = DbContext.Users.Find(id);

            if (token != userLoggedIn.token)        //check if user has valid token
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            try
            {
                RazorpayClient client = new RazorpayClient(_configuration.GetSection("RazorPay:key").Value!, _configuration.GetSection("RazorPay:secret").Value!);
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                attributes.Add("razorpay_payment_id", paymentId);
                attributes.Add("razorpay_order_id", orderId);
                attributes.Add("razorpay_signature", signature);

                TransactionRecords temp = DbContext.TransactionRecords.Where(s => s.orderId == orderId).First();
                Utils.verifyPaymentSignature(attributes);

                if(temp != null)
                {
                    temp.paymentId= paymentId;
                    temp.isPaid= true;
                }
                DbContext.SaveChanges();
                response = new Response(200,"Payment Successful",temp,true);
                code = 200;
                return  response;
            }
            catch (Exception ex)
            {
                code = 400;
                response = new Response(400, ex.Message, "", false);
                return response;
            }
        }
    }
}
