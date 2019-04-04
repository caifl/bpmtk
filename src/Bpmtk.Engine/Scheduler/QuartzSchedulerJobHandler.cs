using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bpmtk.Engine.Internal;
using Bpmtk.Engine.Stores;
using Bpmtk.Infrastructure;
using Quartz;

namespace Bpmtk.Engine.Scheduler
{
    public class QuartzSchedulerJobHandler : Quartz.IJob
    {
        public virtual async Task Execute(IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail;

            var engine = ProcessEngine.GetInstance();

            using (var scopedContext = engine.CreateContext())
            {
                var unitOfWork = scopedContext.GetService<IUnitOfWork>();
                var store = scopedContext.GetService<IScheduledJobStore>();

                var key = jobDetail.Key;

                var scheduledJob = await store.FindByKeyAsync(key.Name);
                var handerClass = scheduledJob.Handler;
                if (scheduledJob != null && handerClass != null)
                {
                    try
                    {
                        var type = Type.GetType(handerClass, true);
                        var handler = Activator.CreateInstance(type) as IScheduledJobHandler;
                        if (handler != null)
                            await handler.Execute(scheduledJob, scopedContext);
                    }
                    catch (Exception ex)
                    {
                        scheduledJob.Message = ex.Message;
                        scheduledJob.StackTrace = ex.StackTrace;
                    }

                    unitOfWork.Commit();
                }            
            }
        }
    }
}
