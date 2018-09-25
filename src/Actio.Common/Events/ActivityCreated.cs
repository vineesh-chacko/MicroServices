using System;

namespace Actio.Common.Events
{
    public class ActivityCreated : IAuthenticatedEvent
    {
        public string Name { get; }
        public Guid UserId { get; }
        public Guid Id { get; }
        public string Category { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }



        protected ActivityCreated()
        {

        }

        public ActivityCreated(Guid id, Guid userId, string name, string category)//string description, DateTime createdAt)
        {
            UserId = userId;
            Id = id;
            Name = name;
            Category = category;
            // Description = description;
            // CreatedAt = createdAt;

        }
    }
}