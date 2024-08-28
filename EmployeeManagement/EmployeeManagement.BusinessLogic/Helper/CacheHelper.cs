using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.Helper
{
    public class CacheHelper
    {
        public void GetCacheDetails()
        {
            var cacheEntryOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
                Size = 1024
            };
        }
    }
}


