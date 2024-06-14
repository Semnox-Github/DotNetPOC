/********************************************************************************************
 * Project Name - Parafait_POS.Waivers
 * Description  - usrCtlWaiverSet 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      26-Sep-2019   Guru S A                Created for Waivers phase 2 enhancement changes  
 ********************************************************************************************/
using System;
using System.Collections.Generic; 
using System.Windows.Forms;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.Customer;
using Semnox.Core.Utilities;
using System.Drawing;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer.Waivers;

namespace Parafait_POS.Waivers
{

    public partial class usrCtlWaiverSet : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerBL customerBL;
        private WaiverSetDTO waiverSetDTO;
        private Utilities utilities;
        private Transaction trx;
        int guestCustomerId;
        private bool evenRowNo;
        private LookupValuesList serverTimeObject;
        public delegate void ResetOtherWaiverSetSelectionDelegate(int waiverSetId);
        public ResetOtherWaiverSetSelectionDelegate ResetOtherWaiverSetSelection; 
        public bool IsSignWaiverSet
        {
            get { return rBtnSelectedWaiverSet.Checked; }
            set { rBtnSelectedWaiverSet.Checked = value; }
        }

        public WaiverSetDTO WaiverSetDTO { get { return waiverSetDTO; } set { waiverSetDTO = value; } }

        public usrCtlWaiverSet(CustomerBL customerBL, WaiverSetDTO waiverSetDTO, Utilities utilities, bool evenRowNo, Transaction trx = null)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.customerBL = customerBL;
            this.WaiverSetDTO = waiverSetDTO;
            this.utilities = utilities;
            this.evenRowNo = evenRowNo;
            serverTimeObject = new LookupValuesList(utilities.ExecutionContext);
            this.trx = trx;
            this.guestCustomerId = CustomerListBL.GetGuestCustomerId(utilities.ExecutionContext);
            // this.waiversDTOBindingSource = new System.Windows.Forms.BindingSource();
            LoadWaiverSetControlData();
            log.LogMethodExit();
        }

        private void LoadWaiverSetControlData()
        {
            log.LogMethodEntry();
            if (customerBL != null && this.WaiverSetDTO != null)
            {
                Color backGroundColor = Color.White;
                if (this.evenRowNo == false)
                {
                    backGroundColor = Color.Azure;
                }
                this.BackColor = backGroundColor;
                //this.tPnlWaiverSet.AutoSize = true;
                this.lblWaiverSetName.Text = WaiverSetDTO.Description;
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(this.lblWaiverSetName, WaiverSetDTO.Description);
                BuildWaiverDetails();
                // waiversDTOBindingSource.DataSource = this.WaiverSetDTO.WaiverSetDetailDTOList;
                //this.dgvWaivers.DataSource = waiversDTOBindingSource.DataSource;
                //SetSignedFlag();
                //AdjustGridStyleNHeight(); 
                this.tPnlWaiverSet.Height = waiverSetDTO.WaiverSetDetailDTOList.Count * 40 + 2;
                this.Height = this.tPnlWaiverSet.Height + 2;
            }
            log.LogMethodExit();
        }

        private void BuildWaiverDetails()
        {
            log.LogMethodEntry();
            List<WaiversDTO> pendingWaiversDTOList = new List<WaiversDTO>();
            if (customerBL.CustomerDTO.Id == guestCustomerId && trx != null)
            {
                pendingWaiversDTOList = trx.GetPendingWaiversDTOList();
                if (pendingWaiversDTOList == null)
                {
                    pendingWaiversDTOList = new List<WaiversDTO>();
                }
            }
            for (int i = 0; i < waiverSetDTO.WaiverSetDetailDTOList.Count; i++)
            {
                WaiversDTO waiversDTO = (waiverSetDTO.WaiverSetDetailDTOList[i] as WaiversDTO);
                Label lblWaiverName = new Label()
                {
                    Text = waiversDTO.Name,
                    Font = lblWaiverSetName.Font,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoEllipsis = true
                };
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(lblWaiverName, waiversDTO.Name);
                lblWaiverName.Size = new Size(240, 40);
                tPnlWaivers.Controls.Add(lblWaiverName, 0, i);
                Label lblValidityDays = new Label()
                {
                    Text = (waiversDTO.ValidForDays == null ? string.Empty : ((int)waiversDTO.ValidForDays).ToString(utilities.ParafaitEnv.NUMBER_FORMAT)),
                    Font = lblWaiverSetName.Font,
                    TextAlign = ContentAlignment.MiddleRight
                };
                lblValidityDays.Size = new Size(60, 40);
                tPnlWaivers.Controls.Add(lblValidityDays, 1, i);
                Button btnViewWaiver = new Button()
                {
                    Text = "...",
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(30, 30),
                    Font = new Font("Arial", 12, FontStyle.Regular),
                    Tag = waiversDTO
                };
                btnViewWaiver.FlatAppearance.MouseOverBackColor = btnViewWaiver.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                btnViewWaiver.Click += new EventHandler(btnViewClick); 
                tPnlWaivers.Controls.Add(btnViewWaiver, 2, i);
                Button btnSignStatus = new Button()
                {
                    Text = string.Empty,
                    FlatStyle = FlatStyle.Flat,
                    BackgroundImage = Properties.Resources.RedCross,
                    BackgroundImageLayout = ImageLayout.Stretch, 
                    Size = new Size(30, 30)
                };

                btnSignStatus.FlatAppearance.MouseDownBackColor = btnSignStatus.FlatAppearance.MouseOverBackColor = btnSignStatus.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                if (this.trx != null && customerBL.CustomerDTO.Id == guestCustomerId 
                    && pendingWaiversDTOList.Exists( ws => ws.WaiverSetDetailId == waiversDTO.WaiverSetDetailId) == false)
                {
                    btnSignStatus.BackgroundImage = Properties.Resources.GreenTick;
                }
                else if (customerBL.CustomerDTO.Id != guestCustomerId  && customerBL.HasSigned(waiversDTO, serverTimeObject.GetServerDateTime()))
                {
                    btnSignStatus.BackgroundImage = Properties.Resources.GreenTick;
                }
                tPnlWaivers.Controls.Add(btnSignStatus, 3, i);
                RowStyle temp = tPnlWaivers.RowStyles[0];
                tPnlWaivers.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                tPnlWaivers.RowCount = i + 1;
            }
            tPnlWaivers.Height = waiverSetDTO.WaiverSetDetailDTOList.Count * 39 + 2;
            log.LogMethodExit();
        }


        private void btnViewClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button btnviewWaiver = (Button)sender;
            if (btnviewWaiver.Tag != null)
            {
                WaiversDTO waiversDTO = (WaiversDTO)btnviewWaiver.Tag;
                if (waiversDTO != null && string.IsNullOrEmpty(waiversDTO.WaiverFileName) == false)
                {
                    using (frmViewWaiverUI frmViewWaiverUI = new frmViewWaiverUI(waiversDTO, utilities))
                    {
                        if (frmViewWaiverUI.Width > Application.OpenForms["POS"].Width + 28)
                        {
                            frmViewWaiverUI.Width = Application.OpenForms["POS"].Width - 30;
                        }
                        frmViewWaiverUI.ShowDialog();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void AdjustGridStyleNHeight()
        {
            log.LogMethodEntry();
            //utilities.setupDataGridProperties(ref dgvWaivers);
            //dgvWaivers.Columns["validForDaysDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            //SetDGVCellFont(dgvWaivers);
            //dgvWaivers.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            //dgvWaivers.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            //int rowCount = (this.WaiverSetDTO.WaiverSetDetailDTOList != null ? this.WaiverSetDTO.WaiverSetDetailDTOList.Count : 1);
            //if (rowCount > 1)
            //{
            //    this.dgvWaivers.Height = rowCount * 40;
            //}
            //else
            //{
            //    this.dgvWaivers.Height = 40;
            //}
            //this.dgvWaivers.EndEdit();
            //this.dgvWaivers.Refresh();
            log.LogMethodExit();
        }

        //private void SetButtonText()
        //{
        //    log.LogMethodEntry();
        //    if (dgvWaivers.Rows.Count> 0)
        //    {
        //        foreach (DataGridViewRow dataRow in dgvWaivers.Rows)
        //        {
        //            DataGridViewButtonCell dgvButtonCell = (DataGridViewButtonCell)dataRow.Cells["ViewWaiver"];
        //            dgvButtonCell.Value = "...";
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        private void SetSignedFlag()
        {
            log.LogMethodEntry();
            //for (int i = 0; i < waiversDTOBindingSource.Count; i++)
            //{
            //    WaiversDTO waiversDTO = (waiversDTOBindingSource[i] as WaiversDTO);
            //    if (customerBL.HasSigned(waiversDTO))
            //    {
            //        ((DataGridViewImageCell)dgvWaivers.Rows[i].Cells["CustomerHasSigned"]).Value = Properties.Resources.GreenTick;
            //    }
            //    else
            //    {
            //        ((DataGridViewImageCell)dgvWaivers.Rows[i].Cells["CustomerHasSigned"]).Value = Properties.Resources.RedCross;
            //    }
            //}
            log.LogMethodExit();
        }

        //private void SetDGVCellFont(DataGridView dgvInput)
        //{
        //    log.LogMethodEntry();
        //    dgvInput.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        //    System.Drawing.Font font;
        //    try
        //    {
        //        font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured while applying new font", ex);
        //        font = new Font("Tahoma", 15, FontStyle.Regular);
        //    }
        //    foreach (DataGridViewColumn c in dgvInput.Columns)
        //    {
        //        c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
        //    }
        //    dgvInput.AllowUserToResizeRows = false;
        //    log.LogMethodExit();
        //}

        //private void dgvWaivers_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    log.LogMethodEntry();
        //    try
        //    {
        //        if (e.RowIndex > -1)
        //        {
        //            if (dgvWaivers.Columns[e.ColumnIndex].Name == "ViewWaiver")
        //            {
        //                WaiversDTO selectedWaiverDTO = this.WaiverSetDTO.WaiverSetDetailDTOList[e.RowIndex];
        //                if (selectedWaiverDTO != null)
        //                {
        //                    using (frmViewWaiverUI frmViewWaiverUI = new frmViewWaiverUI(selectedWaiverDTO, utilities))
        //                    {
        //                        frmViewWaiverUI.ShowDialog();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //    log.LogMethodExit();
        //}

        private void rBtnSelectedWaiverSet_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                RadioButton rBtnSelectedWaiverset = (RadioButton)sender;
                if (rBtnSelectedWaiverset.Checked)
                {
                    ResetOtherWaiverSetSelection(this.WaiverSetDTO.WaiverSetId);
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
