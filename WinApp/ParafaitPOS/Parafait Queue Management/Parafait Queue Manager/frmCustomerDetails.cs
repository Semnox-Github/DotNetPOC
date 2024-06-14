/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - frmCustomerDetails 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        12-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ParafaitQueueManagement
{
    public partial class frmCustomerDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //  Utilities parafaitUtility = new Utilities();
        public frmCustomerDetails()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public void loadCustomerDetails(string cardNo,string customerName,string passPhrase)
        {
            log.LogMethodEntry(cardNo, customerName);
            SqlCommand cmd = Common.Utilities.getCommand();
            

            cmd.CommandText = @"select cu.customer_name name,
                                cu.contact_phone1 as mobile,
                                ca.card_number cardno, cu.PhotoFileName Photo from CustomerView(@PassphraseEnteredByUser) cu join cards ca on 
                                cu.customer_id=ca.customer_id where card_number=@card_number and cu.customer_name=@customer_name";
            cmd.Parameters.AddWithValue("@card_number", cardNo);
            cmd.Parameters.AddWithValue("@customer_name", customerName);
            cmd.Parameters.AddWithValue("@PassphraseEnteredByUser", passPhrase);

            log.Debug("cmd: " + cmd.CommandText);
            log.LogVariableState("@card_number", cardNo);
            log.LogVariableState("@customer_name", customerName);
            log.LogVariableState("@PassphraseEnteredByUser", passPhrase);
            SqlDataAdapter da=new SqlDataAdapter(cmd);
            DataTable dt=new DataTable();
            
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                this.Text = "Customer Details- " + dt.Rows[0]["cardno"].ToString();
                lblCustomerNameValue.Text=dt.Rows[0]["name"].ToString();
                lblMobNoValue.Text=dt.Rows[0]["mobile"].ToString();
                lblCardNoValue.Text=dt.Rows[0]["cardno"].ToString();
                SqlCommand cmdImage = Common.Utilities.getCommand();
                cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";
                cmdImage.Parameters.AddWithValue("@FileName",Common.Utilities.getParafaitDefaults("IMAGE_DIRECTORY") +"\\" + dt.Rows[0]["Photo"].ToString());
                
                 try
                    {
                        object o = cmdImage.ExecuteScalar();
                        picCustomer.Image = Common.Utilities.ConvertToImage(o);
                    }
                    catch
                    {
                        try
                        {
                            SqlCommand cmdSiteLogo = Common.Utilities.getCommand();
                            cmdSiteLogo.CommandText = "select logo from site";
                            object siteImage = cmdSiteLogo.ExecuteScalar();
                            picCustomer.Image = Common.Utilities.ConvertToImage(siteImage);
                        }
                        catch
                        {
                            picCustomer.Image = null;
                        }
                    }
                log.LogMethodExit();
            }

        }

        private void frmCustomerDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
