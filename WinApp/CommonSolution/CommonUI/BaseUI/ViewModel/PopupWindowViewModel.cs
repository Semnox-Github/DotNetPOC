using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public class PopupWindowViewModel : ViewModelBase
    {
        #region Members
        private FooterVM footerVM;
        private DeviceClass cardReader;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerVM);
                return footerVM;
            }
            set
            {
                log.LogMethodEntry(footerVM,value);
                SetProperty(ref footerVM, value);
                log.LogMethodExit();
            }
        }
        public DeviceClass CardReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardReader);
                return cardReader;
            }
            set
            {
                log.LogMethodEntry(cardReader, value);
                SetProperty(ref cardReader, value);
                log.LogMethodExit(cardReader);
            }
        }
        #endregion

        #region Constructor
        public PopupWindowViewModel(ExecutionContext executionContext, DeviceClass cardReader)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;
            FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed,
            };
            this.cardReader = cardReader;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        public void ExecuteActionWithLogin(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
                throw new UnauthorizedException("temporary");
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if(activeWindow != null)
                {
                    activeWindow.Close();
                    ShowLogin();
                }
            }
            finally
            {
                log.LogMethodExit();
            }
        }
        private void ShowLogin()
        {
            log.LogMethodEntry();
            AuthenticateUserView userView = new AuthenticateUserView(false);
            AuthenticateUserVM userVM = new AuthenticateUserVM(ExecutionContext, "", "PARAFAIT POS", loginStyle.FullScreen, true, CardReader);
            userView.DataContext = userVM;
            userView.ShowDialog();
            log.LogMethodExit();
        }
        public void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message, messageType);
            if (FooterVM != null)
            {
                FooterVM.Message = message;
                FooterVM.MessageType = messageType;
            }
        }
        public void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    window.Close();
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
