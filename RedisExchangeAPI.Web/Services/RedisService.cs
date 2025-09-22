using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db { get; set; }
        private ConnectionMultiplexer _redis;
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            _redis = ConnectionMultiplexer.Connect($"{_redisHost}:{_redisPort}");  
        }
        public IDatabase GetDb(int db = -1)
        {
            if (_redis == null || !_redis.IsConnected)
            {
                Connect();
            }
            return _redis.GetDatabase(db);
        }   

    }
}
