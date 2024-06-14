/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Redemption order details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       12-Sep-2018      Archana            Created 
 ********************************************************************************************/
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Redemption_Kiosk
{
    public partial class FrmRedemptionKioskRedemptionDetails : frmRedemptionKioskBaseForm
    {
        ExecutionContext machineUserContext;
        private int RedemptionId;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionId">int</param>
        public FrmRedemptionKioskRedemptionDetails(ExecutionContext executionContext, int redemptionId)
        {
            log.LogMethodEntry(executionContext, redemptionId);
            machineUserContext = executionContext;
            RedemptionId = redemptionId;
            InitializeComponent();
            Common.utils.setLanguage(this);
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskRedemptionDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetDataGridsHeaderFont();
            RedemptionListBL redemptionListBl = new RedemptionListBL();
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, RedemptionId.ToString()));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, "Y"));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(machineUserContext.GetSiteId())));
            List<RedemptionDTO> redemptionDTOList = redemptionListBl.GetRedemptionDTOList(searchParameters);
            if (redemptionDTOList != null && redemptionDTOList.Count > 0)
            {
                txtOrderNo.Text = redemptionDTOList[0].RedemptionOrderNo;
                rctxtbxRemarks.Text = redemptionDTOList[0].Remarks;
                SortableBindingList<RedemptionGiftsDTO> redemptionGiftsDTOSortableList;
                redemptionGiftsDTOSortableList = new SortableBindingList<RedemptionGiftsDTO>(redemptionDTOList[0].RedemptionGiftsListDTO);
                redemptinGiftDOBindingSource.DataSource = redemptionGiftsDTOSortableList;

                SortableBindingList<RedemptionCardsDTO> redemptionCardsDTOSortableList;
                redemptionCardsDTOSortableList = new SortableBindingList<RedemptionCardsDTO>(redemptionDTOList[0].RedemptionCardsListDTO);
                redemptionCardsDTOBindingSource.DataSource = redemptionCardsDTOSortableList;

                SortableBindingList<RedemptionTicketAllocationDTO> rtaDTOSortableList;
                rtaDTOSortableList = new SortableBindingList<RedemptionTicketAllocationDTO>(redemptionDTOList[0].RedemptionTicketAllocationListDTO);
                redemptionTicketAllocationDTOBiningSource.DataSource = rtaDTOSortableList;
            }
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); ;
            this.Dispose();
            this.Close();
            log.LogMethodExit();
        }

        private void dgvGiftInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvGiftInfo.Columns[e.ColumnIndex].DataPropertyName == "RedemptionGiftsId")
            {
                dgvGiftInfo.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }

        private void dgvCardInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvCardInfo.Columns[e.ColumnIndex].DataPropertyName == "RedemptionCardsId")
            {
                dgvCardInfo.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }

        private void dgvTicketAllocation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvTicketAllocation.Columns[e.ColumnIndex].DataPropertyName == "Id")
            {
                dgvTicketAllocation.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }

        private void SetDataGridsHeaderFont()
        {
            log.LogMethodEntry();
            dgvGiftInfo.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Bold);
            dgvCardInfo.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Bold);
            dgvTicketAllocation.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14F, FontStyle.Bold);
            log.LogMethodExit();
        }
    }
}
