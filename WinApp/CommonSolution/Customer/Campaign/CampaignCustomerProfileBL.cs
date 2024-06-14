/********************************************************************************************
 * Project Name - CampaignCustomerProfile BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     27-Jan-2021      Prajwal S     Created 
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
    public class CampaignCustomerProfileBL
    { 
    CampaignCustomerProfileDTO campaignCustomerProfileDTO;
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CampaignCustomerProfileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the CampaignCustomerProfileId parameter
        /// </summary>
        /// <param name="CampaignCustomerProfileId">CampaignCustomerProfileId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public CampaignCustomerProfileBL(ExecutionContext executionContext, int CampaignCustomerProfileId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(CampaignCustomerProfileId, loadChildRecords, activeChildRecords);
            LoadCampaignCustomerProfile(CampaignCustomerProfileId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignCustomerProfile id as the parameter
        /// Would fetch the CampaignCustomerProfile object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadCampaignCustomerProfile(int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            CampaignCustomerProfileDataHandler campaignCustomerProfileDataHandler = new CampaignCustomerProfileDataHandler(sqlTransaction);
            campaignCustomerProfileDTO = campaignCustomerProfileDataHandler.GetCampaignCustomerProfile(id);
            ThrowIfCampaignCustomerProfileIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(CampaignCustomerProfileDTO);
        }

        private void ThrowIfCampaignCustomerProfileIsNull(int CampaignCustomerProfileId)
        {
            log.LogMethodEntry();
            if (campaignCustomerProfileDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomerProfile", CampaignCustomerProfileId);
                log.LogMethodExit(null, "Throwing Exception - "+ message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parametersCampaignCustomerProfileDTO">sCampaignCustomerProfileDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignCustomerProfileBL(ExecutionContext executionContext, CampaignCustomerProfileDTO parametersCampaignCustomerProfileDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parametersCampaignCustomerProfileDTO, sqlTransaction);

            if (parametersCampaignCustomerProfileDTO.CampaignCustomerProfileId > -1)
            {
                LoadCampaignCustomerProfile(parametersCampaignCustomerProfileDTO.CampaignCustomerProfileId, true, true, sqlTransaction);//added sql
                ThrowIfCampaignCustomerProfileIsNull(parametersCampaignCustomerProfileDTO.CampaignCustomerProfileId);
                Update(parametersCampaignCustomerProfileDTO, sqlTransaction);
            }
            else
            {
                ValidateName(parametersCampaignCustomerProfileDTO.Name);
                ValidateDescription(parametersCampaignCustomerProfileDTO.Description);
                ValidateType(parametersCampaignCustomerProfileDTO.Type);
                campaignCustomerProfileDTO = new CampaignCustomerProfileDTO(-1, parametersCampaignCustomerProfileDTO.Name, parametersCampaignCustomerProfileDTO.Description, parametersCampaignCustomerProfileDTO.Type, parametersCampaignCustomerProfileDTO.Query, parametersCampaignCustomerProfileDTO.IsActive);

                if (parametersCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList != null && parametersCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Any())
                {
                    campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                    foreach (CampaignCustomerProfileDetailDTO parameterCampaignCustomerProfileDetailDTO in parametersCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList)
                    {
                        if (parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomerProfileDetail", parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var campaignCustomerProfileDetailDTO = new CampaignCustomerProfileDetailDTO(-1, -1, parameterCampaignCustomerProfileDetailDTO.CustomerId, parameterCampaignCustomerProfileDetailDTO.AccountId, parameterCampaignCustomerProfileDetailDTO.ContactId, parameterCampaignCustomerProfileDetailDTO.IsActive);
                        CampaignCustomerProfileDetailBL campaignCustomerProfileDetailBL = new CampaignCustomerProfileDetailBL(executionContext, campaignCustomerProfileDetailDTO);
                        campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Add(campaignCustomerProfileDetailBL.CampaignCustomerProfileDetailDTO);
                    }

                }

            }
            log.LogMethodExit();
        }


        private void Update(CampaignCustomerProfileDTO parameterCampaignCustomerProfileDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCampaignCustomerProfileDTO, sqlTransaction);
            ChangeName(parameterCampaignCustomerProfileDTO.Name);
            ChangeDescription(parameterCampaignCustomerProfileDTO.Description);
            ChangeQuery(parameterCampaignCustomerProfileDTO.Query);
            ChangeType(parameterCampaignCustomerProfileDTO.Type);
            ChangeIsActive(parameterCampaignCustomerProfileDTO.IsActive);


            Dictionary<int, CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTODictionary = new Dictionary<int, CampaignCustomerProfileDetailDTO>();
            if (campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList != null &&
                campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Any())
            {
                foreach (var campaignCustomerProfileDetailDTO in campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList)
                {
                    campaignCustomerProfileDetailDTODictionary.Add(campaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId, campaignCustomerProfileDetailDTO);
                }
            }
            if (parameterCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList != null &&
                parameterCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Any())
            {
                foreach (var parameterCampaignCustomerProfileDetailDTO in parameterCampaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList)
                {
                    if (campaignCustomerProfileDetailDTODictionary.ContainsKey(parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId))
                    {
                        CampaignCustomerProfileDetailBL campaignCustomerProfileDetail = new CampaignCustomerProfileDetailBL(executionContext, campaignCustomerProfileDetailDTODictionary[parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId]);
                        campaignCustomerProfileDetail.Update(parameterCampaignCustomerProfileDetailDTO, sqlTransaction);
                    }
                    else if (parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId > -1)
                    {
                        CampaignCustomerProfileDetailBL campaignCustomerProfileDetail = new CampaignCustomerProfileDetailBL(executionContext, parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId, sqlTransaction);
                        if (campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList == null)
                        {
                            campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                        }
                        campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Add(campaignCustomerProfileDetail.CampaignCustomerProfileDetailDTO);
                        campaignCustomerProfileDetail.Update(parameterCampaignCustomerProfileDetailDTO, sqlTransaction);
                    }
                    else
                    {
                        CampaignCustomerProfileDetailBL campaignCustomerProfileDetailBL = new CampaignCustomerProfileDetailBL(executionContext, parameterCampaignCustomerProfileDetailDTO);
                        if (campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList == null)
                        {
                            campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                        }
                        campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Add(campaignCustomerProfileDetailBL.CampaignCustomerProfileDetailDTO);
                    }
                }
            }

        }

        /// <summary>
        /// Builds the child records for CampaignDefinition object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CampaignCustomerProfileDetailListBL campaignCustomerProfileDetailListBL = new CampaignCustomerProfileDetailListBL(executionContext);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList = campaignCustomerProfileDetailListBL.GetCampaignCustomerProfileDetailDTOList(new List<int>() { campaignCustomerProfileDTO.CampaignCustomerProfileId }, activeChildRecords, sqlTransaction);
            if (campaignCustomerProfileDetailDTOList.Count != 0 && campaignCustomerProfileDetailDTOList.Any())
            {
                campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList = campaignCustomerProfileDetailDTOList;
            }
            log.LogMethodExit();
        }
        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignCustomerProfileDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to campaignCustomerProfile IsActive");
                return;
            }
            campaignCustomerProfileDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeName(string name)
        {
            log.LogMethodEntry(name);
            if (campaignCustomerProfileDTO.Name == name)
            {
                log.LogMethodExit(null, "No changes to campaignCustomerProfile Name");
                return;
            }
            campaignCustomerProfileDTO.Name = name;
            log.LogMethodExit();
        }

        public void ChangeDescription(string description)
        {
            log.LogMethodEntry(description);
            if (campaignCustomerProfileDTO.Description == description)
            {
                log.LogMethodExit(null, "No changes to campaignCustomerProfile Description");
                return;
            }
            campaignCustomerProfileDTO.Description = description;
            log.LogMethodExit();
        }

        public void ChangeQuery(string query)
        {
            log.LogMethodEntry(query);
            if (campaignCustomerProfileDTO.Query == query)
            {
                log.LogMethodExit(null, "No changes to campaignCustomerProfile Query");
                return;
            }
            campaignCustomerProfileDTO.Query = query;
            log.LogMethodExit();
        }
        public void ChangeType(string type)
        {
            log.LogMethodEntry(type);
            if (campaignCustomerProfileDTO.Type == type)
            {
                log.LogMethodExit(null, "No changes to campaignCustomerProfile Type");
                return;
            }
            campaignCustomerProfileDTO.Type = type;
            log.LogMethodExit();
        }

        private void ValidateName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1132);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name is empty.", "campaignDefinition", "name", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "User Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "campaignDefinition", "name", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateType(string type)
        {
            log.LogMethodEntry(type);
            if (string.IsNullOrWhiteSpace(type))
            {
                log.LogMethodExit(null, "type empty");
                return;
            }
            if (type.Length > 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Type"), 1);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "campaignDefinition", "name", errorMessage);
            }
            log.LogMethodExit();
        }


        private void ValidateDescription(string description)
        {
            log.LogMethodEntry(description);
            if (string.IsNullOrWhiteSpace(description))
            {
                log.LogMethodExit(null, "description empty");
                return;
            }
            if (description.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Description"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("description greater than 100 characters.", "campaignDefinition", "description", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the campaignDefinition
        /// Checks if the User id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            CampaignCustomerProfileDataHandler campaignCustomerProfileDataHandler = new CampaignCustomerProfileDataHandler(sqlTransaction);
            if (campaignCustomerProfileDTO.CampaignCustomerProfileId < 0)
            {
                campaignCustomerProfileDTO = campaignCustomerProfileDataHandler.Insert(campaignCustomerProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                campaignCustomerProfileDTO.AcceptChanges();
            }
            else
            {
                if (campaignCustomerProfileDTO.IsChanged)
                {
                    campaignCustomerProfileDTO = campaignCustomerProfileDataHandler.Update(campaignCustomerProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignCustomerProfileDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }
            // Will Save the Child CampaignCustomerProfileDetailDTO
            log.Debug("campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTO Value :" + campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList);
            if (campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList != null && campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Any())
            {
                List<CampaignCustomerProfileDetailDTO> updatedCampaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                foreach (CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO in campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList)
                {
                    if (campaignCustomerProfileDetailDTO.CampaignCustomerProfileId != campaignCustomerProfileDTO.CampaignCustomerProfileId)
                    {
                        campaignCustomerProfileDetailDTO.CampaignCustomerProfileId = campaignCustomerProfileDTO.CampaignCustomerProfileId;
                    }
                    log.Debug("CampaignCustomerProfileDetailDTO.IsChanged Value :" + campaignCustomerProfileDetailDTO.IsChanged);
                    if (campaignCustomerProfileDetailDTO.IsChanged)
                    {
                        updatedCampaignCustomerProfileDetailDTOList.Add(campaignCustomerProfileDetailDTO);
                    }
                }
                log.Debug("updatedCampaignCustomerProfileDetailDTO Value :" + updatedCampaignCustomerProfileDetailDTOList);
                if (updatedCampaignCustomerProfileDetailDTOList.Any())
                {
                    CampaignCustomerProfileDetailListBL campaignCustomerProfileDetailListBL = new CampaignCustomerProfileDetailListBL(executionContext);
                    campaignCustomerProfileDetailListBL.Save(updatedCampaignCustomerProfileDetailDTOList, sqlTransaction);
                }
            }
            log.LogMethodExit();
    }


    /// <summary>
    /// Gets the DTO
    /// </summary>
    public CampaignCustomerProfileDTO CampaignCustomerProfileDTO
    {

            get
            {
                CampaignCustomerProfileDTO result = new CampaignCustomerProfileDTO(campaignCustomerProfileDTO);
                return result;
            }
        }
}

/// <summary>
/// Manages the list of CampaignCustomerProfile
/// </summary>
/// 

public class CampaignCustomerProfileListBL
{
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;
    

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignCustomerProfileListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        public CampaignCustomerProfileListBL(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        log.LogMethodExit();
    }
        
        /// <summary>
        /// Returns the CampaignDefinition list
        /// </summary>
        public List<CampaignCustomerProfileDTO> GetCampaignCustomerProfileDTOList(List<KeyValuePair<CampaignCustomerProfileDTO.SearchByCampaignCustomerProfileParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            CampaignCustomerProfileDataHandler campaignCustomerProfileDataHandler = new CampaignCustomerProfileDataHandler(sqlTransaction);
            List<CampaignCustomerProfileDTO> campaignCustomerProfileDTOsList = campaignCustomerProfileDataHandler.GetCampaignCustomerProfileList(searchParameters, sqlTransaction);
            if (campaignCustomerProfileDTOsList != null && campaignCustomerProfileDTOsList.Any() && loadChildRecords)
            {
                Build(campaignCustomerProfileDTOsList, loadActiveChildRecords, sqlTransaction);

            }
            log.LogMethodExit(campaignCustomerProfileDTOsList);
            return campaignCustomerProfileDTOsList;
        }


        private void Build(List<CampaignCustomerProfileDTO> campaignCustomerProfileDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignCustomerProfileDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CampaignCustomerProfileDTO> campaignCustomerProfileDTOIdMap = new Dictionary<int, CampaignCustomerProfileDTO>();
            List<int> campaignCustomerProfileIdList = new List<int>();
            for (int i = 0; i < campaignCustomerProfileDTOList.Count; i++)
            {
                if (campaignCustomerProfileDTOIdMap.ContainsKey(campaignCustomerProfileDTOList[i].CampaignCustomerProfileId))
                {
                    continue;
                }
                campaignCustomerProfileDTOIdMap.Add(campaignCustomerProfileDTOList[i].CampaignCustomerProfileId, campaignCustomerProfileDTOList[i]);
                campaignCustomerProfileIdList.Add(campaignCustomerProfileDTOList[i].CampaignCustomerProfileId);
            }


            CampaignCustomerProfileDetailListBL campaignCustomerProfileDetailListBL = new CampaignCustomerProfileDetailListBL(executionContext);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList = campaignCustomerProfileDetailListBL.GetCampaignCustomerProfileDetailDTOList(campaignCustomerProfileIdList, activeChildRecords, sqlTransaction);
            if (campaignCustomerProfileDetailDTOList != null && campaignCustomerProfileDetailDTOList.Any())
            {
                for (int i = 0; i < campaignCustomerProfileDetailDTOList.Count; i++)
                {
                    if (campaignCustomerProfileDTOIdMap.ContainsKey(campaignCustomerProfileDetailDTOList[i].CampaignCustomerProfileId) == false)
                    {
                        continue;
                    }
                    CampaignCustomerProfileDTO campaignCustomerProfileDTO = campaignCustomerProfileDTOIdMap[campaignCustomerProfileDetailDTOList[i].CampaignCustomerProfileId];
                    if (campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList == null)
                    {
                        campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                    }
                    campaignCustomerProfileDTO.CampaignCustomerProfileDetailDTOList.Add(campaignCustomerProfileDetailDTOList[i]);
                }
            }
        }

            /// <summary>
            /// This method should be used to Save and Update the CampaignCustomerProfile.
            /// </summary>
            public List<CampaignCustomerProfileDTO> Save(List<CampaignCustomerProfileDTO> campaignCustomerProfileDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CampaignCustomerProfileDTO> savedCampaignCustomerProfileDTOList = new List<CampaignCustomerProfileDTO>();
            if (campaignCustomerProfileDTOList == null || campaignCustomerProfileDTOList.Any() == false)
            {
                log.LogMethodExit(savedCampaignCustomerProfileDTOList);
                return savedCampaignCustomerProfileDTOList;
            }
            foreach (CampaignCustomerProfileDTO campaignCustomerProfileDTO in campaignCustomerProfileDTOList)
            {
                CampaignCustomerProfileBL campaignCustomerProfileBL = new CampaignCustomerProfileBL(executionContext, campaignCustomerProfileDTO, sqlTransaction);
                campaignCustomerProfileBL.Save(sqlTransaction);
                savedCampaignCustomerProfileDTOList.Add(campaignCustomerProfileBL.CampaignCustomerProfileDTO);
            }
            log.LogMethodExit(savedCampaignCustomerProfileDTOList);
            return savedCampaignCustomerProfileDTOList;
        }
    }
}