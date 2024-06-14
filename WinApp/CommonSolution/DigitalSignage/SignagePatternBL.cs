/********************************************************************************************
 * Project Name - Signage Pattern BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Akshay Gulaganji     Added new constructor(executionContext, signagePatternDTOList) and method SaveUpdateSignagePatternList() in SignagePatternListBL class
 *2.70.2        31-Jul-2019      Dakshakh raj         Modified : Save() method Insert/Update method returns DTO.
 *2.100.0        10-Aug-2020      Mushahid Faizan     Modified : 3 tier changes for Rest API.
  *2.110.0      30-Nov-2020       Prajwal S            Modified : Constructor with Id parameter.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    public class SignagePatternBL
    {
        private SignagePatternDTO signagePatternDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Default constructor of SignagePattern class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private SignagePatternBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            signagePatternDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor will fetch the SignagePattern DTO based on the signagePattern id passed 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="signagePatternId">signagePatternId</param>
        /// <param name="openTransactionsOnly">openTransactionsOnly</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public SignagePatternBL(ExecutionContext executionContext, int signagePatternId, bool openTransactionsOnly = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, signagePatternId, sqlTransaction);
            SignagePatternDataHandler signagePatternDataHandler = new SignagePatternDataHandler(sqlTransaction);
            signagePatternDTO = signagePatternDataHandler.GetSignagePatternDTO(signagePatternId, openTransactionsOnly);
            if (signagePatternDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SignagePattern", signagePatternId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor will fetch the SignagePattern DTO based on the signagePattern id passed 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="signagePatternId">signagePatternId</param>
        /// <param name="openTransactionsOnly">openTransactionsOnly</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public SignagePatternBL(ExecutionContext executionContext, string contentGuid, bool openTransactionsOnly = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, contentGuid, sqlTransaction);
            SignagePatternDataHandler signagePatternDataHandler = new SignagePatternDataHandler(sqlTransaction);
            signagePatternDTO = signagePatternDataHandler.GetSignagePatternDTOByGuid(contentGuid, openTransactionsOnly);
            if (signagePatternDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SignagePattern", contentGuid);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates signagePattern object using the SignagePattern
        /// </summary>
        /// <param name="signagePatternDTO">SignagePattern object</param>
        /// <param name="executionContext">executionContext</param>
        public SignagePatternBL(ExecutionContext executionContext, SignagePatternDTO signagePatternDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, signagePatternDTO);
            this.signagePatternDTO = signagePatternDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Saves the signagePattern record
        /// Checks if the SignagePatternId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (signagePatternDTO.IsChanged)
            {
                if (signagePatternDTO.IsActive)
                {
                    List<ValidationError> validationErrors = Validate(sqlTransaction);
                    if (validationErrors.Any())
                    {
                        string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                        log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                        throw new ValidationException(message, validationErrors);
                    }
                }
                SignagePatternDataHandler signagePatternDataHandler = new SignagePatternDataHandler(sqlTransaction);
                if (signagePatternDTO.SignagePatternId < 0)
                {
                    signagePatternDTO = signagePatternDataHandler.InsertSignagePattern(signagePatternDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    signagePatternDTO.AcceptChanges();
                }
                else
                {
                    if (signagePatternDTO.IsChanged)
                    {
                        signagePatternDTO = signagePatternDataHandler.UpdateSignagePattern(signagePatternDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        signagePatternDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate errors
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(signagePatternDTO.Name))
            {
                validationErrorList.Add(new ValidationError("SignagePattern", "Name", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"))));
            }
            if (string.IsNullOrWhiteSpace(signagePatternDTO.Pattern) ||
                System.Text.RegularExpressions.Regex.IsMatch(signagePatternDTO.Pattern, @"^[0-9]{7}$") == false)
            {
                validationErrorList.Add(new ValidationError("SignagePattern", "Pattern", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Pattern"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SignagePatternDTO SignagePatternDTO { get { return signagePatternDTO; } }
    }

    /// <summary>
    /// Manages the list of signagePattern
    /// </summary>
    public class SignagePatternListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<SignagePatternDTO> signagePatternDTOList;

        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public SignagePatternListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with executionContext and signagePatternDTOList
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="signagePatternDTOList">signagePatternDTOList</param>
        public SignagePatternListBL(ExecutionContext executionContext, List<SignagePatternDTO> signagePatternDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, signagePatternDTOList);
            this.signagePatternDTOList = signagePatternDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the signagePattern list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<SignagePatternDTO> GetSignagePatternDTOList(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SignagePatternDataHandler signagePatternDataHandler = new SignagePatternDataHandler(sqlTransaction);
            List<SignagePatternDTO> signagePatternDTOList = signagePatternDataHandler.GetSignagePatternDTOList(searchParameters);
            log.LogMethodExit(signagePatternDTOList);
            return signagePatternDTOList;
        }

        /// <summary>
        /// Saves the SignagePattern List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (signagePatternDTOList == null ||
                signagePatternDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < signagePatternDTOList.Count; i++)
            {
                var signagePatternDTO = signagePatternDTOList[i];
                if (signagePatternDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    SignagePatternBL signagePatternBL = new SignagePatternBL(executionContext, signagePatternDTO);
                    signagePatternBL.Save();
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
                    log.Error("Error occurred while saving signagePatternDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("signagePatternDTO", signagePatternDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
