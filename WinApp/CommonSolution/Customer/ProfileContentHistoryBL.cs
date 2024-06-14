/********************************************************************************************
 * Project Name - ProfileContentHistory BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2      19-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private   
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    class ProfileContentHistoryBL
    {
        private ProfileContentHistoryDTO profileContentHistoryDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerContentHistoryBL class
        /// </summary>
        private ProfileContentHistoryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CustomerContentHistory id as the parameter
        /// Would fetch the CustomerContentHistory object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ProfileContentHistoryBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProfileContentHistoryDataHandler customerContentHistoryDataHandler = new ProfileContentHistoryDataHandler();
            profileContentHistoryDTO = customerContentHistoryDataHandler.GetProfileContentHistory(id);
            if (profileContentHistoryDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProfileContentHistory", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerContentHistoryBL object using the CustomerContentHistoryDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="CustomerContentHistoryDTO">CustomerContentHistoryDTO object</param>
        public ProfileContentHistoryBL(ExecutionContext executionContext, ProfileContentHistoryDTO profileContentHistoryDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, profileContentHistoryDTO);
            this.profileContentHistoryDTO = profileContentHistoryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomerContentHistory
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
		{
            log.LogMethodEntry(sqlTransaction);
            ProfileContentHistoryDataHandler profileContentHistoryDataHandler = new ProfileContentHistoryDataHandler(sqlTransaction);
            if (profileContentHistoryDTO.Id < 0)
            {
                profileContentHistoryDTO = profileContentHistoryDataHandler.InsertProfileContentHistory(profileContentHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
				profileContentHistoryDTO.AcceptChanges();
            }
            else
            {
                if (profileContentHistoryDTO.IsChanged)
                {
                    profileContentHistoryDTO = profileContentHistoryDataHandler.UpdateProfileContentHistory(profileContentHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
					profileContentHistoryDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProfileContentHistoryDTO ProfileContentHistoryDTO
		{
            get
            {
                return profileContentHistoryDTO;
            }
        }

        
    }
    /// <summary>
    /// Manages the list of content history
    /// </summary>
    public class ProfileContentHistoryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the content history list
        /// </summary>
        public List<ProfileContentHistoryDTO> GetAllProfileContentHistory(List<KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>> searchParameters, SqlTransaction  sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProfileContentHistoryDataHandler profileContentHistoryDataHandler = new ProfileContentHistoryDataHandler(sqlTransaction);
            List<ProfileContentHistoryDTO> profileContentHistoryDTOList = profileContentHistoryDataHandler.GetProfileContentHistoryDTOList(searchParameters);
            log.LogMethodExit(profileContentHistoryDTOList);
            return profileContentHistoryDTOList;
        }

        /// <summary>
        /// Returns the content history list
        /// </summary>
        public List<ProfileContentHistoryDTO> GetAllProfileContentHistory(List<int> profileIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(profileIdList, activeChildRecords, sqlTransaction);
            ProfileContentHistoryDataHandler profileContentHistoryDataHandler = new ProfileContentHistoryDataHandler(sqlTransaction);
            List<ProfileContentHistoryDTO> profileContentHistoryDTOList = profileContentHistoryDataHandler.GetProfileContentHistoryDTOList(profileIdList, activeChildRecords);
            log.LogMethodExit(profileContentHistoryDTOList);
            return profileContentHistoryDTOList;
        }

    }
}
