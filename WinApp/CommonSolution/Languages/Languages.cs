/********************************************************************************************
 * Project Name - Languages
 * Description  - Languages object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        24-June-2016   Rakshith          Created 
 *2.40        08-Oct-2018    Vivek             added constructor to accept language Id parameter
 *2.60        06-May-2019    Mushahid Faizan   Added new Class LanguagesList, Save() method and Constructor.
 *2.60.3      17-Jun-2019    Nagesh Badiger    Added parameterized constructor
 *2.70        29-Jul-2019   Mushahid Faizan    Added Delete in Save() method.
 *2.70.2        29-Jul-2019    Girish Kundar     Modified : Save () method returns the DTO
 *2.70.2        10-Dec-2019   Jinto Thomas       Modified : constructor with language id as parameters  
  *2.70.2      03-Apr-2020   Mushahid Faizan    Added Delete Method for Web Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Languages class
    /// </summary>
    public class Languages
    {
        private LanguagesDTO languageDTO = null;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public Languages(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.languageDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Languages object using the languageDTO
        /// </summary>
        /// <param name="languageDTO">languageDTO object</param>
        public Languages(ExecutionContext executionContext, LanguagesDTO languageDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, languageDTO);
            this.executionContext = executionContext;
            this.languageDTO = languageDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Languages
        /// Checks if the  LanguageId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler(sqlTransaction);
            //if (languageDTO.Active)
            //{
            if (languageDTO.LanguageId < 0)
            {
                Validate(sqlTransaction);
                languageDTO = languagesDatahandler.InsertLanguages(languageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                languageDTO.AcceptChanges();
            }
            else
            {
                if (languageDTO.IsChanged)
                {
                    Validate(sqlTransaction);
                    languageDTO = languagesDatahandler.UpdateLanguages(languageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    languageDTO.AcceptChanges();
                }
            }
            //}
            //else  // Hard Delete
            //{
            //    if (languageDTO.LanguageId >= 0)
            //    {
            //        languagesDatahandler.DeleteLanguages(languageDTO.LanguageId);
            //    }
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Languages records from database based on LanguageId
        /// This method is only used for Web Management Studio.
        /// </summary>
        public int Delete(int languageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(languageId, sqlTransaction);
            try
            {
                LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler();
                log.LogMethodExit();
                return languagesDatahandler.DeleteLanguages(languageId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Constructor with languageId Gets the DTO of given Id.
        /// </summary>
        /// <param name="languageId">languageId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Languages(ExecutionContext executionContext, int languageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, languageId, sqlTransaction);
            this.executionContext = executionContext;
            LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler(sqlTransaction);
            languageDTO = languagesDatahandler.GetLanguage(languageId);
            log.LogMethodExit(languageDTO);
        }

        /// <summary>
        /// Gets the LanguagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LanguagesDTO matching the search criteria</returns>
        public List<LanguagesDTO> GetAllLanguagesList(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler(sqlTransaction);
            List<LanguagesDTO> languagesDTOList = languagesDatahandler.GetAllLanguagesList(searchParameters);
            log.LogMethodExit(languagesDTOList);
            return languagesDTOList;
        }

        /// <summary>
        /// Get the LanguagesDTO
        /// </summary>
        public LanguagesDTO GetLanguagesDTO
        {
            get { return languageDTO; }
        }

        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler(sqlTransaction);
            List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LanguagesDTO> languagesDTOList = languagesDatahandler.GetAllLanguagesList(searchParams);
            if (languagesDTOList != null && languagesDTOList.Any())
            {
                if (languagesDTOList.Exists(x => x.LanguageName == languageDTO.LanguageName) && languageDTO.LanguageId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Language"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (languagesDTOList.Exists(x => x.LanguageCode == languageDTO.LanguageCode) && languageDTO.LanguageId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "LanguageCode"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (languagesDTOList.Exists(x => x.LanguageName == languageDTO.LanguageName && x.LanguageId != languageDTO.LanguageId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Language"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (languagesDTOList.Exists(x => x.LanguageCode == languageDTO.LanguageCode && x.LanguageId != languageDTO.LanguageId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "LanguageCode"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    ///  Manages the list of Languages
    /// </summary>
    public class LanguagesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<LanguagesDTO> languagesList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LanguagesList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LanguagesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.languagesList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="languagesList">languagesList</param>
        public LanguagesList(ExecutionContext executionContext, List<LanguagesDTO> languagesList)
        {
            log.LogMethodEntry(executionContext, languagesList);
            this.executionContext = executionContext;
            this.languagesList = languagesList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the LanguagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LanguagesDTO matching the search criteria</returns>
        public List<LanguagesDTO> GetAllLanguagesList(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LanguagesDatahandler languagesDatahandler = new LanguagesDatahandler(sqlTransaction);
            List<LanguagesDTO> languagesDTOList = languagesDatahandler.GetAllLanguagesList(searchParameters);
            log.LogMethodExit(languagesDTOList);
            return languagesDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Languages details for Web Management Studio. 
        /// </summary>
        public void SaveUpdateLanguages()
        {
            log.LogMethodEntry();
            try
            {
                if (languagesList != null && languagesList.Count > 0)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (LanguagesDTO languagesDTO in languagesList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                Languages languages = new Languages(executionContext, languagesDTO);
                                languages.Save(parafaitDBTrx.SQLTrx);
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
                        }
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
        /// <summary>
        /// Delete the DeleteLanguagesList based on LanguageId
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (languagesList != null && languagesList.Count > 0)
            {
                foreach (LanguagesDTO languagesDTO in languagesList)
                {
                    if (languagesDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();

                                Languages languages = new Languages(executionContext, languagesDTO);
                                languages.Delete(languagesDTO.LanguageId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
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

        internal DateTime? GetLanguageModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            LanguagesDatahandler languagesDataHandler = new LanguagesDatahandler();
            DateTime? result = languagesDataHandler.GetLanguageModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}