using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpmtk.Engine.Storage.Builders
{
    public class IdentityLinkConfiguration : EntityTypeConfiguration<IdentityLink>
    {
        public override void Configure(EntityTypeBuilder<IdentityLink> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Property(x => x.Type).HasMaxLength(50);
            builder.Property(x => x.Created).IsRequired(true);
        }
    }
}
