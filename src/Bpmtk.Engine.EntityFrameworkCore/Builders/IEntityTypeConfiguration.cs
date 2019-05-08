using System;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public interface IEntityTypeConfiguration
    {
        void Apply(ModelBuilder modelBuilder, INamingStrategy namingStrategy);
    }
}
