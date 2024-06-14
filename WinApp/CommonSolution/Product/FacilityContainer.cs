/********************************************************************************************
 * Project Name - Products
 * Description  - FacilityContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      16-Aug-2021      Prajwal S                 Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class FacilityContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<string, FacilityContainerDTO> productIdfacilityDTODictionary = new Dictionary<string, FacilityContainerDTO>();
        private readonly List<FacilityDTO> facilityDTOList;
        private readonly FacilityContainerDTOCollection facilityContainerDTOCollection;
        private readonly DateTime? facilityModuleLastUpdateTime;
        private readonly int siteId;
        /// <summary>
        /// Default Container Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public FacilityContainer(int siteId) : this(siteId, GetFacilityDTOList(siteId), GetFacilityModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters siteId, facilityDTOList, facilityModuleLastUpdateTime
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="facilityDTOList"></param>
        /// <param name="facilityModuleLastUpdateTime"></param>
        public FacilityContainer(int siteId, List<FacilityDTO> facilityDTOList, DateTime? facilityModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, facilityDTOList, facilityModuleLastUpdateTime);
            this.siteId = siteId;
            this.facilityDTOList = facilityDTOList;
            this.facilityModuleLastUpdateTime = facilityModuleLastUpdateTime;
            List<FacilityContainerDTO> facilityContainerDTOList = new List<FacilityContainerDTO>();
            foreach (FacilityDTO facilityDTO in facilityDTOList)
            {
                if (productIdfacilityDTODictionary.ContainsKey(facilityDTO.Guid))
                {
                    continue;
                }
                FacilityContainerDTO facilityContainerDTO = new FacilityContainerDTO(facilityDTO.FacilityId, facilityDTO.FacilityName, facilityDTO.Description, facilityDTO.ActiveFlag, facilityDTO.AllowMultipleBookings,
                                                                                     facilityDTO.Capacity, facilityDTO.InternetKey, facilityDTO.ScreenPosition, facilityDTO.InterfaceType, facilityDTO.InterfaceName,
                                                                                     facilityDTO.ExternalSystemReference, facilityDTO.Guid);
                if (facilityDTO.FacilitySeatLayoutDTOList != null && facilityDTO.FacilitySeatLayoutDTOList.Count > 0)
                {
                    foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilityDTO.FacilitySeatLayoutDTOList)
                    {
                        facilityContainerDTO.FacilitySeatLayoutContainerDTOList.Add(new FacilitySeatLayoutContainerDTO(facilitySeatLayoutDTO.LayoutId, facilitySeatLayoutDTO.FacilityId, facilitySeatLayoutDTO.RowColumnName, facilitySeatLayoutDTO.Type,
                                                                                     facilitySeatLayoutDTO.RowColumnIndex, facilitySeatLayoutDTO.HasSeats, facilitySeatLayoutDTO.Guid));
                    }
                }
                if (facilityDTO.FacilitySeatsDTOList != null && facilityDTO.FacilitySeatsDTOList.Count > 0)
                {
                    foreach (FacilitySeatsDTO facilitySeatsDTO in facilityDTO.FacilitySeatsDTOList)
                    {
                        facilityContainerDTO.FacilitySeatsContainerDTOList.Add(new FacilitySeatsContainerDTO(facilitySeatsDTO.SeatId, facilitySeatsDTO.SeatName, facilitySeatsDTO.RowIndex, facilitySeatsDTO.ColumnIndex,
                                                                                     facilitySeatsDTO.FacilityId, facilitySeatsDTO.IsAccessible, facilitySeatsDTO.BookedSeat, facilitySeatsDTO.Guid));
                    }
                }
                if (facilityDTO.FacilityWaiverDTOList != null && facilityDTO.FacilityWaiverDTOList.Count > 0)
                {
                    foreach (FacilityWaiverDTO facilityWaiverDTO in facilityDTO.FacilityWaiverDTOList)
                    {
                        facilityContainerDTO.FacilityWaiverContainerDTOList.Add(new FacilityWaiverContainerDTO(facilityWaiverDTO.FacilityWaiverId, facilityWaiverDTO.FacilityId, facilityWaiverDTO.WaiverSetId, facilityWaiverDTO.EffectiveFrom,
                                                                                     facilityWaiverDTO.EffectiveTo, facilityWaiverDTO.Guid));
                    }
                }
                if (facilityDTO.FacilityTableDTOList != null && facilityDTO.FacilityTableDTOList.Count > 0)
                {
                    foreach (FacilityTableDTO facilityTableDTO in facilityDTO.FacilityTableDTOList)
                    {
                        facilityContainerDTO.FacilityTableContainerDTOList.Add(new FacilityTableContainerDTO(facilityTableDTO.TableId, facilityTableDTO.TableName, facilityTableDTO.RowIndex, facilityTableDTO.ColumnIndex,
                                                                                     facilityTableDTO.FacilityId, facilityTableDTO.TableType, facilityTableDTO.InterfaceInfo1, facilityTableDTO.InterfaceInfo2, facilityTableDTO.InterfaceInfo3,
                                                                                     facilityTableDTO.Remarks, facilityTableDTO.Guid, facilityTableDTO.MaxCheckIns));
                    }
                }
                facilityContainerDTOList.Add(facilityContainerDTO);
                productIdfacilityDTODictionary.Add(facilityContainerDTO.Guid, facilityContainerDTO);
            }
            facilityContainerDTOCollection = new FacilityContainerDTOCollection(facilityContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the latest update time of Facility table from DB.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static DateTime? GetFacilityModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                Utilities utilities = new Utilities();
                FacilityList facilityListBL = new FacilityList(utilities.ExecutionContext);
                //result = facilityListBL.GetFacilityModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Facility max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get all the active Facility records for the given siteId.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static List<FacilityDTO> GetFacilityDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<FacilityDTO> facilityDTOList = null;
            try
            {
                Utilities utilities = new Utilities();
                ExecutionContext executionContext = utilities.ExecutionContext;
                executionContext.SetSiteId(siteId);
                FacilityList facilityListBL = new FacilityList(executionContext);
                List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                facilityDTOList = facilityListBL.GetFacilityDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Facility.", ex);
            }

            if (facilityDTOList == null)
            {
                facilityDTOList = new List<FacilityDTO>();
            }
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }

        /// <summary>
        /// Returns facilityContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public FacilityContainerDTOCollection GetFacilityContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(facilityContainerDTOCollection);
            return facilityContainerDTOCollection;
        }


        /// <summary>
        /// Refresh the container if there is any update in Db.
        /// </summary>
        /// <returns></returns>
        public FacilityContainer Refresh()
        {
            log.LogMethodEntry();

            Utilities utilities = new Utilities();
            FacilityList facilityListBL = new FacilityList(utilities.ExecutionContext);
            DateTime? updateTime = null; //facilityListBL.GetFacilityModuleLastUpdateTime(siteId);
            if (facilityModuleLastUpdateTime.HasValue
                && facilityModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Facility since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            FacilityContainer result = new FacilityContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
