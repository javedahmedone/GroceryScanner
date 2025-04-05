using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryScanner.Domain.Domain;

namespace GroceryScanner.Business.Service.Place
{
    public interface IPlaceService
    {
        Task<object> GetPlaceBySearchItem(string  searchItem);
        Task<object> PlacesByPlaceIdAsync(string placeId);
        Task<object> PlaceByLonAndLatAsync(string longtitude, string latitude);
    }
}
