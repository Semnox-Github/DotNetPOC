/********************************************************************************************
 * Project Name - Products
 * Description  - Data object of FacilityContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00   16-Aug-2021    Prajwal S          Created                                                       
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// FacilityContainerDTO Class
    /// </summary>
    public class FacilityContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        private int facilityId;
        private string facilityName;
        private string description;
        private bool activeFlag;
        private bool allowMultipleBookings;
        private int? capacity;
        private int? internetKey;
        private string screenPosition;
        private string guid;
        private int interfaceType;
        private int interfaceName;
        private string externalSystemReference;
        private List<FacilitySeatLayoutContainerDTO> facilitySeatLayoutContainerDTOList;
        private List<FacilitySeatsContainerDTO> facilitySeatsContainerDTOList;
        private List<FacilityWaiverContainerDTO> facilityWaiverContainerDTOList;
        private List<FacilityTableContainerDTO> facilityTableContainerDTOList; 

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilityContainerDTO()
        {
            log.LogMethodEntry();
            this.facilitySeatLayoutContainerDTOList = new List<FacilitySeatLayoutContainerDTO>();
            this.facilitySeatsContainerDTOList = new List<FacilitySeatsContainerDTO>();
            this.facilityWaiverContainerDTOList = new List<FacilityWaiverContainerDTO>();
            this.facilityTableContainerDTOList = new List<FacilityTableContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all container parameters
        /// </summary>
        public FacilityContainerDTO(int facilityId, string facilityName, string description, bool activeFlag, bool allowMultipleBooking, int? capacity, int? internetKey, string screenPosition,
                          int interfaceType, int interfaceName, string externalSystemReference, string guid)
            : this()
        {
            log.LogMethodEntry(facilityId, facilityName, description, activeFlag, allowMultipleBooking, capacity, internetKey, screenPosition,
                                interfaceType, interfaceName, externalSystemReference);
            this.facilityId = facilityId;
            this.facilityName = facilityName;
            this.description = description;
            this.activeFlag = activeFlag;
            this.allowMultipleBookings = allowMultipleBooking;
            this.capacity = capacity;
            this.internetKey = internetKey;
            this.screenPosition = screenPosition;
            this.interfaceType = interfaceType;
            this.interfaceName = interfaceName;
            this.externalSystemReference = externalSystemReference;
            this.guid = guid;
        }

       
        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set method of the FacilityName field
        /// </summary>
        public string FacilityName { get { return facilityName; } set { facilityName = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; } }

        /// <summary>
        /// Get/Set method of the allowMultipleBooking field
        /// </summary>
        public bool AllowMultipleBookings { get { return allowMultipleBookings; } set { allowMultipleBookings = value; } }
        /// <summary>
        /// Get/Set method of the Capacity field
        /// </summary>
        public int? Capacity { get { return capacity; } set { capacity = value; } }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        public int? InternetKey { get { return internetKey; } set { internetKey = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the ScreenPosition field
        /// </summary>
        public string ScreenPosition { get { return screenPosition; } set { screenPosition = value; } }

        /// <summary>
        /// Get/Set method of the MaxRowIndex field
        /// </summary>
        public int? MaxRowIndex { get; set; }

        /// <summary>
        /// Get/Set method of the MaxColIndex field
        /// </summary>
        public int? MaxColIndex { get; set; }

        /// <summary>
        /// Get/Set method of the InterfaceType field
        /// </summary>
        public int InterfaceType { get { return interfaceType; } set { interfaceType = value; } }

        /// <summary>
        /// Get/Set method of the InterfaceName field
        /// </summary>
        public int InterfaceName { get { return interfaceName; } set { interfaceName = value; } }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; } }

        /// <summary>
        /// Get/Set method of the FacilitySeatsContainerDTOList field
        /// </summary> 
        public List<FacilitySeatsContainerDTO> FacilitySeatsContainerDTOList
        {
            get
            {
                return facilitySeatsContainerDTOList;
            }
            set
            {
                facilitySeatsContainerDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the FacilitySeatLayoutContainerDTOList field
        /// </summary>
        public List<FacilitySeatLayoutContainerDTO> FacilitySeatLayoutContainerDTOList
        {
            get
            {
                return facilitySeatLayoutContainerDTOList;
            }
            set
            {
                facilitySeatLayoutContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FacilityWaiverContainerDTOList field
        /// </summary>
        public List<FacilityWaiverContainerDTO> FacilityWaiverContainerDTOList
        {
            get
            {
                return facilityWaiverContainerDTOList;
            }
            set
            {
                facilityWaiverContainerDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the FacilityTableContainerDTOList field
        /// </summary>
        public List<FacilityTableContainerDTO> FacilityTableContainerDTOList
        {
            get
            {
                return facilityTableContainerDTOList;
            }
            set
            {
                facilityTableContainerDTOList = value;
            }
        }
    }
}
