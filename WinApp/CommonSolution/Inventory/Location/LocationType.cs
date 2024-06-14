/********************************************************************************************
 * Project Name - Location Type
 * Description  - Bussiness logic of Location Type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       18-Aug-2016    Amaresh          Created
 *2.60.3     17-Jun-2019    Nagesh Badiger   Added parameterized constructor with executionContext and log method entry and exit
 *2.70.0     02-Aug-2019    Jagan Mohana     Removed the GetLocationTypeListOnType() method and used GetAllLocationType() by passing searchParameters
 *2.70       15-Jul-2019    Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Location type will creates and modifies the Location Type
    /// </summary>
    public class LocationType
    {
        private LocationTypeDTO locationTypeDTO;
        private readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LocationType(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            locationTypeDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="locationTypeId">locationTypeId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LocationType(ExecutionContext executionContext, int locationTypeId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, locationTypeId, sqlTransaction);
            this.executionContext = executionContext;
            LocationTypeDataHandler locationTypeDataHandler = new LocationTypeDataHandler(sqlTransaction);
            this.locationTypeDTO = locationTypeDataHandler.GetLocationType(locationTypeId);
            log.LogMethodExit(locationTypeDTO);
        }

        /// <summary>
        /// Constructor with the LocationType DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="locationTypeDTO">Parameter of the type LocationTypeDTO</param>
        public LocationType(ExecutionContext executionContext, LocationTypeDTO locationTypeDTO)
        {
            log.LogMethodEntry(executionContext, locationTypeDTO);
            this.locationTypeDTO = locationTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Saves the Location Type
        /// LocationType will be inserted if LocationTypeId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LocationTypeDataHandler locationTypeDataHandler = new LocationTypeDataHandler(sqlTransaction);

            if (locationTypeDTO.LocationTypeId <= 0)
            {
                locationTypeDTO = locationTypeDataHandler.InsertLocationType(locationTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                locationTypeDTO.AcceptChanges();
            }
            else
            {
                if (locationTypeDTO.IsChanged)
                {
                    locationTypeDTO = locationTypeDataHandler.UpdateLocationType(locationTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    locationTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get LocationTypeDTO
        /// </summary>
        public LocationTypeDTO LocationTypeDTO { get { return this.locationTypeDTO; } }
    }

    /// <summary>
    /// Manages the list of LocationType List
    /// </summary>
    public class LocationTypeList
    {
        private readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        List<LocationTypeDTO> locationTypeDTOList;

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LocationTypeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.locationTypeDTOList = new List<LocationTypeDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LocationType List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>locationType DTO List</returns>
        public List<LocationTypeDTO> GetAllLocationType(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LocationTypeDataHandler locationTypeDataHandler = new LocationTypeDataHandler(sqlTransaction);
            locationTypeDTOList = locationTypeDataHandler.GetLocationTypeList(searchParameters);
            log.LogMethodExit(locationTypeDTOList);
            return locationTypeDTOList;
        }

        public DateTime? GetLocationTypeLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            LocationTypeDataHandler locationTypeDataHandler = new LocationTypeDataHandler(sqlTransaction);
            DateTime? result = locationTypeDataHandler.GetLocationTypeLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
