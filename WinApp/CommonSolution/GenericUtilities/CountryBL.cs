/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - CountryDataHandler
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 ********************************************************************************************
 *2.60         28-Mar-2019   Mushahid Faizan     Modified-  SaveUpdateCountryList(), Save() & removed unused namespaces.
 *                                               Added DeleteCountryList() & DeleteCountry() method.
 *2.70.2        25-Jul-2019      Dakshakh raj      Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities

{
    public class CountryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CountryDTO countryDTO;
        private ExecutionContext executionContext;

        public CountryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.countryDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the countryId parameter
        /// </summary>
        /// <param name="countryId">CountryId</param>
        public CountryBL(int countryId)
        {
            log.LogMethodEntry(countryId);
            CountryDataHandler countryDataHandler = new CountryDataHandler();
            this.countryDTO = countryDataHandler.GetCountryDTO(countryId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CountryDTO parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="CountryDTO">CountryDTO</param>
        public CountryBL(ExecutionContext executionContext, CountryDTO countryDTO)
        {
            log.LogMethodEntry(countryDTO, executionContext);
            this.countryDTO = countryDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// get CountryDTO Object
        /// </summary>
        public CountryDTO GetCountryDTO
        {
            get { return countryDTO; }
        }

        /// <summary>
        /// Saves the CountryDTO
        /// Checks if the countryId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CountryDataHandler countryDataHandler = new CountryDataHandler(sqlTransaction);
            if (countryDTO.CountryId < 0)
            {
                countryDTO = countryDataHandler.InsertCountryDTO(countryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                countryDTO.AcceptChanges();
            }
            else
            {
                if (countryDTO.IsChanged)
                {
                    countryDTO = countryDataHandler.UpdateCountryDTO(countryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    countryDTO.AcceptChanges();
                }
            }
            if (countryDTO.StateList != null && countryDTO.StateList.Count != 0)
            {
                foreach (StateDTO stateDTO in countryDTO.StateList)
                {
                    if (stateDTO.IsChanged)
                    {
                        stateDTO.CountryId = countryDTO.CountryId;
                        StateBL stateBL = new StateBL(executionContext, stateDTO);
                        stateBL.Save(sqlTransaction);
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Country and State based on CountryId
        /// </summary>
        /// <param name="countryId"></param>        
        public void DeleteCountry(int countryId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(countryId, sqlTransaction);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            CountryDataHandler countryDataHandler = new CountryDataHandler();
            try
            {
                parafaitDBTrx.BeginTransaction();
                sqlTransaction = parafaitDBTrx.SQLTrx;
                if (countryDTO.StateList != null && countryDTO.StateList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                if (countryDTO.StateList != null && countryDTO.StateList.Count != 0)
                {
                    foreach (StateDTO stateDTO in countryDTO.StateList)
                    {
                        if (stateDTO.IsChanged)
                        {
                            stateDTO.CountryId = countryDTO.CountryId;
                            StateBL stateBL = new StateBL(executionContext, stateDTO);
                            stateBL.DeleteState(stateDTO.StateId);
                        }
                    }
                }
                else
                {
                    countryDataHandler = new CountryDataHandler(sqlTransaction);
                    countryDataHandler.Delete(countryId);
                }
                parafaitDBTrx.EndTransaction();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                parafaitDBTrx.RollBack();
                throw new Exception(ex.Message);
            }
        }
    }

    /// <summary>
    /// Manages the list of CountryDTO
    /// </summary>
    public class CountryDTOList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CountryDTO> countryDTOList;
        private ExecutionContext executionContext;

        /// <summary>        
        /// Parameterized Constructor having ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CountryDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.countryDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public CountryDTOList(ExecutionContext executionContext, List<CountryDTO> countryDTOList)
        {
            log.LogMethodEntry(executionContext, countryDTOList);
            this.executionContext = executionContext;
            this.countryDTOList = countryDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the GetCountryDTOList list
        /// </summary>
        public List<CountryDTO> GetCountryDTOList(List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            CountryDataHandler countryDataHandler = new CountryDataHandler();
            List<CountryDTO> countryDTOs = countryDataHandler.GetCountryDTOList(searchParameters);
            log.LogMethodExit(countryDTOs);
            return countryDTOs;
        }

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns KeyValuePair List of CountryDTO.SearchByParameters by converting CountryDTO</returns>
        public List<KeyValuePair<CountryDTO.SearchByParameters, string>> BuildCountryDTOSearchParametersList(CountryDTO countryDTO)
        {
            log.LogMethodEntry(countryDTO);
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> countryDTOSearchParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
            if (countryDTO != null)
            {
                if (countryDTO.CountryId >= 0)
                    countryDTOSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.COUNTRY_ID, countryDTO.CountryId.ToString()));

                if (!(string.IsNullOrEmpty(countryDTO.CountryName)))
                    countryDTOSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.COUNTRY_NAME, countryDTO.CountryName.ToString()));

                if (countryDTO.MasterEntityId >= 0)
                    countryDTOSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.MASTER_ENTITY_ID, countryDTO.MasterEntityId.ToString()));

                //if (countryDTO.SiteId > 0)
                countryDTOSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, countryDTO.SiteId.ToString()));

            }
            log.LogMethodExit(countryDTOSearchParams);
            return countryDTOSearchParams;
        }

        /// <summary>
        /// GetCountryDTOsList(CountryDTO countryDTO) method search based on countryDTO
        /// </summary>
        /// <param name="countryDTO">CountryDTO turnstileDTO</param>
        /// <returns>List of CountryDTO object</returns>
        public List<CountryDTO> GetCountryDTOList(CountryDTO countryDTO)
        {
            try
            {
                log.LogMethodEntry(countryDTO);
                List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchParameters = BuildCountryDTOSearchParametersList(countryDTO);
                CountryDataHandler countryDataHandler = new CountryDataHandler();
                List<CountryDTO> countryDTOs = countryDataHandler.GetCountryDTOList(searchParameters);
                log.LogMethodExit(countryDTOs);
                return countryDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method should be used to Save and Update the Country details for Web Management Studio.
        /// </summary>
        public void SaveUpdateCountryList()
        {
            log.LogMethodEntry();
            if (countryDTOList != null && countryDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CountryDTO countryDto in countryDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CountryBL country = new CountryBL(executionContext, countryDto);
                            country.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);

                            if (ex.Message.ToLower().Contains("statement conflicted with the reference constraint"))
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }

                    }
                }
            }
            log.LogMethodExit();
        }
    
    /// <summary>
    /// Delete the Country List 
    /// </summary>
    public void DeleteCountryList()
    {
        log.LogMethodEntry();
        if (countryDTOList != null && countryDTOList.Count > 0)
        {
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                foreach (CountryDTO countryDto in countryDTOList)
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        CountryBL country = new CountryBL(executionContext, countryDto);
                        country.DeleteCountry(countryDto.CountryId, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
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
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
}