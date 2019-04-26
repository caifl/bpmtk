using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bpmtk.Engine.Cfg
{
    public class ActivityInstanceConfiguration : IEntityTypeConfiguration<ActivityInstance>
    {
        public virtual void Configure(EntityTypeBuilder<ActivityInstance> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.HasOne(x => x.ProcessInstance)
            //    .WithMany()
            //    .HasForeignKey("proc_inst_id")
            //    .OnDelete(DeleteBehavior.Cascade);

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
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.IsMIRoot).IsRequired(true).HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Property(x => x.ActivityId).IsRequired(true).HasMaxLength(64);
            builder.Property(x => x.ActivityType).IsRequired(true).HasMaxLength(16);
            builder.Property(x => x.TokenId).IsRequired(true);
            builder.Property(x => x.LastStateTime).IsRequired(true);
            builder.Property(x => x.StartTime);

            builder.Property(x => x.ConcurrencyStamp)
                .HasMaxLength(50)
                .IsConcurrencyToken();

            builder.ApplyNamingStrategy();
        }
    }
}
