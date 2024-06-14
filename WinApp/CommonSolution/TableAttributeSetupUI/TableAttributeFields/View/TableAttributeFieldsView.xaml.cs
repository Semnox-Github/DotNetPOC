/********************************************************************************************
 * Project Name - TableAttributeSetupUI
 * Description  - TableAttributeFieldsView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     09-09-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TableAttributeSetupUI
{
    /// <summary>
    /// Interaction logic for TableAttributeFieldsView.xaml
    /// </summary>
    public partial class TableAttributeFieldsView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        public TableAttributeFieldsView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            
            //this.MainGrid.Background = ;
            //Background = new ImageBrush(new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"C:\Users\fiona.lishal\Documents\PaymentSettlement Enhancements\Background Images\theme2-BG.png")));
            //this.MainGrid.Background = new SolidColorBrush(Colors.AliceBlue);
            //FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
            //if(footerView!=null)
            //{
            //    //footerView.Background = new SolidColorBrush(Colors.AliceBlue);
            //    //footerView.MainGrid.Background = new SolidColorBrush(Colors.Red);
            //}
            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }
        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.DataContext != null)
            {
                TableAttributeFieldsVM tableAttributeFieldsVM = this.DataContext as TableAttributeFieldsVM;

                if (tableAttributeFieldsVM != null)
                {
                    ExecutionContext executioncontext = tableAttributeFieldsVM.ExecutionContext;

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);

                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                           
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
