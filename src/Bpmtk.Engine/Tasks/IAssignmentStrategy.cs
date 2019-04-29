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
        public virtual async Task ExecuteAsync(ITaskInstanceBuilder builder)
        {
            var context = builder.Context;
            var user = await context.IdentityManager.FindUserByNameAsync("felix");

            builder.SetAssignee(user);
        }
    }
}
