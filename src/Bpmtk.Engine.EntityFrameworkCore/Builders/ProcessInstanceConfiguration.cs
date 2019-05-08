using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ProcessInstanceConfiguration : EntityTypeConfiguration<ProcessInstance>
    {
        public override void Configure(EntityTypeBuilder<ProcessInstance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            //identity-links.
            builder.HasMany(x => x.IdentityLinks)
                .WithOne(x => x.ProcessInstance)
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            //Variables
            builder.HasMany(x => x.Variables)
               .WithOne(x => x.ProcessInstance)
               .HasForeignKey("ProcessInstanceId")
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Initiator).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.TenantId).HasMaxLength(Consts.TenantIdLength);
            builder.Property(x => x.Key).HasMaxLength(Consts.ProcessInstanceKeyLength);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(Consts.ProcessInstanceNameLength);
            builder.Property(x => x.LastStateTime).IsRequired(true);
            builder.Property(x => x.StartTime);
            

            builder.HasOne(x => x.Token)
                .WithMany()
                .HasForeignKey("TokenId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Caller)
                .WithMany()
                .HasForeignKey("CallerId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(x => x.Super)
                .WithMany()
                .HasForeignKey("SuperId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Created).IsRequired(true);
            builder.Property(x => x.Modified).IsRequired(true);
            builder.Property(x => x.EndReason).HasMaxLength(Consts.ProcessInstanceEndReasonLength);

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
