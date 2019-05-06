using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class IdentityLinkConfiguration : IEntityTypeConfiguration<IdentityLink>
    {
        public virtual void Configure(EntityTypeBuilder<IdentityLink> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(50);
            builder.Property(x => x.Created).IsRequired(true);

            //builder.HasOne(x => x.User)
            //    .WithMany()
            //    .HasForeignKey("UserId")
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.Group)
            //    .WithMany()
            //    .HasForeignKey("GroupId")
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.ApplyNamingStrategy();
        }
    }
}
