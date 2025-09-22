 using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            _database.StringSet("name", "Oguzhan");
            _database.StringSet("Visitor", 100);
            return View();
        } 
        public IActionResult Show()
        {
            
            var nameRange = _database.StringGetRange("name", 0, 2);
            var nameLength = _database.StringLength("name");
            var nameExists = _database.KeyExists("name");
            var nameAppend = _database.StringAppend("name", " Portakal");
            var name = _database.StringGet("name");
            if (name.HasValue)
                ViewBag.name = name.ToString();
            else
                ViewBag.name = "Not Found";


            _database.StringIncrement("Visitor", 10); 
            //var count = _database.StringDecrementAsync("Visitor", 1).Result; 
            _database.StringDecrementAsync("Visitor", 1).Wait(); 
            var visitor = _database.StringGet("Visitor");

            if (visitor.HasValue)
                ViewBag.visitor = visitor.ToString();
            else
                ViewBag.visitor = "Not Found";


            Byte[] byteValue = default(byte[]);
            _database.StringSet("Image", byteValue);

            return View();
        }
    }
}
