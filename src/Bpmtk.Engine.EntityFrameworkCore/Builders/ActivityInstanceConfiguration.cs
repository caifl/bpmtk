using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ActivityInstanceConfiguration : EntityTypeConfiguration<ActivityInstance>
    {
        public override void Configure(EntityTypeBuilder<ActivityInstance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            //Resource links.
            builder.HasMany(x => x.IdentityLinks)
                .WithOne()
                .HasForeignKey("ActivityInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            //Variables
            builder.HasMany(x => x.Variables)
               .WithOne()
               .HasForeignKey("ActivityInstanceId")
               .OnDelete(DeleteBehavior.Cascade);

            //Children
            builder.HasMany(x => x.Children)
               .WithOne(x => x.Parent)
               .HasForeignKey("ParentId")
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SubProcessInstance)
                .WithMany()
                .HasForeignKey("SubProcessInstanceId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(Consts.CommonNameLength);
            builder.Property(x => x.IsMIRoot).IsRequired(true);
                //.HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Property(x => x.ActivityId).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.ActivityType).IsRequired(true).HasMaxLength(32);
            builder.Property(x => x.TokenId).IsRequired(true);
            builder.Property(x => x.LastStateTime).IsRequired(true);
            builder.Property(x => x.StartTime);

            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .HasMaxLength(Consts.ConcurrencyStampLength)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.Description)
                .HasMaxLength(Consts.DescriptionLength);
        }
    }
}
