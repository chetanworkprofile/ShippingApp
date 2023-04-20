using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShippingApp.Data;
using ShippingApp.Models;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.RabbitMQ;
using System.Security.Claims;

namespace ShippingApp.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShippingHub : Hub
    {
        Response response = new Response();
        ResponseWithoutData response2 = new ResponseWithoutData();
        object result = new object();

        private readonly ShippingDbContext DbContext;
        private readonly ILogger<string> _logger;
        private readonly IMQConsumer _consumer;
        //List<NotifyDriver> notifications;
        // to keep track of online users dict key-value pair
        // key - userId     value - connectionId
        //private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        public ShippingHub(ShippingDbContext dbContext, ILogger<string> logger, IMQConsumer consumer)
        {
            DbContext = dbContext;
            _logger = logger;
            _consumer = consumer;
        }
        public async override Task<Task> OnConnectedAsync()
        {
            _logger.LogInformation("New User connected to socket " + Context.ConnectionId);
            try
            {
                await Clients.Caller.SendAsync("UserConnected");
                string? userId = Context.User.FindFirstValue(ClaimTypes.Sid);
                AddUserConnectionId(userId);

                /*AddUserConnectionId(userId);
                adminId = GetConnectionIdByUser(adminUserId);
                _logger.LogInformation($"admin userId: {adminUserId} admin id: {adminId}");
                if (adminId != null)
                {
                    await Clients.Clients(adminId).SendAsync("GetOnlineUsers");
                }
                //await Clients.Clients(adminId).SendAsync("GetOnlineUsers");
                //await Clients.Caller.SendAsync(adminId);
                //await Clients.AllExcept(Context.ConnectionId).SendAsync("hi All");
                //await Clients.All.SendAsync("GetOnlineUsers");
                *//*await Clients.All.SendAsync("mes",new PlaceOrder());*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("user disconnected");
            var user = GetUserByConnectionId(Context.ConnectionId);
            RemoveUserFromList(user);

            await base.OnDisconnectedAsync(exception);
        }

        public void AddUserConnectionId(string userId)
        {
            _logger.LogInformation("User added to online dictionary " + userId);
            AddUserToList(userId, Context.ConnectionId);
            //await OnlineUsers();
        }
        public bool AddUserToList(string userToAdd, string connectionId)
        {
            lock (ConnectionIds.Users)
            {
                foreach (var user in ConnectionIds.Users)
                {
                    if (user.Key.ToLower() == userToAdd.ToLower())
                    {
                        return false;
                    }
                }

                ConnectionIds.Users.Add(userToAdd, connectionId);
                _logger.LogInformation($"Add user : {userToAdd} connection id: {connectionId}");
                return true;
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (ConnectionIds.Users)
            {
                return  ConnectionIds.Users.Where(x => x.Value == connectionId).Select(x => x.Key).First();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (ConnectionIds.Users)
            {
                return ConnectionIds.Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(string user)
        {
            lock (ConnectionIds.Users)
            {
                if (ConnectionIds.Users.ContainsKey(user))
                {
                    ConnectionIds.Users.Remove(user);
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (ConnectionIds.Users)
            {
                return ConnectionIds.Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }

        //this functin will be called when s3 allocates driver to shipment and calls
        /*public async Task SendShipmentForDelivery(string shipmentId, string driverId)
        {
            *//*var driver = await DbContext.Users.FindAsync(driverId);
            if(driver == null || driver.userRole != "deliverBoy")
            {
                return;
            }*//*
            string connectionId = GetConnectionIdByUser(driverId);
            *//*if(connectionId != null) {
                await Clients.Client(connectionId).SendAsync("DeliveryBoyGetsShipment", shipmentId);
            }*//*
            
            await Clients.All.SendAsync("Deliveryinitiated",shipmentId);
        }*/
    }
}
