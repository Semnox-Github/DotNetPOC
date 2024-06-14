/********************************************************************************************
 * Project Name - CommnonAPI - Job Module                                                                     
 * Description  - Controller for Fiscalization concurrent request submission
 *
 **************
 **Version Log
  *Version         Date            Modified By          Remarks          
 *********************************************************************************************
 *2.155.1.0         11-Aug-20232   Guru S A            Created for Chile fiscalization
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Fiscalization; 
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Job
{
    /// <summary>
    /// FiscalizationConcurrentRequestController
    /// </summary>
    public class FiscalizationConcurrentRequestController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string EXECUTABLENAME = "InvoiceJsonReprocessingProgram.Exe";
        private const string PROGRAMENAME = "Invoice Json Reprocessing Program";
        private const string PARAM_TRX_ID_LIST = "TransactionIdList";
        private const string PARAM_FISCALIZATION_TYPE = "FiscalizationType";
        /// <summary>
        /// Post 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/ConcurrentRequest/Fiscalization")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<FiscalizationReprocessDTO> fiscalizationReprocessDTOList)
        {
            log.LogMethodEntry(fiscalizationReprocessDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                securityTokenDTO = GetSecurityToken();
                executionContext = SetExecutionContext(securityTokenDTO);
                Utilities utilities = SetUtilities(executionContext);

                if (fiscalizationReprocessDTOList != null && fiscalizationReprocessDTOList.Any())
                {
                    List<string> fiscalizationNameList = fiscalizationReprocessDTOList.Select(f => f.Fiscalization).Distinct().ToList();
                    if (fiscalizationNameList == null || fiscalizationNameList.Any() == false)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 165, "Fiscalization");
                        //&1 is mandatory
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                    if (fiscalizationNameList.Count > 1)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5181);
                        //"Cannot process data for mulitple fiscalizations"
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                    List<int> trxIdList = fiscalizationReprocessDTOList.Select(f => f.TransactionId).Distinct().ToList();
                    if (trxIdList == null || trxIdList.Any() == false)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 165, "Tranaction Id List");
                        //&1 is mandatory
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                    if (trxIdList.Count > 100)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5182, 100);
                        //"Cannot process more than &1 records"
                        ValidationException ve = new ValidationException(msg);
                        throw ve; 
                    }
                    foreach (FiscalizationReprocessDTO item in fiscalizationReprocessDTOList)
                    {
                        item.ConcurrentRequestId = -1; //clear old req id info
                    }
                    ParafaitFiscalizationNames parafaitFiscalizationNameValue = FiscalizationFactory.GetParafaitFiscalizationNames(fiscalizationNameList[0]);
                    IParafaitFiscalizationUseCases fiscalizationUseCases = FiscalizationUseCaseFactory.GetParafaitFiscalizationUseCases(executionContext, utilities);
                    IList<FiscalizationReprocessDTO> dTOList = await fiscalizationUseCases.PostFiscalizationReprocessingRequest(parafaitFiscalizationNameValue, fiscalizationReprocessDTOList);
                    dTOList = new SortableBindingList<FiscalizationReprocessDTO>(dTOList);

                    log.LogMethodExit(dTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dTOList });
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
        private static SecurityTokenDTO GetSecurityToken()
        {
            log.LogMethodEntry();
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO tokenDTO = securityTokenBL.GetSecurityTokenDTO;
            log.LogMethodExit("tokenDTO");
            return tokenDTO;
        }
        private static ExecutionContext SetExecutionContext(SecurityTokenDTO securityTokenDTO)
        {
            log.LogMethodEntry("securityTokenDTO");
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            log.LogMethodExit(executionContext);
            return executionContext;
        }

        private static Utilities SetUtilities(ExecutionContext executionContext)
        {
            log.LogMethodEntry("executionContext");
            Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
            Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
            Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
            Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
            log.LogMethodExit("Utilities");
            return Utilities;
        }

    }
}