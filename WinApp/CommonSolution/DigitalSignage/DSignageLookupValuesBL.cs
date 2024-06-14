/********************************************************************************************
 * Project Name - DSignageLookupValues BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created 
 *2.40        29-Nov-2018      Jagan Mohana Rao    Added DSignageLookupValuesBL() method with DTO and ExecutionContext
 *2.70.2        30-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.90        07-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, and 
 *                                                 List class changes as per 3 tier standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for DSignageLookupValues class.
    /// </summary>
    public class DSignageLookupValuesBL
    {
        private DSignageLookupValuesDTO dSignageLookupValuesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of DSignageLookupValuesBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private DSignageLookupValuesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.dSignageLookupValuesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the dSignageLookupValues id as the parameter Would fetch the dSignageLookupValues object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public DSignageLookupValuesBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransaction);
            DSignageLookupValuesDataHandler dSignageLookupValuesDataHandler = new DSignageLookupValuesDataHandler(sqltransaction);
            this.dSignageLookupValuesDTO = dSignageLookupValuesDataHandler.GetDSignageLookupValuesDTO(id);
            if (dSignageLookupValuesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "dSignageLookupValuesDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(dSignageLookupValuesDTO);
        }

        /// <summary>
        /// Creates DSignageLookupValuesBL object using the DSignageLookupValuesDTO 
        /// </summary>
        /// <param name="executionContex">executionContex</param>
        /// <param name="dSignageLookupValuesDTO">dSignageLookupValuesDTO</param>
        public DSignageLookupValuesBL(ExecutionContext executionContext, DSignageLookupValuesDTO dSignageLookupValuesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dSignageLookupValuesDTO);
            this.dSignageLookupValuesDTO = dSignageLookupValuesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///Saves the DSignageLookupValues
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            DSignageLookupValuesDataHandler dSignageLookupValuesDataHandler = new DSignageLookupValuesDataHandler(sqltransaction);
            if (dSignageLookupValuesDTO.IsChanged == false &&
                      dSignageLookupValuesDTO.DSLookupValueID > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (dSignageLookupValuesDTO.DSLookupValueID < 0)
            {
                dSignageLookupValuesDTO = dSignageLookupValuesDataHandler.InsertDSignageLookupValues(dSignageLookupValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dSignageLookupValuesDTO.AcceptChanges();
            }
            else
            {
                if (dSignageLookupValuesDTO.IsChanged)
                {
                    dSignageLookupValuesDTO = dSignageLookupValuesDataHandler.UpdateDSignageLookupValues(dSignageLookupValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dSignageLookupValuesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the dSignageLookupValuesDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DSignageLookupValuesDTO DSignageLookupValuesDTO
        {
            get
            {
                return dSignageLookupValuesDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DSignageLookupValues
    /// </summary>
    public class DSignageLookupValuesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList = new List<DSignageLookupValuesDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DSignageLookupValuesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DSignageLookupValuesListBL(ExecutionContext executionContext, List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dSignageLookupValuesDTOList);
            this.dSignageLookupValuesDTOList = dSignageLookupValuesDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DSignageLookupValuesDTO List for dsLookupIdList
        /// </summary>
        /// <param name="dsLookupIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DSignageLookupValuesDTO</returns>
        public List<DSignageLookupValuesDTO> GetDSignageLookupValuesDTOList(List<int> dsLookupIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dsLookupIdList, activeRecords);
            DSignageLookupValuesDataHandler dSignageLookupValuesDataHandler = new DSignageLookupValuesDataHandler(sqlTransaction);
            this.dSignageLookupValuesDTOList = dSignageLookupValuesDataHandler.GetDSignageLookupValuesDTOList(dsLookupIdList, activeRecords);
            log.LogMethodExit(dSignageLookupValuesDTOList);
            return dSignageLookupValuesDTOList;
        }
        /// <summary>
        ///  Returns the DSignageLookupValues list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>dSignageLookupValues List</returns>
        public List<DSignageLookupValuesDTO> GetDSignageLookupValuesDTOList(List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DSignageLookupValuesDataHandler dSignageLookupValuesDataHandler = new DSignageLookupValuesDataHandler(sqlTransaction);
            this.dSignageLookupValuesDTOList = dSignageLookupValuesDataHandler.GetDSignageLookupValuesDTOList(searchParameters);
            log.LogMethodExit(dSignageLookupValuesDTOList);
            return dSignageLookupValuesDTOList;
        }

        /// <summary>
        /// Saves the DSignageLookupValuesDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (dSignageLookupValuesDTOList == null ||
                dSignageLookupValuesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < dSignageLookupValuesDTOList.Count; i++)
            {
                var dSignageLookupValuesDTO = dSignageLookupValuesDTOList[i];
                if (dSignageLookupValuesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    DSignageLookupValuesBL DSignageLookupValuesBL = new DSignageLookupValuesBL(executionContext, dSignageLookupValuesDTO);
                    DSignageLookupValuesBL.Save(sqlTransaction);
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
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving dSignageLookupValuesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("dSignageLookupValuesDTO", dSignageLookupValuesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
