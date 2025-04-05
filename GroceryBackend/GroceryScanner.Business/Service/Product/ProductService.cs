using System.Text.Json;
using Confluent.Kafka;
using GroceryScanner.Business.Service.Product;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IProducer<Null, string> _kafkaProducer;
    private const string Topic = "search-queries";
    public ProductService(HttpClient httpClient, IProducer<Null, string> kafkaProducer)
    {
        _httpClient = httpClient;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<object> GetProductDetailAsync(string searchItem, string latitude, string longitude, string queryType)
    {
        var url = $"https://qp94doiea4.execute-api.ap-south-1.amazonaws.com/default/qc?lat={latitude}&lon={longitude}&type={queryType}&query={searchItem}";

        //string url = $"https://example.com/api?lat={latitude}&lon={longitude}&type={queryType}&query={Uri.EscapeDataString(searchItem)}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<object>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}



