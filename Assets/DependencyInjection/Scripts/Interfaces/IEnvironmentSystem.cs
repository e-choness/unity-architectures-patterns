namespace DependencyInjection.Scripts.Interfaces
{
    public interface IEnvironmentSystem
    {
        IEnvironmentSystem ProvideEnvironmentSystem();
        void Initialize();
    }
}