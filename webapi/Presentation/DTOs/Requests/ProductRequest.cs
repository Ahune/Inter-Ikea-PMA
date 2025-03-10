namespace webapi.Presentation.DTOs.Requests;

public class ProductRequest
{
    public string Name { get; set; }
    public int ProductTypeId { get; set; }
    public List<int> ColourIds { get; set; } = new List<int>();
}
