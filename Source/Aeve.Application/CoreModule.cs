using System;
using Aeve.Application.Dal;
using Aeve.Application.Domain;
using Aeve.Contracts;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Aeve.Application
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

            builder.RegisterType<MovieRepository>().As<IMovieRepository>().SingleInstance()
                .WithParameter(new NamedParameter("connectionString", $"mongodb://{configuration.GetSection("mongo:host").Value}:{configuration.GetSection("mongo:port").Value}"));

            var mapper = new Mapper(ConfigureMapping());
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
        }

        private IConfigurationProvider ConfigureMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<long, DateTime>().ConstructUsing(x => new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(x)));
                cfg.CreateMap<DateTime, long>().ConstructUsing(x => (long) x.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
                cfg.CreateMap<Movie, MovieDto>();
                cfg.CreateMap<Movie, MovieRepository.Movie>();
                cfg.CreateMap<MovieRepository.Movie, Movie>();

                cfg.CreateMap<CreateMovieRequest, Movie>();
            });

            return config;
        }
    }
}
