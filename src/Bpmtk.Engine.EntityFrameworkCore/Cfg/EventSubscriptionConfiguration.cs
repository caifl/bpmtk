using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class EventSubscriptionConfiguration : IEntityTypeConfiguration<EventSubscription>
    {
        public virtual void Configure(EntityTypeBuilder<EventSubscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EventName).HasMaxLength(50).IsRequired();

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
