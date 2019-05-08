using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ScheduledJobConfiguration : EntityTypeConfiguration<ScheduledJob>
    {
        public override void Configure(EntityTypeBuilder<ScheduledJob> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
                .HasMaxLength(Consts.TenantIdLength);

            //Job key.
            builder.Property(x => x.Key)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();

            //proc_def_id
            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Cascade);

            //activity_id
            builder.Property(x => x.ActivityId)
                .HasMaxLength(64);

            //proc_inst_id
            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            //token_id
            builder.HasOne(x => x.Token)
                .WithMany()
                .HasForeignKey("TokenId")
                .OnDelete(DeleteBehavior.Cascade);

            //task_id
            builder.HasOne(x => x.Task)
                .WithMany()
                .HasForeignKey("TaskId")
                .OnDelete(DeleteBehavior.Cascade);

            //Handler class name.
            builder.Property(x => x.Handler)
                .HasMaxLength(255)
                .IsRequired();

            //Exception Message
            builder.Property(x => x.Message)
                .HasMaxLength(255);

            //Exception stack-trace
            builder.Property(x => x.StackTrace)
                .HasMaxLength(512);

            //JobOptions.
            builder.Property(x => x.Options)
                .HasMaxLength(4000);

            builder.Property(x => x.Created);

            //Indexes.
            builder.HasIndex(x => x.Key).IsUnique();
        }
    }
}
