namespace back_end.Models
{
    public class BusinessWithAddressModel
    {
        public int? BusinessID { get; set; }
        public required string? Name { get; set; }
        public required string? IDNumber { get; set; }
        public required string? Email { get; set; }
        public required string? Telephone { get; set; }
        public string? Permissions { get; set; }

        public required string Province { get; set; }
        public required string Canton { get; set; }
        public required string District { get; set; }
        public required string PostalCode { get; set; }
        public required string OtherSigns { get; set; }
    }
}
