/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerSummary Data Object Class.  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.120.0     15-Mar-2021      Prajwal S                 Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the CustomerSummary data object class. This acts as data holder for the CustomerSummary business object
    /// </summary>
    public class CustomerSummaryDTO
    {
        string membershipCard;
        double membershipTotalPoints;
        int customerId;
        string name;
        int membershipId;
        string membershipName;
        DateTime? membershipValidity;
        private string membershipPointDetails;
        string membershipRewardsDetails;

        public CustomerSummaryDTO()
        {
            membershipCard = string.Empty;
            membershipPointDetails = string.Empty;
            membershipRewardsDetails = string.Empty;
            membershipTotalPoints = -1;
            customerId = -1;
            name = string.Empty;
            membershipId = -1;
            membershipName = string.Empty;
        }
        public CustomerSummaryDTO(int customerId, int membershipId, string membershipCard, string name, string membershipName,DateTime? membershipValidity, double membershipPointDetails, string memberShipPointDetails, string membershipRewardsDetails)
            :this()
        {
            this.membershipCard = membershipCard;
            this.membershipTotalPoints = membershipPointDetails;
            this.customerId = customerId;
            this.name = name;
            this.membershipId = membershipId;
            this.membershipName = membershipName;
            this.membershipPointDetails = memberShipPointDetails;
            this.membershipValidity = membershipValidity;
            this.membershipRewardsDetails = membershipRewardsDetails;
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                customerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipCard field
        /// </summary>
        public string MembershipCard
        {
            get
            {
                return membershipCard;
            }

            set
            {
                membershipCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipTotalPoints field
        /// </summary>
        public double MembershipTotalPoints
        {
            get
            {
                return membershipTotalPoints;
            }

            set
            {
                membershipTotalPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MemberShipName field
        /// </summary>
        public string MemberShipName
        {
            get
            {
                return membershipName;
            }

            set
            {
                membershipName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipRewardsDetails field
        /// </summary>
        public string MembershipRewardsDetails
        {
            get
            {
                return membershipRewardsDetails;
            }

            set
            {
                membershipRewardsDetails = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipPointDetails field
        /// </summary>
        public string MembershipPointDetails
        {
            get
            {
                return membershipPointDetails;
            }

            set
            {
                membershipPointDetails = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipValidity field
        /// </summary>
        public DateTime? MembershipValidity
        {
            get
            {
                return membershipValidity;
            }

            set
            {
                membershipValidity = value;
            }
        }

    }
}
