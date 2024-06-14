/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - DirectSQLBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.3     16-Nov -2019     Girish Kundar       Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    public class DirectSQLBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private DirectSQLDTO directSQLDTO;

        /// <summary>
        /// Default constructor DirectSQLBL class
        /// </summary>
        public DirectSQLBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            directSQLDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with  directSQLDTO parameter
        /// </summary>
        /// <param name=" directSQLDTO">parameter of type  DirectSQLDTO </param>
        public DirectSQLBL(ExecutionContext executionContext, DirectSQLDTO directSQLDTO)
        {
            log.LogMethodEntry(directSQLDTO);
            this.directSQLDTO = directSQLDTO;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the Ticket station details to table
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            DirectSQLDataHandler directSQLDataHandler = new DirectSQLDataHandler();
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (directSQLDTO.DirectSQLId < 0)
            {
                directSQLDTO = directSQLDataHandler.Insert(directSQLDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                directSQLDTO.AcceptChanges();
            }
            else
            {
                if (directSQLDTO.IsChanged)
                {
                    directSQLDTO = directSQLDataHandler.Update(directSQLDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    directSQLDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the   DirectSQLDTO  .
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
        /// Gets the  DirectSQLDTO
        /// </summary>
        public DirectSQLDTO DirectSQLDTO
        {
            get { return directSQLDTO; }
        }

    }
    /// <summary>
    /// Class for DirectSQLListBL List
    /// </summary>
    public class DirectSQLListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DirectSQLDTO> directSQLDTOList = new List<DirectSQLDTO>();
        private ExecutionContext executionContext;
        public DirectSQLListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and  directSQLDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name=" directSQLDTOList"> directSQLDTOList</param>
        public DirectSQLListBL(ExecutionContext executionContext, List<DirectSQLDTO> directSQLDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.directSQLDTOList = directSQLDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns All active the  directSQLDTO records from the table 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of   DirectSQLDTO</returns>
        public List<DirectSQLDTO> GetAllDirectSQLDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            DirectSQLDataHandler directSQLDataHandler = new DirectSQLDataHandler(sqlTransaction);
            List<KeyValuePair<DirectSQLDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DirectSQLDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DirectSQLDTO.SearchByParameters, string>(DirectSQLDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<DirectSQLDTO> directSQLDTOList = directSQLDataHandler.GetDirectSQLDTOList(searchParameters);
            log.LogMethodExit(directSQLDTOList);
            return directSQLDTOList;
        }

        /// <summary>
        ///  Returns All the  DirectSQLDTO records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of  DirectSQLDTO</returns>
        public List<DirectSQLDTO> GetAllDirectSQLDTOList(List<KeyValuePair<DirectSQLDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DirectSQLDataHandler directSQLDataHandler = new DirectSQLDataHandler(sqlTransaction);
            List<DirectSQLDTO> directSQLDTOList = directSQLDataHandler.GetDirectSQLDTOList(searchParameters);
            log.LogMethodExit(directSQLDTOList);
            return directSQLDTOList;
        }

        /// <summary>
        /// Save and Update  DirectSQLDTO list Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (directSQLDTOList != null)
                {
                    foreach (DirectSQLDTO directSQLDTO in directSQLDTOList)
                    {
                        DirectSQLBL directSQLBL = new DirectSQLBL(executionContext, directSQLDTO);
                        directSQLBL.Save(sqlTransaction);
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
