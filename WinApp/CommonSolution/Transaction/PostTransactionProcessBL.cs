/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  PostTransactionProcess
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      16-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for PostTransactionProcess class.
    /// </summary>
    public class PostTransactionProcessBL
    {
        private PostTransactionProcessDTO postTransactionProcessDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PostTransactionProcessBL class
        /// </summary>
        private PostTransactionProcessBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates PostTransactionProcessBL object using the PostTransactionProcessDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="postTransactionProcessDTO">PostTransactionProcessDTO object</param>
        public PostTransactionProcessBL(ExecutionContext executionContext, PostTransactionProcessDTO postTransactionProcessDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, postTransactionProcessDTO);
            this.postTransactionProcessDTO = postTransactionProcessDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PostTransactionProcess id as the parameter
        /// Would fetch the PostTransactionProcess object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of PostTransactionProcess passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public PostTransactionProcessBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PostTransactionProcessDataHandler postTransactionProcessDataHandler = new PostTransactionProcessDataHandler(sqlTransaction);
            postTransactionProcessDTO = postTransactionProcessDataHandler.GetPostTransactionProcessDTO(id);
            if (postTransactionProcessDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "PostTransactionProcess", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PostTransactionProcessDTO
        /// Checks if the  id is not less than  0
        /// If it is less than 0, then inserts else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object to be passed</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (postTransactionProcessDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            PostTransactionProcessDataHandler postTransactionProcessDataHandler = new PostTransactionProcessDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (postTransactionProcessDTO.Id < 0)
            {
                log.LogVariableState("PostTransactionProcessDTO", postTransactionProcessDTO);
                postTransactionProcessDTO = postTransactionProcessDataHandler.Insert(postTransactionProcessDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                postTransactionProcessDTO.AcceptChanges();
            }
            else if (postTransactionProcessDTO.IsChanged)
            {
                log.LogVariableState("PostTransactionProcessDTO", postTransactionProcessDTO);
                postTransactionProcessDTO = postTransactionProcessDataHandler.Update(postTransactionProcessDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                postTransactionProcessDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the PostTransactionProcessDTO  values 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (true) // Fields to be validated here.
            {
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PostTransactionProcessDTO PostTransactionProcessDTO
        {
            get
            {
                return postTransactionProcessDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of PostTransactionProcessDTO
    /// </summary>
    public class PostTransactionProcessListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PostTransactionProcessDTO> postTransactionProcessDTOList = new List<PostTransactionProcessDTO>();
        /// <summary>
        /// Parameterized constructor for PostTransactionProcessListBL
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        public PostTransactionProcessListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for PostTransactionProcessListBL with postTransactionProcessDTOList
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="postTransactionProcessDTOList">PostTransactionProcessDTO List object is passed as parameter</param>
        public PostTransactionProcessListBL(ExecutionContext executionContext,
                                                List<PostTransactionProcessDTO> postTransactionProcessDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, postTransactionProcessDTOList);
            this.postTransactionProcessDTOList = postTransactionProcessDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the PostTransactionProcessDTO List
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the List of PostTransactionProcessDTO</returns>
        public List<PostTransactionProcessDTO> GetPostTransactionProcessDTOList(List<KeyValuePair<PostTransactionProcessDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PostTransactionProcessDataHandler postTransactionProcessDataHandler = new PostTransactionProcessDataHandler(sqlTransaction);
            List<PostTransactionProcessDTO> postTransactionProcessDTOList = postTransactionProcessDataHandler.GetPostTransactionProcessDTOList(searchParameters);
            log.LogMethodExit(postTransactionProcessDTOList);
            return postTransactionProcessDTOList;
        }

        /// <summary>
        /// Saves the  List of PostTransactionProcessDTO objects
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (postTransactionProcessDTOList == null ||
                postTransactionProcessDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < postTransactionProcessDTOList.Count; i++)
            {
                var postTransactionProcessDTO = postTransactionProcessDTOList[i];
                if (postTransactionProcessDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PostTransactionProcessBL postTransactionProcessBL = new PostTransactionProcessBL(executionContext, postTransactionProcessDTO);
                    postTransactionProcessBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving PostTransactionProcessDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PostTransactionProcessDTO", postTransactionProcessDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
