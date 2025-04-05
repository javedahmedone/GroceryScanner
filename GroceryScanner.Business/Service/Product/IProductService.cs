using GroceryScanner.Domain.Domain;

namespace GroceryScanner.Business.Service.Product
{
    public interface IProductService
    {
        Task<object> GetProductDetailAsync(string searchItem, string latitude, string longitude, string queryType);
    }

}
