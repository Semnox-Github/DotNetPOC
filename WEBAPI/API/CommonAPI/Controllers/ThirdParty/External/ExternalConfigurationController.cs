/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalProductGroupController  API -  add and delete product  data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.130.7    07-Apr-2022            Ashish Bhat            Modified( External  REST API.)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Environments
{
    public class ExternalConfigurationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the JSON Object Configurations
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Configurations")]
        public async Task<HttpResponseMessage> Get()
        {
            log.LogMethodEntry();
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<AllowedCreditType> allowedCreditTypeList = new List<AllowedCreditType>();
                AllowedCreditType allowedCreditTypeT = new AllowedCreditType("Tickets","T",true);
                allowedCreditTypeList.Add(allowedCreditTypeT);
                AllowedCreditType allowedCreditTypeB = new AllowedCreditType("Bonus", "B", true);
                allowedCreditTypeList.Add(allowedCreditTypeB);
                AllowedCreditType allowedCreditTypeP = new AllowedCreditType("Items", "P", true);
                allowedCreditTypeList.Add(allowedCreditTypeP);
                AllowedCreditType allowedCreditTypeG = new AllowedCreditType("GamePlay Credit", "G", true);
                allowedCreditTypeList.Add(allowedCreditTypeG);
                AllowedCreditType allowedCreditTypeA = new AllowedCreditType("Card Balance", "A", true);
                allowedCreditTypeList.Add(allowedCreditTypeA);
                AllowedCreditType allowedCreditTypeL = new AllowedCreditType("Loyalty Points", "A", true);
                allowedCreditTypeList.Add(allowedCreditTypeL);
                List<string> paymentTypesList = new List<string>();
                paymentTypesList.Add("Cash");
                paymentTypesList.Add("Credit/Debit");
                List<AllowedTask> allowedTaskList = new List<AllowedTask>();
                AllowedTask allowedTaskLB = new AllowedTask("LoadBonus", true);
                allowedTaskList.Add(allowedTaskLB);
                AllowedTask allowedTaskBT = new AllowedTask("BalanceTransfer", true);
                allowedTaskList.Add(allowedTaskBT);
                AllowedTask allowedTaskC = new AllowedTask("Create", true);
                allowedTaskList.Add(allowedTaskC);
                AllowedTask allowedTaskCo = new AllowedTask("Reverse", true);
                allowedTaskList.Add(allowedTaskCo);
              
                ExternalConfigurationDTO externalConfigurationDTO = new ExternalConfigurationDTO(allowedCreditTypeList, 8, 3, paymentTypesList, false,
                    allowedTaskList);
               
                log.LogMethodExit(externalConfigurationDTO);
                return Request.CreateResponse(HttpStatusCode.OK, externalConfigurationDTO);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
