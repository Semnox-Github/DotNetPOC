/********************************************************************************************
 * Project Name - Promotions
 * Description  - Business logic file for  Promotion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      24-Jun-2019     Girish Kundar           Created 
 *2.70.0    18-Jul-2019     Mushahid Faizan         Modified Save() method in PromotionListBL class.
 * 2.80     04-Dec-2019     Rakesh                  Modified Build() method in PromotionListBL class.
 * 2.80     08-Apr-2020     Mushahid Faizan         Modified : 3 tier changes for Rest API.
                                                               Build method , used PromotionIdList as search parameter.
 *2.130.8   14-Apr-2022      Abhishek               Modified : Added GameAttributes methods as a part of Promotion Game Attribute Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// Business logic for Promotion class.
    /// </summary>
    public class PromotionBL
    {
        private PromotionDTO promotionDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PromotionBL class
        /// </summary>
        private PromotionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PromotionBL object using the PromotionDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="promotionDTO">PromotionDTO object</param>
        public PromotionBL(ExecutionContext executionContext, PromotionDTO promotionDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionDTO);
            this.promotionDTO = promotionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Promotion id as the parameter
        /// Would fetch the Promotion object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - Promotion</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PromotionBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
            promotionDTO = promotionDataHandler.GetPromotionDTO(id);
            if (promotionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PromotionDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for PromotionDTO object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            PromotionRuleListBL promotionRuleListBL = new PromotionRuleListBL(executionContext);
            List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.PROMOTION_ID, promotionDTO.PromotionId.ToString()));
            searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            promotionDTO.PromotionRuleDTOList = promotionRuleListBL.GetPromotionRuleDTOList(searchParameters, sqlTransaction);
            // Build second child
            PromotionDetailListBL promotionDetailListBL = new PromotionDetailListBL(executionContext);
            List<KeyValuePair<PromotionDetailDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<PromotionDetailDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<PromotionDetailDTO.SearchByParameters, string>(PromotionDetailDTO.SearchByParameters.PROMOTION_ID, promotionDTO.PromotionId.ToString()));
            psearchParameters.Add(new KeyValuePair<PromotionDetailDTO.SearchByParameters, string>(PromotionDetailDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<PromotionDetailDTO.SearchByParameters, string>(PromotionDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            promotionDTO.PromotionDetailDTOList = promotionDetailListBL.GetPromotionDetailDTOList(psearchParameters, sqlTransaction);

            // Build Third child
            PromotionExclusionDateListBL promotionExclusionDateListBL = new PromotionExclusionDateListBL(executionContext);
            List<KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>> exclusionSearchParameters = new List<KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>>();
            exclusionSearchParameters.Add(new KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>(PromotionExclusionDateDTO.SearchByParameters.PROMOTION_ID, promotionDTO.PromotionId.ToString()));
            exclusionSearchParameters.Add(new KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>(PromotionExclusionDateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<PromotionDetailDTO.SearchByParameters, string>(PromotionDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            promotionDTO.PromotionExclusionDateDTOLists = promotionExclusionDateListBL.GetPromotionExclusionDateDTOList(exclusionSearchParameters, sqlTransaction);

            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the promotionDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (promotionDTO.IsChangedRecursive == false
                   && promotionDTO.PromotionId > -1)
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
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
            SetToSiteTimeOffset();
            if (promotionDTO.PromotionId < 0)
            {
                log.LogVariableState("PromotionDTO", promotionDTO);
                promotionDTO = promotionDataHandler.Insert(promotionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                promotionDTO.AcceptChanges();
            }
            else if (promotionDTO.IsChanged)
            {
                log.LogVariableState("PromotionDTO", promotionDTO);
                promotionDTO = promotionDataHandler.Update(promotionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                promotionDTO.AcceptChanges();
            }
            SavePromotionChild(sqlTransaction);
            SetFromSiteTimeOffset();
        }

        /// <summary>
        /// Saves the child records : PromotionDetailDTO List and PromotionRule DTO List
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SavePromotionChild(SqlTransaction sqlTransaction)
        {
            if (promotionDTO.PromotionDetailDTOList != null &&
                promotionDTO.PromotionDetailDTOList.Any())
            {
                List<PromotionDetailDTO> updatedPromotionDetailDTOList = new List<PromotionDetailDTO>();
                foreach (var promotionDetailDTO in promotionDTO.PromotionDetailDTOList)
                {
                    if (promotionDetailDTO.PromotionId != promotionDTO.PromotionId)
                    {
                        promotionDetailDTO.PromotionId = promotionDTO.PromotionId;
                    }
                    if (promotionDetailDTO.IsChanged)
                    {
                        updatedPromotionDetailDTOList.Add(promotionDetailDTO);
                    }
                }
                if (updatedPromotionDetailDTOList.Any())
                {
                    log.LogVariableState("UpdatedPromotionDetailDTOList", updatedPromotionDetailDTOList);
                    PromotionDetailListBL promotionDetailListBL = new PromotionDetailListBL(executionContext, updatedPromotionDetailDTOList);
                    promotionDetailListBL.Save(sqlTransaction);
                }
            }

            // For child records :PromotionRule DTO

            if (promotionDTO.PromotionRuleDTOList != null &&
                promotionDTO.PromotionRuleDTOList.Any())
            {
                List<PromotionRuleDTO> updatedPromotionRuleDTOList = new List<PromotionRuleDTO>();
                foreach (var promotionRuleDTO in promotionDTO.PromotionRuleDTOList)
                {
                    if (promotionRuleDTO.PromotionId != promotionDTO.PromotionId)
                    {
                        promotionRuleDTO.PromotionId = promotionDTO.PromotionId;
                    }
                    if (promotionRuleDTO.IsChanged)
                    {
                        updatedPromotionRuleDTOList.Add(promotionRuleDTO);
                    }
                }
                if (updatedPromotionRuleDTOList.Any())
                {
                    PromotionRuleListBL promotionRuleListBL = new PromotionRuleListBL(executionContext, updatedPromotionRuleDTOList);
                    promotionRuleListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug("promotionDTO.PromotionRuleDTOList");
            }

            if (promotionDTO.PromotionExclusionDateDTOLists != null &&
                promotionDTO.PromotionExclusionDateDTOLists.Any())
            {
                List<PromotionExclusionDateDTO> updatedPromotionExclusionDateDTOList = new List<PromotionExclusionDateDTO>();
                foreach (var promotionExclusionDateDTO in promotionDTO.PromotionExclusionDateDTOLists)
                {
                    if (promotionExclusionDateDTO.PromotionId != promotionDTO.PromotionId)
                    {
                        promotionExclusionDateDTO.PromotionId = promotionDTO.PromotionId;
                    }
                    if (promotionExclusionDateDTO.IsChanged)
                    {
                        updatedPromotionExclusionDateDTOList.Add(promotionExclusionDateDTO);
                    }
                }
                if (updatedPromotionExclusionDateDTOList.Any())
                {
                    PromotionExclusionDateListBL promotionExclusionDateListBL = new PromotionExclusionDateListBL(executionContext, updatedPromotionExclusionDateDTOList);
                    promotionExclusionDateListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug("promotionDTO.PromotionExclusionDateDTOLists");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Promotion records from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(promotionDTO, sqlTransaction);
            try
            {
                PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
                if (promotionDTO.PromotionId >= 0 && promotionDTO.ActiveFlag == false)
                {
                    if ((promotionDTO.PromotionDetailDTOList != null &&
                promotionDTO.PromotionDetailDTOList.Any(x => x.IsActive == true))
                || promotionDTO.PromotionRuleDTOList != null &&
                promotionDTO.PromotionRuleDTOList.Any(x => x.IsActive == true)
                || promotionDTO.PromotionExclusionDateDTOLists != null &&
                promotionDTO.PromotionExclusionDateDTOLists.Any(x => x.IsActive == true))
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 1143);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new ForeignKeyException(message);
                    }
                    SavePromotionChild(sqlTransaction);
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                    List<MachineAttributeDTO> machineAttributeDTOList = promotionListBL.GetPromotionGameAttributes(promotionDTO.PromotionId).Where(x => x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION).ToList();
                    if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                    {
                        foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                        {
                            promotionListBL.DeleteMachineAttribute(machineAttributeDTO.AttributeId, promotionDTO.PromotionId, sqlTransaction);
                        }
                    }
                    promotionDataHandler.Delete(promotionDTO);
                }
                else
                {
                    log.LogVariableState("promotionDTO", promotionDTO);
                    SavePromotionChild(sqlTransaction);
                }
                promotionDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the PromotionDTO and PromotionRuleDTOList , PromotionDetailDTO - children 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (promotionDTO.PromotionDetailDTOList != null)
            {
                foreach (var promotionDetailDTO in promotionDTO.PromotionDetailDTOList)
                {
                    if (promotionDetailDTO.IsChanged)
                    {
                        log.LogVariableState("PromotionDetailDTO", promotionDetailDTO);
                        PromotionDetailBL promotionDetailBL = new PromotionDetailBL(executionContext, promotionDetailDTO);
                        validationErrorList.AddRange(promotionDetailBL.Validate(sqlTransaction));
                    }
                }
            }

            //calling validation for child : PromotionRuleDTO
            if (promotionDTO.PromotionRuleDTOList != null)
            {
                foreach (var promotionRuleDTO in promotionDTO.PromotionRuleDTOList)
                {
                    if (promotionRuleDTO.IsChanged)
                    {
                        log.LogVariableState("PromotionRuleDTO", promotionRuleDTO);
                        PromotionRuleBL promotionRuleBL = new PromotionRuleBL(executionContext, promotionRuleDTO);
                        validationErrorList.AddRange(promotionRuleBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PromotionDTO PromotionDTO
        {
            get
            {
                return promotionDTO;
            }
        }

        private void SetFromSiteTimeOffset()
        {
            log.LogMethodEntry(promotionDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (promotionDTO != null)
                {
                    promotionDTO.TimeTo = SiteContainerList.FromSiteDateTime(promotionDTO.SiteId, promotionDTO.TimeTo);
                    promotionDTO.TimeFrom = SiteContainerList.FromSiteDateTime(promotionDTO.SiteId, promotionDTO.TimeFrom);
                    if (promotionDTO.RecurEndDate != null)
                    {
                        promotionDTO.RecurEndDate = SiteContainerList.FromSiteDateTime(promotionDTO.SiteId, (DateTime)promotionDTO.RecurEndDate);
                    }
                    if (promotionDTO.PromotionExclusionDateDTOLists != null && promotionDTO.PromotionExclusionDateDTOLists.Any())
                    {
                        for (int i = 0; i < promotionDTO.PromotionExclusionDateDTOLists.Count; i++)
                        {
                            if (promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate != null)
                            {
                                promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate = SiteContainerList.FromSiteDateTime(promotionDTO.PromotionExclusionDateDTOLists[i].SiteId, (DateTime)promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate);
                            }
                        }
                    }
                    promotionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(promotionDTO);
        }
        private void SetToSiteTimeOffset()
        {
            log.LogMethodEntry(promotionDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (promotionDTO != null && (promotionDTO.PromotionId == -1 || promotionDTO.IsChanged))
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    promotionDTO.TimeFrom = SiteContainerList.ToSiteDateTime(siteId, promotionDTO.TimeFrom);
                    promotionDTO.TimeTo = SiteContainerList.ToSiteDateTime(siteId, promotionDTO.TimeTo);
                    if (promotionDTO.RecurEndDate != null)
                    {
                        promotionDTO.RecurEndDate = SiteContainerList.ToSiteDateTime(siteId, (DateTime)promotionDTO.RecurEndDate);
                    }
                    if (promotionDTO.PromotionExclusionDateDTOLists != null && promotionDTO.PromotionExclusionDateDTOLists.Any())
                    {
                        for (int i = 0; i < promotionDTO.PromotionExclusionDateDTOLists.Count; i++)
                        {
                            if (promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate != null)
                            {
                                promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate = SiteContainerList.ToSiteDateTime(promotionDTO.PromotionExclusionDateDTOLists[i].SiteId, (DateTime)promotionDTO.PromotionExclusionDateDTOLists[i].ExclusionDate);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(promotionDTO);
        }


    }
    /// <summary>
    /// Manages the list of Promotion
    /// </summary>
    public class PromotionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PromotionDTO> promotionDTOList = new List<PromotionDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PromotionListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for PromotionListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        public PromotionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor for PromotionListBL
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        /// <param name="promotionDTOList">PromotionDTOList object as parameter</param>
        public PromotionListBL(ExecutionContext executionContext,
                                              List<PromotionDTO> promotionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, promotionDTOList);
            this.promotionDTOList = promotionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PromotionDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the PromotionDTO List</returns>
        public List<PromotionDTO> GetPromotionDTOList(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
            List<PromotionDTO> promotionDTOList = promotionDataHandler.GetPromotionDTOList(searchParameters);
            if (loadChildRecords && promotionDTOList.Any())
            {
                Build(promotionDTOList, activeChildRecords, sqlTransaction);
            }
            promotionDTOList = SetFromSiteTimeOffset(promotionDTOList);
            log.LogMethodExit(promotionDTOList);
            return promotionDTOList;
        }

        /// <summary>
        /// Builds the List of Promotion objects based on the list of Promotion id.
        /// </summary>
        /// <param name="promotionDTOList">PromotionDTO List</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        private void Build(List<PromotionDTO> promotionDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(promotionDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, PromotionDTO> promotionDTOPromotionIdMap = new Dictionary<int, PromotionDTO>();
            List<int> promotionIdList = new List<int>();
            for (int i = 0; i < promotionDTOList.Count; i++)
            {
                if (promotionDTOPromotionIdMap.ContainsKey(promotionDTOList[i].PromotionId))
                {
                    continue;
                }
                promotionDTOPromotionIdMap.Add(promotionDTOList[i].PromotionId, promotionDTOList[i]);
                promotionIdList.Add(promotionDTOList[i].PromotionId);
            }
            PromotionRuleListBL promotionRuleListBL = new PromotionRuleListBL(executionContext);
            List<PromotionRuleDTO> promotionRuleDTOList = promotionRuleListBL.GetPromotionRuleDTOListForProducts(promotionIdList, activeChildRecords, sqlTransaction);
            if (promotionRuleDTOList != null && promotionRuleDTOList.Any())
            {
                foreach (PromotionRuleDTO promotionRuleDTO in promotionRuleDTOList)
                {
                    if (promotionDTOPromotionIdMap.ContainsKey(promotionRuleDTO.PromotionId) == false)
                    {
                        continue;
                    }
                    PromotionDTO promotionDTO = promotionDTOPromotionIdMap[promotionRuleDTO.PromotionId];
                    if (promotionDTO.PromotionRuleDTOList == null)
                    {
                        promotionDTO.PromotionRuleDTOList = new List<PromotionRuleDTO>();
                    }
                    promotionDTO.PromotionRuleDTOList.Add(promotionRuleDTO);
                }

            }

            PromotionExclusionDateListBL promotionExclusionDateListBL = new PromotionExclusionDateListBL(executionContext);
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = promotionExclusionDateListBL.GetPromotionExclusionDateDTOListForProducts(promotionIdList, activeChildRecords, sqlTransaction);
            if (promotionExclusionDateDTOList != null && promotionExclusionDateDTOList.Any())
            {
                foreach (PromotionExclusionDateDTO promotionExclusionDateDTO in promotionExclusionDateDTOList)
                {
                    if (promotionDTOPromotionIdMap.ContainsKey(promotionExclusionDateDTO.PromotionId) == false)
                    {
                        continue;
                    }
                    PromotionDTO promotionDTO = promotionDTOPromotionIdMap[promotionExclusionDateDTO.PromotionId];
                    if (promotionDTO.PromotionExclusionDateDTOLists == null)
                    {
                        promotionDTO.PromotionExclusionDateDTOLists = new List<PromotionExclusionDateDTO>();
                    }
                    promotionDTO.PromotionExclusionDateDTOLists.Add(promotionExclusionDateDTO);
                }

            }

            PromotionDetailListBL promotionDetailListBL = new PromotionDetailListBL(executionContext);
            List<PromotionDetailDTO> promotionDetailDTOList = promotionDetailListBL.GetPromotionDetailDTOListForProducts(promotionIdList, activeChildRecords, sqlTransaction);
            if (promotionDetailDTOList != null && promotionDetailDTOList.Any())
            {
                foreach (PromotionDetailDTO promotionDetailDTO in promotionDetailDTOList)
                {
                    if (promotionDTOPromotionIdMap.ContainsKey(promotionDetailDTO.PromotionId) == false)
                    {
                        continue;
                    }
                    PromotionDTO promotionDTO = promotionDTOPromotionIdMap[promotionDetailDTO.PromotionId];
                    if (promotionDTO.PromotionDetailDTOList == null)
                    {
                        promotionDTO.PromotionDetailDTOList = new List<PromotionDetailDTO>();
                    }
                    promotionDTO.PromotionDetailDTOList.Add(promotionDetailDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Promotions DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionDTOList == null ||
                promotionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < promotionDTOList.Count; i++)
            {
                var promotionDTO = promotionDTOList[i];
                if (promotionDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    PromotionBL promotionBL = new PromotionBL(executionContext, promotionDTO);
                    promotionBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving PromotionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PromotionDTO", promotionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Promotions DTO List
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (promotionDTOList == null ||
                promotionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < promotionDTOList.Count; i++)
            {
                var promotionDTO = promotionDTOList[i];
                if (promotionDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    PromotionBL promotionBL = new PromotionBL(executionContext, promotionDTO);
                    promotionBL.Delete(sqlTransaction);
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
                    log.Error("Error occurred while saving PromotionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PromotionDTO", promotionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetPromotionModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler();
            DateTime? result = promotionDataHandler.GetPromotionModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private List<PromotionDTO> SetFromSiteTimeOffset(List<PromotionDTO> promotionDTOList)
        {
            log.LogMethodEntry(promotionDTOList);
            if (SiteContainerList.IsCorporate())
            {
                if (promotionDTOList != null && promotionDTOList.Any())
                {
                    for (int i = 0; i < promotionDTOList.Count; i++)
                    {
                        promotionDTOList[i].TimeFrom = SiteContainerList.FromSiteDateTime(promotionDTOList[i].SiteId, promotionDTOList[i].TimeFrom);
                        promotionDTOList[i].TimeTo = SiteContainerList.FromSiteDateTime(promotionDTOList[i].SiteId, promotionDTOList[i].TimeTo);
                        if (promotionDTOList[i].RecurEndDate != null)
                        {
                            promotionDTOList[i].RecurEndDate = SiteContainerList.FromSiteDateTime(promotionDTOList[i].SiteId, (DateTime)promotionDTOList[i].RecurEndDate);
                        }
                        if(promotionDTOList[i].PromotionExclusionDateDTOLists != null && promotionDTOList[i].PromotionExclusionDateDTOLists.Any())
                        {
                            for(int j=0; j< promotionDTOList[i].PromotionExclusionDateDTOLists.Count; j++)
                            {
                                if(promotionDTOList[i].PromotionExclusionDateDTOLists[j].ExclusionDate != null)
                                {
                                    promotionDTOList[i].PromotionExclusionDateDTOLists[j].ExclusionDate = SiteContainerList.FromSiteDateTime(promotionDTOList[i].PromotionExclusionDateDTOLists[j].SiteId, (DateTime)promotionDTOList[i].PromotionExclusionDateDTOLists[j].ExclusionDate);
                                }
                            }
                        }
                        promotionDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(promotionDTOList);
            return promotionDTOList;
        }

        /// <summary>
        /// Get the Promotion Game Attributes
        /// </summary>
        /// <param name="promotionId">promotionId</param>
        public List<MachineAttributeDTO> GetPromotionGameAttributes(int promotionId)
        {
            log.LogMethodEntry(promotionId);
            PromotionBL promotionBL = new PromotionBL(executionContext, promotionId, false, false, null);
            PromotionDTO promotionDTO = new PromotionDTO();
            promotionDTO = promotionBL.PromotionDTO;
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
            List<MachineAttributeDTO> machineAttributeDTOList = new List<MachineAttributeDTO>();
            machineAttributeDTOList = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.PROMOTION, -1, -1, -1, promotionDTO.PromotionId, -1, executionContext.GetSiteId());
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        /// <summary>
        /// Save the Promotion Game Attributes
        /// </summary>
        /// <param name="machineAttributeDTOList">machineAttributeDTOList</param>
        /// <param name="promotionId">promotionId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void SaveAndUpdateGameAttribute(List<MachineAttributeDTO> machineAttributeDTOList, int promotionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineAttributeDTOList);
            if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
                MachineAttributeDTO machineAttributeDTO = new MachineAttributeDTO();
                foreach (MachineAttributeDTO currAttribute in machineAttributeDTOList)
                {
                    if (currAttribute.EnableForPromotion == false)
                    {
                        throw new ValidationException("The machine attribute " + currAttribute.AttributeName + " is not enabled for promotion");
                    }
                    int attributeId = machineAttributeDataHandler.GetAttributeId(currAttribute.AttributeName.ToString(), executionContext.GetSiteId());
                    List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.ATTRIBUTE_ID, Convert.ToString(attributeId)));
                    searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.PROMOTION_ID, Convert.ToString(promotionId)));
                    attributeId = machineAttributeDataHandler.GetEntityIDs(searchByParameters);
                    machineAttributeDTO = currAttribute;
                    if (attributeId == 0 && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION && machineAttributeDTO.EnableForPromotion)
                    {
                        machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.PROMOTION, promotionId, executionContext.GetUserId(), executionContext.GetSiteId());
                        if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                        }
                        machineAttributeDTO.AcceptChanges();
                    }
                    else
                    {
                        if (machineAttributeDTO.IsChanged == true && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION && machineAttributeDTO.EnableForPromotion) //11-Jan-2016 - Checking for context
                        {
                            machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, MachineAttributeDTO.AttributeContext.PROMOTION, promotionId, executionContext.GetUserId(), executionContext.GetSiteId());
                            if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
                            {
                                AuditLog auditLog = new AuditLog(executionContext);
                                auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
                            }
                            machineAttributeDTO.AcceptChanges();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Promotion Game Attributes
        /// </summary>
        /// <param name="attributeId">attributeId</param>
        /// <param name="promotionId">promotionId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeleteMachineAttribute(int attributeId, int promotionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attributeId, promotionId);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            machineAttributeDataHandler.DeleteMachineAttribute(attributeId, promotionId, executionContext.GetSiteId(), MachineAttributeDTO.AttributeContext.PROMOTION);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PromotionViewDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the PromotionDTO List</returns>
        public PromotionViewDTO GetGamePromotionDetailViewDTO(int membershipId, int gameId, int gameProfileId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(membershipId, gameId, gameProfileId, sqlTransaction);
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
            PromotionViewDTO promotionViewDTO = promotionDataHandler.GetGamePromotionDetailViewDTO(membershipId, gameId, gameProfileId, sqlTransaction);
            log.LogMethodExit(promotionViewDTO);
            return promotionViewDTO;
        }

        public List<PromotionViewDTO> GetGamePromotionDetailViewDTOList(int membershipId, string machineIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineIdList, membershipId, sqlTransaction);
            PromotionDataHandler promotionDataHandler = new PromotionDataHandler(sqlTransaction);
            List<PromotionViewDTO> promotionViewDTOList = promotionDataHandler.GetGamePromotionDetailViewDTOList(membershipId, machineIdList);
            log.LogMethodExit(promotionViewDTOList);
            return promotionViewDTOList;
        }
    }
}


