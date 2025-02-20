namespace Patterns.Commands
{
    public interface ICommand
    {
        public void Execute();
    }

    public interface ICommand<in T>
    {
        public void Execute(T input);
    }
}