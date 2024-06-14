/********************************************************************************************
 * Project Name - CampaignDiscountDefinition BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     20-Jan-2021      Prajwal S     Created 
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
    public class CampaignDiscountDefinitionBL
    {
        private CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignDiscountDefinitionBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private CampaignDiscountDefinitionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.campaignDiscountDefinitionDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignDiscountDefinitionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CampaignDiscountDefinitionDataHandler campaignDiscountDefinitionDataHandler = new CampaignDiscountDefinitionDataHandler(sqlTransaction);
            campaignDiscountDefinitionDTO = campaignDiscountDefinitionDataHandler.GetCampaignDiscountDefinition(id);
            if (CampaignDiscountDefinitionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignDiscountDefinition", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates CampaignDiscountDefinitionBL object using the CampaignDiscountDefinitionDTO
        /// </summary>
        /// <param name="campaignDiscountDefinitionDTO">CampaignDiscountDefinitionDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public CampaignDiscountDefinitionBL(ExecutionContext executionContext, CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignDiscountDefinitionDTO);
            if (campaignDiscountDefinitionDTO.CampaignDiscountDefinitionId < 0)
            {
                ValidateDiscountId(campaignDiscountDefinitionDTO.DiscountId);
                ValidateValidFor(campaignDiscountDefinitionDTO.ValidFor);
                ValidateValidForDaysMonths(campaignDiscountDefinitionDTO.ValidForDaysMonths);
                ValidateExpiryDate(campaignDiscountDefinitionDTO.ExpiryDate);

            }
            this.campaignDiscountDefinitionDTO = campaignDiscountDefinitionDTO;
            log.LogMethodExit();
        }

        public void Update(CampaignDiscountDefinitionDTO parameterCampaignDiscountDefinitionDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCampaignDiscountDefinitionDTO, sqlTransaction);
            ChangeCampaignDefinitionId(parameterCampaignDiscountDefinitionDTO.CampaignDefinitionId);
            ChangeDiscountId(parameterCampaignDiscountDefinitionDTO.DiscountId);
            ChangeExpiryDate(parameterCampaignDiscountDefinitionDTO.ExpiryDate);
            ChangeValidFor(parameterCampaignDiscountDefinitionDTO.ValidFor);
            ChangeIsActive(parameterCampaignDiscountDefinitionDTO.IsActive);
            ChangeValidForDaysMonths(parameterCampaignDiscountDefinitionDTO.ValidForDaysMonths);
            log.LogMethodExit();
               
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignDiscountDefinitionDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignDiscountDefinitionDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeExpiryDate(DateTime? expiryDate)
        {
            log.LogMethodEntry(expiryDate);
            if (campaignDiscountDefinitionDTO.ExpiryDate == expiryDate)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignDiscountDefinitionDTO.ExpiryDate = expiryDate;
            ValidateExpiryDate(expiryDate);
            log.LogMethodExit();
        }

        public void ChangeValidForDaysMonths(string validForDaysMonths)
        {
            log.LogMethodEntry(validForDaysMonths);
            if (campaignDiscountDefinitionDTO.ValidForDaysMonths == validForDaysMonths)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignDiscountDefinitionDTO.ValidForDaysMonths = validForDaysMonths;
            log.LogMethodExit();
        }

        public void ChangeValidFor(int validFor)
        {
            log.LogMethodEntry(validFor);
            if (campaignDiscountDefinitionDTO.ValidFor == validFor)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignDiscountDefinitionDTO.ValidFor = validFor;
            log.LogMethodExit();
        }


        public void ChangeCampaignDefinitionId(int campaignDefinitionId)
        {
            log.LogMethodEntry(campaignDefinitionId);
            if (campaignDiscountDefinitionDTO.CampaignDefinitionId == campaignDefinitionId)
            {
                log.LogMethodExit(null, "No changes to user posTypeId");
                return;
            }
            CampaignDiscountDefinitionDTO.CampaignDefinitionId = campaignDefinitionId;
            log.LogMethodExit();
        }


        public void ChangeDiscountId(int discountId)
        {
            log.LogMethodEntry(discountId);
            if (campaignDiscountDefinitionDTO.DiscountId == discountId)
            {
                log.LogMethodExit(null, "No changes to user posTypeId");
                return;
            }
            campaignDiscountDefinitionDTO.DiscountId = discountId;
            log.LogMethodExit();
        }

        private void ValidateValidFor(int validFor)
        {
            log.LogMethodEntry(validFor);
            if (validFor < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 34, MessageContainerList.GetMessage(executionContext, "validFor"), 1);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("validFor is less than 1.", "CampaignDiscountDefinition", "validFor", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateValidForDaysMonths(string validForDaysMonths)
        {
            log.LogMethodEntry(validForDaysMonths);
            if (validForDaysMonths.Length > 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "validForDaysMonths"), 1);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("validForDaysMonths cannot be more than 1 character", "CampaignDiscountDefinition", "validForDaysMonths", errorMessage);
            }
            log.LogMethodExit();
        }
        private void ValidateDiscountId(int discountId)
        {
            log.LogMethodEntry(discountId);
            if (discountId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1773, MessageContainerList.GetMessage(executionContext, "DiscountId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Discount Id cannot be null", "CampaignDiscountDefinition", "Discount", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateExpiryDate(DateTime? expiryDate)
        {
            log.LogMethodEntry(expiryDate);
            if (expiryDate == DateTime.MinValue)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1773, MessageContainerList.GetMessage(executionContext, "expiryDate"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("ExpiryDate Cannot be Minimum value", "CampaignDiscountDefinition", "ExpiryDate", errorMessage);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Saves the CampaignDiscountDefinition
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CampaignDiscountDefinitionDataHandler campaignDiscountDefinitionDataHandler = new CampaignDiscountDefinitionDataHandler(sqlTransaction);
            if (campaignDiscountDefinitionDTO.CampaignDiscountDefinitionId < 0)
            {
                campaignDiscountDefinitionDTO = campaignDiscountDefinitionDataHandler.Insert(campaignDiscountDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                CampaignDiscountDefinitionDTO.AcceptChanges();
            }
            else
            {
                if (campaignDiscountDefinitionDTO.IsChanged)
                {
                    campaignDiscountDefinitionDTO = campaignDiscountDefinitionDataHandler.Update(campaignDiscountDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignDiscountDefinitionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignDiscountDefinitionDTO CampaignDiscountDefinitionDTO
        {
            get
            {
                return campaignDiscountDefinitionDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignDiscountDefinition
    /// </summary>
    public class CampaignDiscountDefinitionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;



        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignDiscountDefinitionListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of CampaignDiscountDefinitionListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CampaignDiscountDefinitionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CampaignDiscountDefinition list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CampaignDiscountDefinition list</returns>
        public List<CampaignDiscountDefinitionDTO> GetCampaignDiscountDefinitionDTOList(List<KeyValuePair<CampaignDiscountDefinitionDTO.SearchByCampaignDiscountDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignDiscountDefinitionDataHandler campaignDiscountDefinitionDataHandler = new CampaignDiscountDefinitionDataHandler(sqlTransaction);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionsList = campaignDiscountDefinitionDataHandler.GetCampaignDiscountDefinitionList(searchParameters, sqlTransaction);
            log.LogMethodExit(campaignDiscountDefinitionsList);
            return campaignDiscountDefinitionsList;
        }

        /// <summary>
        /// This method should be used to Save and Update the CampaignDiscountDefinition.
        /// </summary>
        public List<CampaignDiscountDefinitionDTO> Save(List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CampaignDiscountDefinitionDTO> savedCampaignDiscountDefinitionDTOList = new List<CampaignDiscountDefinitionDTO>();
            foreach (CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO in campaignDiscountDefinitionDTOList)
            {
                CampaignDiscountDefinitionBL campaignDiscountDefinitionBL = new CampaignDiscountDefinitionBL(executionContext, campaignDiscountDefinitionDTO);
                campaignDiscountDefinitionBL.Save(sqlTransaction);
                savedCampaignDiscountDefinitionDTOList.Add(campaignDiscountDefinitionBL.CampaignDiscountDefinitionDTO);
            }
            log.LogMethodExit(savedCampaignDiscountDefinitionDTOList);
            return savedCampaignDiscountDefinitionDTOList;
        }

        /// <summary>
        /// Gets the CampaignDiscountDefinitionDTO List for CampaignDefinitionList
        /// </summary>
        /// <param name="campaignDefinitionList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CampaignDiscountDefinitionDTO</returns>
        public List<CampaignDiscountDefinitionDTO> GetCampaignDiscountDefinitionDTOList(List<int> campaignDefinitionList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignDefinitionList, activeRecords, sqlTransaction);
            CampaignDiscountDefinitionDataHandler campaignDiscountDefinitionDataHandler = new CampaignDiscountDefinitionDataHandler(sqlTransaction);
            List<CampaignDiscountDefinitionDTO> campaignDiscountDefinitionDTOList = campaignDiscountDefinitionDataHandler.GetCampaignDiscountDefinitionDTOList(campaignDefinitionList, activeRecords);
            log.LogMethodExit(campaignDiscountDefinitionDTOList);
            return campaignDiscountDefinitionDTOList;
        }
    }
}