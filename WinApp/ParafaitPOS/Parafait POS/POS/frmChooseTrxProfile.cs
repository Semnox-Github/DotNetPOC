/********************************************************************************************
 * Project Name - POS
 * Description  - UI Selecting transaction profile
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      28-Nov-2018      Guru S A       booking changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmChooseTrxProfile : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Object TrxProfileId = -1;
        public Object TrxProfileVerify = "N";
        public Object TrxProfileName = "";
        public frmChooseTrxProfile()
        {
            InitializeComponent();
            btnTrxProfileDefault.Text = POSStatic.MessageUtils.getMessage(btnTrxProfileDefault.Text);

            if (initializeTrxProfiles())
            {
                this.ShowDialog();
            }
        }

        bool initializeTrxProfiles()
        {
            log.Debug("Starts-initializeTrxProfiles()");
            DataTable dt = POSStatic.Utilities.executeDataTable(@"select * from trxProfiles where active = 'Y' order by ProfileName");
            if (dt.Rows.Count == 0)
            {
                log.Info("Ends-initializeTrxProfiles() as dt.Rows.Count == 0");
                return false;
            }

            flpTrxProfiles.Controls.Remove(btnTrxProfileDefault);
            foreach (DataRow dr in dt.Rows)
            {
                Button btnTrxProfile = new Button();
                btnTrxProfile.FlatStyle = btnTrxProfileDefault.FlatStyle;
                btnTrxProfile.FlatAppearance.BorderSize = btnTrxProfileDefault.FlatAppearance.BorderSize;
                btnTrxProfile.FlatAppearance.BorderColor = btnTrxProfileDefault.FlatAppearance.BorderColor;
                btnTrxProfile.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnTrxProfile.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnTrxProfile.BackColor = Color.Transparent;
                btnTrxProfile.ForeColor = btnTrxProfileDefault.ForeColor;
                btnTrxProfile.Font = btnTrxProfileDefault.Font;
                btnTrxProfile.BackgroundImage = btnTrxProfileDefault.BackgroundImage;
                btnTrxProfile.BackgroundImageLayout = btnTrxProfileDefault.BackgroundImageLayout;
                btnTrxProfile.Tag = dr["TrxProfileId"];
                btnTrxProfile.Text = dr["ProfileName"].ToString();
                btnTrxProfile.Size = btnTrxProfileDefault.Size;
                btnTrxProfile.Margin = btnTrxProfileDefault.Margin;

                btnTrxProfile.Click += new EventHandler(btnTrxProfile_Click);

                flpTrxProfiles.Controls.Add(btnTrxProfile);
            }

            if (POSStatic.TRX_PROFILE_OPTIONS_MANDATORY == false)
            {
                flpTrxProfiles.Controls.Add(btnTrxProfileDefault);
                btnTrxProfileDefault.Click += new EventHandler(btnTrxProfile_Click);
                btnTrxProfileDefault.Tag = -1;
            }

            flpTrxProfiles.Padding = new Padding(Math.Max(3, (flpOutside.Width - flpTrxProfiles.Width) / 2), Math.Max(3, (flpOutside.Height - flpTrxProfiles.Height) / 2), 0, 0);

            log.Debug("Ends-initializeTrxProfiles()");//Added for logger function on 29-Feb-2016

            return true;
        }

        void btnTrxProfile_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTrxProfile_Click()");//Added for logger function on 08-Mar-2016
            Button b = sender as Button;
            TrxProfileId = b.Tag;
            TrxProfileName = b.Text;
            TrxProfileVerify = POSStatic.Utilities.executeScalar(@"SELECT ISNULL(VerificationRequired,'N') VerificationRequired 
                                                                     FROM TrxProfiles WHERE TrxProfileId = @TrxProfileId",
                                                                     new SqlParameter("@TrxProfileId", TrxProfileId));
            if (TrxProfileVerify == null)
                TrxProfileVerify = "N";
            log.Debug("Ends-btnTrxProfile_Click()");//Added for logger function on 08-Mar-2016
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
