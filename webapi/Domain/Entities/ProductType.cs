namespace webapi.Domain.Entities;

public class ProductType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
