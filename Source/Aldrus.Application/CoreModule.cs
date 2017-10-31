using Autofac;
using Microsoft.Extensions.Configuration;
using Policies;
using Serializer;

namespace Aldrus.Application
{
    public class CoreModule : Module
    {
        private readonly IConfigurationRoot configuration;

        public CoreModule(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new ZonaMobiClient("https://zona.mobi", new Serializer.Serializer(new UnderScorePropertyNameContractResolver()), HttpPolicies.SimpleRetry));
        }
    }
}