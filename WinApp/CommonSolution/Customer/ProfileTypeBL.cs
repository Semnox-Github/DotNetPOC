/********************************************************************************************
 * Project Name - ProfileType BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
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
    /// <summary>
    /// Business logic for ProfileType class.
    /// </summary>
    public class ProfileTypeBL
    {
        private ProfileTypeDTO profileTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ProfileTypeBL class
        /// </summary>
        private ProfileTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the profileType id as the parameter
        /// Would fetch the profileType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ProfileTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProfileTypeDataHandler profileTypeDataHandler = new ProfileTypeDataHandler(sqlTransaction);
            profileTypeDTO = profileTypeDataHandler.GetProfileTypeDTO(id);
            if (profileTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProfileType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProfileTypeBL object using the ProfileTypeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="profileTypeDTO">ProfileTypeDTO object</param>
        public ProfileTypeBL(ExecutionContext executionContext, ProfileTypeDTO profileTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, profileTypeDTO);
            this.profileTypeDTO = profileTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProfileType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProfileTypeDataHandler profileTypeDataHandler = new ProfileTypeDataHandler(sqlTransaction);
            if (profileTypeDTO.Id < 0)
            {
                profileTypeDTO = profileTypeDataHandler.InsertProfileType(profileTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                profileTypeDTO.AcceptChanges();
            }
            else
            {
                if (profileTypeDTO.IsChanged)
                {
                    profileTypeDTO = profileTypeDataHandler.UpdateProfileType(profileTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    profileTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProfileTypeDTO ProfileTypeDTO
        {
            get
            {
                return profileTypeDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of ProfileType
    /// </summary>
    public class ProfileTypeListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProfileTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProfileType list
        /// </summary>
        public List<ProfileTypeDTO> GetProfileTypeDTOList(List<KeyValuePair<ProfileTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProfileTypeDataHandler profileTypeDataHandler = new ProfileTypeDataHandler(sqlTransaction);
            List<ProfileTypeDTO> returnValue = profileTypeDataHandler.GetProfileTypeDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
