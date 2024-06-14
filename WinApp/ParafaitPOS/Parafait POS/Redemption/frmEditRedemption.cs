/*  Date          Modification                                          Done by         Version
 *  25-Jun-2018   Redeption Kiosk change: Created to enable redemption  Guru S A        2.3.0 
 *                status update for redemptions created by the kiosk
 *  09-Sep-2018   RedemptionReversal changes                            Guru S A        2.4.0                          
*/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Redemption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    public partial class frmEditRedemption : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int redemptionId;
        RedemptionBL redemptionBL;
        private string parentScreenNumber = string.Empty;
        private VirtualKeyboardController virtualKeyboard;

        internal delegate void SetLastActivityTimeDelegate();
        internal SetLastActivityTimeDelegate SetLastActivityTime;

        public frmEditRedemption(Utilities utilities, int redemptionId)
        {
            log.LogMethodEntry(utilities, redemptionId);
            InitializeComponent();
            this.utilities = utilities;
            this.redemptionId = redemptionId;
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }

        void LoadRedemptionDetails()
        {
            log.LogMethodEntry();
            RedemptionListBL redemptionListBL = new RedemptionListBL();
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchParm.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, this.redemptionId.ToString()));
            searchParm.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
            List<RedemptionDTO> redemptionDTOList = redemptionListBL.GetRedemptionDTOList(searchParm);
            if (redemptionDTOList != null && redemptionDTOList.Count > 0)
            {
                this.redemptionBL = new RedemptionBL(redemptionDTOList[0], this.utilities.ExecutionContext);
                UpdateUIFields();
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                SetLastActivityTime();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                SetLastActivityTime();
                bool saveData = false;
                if (!String.IsNullOrEmpty(txtAddRemarks.Text))
                {

                    string newRemark = ((String.IsNullOrEmpty(txtRemarks.Text) == true) ? "" : txtRemarks.Text + Environment.NewLine) + utilities.ExecutionContext.GetUserId() + ": " + txtAddRemarks.Text;
                    if (newRemark.Length < 2000)
                    {
                        redemptionBL.RedemptionDTO.Remarks = newRemark;
                    }
                    else
                    {
                        //"Remarks contains " + newRemark.Length.ToString() + "charters. Remarks can not have more than 2000 charcters"
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,2670, newRemark.Length.ToString()),
                                                   MessageContainerList.GetMessage(utilities.ExecutionContext,"Update Status") +
                                                   MessageContainerList.GetMessage(utilities.ExecutionContext, 2693, parentScreenNumber));
                        txtAddRemarks.Focus();
                        return;
                    }
                    saveData = true;
                }
                if (rBtnPrepared.Checked)
                {
                    redemptionBL.RedemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString();
                    saveData = true;
                }
                else if (rBtnDelivered.Checked)
                {
                    redemptionBL.RedemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                    saveData = true;
                }
                if (saveData)
                {
                    redemptionBL.Save();
                }
                UpdateUIFields();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frmEditRedemption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            utilities.setLanguage(this);
            SetParentScreenNumber();
            LoadRedemptionDetails();
            log.LogMethodExit();
        }

        void UpdateUIFields()
        {
            log.LogMethodEntry();
            if (this.redemptionBL != null && this.redemptionBL.RedemptionDTO != null)
            {
                txtRedemptionId.Text = this.redemptionBL.RedemptionDTO.RedemptionId.ToString();
                txtRedemptionOrderNo.Text = this.redemptionBL.RedemptionDTO.RedemptionOrderNo;
                txtRedemptionStatus.Text = this.redemptionBL.RedemptionDTO.RedemptionStatus;
                txtRemarks.Text = this.redemptionBL.RedemptionDTO.Remarks;
                txtAddRemarks.Text = "";
                switch (this.redemptionBL.RedemptionDTO.RedemptionStatus)
                {
                    case "OPEN":
                        rBtnPrepared.Enabled = rBtnDelivered.Enabled = true;
                        break;
                    case "PREPARED":
                        rBtnPrepared.Enabled = false;
                        rBtnDelivered.Enabled = true;
                        rBtnPrepared.Checked = true;
                        break;
                    case "DELIVERED":
                        rBtnPrepared.Enabled = rBtnDelivered.Enabled = false;
                        rBtnDelivered.Checked = true;
                        break;
                    default:
                        rBtnPrepared.Enabled = rBtnDelivered.Enabled = false;
                        break;
                }
            }
            log.LogMethodExit();
        }

        private void btnExit_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnExit.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodExit();
        }

        private void btnExit_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnExit.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodExit();
        }

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnSave.BackgroundImage = Properties.Resources.OrderSave;
            log.LogMethodExit();
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnSave.BackgroundImage = Properties.Resources.OrderSavePressed;
            log.LogMethodExit();
        }

        private void rBtnPrepared_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetLastActivityTime();
                if (rBtnPrepared.Checked)
                {
                    rBtnPrepared.Image = Properties.Resources.CheckedNew;
                }
                else
                {
                    rBtnPrepared.Image = Properties.Resources.UncheckedNew;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void rBtnDelivered_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetLastActivityTime();
                if (rBtnDelivered.Checked)
                {
                    rBtnDelivered.Image = Properties.Resources.CheckedNew;
                }
                else
                {
                    rBtnDelivered.Image = Properties.Resources.UncheckedNew;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void SetParentScreenNumber()
        {
            log.LogMethodEntry();
            try
            {
                frm_redemption parentForm = this.Owner as frm_redemption;
                if (parentForm != null)
                {
                    parentScreenNumber = parentForm.GetParentScreenNumber;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

    }
}
