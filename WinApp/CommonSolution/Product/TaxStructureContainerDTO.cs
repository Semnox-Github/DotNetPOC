/********************************************************************************************
 * Project Name -TaxStructure DTO
 * Description  - Data object of asset TaxStructure.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.150.0    18-Jan-2022    Prajwal S           Created
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// This is the TaxStructure data object class. This acts as data holder for the TaxStructure business object
    /// </summary>
    public class TaxStructureContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        private int taxStructureId;
        private int taxId;
        private string structureName;
        private double percentage;
        private int parentStructureId;
        private string guid;
        private decimal effectivePercentage;
        /// <summary>
        /// Default constructor
        /// </summary>
        public TaxStructureContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public TaxStructureContainerDTO(int taxStructureId, int taxId, string structureName, double percentage, int parentStructureId, decimal effectivePercentage, string guid)
            : this()
        {
            log.LogMethodEntry(taxStructureId, taxId, structureName, percentage, parentStructureId, effectivePercentage, guid);
            this.taxStructureId = taxStructureId;
            this.taxId = taxId;
            this.structureName = structureName;
            this.percentage = percentage;
            this.parentStructureId = parentStructureId;
            this.effectivePercentage = effectivePercentage;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the StructureName field
        /// </summary>
        public string StructureName
        {
            get { return structureName; }
            set
            {
                structureName = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ParentStructureId field
        /// </summary>
        public int ParentStructureId
        {
            get { return parentStructureId; }
            set
            {
               
                parentStructureId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Percentage field
        /// </summary>
        public double Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        public int TaxId
        {
            get { return taxId; }
            set
            {
                taxId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TaxStructureId field
        /// </summary>
        public int TaxStructureId
        {
            get { return taxStructureId; }
            set
            {
                taxStructureId = value;
            }
        }
      
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Set method of the effectivePercentage field
        /// </summary>
        public decimal EffectivePercentage
        {
            get
            {
                return effectivePercentage;
            }
            set
            {
                effectivePercentage = value;
            }
        }

    }
}