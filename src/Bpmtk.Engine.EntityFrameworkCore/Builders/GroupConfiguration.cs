using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public override void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id).HasMaxLength(Consts.GroupIdLength);
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Created)
                .IsRequired();

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
