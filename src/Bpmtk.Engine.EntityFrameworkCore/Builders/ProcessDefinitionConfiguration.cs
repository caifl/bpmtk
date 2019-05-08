using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ProcessDefinitionConfiguration : EntityTypeConfiguration<ProcessDefinition>
    {
        public override void Configure(EntityTypeBuilder<ProcessDefinition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.IdentityLinks)
                .WithOne(x => x.ProcessDefinition)
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Key).IsRequired(true).HasMaxLength(Consts.ProcessInstanceKeyLength);
            builder.Property(x => x.Version).IsRequired(true);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(Consts.ProcessDefinitionNameLength);
            builder.Property(x => x.DeploymentId).IsRequired(true);
            builder.Property(x => x.Category).HasMaxLength(Consts.CategoryLength);
            builder.Property(x => x.TenantId).HasMaxLength(Consts.TenantIdLength);
            builder.Property(x => x.ValidFrom);
            builder.Property(x => x.ValidTo);
            builder.Property(x => x.HasDiagram).IsRequired(true);
                //.HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Property(x => x.State).IsRequired(true);
            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);
            builder.Property(x => x.VersionTag).HasMaxLength(Consts.ProcessDefinitionVersionTagLength);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .HasMaxLength(Consts.ConcurrencyStampLength)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Description)
                .HasMaxLength(Consts.DescriptionLength);

            builder.HasIndex(x => new
            {
                x.Key,
                x.Version
            }).IsUnique(true);
        }
    }
}
