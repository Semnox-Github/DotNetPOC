/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmCustomerSignedWaivers UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.2      1-Oct-2019       Dakshakh raj         Created for Waiver phase 2 enhancement changes  
 *2.120       18-May-2021      Dakshakh Raj         Handling text box fore color changes.
 *2.130.0     09-Jul-2021      Dakshak              Theme changes to support customized Font ForeColor
 *2.150.0.0   21-Jun-2022      Vignesh Bhat         Back and Cancel button changes
 *2.150.1     22-Feb-2023      Guru S A             Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.KioskCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmCustomerSignedWaivers : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO customerDTO;
        private Utilities utilities;
        private CustomerBL customerBL;
        public frmCustomerSignedWaivers(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            this.customerDTO = customerDTO;
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            //KioskStatic.Utilities.setupDataGridProperties(ref dgvCustomerSignedWaiver); 
            signedDateDataGridViewTextBoxColumn.DefaultCellStyle =
                    expiryDateDataGridViewTextBoxColumn.DefaultCellStyle =
                    creationDateDataGridViewTextBoxColumn.DefaultCellStyle =
                    lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = KioskStatic.Utilities.gridViewDateTimeCellStyle();
            dgvCustomerSignedWaiver.BackgroundColor = Color.White;
            dgvCustomerSignedWaiver.ColumnHeadersDefaultCellStyle.Font = new Font(dgvCustomerSignedWaiver.ColumnHeadersDefaultCellStyle.Font.FontFamily, 18);
            dgvCustomerSignedWaiver.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            selectRecord.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SetTextBoxFontColors();
            customerBL = null;
            SetCustomizedFontColors();
            //DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            utilities.setLanguage(this);
            KioskStatic.logToFile("Loading View Waiver form");
        }
        private void SetCustomerName()
        {
            log.LogMethodEntry();
            try
            {
                if (this.customerDTO != null && this.customerDTO.Id > -1)
                {
                    lblCustomer.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")
                                       + ": "
                                       + (string.IsNullOrEmpty(customerDTO.FirstName) ? string.Empty : customerDTO.FirstName)
                                       + " " +
                                       (string.IsNullOrEmpty(customerDTO.LastName) ? string.Empty : customerDTO.LastName);
                }
                else
                {
                    lblCustomer.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message))
                {
                    frmOK.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        private void frmCustomerSignedWaivers_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (customerDTO != null && customerDTO.Id > -1)
            {
                customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                customerBL.LoadCustomerSignedWaivers();
            }
            else
            {
                string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2377, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer"));
                using (frmOKMsg frmOK = new frmOKMsg(msg))
                {
                    frmOK.ShowDialog();
                }
            }
            SetCustomerName();
            LoadCustomerSignedWaivers();
            //CreateHeaderCheckBox();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void LoadCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            if (customerBL != null)
            {
                customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>(customerBL.CustomerDTO.CustomerSignedWaiverDTOList);
            }
            dgvCustomerSignedWaiver.DataSource = customerSignedWaiverDTOList;
            log.LogMethodExit();
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("View button is clicked");
            try
            {
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = GetSelectedDTOList();
                if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                {

                    if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Count > 5)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2434));
                        //Please select max 5 records to view the files
                    }

                    using (frmViewWaiverUI frmViewWaiver = new frmViewWaiverUI(customerSignedWaiverDTOList))
                    {
                        frmViewWaiver.ShowDialog();
                    }
                    DeSelectViewedRecords();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message))
                {
                    frmOK.ShowDialog();
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }


        private List<CustomerSignedWaiverDTO> GetSelectedDTOList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            if (dgvCustomerSignedWaiver != null && dgvCustomerSignedWaiver.Rows.Count > 0)
            {
                bool check = false;
                for (int rowIndex = 0; rowIndex < dgvCustomerSignedWaiver.Rows.Count; rowIndex++)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaiver["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        check = true;
                        CustomerSignedWaiverDTO customerSignedWaiverDTO = (dgvCustomerSignedWaiver.DataSource as List<CustomerSignedWaiverDTO>)[rowIndex];
                        if (string.IsNullOrEmpty(customerSignedWaiverDTO.SignedWaiverFileName))
                        {
                            string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2345, customerSignedWaiverDTO.WaiverName,
                                                                     MessageContainerList.GetMessage(utilities.ExecutionContext, "view"));
                            using (frmOKMsg frmOK = new frmOKMsg(msg))
                            {
                                frmOK.ShowDialog();
                            }
                        }
                        else
                        {
                            customerSignedWaiverDTOList.Add(customerSignedWaiverDTO);
                        }
                    }
                    checkBox.Value = false;
                }
                if (check == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2460)); //Please select a record to proceed
                }
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }

        private void DeSelectViewedRecords()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (dgvCustomerSignedWaiver != null && dgvCustomerSignedWaiver.Rows.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < dgvCustomerSignedWaiver.Rows.Count; rowIndex++)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaiver["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                }
                dgvCustomerSignedWaiver.EndEdit();
            }
            log.LogMethodExit();
        }

        private void dgvCustomerSignedWaiver_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (e.RowIndex > -1)
            {
                if (dgvCustomerSignedWaiver.Columns[e.ColumnIndex].Name == "selectRecord")
                {
                    DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaiver["selectRecord", e.RowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                    else
                    {
                        checkBox.Value = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (customerDTO != null)
            {
                //This action will clear current customer session. Do you want to proceed?
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                {
                    if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        base.btnHome_Click(sender, e);
                    }
                }
            }
            else
            {
                base.btnHome_Click(sender, e);
            }
            log.LogMethodExit();
        }

        private void SetTextBoxFontColors()
        {
            if (KioskStatic.CurrentTheme == null ||
                  (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                dgvCustomerSignedWaiver.ForeColor = Color.Black;
            }
            else
            {
                dgvCustomerSignedWaiver.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }//
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void LeftButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void RightButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerSignedWaivers");
            try
            {
                foreach (Control c in dgvCustomerSignedWaiver.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor;//Payment options buttons
                }

                this.dgvCustomerSignedWaiver.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationLblCustomerNameTextForeColor;
                this.lblCustomer.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationChkSignConfirmTextForeColor;
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationPbCheckBoxTextForeColor;
                this.btnView.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnOkayTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnCancelTextForeColor;
                this.bigVerticalScrollCustomerSignedWaiver.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigHorizontalScrollCustomerSignedWaiver.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollLeftEnabled, ThemeManager.CurrentThemeImages.ScrollLeftDisabled, ThemeManager.CurrentThemeImages.ScrollRightEnabled, ThemeManager.CurrentThemeImages.ScrollRightDisabled);
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CustomerSignedWaiversBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnView.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panel1.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.dgvCustomerSignedWaiver.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationDgvCustomerSignedWaiverHeaderTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerSignedWaivers: " + ex.Message);
            }
            log.LogMethodExit();
        } 
    }
}
