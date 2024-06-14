/********************************************************************************************
* Project Name - Parafait_Kiosk -frmFinishOrder.cs
* Description  - frmFinishOrder 
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
    public partial class frmFinishOrder : BaseForm
    {
        public frmFinishOrder()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void frmTentSelection_Load(object sender, EventArgs e)
        {
            Common.logEnter();
            try
            {
                object screenId = Common.utils.executeScalar(@"select screenId 
                                                        from AppScreens 
                                                        where CodeObjectName = 'frmFinishOrder'");
                if (screenId == null)
                    throw new ApplicationException("Finish Order screen not defined in setup");

                _screenModel = new ScreenModel(Convert.ToInt32(screenId));

                base.RenderPanelContent(_screenModel, panelElements, 1);

                lblScreenTitle.Text = _screenModel.UIPanels[0].Elements[0].Attribute.ActionScreenTitle1;
                lblOrderNumber.Text = _screenModel.UIPanels[0].Elements[0].Attribute.ActionScreenTitle2;

                btnPicture.Left = (panelElements.Width - btnPicture.Width) / 2;

                this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }

            Common.logExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
