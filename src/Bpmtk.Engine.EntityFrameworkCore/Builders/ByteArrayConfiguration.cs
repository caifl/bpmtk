using System;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Storage.Builders
{
    public class ByteArrayConfiguration : EntityTypeConfiguration<ByteArray>
    {
        public override void Configure(EntityTypeBuilder<ByteArray> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
