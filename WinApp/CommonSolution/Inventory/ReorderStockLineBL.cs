/********************************************************************************************
 * Project Name - Communications
 * Description  - Business logic for ReorderStockLineBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2      16-Feb-2020      Girish Kundar       Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    public class ReorderStockLineBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ReorderStockLineDTO reorderStockLineDTO;
        private Utilities utilities = new Utilities();

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="Id">Id of the ReorderStockLine</param>
        public ReorderStockLineBL(ExecutionContext executionContext, int id)
        {
            log.LogMethodEntry();
            ReorderStockLineDataHandler reorderStockLineDataHandler = new ReorderStockLineDataHandler();
            reorderStockLineDTO = reorderStockLineDataHandler.GetReorderStockLineDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with reorderStockLineDTO parameter
        /// </summary>
        /// <param name="reorderStockLineDTO">parameter of type reorderStockLineDTO </param>
        public ReorderStockLineBL(ExecutionContext executionContext, ReorderStockLineDTO reorderStockLineDTO)
        {
            log.LogMethodEntry(reorderStockLineDTO);
            this.reorderStockLineDTO = reorderStockLineDTO;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the  station ReorderStockLine details to table
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ReorderStockLineDataHandler reorderStockLineDataHandler = new ReorderStockLineDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (reorderStockLineDTO.ReorderStockLineId < 0)
            {
                reorderStockLineDTO = reorderStockLineDataHandler.Insert(reorderStockLineDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                reorderStockLineDTO.AcceptChanges();
            }
            else
            {
                if (reorderStockLineDTO.IsChanged)
                {
                    reorderStockLineDTO = reorderStockLineDataHandler.Update(reorderStockLineDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    reorderStockLineDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the reorderStockLineDTO .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }

        /// <summary>
        /// Gets the ReorderStockLineDTO
        /// </summary>
        public ReorderStockLineDTO ReorderStockLineDTO
        {
            get { return reorderStockLineDTO; }
        }

    }
    /// <summary>
    /// Class for reorderStockLine BL List
    /// </summary>
    public class  ReorderStockLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ReorderStockLineDTO> reorderStockLineDTOList = new List<ReorderStockLineDTO>();
        private ExecutionContext executionContext;
        public ReorderStockLineListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and reorderStockLineDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="reorderStockLineDTOList">reorderStockLineDTOList</param>
        public ReorderStockLineListBL(ExecutionContext executionContext, List<ReorderStockLineDTO> reorderStockLineDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.reorderStockLineDTOList = reorderStockLineDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns All the reorderStockLineDTOList records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReorderStockLineDTO</returns>
        public List<ReorderStockLineDTO> GetReorderStockLineDTOList(List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReorderStockLineDataHandler reorderStockLineDataHandler = new ReorderStockLineDataHandler(sqlTransaction);
            List<ReorderStockLineDTO> reorderStockLineDTOList = reorderStockLineDataHandler.GetReorderStockLineDTOList(searchParameters,sqlTransaction);
            log.LogMethodExit(reorderStockLineDTOList);
            return reorderStockLineDTOList;
        }

        /// <summary>
        /// Save and Update reorderStockLine DTO list  Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (reorderStockLineDTOList != null)
                {
                    foreach (ReorderStockLineDTO reorderStockLineDTO in reorderStockLineDTOList)
                    {
                        ReorderStockLineBL reorderStockLineBL = new ReorderStockLineBL(executionContext, reorderStockLineDTO);
                        reorderStockLineBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
