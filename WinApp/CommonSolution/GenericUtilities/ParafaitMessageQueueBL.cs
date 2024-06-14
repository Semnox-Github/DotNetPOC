/********************************************************************************************
 * Project Name - GenericUtilities                                                                       
 * Description  - ParafaitMessageQueueBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
  *2.110.0     07-Feb-2022   Fiona Lishal     Updated : Urban Piper changes- Added Remarks, Attempts, added GetParafaitMessageQueues
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// ParafaitMessageQueueBL
    /// </summary>
    public class ParafaitMessageQueueBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitMessageQueueDTO parafaitMessageQueueDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private ParafaitMessageQueueBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterParafaitMessageQueueDTO">parameterParafaitMessageQueueDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitMessageQueueBL(ExecutionContext executionContext, ParafaitMessageQueueDTO parameterParafaitMessageQueueDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterParafaitMessageQueueDTO, sqlTransaction);

            if (parameterParafaitMessageQueueDTO.MessageQueueId > -1)
            {
                LoadParafaitMessageQueueDTO(parameterParafaitMessageQueueDTO.MessageQueueId, sqlTransaction);//added sql
                ThrowIfDTOIsNull(parameterParafaitMessageQueueDTO.MessageQueueId);
                Update(parameterParafaitMessageQueueDTO);
            }
            else
            {
                Validate(sqlTransaction);
                parafaitMessageQueueDTO = new ParafaitMessageQueueDTO(-1, parameterParafaitMessageQueueDTO.EntityGuid, parameterParafaitMessageQueueDTO.EntityName,
                                                                                         parameterParafaitMessageQueueDTO.Message, parameterParafaitMessageQueueDTO.Status,
                                                                          parameterParafaitMessageQueueDTO.IsActive, parameterParafaitMessageQueueDTO.ActionType, parameterParafaitMessageQueueDTO.Remarks, parameterParafaitMessageQueueDTO.Attempts);
            }
            log.LogMethodExit();
        }
        private void ThrowIfDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (parafaitMessageQueueDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ParafaitMessageQueue", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadParafaitMessageQueueDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ParafaitMessageQueueDataHandler parafaitMessageQueueDataHandler = new ParafaitMessageQueueDataHandler(sqlTransaction);
            parafaitMessageQueueDTO = parafaitMessageQueueDataHandler.GetParafaitMessageQueueDTO(id);
            ThrowIfDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(ParafaitMessageQueueDTO parameterParafaitMessageQueueDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterParafaitMessageQueueDTO);
            parafaitMessageQueueDTO.MessageQueueId = parameterParafaitMessageQueueDTO.MessageQueueId;
            parafaitMessageQueueDTO.EntityGuid = parameterParafaitMessageQueueDTO.EntityGuid;
            parafaitMessageQueueDTO.EntityName = parameterParafaitMessageQueueDTO.EntityName;
            parafaitMessageQueueDTO.Message = parameterParafaitMessageQueueDTO.Message;
            parafaitMessageQueueDTO.Status = parameterParafaitMessageQueueDTO.Status;
            parafaitMessageQueueDTO.IsActive = parameterParafaitMessageQueueDTO.IsActive;
            parafaitMessageQueueDTO.Remarks = parameterParafaitMessageQueueDTO.Remarks;
            parafaitMessageQueueDTO.Attempts = parameterParafaitMessageQueueDTO.Attempts;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }

        /// <summary>
        /// ParafaitMessageQueueBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public ParafaitMessageQueueBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadParafaitMessageQueueDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction);
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitMessageQueueDataHandler parafaitMessageQueueDataHandler = new ParafaitMessageQueueDataHandler(sqlTransaction);
            if (parafaitMessageQueueDTO.MessageQueueId < 0)
            {
                parafaitMessageQueueDTO = parafaitMessageQueueDataHandler.Insert(parafaitMessageQueueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parafaitMessageQueueDTO.AcceptChanges();
            }
            else
            {
                if (parafaitMessageQueueDTO.IsChanged)
                {
                    parafaitMessageQueueDTO = parafaitMessageQueueDataHandler.UpdateParafaitMessageQueueDTO(parafaitMessageQueueDTO);
                    parafaitMessageQueueDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get ParafaitMessageQueueDTO Object
        /// </summary>
        public ParafaitMessageQueueDTO ParafaitMessageQueueDTO
        {
            get
            {
                ParafaitMessageQueueDTO result = new ParafaitMessageQueueDTO(parafaitMessageQueueDTO);
                return result;
            }
        }

    }

    /// <summary>
    /// ParafaitMessageQueueListBL list class for order details
    /// </summary>
    public class ParafaitMessageQueueListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList;

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ParafaitMessageQueueListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitMessageQueueDTOList"></param>
        public ParafaitMessageQueueListBL(ExecutionContext executionContext, List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList)
        {
            log.LogMethodEntry(executionContext, parafaitMessageQueueDTOList);
            this.parafaitMessageQueueDTOList = parafaitMessageQueueDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ParafaitMessageQueueDTO> GetParafaitMessageQueues(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitMessageQueueDataHandler parafaitMessageQueueDataHandler = new ParafaitMessageQueueDataHandler(sqlTransaction);
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueDataHandler.GetParafaitMessageQueues(searchParameters);
            log.LogMethodExit(parafaitMessageQueueDTOList);
            return parafaitMessageQueueDTOList;
        }
        public List<ParafaitMessageQueueDTO> GetParafaitMessageQueues(List<string> entityGuidList, List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(entityGuidList, searchParameters, sqlTransaction);
            ParafaitMessageQueueDataHandler parafaitMessageQueueDataHandler = new ParafaitMessageQueueDataHandler(sqlTransaction);
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueDataHandler.GetParafaitMessageQueueDTOList(entityGuidList, searchParameters);
            log.LogMethodExit(parafaitMessageQueueDTOList);
            return parafaitMessageQueueDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ParafaitMessageQueueDTO> Save()
        {
            log.LogMethodEntry();
            List<ParafaitMessageQueueDTO> savedParafaitMessageQueueDTOList = new List<ParafaitMessageQueueDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (parafaitMessageQueueDTOList != null && parafaitMessageQueueDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (ParafaitMessageQueueDTO parafaitMessageQueueDTO in parafaitMessageQueueDTOList)
                        {
                            ParafaitMessageQueueBL parafaitMessageQueueBL = new ParafaitMessageQueueBL(executionContext, parafaitMessageQueueDTO);
                            parafaitMessageQueueBL.Save(parafaitDBTrx.SQLTrx);
                            savedParafaitMessageQueueDTOList.Add(parafaitMessageQueueBL.ParafaitMessageQueueDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }

            }
            log.LogMethodExit(savedParafaitMessageQueueDTOList);
            return savedParafaitMessageQueueDTOList;
        }
    }
}
