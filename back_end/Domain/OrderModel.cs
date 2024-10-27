namespace back_end.Domain
{
    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public int ClientID { get; set; }
        public ClientsAddress DeliveryAddress { get; set; }
        public List<OrderProductsModel> Products { get; set; }
    }
}
