using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using NHibernate;
using NHibernate.Cfg;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using NHibernate.Tool.hbm2ddl;
using Xunit.Abstractions;
using Bpmtk.Infrastructure;
using Bpmtk.Engine;
using Bpmtk.Engine.Repository;
using Microsoft.Extensions.DependencyInjection;
using Jint;
using Jint.Native;
using Jint.Runtime.References;

namespace Bpmtk.Engine.Tests
{
    public class ProcessEngineBuilderUnitTest
    {
        protected ITestOutputHelper output;

        public ProcessEngineBuilderUnitTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ScriptEngine()
        {
            var engine = new Jint.Engine(options =>
            {
                options.SetReferencesResolver(new ReferencesHelper());
            });

            var script = @"
function a() { return 'xyz' }

return a();
";

            var result = engine.Execute(script)
                .GetCompletionValue();

            var type = result.Type;

            //var type = result.GetType().Name;
        }

        class Customer
        {
            public virtual string Name
            {
                get;
                set;
            }

            public virtual string Say()
            {
                return "hello";
            }
        }

        class ReferencesHelper : Jint.Runtime.Interop.IReferenceResolver
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();

            public ReferencesHelper()
            {
                this.variables.Add("customer", new Customer() { Name = "felix" });
                this.variables.Add("test", new Func<string, string>((p) =>
                {
                    Console.WriteLine("test invoked.");

                    return p;
                }));
                this.variables.Add("b", "abc");
            }

            public bool CheckCoercible(JsValue value)
            {
                throw new NotImplementedException();
            }

            public bool TryGetCallable(Jint.Engine engine, object callee, out JsValue value)
            {
                value = null;

                //var baseValue = reference.GetBase();

                //var name = reference.GetReferencedName();
                //var isProperty = reference.IsPropertyReference();
                //var isUnresolved = reference.IsUnresolvableReference();
                //var isPrimi = reference.HasPrimitiveBase();

                //value = baseValue.AsObject().Get(name);

                return true;
            }

            public bool TryPropertyReference(Jint.Engine engine, Reference reference, ref JsValue value)
            {
                value = null;

                var baseValue = reference.GetBase();

                var name = reference.GetReferencedName();
                var isProperty = reference.IsPropertyReference();
                var isUnresolved = reference.IsUnresolvableReference();
                var isPrimi = reference.HasPrimitiveBase();

                value = baseValue.AsObject().Get(name);

                return true;
            }

            public bool TryUnresolvableReference(Jint.Engine engine, Reference reference, out JsValue value)
            {
                value = null;

                var name = reference.GetReferencedName();
                var isProperty = reference.IsPropertyReference();
                var isUnresolved = reference.IsUnresolvableReference();
                var isPrimi = reference.HasPrimitiveBase();

                object result = null;
                if(this.variables.TryGetValue(name, out result))
                {
                    value = JsValue.FromObject(engine, result);

                    return true;
                }

                return false;
            }
        }


        [Fact]
        public void BuildProcessEngine()
        {
            IProcessEngineBuilder builder = new ProcessEngineBuilder();
            builder.ConfigureServices(services =>
            {
                //services.Add<IDeploymentManager>()
            });

            var engine = builder.AddDefaultStores(cfg =>
            {
                cfg.SetInterceptor(new XUnitSqlCaptureInterceptor(this.output));
            }).Build();

            var context = engine.CreateContext();

            Context.SetCurrent(context);

            var uow = context.GetService<IUnitOfWork>();

            var rs = context.GetService<IRepositoryService>();

            var rb = rs.CreateDeploymentBuilder();

            var ms = new MemoryStream();
            var stream = this.GetType().Assembly.GetManifestResourceStream("Bpmtk.Engine.Tests.Resources.sequential_flow.bpmn.xml");
            stream.CopyTo(ms);

            var deployment = rb.SetBpmnModel(ms.ToArray())
                .SetName("test")
                .SetMemo("unit-tests")
                .SetCategory("tests")
                .Build();

            var runtimeService = context.GetService<IRuntimeService>();

            var pi = runtimeService.StartProcessInstanceByKey("Process_0cyms8o");

            uow.Commit();

            //var data = rs.GetBpmnModelData(5);

            //var variables = new Dictionary<string, object>();
            //variables.Add("days", 6);

            //var es = context.GetService<IRuntimeService>();

            //var query = es.CreateProcessInstanceQuery();
            //var items = query
            //    .List(10);
            //var count = query.Count();

            //var processInstance = es.StartProcessByKey("leaveRequest", variables);

            context.Dispose();

            //foreach(var name in names)
            //{
            //    cfg.AddAssembly()
            //}
            //.AddFile()
            //cfg = cfg.AddClass(typeof(ExecutionObject))
            //   .AddClass(typeof(ProcessInstance))
            //   .AddClass(typeof(ActivityInstance))
            //   .AddClass(typeof(Token));

            
            //var factory = cfg.BuildSessionFactory();
            //var session = factory.WithOptions().Interceptor(new XUnitSqlCaptureInterceptor(this.output))
            //    .OpenSession();

            //var list = session.Query<ProcessDefinition>().Where(x => x.DeploymentId == 1).ToList();
            //var query = session.CreateQuery("from InstanceIdentityLink where proc_inst_id=1");
            //var list = query.List<InstanceIdentityLink>();
        }

        public class XUnitSqlCaptureInterceptor : EmptyInterceptor
        {
            public XUnitSqlCaptureInterceptor(ITestOutputHelper output)
            {
                this.Output = output;
            }

            public ITestOutputHelper Output { get; set; }

            public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
            {
                var text = sql.ToString();
                this.Output.WriteLine(text);

                return sql;
            }
        }
    }
}
