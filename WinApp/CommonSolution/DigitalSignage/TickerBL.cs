/********************************************************************************************
 * Project Name - Ticker BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor TickerBL, TickerListBL and 
 *                                                 methods SaveUpdateTickersList
 *2.70.2        31-Jul-2019  Dakshakh raj            Modified : Save() method Insert/Update method returns DTO.
 *2.100.0        29-Jul-2020      Mushahid Faizan     Modified : 3 tier changes for Rest API.
  *2.110.0     30-Nov-2020       Prajwal S          Modified : Constructor with Id parameter
 *                                                 Modified : TickerBL(ExecutionContext executionContext, TickerDTO tickerDTO)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.SignageClient.Service;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for Ticker class.
    /// </summary>
    public class TickerBL
    {
        private TickerDTO tickerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of TickerBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private TickerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.tickerDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ticker id as the parameter Would fetch the ticker object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id">id</param>
        /// <param name="sqltransction">sqltransction</param>
        public TickerBL(ExecutionContext executionContext,int id, SqlTransaction sqltransction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransction);
            TickerDataHandler tickerDataHandler = new TickerDataHandler(sqltransction);
            this.tickerDTO = tickerDataHandler.GetTickerDTO(id);
            if (tickerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Ticker", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(tickerDTO);
        }

        /// <summary>
        /// Constructor with the contentGuid as the parameter Would fetch the ticker object from the database based on the contentGuid passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="contentGuid">id</param>
        /// <param name="sqltransction">sqltransction</param>
        public TickerBL(ExecutionContext executionContext, string contentGuid, SqlTransaction sqltransction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, contentGuid, sqltransction);
            TickerDataHandler tickerDataHandler = new TickerDataHandler(sqltransction);
            this.tickerDTO = tickerDataHandler.GetTickerDTOByGuid(contentGuid);
            if (tickerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Ticker", contentGuid);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(tickerDTO);
        }

        /// <summary>
        /// Creates TickerBL object using the TickerDTO
        /// </summary>
        /// <param name="tickerDTO">TickerDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public TickerBL(ExecutionContext executionContext, TickerDTO tickerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tickerDTO);
            this.tickerDTO = tickerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TickerBL object using the TickerDTO
        /// </summary>
        /// <param name="tickerDTO">TickerDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public TickerBL(TickerDTO tickerDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(tickerDTO, executionContext);
            this.tickerDTO = tickerDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the Ticker
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TickerDataHandler tickerDataHandler = new TickerDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (tickerDTO.TickerId < 0)
            {
                tickerDTO = tickerDataHandler.InsertTicker(tickerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                tickerDTO.AcceptChanges();
            }
            else
            {
                if(tickerDTO.IsChanged)
                {
                    tickerDTO = tickerDataHandler.UpdateTicker(tickerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    tickerDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (tickerDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TickerDTO TickerDTO
        {
            get
            {
                return tickerDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Ticker
    /// </summary>
    public class TickerListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TickerDTO> tickerDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of TickerListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public TickerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="tickerToList">tickerToList</param>
        /// <param name="executionContext">executionContext</param>
        public TickerListBL(ExecutionContext executionContext, List<TickerDTO> tickerDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tickerDTOList);
            this.tickerDTOList = tickerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Ticker list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Ticker list</returns>
        public List<TickerDTO> GetTickerDTOList(List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TickerDataHandler tickerDataHandler = new TickerDataHandler(sqlTransaction);
            List<TickerDTO>  tickersList = tickerDataHandler.GetTickerDTOList(searchParameters);
            log.LogMethodExit(tickersList);
            return tickersList;
        }

        /// <summary>
        /// Save and Updated the tickers details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (tickerDTOList == null ||
                tickerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < tickerDTOList.Count; i++)
            {
                var tickerDTO = tickerDTOList[i];
                if (tickerDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TickerBL tickerBL = new TickerBL(executionContext, tickerDTO);
                    tickerBL.Save();
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
                    log.Error("Error occurred while saving tickerDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("tickerDTO", tickerDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}