using System;
using System.Threading.Tasks;

namespace Bpmtk.Engine.Tasks
{
    public interface IAssignmentStrategy
    {
        Task ExecuteAsync(ITaskInstanceBuilder builder);
    }

    public class DefaultAssignmentStrategy : IAssignmentStrategy
    {
        public virtual Task ExecuteAsync(ITaskInstanceBuilder builder)
        {
            var context = builder.Context;
            //var user = context.IdentityManager.FindUserByNameAsync("felix");

            builder.SetAssignee("felix");

            return Task.CompletedTask;
        }
    }
}
