using System.Net.Security;
using Confluent.Kafka;
using GroceryScanner.Business;
using GroceryScanner.Business.Service.Place;
using GroceryScanner.Business.Service.Product;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowLocalhost",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:4200") // Allow frontend
//                  .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
//                  .AllowAnyHeader() // Allow all headers
//                  .AllowCredentials(); // Allow cookies/auth headers
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var kafkaConfig = new ProducerConfig { BootstrapServers = "localhost:9092" }; // Change if needed

// Register Kafka Producer
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(kafkaConfig).Build());


builder.Services.AddHttpClient<IProductService, ProductService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new SocketsHttpHandler
        {
            SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = (sender, cert, chain, errors) => true
            }
        };
    });

builder.Services.AddHttpClient<IPlaceService, PlaceService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
