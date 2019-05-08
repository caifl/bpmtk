using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ActivityVariableConfiguration : EntityTypeConfiguration<ActivityVariable>
    {
        public override void Configure(EntityTypeBuilder<ActivityVariable> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ByteArray)
                .WithMany()
                .HasForeignKey("ByteArrayId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.LongValue);
            builder.Property(x => x.Text);
            builder.Property(x => x.Text2);
            builder.Property(x => x.DoubleValue);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(128);

            //builder.Property(x => x.IsLocal).HasColumnName("is_local")
            //    .IsRequired();
                //.HasDefaultValue(false);
        }
    }
}
