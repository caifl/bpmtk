using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class ScheduledJobConfiguration : IEntityTypeConfiguration<ScheduledJob>
    {
        public virtual void Configure(EntityTypeBuilder<ScheduledJob> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(50).IsRequired();

            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.ApplyNamingStrategy();
        }
    }
}
