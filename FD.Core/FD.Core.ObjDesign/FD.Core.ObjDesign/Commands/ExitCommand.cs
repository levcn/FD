using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ProjectCreater.Commands
{
    public class DelegateCommand : TCommand
    {
        Func<object, bool> CanExec;
        Action<object> Exec;

        public DelegateCommand(Func<object, bool> canExec, Action<object> exec)
        {
            CanExec = canExec;
            Exec = exec;
        }

        protected override bool CanExecuteDetail(object parameter)
        {
            return CanExec(parameter);
        }

        public override void Execute(object parameter)
        {
            Exec(parameter);
        }
    }

    public abstract class TCommand : ICommand
    {

        protected abstract bool CanExecuteDetail(object parameter);

        public void InvalideCanExecute(object parameter = null)
        {
            ((ICommand) this).CanExecute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            var re = CanExecuteDetail(parameter);
            if (re != _CanExecute)
            {
                _CanExecute = re;
                OnCanExecuteChanged();
            }
            return re;
        }

        private bool _CanExecute = false;
        public abstract void Execute(object parameter);


        
        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
