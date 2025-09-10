using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                //AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.Normal
            };
            cacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key} => {value} => reason = {reason}"); 
            });
            _memoryCache.Set<string>("time", DateTime.Now.ToString(), cacheEntryOptions);


            var p = new Product {  Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("Product:1", p);

            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("time", out string? time);
            _memoryCache.TryGetValue("callback", out string? callback);
            ViewBag.Time = time;
            ViewBag.Callback = callback;
            ViewBag.Product = _memoryCache.Get("Product:1");
            return View();
        }
    }
}

