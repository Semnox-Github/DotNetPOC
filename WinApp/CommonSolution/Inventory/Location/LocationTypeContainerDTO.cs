/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationTypeContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Jan-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationTypeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int locationTypeId;
        private string locationType;
        private string description;

        public LocationTypeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public LocationTypeContainerDTO(int locationTypeId, string locationType, string description)
             : this()
        {
            log.LogMethodEntry(locationTypeId, locationType, description);
            this.locationTypeId = locationTypeId;
            this.locationType = locationType;
            this.description = description;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LocationTypeId field
        /// </summary>
        [DisplayName("Location Type Id")]
        [ReadOnly(true)]
        public int LocationTypeId { get { return locationTypeId; } set { locationTypeId = value; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Location Type")]
        public string LocationType { get { return locationType; } set { locationType = value; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; } }
    }
}