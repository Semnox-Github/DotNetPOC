/********************************************************************************************
 * Project Name - CCTransactionsPGW BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jun-2017      Lakshminarayana     Created 
 *2.70.2        09-Jul-2019      Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                            LogMethodEntry() and LogMethodExit(). 
 *2.70.2        04-Feb-2020      Nitin Pai           Guest App phase 2 changes                                                            
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Business logic for CCTransactionsPGW class.
    /// </summary>
    public class CCTransactionsPGWBL
    {
        private CCTransactionsPGWDTO cCTransactionsPGWDTO;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CCTransactionsPGWBL class
        /// </summary>
        public CCTransactionsPGWBL()
        {
            log.LogMethodEntry();
            cCTransactionsPGWDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the cCTransactionsPGW id as the parameter
        /// Would fetch the cCTransactionsPGW object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param> 
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CCTransactionsPGWBL(int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(id, sqlTransaction);
            CCTransactionsPGWDataHandler cCTransactionsPGWDataHandler = new CCTransactionsPGWDataHandler(sqlTransaction);
            cCTransactionsPGWDTO = cCTransactionsPGWDataHandler.GetCCTransactionsPGWDTO(id);
            log.LogMethodEntry();
        }

        /// <summary>
        /// Creates CCTransactionsPGWBL object using the CCTransactionsPGWDTO
        /// </summary>
        /// <param name="cCTransactionsPGWDTO">CCTransactionsPGWDTO object</param>
        public CCTransactionsPGWBL(CCTransactionsPGWDTO cCTransactionsPGWDTO, ExecutionContext executionContext = null)
            : this()
        {
            log.LogMethodEntry(cCTransactionsPGWDTO);
            this.cCTransactionsPGWDTO = cCTransactionsPGWDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CCTransactionsPGW
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            Semnox.Core.Utilities.ExecutionContext machineUserContext = executionContext == null ? Semnox.Core.Utilities.ExecutionContext.GetExecutionContext() : executionContext;
            CCTransactionsPGWDataHandler cCTransactionsPGWDataHandler = new CCTransactionsPGWDataHandler(sqlTransaction);
            if(cCTransactionsPGWDTO.ResponseID < 0)
            {
                cCTransactionsPGWDTO  = cCTransactionsPGWDataHandler.InsertCCTransactionsPGW(cCTransactionsPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                cCTransactionsPGWDTO.AcceptChanges();
            }
            else
            {
                if(cCTransactionsPGWDTO.IsChanged)
                {
                    cCTransactionsPGWDTO = cCTransactionsPGWDataHandler.UpdateCCTransactionsPGW(cCTransactionsPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cCTransactionsPGWDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CCTransactionsPGWDTO CCTransactionsPGWDTO
        {
            get
            {
                return cCTransactionsPGWDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CCTransactionsPGW
    /// </summary>
    public class CCTransactionsPGWListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the CCTransactionsPGW list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of CCTransactionsPGWDTO</returns>
        public List<CCTransactionsPGWDTO> GetCCTransactionsPGWDTOList(List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CCTransactionsPGWDataHandler cCTransactionsPGWDataHandler = new CCTransactionsPGWDataHandler(sqlTransaction);
            List<CCTransactionsPGWDTO> ccTransactionsPGWDTOList = cCTransactionsPGWDataHandler.GetCCTransactionsPGWDTOList(searchParameters);
            log.LogMethodExit(ccTransactionsPGWDTOList);
            return ccTransactionsPGWDTOList;
        }

        /// <summary>
        /// Returns the CCTransactionsPGW list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of CCTransactionsPGWDTO</returns>
        public List<CCTransactionsPGWDTO> GetNonReversedCCTransactionsPGWDTOList(List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CCTransactionsPGWDataHandler cCTransactionsPGWDataHandler = new CCTransactionsPGWDataHandler(sqlTransaction);
            List<CCTransactionsPGWDTO> ccTransactionsPGWDTOList = cCTransactionsPGWDataHandler.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            log.LogMethodExit(ccTransactionsPGWDTOList);
            return ccTransactionsPGWDTOList;
        }

    }
}
