using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace UOP.AzureFunctions.StudentRetention.Helpers.Connections
{
    class MongoDBConnection
    {
        private MongoClient _client = null;

        // connection with mongo database  
        public MongoDBConnection()
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("mongoDBConnectionString");

                MongoClientSettings settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );

                settings.SslSettings = new SslSettings()
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };

                settings.RetryWrites = false;

                _client = new MongoClient(settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while connecting to MongoDB. The error is: {ex.Message}");
            }
        }

        // get connection
        public MongoClient GetClient()
        {
            return _client;
        }
    }
}
