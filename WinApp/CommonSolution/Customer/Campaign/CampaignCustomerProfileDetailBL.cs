/********************************************************************************************
 * Project Name - CampaignCustomerProfileDetail BL
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
    public class CampaignCustomerProfileDetailBL
    {
        private CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignCustomerProfileDetail class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private CampaignCustomerProfileDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.campaignCustomerProfileDetailDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignCustomerProfileDetail id as the parameter Would fetch the CampaignCustomerProfileDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id">id</param>
        /// <param name="sqltransction">sqltransction</param>
        public CampaignCustomerProfileDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqltransction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransction);
            CampaignCustomerProfileDetailDataHandler campaignCustomerProfileDetailDataHandler = new CampaignCustomerProfileDetailDataHandler(sqltransction);
            this.campaignCustomerProfileDetailDTO = campaignCustomerProfileDetailDataHandler.GetCampaignCustomerProfileDetail(id);
            if (campaignCustomerProfileDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignCustomerProfileDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(campaignCustomerProfileDetailDTO);
        }


        /// <summary>
        /// Creates CampaignCustomerProfileDetailBL object using the CampaignCustomerProfileDetailDTO
        /// </summary>
        /// <param name="campaignCustomerProfileDetailDTO">CampaignCustomerProfileDetailDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public CampaignCustomerProfileDetailBL(ExecutionContext executionContext, CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignCustomerProfileDetailDTO);
            this.campaignCustomerProfileDetailDTO = campaignCustomerProfileDetailDTO;
            log.LogMethodExit();
        }

        public void Update(CampaignCustomerProfileDetailDTO parameterCampaignCustomerProfileDetailDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCampaignCustomerProfileDetailDTO, sqlTransaction);
            ChangeAccountId(parameterCampaignCustomerProfileDetailDTO.AccountId);
            ChangeCampaignCustomerProfileId(parameterCampaignCustomerProfileDetailDTO.CampaignCustomerProfileId);
            ChangeContactId(parameterCampaignCustomerProfileDetailDTO.ContactId);
            ChangeCustomerId(parameterCampaignCustomerProfileDetailDTO.CustomerId);
            ChangeIsActive(parameterCampaignCustomerProfileDetailDTO.IsActive);

        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (campaignCustomerProfileDetailDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            campaignCustomerProfileDetailDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeCampaignCustomerProfileId(int campaignCustomerProfileId)
        {
            log.LogMethodEntry(campaignCustomerProfileId);
            if (campaignCustomerProfileDetailDTO.CampaignCustomerProfileId == campaignCustomerProfileId)
            {
                log.LogMethodExit(null, "No changes to user CampaignCustomerProfileId");
                return;
            }
            campaignCustomerProfileDetailDTO.CampaignCustomerProfileId = campaignCustomerProfileId;
            log.LogMethodExit();
        }

        public void ChangeAccountId(int accountId)
        {
            log.LogMethodEntry(accountId);
            if (campaignCustomerProfileDetailDTO.AccountId == accountId)
            {
                log.LogMethodExit(null, "No changes to user AccountId");
                return;
            }
            campaignCustomerProfileDetailDTO.AccountId = accountId;
            log.LogMethodExit();
        }

        public void ChangeCustomerId(int customerId)
        {
            log.LogMethodEntry(customerId);
            if (campaignCustomerProfileDetailDTO.CustomerId == customerId)
            {
                log.LogMethodExit(null, "No changes to user customerId");
                return;
            }
            campaignCustomerProfileDetailDTO.CustomerId = customerId;
            log.LogMethodExit();
        }

        public void ChangeContactId(int contactId)
        {
            log.LogMethodEntry(contactId);
            if (campaignCustomerProfileDetailDTO.ContactId == contactId)
            {
                log.LogMethodExit(null, "No changes to user ContactId");
                return;
            }
            campaignCustomerProfileDetailDTO.ContactId = contactId;
            log.LogMethodExit();
        }


        /// <summary>
        /// save and updates the record 
        /// </summary>
        /// <param name="sqlTransaction">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignCustomerProfileDetailDTO.IsChanged == false && campaignCustomerProfileDetailDTO.CampaignCustomerProfileDetailId > -1)
            {
                log.LogMethodExit(null, "CampaignCustomerProfileDetailDTO is not changed.");
                return;
            }
            CampaignCustomerProfileDetailDataHandler campaignCustomerProfileDetailDataHandler = new CampaignCustomerProfileDetailDataHandler(sqlTransaction);
            campaignCustomerProfileDetailDataHandler.Save(campaignCustomerProfileDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignCustomerProfileDetailDTO CampaignCustomerProfileDetailDTO
        {
            get
            {
                return campaignCustomerProfileDetailDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignCustomerProfileDetail
    /// </summary>
    public class CampaignCustomerProfileDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignCustomerProfileDetailListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of CampaignCustomerProfileDetailListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CampaignCustomerProfileDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CampaignCustomerProfileDetail list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CampaignCustomerProfileDetail list</returns>
        public List<CampaignCustomerProfileDetailDTO> GetCampaignCustomerProfileDetailDTOList(List<KeyValuePair<CampaignCustomerProfileDetailDTO.SearchByCampaignCustomerProfileDetailParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignCustomerProfileDetailDataHandler campaignCustomerProfileDetailDataHandler = new CampaignCustomerProfileDetailDataHandler(sqlTransaction);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailsList = campaignCustomerProfileDetailDataHandler.GetCampaignCustomerProfileDetailList(searchParameters, sqlTransaction);
            log.LogMethodExit(campaignCustomerProfileDetailsList);
            return campaignCustomerProfileDetailsList;
        }
        /// <summary>
        /// Validates and saves the CampaignCustomerProfileDetailDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public List<CampaignCustomerProfileDetailDTO> Save(List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailDTOList, sqlTransaction);

            CampaignCustomerProfileDetailDataHandler campaignCustomerProfileDetailDataHandler = new CampaignCustomerProfileDetailDataHandler(sqlTransaction);
            campaignCustomerProfileDetailDTOList = campaignCustomerProfileDetailDataHandler.Save(campaignCustomerProfileDetailDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit(campaignCustomerProfileDetailDTOList);
            return campaignCustomerProfileDetailDTOList;
        }

        /// <summary>
        /// Gets the CampaignCustomerProfileDetailDTO List for CampaignCustomerProfileList
        /// </summary>
        /// <param name="campaignCustomerProfileIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CampaignCommuncationDefinitionDTO</returns>
        public List<CampaignCustomerProfileDetailDTO> GetCampaignCustomerProfileDetailDTOList(List<int> campaignCustomerProfileIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignCustomerProfileIdList, activeRecords, sqlTransaction);
            CampaignCustomerProfileDetailDataHandler campaignCustomerProfileDetailDataHandler = new CampaignCustomerProfileDetailDataHandler(sqlTransaction);
            List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList = campaignCustomerProfileDetailDataHandler.GetCampaignCustomerProfileDetailDTOList(campaignCustomerProfileIdList, activeRecords);
            log.LogMethodExit(campaignCustomerProfileDetailDTOList);
            return campaignCustomerProfileDetailDTOList;
        }
    }
}
