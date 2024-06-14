/********************************************************************************************
 * Project Name - RedemptionGifts BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.30        12-July-2018     Archana             Created
 *2.70.2        19-Jul-2019      Deeksha             Modifications as per three tier standard.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for RedemptionGifts class.
    /// </summary>
    public class RedemptionGiftsBL
    {
        private RedemptionGiftsDTO redemptionGiftsDTO;
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;

        /// <summary>
        /// Default constructor of RedemptionGiftsBL class
        /// <param name="executionContext">ExecutionContext</param>
        /// </summary>
        public RedemptionGiftsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            machineUserContext = executionContext;
            redemptionGiftsDTO = new RedemptionGiftsDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemptionGifts id as the parameter
        /// Would fetch the redemptionGifts object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <param name="id">Id</param>
        public RedemptionGiftsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTrx = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTrx);
            RedemptionGiftsDataHandler redemptionGiftsDataHandler = new RedemptionGiftsDataHandler(sqlTrx);
            redemptionGiftsDTO = redemptionGiftsDataHandler.GetRedemptionGiftsDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionGiftsBL object using the RedemptionGiftsDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionGiftsDTO">RedemptionGiftsDTO object</param>
        public RedemptionGiftsBL(ExecutionContext executionContext, RedemptionGiftsDTO redemptionGiftsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionGiftsDTO);
            this.redemptionGiftsDTO = redemptionGiftsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the redemption gifts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTrx">SqlTransaction</param>
        public void Save(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            RedemptionGiftsDataHandler redemptionGiftsDataHandler = new RedemptionGiftsDataHandler(sqlTrx);
            if (redemptionGiftsDTO.RedemptionGiftsId < 0)
            {
                redemptionGiftsDTO = redemptionGiftsDataHandler.InsertRedemptionGifts(redemptionGiftsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                redemptionGiftsDTO.AcceptChanges();
            }
            else
            {
                if (redemptionGiftsDTO.IsChanged)
                {
                    redemptionGiftsDTO = redemptionGiftsDataHandler.UpdateRedemptionGifts(redemptionGiftsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    redemptionGiftsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Redemption Gifts record - Hard Deletion
        /// </summary>
        /// <param name="redemptionGiftsId"></param>
        /// <param name="sqlTransaction"></param>
        public void Delete( SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                RedemptionGiftsDataHandler redemptionGiftsDataHandler = new RedemptionGiftsDataHandler(sqlTransaction);
                redemptionGiftsDataHandler.Delete(redemptionGiftsDTO.RedemptionGiftsId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Get method for DTO
        /// </summary>
        public RedemptionGiftsDTO RedemptionGiftsDTO
        {
            get
            {
                return redemptionGiftsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of redemption gifts
    /// </summary>
    public class RedemptionGiftsListBL
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;
        private readonly List<RedemptionGiftsDTO> redemptionGiftsDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public RedemptionGiftsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineUserContext = executionContext;
            log.LogMethodExit();
        }
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionGiftsDTOList">redemptionGiftsDTOList</param>
        public RedemptionGiftsListBL(ExecutionContext executionContext, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionGiftsDTOList = redemptionGiftsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RedemptionGiftsDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>redemptionGiftsDTOList</returns>
        public List<RedemptionGiftsDTO> GetRedemptionGiftsDTOList(List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParameters, sqlTrx);
            RedemptionGiftsDataHandler redemptionGiftsDataHandler = new RedemptionGiftsDataHandler(sqlTrx);
            List<RedemptionGiftsDTO> redemptionGiftsDTOList = redemptionGiftsDataHandler.GetRedemptionGiftsDTOList(searchParameters);
            log.LogMethodExit(redemptionGiftsDTOList);
            return redemptionGiftsDTOList;
        }
        /// <summary>
        /// Validates and saves the RedemptionGiftsDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (redemptionGiftsDTOList == null ||
                !redemptionGiftsDTOList.Any())
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            RedemptionGiftsDataHandler redemptionGiftsDataHandler = new RedemptionGiftsDataHandler(sqlTrx); ;
            redemptionGiftsDataHandler.Save(redemptionGiftsDTOList, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}
