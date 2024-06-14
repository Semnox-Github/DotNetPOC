/********************************************************************************************
 * Project Name - CampaignExecutionLogDetail BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     28-Jan-2021      Prajwal S     Created 
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
    class CampaignExecutionLogDetailBL
    {
        private CampaignExecutionLogDetailDTO campaignExecutionLogDetailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignExecutionLogDetail class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private CampaignExecutionLogDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.campaignExecutionLogDetailDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignExecutionLogDetail id as the parameter Would fetch the CampaignExecutionLogDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id">id</param>
        /// <param name="sqltransction">sqltransction</param>
        public CampaignExecutionLogDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqltransction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransction);
            CampaignExecutionLogDetailDataHandler campaignExecutionLogDetailDataHandler = new CampaignExecutionLogDetailDataHandler(sqltransction);
            this.campaignExecutionLogDetailDTO = campaignExecutionLogDetailDataHandler.GetCampaignExecutionLogDetail(id);
            if (campaignExecutionLogDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignExecutionLogDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(campaignExecutionLogDetailDTO);
        }

        /// <summary>
        /// Creates CampaignExecutionLogDetail object using the CampaignExecutionLogDetailDTO
        /// </summary>
        /// <param name="campaignExecutionLogDetailDTO">CampaignExecutionLogDetailDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public CampaignExecutionLogDetailBL(ExecutionContext executionContext, CampaignExecutionLogDetailDTO campaignExecutionLogDetailDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignExecutionLogDetailDTO);
            this.campaignExecutionLogDetailDTO = campaignExecutionLogDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CampaignExecutionLogDetail object using the CampaignExecutionLogDetailDTO
        /// </summary>
        /// <param name="campaignExecutionLogDetailDTO">CampaignExecutionLogDetailDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public CampaignExecutionLogDetailBL(CampaignExecutionLogDetailDTO campaignExecutionLogDetailDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(campaignExecutionLogDetailDTO, executionContext);
            this.campaignExecutionLogDetailDTO = campaignExecutionLogDetailDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// save and updates the record 
        /// </summary>
        /// <param name="sqlTransaction">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignExecutionLogDetailDTO.IsChanged == false && campaignExecutionLogDetailDTO.CampaignExecutionLogDetailId > -1)
            {
                log.LogMethodExit(null, "CampaignExecutionLogDetailDTO is not changed.");
                return;
            }
            CampaignExecutionLogDetailDataHandler campaignExecutionLogDetailDataHandler = new CampaignExecutionLogDetailDataHandler(sqlTransaction);
            campaignExecutionLogDetailDataHandler.Save(CampaignExecutionLogDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }


        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (campaignExecutionLogDetailDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignExecutionLogDetailDTO CampaignExecutionLogDetailDTO
        {
            get
            {
                return campaignExecutionLogDetailDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CampaignExecutionLogDetail
    /// </summary>
    public class CampaignExecutionLogDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CampaignExecutionLogDetailListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public CampaignExecutionLogDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="CampaignExecutionLogDetailToList">CampaignExecutionLogDetailToList</param>
        /// <param name="executionContext">executionContext</param>
        public CampaignExecutionLogDetailListBL(ExecutionContext executionContext, List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignExecutionLogDetailDTOList);
            this.campaignExecutionLogDetailDTOList = campaignExecutionLogDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CampaignExecutionLogDetail list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>CampaignExecutionLogDetail list</returns>
        public List<CampaignExecutionLogDetailDTO> GetCampaignExecutionLogDetailDTOList(List<KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CampaignExecutionLogDetailDataHandler campaignExecutionLogDetailDataHandler = new CampaignExecutionLogDetailDataHandler(sqlTransaction);
            List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailsList = campaignExecutionLogDetailDataHandler.GetCampaignExecutionLogDetailList(searchParameters, sqlTransaction);
            log.LogMethodExit(campaignExecutionLogDetailsList);
            return campaignExecutionLogDetailsList;
        }

        /// <summary>
        /// Validates and saves the CampaignExecutionLogDetailDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignExecutionLogDetailDTOList == null ||
                campaignExecutionLogDetailDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            CampaignExecutionLogDetailDataHandler campaignExecutionLogDetailDataHandler = new CampaignExecutionLogDetailDataHandler(sqlTransaction);
            campaignExecutionLogDetailDataHandler.Save(campaignExecutionLogDetailDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the CampaignExecutionLogDetailDTO List for CampaignExecutionLogList
        /// </summary>
        /// <param name="campaignExecutionLogIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of CampaignCommuncationDefinitionDTO</returns>
        public List<CampaignExecutionLogDetailDTO> GetCampaignExecutionLogDetailDTOList(List<int> campaignExecutionLogIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignExecutionLogIdList, activeRecords, sqlTransaction);
            CampaignExecutionLogDetailDataHandler campaignExecutionLogDetailDataHandler = new CampaignExecutionLogDetailDataHandler(sqlTransaction);
            List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList = campaignExecutionLogDetailDataHandler.GetCampaignExecutionLogDetailDTOList(campaignExecutionLogIdList, activeRecords);
            log.LogMethodExit(campaignExecutionLogDetailDTOList);
            return campaignExecutionLogDetailDTOList;
        }
    }

}

