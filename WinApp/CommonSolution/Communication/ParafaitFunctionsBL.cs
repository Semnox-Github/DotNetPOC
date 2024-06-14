/********************************************************************************************
 * Project Name - ParafaitFunctionsBL 
 * Description  -BL class of the ParafaitFunctions 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Communication
{
    public class ParafaitFunctionsBL
    {
        private ParafaitFunctionsDTO parafaitFunctionsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor of ParafaitFunctionsBL class
        /// </summary>
        private ParafaitFunctionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            parafaitFunctionsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ParafaitFunctionsBL object using the ParafaitFunctionsDTO
        /// </summary>
        /// <param name="parafaitFunctionsDTO">ParafaitFunctionsDTO object</param>
        public ParafaitFunctionsBL(ExecutionContext executionContext, ParafaitFunctionsDTO parafaitFunctionsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(parafaitFunctionsDTO);
            this.parafaitFunctionsDTO = parafaitFunctionsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the ParafaitFunctions id as the parameter
        /// Would fetch the ParafaitFunctions object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public ParafaitFunctionsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,id, sqlTransaction);
            ParafaitFunctionsDataHandler parafaitFunctionsDataHandler = new ParafaitFunctionsDataHandler(sqlTransaction);
            parafaitFunctionsDTO = parafaitFunctionsDataHandler.GetParafaitFunctionsDTO(id);
            if (parafaitFunctionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ParafaitFunctions", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            BuildChildDTO(sqlTransaction);
            log.LogMethodExit(parafaitFunctionsDTO);
        }
        /// <summary>
        /// Constructor with the ParafaitFunctions id as the parameter
        /// Would fetch the ParafaitFunctions object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public ParafaitFunctionsBL(ExecutionContext executionContext, ParafaitFunctions parafaitFunctionsName, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitFunctionsName, sqlTransaction);
            ParafaitFunctionsDataHandler parafaitFunctionsDataHandler = new ParafaitFunctionsDataHandler(sqlTransaction);
            parafaitFunctionsDTO = parafaitFunctionsDataHandler.GetParafaitFunctionsDTO(parafaitFunctionsName);
            if (parafaitFunctionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ParafaitFunctions", parafaitFunctionsName.ToString());
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            BuildChildDTO(sqlTransaction);
            log.LogMethodExit(parafaitFunctionsDTO);
        }
        ///// <summary>
        ///// Saves the ParafaitFunctions DTO
        ///// Checks if the  id is not less than or equal to 0
        ///// If it is less than or equal to 0, then inserts
        ///// else updates
        ///// </summary>
        ///// <param name="sqlTransaction"></param>
        //public void Save(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(sqlTransaction);
        //    ParafaitFunctionsDataHandler ParafaitFunctionsDataHandler = new ParafaitFunctionsDataHandler(sqlTransaction);
        //    if (parafaitFunctionsDTO.IsChanged == false
        //         && parafaitFunctionsDTO.ParafaitFunctionId > -1)
        //    {
        //        log.LogMethodExit(null, "Nothing to save.");
        //        return;
        //    }
        //    List<ValidationError> validationErrors = Validate();
        //    if (validationErrors.Any())
        //    {
        //        string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
        //        log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
        //        throw new ValidationException(message, validationErrors);
        //    }
        //    if (parafaitFunctionsDTO.ParafaitFunctionId < 0)
        //    {
        //        parafaitFunctionsDTO = ParafaitFunctionsDataHandler.Insert(parafaitFunctionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //        parafaitFunctionsDTO.AcceptChanges();
        //    }
        //    else
        //    {
        //        if (parafaitFunctionsDTO.IsChanged)
        //        {
        //            parafaitFunctionsDTO = ParafaitFunctionsDataHandler.Update(parafaitFunctionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //            parafaitFunctionsDTO.AcceptChanges();
        //        }
        //    }
        //    if (parafaitFunctionsDTO.IsChangedRecursive && parafaitFunctionsDTO.ParafaitFunctionEventDTOList != null && parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Any())
        //    {
        //        List<ParafaitFunctionEventDTO> updateParafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();
        //        for (int i = 0; i < parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Count; i++)
        //        {
        //            if (parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].ParafaitFunctionId != parafaitFunctionsDTO.ParafaitFunctionId)
        //            {
        //                parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].ParafaitFunctionId = parafaitFunctionsDTO.ParafaitFunctionId;
        //            }
        //            if (parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].IsChanged || parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].ParafaitFunctionEventId < 0)
        //            {
        //                updateParafaitFunctionEventDTOList.Add(parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i]);
        //            }
        //        }
        //        if (updateParafaitFunctionEventDTOList != null && updateParafaitFunctionEventDTOList.Any())
        //        {
        //            ParafaitFunctionEventListBL parafaitFunctionEventListBL = new ParafaitFunctionEventListBL(executionContext, updateParafaitFunctionEventDTOList);
        //            parafaitFunctionEventListBL.Save(sqlTransaction);
        //        }
        //    } 
        //    log.LogMethodExit();
        //}
        ///// <summary>
        ///// Validate the parafaitFunctionsDTO
        ///// </summary>
        ///// <returns></returns>
        //public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry();
        //    List<ValidationError> validationErrorList = new List<ValidationError>();
        //    if (string.IsNullOrWhiteSpace(parafaitFunctionsDTO.ParafaitFunctionName))
        //    {
        //        validationErrorList.Add(new ValidationError("ParafaitFunctions", "ParafaitFunctionName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Parafait Function Name"))));
        //    }
        //    if (parafaitFunctionsDTO.IsChangedRecursive && parafaitFunctionsDTO.ParafaitFunctionEventDTOList != null && parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Any())
        //    {
        //        List<ParafaitFunctionEventDTO> updateParafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();
        //        for (int i = 0; i < parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Count; i++)
        //        { 
        //            if (parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].IsChanged || parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i].ParafaitFunctionEventId < 0)
        //            {
        //                ParafaitFunctionEventBL parafaitFunctionEventBL = new ParafaitFunctionEventBL(executionContext, parafaitFunctionsDTO.ParafaitFunctionEventDTOList[i]);
        //                List<ValidationError> ChildValidationErrorList = new List<ValidationError>();
        //                ChildValidationErrorList = parafaitFunctionEventBL.Validate(sqlTransaction);
        //                if (ChildValidationErrorList != null && ChildValidationErrorList.Any())
        //                {
        //                    validationErrorList.AddRange(ChildValidationErrorList);
        //                }
        //            }
        //        } 
        //    }
        //    log.LogMethodExit(validationErrorList);
        //    return validationErrorList;
        //}
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParafaitFunctionsDTO ParafaitFunctionsDTO
        {
            get
            {
                return parafaitFunctionsDTO;
            }
        }
        private void BuildChildDTO(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitFunctionEventListBL parafaitFunctionEventListBL = new ParafaitFunctionEventListBL(executionContext);
            List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_ID, parafaitFunctionsDTO.ParafaitFunctionId.ToString()));
            searchParameters.Add(new KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>(ParafaitFunctionEventDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            parafaitFunctionsDTO.ParafaitFunctionEventDTOList = parafaitFunctionEventListBL.GetAllParafaitFunctionEventDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }
    }
    public class ParafaitFunctionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ParafaitFunctionsDTO> parafaitFunctionsDTOList = new List<ParafaitFunctionsDTO>();

        public ParafaitFunctionsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parafaitFunctionsDTOList">parafaitFunctionsDTOList</param>
        public ParafaitFunctionsListBL(ExecutionContext executionContext,
                                             List<ParafaitFunctionsDTO> parafaitFunctionsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.parafaitFunctionsDTOList = parafaitFunctionsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParafaitFunctionsDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildren"></param>
        /// <param name="loadActiveChildren"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ParafaitFunctionsDTO> GetAllParafaitFunctionsDTOList(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> searchParameters, 
                                                                         bool loadChildren = false, bool loadActiveChildren = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,  sqlTransaction);
            ParafaitFunctionsDataHandler parafaitFunctionsDataHandler = new ParafaitFunctionsDataHandler(sqlTransaction);
            parafaitFunctionsDTOList = parafaitFunctionsDataHandler.GetParafaitFunctionsDTOList(searchParameters);
            if (loadChildren && parafaitFunctionsDTOList != null && parafaitFunctionsDTOList.Any())
            {
                LoadChildren(loadActiveChildren, sqlTransaction);
            }
            log.LogMethodExit(parafaitFunctionsDTOList);
            return parafaitFunctionsDTOList;
        }

        private void LoadChildren(bool loadActiveChildren, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            Dictionary<int, ParafaitFunctionsDTO> parafaitFunctionsDTOIdMap = new Dictionary<int, ParafaitFunctionsDTO>();
            List<int> parafaitFunctionsIdList = new List<int>();
            for (int i = 0; i < parafaitFunctionsDTOList.Count; i++)
            {
                if (parafaitFunctionsDTOIdMap.ContainsKey(parafaitFunctionsDTOList[i].ParafaitFunctionId))
                {
                    continue;
                }
                parafaitFunctionsDTOIdMap.Add(parafaitFunctionsDTOList[i].ParafaitFunctionId, parafaitFunctionsDTOList[i]);
                parafaitFunctionsIdList.Add(parafaitFunctionsDTOList[i].ParafaitFunctionId);
            }
            ParafaitFunctionEventListBL parafaitFunctionEventListBL = new ParafaitFunctionEventListBL(executionContext);
            List<ParafaitFunctionEventDTO> parafaitFunctionEventDTOList = parafaitFunctionEventListBL.GetAllParafaitFunctionEventDTOList(parafaitFunctionsIdList, loadActiveChildren, sqlTransaction);
            if (parafaitFunctionEventDTOList != null && parafaitFunctionEventDTOList.Any())
            {
                for (int i = 0; i < parafaitFunctionEventDTOList.Count; i++)
                {
                    if (parafaitFunctionsDTOIdMap.ContainsKey(parafaitFunctionEventDTOList[i].ParafaitFunctionId) == false)
                    {
                        continue;
                    }
                    ParafaitFunctionsDTO parafaitFunctionsDTO = parafaitFunctionsDTOIdMap[parafaitFunctionEventDTOList[i].ParafaitFunctionId];
                    if (parafaitFunctionsDTO.ParafaitFunctionEventDTOList == null)
                    {
                        parafaitFunctionsDTO.ParafaitFunctionEventDTOList = new List<ParafaitFunctionEventDTO>();
                    }
                    parafaitFunctionsDTO.ParafaitFunctionEventDTOList.Add(parafaitFunctionEventDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Saves the  list of ParafaitFunctionsDTOList DTO.
        ///// </summary>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        //public void Save(SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(sqlTransaction);
        //    if (parafaitFunctionsDTOList == null ||
        //        parafaitFunctionsDTOList.Count > 0 == false)
        //    {
        //        log.LogMethodExit(null, "List is empty");
        //        return;
        //    }

        //    for (int i = 0; i < parafaitFunctionsDTOList.Count; i++)
        //    {
        //        ParafaitFunctionsDTO parafaitFunctionsDTO = parafaitFunctionsDTOList[i];
        //        if (parafaitFunctionsDTO.IsChanged == false)
        //        {
        //            continue;
        //        }
        //        try
        //        {
        //            ParafaitFunctionsBL parafaitFunctionsBL = new ParafaitFunctionsBL(executionContext, parafaitFunctionsDTO);
        //            parafaitFunctionsBL.Save(sqlTransaction);
        //        }
        //        catch (SqlException ex)
        //        {
        //            log.Error(ex);
        //            if (ex.Number == 2601)
        //            {
        //                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
        //            }
        //            else if (ex.Number == 547)
        //            {
        //                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
        //            }
        //            else
        //            {
        //                throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex);
        //            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
        //            throw;
        //        }
        //    }
        //}

    }
}
