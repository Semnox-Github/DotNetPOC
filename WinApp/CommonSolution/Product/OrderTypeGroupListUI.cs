using Semnox.Core;
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
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// OrderTypeGroup UI
    /// </summary>
    public partial class OrderTypeGroupListUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Constructor of OrderTypeGroupListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public OrderTypeGroupListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvOrderTypeGroupDTOList);
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

        private void OrderTypeGroupListUI_Load(object sender, EventArgs e)
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
            LoadOrderTypeGroupDTOList();
            log.LogMethodExit();
        }

        private void LoadOrderTypeGroupDTOList()
        {
            log.LogMethodEntry();
            try
            {
                OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(machineUserContext);
                List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                }
                searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.NAME, txtName.Text));
                searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParameters);
                SortableBindingList<OrderTypeGroupDTO> orderTypeGroupDTOSortableList;
                if (orderTypeGroupDTOList != null)
                {
                    orderTypeGroupDTOSortableList = new SortableBindingList<OrderTypeGroupDTO>(orderTypeGroupDTOList);
                }
                else
                {
                    orderTypeGroupDTOSortableList = new SortableBindingList<OrderTypeGroupDTO>();
                }
                orderTypeGroupDTOListBS.DataSource = orderTypeGroupDTOSortableList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("",ex);
            }
            log.LogMethodExit();
        }

        private void dgvOrderTypeGroupDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvOrderTypeGroupDTOList.Columns[e.ColumnIndex].HeaderText +
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
            LoadOrderTypeGroupDTOList();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvOrderTypeGroupDTOList.EndEdit();
            SortableBindingList<OrderTypeGroupDTO> orderTypeGroupDTOSortableList = orderTypeGroupDTOListBS.DataSource as SortableBindingList<OrderTypeGroupDTO>;
            string message;
            OrderTypeGroupBL orderTypeGroupBL;
            bool error = false;
            if (orderTypeGroupDTOSortableList != null && orderTypeGroupDTOSortableList.Count > 0)
            {
                for (int i = 0; i < orderTypeGroupDTOSortableList.Count; i++)
                {
                    if (orderTypeGroupDTOSortableList[i].IsChanged)
                    {
                        message = ValidateOrderTypeGroupDTO(orderTypeGroupDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                orderTypeGroupBL = new OrderTypeGroupBL(machineUserContext, orderTypeGroupDTOSortableList[i]);
                                orderTypeGroupBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error("Error while saving orderTypeGroup", ex);
                                dgvOrderTypeGroupDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch (Exception ex)
                            {
                                error = true;
                                log.Error("Error while saving orderTypeGroup.", ex);
                                dgvOrderTypeGroupDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                        else
                        {
                            error = true;
                            dgvOrderTypeGroupDTOList.Rows[i].Selected = true;
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
                dgvOrderTypeGroupDTOList.Update();
                dgvOrderTypeGroupDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private string ValidateOrderTypeGroupDTO(OrderTypeGroupDTO orderTypeGroupDTO)
        {
            log.LogMethodEntry(orderTypeGroupDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(orderTypeGroupDTO.Name) || string.IsNullOrWhiteSpace(orderTypeGroupDTO.Name))
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
            if (dgvOrderTypeGroupDTOList.SelectedRows.Count <= 0 && dgvOrderTypeGroupDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit(null, "No rows selected. Please select the rows you want to delete and press delete");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvOrderTypeGroupDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvOrderTypeGroupDTOList.SelectedCells)
                {
                    dgvOrderTypeGroupDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvOrderTypeGroupDTOList.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvOrderTypeGroupDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<OrderTypeGroupDTO> orderTypeGroupDTOSortableList = (SortableBindingList<OrderTypeGroupDTO>)orderTypeGroupDTOListBS.DataSource;
                        OrderTypeGroupDTO orderTypeGroupDTO = orderTypeGroupDTOSortableList[row.Index];
                        orderTypeGroupDTO.IsActive = false;
                        OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(machineUserContext,orderTypeGroupDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            orderTypeGroupBL.Save();
                        }
                        catch (ForeignKeyException ex)
                        {
                            log.Error("Error occured while inactivating the record",ex);
                            dgvOrderTypeGroupDTOList.Rows[row.Index].Selected = true;
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
    }
}
