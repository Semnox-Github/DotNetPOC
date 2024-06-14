/********************************************************************************************
 * Project Name - ParafaitOptionValuesBL                                                                    
 * Description  - BL for the ParafaitOptionValues tables.
 *
 **************
 **Version Log
  *Version     Date             Modified  By                  Remarks          
 *********************************************************************************************
 *2.40.1       25-Jan-2019    Flavia Jyothi D'Souza         Created new BL class
 * 2.60        25-Mar-2019    Muhammed Mehraj              added  log.Error(ex)
 *2.70         29-Jun-2019    Akshay Gulaganji             added DeleteParafaitOptionValues() and DeleteParafaitOptionValuesDTOList() method
 *             5-Jul -2019    Girish Kundar                Modified : Save() method modified for Insert/Update returns DTO instead if Id.
 *                                                                    Passing Execution Context object as parameter to the constructors.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    public class ParafaitOptionValuesBL
    {

        private ParafaitOptionValuesDTO parafaitOptionValuesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ParafaitOptionValuesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.parafaitOptionValuesDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the default_value_id as the parameter
        /// Would fetch the ParafaitDefaults object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitOptionValuesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id);
            ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);
            parafaitOptionValuesDTO = parafaitOptionValuesDataHandler.GetParafaitOptionValuesDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="OptionValue"></param>
        public ParafaitOptionValuesBL(ExecutionContext executionContext, string OptionValue, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry();

            ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);

            List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.OPTION_VALUE, OptionValue));
            searchParameter.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameter.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = parafaitOptionValuesDataHandler.GetParafaitOptionValuesDTOList(searchParameter);
            if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Count > 0)
            {
                parafaitOptionValuesDTO = parafaitOptionValuesDTOList[0];
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// Creates ParafaitOptionValuesBL object using the ParafaitOptionValuesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="parafaitOptionValuesDTO">ParafaitOptionValuesDTO object</param>
        public ParafaitOptionValuesBL(ExecutionContext executionContext, ParafaitOptionValuesDTO parafaitOptionValuesDTO)
         : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitOptionValuesDTO);
            this.parafaitOptionValuesDTO = parafaitOptionValuesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParafaitOptionValuesDTO ParafaitOptionValuesDTO
        {
            get
            {
                return parafaitOptionValuesDTO;
            }
        }


        /// <summary>
        /// Method to save  sql transactions 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);
            if (parafaitOptionValuesDTO.OptionValueId < 0)
            {
                parafaitOptionValuesDTO = parafaitOptionValuesDataHandler.InsertUtility(parafaitOptionValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parafaitOptionValuesDTO.AcceptChanges();
            }
            else
            {
                if (parafaitOptionValuesDTO.IsChanged)
                {
                    parafaitOptionValuesDTO = parafaitOptionValuesDataHandler.UpdateUtility(parafaitOptionValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    parafaitOptionValuesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the ParafaitOptionValues details based on optionValueId
        /// </summary>
        /// <param name="optionValueId">optionValueId</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>        
        public void DeleteParafaitOptionValues(int optionValueId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(optionValueId);
            try
            {
                ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);
                parafaitOptionValuesDataHandler.DeleteParafaitOptionValues(optionValueId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }


    /// <summary>
    /// Manages the list of Parafait Option Values 
    /// </summary>
    public class ParafaitOptionValuesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">application context</param>
        public ParafaitOptionValuesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            parafaitOptionValuesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitOptionValuesDTOList"></param>
        public ParafaitOptionValuesListBL(ExecutionContext executionContext, List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList)
        {
            log.LogMethodEntry(executionContext, parafaitOptionValuesDTOList);
            this.executionContext = executionContext;
            this.parafaitOptionValuesDTOList = parafaitOptionValuesDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the ParafaitOptionValues list
        /// </summary>
        public List<ParafaitOptionValuesDTO> GetParafaitOptionValuesDTOList(List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);
            List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = parafaitOptionValuesDataHandler.GetParafaitOptionValuesDTOList(searchParameters);
            log.LogMethodExit(parafaitOptionValuesDTOList);
            return parafaitOptionValuesDTOList;
        }

        public List<ParafaitOptionValuesDTO> GetParafaitOptionValuesDTOList(List<int> defaultsIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(defaultsIdList, activeRecords);
            ParafaitOptionValuesDataHandler parafaitOptionValuesDataHandler = new ParafaitOptionValuesDataHandler(sqlTransaction);
            parafaitOptionValuesDTOList = parafaitOptionValuesDataHandler.GetParafaitOptionValuesDTOList(defaultsIdList, activeRecords);
            log.LogMethodExit(parafaitOptionValuesDTOList);
            return parafaitOptionValuesDTOList;
        }


        /// <summary>
        /// Save Update ParafaitOptionValuesDTOList
        /// </summary>
        public void SaveUpdateParafaitOptionValuesDTOList()
        {
            try
            {
                log.LogMethodEntry();
                if (parafaitOptionValuesDTOList != null)
                {
                    foreach (ParafaitOptionValuesDTO parafaitOptionValuesDTO in parafaitOptionValuesDTOList)
                    {
                        ParafaitOptionValuesBL parafaitOptionValuesBL = new ParafaitOptionValuesBL(executionContext, parafaitOptionValuesDTO);
                        parafaitOptionValuesBL.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message); 
            }
        }
        /// <summary>
        /// Hard Deletions for ParafaitOptionValuesDTOList
        /// </summary>
        public void DeleteParafaitOptionValuesDTOList()
        {
            log.LogMethodEntry();
            if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Count > 0)
            {
                foreach (ParafaitOptionValuesDTO parafaitOptionValuesDTO in parafaitOptionValuesDTOList)
                {
                    if (parafaitOptionValuesDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ParafaitOptionValuesBL parafaitOptionValuesBL = new ParafaitOptionValuesBL(executionContext);
                                parafaitOptionValuesBL.DeleteParafaitOptionValues(parafaitOptionValuesDTO.OptionValueId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
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
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}