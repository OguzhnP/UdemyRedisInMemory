using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string listKey = "hashNames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();
            if (_database.KeyExists(listKey))
            {
                _database.SetMembers(listKey).ToList().ForEach(c =>
                {
                    nameList.Add(c .ToString());
                });
            }
              
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            if(!_database.KeyExists(listKey))
                _database.KeyExpire(listKey, DateTime.Now.AddMinutes(10));

            _database.SetAdd(listKey, name);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteItem(string name)
        {
            await _database.SetRemoveAsync(listKey, name); 
            return RedirectToAction(nameof(Index));
        }
    }
}
