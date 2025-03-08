using webapi.Domain.Entities;

public class ProductColour
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ColourId { get; set; }
    public Colour Colour { get; set; }
}