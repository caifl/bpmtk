using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Bpmtk.Engine.Models;

namespace Bpmtk.Engine.Runtime
{
    public class ExecutionContextManager
    {
        protected Context context;
        protected ConcurrentDictionary<long, ExecutionContext> excutionContexts = new ConcurrentDictionary<long, ExecutionContext>();

        public ExecutionContextManager(Context context)
        {
            this.context = context;
        }

        public virtual Context Context => this.context;

        public virtual ExecutionContext Create(Token token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var item = new ExecutionContext(this.context, token);
            if (this.excutionContexts.TryAdd(token.Id, item))
                return item;

            return null;
        }

        public virtual ExecutionContext GetOrCreate(Token token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return this.excutionContexts.GetOrAdd(token.Id, (id) =>
            {
                return new ExecutionContext(this.context, token);
            });
        }

        public virtual bool Remove(Token token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var key = token.Id;
            ExecutionContext executionContext = null;
            return this.excutionContexts.TryRemove(key, out executionContext);
        }

        public virtual bool Remove(ExecutionContext executionContext)
        {
            if (executionContext == null)
                throw new ArgumentNullException(nameof(executionContext));

            var key = executionContext.Token.Id;

            return this.excutionContexts.TryRemove(key, out executionContext);
        }
    }
}
