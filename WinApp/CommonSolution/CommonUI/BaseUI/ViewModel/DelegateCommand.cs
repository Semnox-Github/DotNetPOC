/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Delegate command
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class DelegateCommand : ICommand
    {
        #region Members
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public event EventHandler CanExecuteChanged;
        #endregion

        #region Methods
        public DelegateCommand(Action<object> execute)
                           : this(execute, null)
        {
        }

        public DelegateCommand(Action<object> execute,
                       Predicate<object> canExecute)
        {
            log.LogMethodEntry(execute, canExecute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            log.LogMethodEntry(parameter);
            _execute(parameter);
            log.LogMethodExit();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        #endregion

    }
}
