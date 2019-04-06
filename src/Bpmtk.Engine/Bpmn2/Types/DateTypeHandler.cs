using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Bpmn2.Types
{
    public class DateTypeHandler : ITypeHandler
    {
        public DateTypeHandler()
        {
            this.Name = "date";
            this.ClrType = typeof(DateTime);
        }

        public virtual string Name
        {
            get;
        }

        public virtual Type ClrType
        {
            get;
        }

        public virtual object Parse(string[] array, bool isCollection = false)
        {
            if (array == null || array.Length == 0)
                return null;

            if (!isCollection)
            {
                var text = array[0];
                if (!string.IsNullOrEmpty(text))
                    return DateTime.Parse(text);
            }
            else
            {
                var list = new List<DateTime?>();
                for (var i = 0; i < array.Length; i++)
                {
                    var text = array[0];
                    if (!string.IsNullOrEmpty(text))
                        list[i] = DateTime.Parse(text);
                }

                return list;
            }

            return null;
        }
    }
}
