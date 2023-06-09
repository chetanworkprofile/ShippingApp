﻿using ShippingApp.Models.InputModels;

namespace ShippingApp.Models
{
    public class Shipment
    {
        public Guid customerId { get; set; }
        public Guid transactionRecordId { get; set; }
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public decimal? shipmentWeight { get; set; }
        public Guid origin { get; set; } 
        public Guid destination { get; set; } 
        public string notes { get; set; } = string.Empty;

        public Shipment(){} 

        public Shipment(Guid id,AddShipment a)
        {
            customerId= id;
            transactionRecordId= a.transactionRecordId;
            productTypeId = a.productTypeId;
            containerTypeId = a.containerTypeId;
            shipmentWeight = a.shipmentWeight;
            origin = a.origin;
            destination = a.destination;
            notes = a.notes;
        }
    }
}
