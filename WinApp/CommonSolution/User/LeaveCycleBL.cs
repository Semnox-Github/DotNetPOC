/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Holiday.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.70.0      15-Nov-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for LeaveTemplateBL
    /// </summary>
    public class LeaveCycleBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LeaveCycleDTO leaveCycleDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Paramertized Constructor of LeaveCycleBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LeaveCycleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LeaveCycleBL object using the LeaveCycleDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="leaveCycleDTO">leaveCycleDTO</param>
        public LeaveCycleBL(ExecutionContext executionContext, LeaveCycleDTO leaveCycleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveCycleDTO);
            this.leaveCycleDTO = leaveCycleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the leaveCycle id as the parameter
        /// Would fetch the leaveCycle object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="leaveCycleId">id of LeaveCycle Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LeaveCycleBL(ExecutionContext executionContext, int leaveCycleId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveCycleId, sqlTransaction);
            LeaveCycleDataHandler leaveCycleDataHandler = new LeaveCycleDataHandler(sqlTransaction);
            this.leaveCycleDTO = leaveCycleDataHandler.GetLeaveCycleDTO(leaveCycleId);
            if (leaveCycleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LeaveCycleDTO", leaveCycleId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the LeaveCycleDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Gets LeaveCycleDTO Object
        /// </summary>
        public LeaveCycleDTO GetLeaveCycleDTO
        {
            get { return leaveCycleDTO; }
        }

        /// <summary>
        /// Saves the LeaveCycleBL
        /// Checks if the LeaveCycleBL id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LeaveCycleDataHandler leaveCycleDataHandler = new LeaveCycleDataHandler(sqlTransaction);

            if (leaveCycleDTO.LeaveCycleId < 0)
            {
                leaveCycleDTO = leaveCycleDataHandler.Insert(leaveCycleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                leaveCycleDTO.AcceptChanges();
            }
            else
            {
                if (leaveCycleDTO.IsChanged)
                {
                    leaveCycleDTO = leaveCycleDataHandler.Update(leaveCycleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    leaveCycleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the LeaveCycleDTO based on LeaveCycleId
        /// </summary>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                LeaveCycleDataHandler leaveCycleDataHandler = new LeaveCycleDataHandler(sqlTransaction);
                leaveCycleDataHandler.Delete(leaveCycleDTO.LeaveCycleId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting LeaveTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Manages the list of LeaveCycleBL
    /// </summary>
    public class LeaveCycleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LeaveCycleDTO> leaveCycleDTOList = new List<LeaveCycleDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Paramterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public LeaveCycleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="leaveCycleDTOList">leaveCycleDTOList</param>
        public LeaveCycleListBL(ExecutionContext executionContext, List<LeaveCycleDTO> leaveCycleDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveCycleDTOList);
            this.leaveCycleDTOList = leaveCycleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LeaveCycleBL list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<LeaveCycleDTO> GetAllLeaveCycleList(List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LeaveCycleDataHandler leaveCycleDataHandler = new LeaveCycleDataHandler(sqlTransaction);
            List<LeaveCycleDTO> leaveCycleDTOList = leaveCycleDataHandler.GetLeaveCycleDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(leaveCycleDTOList);
            return leaveCycleDTOList;
        }

        /// <summary>
        /// Save the LeaveCycleBL List
        /// </summary>
        public List<LeaveCycleDTO> SaveUpdateLeaveCycle(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<LeaveCycleDTO> leaveCycleDTOLists = new List<LeaveCycleDTO>();
            if (leaveCycleDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LeaveCycleDTO leaveCycleDTO in leaveCycleDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LeaveCycleBL leaveCycleBL = new LeaveCycleBL(executionContext, leaveCycleDTO);
                            leaveCycleBL.Save(sqlTransaction);
                            leaveCycleDTOLists.Add(leaveCycleBL.GetLeaveCycleDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return leaveCycleDTOLists;
        }

        /// <summary>
        /// Delete the LeaveCycle
        /// <summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (leaveCycleDTOList != null && leaveCycleDTOList.Any())
            {
                foreach (LeaveCycleDTO leaveCycleDTO in leaveCycleDTOList)
                {
                    if (leaveCycleDTO.IsChanged && leaveCycleDTO.LeaveCycleId > -1)
                    {
                        try
                        {
                            LeaveCycleBL leaveCycleBL = new LeaveCycleBL(executionContext, leaveCycleDTO);
                            leaveCycleBL.Delete(sqlTransaction);
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
