/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteInventoryIssueHeaderUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0    28-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 *2.110.1     01-Mar-2021      Mushahid Faizan          Modified : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory
{
    public class RemoteInventoryIssueHeaderUseCases : RemoteUseCases, IInventoryIssueHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string InventoryIssueHeader_URL = "api/Inventory/Issues";
        private const string InventoryIssueHeader_COUNT_URL = "api/Inventory/IssueCounts";
        private string INVENTORY_ISSUE_HEADER_LINE_URL = "api/Inventory/Issue/{issueId}/IssueLines";
        private string INVENTORY_ISSUE_HEADER_STATUS_URL = "api/Inventory/Issue/{issueId}/Status";
        private string PRINT_ISSUE_URL = "api/Inventory/Issue/{issueId}/Print";

        public RemoteInventoryIssueHeaderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<InventoryIssueHeaderDTO>> GetInventoryIssueHeaders(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0, bool buildChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {

                List<InventoryIssueHeaderDTO> result = await Get<List<InventoryIssueHeaderDTO>>(InventoryIssueHeader_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetInventoryIssueCount(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {

                int result = await Get<int>(InventoryIssueHeader_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.INVENTORY_ISSUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("issueId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.DOCUMENT_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("documentTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.PURCHASE_ORDER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseOrderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.REQUISITION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("requsitionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<InventoryIssueHeaderDTO>> SaveInventoryIssueHeaders(List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(inventoryIssueHeaderDTOList);
                List<InventoryIssueHeaderDTO> responseData = await Post<List<InventoryIssueHeaderDTO>>(InventoryIssueHeader_URL, content);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<InventoryIssueHeaderDTO> AddInventoryIssueLines(int issueid, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        {
            log.LogMethodEntry(issueid, inventoryIssueLineDTOList);
            try
            {
                INVENTORY_ISSUE_HEADER_LINE_URL = "api/Inventory/Issue/" + issueid + "/IssueLines";
                InventoryIssueHeaderDTO responseString = await Post<InventoryIssueHeaderDTO>(INVENTORY_ISSUE_HEADER_LINE_URL, inventoryIssueLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<InventoryIssueHeaderDTO> UpdateInventoryIssueLines(int issueid, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        {
            log.LogMethodEntry(issueid, inventoryIssueLineDTOList);
            try
            {
                INVENTORY_ISSUE_HEADER_LINE_URL = "api/Inventory/Issue/" + issueid + "IssueLines";
                InventoryIssueHeaderDTO responseString = await Post<InventoryIssueHeaderDTO>(INVENTORY_ISSUE_HEADER_LINE_URL, inventoryIssueLineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<InventoryIssueHeaderDTO> UpdateIssueStatus(int issueId, string issueStatus)
        {
            log.LogMethodEntry(issueId, issueStatus);
            try
            {
                INVENTORY_ISSUE_HEADER_STATUS_URL = "api/Inventory/Issue/" + issueId + "/Status";
                InventoryIssueHeaderDTO responseString = await Post<InventoryIssueHeaderDTO>(INVENTORY_ISSUE_HEADER_STATUS_URL, issueStatus);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        //public async Task<InventoryIssueHeaderDTO> RemoveInventoryIssueLines(int issueid, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList)
        //{
        //    log.LogMethodEntry(issueid, inventoryIssueLineDTOList);
        //    try
        //    {
        //        INVENTORY_ISSUE_HEADER_LINE_URL = "api/Inventory/Issue/" + issueid + "/InventoryIssueLines";
        //        InventoryIssueHeaderDTO responseString = await Post<InventoryIssueHeaderDTO>(INVENTORY_ISSUE_HEADER_LINE_URL, inventoryIssueLineDTOList);
        //        log.LogMethodExit(responseString);
        //        return responseString;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw ex;
        //    }
        //}


        public async Task<MemoryStream> PrintIssues(int issueId,string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate,  outputFormat);
            {
                try
                {
                    List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                    searchParameterList.Add(new KeyValuePair<string, string>("reportKey".ToString(), reportKey.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("timeStamp".ToString(), timeStamp.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
                    searchParameterList.Add(new KeyValuePair<string, string>("outputFormat".ToString(), outputFormat.ToString()));


                    List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                    backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("IssueId", issueId.ToString()));

                    if (backgroundParameters != null)
                    {
                        foreach (clsReportParameters.SelectedParameterValue selectedParameterValue in backgroundParameters)
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("selectedParameterValue".ToString(), selectedParameterValue.ToString()));
                        }
                    }
                    MemoryStream responseString = await Get<MemoryStream>(PRINT_ISSUE_URL, searchParameterList);
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
