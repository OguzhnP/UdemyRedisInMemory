using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedis.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            await _distributedCache.SetStringAsync("name", "fatih", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(220)
            });
            return View();
        }
        public async Task<IActionResult> Show()
        {
            var res = await _distributedCache.GetStringAsync("name");
            ViewBag.name = res;
            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("name");
            return View();
        }
    }
}
