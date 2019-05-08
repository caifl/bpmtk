using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class EventSubscriptionConfiguration : EntityTypeConfiguration<EventSubscription>
    {
        public override void Configure(EntityTypeBuilder<EventSubscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
                .HasMaxLength(Consts.TenantIdLength);

            builder.Property(x => x.EventType)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.EventName)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Token)
                .WithMany()
                .HasForeignKey("TokenId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ActivityId)
                .HasMaxLength(64);

            builder.Property(x => x.Created)
                .IsRequired();
        }
    }
}
