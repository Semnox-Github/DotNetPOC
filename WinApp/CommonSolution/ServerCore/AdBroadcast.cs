/********************************************************************************************
 * Project Name - AdBroadcast
 * Description  - Business logic of AdBroadcast
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Jagan Mohana Rao        Created
 *2.70.2      26-Jan-2020   Girish Kundar           Modified : Changed to Standard format 
 *2.90       20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ServerCore
{
    public class AdBroadcast
    {
        private AdBroadcastDTO adBroadcastDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private AdBroadcast(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="adBroadcastDTO"></param>
        public AdBroadcast(ExecutionContext executionContext, AdBroadcastDTO adBroadcastDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, adBroadcastDTO);
            this.adBroadcastDTO = adBroadcastDTO;            
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="adId">Parameter of the type adId</param>
        public AdBroadcast(ExecutionContext executionContext, int id)
        {
            log.LogMethodEntry(adBroadcastDTO);
            AdBroadcastDataHandler adBroadcastDataHandler = new AdBroadcastDataHandler();
            this.adBroadcastDTO = adBroadcastDataHandler.GetAdBroadcast(id);
            log.LogMethodExit(adBroadcastDTO);
        }

        /// <summary>
        /// Saves the AdBroadcast
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        internal void Save()
        {
            log.LogMethodEntry();
            if (adBroadcastDTO.IsChanged == false &&
                adBroadcastDTO.Id > -1)
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
            AdBroadcastDataHandler adBroadcastDataHandler = new AdBroadcastDataHandler();
            if (adBroadcastDTO.IsActive)
            {
                if (adBroadcastDTO.Id <= 0)
                {
                    adBroadcastDTO = adBroadcastDataHandler.Insert(adBroadcastDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    adBroadcastDTO.AcceptChanges();
                }
                else
                {
                    if (adBroadcastDTO.IsChanged == true)
                    {
                        adBroadcastDTO = adBroadcastDataHandler.Update(adBroadcastDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        adBroadcastDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if(adBroadcastDTO.Id >= 0)
                {
                    adBroadcastDataHandler.Delete(adBroadcastDTO.Id);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the adBroadcastDTO
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
        /// get AdBroadcastDTO Object
        /// </summary>
        public AdBroadcastDTO GetAdBroadcastDTO
        {
            get { return adBroadcastDTO; }
        }

        /// <summary>
        /// set AdBroadcastDTO Object        
        /// </summary>
        public AdBroadcastDTO SetAdBroadcastDTO
        {
            set { adBroadcastDTO = value; }
        }

    }
    /// <summary>
    /// Manages the list of Ads
    /// </summary>
    public class AdBroadcastList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AdBroadcastDTO> adBroadcastDTOList = new List<AdBroadcastDTO>();
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AdBroadcastList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="adBroadcastDTOList">adBroadcast DTO List as parameter </param>
        public AdBroadcastList(ExecutionContext executionContext,
                                               List<AdBroadcastDTO> adBroadcastDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, adBroadcastDTOList);
            this.adBroadcastDTOList = adBroadcastDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AdBroadcast list
        /// </summary>
        public List<AdBroadcastDTO> GetAllAdBroadcast(List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters , sqlTransaction);
            AdBroadcastDataHandler adBroadcastDataHandler = new AdBroadcastDataHandler(sqlTransaction);
            this.adBroadcastDTOList = adBroadcastDataHandler.GetAdBroadcastList(searchParameters,sqlTransaction);
            log.LogMethodExit(adBroadcastDTOList);
            return adBroadcastDTOList;
        }
        public List<AdBroadcastDTO> GetAllPopulateAdBroadcast(List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AdBroadcastDataHandler adBroadcastDataHandler = new AdBroadcastDataHandler(sqlTransaction);
            List<AdBroadcastDTO> adBroadcastDTOList = adBroadcastDataHandler.GetAllPopulateAdBroadcast(searchParameters, sqlTransaction);
            log.LogMethodExit(adBroadcastDTOList);
            return adBroadcastDTOList;
        }

        /// <summary>
        /// Saves the adBroadcast DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (adBroadcastDTOList == null ||
                adBroadcastDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < adBroadcastDTOList.Count; i++)
            {
                var adBroadcastDTO = adBroadcastDTOList[i];
                if (adBroadcastDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AdBroadcast adBroadcast = new AdBroadcast(executionContext, adBroadcastDTO);
                    adBroadcast.Save();
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving adBroadcastDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("adBroadcastDTO", adBroadcastDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}