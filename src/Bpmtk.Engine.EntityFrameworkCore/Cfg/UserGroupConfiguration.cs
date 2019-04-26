using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Cfg
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public virtual void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey( x => new {
                x.UserId,
                x.GroupId
            });

            builder.Property(x => x.UserId).IsRequired(true);
            builder.Property(x => x.GroupId).IsRequired(true);

            builder.ApplyNamingStrategy();
        }
    }
}
