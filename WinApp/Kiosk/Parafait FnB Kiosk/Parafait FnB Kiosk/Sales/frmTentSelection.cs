/********************************************************************************************
* Project Name - Parafait_Kiosk -frmTentSelection.cs
* Description  - frmTentSelection 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmTentSelection : BaseForm
    {
        string _tableNumber = "";
        public frmTentSelection()
        {
            log.LogMethodEntry();
            InitializeComponent();
            lblScreenTitle.Text = Common.utils.MessageUtils.getMessage(1118);
            lblOrderNumber.Text = Common.utils.MessageUtils.getMessage(1119);
            log.LogMethodExit();
        }

        private void frmTentSelection_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                object screenId = Common.utils.executeScalar(@"select screenId 
                                                        from AppScreens 
                                                        where CodeObjectName = 'frmTentSelection'");
                if (screenId != null)
                {
                    _screenModel = new ScreenModel(Convert.ToInt32(screenId));
                    base.RenderPanelContent(_screenModel, panelTentNumber, 1);
                }
            }
            catch (Exception ex)
            {
                Common.logException(ex);
            }

            try
            {
                _tableNumber = UserTransaction.OrderDetails.TableNumber;
                displayOrderNumber();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (_tableNumber.Length == 3)
            {
                UserTransaction.OrderDetails.TableNumber = _tableNumber;
                frmCheckout fco = new frmCheckout();
                fco.ShowDialog();
                Close();
            }
            else
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1114));
            log.LogMethodExit();
        }

        private void lblDigit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            _tableNumber += (sender as Label).Text;
            if (_tableNumber.Length > 3)
                _tableNumber = _tableNumber.Substring(1, 3);

            displayOrderNumber();
            log.LogMethodExit();
        }

        private void lblBackspace_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            base.ResetTimeOut();
            if (_tableNumber.Length > 0)
                _tableNumber = _tableNumber.Substring(0, _tableNumber.Length - 1);
            displayOrderNumber();
            log.LogMethodExit();
        }

        void displayOrderNumber()
        {
            log.LogMethodEntry();
            lblOrderDigit1.Text =
                    lblOrderDigit2.Text =
                    lblOrderDigit3.Text = "";

            if (_tableNumber.Length > 0)
            {
                int index = 0;
                while (index < _tableNumber.Length)
                {
                    string digit = _tableNumber[index].ToString();
                    switch (index)
                    {
                        case 0: lblOrderDigit1.Text = digit; break;
                        case 1: lblOrderDigit2.Text = digit; break;
                        case 2: lblOrderDigit3.Text = digit; break;
                    }
                    index++;
                }                
            }
            log.LogMethodExit();
        }
    }
}
