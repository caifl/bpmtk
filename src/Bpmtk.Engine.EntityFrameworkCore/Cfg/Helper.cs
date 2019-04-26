using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bpmtk.Engine.Cfg
{
    static class Helper
    {
        private const string tableNamePrefix = "bpm_";
        private readonly static Dictionary<string, string> tableAlias = new Dictionary<string, string>();
        private readonly static Dictionary<string, string> columnAlias = new Dictionary<string, string>();

        static Helper()
        {
            tableAlias.Add("ProcessDefinition", "proc_def");
            tableAlias.Add("ProcessInstance", "proc_inst");
            tableAlias.Add("ActivityInstance", "act_inst");
            tableAlias.Add("EventSubscription", "event_subscr");
            tableAlias.Add("Variable", "proc_data");
            tableAlias.Add("ActivityVariable", "act_data");
            tableAlias.Add("TaskInstance", "task");

            columnAlias.Add("ProcessDefinitionId", "proc_def_id");
            columnAlias.Add("ActivityInstanceId", "act_inst_id");
            columnAlias.Add("ProcessInstanceId", "proc_inst_id");
            columnAlias.Add("SubProcessInstanceId", "sub_proc_inst_id");
            columnAlias.Add("TaskInstanceId", "task_id");
            columnAlias.Add("IsMIRoot", "is_mi_root");
            columnAlias.Add("DoubleValue", "double_val");
            columnAlias.Add("LongValue", "long_val");
        }

        public static void ApplyNamingStrategy(this EntityTypeBuilder builder)
        {
            var props = builder.Metadata.GetProperties();
            foreach (var prop in props)
            {
                var name = prop.Name;
                
                if (!columnAlias.ContainsKey(name))
                    name = ToLowerCase(name);
                else
                    name = columnAlias[name];

                builder.Property(prop.Name).HasColumnName(name);
            }

            var tableName = builder.Metadata.Name;
            var index = tableName.LastIndexOf('.');
            tableName = tableName.Substring(index + 1);

            if (!tableAlias.ContainsKey(tableName))
                tableName = ToLowerCase(tableName);
            else
                tableName = tableAlias[tableName];

            builder.ToTable(string.Concat(tableNamePrefix, tableName));
        }

        //public static void ApplyNamingStrategy(this PropertyBuilder builder)
        //{
        //    var name = builder.Metadata.Name;
        //    name = ToLowerCase(name);

        //    builder.HasColumnName(name);
        //}

        public static string ToLowerCase(string name)
        {
            return Regex.Replace(name, ".[A-Z]", new MatchEvaluator((m) =>
            {
                char ch = m.Value[0];
                var ch1 = m.Value[1];
                return (((char)ch).ToString() + "_" + ((char)ch1).ToString());
            })).ToLower();
        }
    }
}
