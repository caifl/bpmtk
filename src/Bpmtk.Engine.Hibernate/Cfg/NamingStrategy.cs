using System;
using System.Collections.Generic;
using System.Text;

namespace Bpmtk.Engine.Cfg
{
    public class NamingStrategy : NHibernate.Cfg.INamingStrategy
    {
        protected Dictionary<string, string> mapping = new Dictionary<string, string>();

        public NamingStrategy()
        {
            //classes
            this.mapping.Add("ProcessDefinition", "proc_def");
            this.mapping.Add("ProcessInstance", "proc_inst");
            this.mapping.Add("ActivityInstance", "act_inst");
            this.mapping.Add("TaskInstance", "task");

            //fields
            this.mapping.Add("ProcessDefinitionId", "proc_def_id");
            this.mapping.Add("ProcessInstanceId", "proc_inst_id");
            this.mapping.Add("ActivityInstanceId", "act_inst_id");
        }

        public virtual string ClassToTableName(string className)
        {
            throw new NotImplementedException();
        }

        public virtual string ColumnName(string columnName)
        {
            throw new NotImplementedException();
        }

        public virtual string LogicalColumnName(string columnName, string propertyName)
        {
            throw new NotImplementedException();
        }

        public virtual string PropertyToColumnName(string propertyName)
        {
            throw new NotImplementedException();
        }

        public virtual string PropertyToTableName(string className, string propertyName)
        {
            throw new NotImplementedException();
        }

        public virtual string TableName(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
