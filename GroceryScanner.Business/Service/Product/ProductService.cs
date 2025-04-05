using System.Text.Json;
using Confluent.Kafka;
using GroceryScanner.Business.Service.Product;
using Microsoft.Extensions.Configuration;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public ProductService(HttpClient httpClient, IProducer<string, string> producer, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _producer = producer;
        _topic = configuration["Kafka:Topic"]!;
    }

    public async Task<object> GetProductDetailAsync(string searchItem, string latitude, string longitude, string queryType)
    {
        var url = $"https://qp94doiea4.execute-api.ap-south-1.amazonaws.com/default/qc?lat={latitude}&lon={longitude}&type={queryType}&query={searchItem}";

        //string url = $"https://example.com/api?lat={latitude}&lon={longitude}&type={queryType}&query={Uri.EscapeDataString(searchItem)}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
       

        //await _kafkaProducer.ProduceAsync(Topic, new Message<Null, string> { Value = searchItem });
        var result = await _producer.ProduceAsync(_topic, new Message<string, string> {  Value = searchItem });


        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<object>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}



