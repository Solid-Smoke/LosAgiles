namespace back_end.Domain
{
    public class ShoppingCartItemModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? BusinessName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalSales { get; set; }
        
        public decimal Weight { get; set; }
    }
}
