namespace back_end.Domain;

public partial class ClientsAddress
{
    public int AddressID { get; set; }

    public int UserID { get; set; }

    public string Province { get; set; }

    public string Canton { get; set; }

    public string District { get; set; }

    public int PostalCode { get; set; }

    public string OtherSigns { get; set; }
}