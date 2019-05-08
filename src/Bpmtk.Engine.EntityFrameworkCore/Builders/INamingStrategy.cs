using System;

namespace Bpmtk.Engine.Storage.Builders
{
    public interface INamingStrategy
    {
        string GetColumnName(string propertyName);

        string GetTableName(string entityName);
    }
}
