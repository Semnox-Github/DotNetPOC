
/********************************************************************************************
 * Project Name - Printer BL
 * Description  - Business Logic class for Printer
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By    Remarks          
 *********************************************************************************************
*1.00        11-Sep-2017      Vinayaka V     Created 
 *2.00        18-Sep-2018      Mathew Ninan   Added Insert/Update and POSPrinter population
 *                                            logic
 *2.60        04-Feb-2019      Mushahid Faizan Added new SaveUpdatePOSPrinterList() and Save()                                                         
              31-May-2019      Jagan Mohan     Code merge from Development to WebManagementStudio
 *2.70        09-jul-2019      Deeksha        Changed log.debug to log.logMethodentry and
 *                                            log.logMethodExit.
 *            16-Jul-2019      Akshay G        Added DeletePOSPrinters() method
 *            17-jul-2019      Guru S A       Booking phase 2 changes
 *            04-Feb-2019      Mushahid Faizan Added new SaveUpdatePOSPrinterList() and Save()                                                         
 *            31-May-2019      Jagan Mohan     Code merge from Development to WebManagementStudio
 *            16-Jul-2019      Akshay G        Added DeletePOSPrinters() method
 *            14-Oct-2019      Jagan Mohan    UpdatePrinterType() method implemented in Save()
 *2.70.2        15-Nov- 2019    Girish Kundar   Added a method RemovePrinterType(printer type)
 *2.80        06-Mar-2020      Vikas Dwivedi   Modified as per the Standards for Phase 1 Changes.
 *2.110       11-Dec-2020      Dakshakh Raj    Modified: for Peru Invoice Enhancement
 *2.140       14-Sep-2021      Fiona           Modified: Issue fixes and Modified BL Constructor added null checks
*2.130.4     02-Mar-2022      Abhishek        WMS Fix: Modified Save to avoid hard deletion
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
//using Semnox.Parafait.Discounts;
using System.Drawing;
using System.Configuration;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// Printer BL class
    /// </summary>
    public class POSPrinterBL
    {
        private readonly ExecutionContext executionContext;
        private POSPrinterDTO posPrinterDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor Of POSPrinterBL class.
        /// </summary>
        private POSPrinterBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates POSPrinter object using the POSPrinterDTO
        /// </summary>
        /// <param name="posPrinterDTO">POSPrinterDTO object</param>
        /// <param name="executionContext"></param>
        public POSPrinterBL(ExecutionContext executionContext, POSPrinterDTO posPrinterDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(posPrinterDTO, executionContext);
            this.posPrinterDTO = posPrinterDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the POSPrinter id as the parameter
        /// Would fetch the POSPrinter object from the database based on the id passed.
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as the parameter</param>
        /// <param name="posPrinterId">id of POSPrinter Object</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction"></param>
        public POSPrinterBL(ExecutionContext executionContext, int posPrinterId, bool loadChildRecords,
            bool activeChildRecords, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posPrinterId, sqlTransaction);
            POSPrinterDataHandler posPrinterDataHandler = new POSPrinterDataHandler(sqlTransaction);
            posPrinterDTO = posPrinterDataHandler.GetPOSPrinter(posPrinterId);
            if (posPrinterDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSPrinter", posPrinterId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                if (posPrinterDTO.PrinterId > -1)
                {
                    //Get PrinterDTO
                    posPrinterDTO.PrinterDTO = (new PrinterBL(executionContext, posPrinterDTO.PrinterId, true)).PrinterDTO;
                    log.LogVariableState("PrinterDTO", posPrinterDTO.PrinterDTO);
                    //Get POSPrinterOverrideRulesDTOList
                    POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesBLList = new POSPrinterOverrideRulesListBL(executionContext);
                    List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, posPrinterDTO.POSPrinterId.ToString()));
                    if (activeChildRecords)
                    {
                        searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    posPrinterDTO.POSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesBLList.GetPOSPrinterOverrideRulesDTOList(searchByParameters);
                }
                if (posPrinterDTO.SecondaryPrinterId > -1)
                {
                    posPrinterDTO.SecondaryPrinterDTO = (new PrinterBL(executionContext, posPrinterDTO.SecondaryPrinterId, false)).PrinterDTO;
                    log.LogVariableState("SecondaryPrinterDTO", posPrinterDTO.SecondaryPrinterDTO);
                }
                if (posPrinterDTO.PrintTemplateId > -1)
                {
                    posPrinterDTO.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, posPrinterDTO.PrintTemplateId, true)).ReceiptPrintTemplateHeaderDTO;
                    log.LogVariableState("PrinterTemplateHeaderDTO", posPrinterDTO.ReceiptPrintTemplateHeaderDTO);
                }
                posPrinterDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the POSPrinter  
        /// POSPrinter will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            POSPrinterDataHandler pOSPrinterDataHandler = new POSPrinterDataHandler(sqlTransaction);
            UpdatePrinterType(posPrinterDTO);
            if (posPrinterDTO.POSPrinterId < 0)
            {
                posPrinterDTO = pOSPrinterDataHandler.InsertPOSPrinter(posPrinterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                posPrinterDTO.AcceptChanges();
            }
            else if (posPrinterDTO.IsChanged)
            {
                posPrinterDTO = pOSPrinterDataHandler.UpdatePOSPrinter(posPrinterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                posPrinterDTO.AcceptChanges();
            }
            //Call save for posPrinterDTO.POSPrinterOverrideRulesDTOList
            if (posPrinterDTO != null && posPrinterDTO.POSPrinterOverrideRulesDTOList != null && posPrinterDTO.POSPrinterOverrideRulesDTOList.Any())
            {
                List<POSPrinterOverrideRulesDTO> updatedPOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
                for (int i = 0; i < posPrinterDTO.POSPrinterOverrideRulesDTOList.Count; i++)
                {
                    if (posPrinterDTO.POSPrinterOverrideRulesDTOList[i].IsChanged || posPrinterDTO.POSPrinterOverrideRulesDTOList[i].POSPrinterOverrideRuleId < 0)
                    {
                        updatedPOSPrinterOverrideRulesDTOList.Add(posPrinterDTO.POSPrinterOverrideRulesDTOList[i]);
                    }
                }
                if (updatedPOSPrinterOverrideRulesDTOList.Any())
                {
                    POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesListBL = new POSPrinterOverrideRulesListBL(executionContext, updatedPOSPrinterOverrideRulesDTOList);
                    pOSPrinterOverrideRulesListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the POSPrinters based on posPrinterId
        /// </summary>
        /// <param name="posPrinterId">posPrinterId</param>        
        /// <param name="sqlTransaction"></param>        
        public void DeletePOSPrinters(int posPrinterId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posPrinterId, sqlTransaction);
            try
            {
                POSPrinterDataHandler posPrinterDataHandler = new POSPrinterDataHandler(sqlTransaction);
                posPrinterDataHandler.Delete(posPrinterId);
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

        /// <summary>
        /// Update the default printer type id
        /// </summary>
        /// <param name="pOSPrinterDTO"></param>
        private void UpdatePrinterType(POSPrinterDTO pOSPrinterDTO)
        {
            log.LogMethodEntry(posPrinterDTO);
            PrinterBL printerBL = new PrinterBL(executionContext, pOSPrinterDTO.PrinterId, false);
            if (printerBL.PrinterDTO != null)
            {
                pOSPrinterDTO.PrinterTypeId = printerBL.PrinterDTO.PrinterTypeId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public POSPrinterDTO POSPrinterDTO
        {
            get
            {
                return posPrinterDTO;
            }
        }


    }

    /// <summary>
    /// Manages the list of POSPrinters
    /// </summary>
    public class POSPrinterListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSPrinterDTO> pOSPrinterDTOList = new List<POSPrinterDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSPrinterListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSPrinterListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="pOSPrinterDTOList"></param>
        public POSPrinterListBL(ExecutionContext executionContext, List<POSPrinterDTO> pOSPrinterDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrinterDTOList);
            this.pOSPrinterDTOList = pOSPrinterDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method removes the printer from the DTO list 
        /// </summary>
        /// <param name="printerDTOList">printerDTOList</param>
        /// <param name="printerTypes">printerTypes</param>
        /// <returns>List<POSPrinterDTO></returns>
        public List<POSPrinterDTO> RemovePrinterType(List<POSPrinterDTO> printerDTOList, PrinterDTO.PrinterTypes printerTypes)
        {
            log.LogMethodEntry(printerDTOList, printerTypes);
            log.LogVariableState("printerDTOList ", printerDTOList);
            if (printerDTOList == null || printerDTOList.Count < 0)
            {
                throw new Exception("POSPrinterDTO List is either null or empty");
            }
            else
            {
                printerDTOList.RemoveAll(x => x.PrinterDTO.PrinterType == printerTypes);
            }
            log.LogMethodExit(printerDTOList);
            return printerDTOList;
        }
        /// <summary>
        /// Get list of POSPrinterDTO
        /// </summary>
        /// <param name="searchParameters">search parameters for getting list</param>
        /// <param name="loadChildren">loads individual DTO in POSPrinters entity</param>
        /// <param name="sqlTransaction">sql transaction if passed</param>
        public List<POSPrinterDTO> GetPOSPrinterDTOList(List<KeyValuePair<POSPrinterDTO.SearchByParameters, string>> searchParameters, bool loadChildren, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildren, sqlTransaction);
            POSPrinterDataHandler posPrinterDataHandler = new POSPrinterDataHandler(sqlTransaction);
            List<POSPrinterDTO> posPrinterDTOList = posPrinterDataHandler.GetPOSPrinterList(searchParameters);
            if (loadChildren && posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                foreach (POSPrinterDTO posPrinterDTO in posPrinterDTOList)
                {
                    if (posPrinterDTO.PrinterId > -1)
                    {
                        //Get PrinterDTO
                        posPrinterDTO.PrinterDTO = (new PrinterBL(executionContext, posPrinterDTO.PrinterId, false)).PrinterDTO;
                        log.LogVariableState("PrinterDTO", posPrinterDTO.PrinterDTO);
                        //Get POSPrinterOverrideRulesDTOList
                        POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesBLList = new POSPrinterOverrideRulesListBL(executionContext);
                        List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchByParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, posPrinterDTO.POSPrinterId.ToString()));
                        posPrinterDTO.POSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesBLList.GetPOSPrinterOverrideRulesDTOList(searchByParameters);
                        log.LogVariableState("POSPrinterOverrideRulesDTOList", posPrinterDTO.POSPrinterOverrideRulesDTOList);
                    }
                    if (posPrinterDTO.SecondaryPrinterId > -1)
                    {
                        posPrinterDTO.SecondaryPrinterDTO = (new PrinterBL(executionContext, posPrinterDTO.SecondaryPrinterId, false)).PrinterDTO;
                        log.LogVariableState("SecondaryPrinterDTO", posPrinterDTO.SecondaryPrinterDTO);
                    }
                    if (posPrinterDTO.PrintTemplateId > -1)
                    {
                        posPrinterDTO.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, posPrinterDTO.PrintTemplateId, true)).ReceiptPrintTemplateHeaderDTO;
                        log.LogVariableState("PrinterTemplateHeaderDTO", posPrinterDTO.ReceiptPrintTemplateHeaderDTO);
                    }
                    posPrinterDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(posPrinterDTOList);
            return posPrinterDTOList;
        }

        public List<POSPrinterDTO> getPrinterList(String macAddress, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(macAddress, sqlTransaction);

            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.IP_ADDRESS, macAddress));
            POSMachineList posMachineList = new POSMachineList(executionContext);
            List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);

            POSPrinterDataHandler printerDataHandler = new POSPrinterDataHandler(sqlTransaction);
            List<POSPrinterDTO> POSPrinters = new List<POSPrinterDTO>();
            POSPrinters = (printerDataHandler.getPrinterList(posMachines.ElementAt(0).POSMachineId, executionContext.GetSiteId()));

            log.LogMethodExit();
            return POSPrinters;
        }
        /// <summary>
        /// Product is Eligible For KOT Print
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="posPrintersDTOList"></param>
        /// <returns></returns>
        public static bool EligibleForKOTPrint(int productID, List<POSPrinterDTO> posPrintersDTOList)
        {
            log.LogMethodEntry(productID, posPrintersDTOList);
            bool eligibleForKOTPrint = false;
            if (posPrintersDTOList != null && posPrintersDTOList.Any())
            {
                eligibleForKOTPrint = posPrintersDTOList.Exists(prtr => prtr.PrinterDTO != null
                                                                                 && prtr.PrinterDTO.PrinterType == Semnox.Parafait.Printer.PrinterDTO.PrinterTypes.KOTPrinter
                                                                                 && prtr.PrinterDTO.PrintableProductIds.Exists(prodId => prodId == productID));
            }
            log.LogMethodExit(eligibleForKOTPrint);
            return eligibleForKOTPrint;
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the POSPrinterBL List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (pOSPrinterDTOList == null ||
                pOSPrinterDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < pOSPrinterDTOList.Count; i++)
            {
                var pOSPrinterDTO = pOSPrinterDTOList[i];
                if (pOSPrinterDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    POSPrinterBL pOSPrinterBL = new POSPrinterBL(executionContext, pOSPrinterDTO);
                    pOSPrinterBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving POSPrinterDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("POSPrinterDTO", pOSPrinterDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the POSPrinterDTO List for POSMachineIdList
        /// </summary>
        /// <param name="pOSMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of POSPrinterDTO</returns>
        public List<POSPrinterDTO> GetPOSPrinterDTOList(List<int> pOSMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineIdList, activeRecords, sqlTransaction);
            POSPrinterDataHandler pOSPrinterDataHandler = new POSPrinterDataHandler(sqlTransaction);
            List<POSPrinterDTO> pOSPrinterDTOList = pOSPrinterDataHandler.GetPOSPrinterDTOList(pOSMachineIdList, activeRecords);
            log.LogMethodExit(pOSPrinterDTOList);
            return pOSPrinterDTOList;
        }
    }
}
