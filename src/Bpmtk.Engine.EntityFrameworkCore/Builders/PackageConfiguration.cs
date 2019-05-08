using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class PackageConfiguration : EntityTypeConfiguration<Package>
    {
        public override void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId).HasMaxLength(Consts.TenantIdLength);
            builder.Property(x => x.OwnerId).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Category).HasMaxLength(Consts.CategoryLength);
            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);
            builder.Property(x => x.Version).IsRequired(true);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .HasMaxLength(Consts.ConcurrencyStampLength)
                //.IsRowVersion()
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Description).HasMaxLength(Consts.DescriptionLength);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.IdentityLinks)
                .WithOne(x => x.Package)
                .HasForeignKey("PackageId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
