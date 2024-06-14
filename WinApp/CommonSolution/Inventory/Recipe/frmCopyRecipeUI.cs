/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI for Copying Recipe's
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       31-Aug-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Parafait.Languages;
using System.Drawing;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmCopyRecipeUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private DateTime fromDate;

        public frmCopyRecipeUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            executionContext = utilities.ExecutionContext;
            dtpSrcFromDate.CustomFormat = utilities.getDateFormat();
            dtpSourceToDate.CustomFormat = utilities.getDateFormat();
            dtpDestFromDate.CustomFormat = utilities.getDateFormat();
            dtpDestToDate.CustomFormat = utilities.getDateFormat();
            log.LogMethodExit();
        }

        public frmCopyRecipeUI(Utilities utilities, DateTime fromDate)
            : this(utilities)
        {
            log.LogMethodEntry(utilities, fromDate);
            this.fromDate = fromDate;
            dtpSrcFromDate.Value = fromDate;
            dtpDestFromDate.Value = fromDate;
            dtpDestToDate.Value = fromDate;
            dtpSourceToDate.Value = fromDate;
            log.LogMethodExit();
        }

        private void dtpSourceToDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DateTimePicker dtp = (DateTimePicker)sender;
                if (!dtp.Focused)
                {
                    log.LogMethodExit();
                    return;
                }

                dtpSourceToDate.CustomFormat = utilities.getDateFormat();
                dtpSourceToDate.Format = DateTimePickerFormat.Custom;
                if (dtpSourceToDate.Value.Date < dtpSrcFromDate.Value.Date)
                {
                    dtpSourceToDate.Value = dtpSrcFromDate.Value;
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1094), MessageContainerList.GetMessage(executionContext, 1095));
                    return;
                }
                if (dtpSourceToDate.Value.Date != dtpSrcFromDate.Value.Date)
                {
                    dtpDestToDate.Enabled = false;
                    dtpDestToDate.Value = dtpSourceToDate.Value;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Confirm Recipe's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmCopy_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parfaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    lblMessage.Text = string.Empty;
                    bool valid =  ValidateRecipe();
                    if (valid)
                    {
                        DateTime fromdate = dtpSrcFromDate.Value.Date;
                        DateTime todate = dtpSourceToDate.Value.Date;
                        RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                        List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, fromdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, todate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<DateTime> sourceDateList = new List<DateTime>();
                        double datediff = (dtpSourceToDate.Value - dtpSrcFromDate.Value).TotalDays;
                        datediff++;
                        int i = 0;
                        while (i < datediff)
                        {
                            sourceDateList.Add(dtpSrcFromDate.Value.Date.AddDays(i));
                            i++;
                        }
                        List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchParameters, true, true);
                        if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
                        {
                            parfaitDBTrx.BeginTransaction();
                            recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext, recipePlanHeaderDTOList);
                            recipePlanHeaderListBL.CopyRecipe(dtpDestFromDate.Value.Date, dtpDestToDate.Value.Date, sourceDateList, parfaitDBTrx.SQLTrx);
                            parfaitDBTrx.EndTransaction();
                            lblMessage.Text = MessageContainerList.GetMessage(executionContext, "Copy Successful");
                        }
                        else
                        {
                            lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2847); //No Recipe's available for the selected source range. Please Add Recipe's
                            lblMessage.ForeColor = Color.Red;
                        }
                    }
                }
                catch (Exception ex)
                {
                    parfaitDBTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void dtpDestFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DateTimePicker dtp = (DateTimePicker)sender;
                if (!dtp.Focused)
                {
                    log.LogMethodExit();
                    return;
                }
                dtpSourceToDate.CustomFormat = utilities.getDateFormat();
                dtpSourceToDate.Format = DateTimePickerFormat.Custom;
                if (dtpSourceToDate.Value != dtpSrcFromDate.Value)
                {
                    dtpDestToDate.Enabled = false;
                    double datediff = (dtpSourceToDate.Value.Date - dtpSrcFromDate.Value.Date).TotalDays;
                    dtpDestToDate.Value = dtpDestFromDate.Value.Date.AddDays(datediff);
                }
                if (dtpSourceToDate.Value.Date == dtpSrcFromDate.Value.Date)
                {
                    dtpDestToDate.Enabled = true;
                    dtpDestToDate.Value = dtpDestFromDate.Value;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodExit();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates Date Range before Copying Recipe's
        /// </summary>
        private bool ValidateRecipe()
        {
            log.LogMethodEntry();
            if (dtpSrcFromDate.Value.Date.Equals(dtpDestFromDate.Value.Date)
                && dtpSourceToDate.Value.Date.Equals(dtpDestToDate.Value.Date))
            {
                dtpDestToDate.Value = dtpSourceToDate.Value.AddDays(1);
            }
            if (dtpDestToDate.Value.Date < dtpDestFromDate.Value.Date
                || dtpSourceToDate.Value.Date < dtpSrcFromDate.Value.Date)
            {
                dtpSourceToDate.Value = dtpSrcFromDate.Value;
                dtpDestToDate.Enabled = true;
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1093), MessageContainerList.GetMessage(executionContext, 1095));
                return false;
            }
            if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2768, dtpSrcFromDate.Value.ToString(utilities.getDateFormat()),
                    dtpSourceToDate.Value.ToString(utilities.getDateFormat()), dtpDestFromDate.Value.ToString(utilities.getDateFormat()), dtpDestToDate.Value.ToString(utilities.getDateFormat()))
               , "Recipe Manufacturing", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                //&1 - &2 Recipe will be copied to &3 - &4 Do you want to proceed?
                log.LogMethodExit();
                return false;
            }
            log.LogMethodExit();
            return true;
        }

        private void dtpSrcFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dtpSourceToDate.Value = dtpSrcFromDate.Value;
            dtpDestFromDate.Value = dtpSrcFromDate.Value;
            dtpDestToDate.Value = dtpDestFromDate.Value.AddDays(1);
            log.LogMethodExit();
        }
    }
}
