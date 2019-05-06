using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public virtual void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Groups)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name);
            //builder.Property(x => x.UserName)
            //    .HasMaxLength(100);

            //mark concurrency token.
            //builder.Property(x => x.ConcurrencyStamp).HasColumnName("concurrency_stamp")
            //    .IsConcurrencyToken();

            builder.ApplyNamingStrategy();
        }
    }
}
