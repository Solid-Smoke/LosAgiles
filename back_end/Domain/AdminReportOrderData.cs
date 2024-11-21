namespace back_end.Domain {
    public class AdminReportOrderData : ReportOrderData {
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? UserID { get; set; }
    }
}
