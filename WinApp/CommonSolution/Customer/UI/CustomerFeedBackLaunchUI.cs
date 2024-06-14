/********************************************************************************************
 * Project Name - Customer.UI
 * Description  - Class for  of CustomerFeedBackLaunchUI      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Configuration;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomerFeedBackLaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _Utilities;
        private string connstring = "";
        /// <summary>
        /// 
        /// </summary>
        public CustomerFeedBackLaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.SiteId = 1010;
            log.LogMethodExit();
        }

        private void btnCustomerFBQuestion_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender , e);
            try
            {
                CustomerFeedbackQuestionnairUI customerFeedbackQuestionUI = new CustomerFeedbackQuestionnairUI(_Utilities, "", -1, "", "", "");
                customerFeedbackQuestionUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at btnCustomerFBQuestion_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSurvey_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //CustomerFeedbackSurveyUI customerFeedbackSurveyUI = new CustomerFeedbackSurveyUI(_Utilities);
                //customerFeedbackSurveyUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at btnSurvey_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnsurveydetail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerFeedbackSurveyDetailsUI customerFeedbackSurveyDetailsUI = new CustomerFeedbackSurveyDetailsUI(_Utilities);
                customerFeedbackSurveyDetailsUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at btnsurveydetail_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnResponseValue_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerFeedbackResponseValuesUI customerFeedbackSurveyResponseValuesUI = new CustomerFeedbackResponseValuesUI(_Utilities);
                customerFeedbackSurveyResponseValuesUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at btnResponseValue_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerFeedbackResponseUI customerFeedbackResponseUI = new CustomerFeedbackResponseUI(_Utilities);
                customerFeedbackResponseUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at button1_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerFeedbackQuestionUI customerFeedbackQuestionUI = new CustomerFeedbackQuestionUI(_Utilities);
                customerFeedbackQuestionUI.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                log.Error("Error occurred :", ex);
                log.LogMethodExit(null, "Exception at button2_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
