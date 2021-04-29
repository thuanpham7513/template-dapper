using Microsoft.Extensions.Configuration;

namespace DapperTemplate.Helper
{
    public class DbHelper
    {
        private IConfiguration _configuration;
        public DbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDatabaseConnection()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public string GetRedisConnection()
        {
            return _configuration.GetConnectionString("RedisConnection");
        }
    }
}
