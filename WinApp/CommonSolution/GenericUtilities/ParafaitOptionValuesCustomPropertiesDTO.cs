/********************************************************************************************
 * Project Name - ParafaitOptionValuesCustomProperties DTO
 * Description  - Data object of Parafaitoptionvalues
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        14-Mar-2018   Muhammed Mehraj         Created 
 *2.60        30-Apr-2019   Mushahid Faizan         Added get/set property.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class ParafaitOptionValuesCustomPropertiesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int OptionId { get; set; }
        public int DefaultValueId { get; set; }
        public string DefaultValueName { get; set; }

        public string DefaultValueNameText { get; set; }
        public string OptionValue { get; set; }
        public string DefaultValue { get; set; }

        public string UserLevel { get; set; }
        public string POSLevel { get; set; }
        public string IsProtected { get; set; }
        public int DataTypeId { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
        public string ScreenGroup { get; set; }
        public string Description { get; set; }
        public int SiteId { get; set; }
        public int MasterEntityId { get; set; }
        public int POSMachineId { get; set; }

        public int UserId { get; set; }
        public bool SynchStatus { get; set; }

        public string Guid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public List<CommonLookupDTO> Items { get; set; }

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

    public class ParafaitOptionValuesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
