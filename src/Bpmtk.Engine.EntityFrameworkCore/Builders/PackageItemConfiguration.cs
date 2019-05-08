using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class PackageItemConfiguration : EntityTypeConfiguration<PackageItem>
    {
        public override void Configure(EntityTypeBuilder<PackageItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Package)
                .WithMany()
                .HasForeignKey("PackageId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(x => x.Content)
                .WithMany()
                .HasForeignKey("ContentId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Property(x => x.UserId).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Type).HasMaxLength(32).IsRequired();
            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);

            //mark concurrency token.
            //builder.Property(x => x.ConcurrencyStamp)
            //    .HasMaxLength(Consts.ConcurrencyStampLength)
            //    //.IsRowVersion()
            //    .IsConcurrencyToken()
            //    .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Description).HasMaxLength(Consts.DescriptionLength);
        }
    }
}
