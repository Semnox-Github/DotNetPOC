/********************************************************************************************
 * Project Name - Inventory
 * Description  - Business logic ReorderStockBL
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.70.3     29-Nov-2019      Girish Kundar        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    public class ReorderStockBL
    {
        private ReorderStockDTO reorderStockDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ReorderStockBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ReorderStockBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReorderStockBL id as the parameter
        /// Would fetch the ReorderStock object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public ReorderStockBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ReorderStockDataHandler reorderStockDataHandler = new ReorderStockDataHandler(sqlTransaction);
            reorderStockDTO = reorderStockDataHandler.GetReorderStockDTO(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ReorderStockBL object using the ReorderStockDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ReorderStockDTO">reorderStockDTO object</param>
        public ReorderStockBL(ExecutionContext executionContext, ReorderStockDTO reorderStockDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, reorderStockDTO);
            this.reorderStockDTO = reorderStockDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Generate reorderStockDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ReorderStockLineListBL reorderStockLineListBL = new ReorderStockLineListBL(executionContext);
            List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>(ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID, reorderStockDTO.ReorderStockId.ToString()));
            reorderStockDTO.ReorderStockLineDTOList = reorderStockLineListBL.GetReorderStockLineDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// get ReorderStockDTO Object
        /// </summary>
        public ReorderStockDTO GetReorderStockDTO
        {
            get { return reorderStockDTO; }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (reorderStockDTO == null)
            {
                //Validation to be implemented.
            }

            if (reorderStockDTO.ReorderStockLineDTOList != null)
            {
                foreach (var reorderStockLineDTO in reorderStockDTO.ReorderStockLineDTOList)
                {
                    if (reorderStockLineDTO.IsChanged)
                    {
                        ReorderStockLineBL reorderStockLineBL = new ReorderStockLineBL(executionContext, reorderStockLineDTO);
                        validationErrorList.AddRange(reorderStockLineBL.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the ReorderStock
        /// Checks if the MessagingTriggerId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reorderStockDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            ReorderStockDataHandler reorderStockDataHandler = new ReorderStockDataHandler(sqlTransaction);
            if (reorderStockDTO.ReorderStockId < 0)
            {
                reorderStockDTO = reorderStockDataHandler.Insert(reorderStockDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                reorderStockDTO.AcceptChanges();
            }
            else
            {
                if (reorderStockDTO.IsChanged)
                {
                    reorderStockDTO = reorderStockDataHandler.Update(reorderStockDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    reorderStockDTO.AcceptChanges();
                }
            }
            if (reorderStockDTO.ReorderStockLineDTOList != null && reorderStockDTO.ReorderStockLineDTOList.Count != 0)
            {
                foreach (ReorderStockLineDTO reorderStockLineDTO in reorderStockDTO.ReorderStockLineDTOList)
                {
                    if (reorderStockLineDTO.IsChanged)
                    {
                        reorderStockLineDTO.ReorderStockId = reorderStockDTO.ReorderStockId;
                        ReorderStockLineBL reorderStockLineBL = new ReorderStockLineBL(executionContext, reorderStockLineDTO);
                        reorderStockLineBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ReorderStockListBL
    /// </summary>
    public class ReorderStockListBL
    {
        private List<ReorderStockDTO> reorderStockDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ReorderStockListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.reorderStockDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="messagingTriggerDTOList"></param>
        /// <param name="executionContext"></param>
        public ReorderStockListBL(ExecutionContext executionContext, List<ReorderStockDTO> reorderStockDTOList)
        {
            log.LogMethodEntry(reorderStockDTOList, executionContext);
            this.executionContext = executionContext;
            this.reorderStockDTOList = reorderStockDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (reorderStockDTOList != null)
                {
                    foreach (ReorderStockDTO reorderStockDTO in reorderStockDTOList)
                    {
                        ReorderStockBL reorderStockBL = new ReorderStockBL(executionContext, reorderStockDTO);
                        reorderStockBL.Save(sqlTransaction);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the ReorderStock  List
        /// </summary>
        /// </summary>
        public List<ReorderStockDTO> GetReorderStockDTOList(List<KeyValuePair<ReorderStockDTO.SearchByParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool activeChildRecords = true,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ReorderStockDataHandler reorderStockDataHandler = new ReorderStockDataHandler(sqlTransaction);
            List<ReorderStockDTO> reorderStockDTOList = reorderStockDataHandler.GetReorderStockDTOList(searchParameters, sqlTransaction);
            if (reorderStockDTOList != null && reorderStockDTOList.Any() && loadChildRecords)
            {
                Build(reorderStockDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(reorderStockDTOList);
            return reorderStockDTOList;
        }

        private void Build(List<ReorderStockDTO> reorderStockDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reorderStockDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ReorderStockDTO> reorderStockIdDictionary = new Dictionary<int, ReorderStockDTO>();
            List<int> reorderStockIdSet = new List<int>();
            for (int i = 0; i < reorderStockDTOList.Count; i++)
            {
                if (reorderStockDTOList[i].ReorderStockId == -1 ||
                    reorderStockIdDictionary.ContainsKey(reorderStockDTOList[i].ReorderStockId))
                {
                    continue;
                }
                reorderStockIdSet.Add(reorderStockDTOList[i].ReorderStockId);
                reorderStockIdDictionary.Add(reorderStockDTOList[i].ReorderStockId, reorderStockDTOList[i]);
            }

            ReorderStockLineListBL reorderStockLineListBL = new ReorderStockLineListBL(executionContext);
            List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ReorderStockLineDTO.SearchByParameters, string>(ReorderStockLineDTO.SearchByParameters.REORDER_STOCK_ID_LIST, reorderStockIdSet.ToString()));
            List<ReorderStockLineDTO> reorderStockLineDTOList = reorderStockLineListBL.GetReorderStockLineDTOList(searchParameters, sqlTransaction);
            if (reorderStockLineDTOList.Any())
            {
                log.LogVariableState("reorderStockLineDTOList", reorderStockLineDTOList);
                foreach (var reorderStockLineDTO in reorderStockLineDTOList)
                {
                    if (reorderStockIdDictionary.ContainsKey(reorderStockLineDTO.ReorderStockId))
                    {
                        if (reorderStockIdDictionary[reorderStockLineDTO.ReorderStockId].ReorderStockLineDTOList == null)
                        {
                            reorderStockIdDictionary[reorderStockLineDTO.ReorderStockId].ReorderStockLineDTOList = new List<ReorderStockLineDTO>();
                        }
                        reorderStockIdDictionary[reorderStockLineDTO.ReorderStockId].ReorderStockLineDTOList.Add(reorderStockLineDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
