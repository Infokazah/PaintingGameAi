using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PaintGameMVVM.Infrastructure.Commands.Base
{
    internal class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public virtual bool CanExecute(object parameter) { throw new NotImplementedException(); }

        public virtual void Execute(object parameter) { throw new NotImplementedException(); }

    }
}
