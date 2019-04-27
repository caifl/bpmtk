using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bpmtk.Engine.Cfg
{
    public class ProcessDefinitionConfiguration : IEntityTypeConfiguration<ProcessDefinition>
    {
        public virtual void Configure(EntityTypeBuilder<ProcessDefinition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.IdentityLinks)
                .WithOne(x => x.ProcessDefinition)
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Key).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.Version).IsRequired(true);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.DeploymentId).IsRequired(true);
            builder.Property(x => x.Category).HasMaxLength(50);
            builder.Property(x => x.TenantId).HasMaxLength(32);
            builder.Property(x => x.ValidFrom);
            builder.Property(x => x.ValidTo);
            builder.Property(x => x.HasDiagram);
                //.HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Property(x => x.State).IsRequired(true);
            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);
            builder.Property(x => x.VersionTag).HasMaxLength(255);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Description)
                .HasMaxLength(255);

            builder.HasIndex(x => new
            {
                x.Key,
                x.Version
            }).IsUnique(true);

            builder.ApplyNamingStrategy();
        }
    }
}
