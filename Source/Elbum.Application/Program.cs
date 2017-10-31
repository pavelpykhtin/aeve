using System;
using System.Threading;
using Autofac;

namespace Elbum.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CommandLine.Parser.Default.ParseArguments<StartOptions>(args);

            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule(options.Value));

            var container = builder.Build();
            var service = container.Resolve<ElbumService>();
            
            var cancelationTokenSource = new CancellationTokenSource();

            var serviceTask = service.Run(cancelationTokenSource.Token);

            Console.ReadLine();

            cancelationTokenSource.Cancel();

            serviceTask.Wait();
        }
    }
}
