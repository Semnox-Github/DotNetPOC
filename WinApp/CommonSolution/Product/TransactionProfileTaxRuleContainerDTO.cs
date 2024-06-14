/********************************************************************************************
* Project Name - Product
* Description  - Data transfer object of  TransactionProfileTaxRuleContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    1-Sep-2021        Lakshminarayana            Created 
********************************************************************************************/

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data transfer object of  TransactionProfileTaxRuleContainer class 
    /// </summary>
    public class TransactionProfileTaxRuleContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int trxProfileId;
        private int taxId;
        private int taxStructureId;
        private bool exempt;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionProfileTaxRuleContainerDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.trxProfileId = -1;
            this.taxId = -1;
            this.taxStructureId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public TransactionProfileTaxRuleContainerDTO(int id, int trxProfileId, int taxId, int taxStructureId, bool exempt)
            : this()
        {
            log.LogMethodEntry(id, trxProfileId, taxId, taxStructureId, exempt);
            this.id = id;
            this.trxProfileId = trxProfileId;
            this.taxId = taxId;
            this.taxStructureId = taxStructureId;
            this.exempt = exempt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>]
        public int Id { get { return id; } set { id = value;  } }
        /// <summary>
        /// Get/Set method of the TrxProfileId field
        /// </summary>
        public int TrxProfileId { get { return trxProfileId; } set { trxProfileId = value;  } }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        public int TaxId { get { return taxId; } set { taxId = value;  } }

        /// <summary>
        /// Get/Set method of the TaxStructureId field
        /// </summary>
        public int TaxStructure { get { return taxStructureId; } set { taxStructureId = value;  } }

        /// <summary>
        /// Get/Set method of the Exempt field
        /// </summary>
        public bool Exempt { get { return exempt; } set { exempt = value;  } }
    }
}
