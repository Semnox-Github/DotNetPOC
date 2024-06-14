/********************************************************************************************
 * Project Name - CustomAttributesWrapper DTO
 * Description  - Data object of CustomAttributes for Each Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        03-Oct-2018   Jagan                   Created 
 *2.60        08-Apr-2019   Akshay Gulaganji        added default Constructor 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.CommonAPI.Helpers
{
    public class CustomAttributesWrapperDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// default Constructor
        /// </summary>
        public CustomAttributesWrapperDTO()
        {
            log.LogMethodEntry();
            CustomAttributeId = -1;
            CustomDataId = -1;
            CustomDataSetId = -1;
            ValueId = -1;
            SiteId = -1;
            MasterEntityId = -1;
            log.LogMethodExit();
        }
        public int CustomAttributeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Applicability { get; set; }
        public int CustomDataId { get; set; }
        public int CustomDataSetId { get; set; }
        public List<CustomAttributeValueListWrapperDTO> ValueList { get; set; }
        public string CustomDataText { get; set; }
        public decimal? CustomDataNumber { get; set; }
        public DateTime? CustomDataDate { get; set; }
        public int ValueId { get; set; }
        public int SiteId { get; set; }
        public int MasterEntityId { get; set; }
        public bool SynchStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string CustomeDataSetGuid { get; set; }
        public string CustomeDataGuid { get; set; }
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }
    }

    public class CustomAttributeValueListWrapperDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int CustomAttributeId { get; set; }
        public int ValueId { get; set; }
        public string Value { get; set; }

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }
    }

}