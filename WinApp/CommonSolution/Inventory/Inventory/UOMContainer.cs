/********************************************************************************************
* Project Name - Inventory 
* Description  - Container Class for UOM to get all the UOM DTO list & the related UOM DTO list
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.100.0    18-Aug-20      Deeksha              Created 
********************************************************************************************/
using System;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory
{
    public class UOMContainer
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<UOMDTO> uomDTOList;
        private DateTime? uomModuleLastUpdateTime;
        private DateTime? refreshTime;
        private readonly ExecutionContext executionContext;
        private readonly Timer refreshTimer;
        public readonly ConcurrentDictionary<int, List<UOMDTO>> relatedUOMDTOList = new ConcurrentDictionary<int, List<UOMDTO>>();
        private static List<UOMConversionFactorDTO> uOMConversionFactorDTOList;


        public UOMContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            uomDTOList = GetAllUOMDTO();
            refreshTimer = new Timer(5 * 60 * 1000);
            refreshTimer.Elapsed += OnRefreshTimer;
            RefreshUOMList();
            refreshTimer.Start();
            uomModuleLastUpdateTime = GetLastUpdatedDate();
            log.LogMethodExit();
        }


        private DateTime? GetLastUpdatedDate()
        {
            log.LogMethodEntry();
            UOMList uomListBL = new UOMList(executionContext);
            DateTime? updateTime = uomListBL.GetUOMModuleLastUpdateTime(executionContext.GetSiteId());
            log.LogMethodExit(updateTime);
            return updateTime;
        }

        private void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            RefreshUOMList();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Get All UOM's List
        /// </summary>
        /// <returns></returns>
        private List<UOMDTO> GetAllUOMDTO()
        {
            log.LogMethodEntry();
            UOMList uomListBL = new UOMList(executionContext);
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<UOMDTO> uomDTOLists = uomListBL.GetAllUOMDTOList(searchParameters, true, true);
            log.LogMethodExit(uomDTOLists);
            return uomDTOLists;
        }

        /// <summary>
        /// Method to refresh the UOM List
        /// </summary>
        private void RefreshUOMList()
        {
            log.LogMethodEntry();
            if (refreshTime.HasValue && refreshTime > DateTime.UtcNow)
            {
                log.LogMethodExit(null, "Refreshed the list in last 5 minutes.");
                return;
            }

            refreshTime = DateTime.UtcNow.AddMinutes(5);
            UOMList uomListBL = new UOMList(executionContext);
            DateTime? updateTime = uomListBL.GetUOMModuleLastUpdateTime(executionContext.GetSiteId());
            if (updateTime.HasValue && uomModuleLastUpdateTime.HasValue
                && uomModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(null, "No changes in UOM module since " + uomModuleLastUpdateTime);
                return;
            }

            uomModuleLastUpdateTime = updateTime;
            GetUOMCOnversionFactorList();
            uomDTOList = GetAllUOMDTO();
            RefreshRelatedUOMList();
            log.LogMethodExit();
        }


        /// <summary>
        /// Method which refreshes related UOM DTO list and uomId to the dictionary
        /// </summary>
        private void RefreshRelatedUOMList()
        {
            log.LogMethodEntry();
            relatedUOMDTOList.Clear();
            if (uomDTOList != null && uomDTOList.Any())
            {
                foreach (UOMDTO uomDTO in uomDTOList)
                {
                    List<UOMDTO> relatedUOMList = GetRelatedUOMList(uomDTO.UOMId);
                    relatedUOMDTOList.TryAdd(uomDTO.UOMId, relatedUOMList);
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Method to get the related UOM's List
        /// </summary>
        /// <param name="uomId"></param>
        /// <returns></returns>
        private List<UOMDTO> GetRelatedUOMList(int uomId)
        {
            log.LogMethodEntry(uomId);
            List<UOMDTO> relatedList = new List<UOMDTO>();
            try
            {

                List<UOMConversionFactorDTO> childDTOList = uOMConversionFactorDTOList.FindAll(x => x.BaseUOMId == uomId | x.UOMId == uomId).ToList();
                List<int> childUOMIdList = childDTOList.Select(id => id.UOMId).ToList();
                childUOMIdList.Add(uomId);
                childUOMIdList.AddRange(childDTOList.Select(id => id.BaseUOMId).ToList());
                childUOMIdList = childUOMIdList.Distinct().ToList();

                foreach (int id in childUOMIdList)
                {
                    UOMDTO uomDTO = uomDTOList.Find(x => x.UOMId == id);
                    relatedList.Add(uomDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(relatedList);
            return relatedList;
        }

        private  void  GetUOMCOnversionFactorList()
        {
            log.LogMethodEntry();
            UOMConversionFactorListBL uOMConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
            List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            uOMConversionFactorDTOList = uOMConversionFactorListBL.GetUOMConversionFactorDTOList(searchParameters);
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to get the Conversion Factor
        /// </summary>
        /// <param name="userEnterdUOM">From UOM Id</param>
        /// <param name="baseInventoryUOM">To UOM Id</param>
        /// <returns>conversionFactor</returns>
        public static double GetConversionFactor(int userEnterdUOM, int baseInventoryUOM)
        {
            log.LogMethodEntry(userEnterdUOM, baseInventoryUOM);
            double conversionFactor = 1;
            try
            {
                if (uOMConversionFactorDTOList != null && uOMConversionFactorDTOList.Any())
                {
                    List<UOMConversionFactorDTO> conversionDTO = uOMConversionFactorDTOList.FindAll(x => x.BaseUOMId == baseInventoryUOM & x.UOMId == userEnterdUOM).ToList();
                    if (conversionDTO != null && conversionDTO.Any())
                    {
                        conversionFactor = 1 / conversionDTO[0].ConversionFactor;
                    }
                    else
                    {
                        conversionDTO = uOMConversionFactorDTOList.FindAll(x => x.BaseUOMId == userEnterdUOM & x.UOMId == baseInventoryUOM).ToList();
                        if (conversionDTO != null && conversionDTO.Any())
                        {
                            conversionFactor = 1 * conversionDTO[0].ConversionFactor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(conversionFactor);
            return conversionFactor;
        }
    }
}
