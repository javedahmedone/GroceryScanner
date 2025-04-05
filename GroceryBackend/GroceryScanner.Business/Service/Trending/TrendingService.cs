using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace GroceryScanner.Business.Service.Trending
{
    public class TrendingService : ITrendingService
    {
        private readonly IConnectionMultiplexer _redis;

        public TrendingService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task LogSearchAsync(string searchTerm)
        {
        }

        public async Task<List<(string Item, double Count)>> GetTopKTrendingAsync(int k)
        {
            var db = _redis.GetDatabase();
            var keys = Enumerable.Range(0, 12)
                .Select(offset => $"search_trends:{DateTime.UtcNow.AddHours(-offset):yyyyMMddHH}")
                .ToList();

            var aggregated = new Dictionary<string, double>();

            foreach (var key in keys)
            {
                var entries = await db.SortedSetRangeByRankWithScoresAsync(key, 0, -1, Order.Descending);
                foreach (var entry in entries)
                {
                    var item = entry.Element.ToString();
                    aggregated[item] = aggregated.GetValueOrDefault(item, 0) + entry.Score;
                }
            }

            return aggregated.OrderByDescending(x => x.Value).Take(k).Select(x => (x.Key, x.Value)).ToList();
        }
    }

}
