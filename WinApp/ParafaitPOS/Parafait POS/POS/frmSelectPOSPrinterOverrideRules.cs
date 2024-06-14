/********************************************************************************************
 * Project Name - Parafait_POS
 * Description  - frmSelectPOSPrinterOverrideRules form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Dakshak            Created for Peru invoice changes                                                                              
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using System.Threading;
using System.Reflection;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public partial class frmSelectPOSPrinterOverrideRules : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        POSPrinterDTO pOSPrintersDTO = null;
        SqlTransaction sqlTransaction = null;
        POSPrinterOverrideRulesDTO pOSSelectedPrinterOverrideRulesDTO = null;
        private Utilities utilities = POSStatic.Utilities;

        Transaction curTrx;
        bool defaultBtnFlag = false;
        public frmSelectPOSPrinterOverrideRules(POSPrinterDTO pOSPrintersDTO, Transaction NewTrx)
        {
            log.LogMethodEntry(pOSPrintersDTO);
            this.pOSPrintersDTO = pOSPrintersDTO;
            this.curTrx = NewTrx;
            InitializeComponent();
            POSStatic.Utilities.setLanguage(this);
            printerNameLbl.Text = pOSPrintersDTO.PrinterDTO.PrinterName;
            log.LogMethodExit();
        }

        private void frmSelectPOSPrinterOverrideRules_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadOptions();
            log.LogMethodExit();
        }
        private void LoadOptions()
        {
            log.LogMethodEntry();
            try
            {
                flpOptionsPanel.Controls.Clear();
                List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
                List<int> POSPrinterOverrideOptionsIdList = new List<int>();
                POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO = null;
                if (pOSPrintersDTO != null)
                {
                    List<POSPrinterOverrideRulesDTO> POSPrinterOverrideRulesDTOList = pOSPrintersDTO.POSPrinterOverrideRulesDTOList.GroupBy(ppordl => ppordl.POSPrinterOverrideOptionId).Select(ppor => ppor.First()).ToList();
                    if (POSPrinterOverrideRulesDTOList != null && POSPrinterOverrideRulesDTOList.Any() && POSPrinterOverrideRulesDTOList.Count == 1)
                    {
                        pOSSelectedPrinterOverrideRulesDTO = POSPrinterOverrideRulesDTOList[0];
                        SetOverrideOption();
                    }
                    else
                    {
                        foreach (POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO in POSPrinterOverrideRulesDTOList)
                        {
                            POSPrinterOverrideOptionsBL pOSPrinterOverrideOptionsBL = new POSPrinterOverrideOptionsBL(utilities.ExecutionContext, pOSPrinterOverrideRulesDTO.POSPrinterOverrideOptionId);
                            pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsBL.POSPrinterOverrideOptionsDTO;
                            createOverrideOptionsButton(pOSPrinterOverrideRulesDTO, pOSPrinterOverrideOptionsDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void createOverrideOptionsButton(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO, POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO)
        {
            log.LogMethodEntry();
            Button btnOption = new Button();
            btnOption.Text = pOSPrinterOverrideOptionsDTO.OptionName;
            btnOption.Name = pOSPrinterOverrideOptionsDTO.OptionName;
            btnOption.Tag = pOSPrinterOverrideRulesDTO;
            btnOption.FlatStyle = FlatStyle.Flat;
            btnOption.FlatAppearance.BorderSize = 0;
            btnOption.BackColor = Color.Gray;
            btnOption.BackgroundImageLayout = ImageLayout.Zoom;
            btnOption.Click += btnOption_Click;
            btnOption.Size = btnSample1.Size;
            btnOption.Font = btnSample1.Font;
            btnOption.ForeColor = btnSample1.ForeColor;
            flpOptionsPanel.Controls.Add(btnOption);
            if (defaultBtnFlag == false && pOSPrinterOverrideRulesDTO.DefaultOption == true)
            {
                btnOption.BackColor = Color.DarkBlue;
                pOSSelectedPrinterOverrideRulesDTO = pOSPrinterOverrideRulesDTO;
                defaultBtnFlag = true;
            }
            log.LogMethodExit();
        }
        private void btnOption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button btnOption = sender as Button;
                foreach (Control ctrl in flpOptionsPanel.Controls)
                {
                    ctrl.BackColor = Color.Gray;
                }
                if (btnOption != null)
                {
                    btnOption.BackColor = Color.DarkBlue;
                    pOSSelectedPrinterOverrideRulesDTO = (POSPrinterOverrideRulesDTO)btnOption.Tag;
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
            log.LogMethodEntry("Starts-btnClose_Click()");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit("Ends-btnClose_Click()");
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetOverrideOption();
            log.LogMethodExit();
        }

        private void SetOverrideOption()
        {
            log.LogMethodEntry();
            try
            {
                if (pOSSelectedPrinterOverrideRulesDTO != null)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    List<POSPrinterOverrideRulesDTO> selectedSetOfPPORDTOList = pOSPrintersDTO.POSPrinterOverrideRulesDTOList.Where(ppor => ppor.IsActive && ppor.POSPrinterOverrideOptionId == pOSSelectedPrinterOverrideRulesDTO.POSPrinterOverrideOptionId
                                                                                && ppor.POSPrinterId == pOSSelectedPrinterOverrideRulesDTO.POSPrinterId).ToList();
                    if (selectedSetOfPPORDTOList != null && selectedSetOfPPORDTOList.Any())
                    {
                        for (int i = 0; i < selectedSetOfPPORDTOList.Count; i++)
                        {
                            POSPrinterOverrideRulesDTO selectedDTO = selectedSetOfPPORDTOList[i];
                            TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = new TrxPOSPrinterOverrideRulesDTO(-1, curTrx.Trx_id, selectedDTO.POSPrinterId, selectedDTO.POSPrinterOverrideRuleId, selectedDTO.POSPrinterOverrideOptionId,
                                                                                                                            (POSPrinterOverrideOptionItemCode)Enum.Parse(typeof(POSPrinterOverrideOptionItemCode), selectedDTO.OptionItemCode),
                                                                                                                            selectedDTO.ItemSourceColumnGuid, true, selectedDTO.PrinterId);
                            curTrx.AddTrxPOSPrinterOverrideRulesDTO(trxPOSPrinterOverrideRulesDTO, sqlTransaction);
                        }
                    }

                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    log.Error("Please select one of the options to proceed");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2600));
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                displayMessageLine(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        //private void Validate()
        //{
        //    log.LogMethodEntry();
        //    POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO = null;
        //    POSPrinterOverrideOptionsBL pOSPrinterOverrideOptionsBL = new POSPrinterOverrideOptionsBL(utilities.ExecutionContext, pOSSelectedPrinterOverrideRulesDTO.POSPrinterOverrideOptionId);
        //    pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsBL.POSPrinterOverrideOptionsDTO;
        //    if (validationLookupValuesDTOList != null &&
        //        validationLookupValuesDTOList.Any() &&
        //        pOSPrinterOverrideOptionsDTO != null &&
        //        validationLookupValuesDTOList.Exists(vld => vld.LookupValue == pOSPrinterOverrideOptionsDTO.OptionName))
        //    {
        //        LookupValuesDTO lookupValuesDTO = validationLookupValuesDTOList.First(vld => vld.LookupValue == pOSPrinterOverrideOptionsDTO.OptionName);
        //        string[] validationItems = lookupValuesDTO.Description.Split('|');
        //        foreach (string item in validationItems)
        //        {
        //            ValidateItems(item);
        //        }
        //    }
        //    log.LogMethodExit();
        //}


        //private void ValidateItems(string validationItems)
        //{
        //    log.LogMethodEntry(validationItems);
        //    string[] validationParams = validationItems.Split('-');

        //    if (!(curTrx.customerDTO != null) || !(curTrx.customerDTO.ProfileDTO != null))
        //    {
        //        log.Error("Customer must be selected for the transaction");
        //        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2934));
        //    }
        //    if (validationParams[0] == "Profile")
        //    {
        //        if (!(curTrx.customerDTO.ProfileDTO != null))
        //        {
        //            log.Error("Customer profile is empty. Please enter a value.");
        //            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2934));
        //        }
        //        else
        //        {
        //            ValidateProperty(validationParams[1], curTrx.customerDTO.ProfileDTO);
        //        }
        //    }
        //    else if (validationParams[0] == "Address")
        //    {
        //        if (!(curTrx.customerDTO.ProfileDTO.AddressDTOList != null && curTrx.customerDTO.ProfileDTO.AddressDTOList.Any()))
        //        {
        //            log.Error("Address is mandatory. Please enter a value.");
        //            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 249, "AddressLine"));
        //        }
        //        else
        //        {
        //            ValidateProperty(validationParams[1], curTrx.customerDTO.ProfileDTO.AddressDTOList[0]);
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        //private void ValidateProperty(string validationItems, ProfileDTO profileDTO)
        //{
        //    log.LogMethodEntry(validationItems, profileDTO);
        //    Type t = curTrx.customerDTO.ProfileDTO.GetType();
        //    PropertyInfo p = t.GetProperty(validationItems);
        //    if (p == null || string.IsNullOrEmpty(p.GetValue(curTrx.customerDTO.ProfileDTO).ToString()))
        //    {
        //        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 249, validationItems));
        //    }
        //    log.LogMethodExit();
        //}

        //private void ValidateProperty(string validationItems, AddressDTO addressDTO)
        //{
        //    log.LogMethodEntry(validationItems, addressDTO);
        //    Type t = addressDTO.GetType();
        //    PropertyInfo p = t.GetProperty(validationItems);
        //    if (p == null || string.IsNullOrEmpty(p.GetValue(addressDTO).ToString()))
        //    {
        //        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 249, validationItems));
        //    }
        //    log.LogMethodExit();
        //}
        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }



    }

}
