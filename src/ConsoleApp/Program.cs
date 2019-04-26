using System;
using System.Linq;
using NHibernate.Cfg;
//using Microsoft.EntityFrameworkCore;
using Bpmtk.Engine;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {


            //var cfg = new Configuration();

            //cfg.AddFile("ProcessInstance.hbm.xml")
            //    .AddFile("Token.hbm.xml")
            //    .AddFile("ActivityInstance.hbm.xml")
            //    .AddFile("ActivityVariable.hbm.xml")
            //    .AddFile("Variable.hbm.xml");
            //cfg.Configure("nhibernate.cfg.xml");
            ////cfg.AddAssembly(typeof(Program).Assembly)

            //var sessionFactory = cfg.BuildSessionFactory();

            //var session = sessionFactory.OpenSession();

            //var act = new ActivityInstance();
            //act.Name = "step_1";
            //act.Variables = new List<ActivityVariable>();
            //act.Variables.Add(new ActivityVariable() { Name = "test" });

            //session.Save(act);
            //session.Flush();

            //var items = session.Query<ActivityVariable>().ToList();

            //SchemaExport exporter = new SchemaExport(cfg);
            //exporter.Execute(sql =>
            //{
            //    Console.WriteLine(sql);

            //}, true, false);
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();

            var sessionFactory = new DbSessionFactory();
            sessionFactory.Configure(builder =>
            {
                builder.UseLoggerFactory(loggerFactory);
                builder.UseLazyLoadingProxies(true);
                builder.UseMySql("server=localhost;uid=root;pwd=123456;database=bpmtk2");
            });

            var engine = new ProcessEngineBuilder()
                .SetDbSessionFactory(sessionFactory)
                .SetLoggerFactory(loggerFactory)
                .Build();

            var context = engine.CreateContext();
            var query = context.DeploymentManager.Deployments;

            var list = query.ToList();

            foreach (var item in list)
            {
                var model = item.Model;

                var procDefList = item.ProcessDefinitions;
                foreach(var procDef in procDefList)
                {
                    var identityLinks = procDef.IdentityLinks;
                }
            }

            //var variable = db.ActivityVariables.ToArray().First(); // (3L);
            //variable.ByteArray = new ByteArray() { Value = new byte[] { 5, 1, 2, 3 } };

            //var act = new ActivityInstance();
            //act.Name = "new-act";
            //act.Variables = new List<ActivityVariable>();

            //var variable = new ActivityVariable();
            //variable.Name = "var-1";
            //variable.ByteArray = new ByteArray() { Value = new byte[] { 0, 1, 2, 3 } };
            //act.Variables.Add(variable);

            //db.ActivityInstances.Add(act);

            //var items = db.ActivityInstances.ToList();
            //db.ActivityInstances.RemoveRange(items);

            //var vars= db.ActivityVariables.ToList();

            var tokens = context.RuntimeManager.Tokens.ToList();

            foreach(var token in tokens)
            {
                var variables = token.Variables;
                foreach(var item in variables)
                {

                }
            }

            var tasks = context.TaskManager.Tasks.ToList();
            foreach(var task in tasks)
            {
                var identityLinks = task.IdentityLinks;
                var variables = task.Variables;
            }

            var users = context.IdentityManager.Users.ToList();
            foreach (var u in users)
            {
                var g = u.Groups;
            }

            var eventSubscriptions = context.DeploymentManager
                .GetEventSubscriptionsAsync(0)
                .Result;

            var jobs = context.DeploymentManager
                .GetScheduledJobsAsync(0)
                .Result;

            var procInst = context.RuntimeManager.FindAsync(1).Result;
            var super = procInst.Super;

            var hi = context.HistoryManager.ActivityInstances.ToList();

            var bpmnModel = context.DeploymentManager.GetBpmnModelAsync(1).Result;

            try
            {
                var tx = context.BeginTransaction();

                //var task = context.TaskManager.FindTaskAsync(1);
                context.TaskManager.CompleteAsync(1).GetAwaiter().GetResult();

                tx.Commit();
            }
            catch(Exception ex)
            {

            }

            //var procInstList = context.RuntimeManager
            //    .ProcessInstances
            //    .ToList();


            //foreach (var procInst in procInsts)
            //{
            //    var vars = procInst.Variables;
            //    foreach (var item in vars)
            //    {
            //        //if(item.ByteArrayId != null)
            //        //{
            //        //    item.ByteArray = new ByteArray() { Value = new byte[] { 0, 1, 2, 3 }, Id = item.ByteArrayId.Value };
            //        //}
            //        //else
            //            //item.ByteArray = new ByteArray() { Value = new byte[] { 0, 1, 2, 3 } };

            //    }

            //   // Console.WriteLine(vars.Count);
            //}

            //db.SaveChanges();

            //var procInst = new ProcessInstance();
            //procInst.Variables.Add(new Variable() { Name = "pi_var" });

            //var token = new Token() { ProcessInstance = procInst, IsActive = true };
            //token.Children.Add(new Token() { IsActive = true, ProcessInstance = procInst });

            //procInst.Tokens.Add(token);


            //token.Variables.Add(new Variable() { Name = "token_var" });

            //db.Add(procInst);
            //db.SaveChanges();

            //var item = db.ProcessInstances.Where(x => x.Id == 3)
            //    .SingleOrDefault();

            //item.Variables.Add(new Variable() { Name = "xyz" });
            //db.SaveChanges();


            Console.WriteLine("Hello World!");
        }
    }
}
