/********************************************************************************************
* Project Name - CMSBanner  BL Class  
* Description  - Bussiness logic of the CMSBanner  BL class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith          Created 
*2.70        09-Jul-2019   Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
*                                                       LogMethodEntry() and LogMethodExit(). 
*2.80        08-May-2020   Indrajeet Kumar   Modified : GetAllCmsBanners() Method to Support Child & Save()                                                
********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSBannerBL
    {       
        private CMSBannersDTO cmsBannerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Private Parameterized Constructor
        /// </summary>
        private CMSBannerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSBannerBL(ExecutionContext executionContext, CMSBannersDTO cmsBannerDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cmsBannerDTO);            
            this.cmsBannerDTO = cmsBannerDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'cmsBanner"'  Request  ====>  ""cmsBanner"" DataHandler
        public CMSBannerBL(ExecutionContext executionContext, int bannerId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(bannerId, sqlTransaction);
           CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
           cmsBannerDTO = cmsBannerDataHandler.GetcmsBanner(bannerId);
            if (cmsBannerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSBanner", bannerId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsBannerDTO.IsChangedRecursive == false
                && cmsBannerDTO.BannerId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsBannerDTO.BannerId < 0)
            {
                cmsBannerDTO = cmsBannerDataHandler.InsertBanner(cmsBannerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsBannerDTO.AcceptChanges();
            }
            else if (cmsBannerDTO.IsChanged)
            {
                cmsBannerDTO = cmsBannerDataHandler.UpdateBanner(cmsBannerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsBannerDTO.AcceptChanges();
            }
            SaveCMSBannerChild(sqlTransaction); 
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

        /// <summary>
        /// Saves the child records : CMSBannerItem
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveCMSBannerChild(SqlTransaction sqlTransaction)
        {
            if (cmsBannerDTO.CMSBannerItemsDTOList != null &&
               cmsBannerDTO.CMSBannerItemsDTOList.Any())
            {
                List<CMSBannerItemsDTO> updatedCMSBannerItemsDTOList = new List<CMSBannerItemsDTO>();
                foreach (var cMSBannerItemsDTO in cmsBannerDTO.CMSBannerItemsDTOList)
                {
                    if (cMSBannerItemsDTO.BannerId != cmsBannerDTO.BannerId)
                    {
                        cMSBannerItemsDTO.BannerId = cmsBannerDTO.BannerId;
                    }
                    if (cMSBannerItemsDTO.IsChanged)
                    {
                        updatedCMSBannerItemsDTOList.Add(cMSBannerItemsDTO);                        
                    }
                }
                if (updatedCMSBannerItemsDTOList.Any())
                {
                    CMSBannerItemsBLList cMSBannerItemsBLList = new CMSBannerItemsBLList(executionContext, updatedCMSBannerItemsDTOList);
                    cMSBannerItemsBLList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Gets the CMSContentDTO
        /// </summary>
        public CMSBannersDTO GetCMSBannerDTO
        { 
            get { return cmsBannerDTO; }  
        }

        /// <summary>
        /// Delete the record from the database based on  bannerId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
                int id = cmsBannerDataHandler.bannerDelete(cmsBannerDTO.BannerId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                log.Error("Error  at Delete(bannerId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }            
        }        
    }
    
    public class CMSBannerBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSBannersDTO> cMSBannersDTOList = new List<CMSBannersDTO>();
        private List<CMSBannerItemsDTO> cMSBannerItemsDTOList = new List<CMSBannerItemsDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CMSBannerBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();           
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSBannerBLList(ExecutionContext executionContext, List<CMSBannersDTO> cMSBannersDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.cMSBannersDTOList = cMSBannersDTOList;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Get all the CMSBanner based on Search Request and return the List of object.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CMSBannersDTO> GetAllCmsBanners(List<KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>> searchParameters, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
                List<CMSBannersDTO> cmsBannersDTOList = cmsBannerDataHandler.GetBannersList(searchParameters);

                if (cmsBannersDTOList != null && cmsBannersDTOList.Any())
                {
                    Build(cmsBannersDTOList, activeChildRecords, sqlTransaction);
                }
                log.LogMethodExit(cmsBannersDTOList);
                return cmsBannersDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetAllCmsBanners(searchparameters) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }           
        }

        private void Build(List<CMSBannersDTO> cmsBannersDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cmsBannersDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CMSBannersDTO> cmsBannersIdDictionary = new Dictionary<int, CMSBannersDTO>();
            string cmsBannerIdSet = string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < cmsBannersDTOList.Count; i++)
            {
                if (cmsBannersDTOList[i].BannerId == -1 ||
                    cmsBannersIdDictionary.ContainsKey(cmsBannersDTOList[i].BannerId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(cmsBannersDTOList[i].BannerId);
                cmsBannersIdDictionary.Add(cmsBannersDTOList[i].BannerId, cmsBannersDTOList[i]);
            }
            cmsBannerIdSet = sb.ToString();
            CMSBannerItemsBLList cMSBannerItemsBLList = new CMSBannerItemsBLList(executionContext);
            List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>> searchParameter = new List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>>();
            searchParameter.Add(new KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>(CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID_LIST, cmsBannerIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameter.Add(new KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>(CMSBannerItemsDTO.SearchByRequestParameters.ACTIVE, "1"));
            }
            cMSBannerItemsDTOList = cMSBannerItemsBLList.GetAllCmsBannerItems(searchParameter);
            if (cMSBannerItemsDTOList != null && cMSBannerItemsDTOList.Any())
            {
                log.LogVariableState("cMSBannerItemsDTOList", cMSBannerItemsDTOList);
                foreach (var cmsBannerItemsDTO in cMSBannerItemsDTOList)
                {
                    if (cmsBannersIdDictionary.ContainsKey(cmsBannerItemsDTO.BannerId))
                    {
                        if (cmsBannersIdDictionary[cmsBannerItemsDTO.BannerId].CMSBannerItemsDTOList == null)
                        {
                            cmsBannersIdDictionary[cmsBannerItemsDTO.BannerId].CMSBannerItemsDTOList = new List<CMSBannerItemsDTO>();
                        }
                        cmsBannersIdDictionary[cmsBannerItemsDTO.BannerId].CMSBannerItemsDTOList.Add(cmsBannerItemsDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSBannersDTOList == null ||
                cMSBannersDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSBannersDTOList.Count; i++)
            {
                var cMSBannersDTO = cMSBannersDTOList[i];
                try
                {
                    CMSBannerBL cMSBannerBL = new CMSBannerBL(executionContext, cMSBannersDTO);
                    cMSBannerBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSBannersDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSBannersDTO", cMSBannersDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

