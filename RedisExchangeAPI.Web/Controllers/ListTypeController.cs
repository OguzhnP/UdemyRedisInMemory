using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string listKey = "mylist";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            List<string> nameList = new List<string>();
            if (_database.KeyExists(listKey))
            {
                 _database.ListRange(listKey).ToList().ForEach(c =>
                 {
                     nameList.Add(c.ToString());
                 });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        { 
            _database.ListRightPush(listKey, name);
            return RedirectToAction(nameof(Index));
        }  
        public IActionResult DeleteItem(string name)
        { 
            _database.ListRemoveAsync(listKey, name).Wait();
            //delete first item
            //_database.ListLeftPop(listKey);
            //Delete last item
            //_database.ListRightPop(listKey);
            return RedirectToAction(nameof(Index));
        } 
        public IActionResult Show()
        {
             

            return View();
        }
    }
}
