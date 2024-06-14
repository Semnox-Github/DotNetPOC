/********************************************************************************************
 * Project Name - TagsUI
 * Description  - NotificationTagsView 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.TagsUI
{
    /// <summary>
    /// Interaction logic for NotificationTagsView.xaml
    /// </summary>
    public partial class NotificationTagsView : Window
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        /// <summary>
        /// NotificationTagsView
        /// </summary>
        public NotificationTagsView()
        {
            InitializeComponent();

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            this.ContentRendered += TagView_ContentRendered;
            this.Loaded += frmNotificationTags_Loaded;
        }
 

        private void TagView_ContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);


            if (this.DataContext != null)
            {
                NotificationTagsVM notificationTagsVM = this.DataContext as NotificationTagsVM;

                if (notificationTagsVM != null)
                {
                    ExecutionContext executioncontext = notificationTagsVM.GetExecutionContext();

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void frmNotificationTags_Loaded(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender);
            NotificationTagsView notificationTagsView = sender as NotificationTagsView;
            if (notificationTagsView != null)
            {
                if(notificationTagsView.cbFields.SelectedItem != null)
                {
                    NotificationTagFieldsDTO dto = notificationTagsView.cbFields.SelectedItem as NotificationTagFieldsDTO;
                    if(dto.Key.Equals("IS_IN_STORAGE") || dto.Key.Equals("PING_STATUS") || dto.Key.Equals("SIGNAL_STRENGTH") || dto.Key.Equals("DEVICE_STATUS"))
                    {
                        notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Visible;
                        notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        notificationTagsView.cbListFields.Visibility = System.Windows.Visibility.Hidden;
                        notificationTagsView.txtSearchValue.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                
            }
            log.LogMethodExit();
        }
    }
}
