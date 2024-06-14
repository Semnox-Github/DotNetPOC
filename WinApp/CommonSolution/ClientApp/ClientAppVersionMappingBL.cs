/********************************************************************************************
 * Project Name - ClientAppVersionMappingBL
 * Description  - API to return Client App Version from App Manager in HQ of HQ
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Security.Cryptography;

namespace Semnox.Parafait.ClientApp
{
    public class ClientAppVersionMappingBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        ClientAppVersionMappingDTO clientAppVersionMappingDTO;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        public ClientAppVersionMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppVersionMappingId"></param>
        /// <param name="sqlTransaction"></param>
        public ClientAppVersionMappingBL(ExecutionContext executionContext,int clientAppVersionMappingId,SqlTransaction sqlTransaction=null) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,clientAppVersionMappingId,sqlTransaction);
            ClientAppVersionMappingDataHandler clientAppVersionMappingDataHandler = new ClientAppVersionMappingDataHandler(sqlTransaction);
            clientAppVersionMappingDTO = clientAppVersionMappingDataHandler.GetClientAppVersionMappingDTOById(clientAppVersionMappingId);
            if (clientAppVersionMappingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ClientAppVersionMapping", clientAppVersionMappingId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppVersionMappingDTO"></param>
        public ClientAppVersionMappingBL(ExecutionContext executionContext,ClientAppVersionMappingDTO clientAppVersionMappingDTO) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, clientAppVersionMappingDTO);
            this.clientAppVersionMappingDTO = clientAppVersionMappingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (clientAppVersionMappingDTO.IsActive)
            {
                List<ValidationError> validationErrorList = Validate();
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
            }
            
            ClientAppVersionMappingDataHandler ClientAppVersionMappingDataHandler = new ClientAppVersionMappingDataHandler(sqlTransaction);
                if (clientAppVersionMappingDTO.ClientAppVersionMappingId <= 0)
                {
                    clientAppVersionMappingDTO = ClientAppVersionMappingDataHandler.InsertClientAppVersionMapping(clientAppVersionMappingDTO, executionContext.GetUserId());
                    clientAppVersionMappingDTO.AcceptChanges();
                }
                else
                {
                    if (clientAppVersionMappingDTO.IsChanged == true)
                    {
                        ClientAppVersionMappingDataHandler.UpdateClientAppVersionMapping(clientAppVersionMappingDTO, executionContext.GetUserId());
                        clientAppVersionMappingDTO.AcceptChanges();
                    }
                   
                }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClientAppVersionMappingId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int Delete(int ClientAppVersionMappingId,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(ClientAppVersionMappingId, sqlTransaction);
            try
            {
                ClientAppVersionMappingDataHandler ClientAppVersionMappingDataHandler = new ClientAppVersionMappingDataHandler(sqlTransaction);
                int id=ClientAppVersionMappingDataHandler.DeleteClientAppVersionMapping(ClientAppVersionMappingId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                
                log.Error("Error occurred while Deleting ClientAppVersionMappingDTO.", ex);
                log.Error("Throwing exception At Delete() : " + ex.Message);
                log.LogVariableState("ClientAppVersionMappingId", ClientAppVersionMappingId);
                throw;
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        public ClientAppVersionMappingDTO GetClientAppVersionMappingDTO
        {
            get { return clientAppVersionMappingDTO; }
        }

        /// <summary>
        /// Validate the ClientAppVersionMappingDTO
        /// </summary>
        /// <returns>returns List<ClientValidationStruct> </returns>
        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if(clientAppVersionMappingDTO!=null)
            {
                if(string.IsNullOrEmpty(clientAppVersionMappingDTO.AppId))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "ClientAppVersionMapping Type"));
                    validationErrorList.Add(new ValidationError("ClientAppVersionMapping", "AppId", errorMessage));
                    log.LogMethodExit(validationErrorList);
                    return validationErrorList;
                }
                if (clientAppVersionMappingDTO.ClientId<0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "ClientAppVersionMapping Type"));
                    validationErrorList.Add(new ValidationError("ClientAppVersionMapping", "ClientId", errorMessage));
                    log.LogMethodExit(validationErrorList);
                    return validationErrorList;
                }
                if (string.IsNullOrEmpty(clientAppVersionMappingDTO.SecurityCode))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "ClientAppVersionMapping Type"));
                    validationErrorList.Add(new ValidationError("ClientAppVersionMapping", "SecurityCode", errorMessage));
                    log.LogMethodExit(validationErrorList);
                    return validationErrorList;
                }
                if (string.IsNullOrEmpty( clientAppVersionMappingDTO.ReleaseNumber))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "ClientAppVersionMapping Type"));
                    validationErrorList.Add(new ValidationError("ClientAppVersionMapping", "ReleaseNumber", errorMessage));
                    log.LogMethodExit(validationErrorList);
                    return validationErrorList;
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Class ClientAppVersionMappingList contains methods
    /// </summary>
    public class ClientAppVersionMappingListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        List<ClientAppVersionMappingDTO> ClientAppVersionMappingDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppVersionMappingDTOList"></param>
        public ClientAppVersionMappingListBL(ExecutionContext executionContext, List<ClientAppVersionMappingDTO> clientAppVersionMappingDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.ClientAppVersionMappingDTOList = clientAppVersionMappingDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ClientAppVersionMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search based on Parameters And returns  List<ClientAppVersionMappingDTO>   
        /// </summary>
        public List<ClientAppVersionMappingDTO> GetAllClientAppVersionMapping(List<KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>> searchParameters,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            ClientAppVersionMappingDataHandler clientAppUserDatahandler = new ClientAppVersionMappingDataHandler(sqlTransaction);
            List<ClientAppVersionMappingDTO> clientAppVersionMappingDTOList = clientAppUserDatahandler.GetAllClientAppVersionMapping(searchParameters);
            log.LogMethodExit(clientAppVersionMappingDTOList);
            return clientAppVersionMappingDTOList;
        }

        /// <summary>
        /// method to validate the hash sent by the dashboard app. Hash is a combination of AppId, Security Code and Timestamp. 
        /// App Identifier, timestamp and first 2 digits of the security code are sent by the app. 
        /// The method Usea this information to fetch the security code from DB, generate the hash and then compare the values.
        /// </summary>
        /// <param name="securityCodeHash">Generated Hash</param>
        /// <param name="appId">Application Identifier</param>
        /// <param name="buildNumber">Build Number of the app</param>
        /// <param name="generatedTime">Time when the hash was generated</param>
        /// <param name="securityCode">Last 2 digits of the security code</param>
        /// <returns></returns>
        public  ClientAppVersionMappingDTO GetClientAppVersion(string securityCodeHash, string appId, string buildNumber, DateTime generatedTime, String securityCode)
        {
            log.LogMethodEntry(securityCodeHash, appId, buildNumber, generatedTime);
            List<KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>>();
            searchParameters.Add(new KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>(ClientAppVersionMappingDTO.SearchParameters.RELEASE_NUMBER, buildNumber));
            searchParameters.Add(new KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>(ClientAppVersionMappingDTO.SearchParameters.APP_ID, appId));
            searchParameters.Add(new KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>(ClientAppVersionMappingDTO.SearchParameters.SECURITY_TOKEN_PARTIAL, securityCode));
            ClientAppVersionMappingDataHandler clientAppUserDatahandler = new ClientAppVersionMappingDataHandler(null);
            List<ClientAppVersionMappingDTO> clientAppVersionMappingDTOList = clientAppUserDatahandler.GetAllClientAppVersionMapping(searchParameters);
            if(clientAppVersionMappingDTOList != null && clientAppVersionMappingDTOList.Any())
            {
                foreach (ClientAppVersionMappingDTO clientAppVersionMappingDTO in clientAppVersionMappingDTOList)
                {
                    string output = "";
                    using (SHA1Managed sha = new SHA1Managed())
                    {
                        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(clientAppVersionMappingDTO.AppId + clientAppVersionMappingDTO.SecurityCode +
                                            generatedTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")));
                        output = Convert.ToBase64String(hash);
                    }

                    if (securityCodeHash.Equals(output))
                    {
                        if (generatedTime.ToUniversalTime() > DateTime.Now.ToUniversalTime().AddMinutes(15) ||
                        generatedTime.ToUniversalTime() < DateTime.Now.ToUniversalTime().AddMinutes(-15))
                        {
                            throw new ValidationException("Invalid or expired security code");
                        }
                        // remove the security code
                        clientAppVersionMappingDTO.SecurityCode = "";
                        return clientAppVersionMappingDTO;
                    }
                }

                // version not found
                throw new ValidationException("Invalid or expired security code");
            }
            else
            {
                throw new ValidationException("Invalid or expired security code");
            }

        }
    }
}
