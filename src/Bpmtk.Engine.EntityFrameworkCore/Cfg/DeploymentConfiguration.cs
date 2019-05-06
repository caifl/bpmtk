using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class DeploymentConfiguration : IEntityTypeConfiguration<Deployment>
    {
        public virtual void Configure(EntityTypeBuilder<Deployment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey("ModelId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Package)
                .WithMany()
                .HasForeignKey("PackageId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ProcessDefinitions)
                .WithOne(x => x.Deployment)
                .HasForeignKey(x => x.DeploymentId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(x => x.User)
            //    .WithMany()
            //    .HasForeignKey("UserId")
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.UserId).HasMaxLength(32);
            builder.Property(x => x.Memo).HasMaxLength(255);
            builder.Property(x => x.Category).HasMaxLength(64);
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Created).IsRequired(true);

            builder.ApplyNamingStrategy();
        }
    }
}
