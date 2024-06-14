/********************************************************************************************
 * Project Name - CustomerContentForWaiver DTO
 * Description  - Data object of CustomerSignedWaiver
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      26-Sep-2019      Deeksha           Created for waiver phase 2
 *2.100       19-Oct-2020      Guru S A          Enabling minor signature option for waiver
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerContentForWaiverDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private int customerId;
        private string customerName;
        private string phoneNumber;
        private string emailId;
        private string attribute1Name;
        private string attribute2Name;
        private DateTime? customerDOB;
        private List<KeyValuePair<string, string>> waiverCustomAttributeList = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        [DisplayName("Customer Id")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomerName field
        /// </summary>
        [DisplayName("Customer Name")]
        public string CustomerName { get { return customerName; } set { customerName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PhoneNumber field
        /// </summary>
        [DisplayName("Phone Number")]
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EmailId field
        /// </summary>
        [DisplayName("Email Id")]
        public string EmailId { get { return emailId; } set { emailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the customerDOB field
        /// </summary>
        [DisplayName("Customer Date Of Birth")]
        public DateTime? CustomerDOB { get { return customerDOB; } set { customerDOB = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Attribute1Name field
        /// </summary>
        [DisplayName("Attribute1 Name")]
        public string Attribute1Name { get { return attribute1Name; } set { attribute1Name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Attribute2Name field
        /// </summary>
        [DisplayName("Attribute2 Name")]
        public string Attribute2Name { get { return attribute2Name; } set { attribute2Name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the waiverCustomAttributeList field
        /// </summary>
        [DisplayName("Waiver CustomA ttribute List")]
        public List<KeyValuePair<string, string>> WaiverCustomAttributeList { get { return waiverCustomAttributeList; } set { waiverCustomAttributeList = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || customerId < 0;
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
    }
}
