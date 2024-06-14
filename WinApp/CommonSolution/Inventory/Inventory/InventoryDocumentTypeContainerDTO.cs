/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryDocumentTypeContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0      08-Sep-2022     Abhishek             Created : Inventory UI redesign
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDocumentTypeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int documentTypeId;
        private string name;
        private string description;
        private string applicability;
        private string code;

        public InventoryDocumentTypeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public InventoryDocumentTypeContainerDTO(int documentTypeId, string name, string description, string applicability, string code)
             : this()
        {
            log.LogMethodEntry(documentTypeId, name, description, applicability, code);
            this.documentTypeId = documentTypeId;
            this.name = name;
            this.description = description;
            this.applicability = applicability;
            this.code = code;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DocumentTypeId field
        /// </summary>
        public int DocumentTypeId { get { return documentTypeId; } set { documentTypeId = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>
        public string Applicability { get { return applicability; } set { applicability = value; } }

        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        public string Code { get { return code; } set { code = value; } }
        
    }
}
