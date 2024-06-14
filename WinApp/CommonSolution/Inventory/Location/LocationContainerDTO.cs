/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocationContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.150.0         13-Dec-2020         Abhishek               Modified : Added new field locationTypeName as part of Web Inventory Redesign
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Inventory.Location
{
    public class LocationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int locationId;
        private string name;
        private string isAvailableToSell;
        private string barcode;
        private string isTurnInLocation;
        private string isStore;
        private string massUpdateAllowed;
        private string remarksMandatory;
        private int locationTypeId;
        private int customDataSetId;
        private string externalSystemReference;
        private string locationTypeName;

        public LocationContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public LocationContainerDTO(int locationIdPassed, string namePassed, string isSellable, string barcode, string isTurnInLocationPassed, string isStorePassed, string massUpdatePassed,
                             string remarksMandatoryPassed, int locationTypeIdPassed, int customDataSetId, string externalSystemReference, string locationTypeName)
              : this()
        {
            log.LogMethodEntry(locationIdPassed, namePassed, isSellable, barcode, isTurnInLocationPassed, isStorePassed, massUpdatePassed, remarksMandatoryPassed,
                locationTypeIdPassed, customDataSetId, externalSystemReference, locationTypeName);
            this.locationId = locationIdPassed;
            this.name = namePassed;
            this.isAvailableToSell = isSellable;
            this.barcode = barcode;
            this.isTurnInLocation = isTurnInLocationPassed;
            this.isStore = isStorePassed;
            this.massUpdateAllowed = massUpdatePassed;
            this.remarksMandatory = remarksMandatoryPassed;
            this.locationTypeId = locationTypeIdPassed;
            this.customDataSetId = customDataSetId;
            this.externalSystemReference = externalSystemReference;
            this.locationTypeName = locationTypeName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("Location Id")]
        [ReadOnly(true)]
        public int LocationId { get { return locationId; } set { locationId = value;  } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value;  } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks Mandatory")]
        public string RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value;  } }
        /// <summary>
        /// Get/Set method of the isAvailableToSell field
        /// </summary>
        [DisplayName("Available To Sell")]
        public string IsAvailableToSell { get { return isAvailableToSell; } set { isAvailableToSell = value;  } }
        /// <summary>
        /// Get/Set method of the IsStore field
        /// </summary>
        [DisplayName("Is Store")]
        public string IsStore { get { return isStore; } set { isStore = value;  } }
        /// <summary>
        /// Get/Set method of the IsTurnInLocation field
        /// </summary>
        [DisplayName("Turn In Location")]
        public string IsTurnInLocation { get { return isTurnInLocation; } set { isTurnInLocation = value;  } }

        /// <summary>
        /// Get/Set method of the MassUpdatedAllowed field
        /// </summary>
        [DisplayName("Allow Mass Update")]
        public string MassUpdatedAllowed { get { return massUpdateAllowed; } set { massUpdateAllowed = value;  } }
        /// <summary>
        /// Get/Set method of the LocationTypeId field
        /// </summary>
        [DisplayName("Location Type Id")]
        public int LocationTypeId { get { return locationTypeId; } set { locationTypeId = value;  } }
        /// <summary>
        /// Get/Set method of the barcode field
        /// </summary>
        [DisplayName("Barcode")]
        public string Barcode { get { return barcode; } set { barcode = value;  } }

        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        [DisplayName("CustomDataSetId")]
        [ReadOnly(true)]
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value;  } }
        /// <summary>
        /// Get/Set method of the ExternalSystemRefernce field
        /// </summary>
        [DisplayName("External System Refernce")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value;  } }

        /// <summary>
        /// Get/Set method of the LocationTypeName field
        /// </summary>
        public string LocationTypeName { get { return locationTypeName; } set { locationTypeName = value; } }

    }
}
