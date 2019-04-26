using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class ByteArrayConfiguration : IEntityTypeConfiguration<ByteArray>
    {
        public virtual void Configure(EntityTypeBuilder<ByteArray> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ApplyNamingStrategy();
        }
    }
}
