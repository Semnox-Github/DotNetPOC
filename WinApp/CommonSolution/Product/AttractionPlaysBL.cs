/********************************************************************************************
 * Project Name - AttractionPlays List BL
 * Description  - AttractionPlays List Methods   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 **********************************************************************************************************************************************************
 *2.60        04-Feb-2019   Nagesh Badiger           Created
 *2.70        27-Jun-2019   Akshay Gulaganji         Added DeleteAttractionPlays and DeleteAttractionPlaysList() methods
 **********************************************************************************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// will creates and modify the AttractionPlaysBL
    /// </summary>
    public class AttractionPlaysBL
    {
        private AttractionPlaysDTO attractionPlaysDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public AttractionPlaysBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.attractionPlaysDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AttractionPlay DTO parameter
        /// </summary>
        /// <param name="attractionPlayDTO">Parameter of the type AttractionPlayDTO</param>       
        public AttractionPlaysBL(ExecutionContext executionContext, AttractionPlaysDTO attractionPlaysDTO)
        {
            log.LogMethodEntry(executionContext, attractionPlaysDTO);
            this.attractionPlaysDTO = attractionPlaysDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AttractionPlays  
        /// AttractionPlay will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            AttractionPlaysDataHandler attractionPlayDataHandler = new AttractionPlaysDataHandler(sqlTransaction);
            if (attractionPlaysDTO.AttractionPlayId < 0)
            {
                int attractionPlayId = attractionPlayDataHandler.InsertAttractionPlays(attractionPlaysDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attractionPlaysDTO.AttractionPlayId = attractionPlayId;
                attractionPlaysDTO.AcceptChanges();
            }
            else
            {
                int attractionPlayId = attractionPlayDataHandler.UpdateAttractionPlays(attractionPlaysDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                attractionPlaysDTO.AcceptChanges();
            }
            log.LogMethodEntry();
        }

        /// <summary>
        /// Delete the AttractionPlays details based on attractionPlaysId
        /// </summary>
        /// <param name="attractionPlaysId"></param>        
        public void DeleteAttractionPlays(int attractionPlaysId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attractionPlaysId);
            try
            {
                AttractionPlaysDataHandler attractionPlayDataHandler = new AttractionPlaysDataHandler(sqlTransaction);
                attractionPlayDataHandler.DeleteAttractionPlays(attractionPlaysId);
                log.LogMethodExit();
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
    /// Manages the list of AttractionPlayDTO List
    /// </summary>
    public class AttractionPlaysBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AttractionPlaysDTO> attractionPlaysDTOsList;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AttractionPlaysBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.attractionPlaysDTOsList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="attractionPlaysDTOsList"></param>
        /// <param name="executionContext"></param>
        public AttractionPlaysBLList(ExecutionContext executionContext, List<AttractionPlaysDTO> attractionPlaysDTOsList)
        {
            log.LogMethodEntry(attractionPlaysDTOsList, executionContext);
            this.executionContext = executionContext;
            this.attractionPlaysDTOsList = attractionPlaysDTOsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AttractionPlayDTO List
        /// </summary>
        public List<AttractionPlaysDTO> GetAttractionPlaysDTOList(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AttractionPlaysDataHandler attractionPlaysDataHandler = new AttractionPlaysDataHandler(sqlTransaction);
            List<AttractionPlaysDTO> attractionPlaysBLListDTOList = attractionPlaysDataHandler.GetAttractionPlaysDTOList(searchParameters);
            log.LogMethodExit(attractionPlaysBLListDTOList);
            return attractionPlaysBLListDTOList;
        }
        public void SaveUpdateAttractionPlaysList()
        {
            try
            {
                log.LogMethodEntry();
                if (attractionPlaysDTOsList != null)
                {
                    foreach (AttractionPlaysDTO attractionPlayDto in attractionPlaysDTOsList)
                    {
                        AttractionPlaysBL attractionPlaysBL = new AttractionPlaysBL(executionContext, attractionPlayDto);
                        attractionPlaysBL.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Hard Deletions for Attraction Plays 
        /// </summary>
        public void DeleteAttractionPlaysList()
        {
            log.LogMethodEntry();
            if (attractionPlaysDTOsList != null && attractionPlaysDTOsList.Count > 0)
            {
                foreach (AttractionPlaysDTO attractionPlaysDTO in attractionPlaysDTOsList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            if(attractionPlaysDTO.IsChanged && attractionPlaysDTO.IsActive == false)
                            parafaitDBTrx.BeginTransaction();
                            AttractionPlaysBL attractionPlaysBL = new AttractionPlaysBL(executionContext);
                            attractionPlaysBL.DeleteAttractionPlays(attractionPlaysDTO.AttractionPlayId, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
                            }
                            else
                            {
                                throw;
                            }
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
            log.LogMethodExit();
        }
    }
}