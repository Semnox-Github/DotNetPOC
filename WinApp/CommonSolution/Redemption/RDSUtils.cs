/********************************************************************************************
 * Project Name - RDSDisplay BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version       Date                 Modified By          Remarks          
 *********************************************************************************************
 *2.4.0         23-August-2018       Archana              Created 
 *2.70.2          12-Aug-2019          Deeksha              Modified logger methods.
 *2.70.2          24-Sep-2019          Lakshminarayana      changes related to KDS enhancement.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction.KDS;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RDSDisplay class.
    /// </summary>
    public class RDSUtils
    {
        internal static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext;
        private Utilities utilities;
        public RDSUtils(ExecutionContext executionContext, Utilities utils)
        {
            log.LogMethodEntry(executionContext, utils);
            machineUserContext = executionContext;
            utilities = utils;
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to get order list to display in RDS
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<RedemptionDTO> GetRDSOrdersList(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            List<RedemptionDTO> redemptionDTOs = new List<RedemptionDTO>();
            redemptionDTOs = redemptionDataHandler.GetRedemptionDTOList(searchParameters);
            log.LogMethodExit(redemptionDTOs);
            return redemptionDTOs;
        }

        /// <summary>
        /// Method to get orders
        /// </summary>
        /// <param name="isOpenStatus"></param>
        /// <param name="lastMinutes"></param>
        /// <returns></returns>
        public List<KDSDisplayUnitClass> GetRdsOrders(int posMachineId, bool isOpenStatus = false, int lastMinutes = 0)
        {
            log.LogMethodEntry(posMachineId, isOpenStatus, lastMinutes);
            List<KDSDisplayUnitClass> kdsDisplayUnitClass = null;
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FETCH_GIFT_REDEMPTIONS_ONLY, "Y"));
            if (posMachineId != -1)
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
            }
            if (isOpenStatus == true)
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, RedemptionDTO.RedemptionStatusEnum.OPEN.ToString()));
            }
            else
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString()));
            }
            if (lastMinutes != 0)
            {
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_COMPLETED_DATE, DateTime.Now.AddMinutes(lastMinutes * (-1)).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_COMPLETED_DATE, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
            }
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, "Y"));
            List<RedemptionDTO> redemptionDTOList = GetRDSOrdersList(searchParameters);
            if (redemptionDTOList != null && redemptionDTOList.Count > 0)
            {
                kdsDisplayUnitClass = new List<KDSDisplayUnitClass>();
                PrintRedemptionReceipt rdsReceipt = null;
                List<RedemptionDTO> newRedemptionDTOList = ReversedRdsOrdersList(redemptionDTOList);
                if (newRedemptionDTOList.Count > 0)
                {
                    foreach (RedemptionDTO redemptionDto in newRedemptionDTOList)
                    {
                        redemptionDTOList.Remove(redemptionDto);
                    }
                } 

                int rdsReceiptTemplateId = -1;
                POSMachines posMachine = new POSMachines(utilities.ExecutionContext, utilities.ParafaitEnv.POSMachineId);
                List<POSPrinterDTO> posPrinterDTOList = posMachine.PopulatePrinterDetails();
                List<POSPrinterDTO> posRDSPrinterDTOList;
                if (posPrinterDTOList != null && posPrinterDTOList.Count > 0)
                {
                    posRDSPrinterDTOList = posPrinterDTOList.Where(printerEntry => printerEntry.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RDSPrinter).ToList();
                    
                    if (posRDSPrinterDTOList == null || posRDSPrinterDTOList.Count == 0)
                    {
                        log.Error(utilities.MessageUtils.getMessage(1618));
                        //"RDS Printer is not setup"
                        throw new Exception(utilities.MessageUtils.getMessage(1618));
                    }
                    else
                    {
                        if (posRDSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO != null)
                        {
                            rdsReceiptTemplateId = posRDSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO.TemplateId;
                        } 
                    }
                    if (rdsReceiptTemplateId == -1)
                    {
                        log.Error(utilities.MessageUtils.getMessage(1619));
                        throw new Exception(utilities.MessageUtils.getMessage(1619));
                    }
                    foreach (RedemptionDTO redemptionDTO in redemptionDTOList)
                    {
                        rdsReceipt = new PrintRedemptionReceipt(executionContext, utilities);
                        KDSDisplayUnitClass displayUnit = new KDSDisplayUnitClass();
                        displayUnit.TrxNumber = redemptionDTO.RedemptionOrderNo;
                        displayUnit.TrxId = redemptionDTO.RedemptionId;
                        displayUnit.OrderedTime = Convert.ToDateTime(redemptionDTO.RedeemedDate);
                        displayUnit.DeliveryMode = redemptionDTO.RedemptionStatus;
                        RedemptionBL redemptionOrder = new RedemptionBL(redemptionDTO, executionContext);
                        displayUnit.DisplayContent = rdsReceipt.GenerateRedemptionReceipt(redemptionOrder, rdsReceiptTemplateId, null, null, true);
                        kdsDisplayUnitClass.Add(displayUnit);
                    }
                } 
                else
                {
                    log.Error(utilities.MessageUtils.getMessage(1618));
                    throw new Exception(utilities.MessageUtils.getMessage(1618));
                }
            }
            log.LogMethodExit(kdsDisplayUnitClass);
            return kdsDisplayUnitClass;
        }
        /// <summary>
        /// Method to update redemption order status
        /// </summary>
        /// <param name="redemptionId"></param>
        /// <param name="isOpenStatus"></param>
        public void UpdateRdsOrderStatus(int redemptionId, bool isOpenStatus = false)
        {
            log.LogMethodEntry(redemptionId, isOpenStatus);
            RedemptionBL redemptionBL = new RedemptionBL(redemptionId, machineUserContext);
            if (isOpenStatus == true)
            {
                redemptionBL.RedemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString();
            }
            else
            {
                redemptionBL.RedemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
            }
            redemptionBL.Save();
            log.LogMethodExit();
        } 
        /// <summary>
        /// Method to update redemption remarks
        /// </summary>
        /// <param name="redemptionId"></param>
        /// <param name="remarks"></param>
        public void UpdateRedemptionRemarks(int redemptionId, string remarks)
        {
            log.LogMethodEntry(redemptionId, remarks);
            RedemptionBL redemptionBL = new RedemptionBL(redemptionId, machineUserContext);
            if (!String.IsNullOrEmpty(remarks))
            {
                string finalRemarks = (!String.IsNullOrEmpty(redemptionBL.RedemptionDTO.Remarks) ? redemptionBL.RedemptionDTO.Remarks + Environment.NewLine + "RDS: " + remarks : "RDS: " + remarks);
                if (finalRemarks.Length < 2000)
                {
                    redemptionBL.RedemptionDTO.Remarks = finalRemarks;
                    redemptionBL.Save();
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, "Remarks contains " + finalRemarks.Length.ToString() + "charters. Remarks can not have more than 2000 charcters"));

                }
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 201));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to get list orders which are completely reversed
        /// </summary>
        /// <param name="redemptionListDTO"></param>
        /// <returns></returns>
        List<RedemptionDTO> ReversedRdsOrdersList(List<RedemptionDTO> redemptionListDTO)
        {
            log.LogMethodEntry(redemptionListDTO);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            List<RedemptionDTO> newRedemptionListDTO = new List<RedemptionDTO>();
            foreach (RedemptionDTO redemptionDTO in redemptionListDTO)
            {
                RedemptionBL redemptionBL = new RedemptionBL(redemptionDTO, executionContext);
                if (redemptionBL.IsOrderCompletelyReversed())
                {
                    newRedemptionListDTO.Add(redemptionDTO);
                }
            }
            log.LogMethodExit(newRedemptionListDTO);
            return newRedemptionListDTO;
        }
    }
}
