using System.Windows.Input;
using System.Windows;

namespace DisksParserUI.Commands.BaseCommands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void OnCanExecutedChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            });
        }
    }
}