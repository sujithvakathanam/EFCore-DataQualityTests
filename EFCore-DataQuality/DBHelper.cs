using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_DataQuality
{
    public static class DBHelper
    {
        public static string? GetConnectionString(string connectionName = "DefaultConnection")
        {
            var config =  new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = config.GetConnectionString(connectionName);

            return connectionString;
        }

    }
}
