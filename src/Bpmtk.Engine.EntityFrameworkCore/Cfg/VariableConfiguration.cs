using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class VariableConfiguration : IEntityTypeConfiguration<Variable>
    {
        public virtual void Configure(EntityTypeBuilder<Variable> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ByteArray)
                .WithMany()
                .HasForeignKey("ByteArrayId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.LongValue);
            builder.Property(x => x.Text).HasMaxLength(4000);
            builder.Property(x => x.Text2).HasMaxLength(4000);
            builder.Property(x => x.DoubleValue);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(128);

            builder.ApplyNamingStrategy();
        }
    }
}
