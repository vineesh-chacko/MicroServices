using System;
using System.Threading.Tasks;
using Actio.Common.Events;

namespace Actio.Api.Handlers
{
    public class ActivitiyCreatedHandler : IEventHandler<ActivityCreated>
    {
        public async Task HandeAsync(ActivityCreated @event)
        {
            await Task.CompletedTask;
            Console.WriteLine($"Activity created:{@event.Name}");
        }
    }
}