using System;
using System.Configuration;

namespace FeedAccess
{
    public class Config
    {
        public string ApiUsername { get { return this.GetConfigValue<string>("ApiUsername"); } }
        public string ApiPassword { get { return this.GetConfigValue<string>("ApiPassword"); } }
        public string ApiUrl { get { return this.GetConfigValue<string>("ApiUrl"); } }

        public TValue GetConfigValue<TValue>(string propertyName)
        {
            return (TValue)Convert.ChangeType(ConfigurationManager.AppSettings.Get(propertyName), typeof(TValue));
        }
    }
}
