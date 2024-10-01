namespace back_end.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }
        public decimal? Weight { get; set; }
        public bool? Perishable { get; set; }
        public int? DailyAmount { get; set; }
        public string? DaysAvailable { get; set; }
        public int? BusinessID { get; set; }
        public byte[]? ProductImage { get; set; }
    }
}