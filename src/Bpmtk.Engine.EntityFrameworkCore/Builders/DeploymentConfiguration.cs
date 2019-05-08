using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class DeploymentConfiguration : EntityTypeConfiguration<Deployment>
    {
        public override void Configure(EntityTypeBuilder<Deployment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey("ModelId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ProcessDefinitions)
                .WithOne(x => x.Deployment)
                .HasForeignKey(x => x.DeploymentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Package>()
                .WithMany()
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.TenantId).HasMaxLength(Consts.TenantIdLength);
            builder.Property(x => x.UserId).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Category).HasMaxLength(Consts.CategoryLength);
            builder.Property(x => x.Name).HasMaxLength(Consts.CommonNameLength);
            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Memo).HasMaxLength(Consts.MemoLength);
        }
    }
}
