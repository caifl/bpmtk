using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bpmtk.Engine.Storage.Builders
{
    public class DefaultNamingStrategy : INamingStrategy
    {
        protected string tableNamePrefix = "bpm_";
        protected string tableNameSuffix = null;
        protected Dictionary<string, string> tableAlias = new Dictionary<string, string>();
        protected Dictionary<string, string> columnAlias = new Dictionary<string, string>();

        public DefaultNamingStrategy()
        {
            tableAlias.Add("ProcessDefinition", "proc_def");
            tableAlias.Add("ProcessInstance", "proc_inst");
            tableAlias.Add("ActivityInstance", "act_inst");
            tableAlias.Add("EventSubscription", "event_subscr");
            tableAlias.Add("Variable", "proc_data");
            tableAlias.Add("ActivityVariable", "act_data");
            tableAlias.Add("TaskInstance", "task");
            tableAlias.Add("HistoricPackageItem", "hi_package_item");
            tableAlias.Add("HistoricModel", "hi_model");

            columnAlias.Add("ProcessDefinitionId", "proc_def_id");
            columnAlias.Add("ActivityInstanceId", "act_inst_id");
            columnAlias.Add("ProcessInstanceId", "proc_inst_id");
            columnAlias.Add("SubProcessInstanceId", "sub_proc_inst_id");
            columnAlias.Add("TaskInstanceId", "task_id");
            columnAlias.Add("IsMIRoot", "is_mi_root");
            columnAlias.Add("DoubleValue", "double_val");
            columnAlias.Add("LongValue", "long_val");
        }

        public virtual string GetColumnName(string propertyName)
        {
            if (!columnAlias.ContainsKey(propertyName))
                propertyName = ToLowerCase(propertyName);
            else
                propertyName = columnAlias[propertyName];

            return propertyName;
        }

        public virtual string GetTableName(string entityName)
        {
            if (!tableAlias.ContainsKey(entityName))
                entityName = this.ToLowerCase(entityName);
            else
                entityName = this.tableAlias[entityName];

            if(!string.IsNullOrEmpty(tableNamePrefix))
                entityName = string.Concat(tableNamePrefix, entityName);

            return entityName;
        }

        private string ToLowerCase(string name)
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
