namespace back_end.Domain {
    public class OrderModel {
        public int? OrderID { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? ClientID { get; set; }
        public int? DeliveryAddress { get; set; }
        public string? Buyer { get; set; }
        public int? TotalAmount { get; set; }
        public string? Address { get; set; }
    }
}
