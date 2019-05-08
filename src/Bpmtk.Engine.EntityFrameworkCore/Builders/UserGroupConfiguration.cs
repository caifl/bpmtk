using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Storage.Builders
{
    public class UserGroupConfiguration : EntityTypeConfiguration<UserGroup>
    {
        public override void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey( x => new {
                x.UserId,
                x.GroupId
            });

            builder.Property(x => x.UserId)
                .HasMaxLength(Consts.UserIdLength)
                .IsRequired(true);

            builder.Property(x => x.GroupId)
                .HasMaxLength(Consts.GroupIdLength)
                .IsRequired(true);
        }
    }
}
