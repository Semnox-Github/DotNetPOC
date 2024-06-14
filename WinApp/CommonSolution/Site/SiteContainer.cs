/********************************************************************************************
 * Project Name - Site
 * Description  - SiteContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
*2.140.0     21-Jun-2021       Fiona Lishal            Modified for Delivery Order enhancements for F&B and Urban Piper
*2.150.0     09-Mar-2022       Lakshminarayana         Modified : SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    public class SiteContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<SiteDTO> siteDTOList;
        private readonly SiteContainerDTOCollection siteContainerDTOCollection;
        private readonly DateTime? siteLastUpdateTime;
        private readonly ConcurrentDictionary<int, SiteDTO> siteDTODictionary;
        private readonly ConcurrentDictionary<int, SiteContainerDTO> siteContainerDTODictionary;
        private readonly bool isCorporate;
        private readonly int masterSiteId = -1;
        TimeZoneInfo hostTimeZone;
        internal SiteContainer()
        {
            log.LogMethodEntry();
            List<SiteContainerDTO> siteContainerDTOList = new List<SiteContainerDTO>();
            siteDTODictionary = new ConcurrentDictionary<int, SiteDTO>();
            siteContainerDTODictionary = new ConcurrentDictionary<int, SiteContainerDTO>();
            try
            {
                SiteList siteListBL = new SiteList();
                siteLastUpdateTime = siteListBL.GetSiteLastUpdateTime();

                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "1"));
                siteDTOList = siteListBL.GetAllSites(searchParameters);
                isCorporate = siteDTOList.Count > 1;
                foreach (SiteDTO siteDTO in siteDTOList)
                {
                    List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>> searchDetailParameters = new List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>>();
                    searchDetailParameters.Add(new KeyValuePair<SiteDetailDTO.SearchByParameters, string>(SiteDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
                    searchDetailParameters.Add(new KeyValuePair<SiteDetailDTO.SearchByParameters, string>(SiteDetailDTO.SearchByParameters.PARENT_SITE_ID, siteDTO.SiteId.ToString()));
                    SiteDetailListBL siteDetailListBL = new SiteDetailListBL();
                    siteDTO.SiteDetailDTOList = siteDetailListBL.GetSiteDetails(searchDetailParameters);

                }

                if (siteDTOList == null)
                {
                    siteDTOList = new List<SiteDTO>();
                }
                if (siteDTOList.Any())
                {
                    int siteId = -1;
                    foreach (SiteDTO siteDTOs in siteDTOList)
                    {
                        if (siteDTOs.IsMasterSite == true)
                        {
                            if (isCorporate)
                            {
                                masterSiteId = siteDTOs.SiteId;
                            }

                            siteId = siteDTOs.SiteId;
                            break;
                        }
                    }
                    foreach (SiteDTO siteDTO in siteDTOList)
                    {
                        string siteTimeZoneName = ParafaitDefaultContainerList.GetParafaitDefault(isCorporate ? siteDTO.SiteId : -1, "WEBSITE_TIME_ZONE");
                        int businessDayStartTime = ParafaitDefaultContainerList.GetParafaitDefault(isCorporate ? siteDTO.SiteId : -1, "BUSINESS_DAY_START_TIME", 6);
                        if (isCorporate &&
                           siteDTO.SiteId == masterSiteId)
                        {
                            if (string.IsNullOrWhiteSpace(ServerDateTime.TimeZone) == false)
                            {
                                siteTimeZoneName = ServerDateTime.TimeZone;
                            }
                            else
                            {
                                siteTimeZoneName = TimeZoneInfo.Local.Id;
                            }
                        }

                        siteDTODictionary[siteDTO.SiteId] = siteDTO;
                        SiteContainerDTO siteContainerDTO = new SiteContainerDTO(siteDTO.SiteId, siteDTO.SiteName, siteDTO.SiteAddress, siteDTO.IsMasterSite, siteDTO.OnlineEnabled == "Y" ? true : false, siteDTO.PinCode, siteDTO.SiteURL, siteDTO.SiteShortName, siteDTO.City,
                                                                                 siteDTO.State, siteDTO.Country, siteDTO.OpenTime, siteDTO.CloseTime, siteDTO.StoreRanking, siteDTO.StoreType, siteDTO.OpenDate, siteDTO.CloseDate, siteDTO.Logo, siteDTO.Email, siteDTO.PhoneNumber,
                                                                                 businessDayStartTime, siteTimeZoneName, siteDTO.Latitude, siteDTO.Longitude, siteDTO.CustomerKey);
                        if (siteDTO.SiteDetailDTOList != null && siteDTO.SiteDetailDTOList.Any())
                        {
                            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> lookupParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                            lookupParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "ONLINE_FOOD_DELIVERY_TYPE"));

                            lookupParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                            ExecutionContext executionContext = new ExecutionContext("semnox", siteId, -1, 3, false, -9);
                            LookupsList lookupsListBL = new LookupsList(executionContext);
                            List<LookupsDTO> lookupsDTOList = lookupsListBL.GetAllLookups(lookupParameters, true, true);
                            siteContainerDTO.SiteDeliveryDetailsDTOList = new List<SiteDetailContainerDTO>();
                            foreach (SiteDetailDTO siteDetailDTO in siteDTO.SiteDetailDTOList)
                            {
                                string[] zipcodes;
                                zipcodes = siteDetailDTO.ZipCodes.Split(',');
                                List<string> zipcodeList = new List<string>();
                                foreach (string zip in zipcodes)
                                {
                                    zipcodeList.Add(zip);
                                }
                                string orderDeliveryType = string.Empty;
                                if (lookupsDTOList.Any() && siteDetailDTO != null && siteDetailDTO.OrderDeliveryType > -1)
                                {
                                    foreach (LookupsDTO lookupsDTO in lookupsDTOList)
                                    {
                                        foreach (LookupValuesDTO lookupValuesDTO in lookupsDTO.LookupValuesDTOList)
                                        {
                                            if (lookupValuesDTO.LookupValueId == siteDetailDTO.OrderDeliveryType)
                                            {
                                                orderDeliveryType = lookupValuesDTO.LookupValue;
                                            }
                                        }
                                    }
                                }
                                SiteDetailContainerDTO siteDetailContainerDTO = new SiteDetailContainerDTO(siteDetailDTO.SiteDetailId, siteDetailDTO.ParentSiteId, siteDetailDTO.DeliveryChannelId, siteDetailDTO.OnlineChannelStartHour,
                                                                                                           siteDetailDTO.OnlineChannelEndHour, orderDeliveryType, zipcodeList, siteDetailDTO.SiteId);
                                siteContainerDTO.SiteDeliveryDetailsDTOList.Add(siteDetailContainerDTO);
                            }
                        }
                        siteContainerDTODictionary[siteDTO.SiteId] = siteContainerDTO;
                        if (siteDTO.IsMasterSite)
                        {
                            masterSiteId = siteDTO.SiteId;
                            siteContainerDTOList.Insert(0, siteContainerDTO);
                        }
                        else
                        {
                            siteContainerDTOList.Add(siteContainerDTO);
                        }
                    }
                }
                string timeZoneId = ServerDateTime.TimeZone;
                hostTimeZone = string.IsNullOrWhiteSpace(timeZoneId) ? TimeZoneInfo.Local : TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the site container.", ex);
                siteDTOList = new List<SiteDTO>();
                siteDTODictionary.Clear();
                siteContainerDTOList.Clear();
                siteContainerDTODictionary.Clear();
            }
            siteContainerDTOCollection = new SiteContainerDTOCollection(siteContainerDTOList);
            isCorporate = siteDTOList.Count > 1;
            if (isCorporate == false)
            {
                masterSiteId = -1;
            }
            log.Info("Number of items loaded by SiteContainer:" + siteDTOList.Count);
            log.LogMethodExit();
        }

        internal List<SiteContainerDTO> GetSiteContainerDTOList()
        {
            log.LogMethodEntry();
            var result = siteContainerDTOCollection.SiteContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }

        internal int GetMasterSiteId()
        {
            log.LogMethodEntry();
            log.LogMethodExit(masterSiteId);
            return masterSiteId;
        }

        internal bool IsCorporate()
        {
            log.LogMethodEntry();
            log.LogMethodExit(isCorporate);
            return isCorporate;
        }

        public SiteContainerDTOCollection GetSiteContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(siteContainerDTOCollection);
            return siteContainerDTOCollection;
        }

        public string GetCustomerKey(int siteId)
        {
            log.LogMethodEntry(siteId);
            SiteDTO siteDTO = GetSiteDTO(siteId);
            string result = siteDTO.CustomerKey;
            log.LogMethodExit(result);
            return result;
        }

        public DateTime ToSiteDateTime(int siteId, DateTime hqDateTime)
        {
            log.LogMethodEntry(siteId, hqDateTime);
            if (hqDateTime == DateTime.MinValue)
            {
                return hqDateTime;
            }
            int offset = GetOffset(siteId, hqDateTime);
            DateTime result = hqDateTime.AddSeconds(offset);
            log.LogMethodExit(result);
            return result;
        }

        private int GetOffset(int siteId, DateTime dateTime)
        {
            log.LogMethodEntry(siteId);
            int offset = 0;
            string siteTimeZoneName = ParafaitDefaultContainerList.GetParafaitDefault(isCorporate ? siteId : -1, "WEBSITE_TIME_ZONE");
            int businessDayStartTime = ParafaitDefaultContainerList.GetParafaitDefault(isCorporate ? siteId : -1, "BUSINESS_DAY_START_TIME", 6);
            if (string.IsNullOrWhiteSpace(siteTimeZoneName))
            {
                log.LogMethodExit(offset, "site time zone is empty");
                return offset;
            }
            if (siteTimeZoneName == TimeZone.CurrentTimeZone.StandardName)
            {
                log.LogMethodExit(offset, "site time zone is same as current time zone");
                return offset;
            }
            DateTime locUtcTime = dateTime.Date.AddHours(businessDayStartTime).ToUniversalTime();
            TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
            DateTime HostServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, hostTimeZone);
            DateTime SiteServerTime = TimeZoneInfo.ConvertTimeFromUtc(locUtcTime, siteTimeZone);
            TimeSpan datediff = HostServerTime - SiteServerTime;
            offset = (int)datediff.TotalSeconds;
            log.LogMethodExit(offset);
            return offset;
        }

        public DateTime FromSiteDateTime(int siteId, DateTime siteDateTime)
        {
            log.LogMethodEntry(siteId, siteDateTime);
            if (siteDateTime == DateTime.MinValue)
            {
                return siteDateTime;
            }
            int offset = GetOffset(siteId, siteDateTime);
            DateTime result = siteDateTime.AddSeconds(-offset);
            log.LogMethodExit(result);
            return result;
        }

        public DateTime CurrentDateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string siteTimeZoneName = ParafaitDefaultContainerList.GetParafaitDefault(isCorporate ? siteId : -1, "WEBSITE_TIME_ZONE");
            DateTime result = ServerDateTime.Now;
            if (string.IsNullOrWhiteSpace(siteTimeZoneName))
            {
                log.LogMethodExit(result, "site time zone is empty");
                return result;
            }
            if (siteTimeZoneName == TimeZone.CurrentTimeZone.StandardName)
            {
                log.LogMethodExit(result, "site time zone is same as current time zone");
                return result;
            }
            TimeZoneInfo siteTimeZone = TimeZoneInfo.FindSystemTimeZoneById(siteTimeZoneName);
            result = TimeZoneInfo.ConvertTimeFromUtc(ServerDateTime.Now.ToUniversalTime(), siteTimeZone);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the site container DTO for a given siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public SiteContainerDTO GetSiteContainerDTO(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (isCorporate == false && siteContainerDTOCollection.SiteContainerDTOList.Count == 1)
            {
                log.LogMethodExit(siteContainerDTOCollection.SiteContainerDTOList[0], "isCorporate == false && SiteContainerDTOList.Count == 1");
                return siteContainerDTOCollection.SiteContainerDTOList[0];
            }
            if (siteContainerDTODictionary.ContainsKey(siteId) == false)
            {
                string errorMessage = "Site with siteId :" + siteId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            SiteContainerDTO result = siteContainerDTODictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        private SiteDTO GetSiteDTO(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (siteId == -1 && siteDTOList.Count == 1)
            {
                log.LogMethodExit(siteDTOList[0], "siteId == -1 && siteDTOList.Count == 1");
                return siteDTOList[0];
            }
            if (siteDTODictionary.ContainsKey(siteId) == false)
            {
                string errorMessage = "Site with siteId :" + siteId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            SiteDTO result = siteDTODictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        public SiteContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetSiteLastUpdateTime();
            if (siteLastUpdateTime.HasValue
                && siteLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Site since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            SiteContainer result = new SiteContainer();
            log.LogMethodExit(result);
            return result;
        }
        private static DateTime? GetSiteLastUpdateTime()
        {
            log.LogMethodEntry();
            DateTime? result = null;
            try
            {
                SiteList siteList = new SiteList();
                result = siteList.GetSiteLastUpdateTime();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the site module last update time.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
