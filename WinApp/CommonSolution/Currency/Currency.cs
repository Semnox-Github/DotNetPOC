/********************************************************************************************
 * Project Name - Currency
 * Description  - Business logic of Currency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       21-Sep-2016     Amaresh          Created
 *2.60       09-Apr-2019     Mushahid Faizan  Modified : SaveUpdateCurrencyList() method, removed unused namespaces and added LogMethodEntry/Exit
 *2.70       16-June-2019    Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Currency
{
    /// <summary>
    /// Currency will creates and modifies the Currency
    /// </summary>
    public class Currency
    {
        private CurrencyDTO currencyDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parametrized  constructor 
        /// </summary>
        public Currency(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Currency DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="currencyDTO">CurrencyDTO </param>
        public Currency(ExecutionContext executionContext , CurrencyDTO currencyDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext,currencyDTO);
            this.currencyDTO = currencyDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Currency id as the parameter
        /// Would fetch the Currency object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of Currency passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public Currency(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CurrencyDataHandler currencyDataHandler = new CurrencyDataHandler(sqlTransaction);
            currencyDTO = currencyDataHandler.GetCurrency(id);
            if (currencyDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Currency", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Currency  
        /// Currency   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);            
            CurrencyDataHandler currencyDataHandler = new CurrencyDataHandler(sqlTransaction);

            if (currencyDTO.CurrencyId < 0)
            {
                currencyDTO = currencyDataHandler.InsertCurrency(currencyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                currencyDTO.AcceptChanges();
            }
            else
            {
                if (currencyDTO.IsChanged)
                {
                    currencyDTO = currencyDataHandler.UpdateCurrency(currencyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    currencyDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get the DTO
        /// </summary>
        public CurrencyDTO CurrencyDTO
        {
            get { return currencyDTO; }
        }
    }

    /// <summary>
    /// Manages the list of Currency List
    /// </summary>
    public class CurrencyList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CurrencyDTO> currencyDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public CurrencyList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.currencyDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="currencyDTOList"></param>
        public CurrencyList(ExecutionContext executionContext, List<CurrencyDTO> currencyDTOList)
        {
            log.LogMethodEntry(executionContext, currencyDTOList);
            this.executionContext = executionContext;
            this.currencyDTOList = currencyDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the List of CurrencyDTO
        /// </summary>
        public List<CurrencyDTO> GetAllCurrency(List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CurrencyDataHandler currencyDataHandler = new CurrencyDataHandler(sqlTransaction);            
            List<CurrencyDTO>  currencyDTOList = currencyDataHandler.GetCurrencyList(searchParameters);
            log.LogMethodExit(currencyDTOList);
            return currencyDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Currency details for Web Management Studio.
        /// </summary>
        public void SaveUpdateCurrencyList()
        {
            log.LogMethodEntry();
            try
            {
                if (currencyDTOList != null && currencyDTOList.Any())
                {
                    foreach (CurrencyDTO currencyDTO in currencyDTOList)
                    {
                        Currency currency = new Currency(executionContext, currencyDTO);
                        currency.Save();
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