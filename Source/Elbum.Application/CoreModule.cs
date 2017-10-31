using System;
using System.Net;
using Autofac;
using Serializer;

namespace Elbum.Application
{
    public class CoreModule: Module
    {
        private readonly StartOptions options;

        public CoreModule(StartOptions options)
        {
            this.options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            if (!string.IsNullOrEmpty(options.Proxy))
            {
                builder.RegisterInstance(new WebProxy(new Uri(options.Proxy)));
            }
            else
            {
                builder.RegisterInstance<WebProxy>(null);
            }

            builder.Register(c => new ElbumService(new Serializer.Serializer(new UnderScorePropertyNameContractResolver()), c.Resolve<IAeveClient>())).AsSelf();
            builder.Register(x => new AeveClient(options.Aeve, new Serializer.Serializer(), x.Resolve<WebProxy>())).AsImplementedInterfaces();
            //builder.RegisterInstance(new AeveClient("http://docker-test-107h8q8b.cloudapp.net:50001", new Serializer())).AsImplementedInterfaces();
        }
    }
}