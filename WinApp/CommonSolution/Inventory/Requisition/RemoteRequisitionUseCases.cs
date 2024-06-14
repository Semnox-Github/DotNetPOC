/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteRequisitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020       Mushahid Faizan          Created for Inventory UI Redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory.Requisition
{
    public class RemoteRequisitionUseCases : RemoteUseCases, IRequisitionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string REQUISITION_URL = "api/Inventory/Requisitions";
        private const string REQUISITION_COUNT_URL = "api/Inventory/RequisitionCounts";
        private string REQUISITION_LINE_URL = "api/Inventory/Requisition/{requisitionId}/RequisitionLines";
        private string REQUISITION_STATUS_URL = "api/Inventory/Requisition/{requisitionId}/Status";
        private string PRINT_REQUISITION_URL = "api/Inventory/Requisition/{requisitionId}/Print";
        private string RQ_EMAIL_URL = "api/Inventory/Requisition/{requisitionId}/Email";

        public RemoteRequisitionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<RequisitionDTO>> GetRequisitions(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> parameters,
                                                                bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0,
                                                                int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords", loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords", activeChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<RequisitionDTO> requisitionDTOList = await Get<List<RequisitionDTO>>(REQUISITION_URL, searchParameterList);
                log.LogMethodExit(requisitionDTOList);
                return requisitionDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetRequisitionCount(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> parameters,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(REQUISITION_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> redemptionCurrencySearchParams)
        {
            log.LogMethodEntry(redemptionCurrencySearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string> searchParameter in redemptionCurrencySearchParams)
            {
                switch (searchParameter.Key)
                {

                    case RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("requisitionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("requisitionNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("requisitionType".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionDTO.SearchByRequisitionParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<RequisitionDTO>> SaveRequisitions(List<RequisitionDTO> requisitionDTOList)
        {
            log.LogMethodEntry(requisitionDTOList);
            try
            {
                List<RequisitionDTO> responseData = await Post<List<RequisitionDTO>>(REQUISITION_URL, requisitionDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RequisitionDTO> AddRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList)
        {
            log.LogMethodEntry(requisitionId, requisitionLineDTOList);
            try
            {
                REQUISITION_LINE_URL = "api/Inventory/Requsition/" + requisitionId + "/RequisitionLines";
                RequisitionDTO responseString = await Post<RequisitionDTO>(REQUISITION_LINE_URL, requisitionLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RequisitionDTO> UpdateRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList)
        {
            log.LogMethodEntry(requisitionId, requisitionLineDTOList);
            try
            {
                REQUISITION_LINE_URL = "api/Inventory/Requsition/" + requisitionId + "/RequisitionLines";
                RequisitionDTO responseString = await Post<RequisitionDTO>(REQUISITION_LINE_URL, requisitionLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RequisitionDTO> UpdateRequisitionStatus(int requisitionId, string requisitionStatus)
        {
            log.LogMethodEntry(requisitionId, requisitionStatus);
            try
            {
                REQUISITION_STATUS_URL = "api/Inventory/requisition/" + requisitionId + "/Status";
                RequisitionDTO responseString = await Post<RequisitionDTO>(REQUISITION_STATUS_URL, requisitionStatus);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, List<MessagingRequestDTO> messagingRequestDTOList)
        {
            log.LogMethodEntry(requisitionId, messagingRequestDTOList);
            try
            {
                RQ_EMAIL_URL = "api/Inventory/Requisition/" + requisitionId + "/"  + "/Email";
                MessagingRequestDTO responseString = await Post<MessagingRequestDTO>(RQ_EMAIL_URL, messagingRequestDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<MemoryStream> PrintRequisitions(int requisitionId,string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, outputFormat);
            {
                try
                {
                    List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                    searchParameterList.Add(new KeyValuePair<string, string>("reportKey".ToString(), reportKey.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("timeStamp".ToString(), timeStamp.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("outputFormat".ToString(), outputFormat.ToString()));

                    RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionId);
                    RequisitionDTO requisitionDTO = requisitionBL.GetRequisitionsDTO;

                    List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                    backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionId", requisitionId.ToString()));
                    backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("RequisitionNo", requisitionDTO.RequisitionNo.ToString()));

                    if (backgroundParameters != null)
                    {
                        foreach (clsReportParameters.SelectedParameterValue selectedParameterValue in backgroundParameters)
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("selectedParameterValue".ToString(), selectedParameterValue.ToString()));
                        }
                    }

                    MemoryStream responseString = await Get<MemoryStream>(PRINT_REQUISITION_URL, searchParameterList);
                    log.LogMethodExit(responseString);
                    return responseString;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }

            }

        }
    }
}
