using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskInstance>
    {
        public virtual void Configure(EntityTypeBuilder<TaskInstance> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.HasOne(x => x.ProcessDefinition)
            //    .WithMany()
            //    .HasForeignKey("ProcessDefinitionId")
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey(x => x.ProcessInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ActivityInstance)
                .WithMany()
                .HasForeignKey("ActivityInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.AssigneeId);
            //builder.Property(x => x.OwnerId).HasColumnName("owner_id");
            //builder.Property(x => x.TokenId).HasColumnName("token_id");
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.ClaimedTime);
            builder.Property(x => x.DueDate);
            builder.Property(x => x.LastStateTime).IsRequired(true);
            builder.Property(x => x.ActivityId).HasMaxLength(64);

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAddOrUpdate();

            builder.HasMany(x => x.IdentityLinks)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Variables)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Token)
                .WithMany()
                .HasForeignKey("TokenId")
                .OnDelete(DeleteBehavior.SetNull);

            //builder.HasMany(x => x.Children).WithOne(x => x.Parent)
            //    .HasForeignKey("parent_id")
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.ApplyNamingStrategy();
        }
    }
}
