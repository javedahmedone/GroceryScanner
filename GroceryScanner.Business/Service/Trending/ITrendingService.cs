using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryScanner.Business.Service.Trending
{
    public interface ITrendingService
    {
        Task LogSearchAsync(string searchTerm);
        Task<List<(string Item, double Count)>> GetTopKTrendingAsync(int k);

    }
}
