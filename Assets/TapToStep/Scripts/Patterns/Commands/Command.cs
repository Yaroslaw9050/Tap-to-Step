using System;

namespace Patterns.Commands
{
    public class Command: ICommand
    {
        private readonly Action r_execute;

        public Command(Action execute)
        {
            r_execute = execute;
        }
        
        public void Execute() => r_execute?.Invoke();
    }

    public class Command<T> : ICommand<T>
    {
        private readonly Action<T> r_execute;

        public Command(Action<T> execute)
        {
            r_execute = execute;
        }

        public void Execute(T input) => r_execute?.Invoke(input);
    }
}