using System;
using System.IO;
using System.Linq;
using Xunit;
using NHibernate;
using NHibernate.Cfg;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Runtime;
using NHibernate.Tool.hbm2ddl;
using Xunit.Abstractions;

namespace Bpmtk.Engine.Tests
{
    public class UnitTest1
    {
        protected ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
            
            var assembly = typeof(UnitOfWork).Assembly;
            var names = assembly.GetManifestResourceNames();

            var cfg = new Configuration()
                .Configure("nhibernate.cfg.xml")
                .AddAssembly("Bpmtk.Engine.Hibernate");

            //foreach(var name in names)
            //{
            //    cfg.AddAssembly()
            //}
            //.AddFile()
            //cfg = cfg.AddClass(typeof(ExecutionObject))
            //   .AddClass(typeof(ProcessInstance))
            //   .AddClass(typeof(ActivityInstance))
            //   .AddClass(typeof(Token));

            StreamWriter writer = new StreamWriter(@"C:\Users\Felix\bpmtk_create.sql");
            var export = new SchemaExport(cfg);
            export.Create(x =>
            {
                var sql = x;
                writer.WriteLine(sql);
                writer.Flush();
            }, true);
            //export.Execute(true, true, false);

            writer.Flush();
            writer.Close();
            var factory = cfg.BuildSessionFactory();
            var session = factory.WithOptions().Interceptor(new XUnitSqlCaptureInterceptor(this.output))
                .OpenSession();

            var list = session.Query<Token>().Where(x => x.IsActive).ToList();
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
