/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Main 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ParafaitUtils;
using Parafait_POS;

namespace Parafait
{
    public partial class Main : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Main(string Mode, string LoginId, string Password)
        {
            log.LogMethodEntry();
            //  InitializeComponent();

            Utilities.houseKeeping();
            //showSplash();

            if (Mode == "M") // called from other app
            {
                SqlCommand cmd = ParafaitUtils.Utilities.getCommand();

                cmd.CommandText = "select u.user_id, loginid, username, card_number, " +
                                    "r.role, isnull(r.manager_flag, 'N') manager_flag " +
                                    "from users u, user_roles r " +
                                    "where u.role_id = r.role_id " +
                                    "and u.loginid = @loginid " +
                                    "and u.password = @password";
                cmd.Parameters.AddWithValue("@loginid", LoginId);
                cmd.Parameters.AddWithValue("@password", Password);
                DataTable DT = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    da.Fill(DT);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message, "Get user details");
                    Environment.Exit(0);
                }

                if (DT.Rows.Count == 0)
                {
                    MessageBox.Show("Unable to get user details. User not found.");
                    log.LogMethodExit();
                    Environment.Exit(0);
                }

                ParafaitUtils.ParafaitEnv.Username = DT.Rows[0]["username"].ToString();
                ParafaitUtils.ParafaitEnv.LoginID = DT.Rows[0]["loginid"].ToString();
                ParafaitUtils.ParafaitEnv.Role = DT.Rows[0]["role"].ToString();
                ParafaitUtils.ParafaitEnv.UserCardNumber = DT.Rows[0]["card_number"].ToString();
                ParafaitUtils.ParafaitEnv.Manager_Flag = DT.Rows[0]["manager_flag"].ToString().Trim();
                ParafaitUtils.ParafaitEnv.User_Id = Convert.ToInt32(DT.Rows[0]["user_id"]);
                ParafaitUtils.ParafaitEnv.Password = Password;
            }
            else
            {
               // ParafaitEnv.Initialize();
                if (!Parafait_POS.Authenticate.User())
                {

                    Environment.Exit(0);
                }
                
            }
            ParafaitEnv.Initialize();            
            this.Activate();
            log.LogMethodExit();
        }

       
    }
}