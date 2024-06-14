/********************************************************************************************
* Project Name - CommonAPI
* Description  - RequisitionController - Created to get the requisition
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.0   07-Dec-2020    Mushahid Faizan              Created : As part of Inventory UI Redesign      
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Parafait.Inventory;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Requisition;
using System.Globalization;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RequisitionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of RequisitionDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Requisitions")]
        public async Task<HttpResponseMessage> Get(int requisitionId = -1, string requisitionNumber = null, int requisitionType = -1, string status = null, 
                                                   bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false, string requisitionIdList = null,
                                                    string guidIdList = null, int currentPage = 0, int pageSize = 10, DateTime? requiredByDate = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionId, requisitionNumber, requisitionType, status, buildChildRecords, isActive,
                                   loadActiveChild, currentPage, pageSize);

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                RequisitionList requisitionListBL = new RequisitionList(executionContext);
                List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> requisitionSearchParameter = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
                requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (requisitionId > 0)
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID, Convert.ToString(requisitionId)));
                }
                if (!string.IsNullOrEmpty(requisitionNumber))
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER, requisitionNumber));
                }
                if (requisitionType > 0)
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, Convert.ToString(requisitionType)));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.STATUS, status));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, isActive));
                    }
                }

                if (requiredByDate != null)
                {
                    DateTime expectedReceiptDate = Convert.ToDateTime(requiredByDate);
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE, expectedReceiptDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }


                if (!string.IsNullOrEmpty(requisitionIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> requisitionList = new List<int>();

                    requisitionList = requisitionIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String requisitionListString = String.Join(",", requisitionList.ToArray());
                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID_LIST, requisitionListString));
                }

                if (!string.IsNullOrEmpty(guidIdList))
                {

                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<string> guidList = new List<string>();

                    guidList = guidIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).ToList();

                    String guidIdListString = String.Join(",", guidList.ToArray());

                    requisitionSearchParameter.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.GUID_ID_LIST, guidIdListString));
                }
                int totalNoOfPages = 0;
                int totalNoOfRequisitions = await Task<int>.Factory.StartNew(() => { return requisitionListBL.GetRequisitionsCount(requisitionSearchParameter, null); });
                log.LogVariableState("totalNoOfRequisitions", totalNoOfRequisitions);
                totalNoOfPages = (totalNoOfRequisitions / pageSize) + ((totalNoOfRequisitions % pageSize) > 0 ? 1 : 0);

                IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                List<RequisitionDTO> requisitionDTOList = await requisitionUseCases.GetRequisitions(requisitionSearchParameter, buildChildRecords, loadActiveChild, currentPage, pageSize, null);
                log.LogMethodExit(requisitionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = requisitionDTOList, currentPageNo = currentPage, TotalCount = totalNoOfRequisitions }); 
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RequisitionDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Requisitions")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RequisitionDTO> requisitionDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<RequisitionDTO> savedRequisitionDTOList = new List<RequisitionDTO>();
                if (requisitionDTOList == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (requisitionDTOList != null && requisitionDTOList.Any())
                {
                    IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                    savedRequisitionDTOList = await requisitionUseCases.SaveRequisitions(requisitionDTOList);
                    log.LogMethodExit(savedRequisitionDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = savedRequisitionDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Put the JSON Object of RequisitionDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Requisitions")]
        public async Task<HttpResponseMessage> Put([FromBody] List<RequisitionDTO> requisitionDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(requisitionDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<RequisitionDTO> savedRequisitionDTOList = new List<RequisitionDTO>();
                if (requisitionDTOList == null || requisitionDTOList.Any(x => x.RequisitionId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (requisitionDTOList != null && requisitionDTOList.Any())
                {
                    IRequisitionUseCases requisitionUseCases = InventoryUseCaseFactory.GetRequisitionUseCases(executionContext);
                    savedRequisitionDTOList = await requisitionUseCases.SaveRequisitions(requisitionDTOList);
                    log.LogMethodExit(savedRequisitionDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = savedRequisitionDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}