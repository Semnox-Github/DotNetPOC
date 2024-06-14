/********************************************************************************************
 * Project Name - WaiverCustomerAndSignature DTO
 * Description  - Data object of WaiverCustomerAndSignatureDTO
 * 
 **************
 **Version Log
 **************
 *Version       Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        27-Sep-2019      Deeksha           Created for waiver phase 2
 *2.100         19-Oct-2020      Guru S A          Enabling minor signature option for waiver
 ********************************************************************************************/
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Semnox.Parafait.Customer.Waivers
{
    public class WaiverCustomerAndSignatureDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private WaiversDTO waiversDTO;
        private CustomerDTO signatoryCustomerDTO;
        private List<CustomerDTO> signForCustomerDTOList;
        private List<WaiveSignatureImageWithCustomerDetailsDTO> custIdNameSignatureImageList;
        private string channel;
        //private string signatureImageBase64;
        private List<KeyValuePair<int, string>> custIdSignatureImageBase64List; 
        List<CustomerContentForWaiverDTO> customerContentDTOList;


        public enum WhoCanSignForMinor
        {
            /// <summary>
            /// ADULT can sign behalf of minor
            /// </summary>
            ADULT,
            /// <summary>
            /// MINOR can sign for themselves
            /// </summary>
            MINOR,
            /// <summary>
            /// ADULT_AND_MINOR need to sign for minor
            /// </summary>
            ADULT_AND_MINOR 
        }


        public WaiverCustomerAndSignatureDTO()
        {
            waiversDTO = new WaiversDTO();
            signatoryCustomerDTO = new CustomerDTO();
            signForCustomerDTOList = new List<CustomerDTO>();
            customerContentDTOList = new List<CustomerContentForWaiverDTO>();
            custIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
            custIdSignatureImageBase64List = new List<KeyValuePair<int, string>>();
        }

        /// <summary>
        /// Get/Set method of the WaiversDTO field
        /// </summary>
        [Browsable(false)]
        public WaiversDTO WaiversDTO
        {
            get { return waiversDTO; }
            set { waiversDTO = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the signatoryCustomerDTO field
        /// </summary>
        [Browsable(false)]
        public CustomerDTO SignatoryCustomerDTO
        {
            get { return signatoryCustomerDTO; }
            set { signatoryCustomerDTO = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CustIdNameSignatureImageList field
        /// </summary>
        [Browsable(false)]
        public List<WaiveSignatureImageWithCustomerDetailsDTO> CustIdNameSignatureImageList
        {
            get { return custIdNameSignatureImageList; }
            set { custIdNameSignatureImageList = value; this.IsChanged = true; }
        }

        ///// <summary>
        ///// Get/Set method of the signatureImageBase64 field
        ///// </summary>
        //[Browsable(false)]
        //public string SignatureImageBase64
        //{
        //    get { return signatureImageBase64; }
        //    set { signatureImageBase64 = value; this.IsChanged = true; }
        //}
        /// <summary>
        /// Get/Set method of the custIdSignatureImageBase64List field
        /// </summary>
        [Browsable(false)]
        public List<KeyValuePair<int, string>> CustIdSignatureImageBase64List
        {
            get { return custIdSignatureImageBase64List; }
            set { custIdSignatureImageBase64List = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the channel field
        /// </summary>
        [Browsable(false)]
        public string Channel
        {
            get { return channel; }
            set { channel = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the signatoryCustomerDTO field
        /// </summary>
        [Browsable(false)]
        public List<CustomerDTO> SignForCustomerDTOList
        {
            get { return signForCustomerDTOList; }
            set { signForCustomerDTOList = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the customerContentDTOList field
        /// </summary>
        [Browsable(false)]
        public List<CustomerContentForWaiverDTO> CustomerContentDTOList
        {
            get { return customerContentDTOList; }
            set { customerContentDTOList = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Returns whether the DTO changed or any of its children  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (signForCustomerDTOList != null &&
                   signForCustomerDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (customerContentDTOList != null &&
                   customerContentDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

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
    }
}
