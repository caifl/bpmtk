using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Xyz : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder();
           // builder.UseLoggerFactory(loggerFactory);
            builder.UseLazyLoadingProxies(true);
            builder.UseMySql("server=localhost;uid=root;pwd=123456;database=bpmtk3");

            return new ApplicationDbContext(builder.Options);
        }
    }
}
