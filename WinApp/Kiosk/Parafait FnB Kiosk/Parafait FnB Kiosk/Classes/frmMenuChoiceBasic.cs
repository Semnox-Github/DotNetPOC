/********************************************************************************************
* Project Name - Parafait_Kiosk -frmMenuChoiceBasic.cs
* Description  - frmMenuChoiceBasic 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;

namespace Parafait_FnB_Kiosk
{
    public partial class frmMenuChoiceBasic : BaseFormMenuChoice
    {
        public frmMenuChoiceBasic()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void frmMenuChoiceBasic_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                flpChoices.Controls.Remove(panelFiller);

                base.RenderDefaultPanels();
                base.RenderPanelContent(_screenModel, flpChoices, 3);

                flpChoices.Top = panelHeader.Bottom;
                flpChoices.Height = this.Height - panelMenu.Height - panelHeader.Height;

                for (int i = 0; i < flpChoices.Controls.Count; i++)
                {
                    if (flpChoices.Controls[i].Visible == false)
                    {
                        flpChoices.Controls.RemoveAt(i);
                        i = 0;
                    }
                }

                // if last option is a banner, fill the bottom of the panel with filler
                if (flpChoices.Controls[flpChoices.Controls.Count - 1].Width > 1050)
                    flpChoices.Controls.Add(panelFiller);

                UpdateHeader();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }
    }
}
