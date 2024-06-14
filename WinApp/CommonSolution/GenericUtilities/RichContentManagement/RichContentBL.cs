/********************************************************************************************
 * Project Name - RichContentBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019      Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.70.2        04-Feb-2020      Nitin Pai                   Guest App phase 2 changes
 *2.90.0        04-Jun-2020      Girish Kundar    Modified : Phase -2 REST API related changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    public class RichContentBL
    {
        private RichContentDTO richContentDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of RichContentBL class
        /// </summary>
        private RichContentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RichContent id as the parameter
        /// Would fetch the RichContent object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public RichContentBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            RichContentDataHandler richContentDataHandler = new RichContentDataHandler(sqlTransaction);
            richContentDTO = richContentDataHandler.GetRichContent(id);
            richContentDTO.ContentType = RichContentBL.GetRichContentType(richContentDTO.FileName);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RichContentBL object using the RichContentDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="richContentDTO">RichContentDTO object</param>
        public RichContentBL(ExecutionContext executionContext, RichContentDTO richContentDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, richContentDTO);
            this.richContentDTO = richContentDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RichContent
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            RichContentDataHandler richContentDataHandler = new RichContentDataHandler(sqlTransaction);
            if (richContentDTO.IsActive)
            {
                if (richContentDTO.Id < 0)
                {
                    richContentDTO = richContentDataHandler.InsertRichContent(richContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    richContentDTO.AcceptChanges();
                }
                else
                {
                    if (richContentDTO.IsChanged)
                    {
                        richContentDTO = richContentDataHandler.UpdateRichContent(richContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        richContentDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (richContentDTO.Id >= 0)
                {
                    richContentDataHandler.Delete(richContentDTO.Id);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RichContentDTO RichContentDTO
        {
            get
            {
                return richContentDTO;
            }
        }

        public static string GetRichContentType(string fileName)
        {
            log.LogMethodEntry(fileName);
            string fileExtension = Path.GetExtension(fileName).ToLower();
            string supportedImageExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.ico,*.eps,*.tif,*.tiff";

            if (supportedImageExtensions.Contains(fileExtension))
            {
                return "image/" + fileExtension.Replace(".", "");
            }
            else if (fileExtension == ".pdf")
            {
                return "application/pdf";
            }
            else if (fileExtension == ".mht" || fileExtension == ".mhtml")
            {
                return "message/rfc822";
            }
            else if (fileExtension == ".html" || fileExtension == ".htm")
            {
                return "text/html";
            }

            return "";
        }

        public static string GetBase64String(byte[] data)
        {
            return Convert.ToBase64String(data, 0, data.Length);
        }

    }

    /// <summary>
    /// Manages the list of RichContentListBL
    /// </summary>
    public class RichContentListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RichContentDTO> richContentDTOList = new List<RichContentDTO>();
        private readonly ExecutionContext executionContext;
        public RichContentListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public RichContentListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public RichContentListBL(ExecutionContext executionContext, List<RichContentDTO> richContentDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, richContentDTOList);
            this.richContentDTOList = richContentDTOList;
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (richContentDTOList == null ||
                richContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < richContentDTOList.Count; i++)
            {
                var richContentDTO = richContentDTOList[i];
                if (richContentDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RichContentBL richContentBL = new RichContentBL(executionContext, richContentDTO);
                    richContentBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving richContentDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackSurveyDTO", richContentDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetRichContentDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="trx"></param>
        /// <returns></returns>
        public List<RichContentDTO> GetRichContentDTOList(List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RichContentDataHandler richContentDataHandler = new RichContentDataHandler(sqlTransaction);
            List<RichContentDTO> richContentList = richContentDataHandler.GetRichContentDTOList(searchParameters);
            if (richContentList != null && richContentList.Any())
            {
                foreach (RichContentDTO richContentDTO in richContentList)
                {
                    richContentDTO.ContentType = RichContentBL.GetRichContentType(richContentDTO.FileName);
                }
            }
            log.LogMethodExit(richContentList);
            return richContentList;
        }

        //      /// <summary>
        //      /// GetRichContentDTOList
        //      /// </summary>
        //      /// <param name="id"></param>
        //      /// <param name="application"></param>
        //      /// <param name="module"></param>
        //      /// <returns></returns>
        //      public List<RichContentDTO> GetRichContentDTOList(int id, string application, string module, string chapter = "", int siteId = -1,SqlTransaction sqlTransaction =null)
        //{
        //	log.LogMethodEntry(application, module, chapter, siteId, sqlTransaction);

        //	ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
        //	RichContentDataHandler richContentDataHandler = new RichContentDataHandler(sqlTransaction);

        //	List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> appSearchParameters = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
        //	appSearchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.APPLICATION, application));
        //	appSearchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.MODULE, module));
        //          if(!string.IsNullOrEmpty(chapter))
        //          {
        //              appSearchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.CHAPTER, chapter));
        //          }
        //          if (siteId > -1)
        //          {
        //              appSearchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, siteId.ToString()));
        //          }
        //          List<ApplicationContentDTO> appContentList = applicationContentDataHandler.GetApplicationContentDTOList(appSearchParameters);
        //          List<RichContentDTO> richContentList = new List<RichContentDTO>();

        //          if (appContentList != null && appContentList.Count > 0)
        //	{
        //		List<KeyValuePair<RichContentDTO.SearchByParameters, string>> richSearchParameters = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
        //		richSearchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ID, appContentList[0].ContentId.ToString()));
        //		richContentList = richContentDataHandler.GetRichContentDTOList(richSearchParameters);
        //		if (richContentList != null && richContentList.Count > 0)
        //		{
        //                  foreach (RichContentDTO richContentDTO in richContentList)
        //                  {
        //                      richContentDTO.ContentType = RichContentBL.GetRichContentType(richContentDTO.FileName);
        //                  }
        //		}
        //	}

        //	log.LogMethodExit(null);
        //	return richContentList;
        //}



    }
}
