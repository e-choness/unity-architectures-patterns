namespace FiniteStateMachine.Scripts
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}