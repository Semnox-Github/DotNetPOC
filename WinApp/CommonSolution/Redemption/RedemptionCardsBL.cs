/********************************************************************************************
 * Project Name - RedemptionCards BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version        Date           Modified By           Remarks          
 *********************************************************************************************
 *2.3.0          12-July-2018   Archana               Created
 *2.4.0          01-Sep-2018    Archana               Modified to add RedemptionCardsListBL
 *2.70.2           19-Jul-2019    Deeksha               Modifications as per three tier standard.
 ********************************************************************************************/

using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System;
using System.Linq;

namespace Semnox.Parafait.Redemption
{

    /// <summary>
    /// RedemptionCardsBL
    /// </summary>
    public class RedemptionCardsBL 
    {
        private RedemptionCardsDTO redemptionCardsDTO;
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public RedemptionCardsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            machineUserContext = executionContext;
            redemptionCardsDTO = new RedemptionCardsDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemption card id as the parameter
        /// Would fetch the redemption object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">Id</param>
        public RedemptionCardsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction=null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            RedemptionCardsDataHandler redemptionCardsDataHandler = new RedemptionCardsDataHandler(sqlTransaction);
            redemptionCardsDTO = redemptionCardsDataHandler.GetRedemptionCardDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionCardsBL object using the RedemptionCardsDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionCardsDTO">RedemptionCardsDTO object</param>
        public RedemptionCardsBL(ExecutionContext executionContext, RedemptionCardsDTO redemptionCardsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionCardsDTO);
            this.redemptionCardsDTO = redemptionCardsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Redemption
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTrx">SqlTransaction</param>
        public void Save(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);           
            RedemptionCardsDataHandler redemptionCardsDataHandler = new RedemptionCardsDataHandler(sqlTrx);
            if (redemptionCardsDTO.RedemptionCardsId < 0)
            {
                redemptionCardsDTO = redemptionCardsDataHandler.InsertRedemptionCards(redemptionCardsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                redemptionCardsDTO.AcceptChanges();
            }
            else
            {
                if (redemptionCardsDTO.IsChanged)
                {
                    redemptionCardsDTO = redemptionCardsDataHandler.UpdateRedemptionCard(redemptionCardsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    redemptionCardsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Redemption Cards record - Hard Deletion
        /// </summary>
        /// <param name="redemptionCardsId"></param>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                RedemptionCardsDataHandler redemptionCardsDataHandler = new RedemptionCardsDataHandler(sqlTransaction);
                redemptionCardsDataHandler.Delete(redemptionCardsDTO.RedemptionCardsId);
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
        public RedemptionCardsDTO RedemptionCardsDTO
        {
            get
            {
                return redemptionCardsDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of redemption cards
    /// </summary>
    public class RedemptionCardsListBL
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext machineUserContext;
        private readonly List<RedemptionCardsDTO> redemptionCardsDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public RedemptionCardsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineUserContext = executionContext;
            log.LogMethodExit();
        }
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="redemptionGiftsDTOList">redemptionGiftsDTOList</param>
        public RedemptionCardsListBL(ExecutionContext executionContext, List<RedemptionCardsDTO> redemptionCardsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.redemptionCardsDTOList = redemptionCardsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RedemptionGiftsDTO list
        /// </summary>
        public List<RedemptionCardsDTO> GetRedemptionCardsDTOList(List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            RedemptionCardsDataHandler redemptionCardsDataHandler = new RedemptionCardsDataHandler(sqlTransaction);
            List<RedemptionCardsDTO> redemptionCardsDTOList = redemptionCardsDataHandler.GetRedemptionCardsDTOList(searchParameters);
            log.LogMethodExit(redemptionCardsDTOList);
            return redemptionCardsDTOList;
        }
        /// <summary>
        /// Validates and saves the RedemptionCardsDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (redemptionCardsDTOList == null ||
                !redemptionCardsDTOList.Any())
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            RedemptionCardsDataHandler redemptionCardsDataHandler = new RedemptionCardsDataHandler(sqlTrx); ;
            redemptionCardsDataHandler.Save(redemptionCardsDTOList, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}

