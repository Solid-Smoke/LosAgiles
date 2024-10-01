namespace back_end.Models
{
    public class BusinessModel
    {
        public int? BusinessID { get; set; }
        public required string? Name { get; set; }
        public required string? IDNumber { get; set; }
        public required string? Email { get; set; }
        public required string? Telephone { get; set; }
        public string? Permissions { get; set; }
    }
}
