/********************************************************************************************
 * Project Name - Membership
 * Description  - DTO of MembershipRewardContainer
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By    Remarks          
 *********************************************************************************************
 2.150.3.0     3-04-2023     Yashodhara C H     Created
  ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// This is the MembershipRule data object class. This acts as data holder for the MembershipRule business object
    /// </summary>
    public class MembershipRuleContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int membershipRuleID;
        private string ruleName;
        private string description;
        private int qualifyingPoints;
        private int qualificationWindow;
        private string unitOfQualificationWindow;
        private int retentionPoints;
        private int retentionWindow;
        private string unitOfRetentionWindow;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipRuleContainerDTO()
        {
            log.LogMethodEntry();
            membershipRuleID = -1;
            ruleName = "";
            log.LogMethodExit();

        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public MembershipRuleContainerDTO(int membershipRuleID, string ruleName, string description, int qualifyingPoints, int qualificationWindow,
                                 string unitOfQualificationWindow, int retentionPoints, int retentionWindow, string unitOfRetentionWindow)
            : this()
        {
            log.LogMethodEntry(membershipRuleID, ruleName, description, qualifyingPoints, qualificationWindow,
                                  unitOfQualificationWindow, retentionPoints, retentionWindow, unitOfRetentionWindow);
            this.membershipRuleID = membershipRuleID;
            this.ruleName = ruleName;
            this.description = description;
            this.qualifyingPoints = qualifyingPoints;
            this.qualificationWindow = qualificationWindow;
            this.unitOfQualificationWindow = unitOfQualificationWindow;
            this.retentionPoints = retentionPoints;
            this.retentionWindow = retentionWindow;
            this.unitOfRetentionWindow = unitOfRetentionWindow;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipRuleID field
        /// </summary>
        [DisplayName("MembershipRuleID")]
        [ReadOnly(true)]
        public int MembershipRuleID
        {
            get
            {
                return membershipRuleID;
            }

            set
            {
                membershipRuleID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RuleName field
        /// </summary>
        [DisplayName("Rule Name")]
        public string RuleName
        {
            get
            {
                return ruleName;
            }

            set
            {
                ruleName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the QualifyingPoints field
        /// </summary>
        [DisplayName("Qualifying Points")]
        public int QualifyingPoints
        {
            get
            {
                return qualifyingPoints;
            }

            set
            {
                qualifyingPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the QualificationWindow field
        /// </summary>
        [DisplayName("Qualification Window")]
        public int QualificationWindow
        {
            get
            {
                return qualificationWindow;
            }

            set
            {
                qualificationWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfQualificationWindow field
        /// </summary>
        [DisplayName("Unit Of Qualification Window")]
        public string UnitOfQualificationWindow
        {
            get
            {
                return unitOfQualificationWindow;
            }

            set
            {
                unitOfQualificationWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RetentionPoints field
        /// </summary>
        [DisplayName("Retention Points")]
        public int RetentionPoints
        {
            get
            {
                return retentionPoints;
            }

            set
            {
                retentionPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RetentionWindow field
        /// </summary>
        [DisplayName("Retention Window")]
        public int RetentionWindow
        {
            get
            {
                return retentionWindow;
            }

            set
            {
                retentionWindow = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfRetentionWindow field
        /// </summary>
        [DisplayName("Unit Of Retention Window")]
        public string UnitOfRetentionWindow
        {
            get
            {
                return unitOfRetentionWindow;
            }

            set
            {
                unitOfRetentionWindow = value;
            }
        }
    }
}
