namespace back_end.Domain
{
    public class ReportOrderData
    {
        public int OrderID { get; set; }
        public string? BusinessName { get; set; }
        public int Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal SubtotalCost { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
