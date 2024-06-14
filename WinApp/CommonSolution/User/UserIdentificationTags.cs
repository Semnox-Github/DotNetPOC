/********************************************************************************************
 * Project Name - UserIdentificationTags
 * Description  - Bussiness logic of Users Tags
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Apr-2017   Amaresh      Created 
 *2.70        08-May-2019   Mushahid Faizan  Added SaveUpdateUserIdList() and UserIdTagsList constructor.
 *2.70.2        15-Jul-2019   Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *            09-Aug-2019   Mushahid Faizan Added delete in Save() method.
 *2.80      03-Apr-2020   Mushahid Faizan    Added Delete Method for Web Management Studio.
 *            09-Apr-2020   Indrajeet Kumar  Modified - Added - Encrypt of FPTemplate in Save() and Decrypt of FPTemplate in GetAllUserIdenTags()
 *2.110.0     27-Nov-2020   Lakshminarayana Modified : Changed as part of POS UI redesign. Implemented the new design principles 
 *2.140.0     23-June-2021  Prashanth V     Modified : modified Save method to include save of HRApprovalLogsDTOList
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Represents a user identification tag.
    /// </summary>
    internal class UserIdentificationTags
    {
        private UserIdentificationTagsDTO userIdentificationTagsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        private UserIdentificationTags(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="userIdentificationTagsDTO">UserIdentificationTagsDTO</param>
        internal UserIdentificationTags(ExecutionContext executionContext, UserIdentificationTagsDTO userIdentificationTagsDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userIdentificationTagsDTO);
            if(userIdentificationTagsDTO.Id < 0)
            {
                if (userIdentificationTagsDTO.FPTemplate == null && userIdentificationTagsDTO.FPSalt == null && 
                    userIdentificationTagsDTO.FingerNumber <= -1)
                {
                    ValidateCardNumber(userIdentificationTagsDTO.CardNumber);
                }              
                ValidateStartDateAndEndDate(userIdentificationTagsDTO.StartDate, userIdentificationTagsDTO.EndDate);
            }
            this.userIdentificationTagsDTO = userIdentificationTagsDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public UserIdentificationTags(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            UserIdTagsDatahandler userIdTagsDatahandler = new UserIdTagsDatahandler(sqlTransaction);
            userIdentificationTagsDTO = userIdTagsDatahandler.GetUserIdentificationTagsDTO(id);
            if (userIdentificationTagsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UserIdentificationTag", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get UserDto Object
        /// </summary>
        internal UserIdentificationTagsDTO UserIdentificationTagsDTO
        {
            get { return userIdentificationTagsDTO; }
        }


        /// <summary>
        /// Saves the Users
        /// Checks if the User id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserIdTagsDatahandler userIdTagsDatahandler = new UserIdTagsDatahandler(sqlTransaction);

            if (userIdentificationTagsDTO.Id < 0)
            {
                log.Debug("userIdentificationTagsDTO FPTemplate  Value :" + userIdentificationTagsDTO.FPTemplate);
                EncryptFPTemplate();
                userIdentificationTagsDTO = userIdTagsDatahandler.InsertUserIdentificationTag(userIdentificationTagsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(userIdentificationTagsDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("UserIdentificationTags", userIdentificationTagsDTO.Guid, sqlTransaction);
                }
                DecryptFPTemplate();
                userIdentificationTagsDTO.AcceptChanges();
            }
            else
            {
                if (userIdentificationTagsDTO.IsChanged)
                {
                    EncryptFPTemplate();
                    userIdentificationTagsDTO = userIdTagsDatahandler.UpdateUserIdentificationTag(userIdentificationTagsDTO, executionContext.GetUserId(), executionContext.GetSiteId());

                    if (!string.IsNullOrEmpty(userIdentificationTagsDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("UserIdentificationTags", userIdentificationTagsDTO.Guid, sqlTransaction);
                    }
                    DecryptFPTemplate();
                    userIdentificationTagsDTO.AcceptChanges();
                }
            }
            // Will Save the Child HRApprovalLogsDTOList
            if (userIdentificationTagsDTO.HRApprovalLogsDTOList != null && userIdentificationTagsDTO.HRApprovalLogsDTOList.Any())
            {
                List<HRApprovalLogsDTO> updatedHRApprovalLogsDTO = new List<HRApprovalLogsDTO>();
                foreach (HRApprovalLogsDTO hrApprovalLogsDTO in userIdentificationTagsDTO.HRApprovalLogsDTOList)
                {
                    if (hrApprovalLogsDTO.IsChanged)
                    {
                        updatedHRApprovalLogsDTO.Add(hrApprovalLogsDTO);
                    }
                }
                if (updatedHRApprovalLogsDTO.Any())
                {
                    HRApprovalLogsListBL hrApprovalLogsListBL = new HRApprovalLogsListBL(executionContext, updatedHRApprovalLogsDTO);
                    hrApprovalLogsListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void DecryptFPTemplate()
        {
            log.LogMethodEntry();
            if (userIdentificationTagsDTO.FPTemplate != null && userIdentificationTagsDTO.FPSalt != null)
            {
                userIdentificationTagsDTO.FPTemplate = Encryption.Decrypt(userIdentificationTagsDTO.FPTemplate, Encryption.getKey(userIdentificationTagsDTO.FPSalt));
            }
            log.LogMethodExit();
        }

        private void EncryptFPTemplate()
        {
            log.LogMethodEntry();
            if (userIdentificationTagsDTO.FPTemplate != null && userIdentificationTagsDTO.FPTemplate.Length > 0)
            {
                string fpSalt = new RandomString(10);
                userIdentificationTagsDTO.FPTemplate = Encryption.Encrypt(userIdentificationTagsDTO.FPTemplate, new UserEncryptionKey(executionContext, fpSalt));
                userIdentificationTagsDTO.FPSalt = fpSalt;
            }
            log.LogMethodExit();
        }

        internal void Update(UserIdentificationTagsDTO parameterUserIdentificationTagsDTO)
        {
            log.LogMethodEntry(parameterUserIdentificationTagsDTO);
            ChangeCardNumber(parameterUserIdentificationTagsDTO.CardNumber);
            ChangeActiveFlag(parameterUserIdentificationTagsDTO.ActiveFlag);
            ChangeStartDateAndEndDate(parameterUserIdentificationTagsDTO.StartDate, parameterUserIdentificationTagsDTO.EndDate);
            ChangeAttendanceReaderTag(parameterUserIdentificationTagsDTO.AttendanceReaderTag);
            ChangeFPTemplate(parameterUserIdentificationTagsDTO.FPTemplate);
            ChangeFingerNumber(parameterUserIdentificationTagsDTO.FingerNumber);
            ChangeFingerPrint(parameterUserIdentificationTagsDTO.FingerPrint);
            ChangeCardId(parameterUserIdentificationTagsDTO.CardId);
            log.LogMethodExit();
        }

        private void ChangeCardId(int cardId)
        {
            log.LogMethodEntry(cardId);
            if (userIdentificationTagsDTO.CardId == cardId)
            {
                log.LogMethodExit(null, "No changes to user identification tag cardId");
                return;
            }
            userIdentificationTagsDTO.CardId = cardId;
            log.LogMethodExit();
        }

        private void ChangeFingerPrint(string fingerPrint)
        {
            log.LogMethodEntry(fingerPrint);
            if (userIdentificationTagsDTO.FingerPrint == fingerPrint)
            {
                log.LogMethodExit(null, "No changes to user identification tag fingerPrint");
                return;
            }
            userIdentificationTagsDTO.FingerPrint = fingerPrint;
            log.LogMethodExit();
        }

        private void ChangeFingerNumber(int fingerNumber)
        {
            log.LogMethodEntry(fingerNumber);
            if (userIdentificationTagsDTO.FingerNumber == fingerNumber)
            {
                log.LogMethodExit(null, "No changes to user identification tag fingerNumber");
                return;
            }
            userIdentificationTagsDTO.FingerNumber = fingerNumber;
            log.LogMethodExit();
        }

        private void ChangeFPTemplate(byte[] fPTemplate)
        {
            log.LogMethodEntry(fPTemplate);
            if (Core.GenericUtilities.ByteArray.Equals(userIdentificationTagsDTO.FPTemplate, fPTemplate))
            {
                log.LogMethodExit(null, "No changes to user identification tag fPTemplate");
                return;
            }
            userIdentificationTagsDTO.FPTemplate = fPTemplate;
            log.LogMethodExit();
        }

        private void ChangeAttendanceReaderTag(bool attendanceReaderTag)
        {
            log.LogMethodEntry(attendanceReaderTag);
            if (userIdentificationTagsDTO.AttendanceReaderTag == attendanceReaderTag)
            {
                log.LogMethodExit(null, "No changes to user identification tag attendanceReaderTag");
                return;
            }
            userIdentificationTagsDTO.AttendanceReaderTag = attendanceReaderTag;
            log.LogMethodExit();
        }

        private void ChangeStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            log.LogMethodEntry(startDate, endDate);
            if (userIdentificationTagsDTO.StartDate == startDate && 
                userIdentificationTagsDTO.EndDate == endDate)
            {
                log.LogMethodExit(null, "No changes to user identification tag startDate and endDate");
                return;
            }
            ValidateStartDateAndEndDate(startDate, endDate);
            userIdentificationTagsDTO.StartDate = startDate;
            userIdentificationTagsDTO.EndDate = endDate;
            log.LogMethodExit();
        }

        private void ValidateStartDateAndEndDate(DateTime startDate, DateTime endDate)
        {
            log.LogMethodEntry(startDate, endDate);
            if (startDate != DateTime.MinValue &&
                endDate != DateTime.MinValue &&
                startDate > endDate)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "End Date"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "User", "EmpEndDate", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ChangeActiveFlag(bool activeFlag)
        {
            log.LogMethodEntry(activeFlag);
            if (userIdentificationTagsDTO.ActiveFlag == activeFlag)
            {
                log.LogMethodExit(null, "No changes to user identification tag activeFlag");
                return;
            }
            userIdentificationTagsDTO.ActiveFlag = activeFlag;
            log.LogMethodExit();
        }

        private void ChangeCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            if (userIdentificationTagsDTO.FPTemplate == null && userIdentificationTagsDTO.FPSalt == null &&
                    userIdentificationTagsDTO.FingerNumber <= -1)
            {
                ValidateCardNumber(cardNumber);
                if (userIdentificationTagsDTO.CardNumber == cardNumber)
                {
                    log.LogMethodExit(null, "No changes to user identification tag cardNumber");
                    return;
                }
                userIdentificationTagsDTO.CardNumber = cardNumber;
            }                                    
            log.LogMethodExit();
        }

        private void ValidateCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            if(tagNumberParser.IsValid(cardNumber) == false)
            {
                string errorMessage = tagNumberParser.Validate(cardNumber);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "UserIdentificationTag", "CardNumber", errorMessage);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of UserIdTags
    /// </summary>
    public class UserIdentificationTagListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<UserIdentificationTagsDTO> userIdentificationTagsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserIdentificationTagListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public UserIdentificationTagListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="UserIdentificationTagsList"></param>
        public UserIdentificationTagListBL(ExecutionContext executionContext, List<UserIdentificationTagsDTO> UserIdentificationTagsList)
             : this()
        {
            log.LogMethodEntry(executionContext, UserIdentificationTagsList);
            this.executionContext = executionContext;
            this.userIdentificationTagsList = UserIdentificationTagsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the UserIdentificationTagsDTO List for User Id List
        /// </summary>
        /// <param name="userIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<UserIdentificationTagsDTO> GetUserIdentificationTagDTOListOfUsers(List<int> userIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userIdList, activeRecords, sqlTransaction);
            UserIdTagsDatahandler userIdTagsDatahandler = new UserIdTagsDatahandler(sqlTransaction);
            List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdTagsDatahandler.GetUserIdentificationTagDTOListOfUsers(userIdList, activeRecords);
            DecryptFingerPrintTemplate(userIdentificationTagsDTOList);
            log.LogMethodExit(userIdentificationTagsDTOList);
            return userIdentificationTagsDTOList;
        }

        /// <summary>
        /// Returns the users list
        /// </summary>
        public List<UserIdentificationTagsDTO> GetUserIdentificationTagsDTOList(List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserIdTagsDatahandler userIdTagsDatahandler = new UserIdTagsDatahandler(sqlTransaction);
            List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdTagsDatahandler.GetUserIdTagsList(searchParameters);
            DecryptFingerPrintTemplate(userIdentificationTagsDTOList);
            log.LogMethodExit(userIdentificationTagsDTOList);
            return userIdentificationTagsDTOList;
        }

        private void DecryptFingerPrintTemplate(List<UserIdentificationTagsDTO> userIdentificationTagsDTOList)
        {
            if (userIdentificationTagsDTOList != null && userIdentificationTagsDTOList.Any())
            {
                foreach (UserIdentificationTagsDTO userIdentificationTagsDTO in userIdentificationTagsDTOList)
                {
                    if (userIdentificationTagsDTO.FPTemplate != null && userIdentificationTagsDTO.FPSalt != null)
                    {
                        userIdentificationTagsDTO.FPTemplate = Encryption.Decrypt(userIdentificationTagsDTO.FPTemplate, Encryption.getKey(userIdentificationTagsDTO.FPSalt));
                        userIdentificationTagsDTO.AcceptChanges();
                    }
                }
            }
        }

        /// <summary>
        /// GetUsersCards method
        /// </summary>
        /// <returns>List of UserIdentificationTagsDTO objects</returns>
        public List<UserIdentificationTagsDTO> GetUsersCards(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserIdTagsDatahandler userIdTagsDatahandler = new UserIdTagsDatahandler(sqlTransaction);
            List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdTagsDatahandler.GetStaffCards();
            log.LogMethodExit(userIdentificationTagsDTOList);
            return userIdentificationTagsDTOList;
        }
        
        /// <summary>
        /// This method should be used to Save UserIdentificationTags
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {                
                if (userIdentificationTagsList != null && userIdentificationTagsList.Count != 0)
                {
                    foreach (UserIdentificationTagsDTO userIdentificationTagsDTO in userIdentificationTagsList)
                    {                        
                        UserIdentificationTags userIdentificationTags = new UserIdentificationTags(executionContext, userIdentificationTagsDTO);
                        userIdentificationTags.Save(sqlTransaction);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
