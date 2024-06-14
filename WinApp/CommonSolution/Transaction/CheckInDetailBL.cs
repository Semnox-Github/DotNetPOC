/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  CheckInDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        16-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  Business logic for CheckInDetail class.
    /// </summary>
    public class CheckInDetailBL
    {
        private CheckInDetailDTO checkInDetailDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CheckInDetailBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private CheckInDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CheckInDetailBL object using the CheckInDetailDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="checkInDetailDTO"></param>
        public CheckInDetailBL(ExecutionContext executionContext, CheckInDetailDTO checkInDetailDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInDetailDTO);
            this.checkInDetailDTO = checkInDetailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CheckInDetail id as the parameter
        /// Would fetch the CheckInDetailDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>

        public CheckInDetailBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CheckInDetailDataHandler checkInDetailDataHandler = new CheckInDetailDataHandler(sqlTransaction);
            checkInDetailDTO = checkInDetailDataHandler.GetCheckInDetailDTO(id);
            if (checkInDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CheckInDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                // throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CheckInDetail
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (checkInDetailDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }

            CheckInDetailDataHandler checkInDetailDataHandler = new CheckInDetailDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (checkInDetailDTO.CheckInDetailId < 0)
            {
                log.LogVariableState("CheckInDetailDTO", checkInDetailDTO);
                UpdateStatus();
                checkInDetailDTO = checkInDetailDataHandler.Insert(checkInDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInDetailDTO.AcceptChanges();
            }
            else if (checkInDetailDTO.IsChanged)
            {
                log.LogVariableState("CheckInDetailDTO", checkInDetailDTO);
                checkInDetailDTO = checkInDetailDataHandler.Update(checkInDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInDetailDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the CheckInDetailDTO 
        /// </summary>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CheckInDetailDataHandler checkInDetailDataHandler = new CheckInDetailDataHandler(sqlTransaction);
            if (checkInDetailDTO.CheckInDetailId == -1 && checkInDetailDTO.Status == CheckInStatus.PAUSED)
            {
                log.Error("Invalid CheckInStatus");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
            }
            if (checkInDetailDTO.CheckInDetailId == -1 && checkInDetailDTO.Status == CheckInStatus.CHECKEDOUT)
            {
                log.Error("Invalid CheckInStatus");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
            }
            if (checkInDetailDTO.CheckInDetailId > -1)
            {
                CheckInDetailDTO savedCheckInDetailDTO = checkInDetailDataHandler.GetCheckInDetailDTO(checkInDetailDTO.CheckInDetailId);
                CheckInBL checkInBL = new CheckInBL(executionContext, checkInDetailDTO.CheckInId, false, false, sqlTransaction);
                if (savedCheckInDetailDTO.Status != checkInDetailDTO.Status)
                {
                    if (checkInBL.IsValidCheckInStatus(savedCheckInDetailDTO.Status, checkInDetailDTO.Status) == false)
                    {
                        log.Error("Invalid CheckInStatus");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the CheckInDetailDTO 
        /// </summary>
        public void UpdateStatus()
        {
            log.LogMethodEntry();
            bool cardIssueMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS");
            if (checkInDetailDTO.CheckInDetailId == -1 && checkInDetailDTO.CardId > -1
                && checkInDetailDTO.Status != CheckInStatus.CHECKEDIN   )
            {
                checkInDetailDTO.Status = CheckInStatus.ORDERED;
            }
            //if (checkInDetailDTO.CheckInDetailId == -1 && checkInDetailDTO.CardId == -1 && cardIssueMandatory == false)
            //{
            //    checkInDetailDTO.Status = CheckInStatus.ORDERED;
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CheckInDetailDTO CheckInDetailDTO
        {
            get
            {
                return checkInDetailDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of checkInDetailDTO
    /// </summary>
    public class CheckInDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CheckInDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="checkInDetailDTOList"></param>
        public CheckInDetailListBL(ExecutionContext executionContext,
                                               List<CheckInDetailDTO> checkInDetailDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInDetailDTOList);
            this.checkInDetailDTOList = checkInDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CheckInDetailDTO
        /// </summary>
        /// <param name="checkInIdList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        internal List<CheckInDetailDTO> GetCheckInDetailDTOList(List<int> checkInIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(checkInIdList, sqlTransaction);
            CheckInDetailDataHandler checkInDetailDataHandler = new CheckInDetailDataHandler(sqlTransaction);
            List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailDataHandler.GetAllCheckInDetailDTOList(checkInIdList);
            log.LogMethodExit(checkInDetailDTOList);
            return checkInDetailDTOList;
        }

        /// <summary>
        /// Returns the CheckInDetailDTO List bases on search Parameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CheckInDetailDTO> GetCheckInDetailDTOList(List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CheckInDetailDataHandler checkInDetailDataHandler = new CheckInDetailDataHandler(sqlTransaction);
            List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailDataHandler.GetAllCheckInDetailDTOList(searchParameters);
            log.LogMethodExit(checkInDetailDTOList);
            return checkInDetailDTOList;
        }
        /// <summary>
        /// Saves the List of List of CheckInDetailDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInDetailDTOList == null ||
                checkInDetailDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < checkInDetailDTOList.Count; i++)
            {
                var checkInDetailDTO = checkInDetailDTOList[i];
                if (checkInDetailDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CheckInDetailBL checkInDetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO);
                    checkInDetailBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving CheckInDetailDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("CheckInDetailDTO", checkInDetailDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

