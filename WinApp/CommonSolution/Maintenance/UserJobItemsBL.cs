/********************************************************************************************
 * Project Name - User Job Items
 * Description  - Bussiness logic of User job items
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        19-Jan-2016   Raghuveera        Created 
 *2.70        08-Mar-2019   Guru S A          Renamed MaintenanceJob as UserJobItemsBL
 *2.70.0      22-Apr-2019   Mehraj            Added MachineOutOfService() and SaveJobDetails() method 
              29-May-2019   Jagan Mohan       Code merge from Development to WebManagementStudio
 * 2.70       23-Sept-2019  Rakesh Kumar      Modified the SaveJobDetails method
 * 2.70       23-Oct-2019   Rakesh Kumar      Modified the MachineOutOfService method
 *2.80        10-May-2020   Girish Kundar     Modified: REST API Changes merge from WMS  
 *2.100.0     24-Sept-2020  Mushahid Faizan   Modified for Service Request enhancement 
 *2.110.0     12-Dec-2020   Guru S A          Subscription changes               
 *2.120.0     03-Jun-2021   Gururaja Kanjan   Seperated service request and job / tasks save method
 *2.140.0     11-Jan-2022   Abhishek          WMS Fix: draft request deletion
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// User job items will creates and modifies the jobs    
    /// </summary>
    public class UserJobItemsBL
    {
        private UserJobItemsDTO userJobItemsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = new Utilities();
        private const string COMMUNICATION_LOOKUP = "ServiceRequest";
        private const string SEQUENCE_NAME = "ServiceRequest";
        private const string EMAIL_TEMPLATE = "SERVICE_REQUEST_EMAIL_TEMPLATE";
        private const string STATUS_LOOKUP = "MAINTENANCE_OPEN_JOB_STATUS_TRANSITION";
        private const string OPEN_STATUS_LOOKUP = "OPEN";
        private const string ABANDONED_STATUS_LOOKUP = "ABANDONED";
        private int OPEN_STATUS_ID = -1;
        private int ABANDONED_STATUS_ID = -1;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public UserJobItemsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            userJobItemsDTO = null;
            LoadStatusId();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the with job id
        /// </summary>
        /// <param name="userJobItemsId">Id of the job</param>
        /// <returns>UserJobItemsDTO object</returns>
        public UserJobItemsBL(ExecutionContext executionContext, int userJobItemsId)
        {
            log.LogMethodEntry(executionContext, userJobItemsId);
            this.executionContext = executionContext;
            UserJobItemsDatahandler userJobItemsDataHandler = new UserJobItemsDatahandler(null);
            userJobItemsDTO = userJobItemsDataHandler.GetUserJobItemsDTO(userJobItemsId);
            LoadStatusId();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceJobDTO">Parameter of the type UserJobItemsDTO</param>
        public UserJobItemsBL(ExecutionContext executionContext, UserJobItemsDTO userJobItemsDTO)
        {
            log.LogMethodEntry(executionContext, userJobItemsDTO);
            this.executionContext = executionContext;
            this.userJobItemsDTO = userJobItemsDTO;
            LoadStatusId();
            log.LogMethodExit();
        }

        /// <summary>
        /// returns open status id
        /// </summary>
        private void LoadStatusId()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            Dictionary<int, LookupValuesDTO> lookupValues = lookupValuesList.GetLookupValuesMap(STATUS_LOOKUP);
            foreach (KeyValuePair<int, LookupValuesDTO> lookupValue in lookupValues)
            {
                if (lookupValue.Value.LookupValue.Equals(OPEN_STATUS_LOOKUP))
                {
                    OPEN_STATUS_ID = lookupValue.Key;
                }
                else if (lookupValue.Value.LookupValue.Equals(ABANDONED_STATUS_LOOKUP))
                {
                    ABANDONED_STATUS_ID = lookupValue.Key;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the user job items
        /// Job will be inserted if MaintChklstdetId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void SaveServiceRequest(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            if (userJobItemsDTO.IsChangedRecursive == false
                   && userJobItemsDTO.MaintChklstdetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }

            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            UserJobItemsDatahandler userJobItemsDataHandler = new UserJobItemsDatahandler(sqlTrx);
            Boolean StatusChanged = false;
            if (userJobItemsDTO.SaveType != "Save" && userJobItemsDTO.IsActive != false)
            {
                SaveAndFinalize(sqlTrx, userJobItemsDataHandler);
                StatusChanged = true;

            }

            if (userJobItemsDTO.MaintChklstdetId < 0)
            {
                int userJobItemsId = userJobItemsDataHandler.InsertUserJobItems(userJobItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userJobItemsDTO.MaintChklstdetId = userJobItemsId;
                StatusChanged = true;
            }
            else if (userJobItemsDTO.IsChanged)
            {
                if (!StatusChanged)
                {
                    StatusChanged = HasStatusChanged(userJobItemsDTO.MaintChklstdetId, userJobItemsDTO.Status);
                }
                userJobItemsDataHandler.UpdateUserJobItems(userJobItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            if (StatusChanged)
            {
                SaveMaintenanceJobStatus();
                SendStatusChangeMail(sqlTrx);
            }

            userJobItemsDTO.AcceptChanges();
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            if (userJobItemsDTO.IsChanged == false
                   && userJobItemsDTO.MaintChklstdetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            UserJobItemsDatahandler userJobItemsDataHandler = new UserJobItemsDatahandler(sqlTrx);
            if (userJobItemsDTO.MaintChklstdetId < 0)
            {
                int userJobItemsId = userJobItemsDataHandler.InsertUserJobItems(userJobItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userJobItemsDTO.MaintChklstdetId = userJobItemsId;
            }
            else
            {
                if (userJobItemsDTO.IsChanged == true)
                {
                    userJobItemsDataHandler.UpdateUserJobItems(userJobItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    userJobItemsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// checks if status was updated
        /// </summary>
        /// <param name=""></param>
        private bool HasStatusChanged(int maintChklstdetId, int newStatus)
        {
            log.LogMethodEntry(maintChklstdetId, newStatus);
            bool statusChanged = true;
            UserJobItemsBL jobItem = new UserJobItemsBL(executionContext, maintChklstdetId);
            int oldStatus = jobItem.UserJobItemsDTO.Status;
            if (oldStatus == newStatus)
            {
                statusChanged = false;
            }
            log.LogMethodExit(statusChanged);
            return statusChanged;
        }


        /// <summary>
        /// Saves the child records : MaintenanceJobStatusDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveMaintenanceJobStatus()
        {
            LookupValues lookupValues = new LookupValues(executionContext, userJobItemsDTO.Status);
            MaintenanceJobStatusDTO jobStatusDTO = new MaintenanceJobStatusDTO(-1, userJobItemsDTO.MaintChklstdetId, lookupValues.LookupValuesDTO.Description, true);
            MaintenanceJobStatusBL maintenanceJobStatusBL = new MaintenanceJobStatusBL(executionContext, jobStatusDTO);
            maintenanceJobStatusBL.Save();
        }


        /// <summary>
        /// Called when clicking on save and finalize
        /// </summary>
        private void SaveAndFinalize(SqlTransaction sqlTransaction, UserJobItemsDatahandler userJobItemsDataHandler)
        {
            log.LogMethodEntry(sqlTransaction);
            userJobItemsDTO.Status = OPEN_STATUS_ID;
            SqlConnection cnn = utilities.createConnection();
            SqlTransaction sqlTrx = cnn.BeginTransaction();
            userJobItemsDTO.MaintJobNumber = userJobItemsDataHandler.GetNextSeqNo(SEQUENCE_NAME);
            AutoAssignTechnician();
            log.LogMethodExit();
        }

        /// <summary>
        /// Auto assigns the tehcnican based on mapping
        /// </summary>
        private void AutoAssignTechnician()
        {
            log.LogMethodEntry();
            AssetTechnicianMappingMasterList.GetAssetTechnician(executionContext, userJobItemsDTO.AssetId);

            int technicianID = AssetTechnicianMappingMasterList.GetAssetTechnician(executionContext, userJobItemsDTO.AssetId);
            log.Debug("Asset ID: " + userJobItemsDTO.AssetId + " mapped to technician :" + technicianID);
            if (technicianID > 0)
            {
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, Convert.ToString(technicianID)));
                UsersList usersList = new UsersList(executionContext);

                List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameter);

                if (usersDTOList != null && usersDTOList.Any())
                {
                    UsersDTO usersDTO = usersDTOList.First();
                    userJobItemsDTO.AssignedTo = usersDTO.UserName;
                    userJobItemsDTO.AssignedUserId = usersDTO.UserId;
                    log.Debug("Assigned To: " + usersDTO.UserName);
                }
                else
                {
                    throw new ValidationException("Validation failed", "TechnicianMapping", "User", MessageContainerList.GetMessage(executionContext, 2886, null));
                }

            }
            else
            {
                throw new ValidationException("Validation failed", "TechnicianMapping", "User", MessageContainerList.GetMessage(executionContext, 2886, null));
            }


            log.LogMethodExit();

        }



        /// <summary>
        /// Validate the UserJobItemsDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (userJobItemsDTO.SaveType != "Save" && userJobItemsDTO.Status == ABANDONED_STATUS_ID)
            {
                log.Debug("Error: Abandoned State.  You cannot finalize a service request in abandoned state.");
                //validationErrorList.Add(new ValidationError("ServiceRequest", "State", MessageContainerList.GetMessage(executionContext, 2874, MessageContainerList.GetMessage(executionContext, "ServiceRequest"))));
                validationErrorList.Add(new ValidationError("ServiceRequest", "State", "Error: Abandoned State.  You cannot finalize a service request in abandoned state."));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Get property for DTO
        /// </summary>
        public UserJobItemsDTO UserJobItemsDTO
        {
            get { return userJobItemsDTO; }
        }

        /// <summary>
        /// This method sets a machine out of service
        /// </summary>
        /// <param name="maintenanceJobDTO"></param>
        /// <returns></returns>
        public string MachineOutOfService(int assetId, int jobId)
        {
            try
            {
                if (assetId != -1)
                {
                    bool IsExists = false;
                    GenericAssetDTO assetDTO = new GenericAssetDTO();
                    GenericAsset genericAsset = new GenericAsset(executionContext, assetId);
                    assetDTO = genericAsset.AssetDTO;
                    if (assetDTO == null)
                    {
                        return MessageContainerList.GetMessage(executionContext, 620);
                    }
                    else
                    {
                        if (assetDTO.Machineid == -1)
                        {
                            return MessageContainerList.GetMessage(executionContext, 620);
                        }

                        Machine machine = new Machine(assetDTO.Machineid, executionContext, true);
                        MachineDTO machineDTO = machine.GetMachineDTO;

                        if (machineDTO != null)
                        {
                            List<MachineAttributeDTO> updatedAttributeList = new List<MachineAttributeDTO>();
                            foreach (MachineAttributeDTO machineAttributeDTO in machineDTO.GameMachineAttributes)
                            {
                                if (machineAttributeDTO.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE &&
                                    machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
                                {
                                    IsExists = true;
                                    if (machineAttributeDTO.AttributeValue != "1")
                                    {
                                        machineAttributeDTO.AttributeValue = "1";
                                        machineAttributeDTO.IsChanged = true;
                                    }
                                    else
                                    {
                                        log.Info("Out of Service - Machine " + machineDTO.MachineName + " is already in requested state : 1");
                                        log.Debug("Machine attributes logs not created");
                                        return MessageContainerList.GetMessage(executionContext, 622);
                                    }

                                }
                            }
                            if (!IsExists)
                            {
                                MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE, "1", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "");
                                machineDTO.GameMachineAttributes.Add(updatedAttribute);
                            }
                            machine = new Machine(machineDTO, executionContext);
                            machine.PutOutOfService(MessageContainerList.GetMessage(executionContext, "Setting out of service from service request"), MessageContainerList.GetMessage(executionContext, "For reason please check service request id: " + jobId.ToString()));
                            return MessageContainerList.GetMessage(executionContext, 622);
                        }
                    }

                }
                return MessageContainerList.GetMessage(executionContext, 620);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Retrieves email for the user
        /// </summary>
        /// <param name="userName">string</param>
        /// <returns>Returns string</returns>
        private string getEMailFromName(string userName)
        {
            string email = String.Empty;
            if (!String.IsNullOrWhiteSpace(userName))
            {
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_NAME, userName));
                UsersList usersList = new UsersList(executionContext);

                List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameter);

                if (usersDTOList != null && usersDTOList.Any())
                {
                    UsersDTO usersDTO = usersDTOList.First();
                    email = usersDTO.Email;
                }
            }
            return email;
        }

        /// <summary>
        /// Send email on status change.
        /// </summary>
        /// <param name="SqlTransaction"></param>
        /// <returns></returns>
        public void SendStatusChangeMail(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            /*string emailAddress = string.Empty;
            string tempEmailAddress = string.Empty;
            string assignedToEmailAddress = string.Empty;
            //int messagingClientID = -1;
            
            string requestedBy = userJobItemsDTO.RequestedBy;
            int assignedTo = userJobItemsDTO.AssignedUserId;
            if(assignedTo > 0) 
            {
                Users assignedToUser = new Users(executionContext, assignedTo);
                assignedToEmailAddress = assignedToUser.UserDTO.Email;
            }

            string contactEmail = userJobItemsDTO.ContactEmailId;

            if (string.IsNullOrWhiteSpace(contactEmail) == false)
            {
                emailAddress = contactEmail;
            }
            tempEmailAddress = getEMailFromName(requestedBy);
            if (string.IsNullOrWhiteSpace(tempEmailAddress) == false)
            {
                if (String.IsNullOrWhiteSpace(emailAddress))
                {
                    emailAddress = tempEmailAddress;
                }
                else
                {
                    emailAddress = emailAddress + ";" + tempEmailAddress;
                }
            }

            if (string.IsNullOrWhiteSpace(assignedToEmailAddress) == false)
            {
                if (String.IsNullOrWhiteSpace(emailAddress))
                {
                    emailAddress = assignedToEmailAddress;
                }
                else
                {
                    emailAddress = emailAddress + ";" + assignedToEmailAddress;
                }
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                log.LogMethodExit(null, "No Valid Email found to send the registartion mail");
                return;
            }
            */
            //EmailTemplateDTO emailTemplateDTO = null;
            //try
            //{
            //    emailTemplateDTO = new EmailTemplate(executionContext).GetEmailTemplate(EMAIL_TEMPLATE, executionContext.GetSiteId());
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Error occured while retrieving the customer registation template", ex);
            //}

            //if (emailTemplateDTO == null)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 2194);
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new Exception(errorMessage);
            //}
            //string emailContent = emailTemplateDTO.EmailTemplate;
            //emailContent = emailContent.Replace("@ServiceRequest", userJobItemsDTO.MaintJobNumber+", "+ userJobItemsDTO.MaintJobName);

            //LookupValues lookupValues = new LookupValues(executionContext, userJobItemsDTO.Status);
            //emailContent = emailContent.Replace("@Status", lookupValues.LookupValuesDTO.Description);
            //SiteList siteList = new SiteList(executionContext);
            //List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
            //string siteName = string.Empty;
            //if (siteDTOList != null)
            //{
            //    SiteDTO siteDTO = null;
            //    if (executionContext.GetSiteId() == -1)
            //    {
            //        siteDTO = siteDTOList.FirstOrDefault();
            //    }
            //    else
            //    {
            //        siteDTO = siteDTOList.Where(x => x.SiteId == executionContext.GetSiteId()).FirstOrDefault();
            //    }
            //    if (siteDTO != null)
            //    {
            //        siteName = siteDTO.SiteName;
            //    }
            //}
            //emailContent = emailContent.Replace("@siteName", siteName);
            try
            {
                MaintenanceEventsBL maintenanceMessageEventsBL = new MaintenanceEventsBL(executionContext, ParafaitFunctionEvents.SERVICE_REQUEST_STATUS_CHANGE_EVENT, userJobItemsDTO, sqlTransaction);
                maintenanceMessageEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                //MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                //List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTO = messagingClientFunctionLookUpListBL.GetMessagingClientDTOListByFunctionName("CUSTOMER_FUNCTIONS_LOOKUP", COMMUNICATION_LOOKUP, "E");
                //if (messagingClientFunctionLookUpDTO != null && messagingClientFunctionLookUpDTO.Any())
                //    messagingClientID = messagingClientFunctionLookUpDTO[0].MessagingClientDTO.ClientId;

                ////"Status Change Email"
                //MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, null, emailTemplateDTO.Description, "E", emailAddress, "", "", null, null, null, null,
                //    "Service Request Update", emailContent,-1, null, "", true, emailAddress, "", messagingClientID, false,"");
                //MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                //messagingRequestBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while sending status change e-mail", ex);
                Semnox.Parafait.logger.EventLog.Log(executionContext, "Service Request Update", "E", string.Empty, "Unable to save message request for service request status update", string.Empty, string.Empty, 3, string.Empty, string.Empty);
            }
            log.LogMethodExit();
        }

    }


    /// <summary>
    /// Manages the list of User Job Items 
    /// </summary>
    public class UserJobItemsListBL
    {

        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<UserJobItemsDTO> userJobItemsDTOList = new List<UserJobItemsDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public UserJobItemsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.userJobItemsDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public UserJobItemsListBL(ExecutionContext executionContext, List<UserJobItemsDTO> userJobItemsDTOList)
        {
            log.LogMethodEntry(executionContext, userJobItemsDTOList);
            this.userJobItemsDTOList = userJobItemsDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the User job items dto list
        /// </summary>
        public List<UserJobItemsDTO> GetAllUserJobItemDTOList(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters, int userId = -1,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, userId);
            // List<UserJobItemsDTO> userJobItemsDTOList = null;
            UserJobItemsDatahandler userJobItemDatahandler = new UserJobItemsDatahandler(null);
            if (userId == -1)
            {
                userJobItemsDTOList = userJobItemDatahandler.GetAllUserJobItemsDTOList(searchParameters);
            }
            else
            {
                userJobItemsDTOList = userJobItemDatahandler.GetUserJobItemsDTOList(searchParameters, userId);
            }
            if (userJobItemsDTOList != null && userJobItemsDTOList.Any() && loadChildRecords)
            {
                Build(userJobItemsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }

        private void Build(List<UserJobItemsDTO> userJobItemsDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, UserJobItemsDTO> userJobItemsDTODictionary = new Dictionary<int, UserJobItemsDTO>();
            List<int> maintChklstdetIdList = new List<int>();
            for (int i = 0; i < userJobItemsDTOList.Count; i++)
            {
                if (userJobItemsDTODictionary.ContainsKey(userJobItemsDTOList[i].MaintChklstdetId))
                {
                    continue;
                }
                userJobItemsDTODictionary.Add(userJobItemsDTOList[i].MaintChklstdetId, userJobItemsDTOList[i]);
                maintChklstdetIdList.Add(userJobItemsDTOList[i].MaintChklstdetId);
            }
            MaintenanceJobStatusListBL maintenanceJobStatusListBL = new MaintenanceJobStatusListBL(executionContext);
            List<MaintenanceJobStatusDTO> maintenanceJobStatusDTOList = maintenanceJobStatusListBL.GetMaintenanceJobStatusDTOList(maintChklstdetIdList, true, sqlTransaction);

            if (maintenanceJobStatusDTOList != null && maintenanceJobStatusDTOList.Any())
            {
                for (int i = 0; i < maintenanceJobStatusDTOList.Count; i++)
                {
                    if (userJobItemsDTODictionary.ContainsKey(maintenanceJobStatusDTOList[i].MaintChklstdetailId) == false)
                    {
                        continue;
                    }
                    UserJobItemsDTO userJobItemsDTO = userJobItemsDTODictionary[maintenanceJobStatusDTOList[i].MaintChklstdetailId];
                    if (userJobItemsDTO.MaintenanceJobStatusDTOList == null)
                    {
                        userJobItemsDTO.MaintenanceJobStatusDTOList = new List<MaintenanceJobStatusDTO>();
                    }
                    userJobItemsDTO.MaintenanceJobStatusDTOList.Add(maintenanceJobStatusDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Returns the job list in batch. Used for external systems.
        /// <param name="searchParameters">Parameter list for the select query</param>
        /// <param name="maxRows">Maximum Rows to be returned</param>
        /// </summary>
        public List<UserJobItemsDTO> GetAllUserJobItemsDTOBatch(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters, int maxRows = int.MaxValue)
        {
            log.LogMethodEntry(searchParameters, maxRows);
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            UserJobItemsDatahandler maintenanceJobDataHandler = new UserJobItemsDatahandler(null);
            userJobItemsDTOList = maintenanceJobDataHandler.GetUserJobItemsListBatch(searchParameters, maxRows);
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }

        /// <summary>
        /// Save JobDetails
        /// </summary>
        public void SaveJobDetails(string activityType)
        {
            try
            {
                log.LogMethodEntry(activityType);

                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<LookupValuesDTO> jobTypelookupValuesDTOList = new List<LookupValuesDTO>();
                List<LookupValuesDTO> statuslookupValuesDTOList = new List<LookupValuesDTO>();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();

                if (userJobItemsDTOList == null || userJobItemsDTOList.Any() == false)
                {
                    log.LogMethodExit(null, "List is empty");
                    return;
                }

                if (userJobItemsDTOList != null && userJobItemsDTOList.Any() && !string.IsNullOrEmpty(activityType))
                {
                    switch (activityType.ToUpper().ToString())
                    {
                        case "MAINTENANCEREQUESTS":
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                            {
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                            };
                            jobTypelookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                            if (jobTypelookupValuesDTOList != null && jobTypelookupValuesDTOList.Any())
                            {
                                //userJobItemsDTOList.ForEach(u => u.MaintJobType = lookupValuesDTOList[0].LookupValueId);
                                foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                                {
                                    userJobItemsDTO.MaintJobType = jobTypelookupValuesDTOList[0].LookupValueId;
                                    userJobItemsDTO.MaintJobName = userJobItemsDTO.TaskName;
                                    UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTO);
                                    userJobItemsBL.SaveServiceRequest();
                                }
                            }
                            break;
                        case "MAINTENANCEJOBDETAILS":
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                            {
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                            };
                            jobTypelookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                                    {
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"),
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "close"),
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                                    };
                            List<LookupValuesDTO> lookupValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                                    {
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"),
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "Open"),
                                        new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                                    };
                            List<LookupValuesDTO> lookupOpenValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);


                            foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                            {
                                if (userJobItemsDTO.IsChanged)
                                {
                                    if (userJobItemsDTO.ChklistValue)
                                    {
                                        if (lookupValuesDTO != null && lookupValuesDTO.Any())
                                        {
                                            if (userJobItemsDTO.Status != lookupValuesDTO[0].LookupValueId)
                                            {
                                                userJobItemsDTO.Status = lookupValuesDTO[0].LookupValueId;
                                                userJobItemsDTO.ChecklistCloseDate = DateTime.Now.ToString("MM/dd/yyyy, hh:mm:ss");
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception();
                                        }
                                    }
                                    else
                                    {
                                        if (lookupOpenValuesDTO != null && lookupOpenValuesDTO.Any())
                                        {
                                            if (lookupValuesDTO[0].LookupValueId == userJobItemsDTO.Status
                                                && userJobItemsDTO.ChklistValue == false)
                                                userJobItemsDTO.Status = lookupOpenValuesDTO[0].LookupValueId;
                                        }
                                        userJobItemsDTO.ChecklistCloseDate = "";
                                    }
                                }
                                else
                                {
                                    userJobItemsDTO.MaintJobType = jobTypelookupValuesDTOList[0].LookupValueId;
                                }

                                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTO);
                                userJobItemsBL.Save();
                            }
                            break;
                        case "CREATEADHOCJOB":
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                            {
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "open"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                            };
                            statuslookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                            //userJobItemsDTOList.ForEach(u => u.Status = lookupValuesDTOList[0].LookupValueId);
                            lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                            {
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, "Job"),
                                new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString())
                            };
                            jobTypelookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                            //userJobItemsDTOLlookupValuesDTOListist.ForEach(u => u.MaintJobType = lookupValuesDTOList[0].LookupValueId);
                            foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                            {
                                userJobItemsDTO.MaintJobType = jobTypelookupValuesDTOList[0].LookupValueId;
                                userJobItemsDTO.Status = statuslookupValuesDTOList[0].LookupValueId;
                                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTO);
                                userJobItemsBL.Save();
                            }

                            break;
                    }
                }
                /*if (userJobItemsDTOList != null && userJobItemsDTOList.Any())
                {
                    foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                    {
                        userJobItemsDTO.MaintJobName = userJobItemsDTO.TaskName;
                        UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, userJobItemsDTO);
                        userJobItemsBL.Save();
                    }
                }*/
                log.LogMethodExit();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Validation Exception : " + ex.Message);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw;
            }
        }

        public Sheet BuildTemplate(string reqFromDate, string reqToDate, string siteName)
        {
            try
            {
                log.LogMethodEntry();
                Sheet sheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();

                ///All defaultvalues for attributes are in defaultValueRow object
                Row defaultSiteValueRow = new Row();
                defaultSiteValueRow.AddCell(new Cell());
                defaultSiteValueRow.AddCell(new Cell("Site:"));
                defaultSiteValueRow.AddCell(new Cell(siteName));
                sheet.AddRow(defaultSiteValueRow);
                Row defaultReqFromDateValueRow = new Row();
                defaultReqFromDateValueRow.AddCell(new Cell());
                defaultReqFromDateValueRow.AddCell(new Cell("From:"));
                defaultReqFromDateValueRow.AddCell(new Cell(reqFromDate));
                sheet.AddRow(defaultReqFromDateValueRow);
                Row defaultReqToDateValueRow = new Row();
                defaultReqToDateValueRow.AddCell(new Cell());
                defaultReqToDateValueRow.AddCell(new Cell("To:"));
                defaultReqToDateValueRow.AddCell(new Cell(reqToDate));
                sheet.AddRow(defaultReqToDateValueRow);

                Row defaultRunAtValueRow = new Row();
                defaultRunAtValueRow.AddCell(new Cell());
                defaultRunAtValueRow.AddCell(new Cell("Run At:"));
                defaultRunAtValueRow.AddCell(new Cell(DateTime.Now.ToString()));
                sheet.AddRow(defaultRunAtValueRow);

                Row defaultRunByValueRow = new Row();
                defaultRunByValueRow.AddCell(new Cell());
                defaultRunByValueRow.AddCell(new Cell("Run By:"));
                defaultRunByValueRow.AddCell(new Cell(executionContext.GetUserId()));
                sheet.AddRow(defaultRunByValueRow);

                Row defaultEmptyValueRow = new Row();
                sheet.AddRow(defaultEmptyValueRow);


                ///Mapper class thats map sheet object
                UserJobItemsDTODefinition userJobItemsDTODefinition = new UserJobItemsDTODefinition(executionContext, "");
                foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                {
                    userJobItemsDTODefinition.Configure(userJobItemsDTO);
                }
                userJobItemsDTODefinition.BuildHeaderRow(headerRow);
                sheet.AddRow(headerRow);
                foreach (UserJobItemsDTO userJobItemsDTO in userJobItemsDTOList)
                {
                    Row row = new Row();
                    userJobItemsDTODefinition.Serialize(row, userJobItemsDTO);
                    sheet.AddRow(row);
                }
                log.LogMethodExit();
                return sheet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
