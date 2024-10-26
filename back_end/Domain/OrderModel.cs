namespace back_end.Domain {
    public class OrderModel {
        public int? OrderID { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? ClientID { get; set; }
        public int? DeliveryAddress { get; set; }
    }
}
