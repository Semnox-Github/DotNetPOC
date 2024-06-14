/********************************************************************************************
 * Project Name - ClientAppUserBL 
 * Description  - Bussiness logic of the ClientAppUser BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        04-May-2016    Rakshith         Created 
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
using Semnox.Parafait.Site;

namespace Semnox.Parafait.ClientApp
{
    public class ClientAppUser
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ClientAppUserDTO clientAppUserDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientAppUser(ExecutionContext executionContext)
        {
            log.LogMethodEntry();           
            this.executionContext = executionContext;
            log.LogMethodExit();
            
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="clientAppUserDTO">ClientAppUserDTO</param>
        public ClientAppUser(ExecutionContext executionContext, ClientAppUserDTO clientAppUserDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, clientAppUserDTO);
            this.clientAppUserDTO = clientAppUserDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'ClientAppUser"'  Request  ====>  ""ClientAppUser"" DataHandler
        public ClientAppUser(ExecutionContext executionContext,int userId,SqlTransaction sqlTransaction=null)   
            : this(executionContext)
        {
            log.LogMethodEntry();
            ClientAppUserDatahandler clientAppUserDatahandler = new ClientAppUserDatahandler(sqlTransaction);
            this.clientAppUserDTO = clientAppUserDatahandler.GetClientAppUserDTOById(userId);
            log.LogMethodExit();
        }

        public void Register(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            // Save calls validate which does the duplicate device validation
            Save();
            log.LogMethodExit();
        }
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

            if (clientAppUserDTO.IsChanged == false
                     && clientAppUserDTO.ClientAppUserId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }

            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors != null && validationErrors.Any())
                throw new ValidationException("Validation Failed", validationErrors);

            ClientAppUserDatahandler clientAppUserDatahandler = new ClientAppUserDatahandler(sqlTransaction);
            try
            {
                if (clientAppUserDTO.ClientAppUserId <= 0)
                {
                    clientAppUserDTO  = clientAppUserDatahandler.InsertClientAppUser(clientAppUserDTO, executionContext.GetUserId(),executionContext.GetSiteId());
                    clientAppUserDTO.AcceptChanges(); //= programId;
                }
                else
                {
                    if (clientAppUserDTO.IsChanged == true)
                    {
                        clientAppUserDTO=clientAppUserDatahandler.UpdateClientAppUser(clientAppUserDTO, executionContext.GetUserId(),executionContext.GetSiteId());
                        clientAppUserDTO.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit();

        }
        private List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = new List<ValidationError>();

            if (String.IsNullOrEmpty(clientAppUserDTO.DeviceGuid))
                validationErrors.Add(new ValidationError("ClientAppUser", "DeviceGUID", MessageContainerList.GetMessage(executionContext, "Invalid Device")));

            List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>>();
            searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.DEVICE_GUID, clientAppUserDTO.DeviceGuid));
            ClientAppUserList clientAppUserListBL = new ClientAppUserList(executionContext);
            List<ClientAppUserDTO> clientAppUserDTOList = clientAppUserListBL.GetAllClientAppUsers(searchParameters);
            if (clientAppUserDTOList != null && clientAppUserDTOList.Any())
            {
                if (clientAppUserDTO.ClientAppUserId > -1 && clientAppUserDTO.ClientAppUserId != clientAppUserDTOList[0].ClientAppUserId)
                {
                    validationErrors.Add(new ValidationError("ClientAppUser", "DeviceGUID", MessageContainerList.GetMessage(executionContext, "A duplicate device is being registered")));
                }
                else if (clientAppUserDTO.ClientAppUserId == -1)
                {
                    validationErrors.Add(new ValidationError("ClientAppUser", "DeviceGUID", MessageContainerList.GetMessage(executionContext, "A duplicate device is being registered")));
                }
            }

            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        public void LoginUser()
        {
            log.LogMethodEntry();
            log.Debug(clientAppUserDTO);
            List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>>();
            searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.CLIENT_APP_ID, clientAppUserDTO.ClientAppId.ToString()));

            ClientAppUserList clientAppUserListBL = new ClientAppUserList(executionContext);
            List<ClientAppUserDTO> clientAppUserDTOList = clientAppUserListBL.GetAllClientAppUsers(searchParameters);

            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            int activeUsers = 0;
            if (clientAppUserDTOList != null && clientAppUserDTOList.Any())
            {
                log.Debug("Found clientAppUserDTOList " + clientAppUserDTOList.Count);
                activeUsers = clientAppUserDTOList.Count(x => x.IsActive && x.UserSignedIn && x.SignInExpiry > currentTime); // to do add login expiry date validation
                log.Debug("activeUsers " + activeUsers);
            }

            ClientAppUserDTO tempDTO = clientAppUserDTOList.FirstOrDefault(x => x.DeviceGuid == this.clientAppUserDTO.DeviceGuid);
            if (tempDTO != null && (!tempDTO.UserSignedIn || tempDTO.SignInExpiry <= currentTime))
            {
                activeUsers = activeUsers + 1;
            }
            log.Debug("final activeUsers " + activeUsers);
            ClientAppBL clientAppBL = new ClientAppBL(executionContext, clientAppUserDTO.ClientAppId);
            ProductKeyListBL productKeyListBL = new ProductKeyListBL(executionContext);
            if (productKeyListBL.ValidateLicenseCount(clientAppBL.GetClientAppDTO.AppName, activeUsers))
            {
                clientAppUserDTO.IsActive = true;
                clientAppUserDTO.UserSignedIn = true;
                clientAppUserDTO.SignInExpiry = currentTime.AddDays(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "SIGN_IN_DAYS_FOR_APP",15) == 0 ? 
                                                                    7 : ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "SIGN_IN_DAYS_FOR_APP",15));
                Save();
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext,"Free license not found."));
            }
        }

        public void LoginOutUser(Boolean? allDevices = null)
        {
            log.LogMethodEntry(allDevices);
            log.Debug(clientAppUserDTO);
            List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>>();

            log.Debug("Log Out Details " + allDevices + ":" + clientAppUserDTO.UserId + ":" + clientAppUserDTO.CustomerId + ":" + clientAppUserDTO.DeviceGuid);
            if (allDevices != null && Convert.ToBoolean(allDevices) == true)
            {
                if(clientAppUserDTO.UserId !=  -1)
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.USER_ID, clientAppUserDTO.UserId.ToString()));
                else
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.CUSTOMER_ID, clientAppUserDTO.CustomerId.ToString()));
            }
            else
            {
                searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.DEVICE_GUID, clientAppUserDTO.DeviceGuid.ToString()));
            }

            ClientAppUserList clientAppUserListBL = new ClientAppUserList(executionContext);
            List<ClientAppUserDTO> clientAppUserDTOList = clientAppUserListBL.GetAllClientAppUsers(searchParameters);

            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            if (clientAppUserDTOList != null && clientAppUserDTOList.Any())
            {
                log.Debug("Found clientAppUserDTOList " + clientAppUserDTOList.Count);
                clientAppUserDTOList = clientAppUserDTOList.Where(x => x.UserSignedIn).ToList();
                foreach (ClientAppUserDTO tempDTO in clientAppUserDTOList)
                {
                    tempDTO.UserSignedIn = false;
                    tempDTO.SignInExpiry = currentTime;
                }
                ClientAppUserList clientAppUserList = new ClientAppUserList(executionContext, clientAppUserDTOList);
                clientAppUserList.Save();
            }
        }

        /// <summary>
        /// Delete the ClientAppUserDTO based on Id
        /// </summary>
        public int Delete()
        {
            try
            {
                ClientAppUserDatahandler clientAppUserDatahandler = new ClientAppUserDatahandler();
                return clientAppUserDatahandler.DeleteClientAppUser(this.clientAppUserDTO.UserId);
            }
            catch (Exception expn)
            {
                throw new System.Exception(expn.Message.ToString());
            }

        }

        /// <summary>
        /// Gets the ClientAppUserDTO
        /// </summary>
        public ClientAppUserDTO GetClientAppUserDTO
        {
            get { return clientAppUserDTO; }
        }
    }

    /// <summary>
    /// Class ClientAppUserList contains List methods
    /// </summary>
    public class ClientAppUserList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ClientAppUserDTO> clientAppUserList;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
       
        public ClientAppUserList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);            
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="clientAppUserList"></param>
        public ClientAppUserList(ExecutionContext executionContext, List<ClientAppUserDTO> clientAppUserList)
        {
            log.LogMethodEntry(executionContext, clientAppUserList);
            this.clientAppUserList = clientAppUserList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns Search based on Parameters And returns  List<ClientDTO>   
        /// </summary>
        public List<ClientAppUserDTO> GetAllClientAppUsers(List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters)
        {
                log.LogMethodEntry(searchParameters);
                ClientAppUserDatahandler clientAppUserDatahandler = new ClientAppUserDatahandler();
                List<ClientAppUserDTO> clientAppUserDTOList= clientAppUserDatahandler.GetAllClientAppUsers(searchParameters);                
                log.LogMethodExit(clientAppUserDTOList);
                return clientAppUserDTOList;
            
        }

        public void Save()
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (clientAppUserList != null && clientAppUserList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (ClientAppUserDTO clientAppUserDTO in clientAppUserList)
                        {
                            ClientAppUser clientAppUser = new ClientAppUser(executionContext, clientAppUserDTO);
                            clientAppUser.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
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
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
                log.LogMethodExit();
            }
        }
    }
}
