namespace back_end.Domain
{
    public class ReportCompletedOrderData : ReportOrderData
    {
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}