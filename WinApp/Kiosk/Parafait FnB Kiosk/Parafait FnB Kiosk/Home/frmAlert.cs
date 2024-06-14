/********************************************************************************************
* Project Name - Parafait_Kiosk -frmAlert.cs
* Description  - frmAlert 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System.Drawing;


namespace Parafait_FnB_Kiosk
{
    public partial class frmAlert : frmMessage
    {
        public frmAlert(string Message) : base(Message)
        {
            log.LogMethodEntry(Message);
            InitializeComponent();

            panelBG.BackgroundImage = Properties.Resources.Subs_Screen_Background;
            lblDisplayText1.ForeColor = Common.PrimaryForeColor;
            lblDisplayText1.Font = new Font(lblDisplayText1.Font.FontFamily, lblDisplayText1.Font.Size + 8);
            log.LogMethodExit();
        }
    }
}
