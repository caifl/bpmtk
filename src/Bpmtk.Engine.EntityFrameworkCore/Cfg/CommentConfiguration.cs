using System;
using System.Collections.Generic;
using System.Text;
using Bpmtk.Engine.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bpmtk.Engine.Cfg
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public virtual void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.HasOne(x => x.User)
            //    .WithMany()
            //    .HasForeignKey(x => x.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProcessDefinition)
                .WithMany()
                .HasForeignKey("ProcessDefinitionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Task)
                .WithMany()
                .HasForeignKey("TaskId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProcessInstance)
                .WithMany()
                .HasForeignKey("ProcessInstanceId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.UserId).HasMaxLength(32);
            builder.Property(x => x.Body).HasMaxLength(512).IsRequired();

            builder.ApplyNamingStrategy();
        }
    }
}
