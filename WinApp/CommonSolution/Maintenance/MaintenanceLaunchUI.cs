/********************************************************************************************
 * Class Name - Maintenance                                                                         
 * Description - Maintenance Launch UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Semnox.Parafait.Maintenance
{
    public partial class MaintenanceLaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _Utilities;
        private string connstring = "";
        public MaintenanceLaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.Manager_Flag = "Y";
            _Utilities.ParafaitEnv.SiteId = 1010;
            log.LogMethodExit();
        }

        private void btnMaintTask_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MaintenanceTaskUI MaintTaskUI = new MaintenanceTaskUI(_Utilities);
                MaintTaskUI.ShowDialog();
                MaintTaskUI.Dispose();
            }
            catch (Exception e1)
            {
                log.Error("Error while executing btnMaintTask_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void btnMaintGrp_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MaintenanceGroupUI MaintGroupUI = new MaintenanceGroupUI(_Utilities);
                MaintGroupUI.ShowDialog();
                MaintGroupUI.Dispose();
            }
            catch (Exception e1)
            {
                log.Error("Error while executing btnMaintTask_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void btnAdhoc_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            AdhocJobUI adhocJobUI = new AdhocJobUI(_Utilities);
            adhocJobUI.ShowDialog();
            adhocJobUI.Dispose();
            log.LogMethodExit();
        }


        private void btnJobDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            MaintenanceJobDetailsUI maintenanceJobDetailsUI = new MaintenanceJobDetailsUI(_Utilities);
            maintenanceJobDetailsUI.ShowDialog();
            maintenanceJobDetailsUI.Dispose();
            log.LogMethodExit();
        }

        private void MaintRequest_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            MaintenanceRequests maintenanceRequests = new MaintenanceRequests(_Utilities);
            maintenanceRequests.ShowDialog();
            log.LogMethodExit();
        }

    }
}
