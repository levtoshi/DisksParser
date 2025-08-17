using System.Windows.Input;
using System.Windows;

namespace DisksParserUI.Commands.BaseCommands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected void OnCanExecutedChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            });
        }

        public virtual void Dispose() { }
    }
}