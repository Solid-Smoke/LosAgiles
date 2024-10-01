namespace back_end.Models
{
    public class BusinessAddressModel
    {
        public int? BusinessID { get; set; }
        public required string Province { get; set; }
        public required string Canton { get; set; }
        public required string District { get; set; }
        public required string PostalCode { get; set; }
        public required string OtherSigns { get; set; }
    }
}
