namespace Myco.Domain
{
    using Microsoft.Extensions.Configuration.Json;

    public class MycoFileConfigurationProvider : JsonConfigurationProvider
    {
        public MycoFileConfigurationProvider() 
            : base(new MycoJsonSource())
        {
        }
    }
}