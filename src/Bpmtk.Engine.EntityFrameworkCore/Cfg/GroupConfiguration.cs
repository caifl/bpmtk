using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public virtual void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            //builder.Property(x => x.UserName).HasColumnName("user_name");

            //mark concurrency token.
            //builder.Property(x => x.ConcurrencyStamp).HasColumnName("concurrency_stamp")
            //    .IsConcurrencyToken();

            builder.ApplyNamingStrategy();
        }
    }
}
