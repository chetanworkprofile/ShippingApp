using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShippingApp.Models.OutputModels;
using ShippingApp.RabbitMQ;
using System.Security.Claims;

namespace ShippingApp.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShippingHub : Hub
    {
        Response response = new Response();             //response model
        ResponseWithoutData response2 = new ResponseWithoutData();  //response model without data
        object result = new object();

        private readonly ILogger<string> _logger;

        //List<NotifyDriver> notifications;
        // to keep track of online users dict key-value pair
        // key - userId     value - connectionId
        //private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

        // this dictionary is moved to another file in MQConsumer/producer and all the connected users are stored there
        public ShippingHub(ILogger<string> logger)
        {
            _logger = logger;
        }
        public async override Task<Task> OnConnectedAsync()
        {
            _logger.LogInformation("New User connected to socket " + Context.ConnectionId);
            try
            {
                await Clients.Caller.SendAsync("UserConnected",Context.ConnectionId);       // reply back to connection to confirm his connection to socket
                string? userId = Context.User.FindFirstValue(ClaimTypes.Sid);               //get user id
                AddUserConnectionId(userId);                                                // add user to dictionary of connected users
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
            RemoveUserFromList(user);           //remove user from connected users dictionary

            await base.OnDisconnectedAsync(exception);
        }

        public void AddUserConnectionId(string userId)
        {
            _logger.LogInformation("User added to online dictionary " + userId);
            AddUserToList(userId, Context.ConnectionId);    //helper functon to add
            //await OnlineUsers();
        }
        public bool AddUserToList(string userToAdd, string connectionId)    //helper function to add users to list
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

        public string GetUserByConnectionId(string connectionId)        // get user id by connectionid
        {
            lock (ConnectionIds.Users)
            {
                return  ConnectionIds.Users.Where(x => x.Value == connectionId).Select(x => x.Key).First();
            }
        }

        public string GetConnectionIdByUser(string user)            //get connection id by userid
        {
            lock (ConnectionIds.Users)
            {
                return ConnectionIds.Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(string user)         //remove user from list/dictionary
        {
            lock (ConnectionIds.Users)
            {
                if (ConnectionIds.Users.ContainsKey(user))
                {
                    ConnectionIds.Users.Remove(user);
                }
            }
        }

        public string[] GetOnlineUsers()        //get userids of user that are connected currently
        {
            lock (ConnectionIds.Users)
            {
                return ConnectionIds.Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }

        public async Task RefreshAll()          // refreshall function is for front end to tell everyone to refresh list
        {
            await Clients.All.SendAsync("refreshList");
        }
    }
}







//code snippets just as backup

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