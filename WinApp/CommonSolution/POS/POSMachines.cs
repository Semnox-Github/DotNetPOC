/********************************************************************************************
 * Project Name - POSMachines
 * Description  - Bussiness logic of POSMachines
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
  *1.00        27-Jan-2016   Raghuveera     Created 
 *1.01        30-Jun-2016   Jeevan         Modified -  Added Method  Users(string  LoginId)
 *2.00        21-Sep-2018   Mathew Ninan   Added new method to get Printers associated with POS.
 *                                         Added constructor to query based on POSMachineid.
*2.60         23-Jan-2019   Jagan Mohana    Modified - Created Constructor and  SaveUpdatePOSMachinesList Method
*             22-Feb-2019   Mushahid Faizan Modified - Added GetPOSMachinesDTOList for Getting All child records and
              04-Mar-2019   Indhu          Modified for Remote Shift Open/Close changes
              29-Mar-2019   Nagesh Badiger Modified-  Added  isactive parameter in GetPOSMachinesDTOList()
              25-May-2019   Nitin Pai      Modified for Guest App              
2.70          09-Jul-2019   Deeksha        Modified Save() method for Insert /Update returns DTO instead of Id
*                                          changed log.debug to log.logMethodEntry and log.logMethodExit
*             16-Jul-2019   Akshay G       Modified - GetPOSMachinesDTOList() and Added DeletePOSMachines(), DeletePOSMachinesList() method
*             24-Jul-2019   Jagan Mohana   Removed GetPOSMachinesDTOList() method and moved GetPOSCounterByRoleId() method to POSTypeListBL class
*2.70.2       14-Nov-2019   Girish Kundar  Modified method :  PopulatePrinterDetails() for adding the receipt printer at the end of the list
*2.70.2       18-Dec-2019   Jinto Thomas   Added parameter execution context for userbl declaration with userid 
*2.80         23-Apr-2020   Girish Kundar  Modified:  3 tier changes for REST API changes
*2.90.0      02-Jun-2020   Girish Kundar   Modified :  added posPaymentModeInclusionDTOList for Payment mode display enhancement
*2.100       22-Oct-2020   Girish Kundar   Modified : for POS UI redesign
*2.130.0     12-Jul-2021   Lakshminarayana Modified : Static menu enhancement
*2.140.0     14-Sep-2021   Deeksha         Modified : POS attendance, Provisional shift and Cash Drawer related changes
*2.140.0     11-Apr-2022   Girish Kundar   Modified : Issue Fix- Assigning default cashdrawer when there is no shift
*2.140.2     18-Apr-2022      Girish Kundar   Modified:  BOCA changes - Added new column WBModel to printer class
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
*2.140.5     28-Apr-2023      Rakshith Shetty    Added method call to PrintReceiptReport which is used to print the report source instead of generating the pdf and then printing.   
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using Semnox.Parafait.DisplayGroup;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Reports;
using System.Globalization;
using System.Globalization;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Printer.Cashdrawers;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// Bussiness Logic for POSMachine Class.
    /// </summary>
    public class POSMachines
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private POSMachineDTO posMachineDTO;

        /// <summary>
        /// Parameterized constructor of POSMachines Class
        /// </summary>
        private POSMachines(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates POSMachines Object using POSMachineDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="posMachineDTO">POSMachine DTO</param>
        public POSMachines(ExecutionContext executionContext, POSMachineDTO posMachineDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posMachineDTO);
            this.posMachineDTO = posMachineDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the POSMachine id as the parameter
        /// Would fetch the posMachine object from the database based on the id passed.
        /// </summary>
        public POSMachines(ExecutionContext executionContext, int posMachineId, bool loadChildRecords = false,
                           bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posMachineId, sqlTransaction);
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            posMachineDTO = posMachineDataHandler.GetPOSMachineDTO(posMachineId);
            if (posMachineDTO == null)
            {
                FireValidationException("posMachineId", 1743, Environment.MachineName);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the child record - POSPrinter, POSExclusion and Peripherals based on the POSMachine id.
        /// </summary>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            POSPrinterListBL pOSPrinterListBL = new POSPrinterListBL(executionContext);
            List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>> searchByPOSPrinterParams = new List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>>();
            searchByPOSPrinterParams.Add(new KeyValuePair<POSPrinterDTO.SearchByParameters, string>(POSPrinterDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (activeChildRecords)
            {
                searchByPOSPrinterParams.Add(new KeyValuePair<POSPrinterDTO.SearchByParameters, string>(POSPrinterDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            posMachineDTO.PosPrinterDtoList = pOSPrinterListBL.GetPOSPrinterDTOList(searchByPOSPrinterParams, true, sqlTransaction);

            posProductExclusionsListBL posProductExclusionsListBL = new posProductExclusionsListBL(executionContext);
            List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>> searchByPOSProductExclusionParams = new List<KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>>();
            searchByPOSProductExclusionParams.Add(new KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>(POSProductExclusionsDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (activeChildRecords)
            {
                searchByPOSProductExclusionParams.Add(new KeyValuePair<POSProductExclusionsDTO.SearchByParameters, string>(POSProductExclusionsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            posMachineDTO.PosProductExclusionDtoList = posProductExclusionsListBL.GetPOSProductExclusionDTOList(searchByPOSProductExclusionParams, true, activeChildRecords, sqlTransaction);

            // load child records for - ProductDisplayGroup
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchByProductDisplayGroupFormatParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchByProductDisplayGroupFormatParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (SiteContainerList.IsCorporate())
            {
                searchByProductDisplayGroupFormatParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, posMachineDTO.SiteId.ToString()));
            }
            if (activeChildRecords)
            {
                searchByProductDisplayGroupFormatParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "1"));
            }
            posMachineDTO.PosProductDisplayList = productDisplayGroupList.GetProductGroupInclusionList(searchByProductDisplayGroupFormatParams, sqlTransaction);

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchByPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchByPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (activeChildRecords)
            {
                searchByPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            }
            posMachineDTO.PeripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchByPeripheralsParams);

            POSPaymentModeInclusionListBL posPaymentModeInclusionListBL = new POSPaymentModeInclusionListBL(executionContext);
            List<KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>> searchByPaymentModeInclusion = new List<KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>>();
            searchByPaymentModeInclusion.Add(new KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>(POSPaymentModeInclusionDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (activeChildRecords)
            {
                searchByPaymentModeInclusion.Add(new KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>(POSPaymentModeInclusionDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            posMachineDTO.POSPaymentModeInclusionDTOList = posPaymentModeInclusionListBL.GetPOSPaymentModeInclusionDTOList(searchByPaymentModeInclusion, true, sqlTransaction);

            ProductMenuPOSMachineMapListBL productMenuPOSMachineMapListBL = new ProductMenuPOSMachineMapListBL();
            List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList = productMenuPOSMachineMapListBL.GetProductMenuPanelMappingDTOList(new List<int>() { posMachineDTO.POSMachineId }, activeChildRecords, sqlTransaction);
            if(productMenuPOSMachineMapDTOList != null)
            {
                posMachineDTO.ProductMenuPOSMachineMapDTOList = productMenuPOSMachineMapDTOList;
            }

            ProductMenuPanelExclusionListBL productMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL();
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionList = productMenuPanelExclusionListBL.GetProductMenuPanelExclusionDTOListForPOSMachines(new List<int>() { posMachineDTO.POSMachineId }, activeChildRecords, sqlTransaction);
            if (productMenuPanelExclusionList != null)
            {
                posMachineDTO.ProductMenuPanelExclusionDTOList = productMenuPanelExclusionList;
            }
            // POS Cashdrawers
            POSCashdrawerListBL posCashdrawerListBL = new POSCashdrawerListBL(executionContext);
            List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>> searchByPOSCashdrawer = new List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>>();
            searchByPOSCashdrawer.Add(new KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>(POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            if (activeChildRecords)
            {
                searchByPOSCashdrawer.Add(new KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>(POSCashdrawerDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            posMachineDTO.POSCashdrawerDTOList = posCashdrawerListBL.GetPOSCashdrawerDTOList(searchByPOSCashdrawer, sqlTransaction);
            log.LogMethodExit();
        }

        private void FireValidationException(string errorAttribute, int errorMsgNo, string additionalMsgParam)
        {
            log.LogMethodEntry(errorAttribute, errorMsgNo, additionalMsgParam);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = (String.IsNullOrEmpty(additionalMsgParam) ? MessageContainerList.GetMessage(this.executionContext, errorMsgNo) : MessageContainerList.GetMessage(this.executionContext, errorMsgNo, Environment.MachineName));
            ValidationError validationError = new ValidationError("POSMachines", errorAttribute, errorMessage);
            validationErrorList.Add(validationError);
            log.LogMethodExit(validationErrorList);
            throw new ValidationException("POSMachine validation failed", validationErrorList);
        }

        /// <summary>
        /// Saves the POSMachine record
        /// Checks if the posMachine id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.IsChangedRecursive == false
                          && posMachineDTO.POSMachineId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            POSMachineDataHandler pOSMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            if (posMachineDTO.IsActive)
            {
                List<ValidationError> validationErrorList = ValidatePOSMachines();
                if (validationErrorList.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrorList.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrorList);
                }
                if (posMachineDTO.POSMachineId < 0)
                {
                    log.LogVariableState("POSMachineDTO", posMachineDTO);
                    posMachineDTO = pOSMachineDataHandler.InsertPOSMachine(posMachineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posMachineDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(posMachineDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("POSMachines", posMachineDTO.Guid,sqlTransaction);
                    }
                    // Add default cashdrawer if not exists 
                    MapDefaultCashdrawer(sqlTransaction);
                    AddManagementFormAccess(sqlTransaction);

                }
                else if (posMachineDTO.IsChanged)
                {
                    log.LogVariableState("POSMachineDTO", posMachineDTO);
                    POSMachineDTO existingPOSMachineDTO = new POSMachines(executionContext,posMachineDTO.POSMachineId).POSMachineDTO;
                    posMachineDTO = pOSMachineDataHandler.UpdatePOSMachine(posMachineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posMachineDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(posMachineDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("POSMachines", posMachineDTO.Guid,sqlTransaction);
                    }
                    if (existingPOSMachineDTO.POSName.ToLower().ToString() != posMachineDTO.POSName.ToLower().ToString())
                    {
                        RenameManagementFormAccess(existingPOSMachineDTO.POSName, sqlTransaction);
                    }
                    if (existingPOSMachineDTO.IsActive != posMachineDTO.IsActive)
                    {
                        UpdateManagementFormAccess(posMachineDTO.POSName, posMachineDTO.IsActive, posMachineDTO.Guid, sqlTransaction);
                    }
                }
                SavePOSPrinter(sqlTransaction);
                SavePOSProductExclusion(sqlTransaction);
                SavePeripherals(sqlTransaction);
                SavePOSPaymentInclusions(sqlTransaction);
                SaveProductMenuPOSMachineMaps(sqlTransaction);
                SaveProductMenuPanelExclusions(sqlTransaction);
                SavePOSCashDrawers(sqlTransaction);

            }
            else
            {
                if (posMachineDTO.POSMachineId >= 0)
                {
                    UpdateManagementFormAccess(posMachineDTO.POSName, posMachineDTO.IsActive, posMachineDTO.Guid, sqlTransaction);
                    DeletePOSMachines(sqlTransaction);
                }
                posMachineDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSMachineId > -1)
            {
                POSMachineDataHandler pOSMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
                pOSMachineDataHandler.AddManagementFormAccess(posMachineDTO.POSName, posMachineDTO.Guid, executionContext.GetSiteId(), posMachineDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSMachineId > -1)
            {
                POSMachineDataHandler pOSMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
                pOSMachineDataHandler.RenameManagementFormAccess(posMachineDTO.POSName, existingFormName, executionContext.GetSiteId(), posMachineDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSMachineId > -1)
            {
                POSMachineDataHandler pOSMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
                pOSMachineDataHandler.UpdateManagementFormAccess(formName, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void SaveProductMenuPOSMachineMaps(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.ProductMenuPOSMachineMapDTOList != null &&
                posMachineDTO.ProductMenuPOSMachineMapDTOList.Any())
            {
                List<ProductMenuPOSMachineMapDTO> updatedProductMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
                foreach (var productMenuPOSMachineMapDTO in posMachineDTO.ProductMenuPOSMachineMapDTOList)
                {
                    if (productMenuPOSMachineMapDTO.POSMachineId != posMachineDTO.POSMachineId)
                    {
                        productMenuPOSMachineMapDTO.POSMachineId = posMachineDTO.POSMachineId;
                    }
                    if (productMenuPOSMachineMapDTO.IsChanged)
                    {
                        updatedProductMenuPOSMachineMapDTOList.Add(productMenuPOSMachineMapDTO);
                    }
                }
                if (updatedProductMenuPOSMachineMapDTOList.Any())
                {
                    ProductMenuPOSMachineMapListBL productMenuPOSMachineMapListBL = new ProductMenuPOSMachineMapListBL(executionContext, updatedProductMenuPOSMachineMapDTOList);
                    productMenuPOSMachineMapListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void SaveProductMenuPanelExclusions(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.ProductMenuPanelExclusionDTOList != null &&
                posMachineDTO.ProductMenuPanelExclusionDTOList.Any())
            {
                List<ProductMenuPanelExclusionDTO> updatedProductMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
                foreach (var ProductMenuPanelExclusionDTO in posMachineDTO.ProductMenuPanelExclusionDTOList)
                {
                    if (ProductMenuPanelExclusionDTO.POSMachineId != posMachineDTO.POSMachineId)
                    {
                        ProductMenuPanelExclusionDTO.POSMachineId = posMachineDTO.POSMachineId;
                    }
                    if (ProductMenuPanelExclusionDTO.IsChanged)
                    {
                        updatedProductMenuPanelExclusionDTOList.Add(ProductMenuPanelExclusionDTO);
                    }
                }
                if (updatedProductMenuPanelExclusionDTOList.Any())
                {
                    ProductMenuPanelExclusionListBL ProductMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL(executionContext, updatedProductMenuPanelExclusionDTOList);
                    ProductMenuPanelExclusionListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
        private void SavePOSCashDrawers(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSCashdrawerDTOList != null &&
                posMachineDTO.POSCashdrawerDTOList.Any())
            {
                List<POSCashdrawerDTO> updatedPOSCashdrawerDTOList = new List<POSCashdrawerDTO>();
                foreach (var posCashdrawerDTO in posMachineDTO.POSCashdrawerDTOList)
                {
                    if (posCashdrawerDTO.POSMachineId != posMachineDTO.POSMachineId)
                    {
                        posCashdrawerDTO.POSMachineId = posMachineDTO.POSMachineId;
                    }
                    if (posCashdrawerDTO.IsChanged)
                    {
                        updatedPOSCashdrawerDTOList.Add(posCashdrawerDTO);
                    }
                }
                if (updatedPOSCashdrawerDTOList.Any())
                {
                    POSCashdrawerListBL posCashdrawerListBL = new POSCashdrawerListBL(executionContext, updatedPOSCashdrawerDTOList);
                    posCashdrawerListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method assigns the default cashdrawer when New POS machine being created.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void MapDefaultCashdrawer(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSCashdrawerDTOList == null || posMachineDTO.POSCashdrawerDTOList.Any() == false)
            {
                // Get the system cashdrawer. There will be only one system cashdrawer
                CashdrawerListBL cashdrawerListBL = new CashdrawerListBL(executionContext);
                List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> cashdrawerSearchParams = new List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>>();
                cashdrawerSearchParams.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                cashdrawerSearchParams.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.IS_ACTIVE, "1"));
                cashdrawerSearchParams.Add(new KeyValuePair<CashdrawerDTO.SearchByParameters, string>(CashdrawerDTO.SearchByParameters.IS_SYSTEM, "1"));
                List<CashdrawerDTO> cashdrawerDTOList = cashdrawerListBL.GetCashdrawers(cashdrawerSearchParams, sqlTransaction);
                int cashdrawerId = (cashdrawerDTOList == null || cashdrawerDTOList.Any()== false) ? -1 : cashdrawerDTOList.FirstOrDefault().CashdrawerId;
                log.Debug("cashdrawerId :" + cashdrawerId);
                if (cashdrawerId > -1)
                {
                    POSCashdrawerDTO pOSCashdrawerDTO = new POSCashdrawerDTO(-1, cashdrawerId, posMachineDTO.POSMachineId, true);
                    pOSCashdrawerDTO.IsActive = false;
                    POSCashdrawerBL posCashdrawerBL = new POSCashdrawerBL(executionContext, pOSCashdrawerDTO);
                    posCashdrawerBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void SavePOSPaymentInclusions(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.POSPaymentModeInclusionDTOList != null &&
                posMachineDTO.POSPaymentModeInclusionDTOList.Any())
            {
                List<POSPaymentModeInclusionDTO> updatedPOSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
                foreach (var posPaymentModeInclusionDTO in posMachineDTO.POSPaymentModeInclusionDTOList)
                {
                    if (posPaymentModeInclusionDTO.POSMachineId != posMachineDTO.POSMachineId)
                    {
                        posPaymentModeInclusionDTO.POSMachineId = posMachineDTO.POSMachineId;
                    }
                    if (posPaymentModeInclusionDTO.IsChanged)
                    {
                        updatedPOSPaymentModeInclusionDTOList.Add(posPaymentModeInclusionDTO);
                    }
                }
                if (updatedPOSPaymentModeInclusionDTOList.Any())
                {
                    POSPaymentModeInclusionListBL posPaymentModeInclusionListBL = new POSPaymentModeInclusionListBL(executionContext, updatedPOSPaymentModeInclusionDTOList);
                    posPaymentModeInclusionListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the child records : POSPrinterDTOList 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        private void SavePOSPrinter(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.PosPrinterDtoList != null &&
                posMachineDTO.PosPrinterDtoList.Any())
            {
                List<POSPrinterDTO> updatedPOSPrinterDTOList = new List<POSPrinterDTO>();
                foreach (var pOSPrinterDTO in posMachineDTO.PosPrinterDtoList)
                {
                    if (pOSPrinterDTO.POSMachineId != posMachineDTO.POSMachineId)
                    {
                        pOSPrinterDTO.POSMachineId = posMachineDTO.POSMachineId;
                    }
                    if (pOSPrinterDTO.IsChanged)
                    {
                        updatedPOSPrinterDTOList.Add(pOSPrinterDTO);
                    }
                }
                if (updatedPOSPrinterDTOList.Any())
                {
                    POSPrinterListBL pOSPrinterListBL = new POSPrinterListBL(executionContext, updatedPOSPrinterDTOList);
                    pOSPrinterListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the child records : POSProductExclusionDTOList 
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        private void SavePOSProductExclusion(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.PosProductExclusionDtoList != null &&
                posMachineDTO.PosProductExclusionDtoList.Any())
            {
                List<POSProductExclusionsDTO> updatedPOSProductExclusionDTOList = new List<POSProductExclusionsDTO>();
                foreach (var pOSProductExclusionDTO in posMachineDTO.PosProductExclusionDtoList)
                {
                    if (pOSProductExclusionDTO.PosMachineId != posMachineDTO.POSMachineId)
                    {
                        pOSProductExclusionDTO.PosMachineId = posMachineDTO.POSMachineId;
                    }
                    if (pOSProductExclusionDTO.IsChanged)
                    {
                        updatedPOSProductExclusionDTOList.Add(pOSProductExclusionDTO);
                    }
                }
                if (updatedPOSProductExclusionDTOList.Any())
                {
                    posProductExclusionsListBL posProductExclusionsListBL = new posProductExclusionsListBL(executionContext, updatedPOSProductExclusionDTOList);
                    posProductExclusionsListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Save the child records : PeripheralsDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        private void SavePeripherals(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posMachineDTO.PeripheralsDTOList != null &&
                posMachineDTO.PeripheralsDTOList.Any())
            {
                List<PeripheralsDTO> updatedPeripheralsDTOList = new List<PeripheralsDTO>();
                foreach (var peripheralsDTO in posMachineDTO.PeripheralsDTOList)
                {
                    if (peripheralsDTO.PosMachineId != posMachineDTO.POSMachineId)
                    {
                        peripheralsDTO.PosMachineId = posMachineDTO.POSMachineId;
                    }
                    if (peripheralsDTO.IsChanged)
                    {
                        updatedPeripheralsDTOList.Add(peripheralsDTO);
                    }
                }
                if (updatedPeripheralsDTOList.Any())
                {
                    PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext, updatedPeripheralsDTOList);
                    peripheralsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// PopulatePrinterDetails
        /// </summary>
        /// <param name=""></param>
        /// <returns>returns list of POS Printers for specific POS machine</returns>
        public List<POSPrinterDTO> PopulatePrinterDetails()
        {
            if (executionContext == null)
                return null;
            POSPrinterListBL posPrinterListBL = new POSPrinterListBL(executionContext);
            List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>> searchProductsParams = new List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>>();
            searchProductsParams.Add(new KeyValuePair<POSPrinterDTO.SearchByParameters, string>(POSPrinterDTO.SearchByParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
            searchProductsParams.Add(new KeyValuePair<POSPrinterDTO.SearchByParameters, string>(POSPrinterDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<POSPrinterDTO> posPrinterDTOList = posPrinterListBL.GetPOSPrinterDTOList(searchProductsParams, true);
            PrinterDTO printerProcessDTO = new PrinterDTO();
            if (posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                foreach (POSPrinterDTO posPrinterDTOProcessList in posPrinterDTOList)
                {
                    if (posPrinterDTOProcessList.PrinterDTO.PrinterId == printerProcessDTO.PrinterId)
                        posPrinterDTOProcessList.PrinterDTO.PrintableProductIds = printerProcessDTO.PrintableProductIds;
                    else
                    {
                        PrinterBL printerProcessBL = new PrinterBL(executionContext, posPrinterDTOProcessList.PrinterDTO);
                        printerProcessBL.GetPrinterProducts();
                        printerProcessDTO = posPrinterDTOProcessList.PrinterDTO;
                        printerProcessDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                //create PrinterDTO with Default when no printers are mapped. This is required to print Transaction receipt
                PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1,-1, 0);
                //get default value for Receipt Template ID based on configuration RECEIPT_PRINT_TEMPLATE
                int printTemplateId = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RECEIPT_PRINT_TEMPLATE"));
                ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, printTemplateId, true).ReceiptPrintTemplateHeaderDTO;
                POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, posMachineDTO.POSMachineId, -1, -1, -1, -1, printTemplateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                if (posPrinterDTOList == null)
                    posPrinterDTOList = new List<POSPrinterDTO>();
                posPrinterDTOList.Add(posPrinterDTO);
            }

            // returns the list with receipt printer is at the last.
            List<POSPrinterDTO> receiptPrinterDTOList = new List<POSPrinterDTO>();
            receiptPrinterDTOList = posPrinterDTOList.FindAll(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
            log.LogVariableState("receiptPrinterDTOList", receiptPrinterDTOList);
            foreach (POSPrinterDTO receiptPOSPrinterDTO in receiptPrinterDTOList)
            {
                posPrinterDTOList.Remove(receiptPOSPrinterDTO);
                posPrinterDTOList.Add(receiptPOSPrinterDTO);
            }
            log.LogMethodExit(posPrinterDTOList);
            return posPrinterDTOList;
        }

        /// <summary>
        /// Get Last Trx No of POS machine
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>Last Trx No for POS machine</returns>
        public int GetLastTrxNo(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            int lastTrxNo;
            if (posMachineDTO == null)
            {
                lastTrxNo = posMachineDataHandler.GetLastTrxNo(-1);
                log.LogMethodExit(lastTrxNo);
                return lastTrxNo;
            }
            else
            {
                lastTrxNo = posMachineDataHandler.GetLastTrxNo(posMachineDTO.POSMachineId);
                log.LogMethodExit(lastTrxNo);
                return lastTrxNo;
            }
        }

        /// <summary>
        /// ValidatePOS
        /// </summary>
        /// <param name="macAddress">macAddress</param>
        /// <returns>returns bool type status flag of the result</returns>
        public bool ValidatePOS(string macAddress)
        {
            log.LogMethodEntry(macAddress);

            bool posMachineStatus = false;
            string keyMessage = "";
            try
            {
                string connstring = "";
                using (Utilities parafaitUtility = new Utilities(connstring))
                {
                    KeyManagement keymgmt = new KeyManagement(parafaitUtility.DBUtilities, parafaitUtility.ParafaitEnv);
                    if (!keymgmt.validateLicense(ref keyMessage))
                        throw new Exception(keyMessage);

                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, macAddress));

                    List<POSMachineDTO> POSMachineDTOList = new POSMachineList(executionContext).GetAllPOSMachines(searchParameters);
                    if (POSMachineDTOList == null || POSMachineDTOList.Any() == false)
                    {
                        throw new Exception("POS is not setup in the Parafait system");
                    }
                    //if (POSMachineDTOList.Count <= 0)
                    //{
                    //    throw new Exception("POS is not setup in the Parafait system");
                    //}

                    //AlohaUtility alohaUtility = new AlohaUtility();
                    //bool isAlohaEnv = alohaUtility.IsAlohaDevice(parafaitUtility);
                    //if (isAlohaEnv == true)
                    //{
                    //    int terminalId = alohaUtility.GetAlohaTermId(parafaitUtility);
                    //    ParafaitAlohaIntegChannelFactory channelFactory = new ParafaitAlohaIntegChannelFactory(false);
                    //    IParafaitAlohaIntegrationService httpProxy = channelFactory.CreateChannel();
                    //    httpProxy.ValidateAloha(terminalId);
                    //    channelFactory.Close();
                    //}

                    posMachineStatus = true;
                }
            }
            catch
            {
                posMachineStatus = false;
            }

            log.LogMethodExit(posMachineStatus);

            return posMachineStatus;
        }

        /// <summary>
        /// Delete the POSMachines based on posMachineId
        /// </summary>
        /// <param name="posMachineId"></param>        
        /// <param name="sqlTransaction"></param>        
        private void DeletePOSMachines(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (posMachineDTO.PosPrinterDtoList != null && posMachineDTO.PosPrinterDtoList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SavePOSPrinter(sqlTransaction);
                if (posMachineDTO.PosProductExclusionDtoList != null && posMachineDTO.PosProductExclusionDtoList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SavePOSProductExclusion(sqlTransaction);
                if (posMachineDTO.PeripheralsDTOList != null && posMachineDTO.PeripheralsDTOList.Any(x => x.Active))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SavePeripherals(sqlTransaction);
                if (posMachineDTO.POSPaymentModeInclusionDTOList != null && posMachineDTO.POSPaymentModeInclusionDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SavePOSPaymentInclusions(sqlTransaction);
                if (posMachineDTO.ProductMenuPOSMachineMapDTOList != null && posMachineDTO.ProductMenuPOSMachineMapDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SaveProductMenuPOSMachineMaps(sqlTransaction);

                if (posMachineDTO.ProductMenuPanelExclusionDTOList != null && posMachineDTO.ProductMenuPanelExclusionDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SaveProductMenuPanelExclusions(sqlTransaction);

                if (posMachineDTO.POSCashdrawerDTOList != null && posMachineDTO.POSCashdrawerDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SavePOSCashDrawers(sqlTransaction);

                if (posMachineDTO.POSMachineId >= 0)
                {
                    POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
                    posMachineDataHandler.Delete(posMachineDTO.POSMachineId);
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        private List<ValidationError> ValidatePOSMachines(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, posMachineDTO.POSName));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            List<POSMachineDTO> pOSMachineDTOList = new POSMachineList(executionContext).GetAllPOSMachines(searchParameters);

            if (posMachineDTO.POSMachineId < 0)
            {
                if (pOSMachineDTOList != null && pOSMachineDTOList.Any())
                {
                    validationErrorList.Add(new ValidationError("POS Machine", "POS Machine", MessageContainerList.GetMessage(executionContext, 1900))); /// Duplicate pos machine not allowed
                }
            }
            else
            {
                if (pOSMachineDTOList != null && pOSMachineDTOList.Count > 1)
                {
                    validationErrorList.Add(new ValidationError("POS Machine", "POS Machine", MessageContainerList.GetMessage(executionContext, 1900))); /// Duplicate pos machine not allowed
                }
            }
            return validationErrorList;
        }

        public List<ValidationError> ValidateLicensedPOSMachines(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            int licensedNumberOfPOSMachines = 0;
            POSMachineList pOSMachineList = new POSMachineList(executionContext);
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
            List<POSMachineDTO> pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false, sqlTransaction);
            if (pOSMachineDTOList != null && pOSMachineDTOList.Any())
            {
                licensedNumberOfPOSMachines = CanAddPOSMachines(sqlTransaction);
                if (pOSMachineDTOList.Count > licensedNumberOfPOSMachines)
                {
                    validationErrorList.Add(new ValidationError("NoOfLicensedPOSMachines", "NoOfPOSMachines", MessageContainerList.GetMessage(executionContext, 1871))); /// Number of registered POS Machines is more than the number of licensed POS Machines
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Number of registered POS Machines is more than the number of licensed POS Machines
        /// </summary>
        private int CanAddPOSMachines(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int licensedNumberOfPOSMachines = 0;
            int siteId = executionContext.GetSiteId();
            string siteKey = "";
            string dummy = "";
            log.LogVariableState("siteId", siteId);
            POSTypeDataHandler pOSTypeDataHandler = new POSTypeDataHandler(sqlTransaction);
            pOSTypeDataHandler.ReadKeysFromDB(siteId, ref siteKey, ref dummy);
            if (siteKey == "")
            {
                licensedNumberOfPOSMachines = 0;
                log.LogMethodExit("siteKey == ''");
                return licensedNumberOfPOSMachines;
            }
            try
            {
                var noOfPOSMachinesLicensedCode = pOSTypeDataHandler.NoOfPOSMachinesLicensed(siteId);
                if (noOfPOSMachinesLicensedCode != null)
                {
                    string codeValue = Encryption.Decrypt(noOfPOSMachinesLicensedCode.ToString());
                    string[] codeValueArray = codeValue.Split('|');
                    if (codeValueArray[0] == siteKey)
                    {
                        licensedNumberOfPOSMachines = Convert.ToInt32(codeValueArray[1]);
                    }
                }
                else
                {
                    log.Error("null code");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                licensedNumberOfPOSMachines = 0;
            }
            log.LogMethodExit(licensedNumberOfPOSMachines);
            return licensedNumberOfPOSMachines;
        }

        public void RunXReport(DateTime shiftTime, int userId)
        {
            log.LogMethodEntry();
            ReportsList reportsList = new ReportsList(executionContext);
            List<ReportsDTO> reportsDTOList = null;
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            DateTime shiftEnd = ServerDateTime.Now;//Convert.ToDateTime(txttb_logoutdate.Text);
            DateTime shiftBegin = shiftTime;
            try
            {
                //Get the receipt report POSXReceiptCustom or POSXReceipt                           
                string posxReportKey = "POSXReceipt";
                reportsDTOList = reportsList.GetReportDTOWithCustomKey(posxReportKey);
                if (reportsDTOList != null && reportsDTOList.Count > 0)
                {
                    List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
                    reportParam.Add(new clsReportParameters.SelectedParameterValue("posname", posMachineDTO.POSName));
                    reportParam.Add(new clsReportParameters.SelectedParameterValue("userid", userId));
                    ReceiptReports receiptReports = new ReceiptReports(executionContext, "POSXReceipt", "", posMachineDTO.XReportRunTime.Equals(DateTime.MinValue) ? shiftBegin
                                                                        : posMachineDTO.XReportRunTime, shiftEnd, reportParam, "P");
                    receiptReports.PrintReceiptReport();

                    posMachineDTO.XReportRunTime = shiftEnd;
                    Save();

                    DateTime startTime = ((posMachineDTO.XReportRunTime.Equals(DateTime.MinValue)) ? shiftBegin : posMachineDTO.XReportRunTime);
                    POSMachineReportLogDTO pOSMachineReportLogDTO = new POSMachineReportLogDTO(-1, posMachineDTO.POSName, reportsDTOList[0].ReportId, startTime, shiftEnd, true, null);
                    POSMachineReportLogDataHandler pOSMachineReportLogDataHandler = new POSMachineReportLogDataHandler();
                    pOSMachineReportLogDataHandler.Insert(pOSMachineReportLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Exception :" + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// AssignCashdrawer - This method assigns the cashdrawer to shift or returns the default cashdrawer to POS
        /// </summary>
        /// <returns></returns>
        public POSCashdrawerDTO AutomaticAssignCashdrawer()
        {
            log.LogMethodEntry();
            POSCashdrawerDTO pOSCashdrawerDTO = new POSCashdrawerDTO();
            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CASHDRAWER_INTERFACE_MODE");
            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
            string cashdrawerAssignmentMode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CASHDRAWER_ASSIGNMENT_MODE");
            log.Debug("cashdrawerAssignmentMode :" + cashdrawerAssignmentMode);
            bool cashdrawerMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CASHDRAWER_ASSIGNMENT_MANDATORY_FOR_TRX");
            log.Debug("cashdrawerMandatory :" + cashdrawerMandatory);
            POSCashdrawerListBL posCashdrawerListBL = new POSCashdrawerListBL(executionContext);
            List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>> searchByPOSCashdrawer = new List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>>();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.SiteId, this.posMachineDTO.POSName, "", -1);

            List<ShiftDTO> openShiftDTOList = GetAllOpenShifts();
            if(openShiftDTOList == null
                && cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE)
                && (pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null && pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any()))
            {
                log.Debug("No shifts for the POS.  Assigning default cashdrawer to POS");
                POSCashdrawerContainerDTO posCashdrawerContainerDTO = pOSMachineContainerDTO.POSCashdrawerContainerDTOList
                                                                          .Where(x => x.IsActive).FirstOrDefault();
                pOSCashdrawerDTO = new POSCashdrawerDTO(posCashdrawerContainerDTO.POSCashdrawerId,
                                                                  posCashdrawerContainerDTO.CashdrawerId,
                                                                  posCashdrawerContainerDTO.POSMachineId,
                                                                  posCashdrawerContainerDTO.IsActive);
                log.LogMethodExit(pOSCashdrawerDTO);
                return pOSCashdrawerDTO;
            }
            if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE))
            {
                log.LogMethodExit(pOSCashdrawerDTO);
                return pOSCashdrawerDTO;
            }
            else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
            {
                if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                {
                    // Get always system cashdrawer from the list first record. Always only one system cashdrawer either serial or receipt
                    POSCashdrawerContainerDTO posCashdrawerContainerDTO = pOSMachineContainerDTO.POSCashdrawerContainerDTOList
                                                                           .Where(x => x.IsActive).FirstOrDefault();
                    var openShiftDTO = openShiftDTOList.Where(s => s.ShiftLoginId == executionContext.UserId).FirstOrDefault();
                    if (openShiftDTO != null)
                    {
                        if (openShiftDTO != null && openShiftDTO.CashdrawerId > -1)
                        {
                            log.Debug("Cashdrawer is already Assigned .");
                            pOSCashdrawerDTO = new POSCashdrawerDTO(posCashdrawerContainerDTO.POSCashdrawerId,
                                                              openShiftDTO.CashdrawerId,
                                                              posCashdrawerContainerDTO.POSMachineId,
                                                              posCashdrawerContainerDTO.IsActive);
                        }
                        else
                        {
                            using (NoSynchronizationContextScope.Enter())
                            {
                                IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                                CashdrawerActivityDTO cashdrawerActivityDTO = new CashdrawerActivityDTO(posCashdrawerContainerDTO.CashdrawerId, string.Empty);
                                Task<ShiftDTO> task = shiftUseCases.AssignCashdrawer(openShiftDTO.ShiftKey, cashdrawerActivityDTO);
                                task.Wait();
                                log.LogVariableState("Updated ShiftDTO", task.Result);
                                pOSCashdrawerDTO = new POSCashdrawerDTO(posCashdrawerContainerDTO.POSCashdrawerId,
                                                              posCashdrawerContainerDTO.CashdrawerId,
                                                              posCashdrawerContainerDTO.POSMachineId,
                                                              posCashdrawerContainerDTO.IsActive);
                            }
                        }
                    }
                    else // shift not created . then return the default cashdrawer .
                    {
                        pOSCashdrawerDTO = new POSCashdrawerDTO(posCashdrawerContainerDTO.POSCashdrawerId,
                                                                  posCashdrawerContainerDTO.CashdrawerId,
                                                                  posCashdrawerContainerDTO.POSMachineId,
                                                                  posCashdrawerContainerDTO.IsActive);
                        log.LogMethodExit(pOSCashdrawerDTO);
                        return pOSCashdrawerDTO;
                    }
                }
            }
            else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE)
                     && cashdrawerAssignmentMode == CashdrawerAssignmentModes.AUTOMATIC.ToString())
            {
                if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                {
                    // Get always system cashdrawer from the list first record. In multi mode system cashdrawers should be inactive
                    // Get first active record 
                    int shiftId = -1;
                    foreach (POSCashdrawerContainerDTO posCashdrawerContainerDTO in pOSMachineContainerDTO.POSCashdrawerContainerDTOList)
                    {
                        // check if the cashdrawer is already assigned . If not assign
                        ShiftDTO currentShiftDTO = openShiftDTOList.Where(x => x.CashdrawerId == posCashdrawerContainerDTO.CashdrawerId
                                                                              && x.POSMachine == this.posMachineDTO.POSName).FirstOrDefault();
                        if (currentShiftDTO != null)
                        {
                            continue;
                        }
                        else
                        {
                            if (openShiftDTOList != null && openShiftDTOList.Any() && openShiftDTOList.Exists(x => x.ShiftLoginId == executionContext.UserId))
                            {
                                currentShiftDTO = openShiftDTOList.Find(x => x.ShiftLoginId == executionContext.UserId);
                            }
                            if (currentShiftDTO.ShiftKey > -1 && currentShiftDTO.CashdrawerId == -1)
                            {
                                using (NoSynchronizationContextScope.Enter())
                                {
                                    IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(executionContext);
                                    CashdrawerActivityDTO cashdrawerActivityDTO = new CashdrawerActivityDTO(posCashdrawerContainerDTO.CashdrawerId, string.Empty);
                                    Task<ShiftDTO> task = shiftUseCases.AssignCashdrawer(currentShiftDTO.ShiftKey, cashdrawerActivityDTO);
                                    task.Wait();
                                    log.LogVariableState("Updated ShiftDTO", task.Result);
                                    pOSCashdrawerDTO = new POSCashdrawerDTO(posCashdrawerContainerDTO.POSCashdrawerId,
                                                                 posCashdrawerContainerDTO.CashdrawerId,
                                                                 posCashdrawerContainerDTO.POSMachineId,
                                                                 posCashdrawerContainerDTO.IsActive);
                                    log.LogMethodExit(pOSCashdrawerDTO);
                                    return pOSCashdrawerDTO;
                                }
                            }
                        }
                    }
                }
            }
            else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE)
                     && cashdrawerAssignmentMode == CashdrawerAssignmentModes.MANUAL.ToString())
            {
                log.Debug("CASHDRAWER_ASSIGNMENT_MODE is set as MANUAL and cashdrawer is not mapped to the POS");
                log.Debug("Manual Assignment to be done at POS tasks");
            }
            log.LogMethodExit(pOSCashdrawerDTO);
            return pOSCashdrawerDTO;
        }

        public List<ShiftDTO> GetAllOpenShifts(int numberOfDays = -7)
        {
            log.LogMethodEntry();
            List<ShiftDTO> openShiftListDTO;
            ShiftListBL shiftListBL = new ShiftListBL(executionContext);
            List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION, ShiftDTO.ShiftActionType.Open.ToString()));
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (ServerDateTime.Now.AddDays(numberOfDays).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, posMachineDTO.POSName.ToString()));
            openShiftListDTO = shiftListBL.GetShiftDTOList(searchParams,true,true,null);
            log.LogMethodExit(openShiftListDTO);
            return openShiftListDTO;
        }

        /// <summary>
        /// Get the DTO
        /// </summary>
        public POSMachineDTO POSMachineDTO
        {
            get { return posMachineDTO; }
        }

        public ShiftDTO CreateNewShift(string userName, DateTime loginTime, string shiftApplication, double openingAmount, int cardCount,
                                    int shiftTicketNumber, string shiftRemarks, string loginId, double creditCardAmount, double ChequeAmount,
                                    double CouponAmount, DateTime? approvalTime, int userId, bool isRemoteOpenShift, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(userName, loginTime, shiftApplication, openingAmount, cardCount, shiftTicketNumber, shiftRemarks,
                                  loginId, creditCardAmount, ChequeAmount, CouponAmount, approvalTime, userId, sqlTransaction);
            if ((posMachineDTO != null && posMachineDTO.DayBeginTime.CompareTo(posMachineDTO.DayEndTime) < 0 || (posMachineDTO.DayBeginTime.Equals(DateTime.MinValue) && posMachineDTO.DayEndTime.Equals(DateTime.MinValue))))
            {
                posMachineDTO.DayBeginTime = DateTime.Today.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME")));
                Save(sqlTransaction);
            }
            Users user = new Users(executionContext, userId);
            ShiftDTO shiftDTO = user.CreateNewShift(userName, loginTime, shiftApplication, openingAmount, cardCount, shiftTicketNumber,
                                        shiftRemarks, loginId, creditCardAmount, ChequeAmount, CouponAmount, approvalTime, userId, posMachineDTO.POSName, isRemoteOpenShift, sqlTransaction);
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }

        public void CloseShift(int shiftId, int userId, DateTime loginTime, string loginId, double actualAmount, int cardCount, int actualTickets, string remarks, decimal shiftAmount,
                               string shiftApplication, int actualCards, decimal couponAmount, decimal creditCardAmount, decimal chequeAmount, decimal actualGameCardAmount, decimal actualcreditCardAmount
                               , decimal actualChequeAmount, decimal actualCouponAmount, decimal gamecardAmount, double shiftTicketnumber, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(shiftId, userId, loginTime, loginId, actualAmount, cardCount, actualTickets, remarks, shiftAmount,
                                shiftApplication, actualCards, couponAmount, creditCardAmount, chequeAmount, actualGameCardAmount, actualcreditCardAmount
                               , actualChequeAmount, actualCouponAmount, gamecardAmount);
            Users users = new Users(executionContext, userId);
            users.CloseShift(shiftId, actualAmount, cardCount, actualTickets, remarks, shiftAmount, shiftApplication, actualCards, couponAmount, creditCardAmount, chequeAmount, actualGameCardAmount,
                                actualcreditCardAmount, actualChequeAmount, actualCouponAmount, gamecardAmount, shiftTicketnumber,sqlTransaction);

            log.LogMethodExit();
        }

        public void ProvisionalClose(int shiftKey, int userId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(shiftKey, userId, sqlTransaction);
            Users users = new Users(executionContext, userId);
            users.ProvisionalClose(shiftKey, sqlTransaction);
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of posMachine DTOs
    /// </summary>
    public class POSMachineList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSMachineDTO> posMachinesList = new List<POSMachineDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSMachineList()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSMachineList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="posMachinesList"></param>
        /// <param name="executionContext"></param>
        public POSMachineList(ExecutionContext executionContext, List<POSMachineDTO> posMachinesList)
            : this(executionContext)
        {
            log.LogMethodEntry(posMachinesList, executionContext);
            this.posMachinesList = posMachinesList;
            log.LogMethodExit();
        }

        /// <summary>       
        /// Returns the posMachine list
        /// </summary>
        public POSMachineDTO GetPOSMachine(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler();
            POSMachineDTO pOSMachineDTO = new POSMachineDTO();
            pOSMachineDTO = posMachineDataHandler.GetPOSMachineDTO(posMachineId);
            log.LogMethodExit(pOSMachineDTO);
            return pOSMachineDTO;
        }

        /// <summary>
        /// Builds the List of POSMachine object based on the list of POSMachine id.
        /// </summary>
        /// <param name="pOSMachineDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<POSMachineDTO> pOSMachineDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, POSMachineDTO> pOSMachineDTOIdMap = new Dictionary<int, POSMachineDTO>();
            List<int> pOSMachineIdList = new List<int>();
            for (int i = 0; i < pOSMachineDTOList.Count; i++)
            {
                if (pOSMachineDTOIdMap.ContainsKey(pOSMachineDTOList[i].POSMachineId))
                {
                    continue;
                }
                pOSMachineDTOIdMap.Add(pOSMachineDTOList[i].POSMachineId, pOSMachineDTOList[i]);
                pOSMachineIdList.Add(pOSMachineDTOList[i].POSMachineId);
            }

            POSPrinterListBL pOSPrinterListBL = new POSPrinterListBL(executionContext);
            List<POSPrinterDTO> pOSPrinterDTOList = pOSPrinterListBL.GetPOSPrinterDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (pOSPrinterDTOList != null && pOSPrinterDTOList.Any())
            {
                for (int i = 0; i < pOSPrinterDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(pOSPrinterDTOList[i].POSMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[pOSPrinterDTOList[i].POSMachineId];
                    if (pOSMachineDTO.PosPrinterDtoList == null)
                    {
                        pOSMachineDTO.PosPrinterDtoList = new List<POSPrinterDTO>();
                    }
                    pOSMachineDTO.PosPrinterDtoList.Add(pOSPrinterDTOList[i]);
                }
            }

            posProductExclusionsListBL pOSProductExclusionsListBL = new posProductExclusionsListBL(executionContext);
            List<POSProductExclusionsDTO> pOSProductExclusionsDTOList = pOSProductExclusionsListBL.GetPOSProductExclusionsDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (pOSProductExclusionsDTOList != null && pOSProductExclusionsDTOList.Any())
            {
                for (int i = 0; i < pOSProductExclusionsDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(pOSProductExclusionsDTOList[i].PosMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[pOSProductExclusionsDTOList[i].PosMachineId];
                    if (pOSMachineDTO.PosProductExclusionDtoList == null)
                    {
                        pOSMachineDTO.PosProductExclusionDtoList = new List<POSProductExclusionsDTO>();
                    }
                    pOSMachineDTO.PosProductExclusionDtoList.Add(pOSProductExclusionsDTOList[i]);
                }
            }
            // load child records - Peripherals
            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext);
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (peripheralsDTOList != null && peripheralsDTOList.Any())
            {
                for (int i = 0; i < peripheralsDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(peripheralsDTOList[i].PosMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[peripheralsDTOList[i].PosMachineId];
                    if (pOSMachineDTO.PeripheralsDTOList == null)
                    {
                        pOSMachineDTO.PeripheralsDTOList = new List<PeripheralsDTO>();
                    }
                    pOSMachineDTO.PeripheralsDTOList.Add(peripheralsDTOList[i]);

                }
            }
            //loads display groups
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList();
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchByParameters;
            foreach (POSMachineDTO posMachineDTO in pOSMachineDTOList)
            {
                searchByParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchByParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
                if (SiteContainerList.IsCorporate())
                {
                    searchByParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, posMachineDTO.SiteId.ToString()));
                }
                if (activeChildRecords)
                {
                    searchByParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "1"));
                }
                List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTO = productDisplayGroupList.GetProductGroupInclusionList(searchByParameters, sqlTransaction);
                if (productDisplayGroupFormatDTO != null)
                {
                    posMachineDTO.PosProductDisplayList = new List<ProductDisplayGroupFormatDTO>(productDisplayGroupFormatDTO);
                }

            }
            //load pos payment inclusion list
            POSPaymentModeInclusionListBL pOSPaymentModeInclusionListBL = new POSPaymentModeInclusionListBL(executionContext);
            List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = pOSPaymentModeInclusionListBL.GetPOSPaymentModeInclusionDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
            {
                for (int i = 0; i < pOSPaymentModeInclusionDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(pOSPaymentModeInclusionDTOList[i].POSMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[pOSPaymentModeInclusionDTOList[i].POSMachineId];
                    if (pOSMachineDTO.POSPaymentModeInclusionDTOList == null)
                    {
                        pOSMachineDTO.POSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
                    }
                    pOSMachineDTO.POSPaymentModeInclusionDTOList.Add(pOSPaymentModeInclusionDTOList[i]);
                }
            }

            //load product menu pos machine mapping list
            ProductMenuPOSMachineMapListBL productMenuPOSMachineMapListBL = new ProductMenuPOSMachineMapListBL();
            List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList = productMenuPOSMachineMapListBL.GetProductMenuPanelMappingDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (productMenuPOSMachineMapDTOList != null && productMenuPOSMachineMapDTOList.Any())
            {
                for (int i = 0; i < productMenuPOSMachineMapDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(productMenuPOSMachineMapDTOList[i].POSMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[productMenuPOSMachineMapDTOList[i].POSMachineId];
                    if (pOSMachineDTO.ProductMenuPOSMachineMapDTOList == null)
                    {
                        pOSMachineDTO.ProductMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
                    }
                    pOSMachineDTO.ProductMenuPOSMachineMapDTOList.Add(productMenuPOSMachineMapDTOList[i]);
                }
            }

            //load product menu panel exclusion for pos machine list
            ProductMenuPanelExclusionListBL ProductMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL();
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = ProductMenuPanelExclusionListBL.GetProductMenuPanelExclusionDTOListForPOSMachines(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (productMenuPanelExclusionDTOList != null && productMenuPanelExclusionDTOList.Any())
            {
                for (int i = 0; i < productMenuPanelExclusionDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(productMenuPanelExclusionDTOList[i].POSMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[productMenuPanelExclusionDTOList[i].POSMachineId];
                    if (pOSMachineDTO.ProductMenuPanelExclusionDTOList == null)
                    {
                        pOSMachineDTO.ProductMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
                    }
                    pOSMachineDTO.ProductMenuPanelExclusionDTOList.Add(productMenuPanelExclusionDTOList[i]);
                }
            }

            // Load cashdrawers
            POSCashdrawerListBL posCashdrawerListBL = new POSCashdrawerListBL(executionContext);
            List<POSCashdrawerDTO> posCashdrawerDTOList = posCashdrawerListBL.GetPOSCashdrawerDTOList(pOSMachineIdList, activeChildRecords, sqlTransaction);
            if (posCashdrawerDTOList != null && posCashdrawerDTOList.Any())
            {
                for (int i = 0; i < posCashdrawerDTOList.Count; i++)
                {
                    if (pOSMachineDTOIdMap.ContainsKey(posCashdrawerDTOList[i].POSMachineId) == false)
                    {
                        continue;
                    }
                    POSMachineDTO pOSMachineDTO = pOSMachineDTOIdMap[posCashdrawerDTOList[i].POSMachineId];
                    if (pOSMachineDTO.POSCashdrawerDTOList == null)
                    {
                        pOSMachineDTO.POSCashdrawerDTOList = new List<POSCashdrawerDTO>();
                    }
                    pOSMachineDTO.POSCashdrawerDTOList.Add(posCashdrawerDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// Returns the posMachine list
        /// </summary>
        public List<POSMachineDTO> GetAllPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters,
                                                      bool loadChildRecords = false, bool loadActiveChildRecords = false,
                                                      SqlTransaction sqlTransaction = null)
        {
            // child records needs to be build
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            List<POSMachineDTO> posMachineList = posMachineDataHandler.GetPOSMachineList(searchParameters);
            if (posMachineList != null && posMachineList.Any() && loadChildRecords)
            {
                Build(posMachineList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodEntry(posMachineList);
            return posMachineList;
        }

        /// <summary>
        /// Fetches POS Machines List under a User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public List<POSMachineDTO> GetPOSMachineListByUserId(int userId, ExecutionContext executionContext)
        {
            //Users users = new Users(executionContext);
            Users users = new Users(executionContext, userId);
            List<POSMachineDTO> pOSMachineDTOList = new List<POSMachineDTO>();
            List<POSTypeDTO> pOSTypeDTOList = new List<POSTypeDTO>();
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
            //Fetches all POS Counters and respective POS Machine
            pOSTypeDTOList = pOSTypeListBL.GetPOSCounterByRoleId(users.UserDTO.RoleId, executionContext);
            if (pOSTypeDTOList != null && pOSTypeDTOList.Any())
            {
                pOSMachineDTOList = GetPOSMachinesByPOSCounter(pOSTypeDTOList);
            }

            if (pOSMachineDTOList == null)
                return pOSMachineDTOList;

            //Fetches all POS Machine
            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, "POS Machine"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, "1"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, users.UserDTO.RoleId.ToString()));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
            List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
            if (managementFormAccessDTOList != null && managementFormAccessDTOList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < managementFormAccessDTOList.Count; i++)
                {
                    // stringBuilder.Append("'");
                    stringBuilder.Append(managementFormAccessDTOList[i].FormName);
                    // stringBuilder.Append("'");
                    if (managementFormAccessDTOList.Count > 1 && i != managementFormAccessDTOList.Count - 1)
                        stringBuilder.Append(",");
                }
                POSMachineList posMachineList = new POSMachineList(executionContext);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME_LIST, stringBuilder.ToString()));
                List<POSMachineDTO> posMachinesList = posMachineList.GetAllPOSMachines(searchParameters);
                if (posMachinesList != null && posMachinesList.Any())
                {
                    List<POSMachineDTO> temp = posMachinesList.Where(x => pOSMachineDTOList.Any(y => y.POSMachineId == x.POSMachineId) == false).ToList();
                    //Checks if POS Machine is already added
                    pOSMachineDTOList.AddRange(temp);
                }
            }
            log.LogMethodExit(pOSMachineDTOList);
            return pOSMachineDTOList;
        }

        /// <summary>
        /// Get POSMachines by RoleID
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        private List<POSMachineDTO> GetPOSMachinesByPOSCounter(List<POSTypeDTO> pOSTypeDTOList)
        {
            List<POSMachineDTO> pOSMachineDTOList = new List<POSMachineDTO>();

            StringBuilder pOSTypeDTOStringBuilder = new StringBuilder();
            for (int i = 0; i < pOSTypeDTOList.Count; i++)
            {
                pOSTypeDTOStringBuilder.Append(pOSTypeDTOList[i].POSTypeId);
                if (pOSTypeDTOList.Count > 1 && pOSTypeDTOList.Count - 1 != i)
                    pOSTypeDTOStringBuilder.Append(",");
            }

            POSMachineList posMachineList = new POSMachineList(executionContext);
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> POSMachinesearchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            POSMachinesearchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_TYPE_ID_LIST, pOSTypeDTOStringBuilder.ToString()));
            List<POSMachineDTO> posMachinesList = posMachineList.GetAllPOSMachines(POSMachinesearchParameters);
            if (posMachinesList != null && posMachinesList.Any())
            {
                pOSMachineDTOList.AddRange(posMachinesList);
            }
            return pOSMachineDTOList;
        }

        /// <summary>
        /// Returns the posMachine list with all Child Lists
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<POSMachineDTO> GetPOSMachinesDTOList(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<POSMachineDTO> posMachineList;
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);

            posMachineList = posMachineDataHandler.GetPOSMachineList(searchParameters);

            if (posMachineList != null && posMachineList.Any())
            {
                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchByParameters;
                foreach (POSMachineDTO posMachineDTO in posMachineList)
                {
                    searchByParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID, posMachineDTO.POSMachineId.ToString()));
                    searchByParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, posMachineDTO.SiteId.ToString()));
                    List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTO = productDisplayGroupList.GetOnlyUsedProductDisplayGroup(searchByParameters);
                    if (productDisplayGroupFormatDTO != null)
                    {
                        //posMachineDTO.PosProductDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>(productDisplayGroupFormatDTO);
                    }
                }
            }
            log.LogMethodEntry(posMachineList);
            return posMachineList;
        }



        /// <summary>
        /// Save or update Pos management machines
        /// </summary>
        public void Save(bool isLicensedPOSMachines = false)
        {
            log.LogMethodEntry();
            if (posMachinesList != null && posMachinesList.Any())
            {
                foreach (POSMachineDTO posMachineDto in posMachinesList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            POSMachines posMachinesObj = new POSMachines(executionContext, posMachineDto);
                            if (!isLicensedPOSMachines)
                            {
                                List<ValidationError> validationErrorList = posMachinesObj.ValidateLicensedPOSMachines(parafaitDBTrx.SQLTrx);
                                if (validationErrorList.Count > 0)
                                {
                                    throw new POSMachinesLicenseException(validationErrorList[0].Message);
                                }
                            }
                            posMachinesObj.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing Validation Exception -" + valEx.Message);
                            parafaitDBTrx.RollBack();
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();

                }
            }
        }

        public DateTime? GetPOSModuleLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            POSMachineDataHandler posMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            DateTime? result = posMachineDataHandler.GetPOSModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// It will return the List of OpenShifts
        /// </summary>
        /// <param name="pOSMachineDTOList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ShiftDTO> GetOpenShiftDTOList(List<POSMachineDTO> pOSMachineDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineDTOList, sqlTransaction);
            POSMachineDataHandler pOSMachineDataHandler = new POSMachineDataHandler(sqlTransaction);
            List<ShiftDTO> returnValue = pOSMachineDataHandler.GetOpenShiftDTOList(pOSMachineDTOList, sqlTransaction);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}