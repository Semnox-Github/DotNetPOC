/********************************************************************************************
 * Project Name - Customers  DTO Programs 
 * Description  - Data object of the CustomerParams
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        16-feb-2017   Rakshith           Created 
 *2.70.2      10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 *2.70.3      20-Feb-2020   Jeevan  Modified : Added proprty uniqueId for search
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerParams
    /// </summary>
    public class CustomerParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string loginId;
        private string cardNumber;
        private string userName;
        private string verificationCode;
        private string fName;
        private string mName;
        private string lName;
        private string email;
        private string phone;
        private bool orderBylastUpdatedDate;
        private bool orderByUsername;
        private int siteId;
        private string password;
        private bool setRoamingCustomer;
        private string searchCriteriaMethod;


        /// <summary>
        /// CustomerParams Default Constructor 
        /// </summary>
        public CustomerParams()
        {
            log.LogMethodEntry();
            this.loginId = "";
            this.cardNumber = "";
            this.userName = "";
            this.verificationCode = "";
            this.fName = "";
            this.mName = "";
            this.lName = "";
            this.email = "";
            this.phone = "";
            this.orderBylastUpdatedDate = false;
            this.orderByUsername = false;
            this.siteId = -1;
            this.password = "";
            this.setRoamingCustomer = false;
            this.searchCriteriaMethod = "";
            this.CustomerIdFrom = -1;
            this.CustomerIdTo = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DefaultValue("")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }

        /// <summary>
        /// Get/Set method of the VerificationCode field
        /// </summary>
        [DefaultValue("")]
        public string VerificationCode { get { return verificationCode; } set { verificationCode = value; } }

        /// <summary>
        /// Get/Set method of the LogindId field
        /// </summary>
        [DefaultValue("")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DefaultValue("")]
        public string UserName { get { return userName; } set { userName = value; } }

        /// <summary>
        /// Get/Set method of the FirstName field
        /// </summary>
        [DefaultValue("")]
        public string FirstName { get { return fName; } set { fName = value; } }

        /// <summary>
        /// Get/Set method of the MiddleName field
        /// </summary>
        [DefaultValue("")]
        public string MiddleName { get { return mName; } set { mName = value; } }

        /// <summary>
        /// Get/Set method of the LastName field
        /// </summary>
        [DefaultValue("")]
        public string LastName { get { return lName; } set { lName = value; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DefaultValue("")]
        public string Email { get { return email; } set { email = value; } }


        /// <summary>
        /// Get/Set method of the Phone field
        /// </summary>
        [DefaultValue("")]
        public string Phone { get { return phone; } set { phone = value; } }


        /// <summary>
        /// Get/Set method of the PhoneExact field
        /// </summary>
        [DefaultValue("")]
        public string PhoneExact { get; set; }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the Password field
        /// </summary>
        [DefaultValue("")]
        public string Password { get { return password; } set { password = value; } }

        /// <summary>
        /// Get/Set method of the SetRoamingCustomer field
        /// </summary>
        public bool SetRoamingCustomer { get { return setRoamingCustomer; } set { setRoamingCustomer = value; } }

        ///<summary>
        /// Get/Set method of the SearchByField field
        /// </summary>
        [DefaultValue("")]
        public string SearchCriteriaMethod { get { return searchCriteriaMethod; } set { searchCriteriaMethod = value; } }

        ///<summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DefaultValue(-1)]
        public int CustomerId { get; set; }

        ///<summary>
        /// Get/Set method of the CustomerIdFrom field
        /// </summary>
        [DefaultValue(-1)]
        public int CustomerIdFrom { get; set; }
        ///<summary>
        /// Get/Set method of the CustomerIdTo field
        /// </summary>
        [DefaultValue(-1)]
        public int CustomerIdTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool OrderBylastUpdatedDate
        {
            get
            {
                return orderBylastUpdatedDate;
            }

            set
            {
                orderBylastUpdatedDate = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool OrderByUsername
        {
            get
            {
                return orderByUsername;
            }

            set
            {
                orderByUsername = value;
            }
        }


		/// <summary>
		/// Get/Set method of the NewPassword field
		/// </summary>
		[DefaultValue("")]
		public string NewPassword { get; set; }

		/// <summary>
		/// Get/Set method of the Token field
		/// </summary>
		[DefaultValue("")]
		public string Token { get; set; }

		/// <summary>
		/// Get/Set method of the LastErrorMessage field
		/// </summary>
		[DefaultValue("")]
		public string LastErrorMessage { get; set; }

		// <summary>
		/// Get/Set method of the UniqueId field
		/// </summary>
		[DefaultValue("")]
		public string UniqueId { get; set; }


	}
}
