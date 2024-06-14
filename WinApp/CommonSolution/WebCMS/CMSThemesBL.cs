/********************************************************************************************
 * Project Name - WebCMS
 * Description  - Business logic class for CMSThemes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.80       22-Oct-2019   Mushahid Faizan    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.WebCMS
{
    public class CMSThemesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private CMSThemesDTO cmsThemesDTO;

        private CMSThemesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the cmsThemesDTO parameter
        /// </summary>
        /// <param name="cmsThemesDTO">Parameter of the type cmsThemesDTO</param>       
        /// <param name="executionContext">ExecutionContext</param>       
        public CMSThemesBL(ExecutionContext executionContext, CMSThemesDTO cmsThemesDTO) : 
            this(executionContext)
        {
            log.LogMethodEntry(cmsThemesDTO, executionContext);
            this.cmsThemesDTO = cmsThemesDTO;
            log.LogMethodExit();
        }

        ///    *******NOT TESTED ******** Uncomment this code when Insert/Update is required.
        ///    
        ///// <summary>
        /////    Saves the CMSThemesDTO
        ///// Checks if the ThemeId is not less than or equal to 0
        /////     If it is less than or equal to 0, then inserts
        /////     else updates
        ///// </summary>
        //public void Save()
        //{
        //    log.LogMethodEntry();
        //    CMSThemesDataHandler cmsThemesDataHandler = new CMSThemesDataHandler();
        //    if (cmsThemesDTO.ThemeId == -1)
        //    {
        //        int themeId = cmsThemesDataHandler.Insert(cmsThemesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //        cmsThemesDTO.ThemeId = themeId;
        //    }
        //    else
        //    {
        //        if (cmsThemesDTO.IsChanged)
        //        {
        //            cmsThemesDataHandler.Update(cmsThemesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
        //            cmsThemesDTO.AcceptChanges();
        //        }
        //    }
        //    log.LogMethodExit();
        //}

    }
    public class CMSThemesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CMSThemesDTO> cmsThemesDTOList = new List<CMSThemesDTO>();

        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CMSThemesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cmsThemesDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the cmsThemesDTOList
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<CMSThemesDTO> GetAllCMSThemes(List<KeyValuePair<CMSThemesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CMSThemesDataHandler cMSThemesDataHandler = new CMSThemesDataHandler();
            cmsThemesDTOList = cMSThemesDataHandler.GetCMSThemeDTOList(searchParameters);
            log.LogMethodExit(cmsThemesDTOList);
            return cmsThemesDTOList;
        }

        ///    *******NOT TESTED ******** Uncomment this code when Insert/Update is required.

        ///// <summary>
        ///// Save or CMSThemes  details
        ///// </summary>
        //public void Save()
        //{
        //    log.LogMethodEntry();
        //    if (cmsThemesDTOList != null && cmsThemesDTOList.Any())
        //    {
        //        foreach (CMSThemesDTO cmsThemesDTO in cmsThemesDTOList)
        //        {
        //            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //            {
        //                try
        //                {
        //                    parafaitDBTrx.BeginTransaction();
        //                    CMSThemesBL cmsThemesBL = new CMSThemesBL(cmsThemesDTO, executionContext);
        //                    cmsThemesBL.Save(parafaitDBTrx.SQLTrx);
        //                    parafaitDBTrx.EndTransaction();
        //                }
        //                catch (ValidationException valEx)
        //                {
        //                    parafaitDBTrx.RollBack();
        //                    log.Error(valEx);
        //                    throw valEx;
        //                }
        //                catch (Exception ex)
        //                {
        //                    parafaitDBTrx.RollBack();
        //                    log.Error(ex);
        //                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                    throw new Exception(ex.Message, ex);
        //                }
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}
    }
}
