/********************************************************************************************
 * Project Name - Booking
 * Description  - Launch UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019     Deeksha                 Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// Launch UI
    /// </summary>
    public partial class LaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//Added on 26 Feb 2019 by Akshay Gulaganji

        Utilities utilities;
        string connstring = "";
        /// <summary>
        /// 
        /// </summary>
        public LaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            try
            {
                connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                utilities = new Utilities(connstring);
                utilities.ParafaitEnv.User_Id = 3;
                utilities.ParafaitEnv.LoginID = "semnox";
                utilities.ParafaitEnv.SiteId = -1;
                //utilities.ParafaitEnv.SiteId = 1008;
                //utilities.ParafaitEnv.IsCorporate = true;
            }
            catch (Exception e1)
            {
                log.Error("Error while executing LaunchUI()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
               // ReservationDiscountsListUI reservationDiscountsListUI = new ReservationDiscountsListUI(utilities, 10);
                //if (reservationDiscountsListUI.ShowDialog() == DialogResult.OK)
                //{
                //    reservationDiscountsListUI.Close();
                //}
            }
            catch (Exception e1)
            {
                log.Error(e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }
    }
}
