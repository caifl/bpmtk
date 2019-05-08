using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class HistoricPackageItemConfiguration : EntityTypeConfiguration<HistoricPackageItem>
    {
        public override void Configure(EntityTypeBuilder<HistoricPackageItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey("ItemId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(x => x.Content)
                .WithMany()
                .HasForeignKey("ContentId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Property(x => x.UserId).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Created).IsRequired(true);

            builder.Property(x => x.Comment).HasMaxLength(Consts.DescriptionLength);
        }
    }
}
