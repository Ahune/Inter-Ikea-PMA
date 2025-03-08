namespace webapi.Domain.Entities;

public class Colour
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductColour> ProductColours { get; set; } = new List<ProductColour>();
}
