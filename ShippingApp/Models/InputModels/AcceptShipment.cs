﻿namespace ShippingApp.Models
{
    public class AcceptShipment
    {
        public Guid mapId { get; set; }
        public Guid driverId { get; set; }
        public bool isAccepted { get; set; }
    }
}
