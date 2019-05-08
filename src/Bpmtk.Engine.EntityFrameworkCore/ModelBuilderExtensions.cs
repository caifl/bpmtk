using System;
using Bpmtk.Engine.Storage.Builders;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyBpmtkModelConfigurations(this ModelBuilder modelBuilder)
        {
            return ApplyBpmtkModelConfigurations(modelBuilder, new DefaultNamingStrategy());
        }

        public static ModelBuilder ApplyBpmtkModelConfigurations(this ModelBuilder modelBuilder,
            INamingStrategy namingStrategy)
        {
            if (namingStrategy == null)
                throw new ArgumentNullException(nameof(namingStrategy));

            var configurations = new IEntityTypeConfiguration[]
            {
                new ByteArrayConfiguration(),
                new IdentityLinkConfiguration(),
                new UserConfiguration(),
                new GroupConfiguration(),
                new UserGroupConfiguration(),
                new PackageConfiguration(),
                new DeploymentConfiguration(),
                new ProcessDefinitionConfiguration(),
                new ProcessInstanceConfiguration(),
                new TokenConfiguration(),
                new ActivityInstanceConfiguration(),
                new ActivityVariableConfiguration(),
                new VariableConfiguration(),
                new TaskConfiguration(),
                new ScheduledJobConfiguration(),
                new EventSubscriptionConfiguration(),
                new CommentConfiguration()
            };

            foreach (var configuration in configurations)
                configuration.Apply(modelBuilder, namingStrategy);

            return modelBuilder;
        }
    }
}
