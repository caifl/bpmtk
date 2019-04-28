namespace Bpmtk.Engine.Storage
{
    public interface IDbSessionFactory
    {
        IDbSession Create();
    }
}
