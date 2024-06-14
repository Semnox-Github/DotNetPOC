using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Device;
using System.Threading;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty;

namespace Parafait_POS.Redemption
{
    public partial class frmCapillaryRedemption : Form
    {
        public string couponType = string.Empty;
        public double couponValue = -1;
        public string couponNumber = string.Empty;
        public int loyaltyPoints;
        public double appliedPoints = 0;
        public string validationCode = string.Empty;
        public string redemptionType = string.Empty;
        string CustomerPhoneNo;
        public bool redemptionApplied = false;
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        LoyaltyRedemptionBL objRedemptionBL = new LoyaltyRedemptionBL();
        LoyaltyRedemptionDTO objRedemptionDTO = new LoyaltyRedemptionDTO();

        public frmCapillaryRedemption()
        {
            log.Debug("starts-frmMainRedemption()");//Added for logger function on 01-July-2016
            InitializeComponent();
            log.Debug("Ends-frmMainRedemption()");//Added for logger function on 01-July-2016
        }


        public frmCapillaryRedemption(string phoneNo, int points, bool isCouponApplied, bool isPointsApplied)
        {
            log.Debug("starts-frmMainRedemption(phoneNo,points,isCouponApplied,isPointsApplied)");//Added for logger function on 01-July-2016
            InitializeComponent();
            CustomerPhoneNo = phoneNo;
            loyaltyPoints = points;
            DisableAllControls();     
            grpCouponPayment.Enabled = isCouponApplied;
            grpPointsPayment.Enabled = isPointsApplied;
            this.MaximizeBox = false;
            log.Debug("Ends-frmMainRedemption(phoneNo,points,isCouponApplied,isPointsApplied)");//Added for logger function on 01-July-2016
        }

        void DisableAllControls()
        {
            log.Debug("starts-DisableAllControls()");//Added for logger function on 01-July-2016
            txtCouponNumber.Enabled = false;
            txtPoints.Enabled = false;
            txtValidationCode.Enabled = false;
            rdbtnCoupon.Enabled = false;
            rdbtnPoints.Enabled = false;
            rdbtnCoupon.Checked = false;
            rdbtnPoints.Checked = false;
            lblAvailablePoints.Visible = false;
            btnOk.Enabled = false;
            btnPointOK.Enabled = false;
            txtMessage.Text = "";
            if (keypad != null && keypad.Visible)
                keypad.Hide();

            log.Debug("ends-DisableAllControls()");//Added for logger function on 01-July-2016
        }

        private void chkboxRedemption_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("starts-chkboxRedemption_CheckedChanged()");//Added for logger function on 01-July-2016
            
            if(chkboxRedemption.Checked == true)
            {
                if (grpCouponPayment.Enabled == true && grpPointsPayment.Enabled == false)
                {   
                    rdbtnCoupon.Enabled = true;
                    rdbtnCoupon.Checked = true;
                    txtMessage.Text = "Points already applied/not applicable to the current transaction";

                }
                else if (grpCouponPayment.Enabled == false && grpPointsPayment.Enabled == true)
                {
                    rdbtnPoints.Enabled = true;
                    rdbtnPoints.Checked = true;
                    txtMessage.Text = "Coupon already applied/not applicable to the current transaction";
                }  
                else if(grpCouponPayment.Enabled == true && grpPointsPayment.Enabled == true)
                {
                    rdbtnPoints.Enabled = true;
                    rdbtnCoupon.Enabled = true;
                }
            }
            else
            {
                DisableAllControls();
            }

            log.Debug("ends-chkboxRedemption_CheckedChanged()");//Added for logger function on 01-July-2016
        }

        private void rdbtnCoupon_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("starts-rdbtnCoupon_CheckedChanged() event");//Added for logger function on 01-July-2016
            if(rdbtnCoupon.Checked == true)
            {
                UpdateCouponControls(true);
            }
            else
            {
                UpdateCouponControls(false);
            }
            log.Debug("ends-rdbtnCoupon_CheckedChanged() event");//Added for logger function on 01-July-2016
        }

        void UpdateCouponControls(bool couponChecked)
        {
            log.Debug("starts-UpdateCouponControls(couponChecked) method");//Added for logger function on 01-July-2016
            if(couponChecked)
            {
                rdbtnPoints.Checked = false;
                btnPointOK.Enabled = false;
                lblAvailablePoints.Visible = false;
                txtCouponNumber.Enabled = true;
                btnOk.Enabled = true;    
            }
            else
            {
                txtCouponNumber.Text = "";
                txtCouponNumber.Enabled = false;
                btnOk.Enabled = false;
                btnPointOK.Enabled = false;
            }

            if (keypad != null && keypad.Visible)
                keypad.Hide();

            txtMessage.Text = "";
            log.Debug("ends-UpdateCouponControls(couponChecked) method");//Added for logger function on 01-July-2016
        }

        void UpdatePointsControls(bool pointChecked)
        {
            log.Debug("ends-UpdatePointsControls(pointChecked) method");//Added for logger function on 01-July-2016
           
            if(pointChecked)
            {
                rdbtnCoupon.Checked = false;
                txtValidationCode.Enabled = false;
                btnOk.Enabled = false;
                txtPoints.Enabled = true;       
                lblAvailablePoints.Visible = true;
                lblAvailablePoints.Text = "Available Points :" + loyaltyPoints;
            }
            else
            {
                txtPoints.Enabled = false;
                txtPoints.Text = "";
                txtValidationCode.Text = "";
                txtValidationCode.Enabled = false;
                btnPointOK.Enabled = false;
                lblAvailablePoints.Visible = false;
            }

            if (keypad != null && keypad.Visible)
                keypad.Hide();

              txtMessage.Text = "";
              log.Debug("ends-UpdatePointsControls(pointChecked) method");//Added for logger function on 01-July-2016
        }

        private void rdbtnPoints_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("starts-rdbtnPoints_CheckedChanged() event");//Added for logger function on 01-July-2016
            if (rdbtnPoints.Checked == true)
            {
                UpdatePointsControls(true);
            }
            else
            {
                UpdatePointsControls(false);     
            }
            log.Debug("ends-rdbtnPoints_CheckedChanged() event");//Added for logger function on 01-July-2016
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("starts-btnCancel_Click() event");//Added for logger function on 01-July-2016
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.Debug("ends-btnCancel_Click() event");//Added for logger function on 01-July-2016
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.Debug("starts-btnOk_Click() event");//Added for logger function on 01-July-2016          
           
            if (keypad != null && keypad.Visible)
                keypad.Hide();

            if(rdbtnCoupon.Checked == true)
            {
                if(!string.IsNullOrEmpty(txtCouponNumber.Text))
                {
                    txtMessage.Text = MessageUtils.getMessage(1008, WARNING);
                    try
                    {
                        btnOk.Enabled = false;
                        objRedemptionDTO = objRedemptionBL.IsCouponRedeemable(txtCouponNumber.Text.Trim().ToString(), CustomerPhoneNo);
                        if (objRedemptionDTO.success && objRedemptionDTO.is_redeemable)
                        {
                            Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCouponRedeeemable() method", objRedemptionDTO.message, "CouponRedeemable", 0, "Y", txtCouponNumber.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                            couponType = objRedemptionDTO.discount_type;
                            couponValue = objRedemptionDTO.discount_value;
                            couponNumber = txtCouponNumber.Text.Trim().ToString();
                            redemptionType = "CouponType";
                            redemptionApplied = true;
                            this.Close();
                            log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016     
                            return;
                        }
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCouponRedeeemable() method", objRedemptionDTO.message, "CouponRedeemable", 0, "N", txtCouponNumber.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        txtMessage.Text = objRedemptionDTO.message;
                        btnOk.Enabled = true;
                        log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016    
                        return;
                    }
                    catch(Exception ex)
                    {
                        btnOk.Enabled = true;
                        txtMessage.Text = ex.Message;
                        log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016    
                    } 
                }
                else
                {
                    txtMessage.Text = "Enter Coupon Number";
                    log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016    
                    return;
                }
            }
            else if(rdbtnPoints.Checked == true)
            {
                if (ValidatePoints() == false)
                    return;

                if (!string.IsNullOrEmpty(txtValidationCode.Text))
                {
                    txtMessage.Text = MessageUtils.getMessage(1008, WARNING);
                    btnOk.Enabled = false;
                    objRedemptionDTO = objRedemptionBL.IsPointsRedeemable(txtPoints.Text.ToString(), txtValidationCode.Text.Trim().ToString(), CustomerPhoneNo);
                    if (objRedemptionDTO.success && objRedemptionDTO.is_redeemable)
                    {
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsPointsRedeemable method", objRedemptionDTO.message, "IsPointsRedeemable", 0, "Y", txtValidationCode.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        redemptionType = "PointsType";
                        appliedPoints = Convert.ToDouble(txtPoints.Text);
                        validationCode = txtValidationCode.Text.Trim().ToString();
                        redemptionApplied = true;
                        this.Close();
                        log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016    
                        return;
                    }
                    else
                    {
                        btnOk.Enabled = true;
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsPointsRedeemable method", objRedemptionDTO.message, "IsPointsRedeemable", 0, "N", txtValidationCode.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        txtMessage.Text = objRedemptionDTO.message;
                    }          
                }
                else 
                {
                    btnOk.Enabled = true;
                    txtMessage.Text = "Enter Validation Code";
                    log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016    
                    return;
                }
            }
            log.Debug("ends-btnOk_Click() event");//Added for logger function on 01-July-2016
        }

        private void btnPointOK_Click(object sender, EventArgs e)
        {
            log.Debug("starts-btnPointOK_Click() event");//Added for logger function on 01-July-2016
            if (ValidatePoints())
            {
                txtMessage.Text = MessageUtils.getMessage(1008, WARNING);
                try
                {
                    btnPointOK.Enabled = false;       
                    objRedemptionDTO = objRedemptionBL.GetValidationCode(txtPoints.Text.Trim().ToString(), CustomerPhoneNo);             
                    if(objRedemptionDTO.success == true)
                    {
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called GetValidationCode method", objRedemptionDTO.message, "ValidationCode", 0, "Y", txtPoints.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        txtValidationCode.Enabled = true;
                        btnOk.Enabled = true;
                        txtMessage.Text = "Enter Validation Code";                        
                    }
                    else
                    {                      
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called GetValidationCode method", objRedemptionDTO.message, "ValidationCode", 0, "N", txtPoints.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        txtMessage.Text = objRedemptionDTO.message;                
                    }
                    btnPointOK.Enabled = true;
                }
                catch(Exception ex)
                {
                   btnPointOK.Enabled = true;
                   Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called GetValidationCode method", ex.Message, "IsPointsRedeemable", 0, "N", txtPoints.Text, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                   txtMessage.Text = ex.Message;
                }
            }
            log.Debug("ends-btnPointOK_Click() event");//Added for logger function on 01-July-2016
        }

        bool ValidatePoints()
        {
            log.Debug("starts-ValidatePoints() method");//Added for logger function on 01-July-2016
            try
            {
                if (string.IsNullOrEmpty(txtPoints.Text))
                {
                    txtMessage.Text = "Enter Points";
                    log.Debug("ends-ValidatePoints() method");//Added for logger function on 01-July-2016
                    return false;
                }
                if (Convert.ToDouble(txtPoints.Text) < 1)
                {
                    txtMessage.Text = "Enter points greater than or equal to 1";
                    log.Debug("ends-ValidatePoints() method");//Added for logger function on 01-July-2016
                    return false;
                }
                if (Convert.ToDouble(txtPoints.Text) > loyaltyPoints)
                {
                    txtMessage.Text = "Enter points less than or equal to available points";
                    log.Debug("ends-ValidatePoints() method");//Added for logger function on 01-July-2016
                    return false;
                }

                txtMessage.Text = "";
                log.Debug("ends-ValidatePoints() method");//Added for logger function on 01-July-2016
                return true;
            }
            catch(Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Debug("ends-ValidatePoints() method");//Added for logger function on 01-July-2016
                return false;
            }  
        }

        private void txtPoints_Enter(object sender, EventArgs e)
        {
            log.Debug("starts-txtPoints_Enter() event");//Added for logger function on 01-July-2016
            btnPointOK.Enabled = true;
            log.Debug("ends-txtPoints_Enter() event");//Added for logger function on 01-July-2016
        }

        private void txtPoints_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.Debug("starts-txtPoints_KeyPress() event");//Added for logger function on 01-July-2016
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))   
            {
                e.Handled = true;
            }
            log.Debug("ends-txtPoints_KeyPress() event");//Added for logger function on 01-July-2016
        }

        AlphaNumericKeyPad keypad;
        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowNumPad_Click()");//Added for logger function on 08-Mar-2016
            if (keypad == null || keypad.IsDisposed)
            {
                ShowKeyPad();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                ShowKeyPad();
            }
            log.Debug("Ends-btnShowNumPad_Click()");//Added for logger function on 08-Mar-2016
        }

        void ShowKeyPad()
        {
            log.Debug("Starts-ShowKeyPad()");//Added for logger function on 08-Mar-2016
            if (chkboxRedemption.Checked)
            {
                if (rdbtnCoupon.Checked || rdbtnPoints.Checked)
                {
                    if (rdbtnCoupon.Checked)
                    {
                        keypad = new AlphaNumericKeyPad(this, txtCouponNumber);
                        keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Location.Y + this.Height + 4);
                        keypad.Show();
                    }
                    else if (rdbtnPoints.Checked && txtValidationCode.Enabled == true)
                    {
                        keypad = new AlphaNumericKeyPad(this, txtValidationCode);
                        keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Location.Y + this.Height + 4);
                        keypad.Show();
                    }
                    // keypad.Location = new Point(this.PointToScreen(btnShowNumPad.Location).X - keypad.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                }
            }
            log.Debug("Ends-ShowKeyPad()");//Added for logger function on 08-Mar-2016
        }

        private void btnOtherPayment_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnOtherPayment_Click()");//Modified for Adding logger feature on 04-July-2016
            if (chkboxRedemption.Checked)
            {
                if (rdbtnPoints.Checked)
                {
                    double pointsEntered = NumberPadForm.ShowNumberPadForm("Enter Points to be redeemed", "0.0", Utilities,this);
                    txtPoints.Text = Convert.ToString(pointsEntered);
                    if (ValidatePoints())
                    {
                        btnPointOK.Enabled = true;
                    }
                }
            }
            log.Debug("Ends-btnOtherPayment_Click()");//Modified for Adding logger feature on 04-July-2016
        }

        private void frmMainRedemption_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-GenericDataEntry_FormClosing()");//Added for logger function on 04-July-2016
            if (keypad != null)
                keypad.Close();

            log.Debug("Ends-GenericDataEntry_FormClosing()");//Added for logger function on 04-July-2016
        }

        private void txtPoints_Leave(object sender, EventArgs e)
        {
            log.Debug("Starts-txtPoints_Leave()");//Added for logger function on 04-July-2016
            ValidatePoints();
            log.Debug("ends-txtPoints_Leave()");//Added for logger function on 04-July-2016
        }
    }
}
