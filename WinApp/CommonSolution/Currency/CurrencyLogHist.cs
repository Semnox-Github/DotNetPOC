/********************************************************************************************
 * Project Name - Currency
 * Description  - Business logic of CurrencyLogHist
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       28-Sep-2016     Amaresh          Created 
 **2.70      16-June-2019    Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Currency
{
    /// <summary>
    /// CurrencyLogHist will creates and modifies the Currency
    /// </summary>
    public class CurrencyLogHist
    {
        private CurrencyLogHistDTO currencyLogHistDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parametrized constructor 
        /// </summary>
        public CurrencyLogHist(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CurrencyLogHist DTO parameter
        /// </summary>
        /// <param name="currencyLogHistDTO">Parameter of the type CurrencyLogHistDTO</param>
        public CurrencyLogHist(ExecutionContext executionContext ,CurrencyLogHistDTO currencyLogHistDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(currencyLogHistDTO);
            this.currencyLogHistDTO = currencyLogHistDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CurrencyLogHist id as the parameter
        /// Would fetch the CurrencyLogHist object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of CurrencyLogHist passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public CurrencyLogHist(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CurrencyLogHistDataHandler currencyLogHistDataHandler = new CurrencyLogHistDataHandler(sqlTransaction);
            currencyLogHistDTO = currencyLogHistDataHandler.GetCurrencyLogHist(id);
            if (currencyLogHistDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CurrencyLogHist", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CurrencyLogHist  
        /// CurrencyLogHist will be inserted if CurrencyId is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
            CurrencyLogHistDataHandler currencyLogHistDataHandler = new CurrencyLogHistDataHandler(sqlTransaction);

            if (currencyLogHistDTO.LogId < 0)
            {
                currencyLogHistDTO = currencyLogHistDataHandler.InsertCurrencyLogHist(currencyLogHistDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                currencyLogHistDTO.AcceptChanges();
            }
            else
            {
                if (currencyLogHistDTO.IsChanged == true)
                {
                    currencyLogHistDTO = currencyLogHistDataHandler.UpdateCurrencyLogHist(currencyLogHistDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    currencyLogHistDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the DTO
        /// </summary>
        public CurrencyLogHistDTO CurrencyLogHistDTO
        {
            get { return currencyLogHistDTO; }
        }
    }

    /// <summary>
    /// Manages the list of CurrencyLogHist List
    /// </summary>
    public class CurrencyLogHistList
    {
         private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parametrized  constructor 
        /// </summary>
        public CurrencyLogHistList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the CurrencyLogHistDTO List
        /// </summary>
        public List<CurrencyLogHistDTO> GetAllCurrencyLogHist(List<KeyValuePair<CurrencyLogHistDTO.SearchByCurrencyLogHistParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CurrencyLogHistDataHandler currencyLogHistDataHandler = new CurrencyLogHistDataHandler(sqlTransaction);
            List<CurrencyLogHistDTO> currencyLogHistDTOList = currencyLogHistDataHandler.GetCurrencyLogHistList(searchParameters);
            log.LogMethodExit(currencyLogHistDTOList);
            return currencyLogHistDTOList;
        }
    }

 }

