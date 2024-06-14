
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
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// OrderType UI
    /// </summary>
    public partial class OrderTypeListUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Constructor of OrderTypeListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public OrderTypeListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvOrderTypeDTOList);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void OrderTypeListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            descriptionDataGridViewTextBoxColumn.MaxInputLength = 100;
            nameDataGridViewTextBoxColumn.MaxInputLength = 50;
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadOrderTypeDTOList();
            log.LogMethodExit();
        }

        private void LoadOrderTypeDTOList()
        {
            log.LogMethodEntry();
            try
            {
                OrderTypeListBL orderTypeListBL = new OrderTypeListBL(machineUserContext);
                List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
                if(chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                }
                searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.NAME, txtName.Text));
                searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<OrderTypeDTO> orderTypeDTOList = orderTypeListBL.GetOrderTypeDTOList(searchParameters);
                SortableBindingList<OrderTypeDTO> orderTypeDTOSortableList;
                if (orderTypeDTOList != null)
                {
                    orderTypeDTOSortableList = new SortableBindingList<OrderTypeDTO>(orderTypeDTOList);
                }
                else
                {
                    orderTypeDTOSortableList = new SortableBindingList<OrderTypeDTO>();
                }
                orderTypeDTOListBS.DataSource = orderTypeDTOSortableList;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading OrderTypeDTO list", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvOrderTypeDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvOrderTypeDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            txtName.ResetText();
            RefreshData();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadOrderTypeDTOList();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvOrderTypeDTOList.EndEdit();
            SortableBindingList<OrderTypeDTO> orderTypeDTOSortableList = orderTypeDTOListBS.DataSource as SortableBindingList<OrderTypeDTO>;
            string message;
            OrderTypeBL orderTypeBL;
            bool error = false;
            if (orderTypeDTOSortableList != null && orderTypeDTOSortableList.Count > 0)
            {
                for (int i = 0; i < orderTypeDTOSortableList.Count; i++)
                {
                    if (orderTypeDTOSortableList[i].IsChanged)
                    {
                        message = ValidateOrderTypeDTO(orderTypeDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                orderTypeBL = new OrderTypeBL(machineUserContext,orderTypeDTOSortableList[i]);
                                orderTypeBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error("Error occured while saving the record", ex);
                                dgvOrderTypeDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch (Exception ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record", ex);
                                dgvOrderTypeDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                        else
                        {
                            error = true;
                            dgvOrderTypeDTOList.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if (!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvOrderTypeDTOList.Update();
                dgvOrderTypeDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private string ValidateOrderTypeDTO(OrderTypeDTO orderTypeDTO)
        {
            log.LogMethodEntry(orderTypeDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(orderTypeDTO.Name) || string.IsNullOrWhiteSpace(orderTypeDTO.Name))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", nameDataGridViewTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvOrderTypeDTOList.SelectedRows.Count <= 0 && dgvOrderTypeDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit(null, "No rows selected. Please select the rows you want to delete and press delete");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvOrderTypeDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvOrderTypeDTOList.SelectedCells)
                {
                    dgvOrderTypeDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvOrderTypeDTOList.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvOrderTypeDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<OrderTypeDTO> orderTypeDTOSortableList = (SortableBindingList<OrderTypeDTO>)orderTypeDTOListBS.DataSource;
                        OrderTypeDTO orderTypeDTO = orderTypeDTOSortableList[row.Index];
                        orderTypeDTO.IsActive = false;
                        OrderTypeBL orderTypeBL = new OrderTypeBL(machineUserContext, orderTypeDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            orderTypeBL.Save();
                        }
                        catch (ForeignKeyException ex)
                        {
                            log.Error(ex.Message);
                            dgvOrderTypeDTOList.Rows[row.Index].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                            continue;
                        }
                    }
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if (refreshFromDB == true)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private void btnMapOrderType_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (orderTypeDTOListBS.Current != null && orderTypeDTOListBS.Current is OrderTypeDTO)
            {
                OrderTypeDTO orderTypeDTO = orderTypeDTOListBS.Current as OrderTypeDTO;
                if(orderTypeDTO.Id < 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(665));
                    log.LogMethodExit(null, utilities.MessageUtils.getMessage(665));
                    return;
                }
                else
                {
                    OrderTypeGroupMapListUI orderTypeGroupMapListUI = new OrderTypeGroupMapListUI(utilities, orderTypeDTO);
                    orderTypeGroupMapListUI.StartPosition = FormStartPosition.CenterScreen;
                    orderTypeGroupMapListUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void orderTypeDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnMapOrderType.Enabled = false;
            if (orderTypeDTOListBS.Current != null && orderTypeDTOListBS.Current is OrderTypeDTO)
            {
                OrderTypeDTO orderTypeDTO = orderTypeDTOListBS.Current as OrderTypeDTO;
                if(orderTypeDTO.IsActive)
                {
                    btnMapOrderType.Enabled = true;
                }
            }
            log.LogMethodExit();
        }
    }
}
