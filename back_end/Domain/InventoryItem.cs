namespace back_end.Domain
{
    public class InventoryItem
    {
        public int? ProductID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }
        public decimal? Weight { get; set; }
        public bool? IsPerishable { get; set; }
        public int? DailyAmount { get; set; }
        public string? DaysAvailable { get; set; }
        public int? BusinessID { get; set; }
    }
}
