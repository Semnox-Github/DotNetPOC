/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingGroupContainerDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes 
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class CustomerProfilingContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerProfilingId;
        private int customerProfilingGroupId;
        private int profileType;
        private string profileTypeName;
        private string compareOperator;
        private decimal? profileValue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerProfilingContainerDTO()
        {
            log.LogMethodEntry();
            customerProfilingGroupId = -1;
            customerProfilingId = -1;
            profileType = -1;
            profileTypeName = string.Empty;
            compareOperator = string.Empty;
            profileValue = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CustomerProfilingContainerDTO(int customerProfileId, int customerProfileGroupId, int profileType,
                                      string compareOperator, decimal? profileValue, string profileTypeName)
          : this()
        {
            log.LogMethodEntry(customerProfileId, customerProfileGroupId, profileType, compareOperator, profileValue,profileValue, profileTypeName);
            this.customerProfilingGroupId = customerProfileGroupId;
            this.customerProfilingId = customerProfileId;
            this.profileType = profileType;
            this.compareOperator = compareOperator;
            this.profileValue = profileValue;
            this.profileTypeName = profileTypeName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the customerProfileId field
        /// </summary>
        public int CustomerProfilingId { get { return customerProfilingId; } set {  customerProfilingId = value; } }
        /// <summary>
        /// Get/Set method of the CustomerProfileGroupId field
        /// </summary>
        public int CustomerProfilingGroupId { get { return customerProfilingGroupId; } set {  customerProfilingGroupId = value; } }
        /// <summary>
        /// Get/Set method of the profileType field
        /// </summary>
        public int ProfileType { get { return profileType; } set {  profileType = value; } }
        /// <summary>
        /// Get/Set method of the profileTypeName field
        /// </summary>
        public string ProfileTypeName { get { return profileTypeName; } set {  profileTypeName = value; } }
        /// <summary>
        /// Get/Set method of the CompareOperator field
        /// </summary>
        public string CompareOperator { get { return compareOperator; } set {  compareOperator = value; } }
        /// <summary>
        /// Get/Set method of the CompareOperator field
        /// </summary>
        public decimal? ProfileValue { get { return profileValue; } set {  profileValue = value; } }

    }
}
