using Jint.Runtime.Interop;
using System;

namespace Bpmtk.Engine.Scripting
{
    public class JavascriptScope : IScriptingScope
    {
        protected Jint.Engine engine;

        public JavascriptScope(IVariableResolver variableResolver = null)
        {
            this.engine = new Jint.Engine(x =>
            {
                x.Culture(System.Globalization.CultureInfo.CurrentCulture);
                x.AllowClr();

                if (variableResolver != null)
                    x.SetReferencesResolver(new JavascriptReferenceResolver(variableResolver));
                //x.AllowClr(typeof(MailItem).GetTypeInfo().Assembly);
                //x.AddObjectConverter(new EnumsToStringConverter());
                //x.AddObjectConverter(new NegateBoolConverter());
            });
        }

        public virtual Jint.Engine Engine => this.engine;

        public IScriptingScope AddClrType(string name, Type type)
        {
            var typeRef = TypeReference.CreateTypeReference(this.engine, type);
            this.engine.SetValue(name, typeRef);

            return this;
        }

        public IScriptingScope AddClrType(Type type)
        {
            var name = type.GetType().Name;

            var typeRef = TypeReference.CreateTypeReference(this.engine, type);
            this.engine.SetValue(name, typeRef);

            return this;
        }

        public object GetValue(string name)
        {
            return this.engine.GetValue(name)?.ToObject();
        }

        public IScriptingScope SetValue(string name, object value)
        {
            this.engine.SetValue(name, value);

            return this;
        }

        public IScriptingScope SetValue(string name, Delegate value)
        {
            this.engine.SetValue(name, value);

            return this;
        }

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                isDisposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~JavascriptScope()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public virtual IScriptingScope SetVariableResolver(IVariableResolver variableResolver)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
