using System;
using System.Collections.Generic;

namespace MysqlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new BpmDesignTimeDbContextFactory();
            var db = factory.CreateDbContext(null);

            Console.WriteLine("Hello World!");
        }
    }
}
