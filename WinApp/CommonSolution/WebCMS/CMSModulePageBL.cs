/********************************************************************************************
* Project Name - CMSModulePage DTO Programs 
* Description  - Mapper between Module Page
* 
**************
**Version Log
**************
*Version     Date           Modified By        Remarks          
*********************************************************************************************
*2.80        06-May-2020    Indrajeet K        Created  
********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModulePageBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CMSModulePageDTO cmsModulePageDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Private Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private CMSModulePageBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Public Parameterized Constructor with executionContext and DTO as parameters 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmsModulePageDTO"></param>
        public CMSModulePageBL(ExecutionContext executionContext, CMSModulePageDTO cmsModulePageDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cmsModulePageDTO);
            this.cmsModulePageDTO = cmsModulePageDTO;
            log.LogMethodExit();
        }

        public CMSModulePageBL(ExecutionContext executionContext, int id,bool loadChildRecords = false, bool activeChildRecords = false,
        SqlTransaction sqlTransaction = null)
        : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id,loadChildRecords,activeChildRecords, sqlTransaction);
            CMSModulePageDataHandler cMSModulePageDataHandler = new CMSModulePageDataHandler(sqlTransaction);
            cmsModulePageDTO = cMSModulePageDataHandler.GetCMSModulePageDTO(id);
            if (cmsModulePageDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "cmsModulePage", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if(loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> cMSPagesSearchParameter = new List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>>();
            cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.PAGE_ID, cmsModulePageDTO.PageId.ToString()));
            if (activeChildRecords)
            {
                cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.ACTIVE, "1"));
            }
            CMSPagesBLList cMSPagesList = new CMSPagesBLList(executionContext);
            cmsModulePageDTO.CMSPagesDTOList = cMSPagesList.GetAllPages(cMSPagesSearchParameter, true , activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Used to save - It may by Insert Or Update based on condition.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsModulePageDTO.IsChangedRecursive == false
                && cmsModulePageDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSModulePageDataHandler cMSModulePageDataHandler = new CMSModulePageDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            
            if (cmsModulePageDTO.Id < 0)
            {
                cmsModulePageDTO = cMSModulePageDataHandler.InsertCMSModulePage(cmsModulePageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModulePageDTO.AcceptChanges();
            }
            else if (cmsModulePageDTO.IsChanged)
            {                        
                cmsModulePageDTO = cMSModulePageDataHandler.UpdateCMSModulePage(cmsModulePageDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModulePageDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the details
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
    }
    public class CMSModulePageBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSModulePageDTO> cMSModulePageDTOList = new List<CMSModulePageDTO>();
        private List<CMSPagesDTO> cMSPagesDTOList = new List<CMSPagesDTO>();
        private ExecutionContext executionContext;

        public CMSModulePageBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSModulePageBLList(ExecutionContext executionContext, 
            List<CMSModulePageDTO> cMSModulePageDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cMSModulePageDTOList = cMSModulePageDTOList;           
            log.LogMethodExit();
        }

        public List<CMSModulePageDTO> GetAllCMSModulePage(List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                CMSModulePageDataHandler cmsModulePageDataHandler = new CMSModulePageDataHandler(sqlTransaction);
                cMSModulePageDTOList = cmsModulePageDataHandler.GetCMSModulePageDTOList(searchParameters);
                if (cMSModulePageDTOList != null && cMSModulePageDTOList.Any() && loadChildRecords)
                {
                    Build(cMSModulePageDTOList, loadChildRecords, activeChildRecords, sqlTransaction);
                }
                log.LogMethodExit(cMSModulePageDTOList);
                return cMSModulePageDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at  GetAllCMSModulePage(searchParameters) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        private void Build(List<CMSModulePageDTO> cMSModulePageDTOList, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSModulePageDTOList, loadChildRecords,activeChildRecords,sqlTransaction);
            if (cMSModulePageDTOList != null && cMSModulePageDTOList.Any())
            {
                foreach (CMSModulePageDTO cMSModulePageDTO in cMSModulePageDTOList)
                {
                    List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> cMSPagesSearchParameter = new List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>>();
                    cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.PAGE_ID, cMSModulePageDTO.PageId.ToString()));
                    
                    if (activeChildRecords)
                    {
                        cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.ACTIVE, "1"));                        
                    }
                    CMSPagesBLList cMSPagesList = new CMSPagesBLList(executionContext);
                    cMSModulePageDTO.CMSPagesDTOList = cMSPagesList.GetAllPages(cMSPagesSearchParameter, loadChildRecords, activeChildRecords, sqlTransaction);                    
                }
            }
            log.LogMethodExit();


            //if (cMSModulePageDTOList != null && cMSModulePageDTOList.Any())
            //{
            //    Dictionary<int, CMSModulePageDTO> cMSModulePageDictionary = new Dictionary<int, CMSModulePageDTO>();
            //    StringBuilder sb = new StringBuilder("");
            //    string cMSModulePageList;
            //    for (int i = 0; i < cMSModulePageDTOList.Count; i++)
            //    {
            //        if (cMSModulePageDTOList[i].Id == -1 ||
            //            cMSModulePageDictionary.ContainsKey(cMSModulePageDTOList[i].Id))
            //        {
            //            continue;
            //        }
            //        if (i != 0)
            //        {
            //            sb.Append(",");
            //        }
            //        sb.Append(cMSModulePageDTOList[i].PageId.ToString());
            //        cMSModulePageDictionary.Add(cMSModulePageDTOList[i].PageId, cMSModulePageDTOList[i]);
            //    }

            //    cMSModulePageList = sb.ToString();

            //    List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> cMSPagesSearchParameter = new List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>>();
            //    cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.PAGE_ID_LIST, cMSModulePageList.ToString()));
            //    if (activeChildRecords)
            //    {
            //        cMSPagesSearchParameter.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.ACTIVE, "1"));
            //    }

            //    CMSPagesBLList cMSPagesList = new CMSPagesBLList(executionContext);
            //    cMSPagesDTOList = cMSPagesList.GetAllPages(cMSPagesSearchParameter, activeChildRecords, sqlTransaction);

            //    if (cMSPagesDTOList != null && cMSPagesDTOList.Any())
            //    {
            //        foreach (var cMSPagesDTO in cMSPagesDTOList)
            //        {
            //            if (cMSModulePageDictionary.ContainsKey(cMSPagesDTO.PageId))
            //            {
            //                if (cMSModulePageDictionary[cMSPagesDTO.PageId].CMSPagesDTOList == null)
            //                {
            //                    cMSModulePageDictionary[cMSPagesDTO.PageId].CMSPagesDTOList = new List<CMSPagesDTO>();
            //                }
            //                cMSModulePageDictionary[cMSPagesDTO.PageId].CMSPagesDTOList.Add(cMSPagesDTO);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Saves the  list of cMSModulePageDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSModulePageDTOList == null ||
                cMSModulePageDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSModulePageDTOList.Count; i++)
            {
                var cMSModulePageDTO = cMSModulePageDTOList[i];
                try
                {
                    CMSModulePageBL cMSModulePageBL = new CMSModulePageBL(executionContext, cMSModulePageDTO);
                    cMSModulePageBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSModulePageDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSModulePageDTO", cMSModulePageDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
