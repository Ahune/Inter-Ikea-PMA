namespace webapi.Presentation.DTOs.Responses;

public class ProductDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProductType { get; set; }
    public List<string> Colours { get; set; } = new List<string>();
}
