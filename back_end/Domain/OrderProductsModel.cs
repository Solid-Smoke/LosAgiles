namespace back_end.Domain
{
    public class OrderProductsModel
    {
        bool DataRetrieved { get; set; }
        public int? OrderID { get; set; }
        public int? ProductID { get; set; }
        public int? Amount { get; set; }
        public string? ProductName { get; set; }
    }
}
