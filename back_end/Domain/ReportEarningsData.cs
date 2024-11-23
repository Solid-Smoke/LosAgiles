namespace back_end.Domain
{
    public class ReportEarningsData
    {
        public string? BusinessName { get; set; }
        public string? Month { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
