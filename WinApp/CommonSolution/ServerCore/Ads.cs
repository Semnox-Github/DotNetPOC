/********************************************************************************************
 * Project Name - Ads
 * Description  - Business logic of Ads
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Jagan Mohana Rao    Created
 *2.70.2      26-Jan-2020   Girish Kundar       Modified : Changed to Standard format 
 *2.90        20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 *2.90        24-Aug-2020   Girish Kundar       Modified : Issue Fix ad image save for WMS 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ServerCore
{
    public class Ads
    {
        private AdsDTO adsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private Ads(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="adsDTO"></param>
        public Ads(ExecutionContext executionContext, AdsDTO adsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, adsDTO);
            this.adsDTO = adsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the adsBL id as the parameter
        /// Would fetch the adsDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public Ads(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
            adsDTO = adsDataHandler.GetAds(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate adBroadcast list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AdBroadcastList adBroadcastList = new AdBroadcastList(executionContext);
            List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.AD_ID, adsDTO.AdId.ToString()));
            adsDTO.AdBroadcastDTOList = adBroadcastList.GetAllAdBroadcast(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ads
        /// ads will be inserted if ads is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
            if (adsDTO.AdId > -1 && 
                 adsDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (adsDTO.AdId <= 0)
            {
                if (adsDTO.Image != null)
                {
                    SaveAdsImage();
                }
                adsDTO = adsDataHandler.InsertAds(adsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                adsDTO.AcceptChanges();
            }
            else if (adsDTO.IsChanged)
            {
                if (adsDTO.Image != null)
                {
                    SaveAdsImage();
                }
                adsDTO = adsDataHandler.UpdateAds(adsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                adsDTO.AcceptChanges();
            }
            SaveAdsChild(sqlTransaction);

            log.LogMethodExit();
        }

        private void SaveAdsImage()
        {
            log.LogMethodEntry();
            if (adsDTO.Image != null)
            {
                string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AD_IMAGE_DIRECTORY");
                string fileName = adsDTO.AdImageFileUser;
                string imagePath = imageFolder + fileName; // + (new System.IO.FileInfo(adsDTO.AdImageFileUser)).Extension; ;
                MemoryStream ms = new MemoryStream(adsDTO.Image);
                Image img = Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                adsDTO.AdImageFileSystem = DateTime.Now.Ticks.ToString();
                adsDTO.AdImageFileSystem = adsDTO.AdImageFileSystem.Substring(4, 8) + (new System.IO.FileInfo(adsDTO.AdImageFileUser)).Extension;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveAdsChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : :AdBroadcastDTO
            if (adsDTO.AdBroadcastDTOList != null &&
                adsDTO.AdBroadcastDTOList.Any())
            {
                List<AdBroadcastDTO> updatedAdBroadcastDTOList = new List<AdBroadcastDTO>();
                foreach (AdBroadcastDTO adBroadcastDTO in adsDTO.AdBroadcastDTOList)
                {
                    if (adBroadcastDTO.AdId != adsDTO.AdId)
                    {
                        adBroadcastDTO.AdId = adsDTO.AdId;
                    }
                    if (adBroadcastDTO.IsChanged)
                    {
                        updatedAdBroadcastDTOList.Add(adBroadcastDTO);
                    }
                }
                if (updatedAdBroadcastDTOList.Any())
                {
                    AdBroadcastList adBroadcastList = new AdBroadcastList(executionContext, updatedAdBroadcastDTOList);
                    adBroadcastList.Save();
                }
            }
        }

        /// <summary>
        /// Delete the Ads records from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(adsDTO, sqlTransaction);
            try
            {
                AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
                if ((adsDTO.AdBroadcastDTOList != null && adsDTO.AdBroadcastDTOList.Any((x => x.IsActive == true))))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("adsDTO", adsDTO);
                SaveAdsChild(sqlTransaction);
                if (adsDTO.AdId >= 0)
                {
                    adsDataHandler.DeleteAds(adsDTO.AdId);
                }
                adsDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (adsDTO == null)
            {
                //Validation to be implemented.
            }

            if (adsDTO.AdBroadcastDTOList != null && adsDTO.AdBroadcastDTOList.Any())
            {
                foreach (var adBroadcastDTO in adsDTO.AdBroadcastDTOList)
                {
                    if (adBroadcastDTO.IsChanged)
                    {
                        AdBroadcast adBroadcast = new AdBroadcast(executionContext, adBroadcastDTO);
                        validationErrorList.AddRange(adBroadcast.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// get adsDTO Object
        /// </summary>
        public AdsDTO GetAdsDTO
        {
            get { return adsDTO; }
        }

    }
    /// <summary>
    /// Manages the list of Ads
    /// </summary>
    public class AdsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AdsDTO> adsDTOList = new List<AdsDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AdsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="adsDTOList"></param>
        public AdsList(ExecutionContext executionContext, List<AdsDTO> adsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, adsDTOList);
            this.adsDTOList = adsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Machine Groups list
        /// </summary>
        public List<AdsDTO> GetAllAds(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters,
                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
            List<AdsDTO> adsDTOList = adsDataHandler.GetAdsDTOList(searchParameters);
            log.LogMethodExit(adsDTOList);
            return adsDTOList;
        }

        /// <summary>
        /// Returns the Ads list
        /// </summary>
        /// <summary>
        /// Returns the Ads list
        /// </summary>
        public List<AdsDTO> GetAllAdsDTOList(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                    bool buildImage = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
            List<AdsDTO> adsDTOList = adsDataHandler.GetAdsDTOList(searchParameters, sqlTransaction);
            if (adsDTOList != null && adsDTOList.Any() && loadChildRecords)
            {
                Build(adsDTOList, loadActiveRecords, sqlTransaction);
            }
            if (adsDTOList != null && adsDTOList.Any() && buildImage)
            {
                foreach (AdsDTO adsDTO in adsDTOList)
                {
                   adsDTO.Image  = ReadAdImage(adsDTO.AdImageFileUser);
                   adsDTO.IsChanged = false;
                }
            }
            log.LogMethodExit(adsDTOList);
            return adsDTOList;
        }
        internal byte[] ReadAdImage(string adImageFileSystem)
        {
            log.LogMethodEntry();
            try
            {
                string imageFolder = ParafaitDefaultContainerList.GetParafaitDefault( executionContext ,"AD_IMAGE_DIRECTORY");
                if (string.IsNullOrEmpty(adImageFileSystem))
                {
                    return null;
                }
                byte[] imageBytes = System.IO.File.ReadAllBytes(imageFolder + "\\" + adImageFileSystem.ToString());
                return imageBytes;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        private void Build(List<AdsDTO> adsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(adsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AdsDTO> adIdDictionary = new Dictionary<int, AdsDTO>();
            StringBuilder sb = new StringBuilder("");
            string adIdList;
            for (int i = 0; i < adsDTOList.Count; i++)
            {
                if (adsDTOList[i].AdId == -1 ||
                    adIdDictionary.ContainsKey(adsDTOList[i].AdId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(adsDTOList[i].AdId.ToString());
                adIdDictionary.Add(adsDTOList[i].AdId, adsDTOList[i]);
            }
            adIdList = sb.ToString();
            AdBroadcastList adBroadcastList = new AdBroadcastList(executionContext);
            List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.AD_ID_LIST, adIdList.ToString()));
            searchByParams.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));

            if (activeChildRecords)
            {
                searchByParams.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.ISACTIVE, "1"));
            }
            List<AdBroadcastDTO> adBroadcastDTOList = adBroadcastList.GetAllAdBroadcast(searchByParams, sqlTransaction);
            if (adBroadcastDTOList != null && adBroadcastDTOList.Any())
            {
                log.LogVariableState("adBroadcastDTOList", adBroadcastDTOList);
                foreach (var adBroadcastDTO in adBroadcastDTOList)
                {
                    if (adIdDictionary.ContainsKey(adBroadcastDTO.AdId))
                    {
                        if (adIdDictionary[adBroadcastDTO.AdId].AdBroadcastDTOList == null)
                        {
                            adIdDictionary[adBroadcastDTO.AdId].AdBroadcastDTOList = new List<AdBroadcastDTO>();
                        }
                        adIdDictionary[adBroadcastDTO.AdId].AdBroadcastDTOList.Add(adBroadcastDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        // Faizan : this method need to be check
        /// <summary>
        /// Returns the Ads list
        /// </summary>
        public List<AdsDTO> GetAllAddsPopulateMachineDTOList(List<KeyValuePair<AdsDTO.SearchByParameters, string>>
                                                            searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AdsDataHandler adsDataHandler = new AdsDataHandler(sqlTransaction);
            log.LogMethodExit();
            List<AdsDTO> adsDTOList = adsDataHandler.GetAdsDTOList(searchParameters, sqlTransaction);
            if (adsDTOList != null && adsDTOList.Count != 0)
            {
                foreach (AdsDTO adsDTO in adsDTOList)
                {
                    List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>>();

                    searchByParams.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.SITE_ID, Convert.ToString(adsDTO.Siteid)));
                    searchByParams.Add(new KeyValuePair<AdBroadcastDTO.SearchByParameters, string>(AdBroadcastDTO.SearchByParameters.AD_ID, adsDTO.AdId.ToString()));
                    AdBroadcastList adBroadcastList = new AdBroadcastList(executionContext);
                    List<AdBroadcastDTO> existingAdBroadcastDTOList = adBroadcastList.GetAllAdBroadcast(searchByParams, sqlTransaction);

                    if (existingAdBroadcastDTOList == null && existingAdBroadcastDTOList.Count == 0)
                    {
                        existingAdBroadcastDTOList = new List<AdBroadcastDTO>();
                    }
                    else
                    {
                        adsDTO.AdBroadcastDTOList = (existingAdBroadcastDTOList);
                    }
                    List<AdBroadcastDTO> populateMachineList = adBroadcastList.GetAllPopulateAdBroadcast(searchByParams, sqlTransaction);
                    if (populateMachineList != null && populateMachineList.Count != 0)
                    {
                        adsDTO.AdBroadcastDTOList.AddRange(populateMachineList);
                    }
                }
            }
            log.LogMethodExit(adsDTOList);
            return adsDTOList;
        }


        /// <summary>
        /// Saves the  list of adsDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (adsDTOList == null ||
                adsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < adsDTOList.Count; i++)
            {
                var adsDTO = adsDTOList[i];
                if (adsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    Ads ads = new Ads(executionContext, adsDTO);
                    ads.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving adsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("adsDTO", adsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AdsDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (adsDTOList != null && adsDTOList.Any())
            {
                foreach (AdsDTO adsDTO in adsDTOList)
                {
                    if (adsDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                Ads ads = new Ads(executionContext, adsDTO);
                                ads.Delete(parafaitDBTrx.SQLTrx);
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
    }
}