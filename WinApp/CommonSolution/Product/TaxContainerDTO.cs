/********************************************************************************************
 * Project Name -Tax Container DTO
 * Description  -Container Data object of tax
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.00     18-Jan-2022  Prajwal S      Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the  Tax data object class. This acts as data holder for the  Tax business object
    /// </summary>
    public class TaxContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int taxId;
        private string taxName;
        private double taxPercentage;
        private string guid;
        private List<TaxStructureContainerDTO> taxStructureContainerDTOList;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaxContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the required fields
        /// </summary>
        public TaxContainerDTO(int taxId, string taxName, double taxPercentage, string attribute1,
                      string attribute2, string attribute3, string attribute4, string attribute5, string guid)
            : this()

        {
            log.LogMethodEntry(taxId, taxName, taxPercentage, guid, attribute1, attribute2, attribute3, attribute4, attribute5);
            this.taxId = taxId;
            this.taxName = taxName;
            this.taxPercentage = taxPercentage;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Tax Id field
        /// </summary>
        [ReadOnly(true)]
        public int TaxId { get { return taxId; } set { taxId = value; } }

        /// <summary>
        /// Get/Set method of the Tax Name field
        /// </summary>        
        public string TaxName { get { return taxName; } set { taxName = value; } }

        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return attribute1; }
            set { attribute1 = value; }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return attribute2; }
            set { attribute2 = value; }
        }

        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3
        {
            get { return attribute3; }
            set { attribute3 = value; }
        }

        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4
        {
            get { return attribute4; }
            set { attribute4 = value; }
        }

        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5
        {
            get { return attribute5; }
            set { attribute5 = value; }
        }



        /// <summary>
        /// Get/Set method of the TaxStructureDTOList field
        /// </summary>
        public List<TaxStructureContainerDTO> TaxStructureContainerDTOList { get { return taxStructureContainerDTOList; } set { taxStructureContainerDTOList = value; } }

    }
}
