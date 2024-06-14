using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class PurchaseTaxDTOTemp
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByPurchaseTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPurchaseTaxParameters
        {
            /// <summary>
            /// Search by TAXID field
            /// </summary>
            TAXID = 0,
            /// <summary>
            /// Search by TAXNAME field
            /// </summary>
            TAXNAME = 1,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID = 2,
            /// <summary>
            /// Search by ACTIVEFLAG field
            /// </summary>
            ACTIVEFLAG =3
        }

        int taxId;
        string taxName;
        double taxPercentage;
        string activeFlag;
        int Site_id;
        string guid;
        bool synchStatus;
        int masterEntityId;


        /// <summary>
        /// Default Contructor
        /// </summary>
        public PurchaseTaxDTOTemp()
        {
            log.LogMethodEntry();
            taxId = -1;
            Site_id = -1;
            masterEntityId = -1;
            activeFlag = "Y";
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public PurchaseTaxDTOTemp(int taxId, string taxName, double taxPercentage, string activeFlag, int Site_id, 
                                string guid, bool synchStatus, int masterEntityId)
        {
            log.LogMethodEntry();
            this.taxId = taxId;
            this.taxName = taxName;
            this.taxPercentage = taxPercentage;
            this.activeFlag = activeFlag;
            this.Site_id = Site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        [DisplayName("TaxId")]
        [ReadOnly(true)]
        public int TaxId { get { return taxId; } set { taxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxName field
        /// </summary>
        [DisplayName("TaxName")]
        public string TaxName { get { return taxName; } set { taxName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("TaxPercentage")]
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        public string ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        public int site_id { get { return Site_id; } set { Site_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || taxId < 0;
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
            log.LogMethodExit();
        }
    }
}
