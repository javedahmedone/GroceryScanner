using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using GroceryScanner.Business.Service.Place;
using GroceryScanner.Domain.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class PlaceService : IPlaceService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly string _autoComplete;
    private readonly string _placeByPlaceId;
    public PlaceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["PlacesAPI:BaseUrl"]!;
        _apiKey = configuration["PlacesAPI:APIKey"]!;
        _autoComplete = configuration["PlacesAPI:Autocomplete"]!;
        _placeByPlaceId = configuration["PlacesAPI:PlacesByPlaceId"]!;
    }

    public async Task<object> GetPlaceBySearchItem1(string searchItem)
    {
        //var url = "https://blinkit.com/mapAPI/autosuggest_google?lat=28.4652382&lng=77.0615957&query="+searchItem;
        var url = "https://qp94doiea4.execute-api.ap-south-1.amazonaws.com/default/qc?type=mapsuggest&query=" + searchItem;
        //_httpClient.DefaultRequestHeaders.Add("Referer", "https://blinkit.com/");
        _httpClient.DefaultRequestHeaders.Add("Cookie", "gr_1_deviceId=your-cookie-value");
        _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(request);
        var encoding = response.Content.Headers.ContentEncoding;
        Stream responseStream = await response.Content.ReadAsStreamAsync();

        if (encoding.Contains("gzip"))
        {
            using var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressedStream);
            var json = await reader.ReadToEndAsync();
            var dataList = System.Text.Json.JsonSerializer.Deserialize<object>(json);

            return dataList;
        }
        else if (encoding.Contains("deflate"))
        {
            using var decompressedStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressedStream);
            var json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<PlaceModel>(json);
        }
        else if (encoding.Contains("br")) // Brotli compression
        {
            using var decompressedStream = new BrotliStream(responseStream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressedStream);
            var json = await reader.ReadToEndAsync();
            var dataList = System.Text.Json.JsonSerializer.Deserialize<object>(json);

            return JsonConvert.DeserializeObject<PlaceModel>(json);
        }
        else
        {
            // No compression, read as a normal string
            var json = await new StreamReader(responseStream).ReadToEndAsync();
            return JsonConvert.DeserializeObject<PlaceModel>(json);
        }
    }

    public async Task<object> GetPlaceBySearchItem(string query)
    {

        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be empty");
        var requestUrl = _baseUrl + _autoComplete + query + "&key=" + _apiKey;

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dataList = System.Text.Json.JsonSerializer.Deserialize<object>(json);
            return dataList!;

        }

        return null;
    }


    public async Task<object> PlacesByPlaceIdAsync(string placeId)
    {
        if (string.IsNullOrWhiteSpace(placeId))
            throw new ArgumentException("Query cannot be empty");
        var requestUrl = _baseUrl + _placeByPlaceId + placeId + "&key=" + _apiKey;


        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dataList = System.Text.Json.JsonSerializer.Deserialize<object>(json);
            return dataList!;

        }
        return null;

    }

    public async Task<object> PlaceByLonAndLatAsync(string longtitude, string latitude)
    {
        var requestUrl = "https://nominatim.openstreetmap.org/reverse?format=json&lat=" + latitude + "&lon=" + latitude;

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dataList = System.Text.Json.JsonSerializer.Deserialize<object>(json);
            return dataList!;

        }
        return null;
    }
}
