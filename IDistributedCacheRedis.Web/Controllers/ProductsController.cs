using IDistributedCacheRedis.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

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
            Product product = new Product { Id = 1, Name = "Kalem", Price = 200 };
            var jsonProduct = System.Text.Json.JsonSerializer.Serialize(product);

            // set byte
            //Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            //_distributedCache.Set($"Product:{product.Id}", byteProduct);

            await _distributedCache.SetStringAsync($"Product:{product.Id}",jsonProduct, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(220)
            });
            return View();
        }
        public async Task<IActionResult> Show()
        {
            //get byte
            //Byte[] byteProduct = _distributedCache.Get("Product:1");
            //string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            var res = await _distributedCache.GetStringAsync("Product:1");
            var product = System.Text.Json.JsonSerializer.Deserialize<Product>(res);
            ViewBag.product = product;
            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("name");
            return View();
        }
        public async Task<IActionResult> ImageCache()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", "erdal.jpg");
            await _distributedCache.SetAsync("image", await System.IO.File.ReadAllBytesAsync(path), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(120)
            }); 
            return View();
        }
        public async Task<IActionResult> ShowImage()
        {
            var imageByte = await _distributedCache.GetAsync("image");
            return File(imageByte, "image/jpg");
        }
    }
}
