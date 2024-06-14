/********************************************************************************************
 * Project Name - Sequences BL
 * Description  - Business Logic for Sequences
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        17-Mar-2019   Jagan Mohana     Created 
              13-May-2019   Mushahid Faizan  Modified SaveUpdateSequencesList() try-Catch block.
2.90          11-May-2020   Girish Kundar    Modified : Changes as part of the REST API              
2.100.0       12-oct-2020   Deeksha          Added  Sequence number generation logic 
 *2.110.0     20-Feb-2020   Dakshakh Raj     Modified: Get Sequence method changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    public class SequencesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SequencesDTO sequencesDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        private SequencesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates SequencesBL object using the SequencesDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object</param>
        /// <param name="sequencesDTO">sequencesDTO object</param>
        public SequencesBL(ExecutionContext executionContext, SequencesDTO sequencesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, sequencesDTO);
            this.executionContext = executionContext;
            this.sequencesDTO = sequencesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get sequence DTO by Id
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="sequenceId"></param>
        /// <param name="sqlTransaction"></param>
        public SequencesBL(ExecutionContext executionContext, int sequenceId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, sequenceId, sqlTransaction);
            SequencesDataHandler sequencesDataHandler = new SequencesDataHandler(sqlTransaction);
            sequencesDTO = sequencesDataHandler.GetSequenceDTO(sequenceId);
            if (sequencesDTO == null)
            {
                string message = " Record Not found with id" + sequenceId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Get Sequence Number by Sequence Name 
        ///// </summary>
        ///// <param name="executionContext"></param>
        ///// <param name="sequencename"></param>
        ///// <param name="sqlTransaction"></param>
        //public SequencesBL(ExecutionContext executionContext , string sequencename, SqlTransaction sqlTransaction)
        //    :this(executionContext)
        //{
        //    log.LogMethodEntry(sequencename);
        //    SequencesDataHandler sequencesDataHandler = new SequencesDataHandler(sqlTransaction);
        //    sequenceNumber = sequencesDataHandler.GetNextSeqNo(sequencename);
        //    log.LogMethodExit(sequenceNumber);
        //}

        /// <summary>
        /// Validate DTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }
            /// <summary>
            /// Saves the Sequences
            /// Checks if the sequence id is not less than or equal to 0
            /// If it is less than or equal to 0, then inserts
            /// else updates
            /// </summary>
            public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (sequencesDTO.IsChanged == false
               && sequencesDTO.SequenceId > -1)
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
            SequencesDataHandler sequencesDataHandler = new SequencesDataHandler(sqlTransaction);
            if (sequencesDTO.SequenceId < 0)
            {
                sequencesDTO = sequencesDataHandler.InsertSequences(sequencesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                sequencesDTO.AcceptChanges();
                if (!string.IsNullOrEmpty(sequencesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("sequences", sequencesDTO.Guid, sqlTransaction);
                }
            }
            else
            {
                if (sequencesDTO.IsChanged)
                {
                    sequencesDTO = sequencesDataHandler.UpdateSequences(sequencesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    sequencesDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(sequencesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("sequences", sequencesDTO.Guid, sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets DTO
        /// </summary>
        public SequencesDTO GetSequencesDTO { get { return sequencesDTO; } }

        /// <summary>
        /// Get Next Sequence No
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public string GetNextSequenceNo(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            string sequenceNumber = string.Empty;
            SequencesDataHandler sequencesDataHandler = new SequencesDataHandler(sqlTrx);
            if (sequencesDTO != null)
            {
                sequenceNumber = sequencesDataHandler.GetNextSeqNo(this.sequencesDTO);
            }
            else
            {
                string message = "Sequence does not exist";
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            log.LogMethodExit(sequenceNumber);
            return sequenceNumber;
        }
    }

    /// <summary>
    /// List BL class for sequence
    /// </summary>
    public class SequencesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SequencesDTO> sequencesDTOsList = new List<SequencesDTO>();
        /// <summary>
        /// Default constructor
        /// </summary>
        public SequencesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="sequencesDTOsList"></param>
        /// <param name="executionContext"></param>
        public SequencesListBL(ExecutionContext executionContext, List<SequencesDTO> sequencesDTOsList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, sequencesDTOsList);
            this.sequencesDTOsList = sequencesDTOsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the sequencesDTO  matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of  sequencesDTO matching the search criteria</returns>
        public List<SequencesDTO> GetAllSequencesList(List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SequencesDataHandler sequencesDataHandler = new SequencesDataHandler(sqlTransaction);
            List<SequencesDTO> sequencesDTOList = sequencesDataHandler.GetAllSequencesList(searchParameters);
            log.LogMethodExit(sequencesDTOList);
            return sequencesDTOList;
        }

       
        /// <summary>
        /// This method should be used to Save and Update the Sequences details for Web Management Studio.
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();

            if (sequencesDTOsList != null && sequencesDTOsList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (SequencesDTO sequencesDTO in sequencesDTOsList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SequencesBL sequencesBL = new SequencesBL(executionContext, sequencesDTO);
                            sequencesBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
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
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
