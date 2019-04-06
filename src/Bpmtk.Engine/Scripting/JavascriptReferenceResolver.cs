using Jint.Native;
using Jint.Runtime.References;
using System;
using System.Collections.Generic;

namespace Bpmtk.Engine.Scripting
{
    class JavascriptReferenceResolver : Jint.Runtime.Interop.IReferenceResolver
    {
        private readonly IVariableResolver variableResolver;

        public JavascriptReferenceResolver(IVariableResolver variableResolver)
        {
            this.variableResolver = variableResolver;
        }

        public bool CheckCoercible(JsValue value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetCallable(Jint.Engine engine, object callee, out JsValue value)
        {
            throw new NotImplementedException();
        }

        public bool TryPropertyReference(Jint.Engine engine, Reference reference, ref JsValue value)
        {
            value = null;

            var baseValue = reference.GetBase();

            var name = reference.GetReferencedName();
            var isProperty = reference.IsPropertyReference();
            var isUnresolved = reference.IsUnresolvableReference();
            var isPrimi = reference.HasPrimitiveBase();

            value = baseValue.AsObject().Get(name);

            return true;
        }

        public bool TryUnresolvableReference(Jint.Engine engine, Reference reference, out JsValue value)
        {
            value = null;

            var name = reference.GetReferencedName();
            var isProperty = reference.IsPropertyReference();
            var isUnresolved = reference.IsUnresolvableReference();
            var isPrimi = reference.HasPrimitiveBase();

            object result = null;
            if (this.variableResolver.Resolve(name, out result))
            {
                value = JsValue.FromObject(engine, result);

                return true;
            }

            return false;
        }
    }
}
