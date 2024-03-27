using Chat.Framework.EDD;
using Chat.Framework.ORM;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.DDD;

public abstract class RepositoryBaseWrapper<T> : RepositoryBase<T> 
    where T : class, IEntity, IRepositoryItem
{
    protected readonly IEventService EventService;

    protected RepositoryBaseWrapper(DatabaseInfo databaseInfo, IDbContext dbContext, IEventService eventService) 
        : base(databaseInfo, dbContext)
    {
        EventService = eventService;
    }

    public override async Task<bool> SaveAsync(T entity)
    {
        var save = await base.SaveAsync(entity);

        if (!save)
        {
            return save;
        }

        await PublishDomainEvents(entity.DomainEvents);

        return save;
    }

    public override async Task<bool> SaveAsync(List<T> entities)
    {
        var save = await base.SaveAsync(entities);

        if (!save)
        {
            return save;
        }

        var domainEvents = new List<IDomainEvent>();
        
        entities.ForEach(entity => domainEvents.AddRange(entity.DomainEvents));

        await PublishDomainEvents(domainEvents);

        return save;
    }

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        if (!domainEvents.Any()) return;

        var tasks = new List<Task>();

        domainEvents.ForEach(domainEvent =>
        {
            var task = EventService.PublishEventAsync(domainEvent);

            tasks.Add(task);
        });

        if (tasks.Any())
        {
            await Task.WhenAll(tasks);
        }
    }
}
