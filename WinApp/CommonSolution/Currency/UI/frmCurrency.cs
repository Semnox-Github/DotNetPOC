/********************************************************************************************
 * Project Name - Currency
 * Description  - Business logic of Currency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.70       2-Jul-2019    Girish Kundar    Modified : Moved this file from Currency to Product Module.
 *2.70.2       13-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Currency
{
    /// <summary>
    /// User interface for frmCurrency
    /// </summary>
    public partial class frmCurrency : Form
    {
        private static Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// frmCurrency
        /// </summary>
        /// <param name="_utilities"></param>
        public frmCurrency(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            utilities = _utilities;
            InitializeComponent();
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref dgvCurrency);

            dgvCurrency.BorderStyle = BorderStyle.FixedSingle;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.Username);
            PoupulateCurrencyGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// PoupulateCurrencyGrid
        /// </summary>
        void PoupulateCurrencyGrid()
        {
            log.LogMethodEntry();
            CurrencyList currencyList = new CurrencyList(machineUserContext);
            List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParams = new List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>>();
            searchParams.Add(new KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>(CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE, "1"));
            List<CurrencyDTO> currencyListOnDisplay = currencyList.GetAllCurrency(searchParams);

            BindingSource currencyListBS = new BindingSource();

            if (currencyListOnDisplay != null)
            {
                currencyListBS.DataSource = currencyListOnDisplay;
            }
            else
            {
                currencyListBS.DataSource = new List<CurrencyDTO>();
            }

            dgvCurrency.DataSource = currencyListBS;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PoupulateCurrencyGrid();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                BindingSource dgvCurrencyBS = (BindingSource)dgvCurrency.DataSource;
                var currencyListOnDisplay = (List<CurrencyDTO>)dgvCurrencyBS.DataSource;

                Currency currency;
                foreach (CurrencyDTO currencyDTO in currencyListOnDisplay)
                {
                    //Continue if Inserting new Record with InActive Records
                    if (currencyDTO.CurrencyId == -1 && currencyDTO.IsActive == false)
                    {
                        continue;
                    }

                    if (currencyDTO.IsChanged && currencyDTO.CurrencyId != -1)
                    {
                        UpdateCurrencyLogHist(currencyDTO);
                    }
                    currency = new Currency(machineUserContext, currencyDTO);
                    currency.Save();
                }

                PoupulateCurrencyGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update CurrencyLogHist for Modified currency
        /// </summary>
        /// <param name="changedCurrencyDTO">changedCurrencyDTO</param>
        void UpdateCurrencyLogHist(CurrencyDTO changedCurrencyDTO)
        {
            log.LogMethodEntry(changedCurrencyDTO);
            CurrencyLogHistDTO currencyLogHistDTO;
            Currency currency = new Currency(machineUserContext, changedCurrencyDTO.CurrencyId);
            CurrencyDTO currencyDTO = currency.CurrencyDTO;

            if (currencyDTO != null && (currencyDTO.BuyRate != changedCurrencyDTO.BuyRate || currencyDTO.SellRate != changedCurrencyDTO.SellRate))
            {
                currencyLogHistDTO = new CurrencyLogHistDTO();
                currencyLogHistDTO.CurrencyId = currencyDTO.CurrencyId;
                currencyLogHistDTO.CurrencyCode = currencyDTO.CurrencyCode;
                currencyLogHistDTO.CurrencySymbol = currencyDTO.CurrencySymbol;
                currencyLogHistDTO.Description = currencyDTO.Description;
                currencyLogHistDTO.BuyRate = currencyDTO.BuyRate;
                currencyLogHistDTO.SellRate = currencyDTO.SellRate;
                currencyLogHistDTO.EffectiveStartDate = currencyDTO.EffectiveDate;
                currencyLogHistDTO.SynchStatus = currencyDTO.SynchStatus;
                currencyLogHistDTO.MasterEntityId = currencyDTO.MasterEntityId;
                currencyLogHistDTO.IsActive = currencyDTO.IsActive;

                CurrencyLogHist currencyLogHist = new CurrencyLogHist(machineUserContext, currencyLogHistDTO);
                currencyLogHist.Save();

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// dgvCurrency_CellValidating
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvCurrency_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvCurrency.Columns[e.ColumnIndex].Name == "currencyCodeDataGridViewTextBoxColumn")
                {
                    if (!String.IsNullOrEmpty(e.FormattedValue.ToString().Trim()))
                        e.Cancel = false;
                    else
                    {
                        MessageBox.Show("Currency Code cannot be empty", utilities.MessageUtils.getMessage("Currency Code Validation"));
                        e.Cancel = true;
                    }
                }
                else if (dgvCurrency.Columns[e.ColumnIndex].Name == "buyRateDataGridViewTextBoxColumn")
                {
                    double buyRate;
                    if (!double.TryParse(Convert.ToString(e.FormattedValue), out buyRate))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Please Enter Valid Data", utilities.MessageUtils.getMessage("Buy Rate Validation"));
                    }
                    else
                    {
                        e.Cancel = false;
                    }

                }
                else if (dgvCurrency.Columns[e.ColumnIndex].Name == "sellRateDataGridViewTextBoxColumn")
                {
                    double sellRate;
                    if (!double.TryParse(Convert.ToString(e.FormattedValue), out sellRate))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Please Enter Valid Data", utilities.MessageUtils.getMessage("sell Rate Validation"));
                    }
                    else
                    {
                        e.Cancel = false;
                    }
                }

            }     
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
