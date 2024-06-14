/********************************************************************************************
 * Project Name - Products Availability Attributes
 * Description  - Form to get attributes like remaining quantity, unavailable till etc
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        02-FEB-2019      Nitin Pai      Bear Cat 86-68 Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Product
{
    public partial class ProductsAvailabilityAttributes : Form
    {
        private Utilities Utilities;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int remainingQuantity;
        public DateTime unavailableTillDate;
        public string comments;
        public string productName;

        List<LookupValuesDTO> unavailableTillLookUp;

        public ProductsAvailabilityAttributes()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public ProductsAvailabilityAttributes(Utilities utilities, String productName)
            : this()
        {
            log.LogMethodEntry(utilities);

            this.Utilities = utilities;
            this.productName = productName;
            toolTip1.SetToolTip(txtProductName, productName);
            Utilities.setLanguage(this);

            log.LogMethodExit();
        }

        public ProductsAvailabilityAttributes(Utilities utilities, int remainingQuantity, DateTime unavailableTillDate, string comments, string productName)
            :this(utilities, productName)
        {
            log.LogMethodEntry(utilities);

            this.remainingQuantity = remainingQuantity;
            this.unavailableTillDate = unavailableTillDate;
            this.comments = comments;
            log.LogMethodExit();
        }

        private void ProductsAvailabilityAttributes_Load(object sender, EventArgs e)
        {
            Point newLocation = new Point();
            newLocation.X = this.Location.X;
            newLocation.Y = (int)(0.2* this.Location.Y);
            this.Location = newLocation;

            DateTime temp = unavailableTillDate;
            ProductsAvailabilityBL productsAvailabilityBL = new ProductsAvailabilityBL(Utilities.ExecutionContext);
            unavailableTillLookUp = productsAvailabilityBL.GetUnavailableTillLookupValue();
            cbRemainingTill.ValueMember = "LookupValue";
            cbRemainingTill.DisplayMember = "Description";
            cbRemainingTill.DataSource = unavailableTillLookUp;

            // doing this to avoid loosing user time on cbRemainingTill selection change on load change
            unavailableTillDate = temp;

            if(!String.IsNullOrEmpty(productName))
            {
                txtProductName.Text = productName;
                
            }
            else
            {
                txtProductName.Visible = false;
                lblProdName.Visible = false;
                this.Height = this.Height - txtProductName.Height;
            }

            if (remainingQuantity >= 0)
            {
                txtRemainingQuantity.Text = remainingQuantity.ToString();
            }

            if (!String.IsNullOrEmpty(comments))
            {
                txtHistory.Text = comments;
            }
            else
            {
                lblPreviousComments.Visible = false;
                txtHistory.Visible = false;
                txtComments.Height += txtHistory.Height;
            }

            if (unavailableTillDate != null && unavailableTillDate != DateTime.MinValue)
            { 
                txtUnavailableTill.Text = Convert.ToDateTime(unavailableTillDate).ToString(Utilities.getDateTimeFormat());
                cbRemainingTill.SelectedValue = "-1";
            }
            else
            {
                txtUnavailableTill.Text = Convert.ToDateTime(Utilities.getServerTime().Date.AddDays(1)).ToString(Utilities.getDateTimeFormat());
                cbRemainingTill.SelectedValue= "0";
            }

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if(!int.TryParse(this.txtRemainingQuantity.Text, out remainingQuantity) || remainingQuantity < 0)
            {
                MessageBox.Show(this.Utilities.MessageUtils.getMessage(648));
                txtRemainingQuantity.Focus();
                return;
            }

            int duration = Convert.ToInt32(cbRemainingTill.SelectedValue);
            DateTime unAvTill = Utilities.getServerTime();
            if (duration <= 0)
            {
                unAvTill = unAvTill.Date.AddDays(1).Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 0));
            }
            else
            {
                unAvTill = unAvTill.AddHours(duration);
            }
            this.unavailableTillDate = unAvTill;

            //Regex r = new Regex("^[ A-Za-z0-9_@./#&+-]*$");
            //if (!r.IsMatch(txtComments.Text))
            //{
            //    MessageBox.Show(this.Utilities.MessageUtils.getMessage(985));
            //    txtComments.Focus();
            //    return;
            //}
            //else
            //{
            comments = txtComments.Text;
            //}

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void cbRemainingTill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(String.Equals(cbRemainingTill.SelectedValue.ToString(), "-1"))
            {
                return;
            }
            DateTime unAvTill = Utilities.getServerTime();

            int duration = -1;

            int.TryParse(this.cbRemainingTill.SelectedValue.ToString(), out duration);

            if(duration <= 0)
            {
                unAvTill = unAvTill.Date.AddDays(1); 
            }
            else
            {
                unAvTill = unAvTill.AddHours(duration);
            }

            this.unavailableTillDate = unAvTill;
            txtUnavailableTill.Text = Convert.ToDateTime(unAvTill).ToString(Utilities.getDateTimeFormat()); 

        }

        TextBox currentTextBox;
        void txt_Enter(object sender, EventArgs e)
        {
            currentTextBox = sender as TextBox;
        }

        AlphaNumericKeyPad keypad;
        private void btnKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                keypad.Show();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }
    }
}
