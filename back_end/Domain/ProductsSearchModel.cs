namespace back_end.Domain;

public partial class ProductsSearchModel
{
    public int ProductID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public string BusinessName { get; set; }

    public byte[] ProductImage { get; set; }

    public string? ProductImageBase64 { get; set; }
}