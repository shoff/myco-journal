namespace Myco.Domain
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;

    public class MycoJsonSource : JsonConfigurationSource
    {
        public MycoJsonSource()
        {
            if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json"))
            {
                throw new Exception($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json not found.");
            }

            this.Path = $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json";
            this.Optional = false;
            this.ReloadOnChange = true;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}