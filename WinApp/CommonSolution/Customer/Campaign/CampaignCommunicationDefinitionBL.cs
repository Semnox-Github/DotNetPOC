/********************************************************************************************
 * Project Name - CampaignCommunicationDefinition BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     21-Jan-2021      Prajwal S     Created 
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
    public class CampaignCommunicationDefinitionBL
    {
        private CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignCommunicationDefinitionBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private CampaignCommunicationDefinitionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.campaignCommunicationDefinitionDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CampaignCommunicationDefinitionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CampaignCommunicationDefinitionDataHandler campaignCommunicationDefinitionDataHandler = new CampaignCommunicationDefinitionDataHandler(sqlTransaction);
            campaignCommunicationDefinitionDTO = campaignCommunicationDefinitionDataHandler.GetCampaignCommunicationDefinition(id);
            if (campaignCommunicationDefinitionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCommunicationDefinition", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates CampaignCommunicationDefinitionBL object using the CampaignCommunicationDefinitionDTO
        /// </summary>
        /// <param name="campaignCommunicationDefinitionDTO">CampaignCommunicationDefinitionDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public CampaignCommunicationDefinitionBL(ExecutionContext executionContext, CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignCommunicationDefinitionDTO);
            if (campaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId < 0)
            {
                ValidateMessagingClientId(campaignCommunicationDefinitionDTO.MessagingClientId);
                ValidateMessageTemplateId(campaignCommunicationDefinitionDTO.MessageTemplateId);

            }
            this.campaignCommunicationDefinitionDTO = campaignCommunicationDefinitionDTO;
            log.LogMethodExit();
        }

        public void Update(CampaignCommunicationDefinitionDTO parameterCampaignCommunicationDefinitionDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCampaignCommunicationDefinitionDTO, sqlTransaction);
            ChangeCampaignDefinitionId(parameterCampaignCommunicationDefinitionDTO.CampaignDefinitionId);
            ChangeMessageTemplateid(parameterCampaignCommunicationDefinitionDTO.MessageTemplateId);
            ChangeMessagingClientId(parameterCampaignCommunicationDefinitionDTO.MessagingClientId);
            ChangeRetry(parameterCampaignCommunicationDefinitionDTO.Retry);
            ChangeIsActive(parameterCampaignCommunicationDefinitionDTO.IsActive);

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignCommunicationDefinitionDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignCommunicationDefinitionDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeMessagingClientId(int messagingClientId)
        {
            log.LogMethodEntry(messagingClientId);
            if (campaignCommunicationDefinitionDTO.MessagingClientId == messagingClientId)
            {
                log.LogMethodExit(null, "No changes to user MessagingClientId");
                return;
            }
            campaignCommunicationDefinitionDTO.MessagingClientId = messagingClientId;
            log.LogMethodExit();
        }

        public void ChangeMessageTemplateid(int messageTemplateid)
        {
            log.LogMethodEntry(messageTemplateid);
            if (campaignCommunicationDefinitionDTO.MessageTemplateId == messageTemplateid)
            {
                log.LogMethodExit(null, "No changes to user MessageTemplateid");
                return;
            }
            campaignCommunicationDefinitionDTO.MessageTemplateId = messageTemplateid;
            log.LogMethodExit();
        }

        public void ChangeRetry(bool retry)
        {
            log.LogMethodEntry(retry);
            if (campaignCommunicationDefinitionDTO.Retry == retry)
            {
                log.LogMethodExit(null, "No changes to user Retry");
                return;
            }
            campaignCommunicationDefinitionDTO.Retry = retry;
            log.LogMethodExit();
        }


        public void ChangeCampaignDefinitionId(int campaignDefinitionId)
        {
            log.LogMethodEntry(campaignDefinitionId);
            if (campaignCommunicationDefinitionDTO.CampaignDefinitionId == campaignDefinitionId)
            {
                log.LogMethodExit(null, "No changes to user posTypeId");
                return;
            }
            campaignCommunicationDefinitionDTO.CampaignDefinitionId = campaignDefinitionId;
            log.LogMethodExit();
        }

        private void ValidateMessagingClientId(int messageClientId)
        {
            log.LogMethodEntry(messageClientId);
            if (messageClientId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1773, MessageContainerList.GetMessage(executionContext, "MessagingClientId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("MessagingClientId  cannot be null", "CampaignCommunicationDefinition", "MessagingClient", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateMessageTemplateId(int messageTemplateId)
        {
            log.LogMethodEntry(messageTemplateId);
            if (messageTemplateId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1773, MessageContainerList.GetMessage(executionContext, "MessageTemplateId"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("MessageTemplate Id cannot be null", "CampaignCommunicationDefinition", "MessageTemplate", errorMessage);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Saves the CampaignCommunicationDefinition
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CampaignCommunicationDefinitionDataHandler CampaignCommunicationDefinitionDataHandler = new CampaignCommunicationDefinitionDataHandler(sqlTransaction);
            if (campaignCommunicationDefinitionDTO.CampaignCommunicationDefinitionId < 0)
            {
                campaignCommunicationDefinitionDTO = CampaignCommunicationDefinitionDataHandler.Insert(campaignCommunicationDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                campaignCommunicationDefinitionDTO.AcceptChanges();
            }
            else
            {
                if (campaignCommunicationDefinitionDTO.IsChanged)
                {
                    campaignCommunicationDefinitionDTO = CampaignCommunicationDefinitionDataHandler.Update(campaignCommunicationDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    campaignCommunicationDefinitionDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignCommunicationDefinitionDTO CampaignCommunicationDefinitionDTO
        {
            get
            {
                return campaignCommunicationDefinitionDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignCommunicationDefinition
    /// </summary>
    public class CampaignCommunicationDefinitionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignCommunicationDefinitionListBL class
        /// </summary>
        public CampaignCommunicationDefinitionListBL()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of CampaignCommunicationDefinitionListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CampaignCommunicationDefinitionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CampaignCommunicationDefinition list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CampaignCommunicationDefinition list</returns>
        public List<CampaignCommunicationDefinitionDTO> GetCampaignCommunicationDefinitionDTOList(List<KeyValuePair<CampaignCommunicationDefinitionDTO.SearchByCampaignCommunicationDefinitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignCommunicationDefinitionDataHandler campaignCommunicationDefinitionDataHandler = new CampaignCommunicationDefinitionDataHandler(sqlTransaction);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionsList = campaignCommunicationDefinitionDataHandler.GetCampaignCommunicationDefinitionList(searchParameters, sqlTransaction);
            log.LogMethodExit(campaignCommunicationDefinitionsList);
            return campaignCommunicationDefinitionsList;
        }

        /// <summary>
        /// This method should be used to Save and Update the CampaignCommunicationDefinition.
        /// </summary>
        public List<CampaignCommunicationDefinitionDTO> Save(List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CampaignCommunicationDefinitionDTO> savedCampaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
            foreach (CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO in campaignCommunicationDefinitionDTOList)
            {
                CampaignCommunicationDefinitionBL campaignCommunicationDefinitionBL = new CampaignCommunicationDefinitionBL(executionContext, campaignCommunicationDefinitionDTO);
                campaignCommunicationDefinitionBL.Save(sqlTransaction);
                savedCampaignCommunicationDefinitionDTOList.Add(campaignCommunicationDefinitionBL.CampaignCommunicationDefinitionDTO);
            }
            log.LogMethodExit(savedCampaignCommunicationDefinitionDTOList);
            return savedCampaignCommunicationDefinitionDTOList;
        }
        /// <summary>
        /// Gets the CampaignCommuncationDefinitionDTO List for CampaignDefinitionList
        /// </summary>
        /// <param name="campaignDefinitionList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CampaignCommuncationDefinitionDTO</returns>
        public List<CampaignCommunicationDefinitionDTO> GetCampaignCommunicationDefinitionDTOList(List<int> campaignDefinitionList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignDefinitionList, activeRecords, sqlTransaction);
            CampaignCommunicationDefinitionDataHandler campaignCommunicationDefinitionDataHandler = new CampaignCommunicationDefinitionDataHandler(sqlTransaction);
            List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList = campaignCommunicationDefinitionDataHandler.GetCampaignCommunicationDefinitionDTOList(campaignDefinitionList, activeRecords);
            log.LogMethodExit(campaignCommunicationDefinitionDTOList);
            return campaignCommunicationDefinitionDTOList;
        }
    }
}
