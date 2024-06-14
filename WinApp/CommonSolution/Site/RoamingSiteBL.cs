/********************************************************************************************
 * Project Name - Site
 * Description  - BL Logic for RoamingSite
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0       21-Dec-2020   Lakshminarayana    Created for POS UI Redesign.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Roaming Site BL
    /// </summary>
    public class RoamingSiteBL
    {
        private RoamingSiteDTO roamingSiteDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RoamingSiteBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RoamingSiteBL object using the roamingSiteDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="roamingSiteDTO">roamingSiteDTO DTO object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RoamingSiteBL(ExecutionContext executionContext, RoamingSiteDTO roamingSiteDTO, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, roamingSiteDTO);
            if(roamingSiteDTO.Id > -1)
            {
                LoadRoamingSiteDTO(roamingSiteDTO.Id, sqlTransaction);
                Update(roamingSiteDTO);
            }
            else
            {
                ValidateSiteName(roamingSiteDTO.SiteName);
                ValidateSiteAddress(roamingSiteDTO.SiteAddress);
                this.roamingSiteDTO = new RoamingSiteDTO(-1, 
                                                         roamingSiteDTO.RoamingSiteId, 
                                                         roamingSiteDTO.SiteName, 
                                                         roamingSiteDTO.SiteAddress, 
                                                         roamingSiteDTO.LastUploadTime, 
                                                         roamingSiteDTO.AutoRoam);
            }
            log.LogMethodExit();
        }

        private void Update(RoamingSiteDTO parameterRoamingSiteDTO)
        {
            log.LogMethodEntry(parameterRoamingSiteDTO);
            ChangeRoamingSiteId(parameterRoamingSiteDTO.RoamingSiteId);
            ChangeSiteName(parameterRoamingSiteDTO.SiteName);
            ChangeSiteAddress(parameterRoamingSiteDTO.SiteAddress);
            ChangeLastUploadTime(parameterRoamingSiteDTO.LastUploadTime);
            ChangeAutoRoam(parameterRoamingSiteDTO.AutoRoam);
            log.LogMethodExit();
        }

        private void ChangeLastUploadTime(DateTime? lastUploadTime)
        {
            log.LogMethodEntry(lastUploadTime);
            if (roamingSiteDTO.LastUploadTime == lastUploadTime)
            {
                log.LogMethodExit(null, "No changes to last upload time");
                return;
            }
            roamingSiteDTO.LastUploadTime = lastUploadTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change site address
        /// </summary>
        /// <param name="siteAddress"></param>
        public void ChangeSiteAddress(string siteAddress)
        {
            log.LogMethodEntry(siteAddress);
            if (roamingSiteDTO.SiteAddress == siteAddress)
            {
                log.LogMethodExit(null, "No changes to site address");
                return;
            }
            ValidateSiteAddress(siteAddress);
            roamingSiteDTO.SiteAddress = siteAddress;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change site name
        /// </summary>
        /// <param name="siteName"></param>
        private void ChangeSiteName(string siteName)
        {
            log.LogMethodEntry(siteName);
            if (roamingSiteDTO.SiteName == siteName)
            {
                log.LogMethodExit(null, "No changes to site name");
                return;
            }
            ValidateSiteAddress(siteName);
            roamingSiteDTO.SiteName = siteName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Change auto roam
        /// </summary>
        /// <param name="autoRoam"></param>
        private void ChangeAutoRoam(bool autoRoam)
        {
            log.LogMethodEntry(autoRoam);
            if (roamingSiteDTO.AutoRoam == autoRoam)
            {
                log.LogMethodExit(null, "No changes to auto roam");
                return;
            }
            roamingSiteDTO.AutoRoam = autoRoam;
            log.LogMethodExit();
        }

        /// <summary>
        /// change roaming site id
        /// </summary>
        /// <param name="roamingSiteId"></param>
        public void ChangeRoamingSiteId(int roamingSiteId)
        {
            log.LogMethodEntry(roamingSiteId);
            if (roamingSiteDTO.RoamingSiteId == roamingSiteId)
            {
                log.LogMethodExit(null, "No changes to roaming site");
                return;
            }
            roamingSiteDTO.RoamingSiteId = roamingSiteId;
            log.LogMethodExit();
        }

        private void ValidateSiteName(string siteName)
        {
            log.LogMethodEntry(siteName);
            if (string.IsNullOrWhiteSpace(siteName))
            {
                string errorMessage = "Please enter valid value for siteName"; 
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "RoamingSite", "SiteName", errorMessage);
            }
            if (siteName.Length > 100)
            {
                string errorMessage = "Length of siteName should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "RoamingSite", "SiteName", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateSiteAddress(string siteAddress)
        {
            log.LogMethodEntry(siteAddress);
            if (siteAddress.Length > 1000)
            {
                string errorMessage = "Length of siteAddress should not exceed 1000 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "RoamingSite", "SiteAddress", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RoamingSite  id as the parameter
        /// Would fetch the RoamingSite object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RoamingSite </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RoamingSiteBL(ExecutionContext executionContext, int id,
                            SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoadRoamingSiteDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        private void LoadRoamingSiteDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            RoamingSiteDataHandler roamingSiteDataHandler = new RoamingSiteDataHandler(sqlTransaction);
            roamingSiteDTO = roamingSiteDataHandler.GetRoamingSiteDTO(id);
            if (roamingSiteDTO == null)
            {
                string message = "Unable to find a Roaming Site with id: " + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RoamingSite DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (roamingSiteDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RoamingSiteDataHandler roamingSiteDataHandler = new RoamingSiteDataHandler(sqlTransaction);
            if (roamingSiteDTO.RoamingSiteId < 0)
            {
                roamingSiteDTO = roamingSiteDataHandler.Insert(roamingSiteDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                roamingSiteDTO.AcceptChanges();
            }
            else if (roamingSiteDTO.IsChanged)
            {
                roamingSiteDTO = roamingSiteDataHandler.Update(roamingSiteDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                roamingSiteDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the RoamingSiteDTO
        /// </summary>
        public RoamingSiteDTO RoamingSiteDTO
        {
            get
            {
                return new RoamingSiteDTO(roamingSiteDTO);
            }
        }
    }

    /// <summary>
    /// Manages the list of RoamingSite
    /// </summary>
    public class RoamingSiteListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RoamingSiteDTO> roamingSiteDTOList = new List<RoamingSiteDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RoamingSiteListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="roamingSiteDTOList">RoamingSite DTO List as parameter </param>
        public RoamingSiteListBL(ExecutionContext executionContext,
                                               List<RoamingSiteDTO> roamingSiteDTOList)
            : this()
        {
            log.LogMethodEntry(executionContext, roamingSiteDTOList);
            this.executionContext = executionContext;
            this.roamingSiteDTOList = roamingSiteDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the roamingSiteDTO list
        /// </summary>
        public List<RoamingSiteDTO> GetAllRoamingSiteDTOList(List<KeyValuePair<RoamingSiteDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RoamingSiteDataHandler roamingSiteDataHandler = new RoamingSiteDataHandler(sqlTransaction);
            List<RoamingSiteDTO> roamingSiteDTOList = roamingSiteDataHandler.GetRoamingSiteDTOList(searchParameters);
            log.LogMethodExit(roamingSiteDTOList);
            return roamingSiteDTOList;
        }

        /// <summary>
        /// Saves the  list of RoamingSite DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (roamingSiteDTOList == null ||
                roamingSiteDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < roamingSiteDTOList.Count; i++)
            {
                RoamingSiteDTO roamingSiteDTO = roamingSiteDTOList[i];
                if (roamingSiteDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RoamingSiteBL roamingSiteBL = new RoamingSiteBL(executionContext, roamingSiteDTO);
                    roamingSiteBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException("You cannot insert the duplicate record");
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException("Unable to delete this record. Please check the reference record first.");
                    }
                    else
                    {
                        throw new ValidationException(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving RoamingSiteDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RoamingSiteDTO", roamingSiteDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetRoamingSiteModuleLastUpdateTime()
        {
            log.LogMethodEntry();
            RoamingSiteDataHandler roamingSiteDataHandler = new RoamingSiteDataHandler();
            DateTime? result = roamingSiteDataHandler.GetRoamingSiteModuleLastUpdateTime();
            log.LogMethodExit(result);
            return result;
        }

    }
}
