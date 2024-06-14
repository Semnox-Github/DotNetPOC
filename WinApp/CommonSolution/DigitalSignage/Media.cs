/********************************************************************************************
 * Project Name - Media BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        09-Feb-2017      Raghuveera     Created 
 *2.40        27-Sep-2018      Jagan          Added new constructor Media and methods DeleteMediaList
 *2.70.2        31-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.90        12-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, and 
 *                                                 List class changes as per 3 tier standards.
*2.110.0     27-Nov-2020       Prajwal S          Modified : Constructor with Id parameter
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for Media class.
    /// </summary>
    public class Media
    {
        private MediaDTO media;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of Media class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private Media(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.media = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the media id as the parameter
        /// Would fetch the media object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="mediaId">Asset Type id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public Media(ExecutionContext executionContext, int mediaId, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, mediaId, sqltransaction);
            MediaDataHandler mediaDataHandler = new MediaDataHandler(sqltransaction);
            media = mediaDataHandler.GetMedia(mediaId);
            if (media == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "media", mediaId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the media id as the parameter
        /// Would fetch the media object from the database based on the contentGuid passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="contentGuid">Asset Type id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public Media(ExecutionContext executionContext, string contentGuid, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(contentGuid, executionContext, sqltransaction);
            MediaDataHandler mediaDataHandler = new MediaDataHandler(sqltransaction);
            this.media = mediaDataHandler.GetMediaByGuid(contentGuid);
            if (media == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "media", contentGuid);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(media);
        }

        /// <summary>
        /// Creates media object using the MediaDTO
        /// </summary>
        /// <param name="media">MediaDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public Media(ExecutionContext executionContext, MediaDTO media)
            : this(executionContext)
        {
            log.LogMethodEntry(media, executionContext);
            this.media = media;
            log.LogMethodExit(media);
        }

        /// <summary>
        ///  Saves the media
        /// Checks if the MediaId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqltransaction">sqltransaction</param>
        public void Save(SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(sqltransaction);
            MediaDataHandler mediaDataHandler = new MediaDataHandler(sqltransaction);
            if (media.IsChanged == false && media.MediaId > -1)
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

            if (media.MediaId < 0)
            {
                media = mediaDataHandler.InsertMedia(media, executionContext.GetUserId(), executionContext.GetSiteId());
                media.AcceptChanges();
            }
            else
            {
                if (media.IsChanged)
                {
                    media = mediaDataHandler.UpdateMedia(media, executionContext.GetUserId(), executionContext.GetSiteId());
                    media.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the MediaDTO
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
        /// Gets the DTO
        /// </summary>
        public MediaDTO GetMedia { get { return media; } }
    }

    /// <summary>
    /// Manages the list of media
    /// </summary>
    public class MediaList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private List<MediaDTO> mediaDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor of Media class 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public MediaList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);            
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="mediadToList">mediadToList</param>
        /// <param name="executionContext">executionContext</param>
        public MediaList(ExecutionContext executionContext, List<MediaDTO> mediaDTOList) : this(executionContext)
        {
            log.LogMethodEntry(mediaDTOList, executionContext);
            this.mediaDTOList = mediaDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the media list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<MediaDTO> GetAllMedias(List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MediaDataHandler mediaDataHandler = new MediaDataHandler(sqlTransaction);
            this.mediaDTOList = mediaDataHandler.GetMediaList(searchParameters);
            log.LogMethodExit(mediaDTOList);
            return mediaDTOList;
        }

        /// <summary>
        /// Saves the mediaDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (mediaDTOList == null ||
                mediaDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < mediaDTOList.Count; i++)
            {
                var mediaDTO = mediaDTOList[i];
                if (mediaDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    Media media = new Media(executionContext, mediaDTO);
                    media.Save(sqlTransaction);
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
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving mediaDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("mediaDTO", mediaDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
