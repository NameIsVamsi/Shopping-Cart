using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ShoppingApp_DS_VK_JMK_SNA.ViewModels
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Func<T, bool> canExecuteFunction;
        private readonly Action<T> executeAction;

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteFunction = null)
        {
            this.executeAction = executeAction;
            this.canExecuteFunction = canExecuteFunction ?? (parameter => true);
        }
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecuteFunction.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            executeAction?.Invoke((T)parameter);
        }

        #endregion
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action<object> executeAction, Func<object, bool>
            canExecuteFunction = null) : base(executeAction, canExecuteFunction)
        { }
    }
}
