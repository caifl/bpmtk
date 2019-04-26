using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public virtual void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.Property(x => x.OwnerId).HasColumnName("owner_id");
            //builder.Property(x => x.CategoryId).HasColumnName("category_id");
            //builder.Property(x => x.OrganizationId).HasColumnName("organization_id");

            //builder.HasMany(x => x.ResourceLinks)
            //    .WithOne(x => x.Package)
            //    .HasForeignKey("package_id")
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Metadata.FindNavigation(nameof(Package.ResourceLinks))
            //    .SetPropertyAccessMode(PropertyAccessMode.Field);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            //builder.HasOne(x => x.Model).WithMany().HasForeignKey("model_id").OnDelete(DeleteBehavior.Restrict);
            //builder.OwnsOne(x => x.Model).OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(x => x.de)
            //builder.Property("ProcessInstanceId").HasColumnName("proc_inst_id");
            //builder.Property("ActivityInstanceId").HasColumnName("act_inst_id");
            //builder.Property(x => x.Name).HasColumnName("name");
            //builder.Property(x => x.IsActive).HasField("isActive").HasColumnName("is_active");
            //builder.Property(x => x.IsMIRoot).HasColumnName("is_mi_root");
            //builder.Property(x => x.ActivityId).HasField("activityId").HasColumnName("activity_id");
            //builder.Ignore(x => x.Node);

            //builder.HasMany(x => x.Children).WithOne(x => x.Parent)
            //    .HasForeignKey("parent_id")
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.ApplyNamingStrategy();
        }
    }
}
