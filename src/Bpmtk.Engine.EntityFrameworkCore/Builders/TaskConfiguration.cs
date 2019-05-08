using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class TaskConfiguration : EntityTypeConfiguration<TaskInstance>
    {
        public override void Configure(EntityTypeBuilder<TaskInstance> builder)
        {
            builder.HasKey(x => x.Id);

            //proc_def_id
            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Restrict);

            //proc_inst_id
            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey(x => x.ProcessInstanceId)
                .OnDelete(DeleteBehavior.Cascade);

            //act_inst_id
            builder.HasOne(x => x.ActivityInstance)
                .WithMany()
                .HasForeignKey("ActivityInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.DelegationState).IsRequired(false);
            builder.Property(x => x.Assignee).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Owner).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Name).HasMaxLength(Consts.CommonNameLength).IsRequired(true);
            builder.Property(x => x.ClaimedTime);
            builder.Property(x => x.DueDate);
            builder.Property(x => x.LastStateTime).IsRequired(true);
            builder.Property(x => x.ActivityId).HasMaxLength(64);

            //identity-links.
            builder.HasMany(x => x.IdentityLinks)
                .WithOne(x => x.Task)
                .HasForeignKey("TaskId")
                .OnDelete(DeleteBehavior.Cascade);

            //variables.
            builder.HasMany(x => x.Variables)
                .WithOne()
                .HasForeignKey("TaskInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            //token.
            builder.HasOne(x => x.Token)
                .WithMany()
                .HasForeignKey("TokenId")
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

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
