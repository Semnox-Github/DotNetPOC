/********************************************************************************************
 * Project Name - Waiver
 * Description  - Business logic of WaiverSignature
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       1-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.2       15-Oct-2019    GUru S A         Waiver phase 2 changes
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// Business logic for WaiverSignature class.
    /// </summary>
    public class WaiverSignatureBL
    {
        private WaiverSignatureDTO waiverSignatureDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Constructor with the  waiverSignature id as the parameter
        /// Would fetch the waiverSignature object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public WaiverSignatureBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            this.executionContext = executionContext;
            WaiverSignatureDataHandler waiverSignatureDataHandler = new WaiverSignatureDataHandler(sqlTransaction);
            waiverSignatureDTO = waiverSignatureDataHandler.GetWaiverSignatureDTO(id);
            if (waiverSignatureDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "WaiverSignature", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates WaiverSignatureBL object using the WaiverSignatureDTO
        /// </summary>
        /// <param name="waiverSignatureDTO">WaiverSignatureDTO object</param>
        public WaiverSignatureBL(ExecutionContext executionContext, WaiverSignatureDTO waiverSignatureDTO)
        {
            log.LogMethodEntry(executionContext, waiverSignatureDTO);
            this.executionContext = executionContext;
            this.waiverSignatureDTO = waiverSignatureDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the WaiverSignature
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (executionContext == null)
                executionContext = ExecutionContext.GetExecutionContext();

            WaiverSignatureDataHandler waiverSignatureDataHandler = new WaiverSignatureDataHandler(sqlTransaction);
            if (waiverSignatureDTO.WaiverSignedId < 0)
            {
                waiverSignatureDTO = waiverSignatureDataHandler.InsertWaiverSignature(waiverSignatureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                waiverSignatureDTO.AcceptChanges();
            }
            else
            {
                if (waiverSignatureDTO.IsChanged)
                {
                    waiverSignatureDTO = waiverSignatureDataHandler.UpdateWaiverSignature(waiverSignatureDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    waiverSignatureDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }


        public WaiverSignatureDTO GetWaiverSignatureDTO { get { return waiverSignatureDTO; } }


        /// <summary>
        /// Represents foreign key error that occur during application execution. 
        /// </summary>
        public class ForeignKeyException : Exception
        {
            /// <summary>
            /// Default constructor of ForeignKeyException.
            /// </summary>
            public ForeignKeyException()
            {
            }

            /// <summary>
            /// Initializes a new instance of ForeignKeyException class with a specified error message.
            /// </summary>
            /// <param name="message">message</param>
            public ForeignKeyException(string message)
            : base(message)
            {
            }
            /// <summary>
            /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">message</param>
            /// <param name="inner">inner exception</param>
            public ForeignKeyException(string message, Exception inner)
            : base(message, inner)
            {
            }
        }
    }

    /// <summary>
    /// Manages the list of WaiverSetDetail
    /// </summary>
    public class WaiverSignatureListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public WaiverSignatureListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<WaiverSignatureDTO> GetWaiverSignatureDTOList(List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            WaiverSignatureDataHandler waiverSignatureDataHandler = new WaiverSignatureDataHandler(sqlTransaction);
            List<WaiverSignatureDTO> returnValue = waiverSignatureDataHandler.GetWaiverSignatureDTOList(searchParameters); 
            log.LogMethodExit(returnValue);
            return returnValue;
        } 

        public DataTable GetTransactionWaiverList(int trxId, DateTime fromDate, DateTime toDate,
                                                string phoneNumber1, string phoneNumber2, string cardNumber,
                                                string customerName, int customAttributeId, string customAttributeValue)
        {
            WaiverSignatureDataHandler waiverSignatureDataHandler = new WaiverSignatureDataHandler(null);
            DataTable dataTable = waiverSignatureDataHandler.GetTransactionWaiverList(trxId, fromDate, toDate,
                                                 phoneNumber1, phoneNumber2, cardNumber,
                                                customerName, customAttributeId, customAttributeValue);
            return dataTable;
        }
    }
}
