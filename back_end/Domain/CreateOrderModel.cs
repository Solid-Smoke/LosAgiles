﻿namespace back_end.Domain
{
    public class CreateOrderModel
    {
        public int OrderId { get; set; }
        public int ClientID { get; set; }
        public ClientsAddress DeliveryAddress { get; set; }
        public List<CreateOrderProductsModel> Products { get; set; }
    }
}