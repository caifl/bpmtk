using Microsoft.Extensions.Logging;

namespace Bpmtk.Engine
{
    public interface IProcessEngine
    {
        ILoggerFactory LoggerFactory
        {
            get;
        }

        IContext CreateContext();
    }
}
