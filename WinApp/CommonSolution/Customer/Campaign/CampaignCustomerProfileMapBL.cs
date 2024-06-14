/********************************************************************************************
 * Project Name - CampaignCustomerProfileMap BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     22-Jan-2021      Prajwal S     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Campaign
{
    public class CampaignCustomerProfileMapBL
    {
        private CampaignCustomerProfileMapDTO campaignCustomerProfileMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignCustomerProfileMapBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private CampaignCustomerProfileMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.campaignCustomerProfileMapDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignCustomerProfileMapBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CampaignCustomerProfileMapDataHandler campaignCustomerProfileMapDataHandler = new CampaignCustomerProfileMapDataHandler(sqlTransaction);
            campaignCustomerProfileMapDTO = campaignCustomerProfileMapDataHandler.GetCampaignCustomerProfileMap(id);
            if (campaignCustomerProfileMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomerProfileMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates CampaignCustomerProfileMapBL object using the CampaignCustomerProfileMapDTO
        /// </summary>
        /// <param name="CampaignCustomerProfileMapDTO">CampaignCustomerProfileMapDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public CampaignCustomerProfileMapBL(ExecutionContext executionContext, CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, CampaignCustomerProfileMapDTO);
            if (CampaignCustomerProfileMapDTO.CampaignCustomerProfileMapId < 0)
            {
                ValidateCampaignCustomerProfileId(CampaignCustomerProfileMapDTO.CampaignCustomerProfileId);

            }
            this.campaignCustomerProfileMapDTO = CampaignCustomerProfileMapDTO;
            log.LogMethodExit();
        }

        public void Update(CampaignCustomerProfileMapDTO parameterCampaignCustomerProfileMapDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCampaignCustomerProfileMapDTO, sqlTransaction);
            ChangeCampaignDefinitionId(parameterCampaignCustomerProfileMapDTO.CampaignDefinitionId);
            ChangeCampaignCustomerProfileId(parameterCampaignCustomerProfileMapDTO.CampaignCustomerProfileId);
            ChangeIsActive(parameterCampaignCustomerProfileMapDTO.IsActive);

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignCustomerProfileMapDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignCustomerProfileMapDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeCampaignCustomerProfileId(int campaignCustomerProfileId)
        {
            log.LogMethodEntry(campaignCustomerProfileId);
            if (campaignCustomerProfileMapDTO.CampaignCustomerProfileId == campaignCustomerProfileId)
            {
                log.LogMethodExit(null, "No changes to user CampaignCustomerProfileId");
                return;
            }
            campaignCustomerProfileMapDTO.CampaignCustomerProfileId = campaignCustomerProfileId;
            ValidateCampaignCustomerProfileId(campaignCustomerProfileId);
            log.LogMethodExit();
        }


        public void ChangeCampaignDefinitionId(int campaignDefinitionId)
        {
            log.LogMethodEntry(campaignDefinitionId);
            if (campaignCustomerProfileMapDTO.CampaignDefinitionId == campaignDefinitionId)
            {
                log.LogMethodExit(null, "No changes to user posTypeId");
                return;
            }
            campaignCustomerProfileMapDTO.CampaignDefinitionId = campaignDefinitionId;
            log.LogMethodExit();
        }

        private void ValidateCampaignCustomerProfileId(int campaignCustomerProfileId)
        {
            log.LogMethodEntry(campaignCustomerProfileId);
            if (campaignCustomerProfileId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1773, MessageContainerList.GetMessage(executionContext, "CampaignCustomerProfileId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("CampaignCustomerProfile Id cannot be null", "CampaignCustomerProfileMap", "CampaignCustomerProfile", errorMessage);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Saves the CampaignCustomerProfileMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CampaignCustomerProfileMapDataHandler campaignCustomerProfileMapDataHandler = new CampaignCustomerProfileMapDataHandler(sqlTransaction);
            if (campaignCustomerProfileMapDTO.CampaignCustomerProfileMapId < 0)
            {
                campaignCustomerProfileMapDTO = campaignCustomerProfileMapDataHandler.Insert(campaignCustomerProfileMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                campaignCustomerProfileMapDTO.AcceptChanges();
            }
            else
            {
                if (campaignCustomerProfileMapDTO.IsChanged)
                {
                    campaignCustomerProfileMapDTO = campaignCustomerProfileMapDataHandler.Update(campaignCustomerProfileMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignCustomerProfileMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDTO
        {
            get
            {
                return campaignCustomerProfileMapDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignCustomerProfileMap
    /// </summary>
    public class CampaignCustomerProfileMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignCustomerProfileMapListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of CampaignCustomerProfileMapListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CampaignCustomerProfileMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the CampaignCustomerProfileMap list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CampaignCustomerProfileMap list</returns>
        public List<CampaignCustomerProfileMapDTO> GetCampaignCustomerProfileMapDTOList(List<KeyValuePair<CampaignCustomerProfileMapDTO.SearchByCampaignCustomerProfileMapParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignCustomerProfileMapDataHandler campaignCustomerProfileMapDataHandler = new CampaignCustomerProfileMapDataHandler(sqlTransaction);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapsList = campaignCustomerProfileMapDataHandler.GetCampaignCustomerProfileMapList(searchParameters, sqlTransaction);
            log.LogMethodExit(campaignCustomerProfileMapsList);
            return campaignCustomerProfileMapsList;
        }

        /// <summary>
        /// This method should be used to Save and Update the CampaignCustomerProfileMap.
        /// </summary>
        public List<CampaignCustomerProfileMapDTO> Save(List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CampaignCustomerProfileMapDTO> savedCampaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
            foreach (CampaignCustomerProfileMapDTO CampaignCustomerProfileMapDTO in campaignCustomerProfileMapDTOList)
            {
                CampaignCustomerProfileMapBL CampaignCustomerProfileMapBL = new CampaignCustomerProfileMapBL(executionContext, CampaignCustomerProfileMapDTO);
                CampaignCustomerProfileMapBL.Save(sqlTransaction);
                savedCampaignCustomerProfileMapDTOList.Add(CampaignCustomerProfileMapBL.CampaignCustomerProfileMapDTO);
            }
            log.LogMethodExit(savedCampaignCustomerProfileMapDTOList);
            return savedCampaignCustomerProfileMapDTOList;
        }

        /// <summary>
        /// Gets the CampaignCommuncationDefinitionDTO List for CampaignDefinitionList
        /// </summary>
        /// <param name="campaignDefinitionList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CampaignCommuncationDefinitionDTO</returns>
        public List<CampaignCustomerProfileMapDTO> GetCampaignCustomerProfileMapDTOList(List<int> campaignDefinitionList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignDefinitionList, activeRecords, sqlTransaction);
            CampaignCustomerProfileMapDataHandler campaignCustomerProfileMapDataHandler = new CampaignCustomerProfileMapDataHandler(sqlTransaction);
            List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList = campaignCustomerProfileMapDataHandler.GetCampaignCustomerProfileMapDTOList(campaignDefinitionList, activeRecords);
            log.LogMethodExit(campaignCustomerProfileMapDTOList);
            return campaignCustomerProfileMapDTOList;
        }
    }
}
