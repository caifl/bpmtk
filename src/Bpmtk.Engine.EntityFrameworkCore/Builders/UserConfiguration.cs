using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            //groups.
            builder.HasMany(x => x.Groups)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //
            builder.Property(x => x.Id).HasMaxLength(Consts.UserIdLength);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(Consts.CommonNameLength);

            builder.Property(x => x.Created)
                .IsRequired();

            //mark concurrency token.
            builder.Property(x => x.ConcurrencyStamp)
                .HasMaxLength(Consts.ConcurrencyStampLength)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
