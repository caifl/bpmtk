using System;

namespace Bpmtk.Infrastructure
{
    public interface IRepository
    {
        IAggregateRoot Find(object id);

        void Add(IAggregateRoot entity);

        void Update(IAggregateRoot entity);

        void Remove(IAggregateRoot entity);
    }
}
