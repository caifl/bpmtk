using System;

namespace Bpmtk.Engine.Variables
{
    public interface IValueFields
    {
        string Text
        {
            get;
            set;
        }

        string Text2
        {
            get;
            set;
        }

        Byte[] Bytes
        {
            get;
            set;
        }

        long? LongValue
        {
            get;
            set;
        }

        double? DoubleValue
        {
            get;
            set;
        }

        object CachedValue
        {
            get;
            set;
        }
    }
}
