using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpmtk.Engine.Storage.Builders
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>, IEntityTypeConfiguration
        where TEntity : class
    {
        public virtual void Apply(ModelBuilder modelBuilder, INamingStrategy namingStrategy)
        {
            modelBuilder.ApplyConfiguration(this);

            if (namingStrategy != null)
            {
                var builder = modelBuilder.Entity<TEntity>();
                this.ApplyNamingStrategy(builder, namingStrategy);
            }
        }

        public abstract void Configure(EntityTypeBuilder<TEntity> builder);

        protected virtual void ApplyNamingStrategy(EntityTypeBuilder builder,
            INamingStrategy namingStrategy)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var props = builder.Metadata.GetProperties();
            foreach (var prop in props)
            {
                var name = prop.Name;

                if (namingStrategy != null)
                    name = namingStrategy.GetColumnName(name);

                builder.Property(prop.Name).HasColumnName(name);
            }

            var tableName = builder.Metadata.Name;
            var index = tableName.LastIndexOf('.');
            tableName = tableName.Substring(index + 1);

            if (namingStrategy != null)
                tableName = namingStrategy.GetTableName(tableName);

            builder.ToTable(tableName);
        }
    }
}
