using System;
using System.Threading.Tasks;
using Bpmtk.Engine.Internal;
using Quartz;

namespace Bpmtk.Engine.Scheduler
{
    public class QuartzSchedulerJobHandler : Quartz.IJob
    {
        public virtual async Task Execute(IJobExecutionContext jobExecutionContext)
        {
            var jobDetail = jobExecutionContext.JobDetail;

            var engine = ProcessEngine.GetInstance();

            using (var context = engine.CreateContext())
            {
                var jobManager = context.ScheduledJobManager;

                var key = jobDetail.Key;

                var scheduledJob = await jobManager.FindByKeyAsync(key.Name);
                var handerClass = scheduledJob.Handler;
                if (scheduledJob != null && handerClass != null)
                {
                    try
                    {
                        var type = Type.GetType(handerClass, true);
                        var handler = Activator.CreateInstance(type) as IScheduledJobHandler;
                        if (handler != null)
                            await handler.Execute(context, scheduledJob);
                    }
                    catch (Exception ex)
                    {
                        scheduledJob.Message = ex.Message;
                        scheduledJob.StackTrace = ex.StackTrace;
                    }
                }            
            }
        }
    }
}
