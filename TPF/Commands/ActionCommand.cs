using System;
using System.Windows.Input;

namespace TPF.Controls
{
    public class ActionCommand : ICommand
    {
        public ActionCommand(Action<object> action)
        {
            _execute = action;
        }

        public ActionCommand(Action<object> action, Predicate<object> predicate)
        {
            _execute = action;
            _canExecute = predicate;
        }

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null) return _canExecute(parameter);
            else return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}