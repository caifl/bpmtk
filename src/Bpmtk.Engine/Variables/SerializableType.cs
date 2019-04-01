using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bpmtk.Engine.Variables
{
    public class SerializableType : ByteArrayType
    {
        public override string Name => "serializable";

        protected virtual byte[] Serialize(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            using (var ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);

                ms.Position = 0;

                return ms.ToArray();
            }
        }

        protected virtual object Deserialize(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            using (var ms = new MemoryStream(value))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }

        public override object GetValue(IValueFields fields)
        {
            if (fields.CachedValue != null)
                return fields.CachedValue;

            var bytes = base.GetValue(fields) as byte[];

            object value = null;
            if(bytes != null)
                value = this.Deserialize(bytes);

            fields.CachedValue = value;

            return value;
        }

        public override void SetValue(IValueFields fields, object value)
        {
            byte[] bytes = null;
            if (value != null)
                bytes = this.Serialize(value);

            fields.CachedValue = value;

            base.SetValue(fields, bytes);
        }
    }
}
