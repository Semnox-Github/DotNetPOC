/********************************************************************************************
 * Project Name - UserPayRateBL
 * Description  - BL to setup User Pay Rate
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130       05-Jul-2021      Nitin Pai  Added: Attendance and Pay Rate enhancement
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business logic for UserPayRate class.
    /// </summary>
    public class UserPayRateBL
    {
        private UserPayRateDTO userPayRateDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of UserPayRateBL class
        /// </summary>
        private UserPayRateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the UserPayRate id as the parameter
        /// Would fetch the UserPayRate object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="userPayRateId">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public UserPayRateBL(ExecutionContext executionContext, int userPayRateId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userPayRateId, sqlTransaction);
            UserPayRateDataHandler userPayRateDataHandler = new UserPayRateDataHandler(sqlTransaction);
            userPayRateDTO = userPayRateDataHandler.GetUserPayRateDTO(userPayRateId);
            if (userPayRateDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UserPayRate", userPayRateId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates UserPayRateBL object using the UserPayRateDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="userPayRateDTO">UserPayRateDTO object</param>
        public UserPayRateBL(ExecutionContext executionContext, UserPayRateDTO userPayRateDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userPayRateDTO);
            this.userPayRateDTO = userPayRateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the UserPayRate
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            int threshHoldDays = -1 * ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "USERPAY_EFFECTIVEDATE_PAST_THRESHOLD", 30);
            log.Debug("Threshold days " + threshHoldDays);
            // Pay rate cannot be in past
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            if (UserPayRateDTO.EffectiveDate.Date < lookupValuesList.GetServerDateTime().Date.AddDays(threshHoldDays))
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5106, threshHoldDays * -1));

            // Check if there are any other active pay rates for this day
            List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.EFFECTIVE_DATE, UserPayRateDTO.EffectiveDate.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            if(executionContext.IsCorporate)
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            if (UserPayRateDTO.UserRoleId != null && UserPayRateDTO.UserId != null)
            {
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ID, UserPayRateDTO.UserId.ToString()));
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ROLE_ID, UserPayRateDTO.UserRoleId.ToString()));
            }
            else if (UserPayRateDTO.UserRoleId != null && UserPayRateDTO.UserId == null)
            {
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ID_IS_NULL, "1"));
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ROLE_ID, UserPayRateDTO.UserRoleId.ToString()));
            }
            else if (UserPayRateDTO.UserRoleId == null && UserPayRateDTO.UserId != null)
            {
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ID, UserPayRateDTO.UserId.ToString()));
                searchParameters.Add(new KeyValuePair<UserPayRateDTO.SearchByParameters, string>(UserPayRateDTO.SearchByParameters.USER_ROLE_ID_IS_NULL, "1"));
            }

            UserPayRateListBL userPayRateListBL = new UserPayRateListBL(executionContext);
            List<UserPayRateDTO> existingPayRateDTOList = userPayRateListBL.GetUserPayRateDTOList(searchParameters);
            if (existingPayRateDTOList != null && existingPayRateDTOList.Any())
            {
                if (this.UserPayRateDTO.UserPayRateId != -1)
                    existingPayRateDTOList = existingPayRateDTOList.Where(x => x.UserPayRateId != this.UserPayRateDTO.UserPayRateId).ToList();
            }

            if (existingPayRateDTOList != null && existingPayRateDTOList.Any())
                throw new ValidationException(MessageContainerList.GetMessage(executionContext,"An active pay rate exists for this date"));

            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the UserPayRate
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            Validate(sqlTransaction);

            UserPayRateDataHandler userPayRateDataHandler = new UserPayRateDataHandler(sqlTransaction);
            if (userPayRateDTO.UserPayRateId < 0)
            {
                userPayRateDTO = userPayRateDataHandler.Insert(userPayRateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userPayRateDTO.AcceptChanges();
            }
            else
            {
                if (userPayRateDTO.IsChanged)
                {
                    userPayRateDTO = userPayRateDataHandler.Update(userPayRateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    userPayRateDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserPayRateDTO UserPayRateDTO
        {
            get
            {
                return userPayRateDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of UserPayRate
    /// </summary>
    public class UserPayRateListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public UserPayRateListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the UserPayRate list
        /// </summary>
        public List<UserPayRateDTO> GetUserPayRateDTOList(List<KeyValuePair<UserPayRateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            UserPayRateDataHandler userPayRateDataHandler = new UserPayRateDataHandler(sqlTransaction);
            List<UserPayRateDTO> returnValue = userPayRateDataHandler.GetUserPayRateDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the UserPayRate list
        /// </summary>
        public List<UserPayRateDTO> SaveUserPayRateDTOList(List<UserPayRateDTO> userPayRateDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userPayRateDTOList);

            foreach (UserPayRateDTO userPayRateDTO in userPayRateDTOList)
            {
                UserPayRateBL userPayRateBL = new UserPayRateBL(executionContext, userPayRateDTO);
                userPayRateBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
            return userPayRateDTOList;
        }
    }
}


