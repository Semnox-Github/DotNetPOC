/********************************************************************************************
 * Project Name - Waiver
 * Description  - WaiverLaunchUI class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish kundar    Modified :Added Logger methods and Removed Unused namespace's. 
 ********************************************************************************************/
using System;
using System.Configuration;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    public partial class WaiverLaunch : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _Utilities;
        private string connstring = "";

        public WaiverLaunch()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);

            _Utilities.ParafaitEnv.User_Id = 69;
            _Utilities.ParafaitEnv.LoginID = "Semnox";

            _Utilities.ParafaitEnv.SiteId = 1011;

            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            if (_Utilities.ParafaitEnv.IsCorporate)
            {
                log.Debug("IsCorporate");
                machineUserContext.SetSiteId(_Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                log.Debug("Set SiteId to -1");
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(_Utilities.ParafaitEnv.LoginID);
            log.LogMethodExit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry("waiverSetUI form Launch");
            //waiverSetUI frm = new waiverSetUI(_Utilities);
            //frm.ShowDialog();
            log.LogMethodExit();
        }
    }
}
