/********************************************************************************************
 * Project Name - Waiver
 * Description  - Business logic of waiverSet
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       1-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.2      03-Oct-2019      Girish Kundar    Waiver phase 2 changes
 *2.70.2      06-Feb-2020      Divya A          Changes for WMS 
 *2.110.0    03-Dec-2020      Girish Kundar   Modified : 3 tier changes /NUnit test
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 *********************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Linq;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Waiver
{    /// <summary>
     /// Business logic for WaiverSet class.
     /// </summary>
    public class WaiverSetBL
    {
        private WaiverSetDTO waiverSetDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        private WaiverSetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the  waiverSet id as the parameter
        /// Would fetch the waiver set object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public WaiverSetBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(id);
            WaiverSetDataHandler waiverSetDataHandler = new WaiverSetDataHandler(sqlTransaction);
            waiverSetDTO = waiverSetDataHandler.GetWaiverSetDTO(id);
            if (waiverSetDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "WaiverSet", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor of WaiverSetBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="waiverSetDTO">WaiverSetDTO</param>
        public WaiverSetBL(ExecutionContext executionContext, WaiverSetDTO waiverSetDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, waiverSetDTO);
            this.waiverSetDTO = waiverSetDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (waiverSetDTO.WaiverSetId > -1 && waiverSetDTO.IsActive == false)
            {
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.WAIVER_SET_ID, waiverSetDTO.WaiverSetId.ToString()));
                ProductsList productsList = new ProductsList(executionContext);
                List<ProductsDTO> productsDTOList = productsList.GetProductsList(searchParameters, 0, 0, false, false, sqlTransaction, false);
                if (productsDTOList != null && productsDTOList.Any())
                {
                    log.Debug("Active Product exists for this Waiver.");
                    validationErrorList.Add(new ValidationError("WaiverSet", "WaiverSetId", MessageContainerList.GetMessage(executionContext, 4753)));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the WaiverSet
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            WaiverSetDataHandler waiverSetDataHandler = new WaiverSetDataHandler(sqlTransaction);
            if (waiverSetDTO.WaiverSetId < 0)
            {
                waiverSetDTO = waiverSetDataHandler.InsertWaiverSet(waiverSetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                waiverSetDTO.AcceptChanges();
            }
            else
            {
                if (waiverSetDTO.IsChanged)
                {
                    waiverSetDTO = waiverSetDataHandler.UpdateWaiverSet(waiverSetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    waiverSetDTO.AcceptChanges();
                }
            }
            if (waiverSetDTO.WaiverSetDetailDTOList != null)
            {
                foreach (WaiversDTO waiverSetDetailDTO in waiverSetDTO.WaiverSetDetailDTOList)
                {
                    waiverSetDetailDTO.WaiverSetId = waiverSetDTO.WaiverSetId;
                    WaiversBL waiverSetDetailBL = new WaiversBL(executionContext, waiverSetDetailDTO);
                    waiverSetDetailBL.Save();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the reference count of WaiverSet
        /// </summary>
        public int RefWaiverCount(int WaiverSetId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(WaiverSetId);
            WaiverSetDataHandler waiverSetDataHandler = new WaiverSetDataHandler(sqlTransaction);
            int refCount = 0;
            refCount = waiverSetDataHandler.GetWaiverSetReferenceCount(WaiverSetId);
            log.LogMethodExit(refCount);
            return refCount;
        }

        public WaiverSetDTO WaiverSetDTO { get { return waiverSetDTO; } }
    }
    /// <summary>
    /// Manages the list of WaiverSet
    /// </summary>
    public class WaiverSetListBL
    {

        private List<WaiverSetDTO> waiverSetDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LookupValuesList serverTimeObject;

        public WaiverSetListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public WaiverSetListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            serverTimeObject = new LookupValuesList(executionContext);
            this.waiverSetDTOList = new List<WaiverSetDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="waiverSetDTOList"></param>
        /// <param name="executionContext"></param>
        public WaiverSetListBL(ExecutionContext executionContext, List<WaiverSetDTO> waiverSetDTOList)
        {
            log.LogMethodEntry(waiverSetDTOList, executionContext);
            this.executionContext = executionContext;
            serverTimeObject = new LookupValuesList(executionContext);
            this.waiverSetDTOList = waiverSetDTOList;
        }
        /// <summary>
        /// Returns the Waiverset and Waiverdetails
        /// </summary>
        public List<WaiverSetDTO> GetWaiverSetDTOList(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, bool removeIncompleteRecords = false, bool loadWaiverFileContent = false, SqlTransaction sqlTransaction = null, List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> signingOptionsSearchParam = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords, removeIncompleteRecords, loadWaiverFileContent);
            WaiverSetDataHandler waiverSetDataHandler = new WaiverSetDataHandler(sqlTransaction);
            waiverSetDTOList = waiverSetDataHandler.GetWaiverSetDTOList(searchParameters);
            if (loadChildRecords)
            {
                BuildChildRecords(loadActiveChildRecords, loadWaiverFileContent, signingOptionsSearchParam);
                if (removeIncompleteRecords)
                {
                    waiverSetDTOList = RemoveInCompleteWaiverSet(waiverSetDTOList);
                }
            }
            log.LogMethodExit(waiverSetDTOList);
            return waiverSetDTOList;
        }

        private void BuildChildRecords(bool loadActiveChildRecords, bool loadWaiverFileContent, List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> signingOptionsSearchParam = null)
        {
            log.LogMethodEntry(loadActiveChildRecords, loadWaiverFileContent);
            string waiverSetIdList = string.Empty;
            if (waiverSetDTOList != null && waiverSetDTOList.Any())
            {

                foreach (WaiverSetDTO WaiverSetDTO in waiverSetDTOList)
                {
                    waiverSetIdList = waiverSetIdList + WaiverSetDTO.WaiverSetId + ",";
                }
                if (string.IsNullOrEmpty(waiverSetIdList) == false && waiverSetIdList.Length > 1)
                {
                    waiverSetIdList = waiverSetIdList.Substring(0, waiverSetIdList.Length - 1); //remove trailing ","
                }
                LoadWaiversDTOList(loadActiveChildRecords, loadWaiverFileContent, waiverSetIdList);
                LoadWaiverSetSignOptionDTOList(waiverSetIdList, signingOptionsSearchParam);
            }

            log.LogMethodExit();
        }

        private void LoadWaiversDTOList(bool loadActiveChildRecords, bool loadWaiverFileContent, string waiverSetIdList)
        {
            log.LogMethodEntry(loadActiveChildRecords, loadWaiverFileContent, waiverSetIdList);
            WaiversListBL waiverSetDetailListBL = new WaiversListBL(executionContext);
            List<KeyValuePair<WaiversDTO.SearchByWaivers, string>> searchParams = new List<KeyValuePair<WaiversDTO.SearchByWaivers, string>>();
            searchParams.Add(new KeyValuePair<WaiversDTO.SearchByWaivers, string>(WaiversDTO.SearchByWaivers.WAIVERSET_ID_LIST, waiverSetIdList));
            if (loadActiveChildRecords)
            {
                searchParams.Add(new KeyValuePair<WaiversDTO.SearchByWaivers, string>(WaiversDTO.SearchByWaivers.IS_ACTIVE, "1"));
            }
            List<WaiversDTO> waiverSetDetailsList = waiverSetDetailListBL.GetWaiversDTOList(searchParams);
            if (waiverSetDetailsList != null && waiverSetDetailsList.Any())
            {
                foreach (WaiversDTO waiverSetDetailItem in waiverSetDetailsList)
                {
                    waiverSetDetailItem.ObjectTranslationsDTOList = LoadWaiverSetDetailLanguages(waiverSetDetailItem.Guid);
                }
                //waiverSetDetailsList = waiverSetDetailsList.OrderBy(waivers => waivers.WaiverSetId).ToList();
                foreach (WaiverSetDTO WaiverSetDTO in waiverSetDTOList)
                {
                    List<WaiversDTO> waiverDTOForTheSet = waiverSetDetailsList.Where(waivers => waivers.WaiverSetId == WaiverSetDTO.WaiverSetId).ToList();
                    if (waiverDTOForTheSet != null && waiverDTOForTheSet.Any())
                    {
                        WaiverSetDTO.WaiverSetDetailDTOList = new List<WaiversDTO>(waiverDTOForTheSet);
                    }
                }
            }
            if (loadWaiverFileContent)
            {
                GenericUtils genericUtils = new GenericUtils();
                foreach (WaiverSetDTO WaiverSetDTO in waiverSetDTOList)
                {
                    if (WaiverSetDTO.WaiverSetDetailDTOList != null && WaiverSetDTO.WaiverSetDetailDTOList.Any())
                    {
                        for (int i = 0; i < WaiverSetDTO.WaiverSetDetailDTOList.Count; i++)
                        {
                            WaiversBL waiversBL = new WaiversBL(executionContext, WaiverSetDTO.WaiverSetDetailDTOList[i]);
                            waiversBL.GetWaiverFileContentInBase64Format();

                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LoadWaiverSetSignOptionDTOList(string waiverSetIdList, List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> signingOptionsSearchParam = null)
        {
            log.LogMethodEntry(waiverSetIdList);
            WaiverSetSigningOptionsListBL signingOptionList = new WaiverSetSigningOptionsListBL(executionContext);
            List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_ID_LIST, waiverSetIdList));
            searchParam.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            if (signingOptionsSearchParam != null)
                searchParam.AddRange(signingOptionsSearchParam);

            List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOList = signingOptionList.GetWaiverSetSigningOptionsList(searchParam);
            if (waiverSetSigningOptionsDTOList != null && waiverSetSigningOptionsDTOList.Count > 0)
            {
                foreach (WaiverSetDTO WaiverSetDTO in waiverSetDTOList)
                {
                    List<WaiverSetSigningOptionsDTO> signOptionsForTheSet = waiverSetSigningOptionsDTOList.Where(signOptions => signOptions.WaiverSetId == WaiverSetDTO.WaiverSetId).ToList();
                    if (signOptionsForTheSet != null && signOptionsForTheSet.Any())
                    {
                        WaiverSetDTO.WaiverSetSigningOptionDTOList = new List<WaiverSetSigningOptionsDTO>(signOptionsForTheSet);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns WaiverLanguages collection 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private List<ObjectTranslationsDTO> LoadWaiverSetDetailLanguages(string guid)
        {
            List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
            List<ObjectTranslationsDTO> objectTranslationsDTOSortableList = new List<ObjectTranslationsDTO>();
            if (guid != null && guid != "")
            {
                ObjectTranslationsList objectTransaltionListBL = new ObjectTranslationsList(executionContext);
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, guid.ToString()));
                List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTransaltionListBL.GetAllObjectTranslations(searchParameters);
                if (objectTranslationsDTOList != null)
                {
                    objectTranslationsDTOSortableList = new List<ObjectTranslationsDTO>(objectTranslationsDTOList);
                }
            }
            return objectTranslationsDTOSortableList;
        }


        private List<WaiverSetDTO> RemoveInCompleteWaiverSet(List<WaiverSetDTO> waiverSetDTOList)
        {
            log.LogMethodEntry(waiverSetDTOList);
            if (waiverSetDTOList != null && waiverSetDTOList.Any())
            {
                DateTime serverTime = serverTimeObject.GetServerDateTime();
                for (int j = 0; j < waiverSetDTOList.Count; j++)
                {
                    if (waiverSetDTOList[j].WaiverSetDetailDTOList == null
                         || waiverSetDTOList[j].WaiverSetSigningOptionDTOList == null
                         || (waiverSetDTOList[j].WaiverSetDetailDTOList != null && waiverSetDTOList[j].WaiverSetDetailDTOList.Exists(ws => ws.IsActive == true) == false)
                         || (waiverSetDTOList[j].WaiverSetSigningOptionDTOList != null && waiverSetDTOList[j].WaiverSetSigningOptionDTOList.Count == 0))
                    {
                        waiverSetDTOList.RemoveAt(j);
                        j = j - 1;
                    }
                    else
                    {
                        if (waiverSetDTOList[j].WaiverSetDetailDTOList != null && waiverSetDTOList[j].WaiverSetDetailDTOList.Any())
                        {
                            for (int i = 0; i < waiverSetDTOList[j].WaiverSetDetailDTOList.Count; i++)
                            {
                                if (waiverSetDTOList[j].WaiverSetDetailDTOList[i].EffectiveDate != null
                                    && waiverSetDTOList[j].WaiverSetDetailDTOList[i].EffectiveDate > serverTime)
                                {
                                    waiverSetDTOList[j].WaiverSetDetailDTOList.RemoveAt(i);
                                    i = i - 1;
                                }
                            }
                        }
                        if (waiverSetDTOList[j].WaiverSetDetailDTOList == null || waiverSetDTOList[j].WaiverSetDetailDTOList.Any() == false)
                        {
                            waiverSetDTOList.RemoveAt(j);
                            j = j - 1;
                        }
                    }
                }
            }
            log.LogMethodExit(waiverSetDTOList);
            return waiverSetDTOList;
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveUpdateWaivers()
        {
            try
            {
                log.LogMethodEntry();
                if (waiverSetDTOList != null)
                {
                    foreach (WaiverSetDTO waiverSetDTO in waiverSetDTOList)
                    {
                        WaiverSetBL waiverSetBL = new WaiverSetBL(executionContext, waiverSetDTO);
                        waiverSetBL.Save();
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }


        public DateTime? GetWaiverSetLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            WaiverSetDataHandler waiverSetDataHandler = new WaiverSetDataHandler(sqlTransaction);
            DateTime? result = waiverSetDataHandler.GetWaiverSetLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Represents foreign key error that occur during application execution. 
    /// </summary>
    public class ForeignKeyException : Exception
    {
        /// <summary>
        /// Default constructor of ForeignKeyException.
        /// </summary>
        public ForeignKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ForeignKeyException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ForeignKeyException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
