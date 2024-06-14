/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteRequisitionTemplatesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     11-Dec-2020       Mushahid Faizan            Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory.Requisition
{
    public class RemoteRequisitionTemplatesUseCases : RemoteUseCases, IRequisitionTemplatesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string REQUISITION_TEMPLATES_URL = "api/Inventory/RequisitionTemplates";
        private const string REQUISITION_TEMPLATE_COUNTS_URL = "api/Inventory/RequisitionTemplateCounts";

        public RemoteRequisitionTemplatesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RequisitionTemplatesDTO>> GetRequisitionTemplates(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> parameters,
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
                List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = await Get<List<RequisitionTemplatesDTO>>(REQUISITION_TEMPLATES_URL, searchParameterList);
                log.LogMethodExit(requisitionTemplatesDTOList);
                return requisitionTemplatesDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetRequisitionTemplateCount(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> parameters,
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
                int result = await Get<int>(REQUISITION_TEMPLATE_COUNTS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> requisitionTemplatesSearchParams)
        {
            log.LogMethodEntry(requisitionTemplatesSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string> searchParameter in requisitionTemplatesSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("templateId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("templateName".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.REQUISITION_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("requisitionType".ToString(), searchParameter.Value));
                        }
                        break;
                    case RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRequisitionTemplates(List<RequisitionTemplatesDTO> requisitionTemplatesDTOList)
        {
            log.LogMethodEntry(requisitionTemplatesDTOList);
            try
            {
                string responseString = await Post<string>(REQUISITION_TEMPLATES_URL, requisitionTemplatesDTOList);
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
