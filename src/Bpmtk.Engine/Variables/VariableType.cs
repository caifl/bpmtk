using System;
using System.Collections.Generic;
using System.Linq;

namespace Bpmtk.Engine.Variables
{
    public abstract class VariableType : IVariableType
    {
        private static readonly NullType NullType = new NullType();
        private static readonly IDictionary<string, IVariableType> typeMapping;
        private static readonly List<IVariableType> types = new List<IVariableType>();

        public abstract string Name { get; }

        public virtual bool IsCachable { get => true; }

        static VariableType()
        {
            var defaultTypes = new IVariableType[]
            {
                new StringType(),
                new LongType(),
                new DoubleType(),
                new BooleanType(),
                new DateType(),
                new ByteArrayType(),
                new ListType(),
                new SerializableType()
            };

            types.AddRange(defaultTypes);
            typeMapping = types.ToDictionary(x => x.Name);

            //add NullType.
            typeMapping.Add("null", NullType);
        }
        
        /// <summary>
        /// Find the specified variable type.
        /// </summary>
        public static IVariableType Get(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            IVariableType type = null;
            if (typeMapping.TryGetValue(name, out type))
                return type;

            throw new NotSupportedException($"The variable type '{name}' was not supported.");
        }

        /// <summary>
        /// Find the variable type which can assigned to.
        /// </summary>
        public static IVariableType Resolve(object value, bool throwIfNotFound = true)
        {
            if (value == null)
                return NullType;

            foreach (var type in types)
            {
                if(type.IsAssignableFrom(value))
                    return type;
            }

            if(throwIfNotFound)
                throw new NotSupportedException($"The type '{value.GetType().Name}' was not supported.");

            return null;
        }

        public abstract object GetValue(IValueFields fields);

        public abstract bool IsAssignableFrom(object value);

        public abstract void SetValue(IValueFields fields, object value);
    }
}
