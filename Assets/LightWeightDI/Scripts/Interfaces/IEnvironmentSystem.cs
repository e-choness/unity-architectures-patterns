namespace DependencyInjection
{
    public interface IEnvironmentSystem
    {
        IEnvironmentSystem ProvideEnvironmentSystem();
        void Initialize();
    }
}