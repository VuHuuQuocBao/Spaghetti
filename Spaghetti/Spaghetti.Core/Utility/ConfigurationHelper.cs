using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Spaghetti.Core.Utility
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }

        #region get config file
        public string GetConnectionString(string connection)
        {
            var connectionString = Resolve(connection);
            return connectionString;
        }

        public string GetFileGroupFolder(string fileGroupFolder)
        {
            var address = _configuration[fileGroupFolder];
            return address;
        }

/*        public T GetConnectionSetting<T>(string connection)
        {
            var setting = Resolve<T>(connection);
            return setting;
        }*/
        #endregion

        public string Resolve(string node)
        {
            string nodeValue = _configuration[node];
            return nodeValue;
        }

      /*  public T Resolve<T>(string node)
        {
            var targetType = typeof(T);
            var instance = (T)targetType.Assembly.CreateInstance(targetType.FullName);
            var targetSection = _configuration.GetSection(node);
            if (targetSection.Exists())
            {
                targetSection.Bind(instance);
            }
            else
            {
                //throw new Exception($"The node {node} is not exist in the configuration file.");
            }
            return instance;
        }*/
    }
}
