/********************************************************************************************
 * Project Name - CampaignExecutionLog BL
 * Description  - Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     25-Jan-2021      Prajwal S     Created 
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
    class CampaignExecutionLogBL
    {
        CampaignExecutionLogDTO campaignExecutionLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CampaignExecutionLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CampaignExecutionLog id as the parameter
        /// Would fetch the CampaignExecutionLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public CampaignExecutionLogBL(ExecutionContext executionContext, int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            CampaignExecutionLogDataHandler campaignExecutionLogDataHandler = new CampaignExecutionLogDataHandler(sqlTransaction);
            campaignExecutionLogDTO = campaignExecutionLogDataHandler.GetCampaignExecutionLog(id);
            if (campaignExecutionLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignExecutionLogDTO", id);   //added to thow exception
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(campaignExecutionLogDTO);
        }


        /// <summary>
        /// Builds the child records for Tax object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CampaignExecutionLogDetailListBL campaignExecutionLogDetailListBL = new CampaignExecutionLogDetailListBL(executionContext);
            List<KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>> searchParameters = new List<KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>>();
            searchParameters.Add(new KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>(CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.CAMPAIGN_EXECUTION_LOG_ID, campaignExecutionLogDTO.CampaignExecutionLogId.ToString()));
            searchParameters.Add(new KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>(CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters, string>(CampaignExecutionLogDetailDTO.SearchByCampaignExecutionLogDetailParameters.IS_ACTIVE, "1"));
            }
            List<CampaignExecutionLogDetailDTO> CampaignExecutionLogDetailDTOList = campaignExecutionLogDetailListBL.GetCampaignExecutionLogDetailDTOList(searchParameters, sqlTransaction);
            campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList = new List<CampaignExecutionLogDetailDTO>(CampaignExecutionLogDetailDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CampaignExecutionLogBL object using the CampaignExecutionLogDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="campaignExecutionLogDTO"></param>
        public CampaignExecutionLogBL(ExecutionContext executionContext, CampaignExecutionLogDTO campaignExecutionLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, campaignExecutionLogDTO);
            if(campaignExecutionLogDTO.CampaignExecutionLogId > -1)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CampaignExecutionLog", campaignExecutionLogDTO.CampaignExecutionLogId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            ValidateRunDate(campaignExecutionLogDTO.RunDate);
            this.campaignExecutionLogDTO = new CampaignExecutionLogDTO(-1, campaignExecutionLogDTO.CampaignDefinitionId, campaignExecutionLogDTO.RunDate, true);
            log.LogMethodExit();
        }

        private void ValidateRunDate(DateTime runDate)
        {
            log.LogMethodEntry(runDate);
            if (runDate == DateTime.MinValue || runDate == DateTime.MaxValue)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "RunDate"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "CampaignExecutionLog", "EmpEndDate", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// save and updates the record 
        /// </summary>
        /// <param name="sqlTransaction">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignExecutionLogDTO.IsChanged == false && campaignExecutionLogDTO.CampaignExecutionLogId > -1)
            {
                log.LogMethodExit(null, "CampaignExecutionLogDTO is not changed.");
                return;
            }
            CampaignExecutionLogDataHandler campaignExecutionLogDataHandler = new CampaignExecutionLogDataHandler(sqlTransaction);
            campaignExecutionLogDataHandler.Save(campaignExecutionLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            if (campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList != null && campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList.Any())
            {
                CampaignExecutionLogDetailListBL campaignExecutionLogDetailListBL = new CampaignExecutionLogDetailListBL(executionContext, campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList);
                campaignExecutionLogDetailListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }


        



        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CampaignExecutionLogDTO CampaignExecutionLogDTO
        {
            get
            {
                return campaignExecutionLogDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of CampaignExecutionLog
    /// </summary>
    /// 

    public class CampaignExecutionLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CampaignExecutionLogDTO> campaignExecutionLogDTOsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignExecutionLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="CampaignExecutionLogDTOsList"></param>
        /// <param name="executionContext"></param>
        public CampaignExecutionLogListBL(ExecutionContext executionContext, List<CampaignExecutionLogDTO> CampaignExecutionLogDTOsList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, CampaignExecutionLogDTOsList);
            this.campaignExecutionLogDTOsList = CampaignExecutionLogDTOsList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CampaignDefinition list
        /// </summary>
        public List<CampaignExecutionLogDTO> GetCampaignExecutionLogDTOList(List<KeyValuePair<CampaignExecutionLogDTO.SearchByCampaignExecutionLogParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            CampaignExecutionLogDataHandler campaignExecutionLogDataHandler = new CampaignExecutionLogDataHandler(sqlTransaction);
            List<CampaignExecutionLogDTO> campaignExecutionLogDTOsList = campaignExecutionLogDataHandler.GetCampaignExecutionLogList(searchParameters, sqlTransaction);
            if (campaignExecutionLogDTOsList != null && campaignExecutionLogDTOsList.Any() && loadChildRecords)
            {
                Build(campaignExecutionLogDTOsList, loadActiveChildRecords, sqlTransaction);

            }
            log.LogMethodExit(campaignExecutionLogDTOsList);
            return campaignExecutionLogDTOsList;
        }


        private void Build(List<CampaignExecutionLogDTO> campaignExecutionLogDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(campaignExecutionLogDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CampaignExecutionLogDTO> campaignExecutionLogDTOIdMap = new Dictionary<int, CampaignExecutionLogDTO>();
            List<int> campaignExecutionLogIdList = new List<int>();
            for (int i = 0; i < campaignExecutionLogDTOList.Count; i++)
            {
                if (campaignExecutionLogDTOIdMap.ContainsKey(campaignExecutionLogDTOList[i].CampaignExecutionLogId))
                {
                    continue;
                }
                campaignExecutionLogDTOIdMap.Add(campaignExecutionLogDTOList[i].CampaignExecutionLogId, campaignExecutionLogDTOList[i]);
                campaignExecutionLogIdList.Add(campaignExecutionLogDTOList[i].CampaignExecutionLogId);
            }


            CampaignExecutionLogDetailListBL campaignExecutionLogDetailListBL = new CampaignExecutionLogDetailListBL(executionContext);
            List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailDTOList = campaignExecutionLogDetailListBL.GetCampaignExecutionLogDetailDTOList(campaignExecutionLogIdList, activeChildRecords, sqlTransaction);
            if (campaignExecutionLogDetailDTOList != null && campaignExecutionLogDetailDTOList.Any())
            {
                for (int i = 0; i < campaignExecutionLogDetailDTOList.Count; i++)
                {
                    if (campaignExecutionLogDTOIdMap.ContainsKey(campaignExecutionLogDetailDTOList[i].CampaignExecutionLogId) == false)
                    {
                        continue;
                    }
                    CampaignExecutionLogDTO campaignExecutionLogDTO = campaignExecutionLogDTOIdMap[campaignExecutionLogDetailDTOList[i].CampaignExecutionLogId];
                    if (campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList == null)
                    {
                        campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList = new List<CampaignExecutionLogDetailDTO>();
                    }
                    campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList.Add(campaignExecutionLogDetailDTOList[i]);
                }
            }
        }


        /// <summary>
        /// Validates and saves the CampaignExecutionLogDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (campaignExecutionLogDTOsList == null ||
                campaignExecutionLogDTOsList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            CampaignExecutionLogDataHandler campaignExecutionLogDataHandler = new CampaignExecutionLogDataHandler(sqlTransaction);
            campaignExecutionLogDataHandler.Save(campaignExecutionLogDTOsList, executionContext.GetUserId(), executionContext.GetSiteId());
            foreach (CampaignExecutionLogDTO campaignExecutionLogDTO in campaignExecutionLogDTOsList)
            {
                if (campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList != null && campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList.Any())
                {
                    CampaignExecutionLogDetailListBL campaignExecutionLogDetailListBL = new CampaignExecutionLogDetailListBL(executionContext, campaignExecutionLogDTO.CampaignExecutionLogDetailDTOList);
                    campaignExecutionLogDetailListBL.Save(sqlTransaction);
                }
                log.LogMethodExit();
            }
        }
    }
}

