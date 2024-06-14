/********************************************************************************************
 * Project Name - Asset Tax DTO
 * Description  - Data object of asset tax
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Asset Tax data object class. This acts as data holder for the Asset Tax business object
    /// </summary>
    public class AssetTaxDTO
    {
        int taxId;
        string taxName;
        double taxPercentage;
        bool activeFlag;
        string guid;
        int siteId;
        bool synchStatus;
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByAssetTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAssetTaxParameters
        {
            /// <summary>
            /// Search by TAX_ID field
            /// </summary>
            TAX_ID = 0,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 1
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssetTaxDTO()
        {
            log.Debug("Starts-AssetTaxDTO() default constructor.");
            taxId = -1;
            log.Debug("Ends-AssetTaxDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AssetTaxDTO(int taxId, string taxName, double taxPercentage, bool activeFlag,
                            string guid, int siteId, bool synchStatus)
        {
            log.Debug("Starts-AssetTaxDTO(with all the data fields) Parameterized constructor.");
            this.taxId = taxId;
            this.taxName = taxName;
            this.taxPercentage = taxPercentage;
            this.activeFlag = activeFlag;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.Debug("Ends-AssetTaxDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the Tax Id field
        /// </summary>
        [DisplayName("Tax Id")]
        [ReadOnly(true)]
        public int TaxId { get { return taxId; } set { taxId = value; } }

        /// <summary>
        /// Get/Set method of the Tax Name field
        /// </summary>        
        [DisplayName("Tax Name")]
        public string TaxName { get { return taxName; } set { taxName = value; } }

        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("Tax Percentage")]
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; } }

        /// <summary>
        /// Get/Set method of the Active Flag field
        /// </summary>
        [DisplayName("Active Flag")]
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
    }
}
