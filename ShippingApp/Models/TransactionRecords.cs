namespace ShippingApp.Models
{
    public class TransactionRecords
    {
        public Guid transactionRecordsId { get; set; }
        public string orderId { get; set; } = string.Empty;
        public string paymentId { get; set; } = string.Empty;
        public Guid transactionId { get; set; }
        public DateTime dateTime { get; set; }
        public bool isPaid { get; set; } = false;
        public int amount { get; set; } = 0;
        public TransactionRecords()
        {

        }
        public TransactionRecords(string orderId, string paymentId, Guid transactionId, DateTime dateTime, bool isPaid, int amount)
        {
            transactionRecordsId = new Guid();
            this.transactionId = transactionId;
            this.orderId = orderId;
            this.paymentId = paymentId;
            this.transactionId = transactionId;
            this.dateTime = dateTime;
            this.isPaid = isPaid;
            this.amount = amount;
        }
    }
}
