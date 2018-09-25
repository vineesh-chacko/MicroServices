using System;
using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.RabbitMq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RawRabbit;

namespace Actio.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }
        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartUp>(string[] args) where TStartUp : class
        {
            Console.Title = typeof(TStartUp).Namespace;
            var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseStartup<TStartUp>();

            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();

        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _bus;
            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;


            }

            public BusBuilder UseRabbitMq()
            {
                _bus = (IBusClient)_webHost.Services.GetService(typeof(IBusClient));
                return new BusBuilder(_webHost, _bus);
            }
            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {

            private readonly IWebHost _webHost;
            private IBusClient _bus;

            public BusBuilder(IWebHost webHost, IBusClient bus)
            {
                _webHost = webHost;
                _bus = bus;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                var handler = (ICommandHandler<TCommand>)_webHost.Services
                .GetService(typeof(ICommandHandler<TCommand>));
                _bus.WithCommandHandlerAsync(handler);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var handler = (IEventHandler<IEvent>)_webHost.Services
                .GetService(typeof(IEventHandler<IEvent>));
                _bus.WithEventHandlerAsync(handler);

                return this;
            }
            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }
    }
}