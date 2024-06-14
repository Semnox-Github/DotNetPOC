/********************************************************************************************
* Project Name - Product
* Description  - Data transfer object of  TransactionProfileContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    1-Sep-2021        Lakshminarayana            Created 
********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data transfer object of  TransactionProfileContainer class 
    /// </summary>
    public class TransactionProfileContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int transactionProfileId;
        private string profileName;
        private bool verificationRequired;
        private int priceListId;
        private List<TransactionProfileTaxRuleContainerDTO> transactionProfileTaxRuleContainerDTOList = new List<TransactionProfileTaxRuleContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionProfileContainerDTO()
        {
            log.LogMethodEntry();
            transactionProfileId = -1;
            priceListId = -1;
            profileName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public TransactionProfileContainerDTO(int transactionProfileId, string profileName, bool verificationRequired, int priceListId)
        {
            log.LogMethodEntry(transactionProfileId, profileName, verificationRequired, priceListId);
            this.transactionProfileId = transactionProfileId;
            this.profileName = profileName;
            this.verificationRequired = verificationRequired;
            this.priceListId = priceListId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TransactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get
            {
                return transactionProfileId;
            }

            set
            {
                transactionProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProfileName field
        /// </summary>
        public string ProfileName
        {
            get
            {
                return profileName;
            }

            set
            {
                profileName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the VerificationRequired field
        /// </summary> 
        public bool VerificationRequired
        {
            get
            {
                return verificationRequired;
            }

            set
            {
                verificationRequired = value;
            }
        }

        /// <summary>
        /// Get/Set method of the priceListId field
        /// </summary>
        public int PriceListId
        {
            get
            {
                return priceListId;
            }

            set
            {
                priceListId = value;
            }
        }

        /// <summary>
        /// Get/Set methods for transactionProfileTaxRuleContainerDTOList 
        /// </summary>
        public List<TransactionProfileTaxRuleContainerDTO> TransactionProfileTaxRuleContainerDTOList
        {
            get
            {
                return transactionProfileTaxRuleContainerDTOList;
            }

            set
            {
                transactionProfileTaxRuleContainerDTOList = value;
            }
        }
    }
}
