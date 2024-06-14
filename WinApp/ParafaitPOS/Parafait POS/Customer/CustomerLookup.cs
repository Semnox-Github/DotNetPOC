/********************************************************************************************
 * Project Name - POS Static Class
 * Description  -  A class for CustomerLookup
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.80         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public partial class CustomerLookup : Form
    {
        public int SelectedCustomerId = -1;

        bool _exact;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        //Modified on March 8 2017 for adding wechatID
        string wechatId = string.Empty;
        public void showForm(string name, string email, string phone, string uid, bool exact = false)
        {
            log.LogMethodEntry(name, email, phone, uid, exact);
            showForm(name, email, phone, uid, "", exact);
            log.LogMethodExit();
        }

        public void showForm(string name, string email, string phone, string uid, string wechatId, bool exact = false)
        {
            log.LogMethodEntry( name , email ,phone , uid , exact );//Added for logger function on 08-Mar-2016
            txtName.Text = name;
            txtEmail.Text = email;
            txtPhone.Text = phone;
            txtUID.Text = uid;
            _exact = exact;
            this.wechatId = wechatId;
            log.LogMethodExit();
        }

        public CustomerLookup()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016
            log.LogMethodEntry();
            POSStatic.Utilities.setLanguage();
            InitializeComponent();

            SqlCommand cmd = POSStatic.Utilities.getCommand();
            //cmd.CommandText = "select cardTypeId, CardType + isnull(' - ' + site_name, '') cardType from cardType left outer join roamingSites on RoamingSiteId = site_id union all select -1, ' -All-' order by 2";
            cmd.CommandText = "select MembershipId, MembershipName + isnull(' - ' + site_name, '') MembershipName from Membership left outer join roamingSites on RoamingSiteId = site_id union all select -1, ' -All-' order by 2";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbMembership.DataSource = dt;
            //cmbMembership.ValueMember = "CardTypeId";
            //cmbMembership.DisplayMember = "CardType";
            cmbMembership.ValueMember = "MembershipId";
            cmbMembership.DisplayMember = "MembershipName";

            POSStatic.Utilities.setLanguage(this);

            search();
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
           
            //Modified on March 8 2017 for adding wechatID
            this.wechatId = string.Empty;
            //end

            log.LogMethodEntry();
            search();
            log.LogMethodExit();
        }

        //Modified on March 8 2017 for adding wechatID
        public int search()
            {
            log.LogMethodEntry();
            SqlCommand cmd = POSStatic.Utilities.getCommand();
            
            string encryptedPassPhrase = POSStatic.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;

            cmd.CommandText = @"select top 20 case when c.valid_flag = 'Y' then c.card_number else c.card_number + '[Inactive]' end card_number,
                                    --ct.cardType Membership, 
                                    m.MembershipName Membership,
                                    c.vip_customer as ""VIP?"", 
                                    cu.customer_id,cu.customer_name,cu.city,cu.state,cu.country,cu.pin,cu.gender,cu.notes,Title,company,cu.middle_name,
                                    cu.last_name,cu.last_updated_date,cu.last_updated_user,cu.password,
                                    cu.Designation,cu.PhotoFileName,cu.ExternalSystemRef,cu.CustomDataSetId,cu.TeamUser,cu.Verified,cu.Username,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address1Encrypted)) as address1,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address2Encrypted)) as address2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address3Encrypted)) as address3,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, emailEncrypted)) as email,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, WeChatAccessTokenEncrypted)) as WeChatAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, IDProofFileNameEncrypted)) as IDProofFileName,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,birthDateEncrypted),121) as birth_date,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,anniversaryEncrypted),121) as anniversary,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)) as contact_phone1 ,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)) as contact_phone2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBUserIdEncrypted)) as FBUserId,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBAccessTokenEncrypted)) as FBAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessSecretEncrypted)) as TWAccessSecret,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessTokenEncrypted)) as TWAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TaxCodeEncrypted)) as TaxCode, 
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,UniqueIDEncrypted)) as Unique_ID 
                                from customers cu left outer join cards c
                                    on cu.customer_id = c.customer_Id
                                 -- left outer join cardType ct on ct.CardTypeId = c.cardTypeId
                                    left outer join Membership m on m.MembershipId = cu.MembershipId
                                where (customer_name like @name
                                    or middle_name like @name
                                    or last_name like @name)
                                and isnull(convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,emailEncrypted)), '%') like @email
                                and isnull(convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,WeChatAccessTokenEncrypted)), '%') like @wechatId
                                and (isnull(convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)), '%') like @phone
                                    or isnull(convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)), '%') like @phone)
                                and (unique_id like @uid or ('%' like @uid and unique_id is null))
                                --and (@cardTypeId = -1 or c.CardTypeId = @cardTypeId)
                                and (@membershipId = -1 or m.MembershipId = @membershipId)
                                order by customer_name";
            string pad = _exact ? "" : "%";
            cmd.Parameters.AddWithValue("@name", "%" + txtName.Text + "%");
            cmd.Parameters.AddWithValue("@email", pad + (string.IsNullOrEmpty(txtEmail.Text) ? "%" : txtEmail.Text) + pad);
            cmd.Parameters.AddWithValue("@phone", pad + (string.IsNullOrEmpty(txtPhone.Text) ? "%" : txtPhone.Text) + pad);
            cmd.Parameters.AddWithValue("@uid", "%" + txtUID.Text.Trim() + "%");
            cmd.Parameters.AddWithValue("@wechatId", "%" + wechatId.Trim() + "%");
            //cmd.Parameters.AddWithValue("@cardTypeId", cmbMembership.SelectedValue);
            cmd.Parameters.AddWithValue("@membershipId", cmbMembership.SelectedValue);
            cmd.Parameters.AddWithValue("@PassphraseEnteredByUser", passPhrase);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-search() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            dgvCustomers.DataSource = dt;
            if (dgvCustomers.BackgroundColor != this.BackColor)
            {
                POSStatic.Utilities.setupDataGridProperties(ref dgvCustomers);
                dgvCustomers.BackgroundColor = this.BackColor;

                SelectCustomer.DisplayIndex = 0;
                dgvCustomers.Columns["customer_name"].Frozen = true;
            }
            POSStatic.Utilities.setLanguage(dgvCustomers);
            log.LogMethodExit(dt.Rows.Count);
            return dt.Rows.Count;
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex < 0)
                {
                    log.Info("Ends-displayCardDetails() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
                if (e.ColumnIndex == 0)
                {
                    SelectedCustomerId = Convert.ToInt32(dgvCustomers["customer_id", e.RowIndex].Value);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
               
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-dgvCustomers_CellContentClick() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void CustomerLookup_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbMembership.SelectedValue = -1;
            search();
            log.LogMethodExit();
        }

        public bool VerifyExisting(int custId)
        {
            log.LogMethodEntry(custId);//Added for logger function on 08-Mar-2016
            int count = search();
            if (count == 1 && custId.Equals(Convert.ToInt32(dgvCustomers["customer_id", 0].Value)))
            { 
		        log.Info("Ends-VerifyExisting(" + custId + ") as custId == dgvCustomers[customer_id, 0].Value");//Added for logger function on 08-Mar-2016
		        return false;
            } 

            if (count > 0)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(524), "Validate Phone / Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (this.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    log.Info("Ends-VerifyExisting(" + custId + ") as Validate Phone / Email dialog OK was Clicked");//Added for logger function on 08-Mar-2016
                    return true;
                }
                else
                {                    
                    //Added to clear the entered customer details
                    if (POSStatic.Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y"))
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Duplicate customer is not allowed"));
                        SelectedCustomerId = -1;
                        return true;
                    }

                    log.Info("Ends-VerifyExisting(" + custId + ") as Validate Phone / Email dialog was Closed");//Added for logger function on 08-Mar-2016
                    return false;
                }
            }

            log.LogMethodExit(false);
            return false;
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex < 0)
                {
                    log.Info("Ends-dgvCustomers_CellDoubleClick() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
                SelectedCustomerId = Convert.ToInt32(dgvCustomers["customer_id", e.RowIndex].Value);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-dgvCustomers_CellDoubleClick() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }
    }
}
