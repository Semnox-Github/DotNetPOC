/********************************************************************************************
 * Project Name - Waiver
 * Description  - Business logic of WaiverSetDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       21-Sep-2016     Amaresh          Created 
 *2.70       1-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.2     03-Oct-2019      Girish Kundar    Waiver phase 2 changes
 *2.110.0    03-Dec-2020      Girish Kundar   Modified : 3 tier changes /NUnit test
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// Business logic for WaiversBL class.
    /// </summary>
    public class WaiversBL
    {
        private WaiversDTO waiversDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        private WaiversBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);    
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametrized constructor of WaiversBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="waiversDTO">waiversDTO</param>
        public WaiversBL(ExecutionContext executionContext, WaiversDTO waiversDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, waiversDTO);
            this.waiversDTO = waiversDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the  waiverSetDetail id as the parameter
        /// Would fetch the waiversetDetail object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public WaiversBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            WaiversDataHandler waiversDataHandler = new WaiversDataHandler(sqlTransaction);
            waiversDTO = waiversDataHandler.GetWaiversDTO(id);
            if (waiversDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "WaiverSetDeatil", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the WaiverSetDetail
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            WaiversDataHandler waiversDataHandler = new WaiversDataHandler(sqlTransaction);
            if (waiversDTO.WaiverSetDetailId < 0)
            {
                waiversDTO = waiversDataHandler.InsertWaiverSetDetail(waiversDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                waiversDTO.AcceptChanges();
            }
            else
            {
                if (waiversDTO.IsChanged)
                {
                    waiversDTO = waiversDataHandler.UpdateWaiverSetDetail(waiversDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    waiversDTO.AcceptChanges();
                }
            }
            if (waiversDTO.ObjectTranslationsDTOList != null && waiversDTO.ObjectTranslationsDTOList.Count > 0)
            {
                foreach (ObjectTranslationsDTO objectTranslationsDTO in waiversDTO.ObjectTranslationsDTOList)
                {
                    if (objectTranslationsDTO.IsChanged || objectTranslationsDTO.Id < 0)
                    {
                        WaiversListBL waiversListBL = new WaiversListBL(executionContext);

                        List<KeyValuePair<WaiversDTO.SearchByWaivers, string>> waiverkeyValues = new List<KeyValuePair<WaiversDTO.SearchByWaivers, string>>();
                        waiverkeyValues.Add(new KeyValuePair<WaiversDTO.SearchByWaivers, string>(WaiversDTO.SearchByWaivers.WAIVERSETDETAIL_ID, Convert.ToString(waiversDTO.WaiverSetDetailId)));
                        var waiversList = waiversListBL.GetWaiversDTOList(waiverkeyValues);
                        if (waiversList != null)
                        {
                            foreach (WaiversDTO waiversDTORecord in waiversList)
                            {
                                objectTranslationsDTO.ElementGuid = waiversDTORecord.Guid;
                                ObjectTranslations objectTranslations = new ObjectTranslations(executionContext, objectTranslationsDTO);
                                objectTranslations.Save();
                            }
                        }
                    }
                }
            }

            log.LogMethodExit();
        }


        public WaiversDTO GetWaiversDTO { get { return waiversDTO; } }
        /// <summary>
        /// GetWaiverFileContentInBase64Format
        /// </summary>
        /// <returns></returns>
        public string GetWaiverFileContentInBase64Format()
        {
            log.LogMethodEntry();
            string base64FileContent = string.Empty;
            if (this.waiversDTO != null)
            {
                if (string.IsNullOrEmpty(waiversDTO.WaiverFileName) == false)
                {
                    GenericUtils genericUtils = new GenericUtils();
                    waiversDTO.WaiverFileContentInBase64Format = genericUtils.DownloadFileFromDBInBase64Format(executionContext, waiversDTO.WaiverFileName);
                    base64FileContent = waiversDTO.WaiverFileContentInBase64Format;
                    log.LogVariableState("waiversDTO.WaiverFileContentInBase64Format", waiversDTO.WaiverFileContentInBase64Format);
                }
            }
            log.LogMethodExit();
            return base64FileContent;

        }

    }
    /// <summary>
    /// Manages the list of WaiversListBL
    /// </summary>
    public class WaiversListBL
    {
        List<WaiversDTO> waiversDTOList;
        ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaiversListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.waiversDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Paramiterzied constructor with 2 params
        /// </summary>
        /// <param name="waiversDTOList"></param>
        /// <param name="executionContext"></param>
        public WaiversListBL(ExecutionContext executionContext, List<WaiversDTO> waiversDTOList)
        {
            log.LogMethodEntry(waiversDTOList, executionContext);
            this.executionContext = executionContext;
            this.waiversDTOList = waiversDTOList;
            log.LogMethodExit();
        }
        public List<WaiversDTO> GetWaiversDTOList(List<KeyValuePair<WaiversDTO.SearchByWaivers, string>> searchParameters, SqlTransaction sqlTransaction = null, bool showTranslations = false)
        {
            log.LogMethodEntry(searchParameters);
            WaiversDataHandler waiversDataHandler = new WaiversDataHandler(sqlTransaction);
            List<WaiversDTO> returnValue = waiversDataHandler.GetWaiversDTOList(searchParameters);

            if (returnValue != null && showTranslations)
            {
                List<ObjectTranslationsDTO> objectTranslationsDTOList = new List<ObjectTranslationsDTO>();

                List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchObjTranslationParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                searchObjTranslationParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.OBJECT, "WAIVERSETDETAILS"));
                ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext);
                objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchObjTranslationParameters);

                if (objectTranslationsDTOList != null && objectTranslationsDTOList.Count() > 0)
                {
                    foreach (WaiversDTO wSetDTO in returnValue)
                    {
                        if (objectTranslationsDTOList != null && objectTranslationsDTOList.Where(e => e.ElementGuid == wSetDTO.Guid.ToString()).Count() > 0)
                            wSetDTO.ObjectTranslationsDTOList = objectTranslationsDTOList.Where(e => e.ElementGuid == wSetDTO.Guid.ToString()).ToList();
                    }
                }

            }

            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }




}
