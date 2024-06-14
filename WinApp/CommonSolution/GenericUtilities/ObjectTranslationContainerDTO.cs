/********************************************************************************************
 * Project Name - Object Translations Container DTO
 * Description  - Data object of  object translations 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00   29-July-2021   Prajwal S          Created                                                       
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the object translation data object class. This acts as data holder for the object translation business object
    /// </summary>
    public class ObjectTranslationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int languageId;
        private string elementGuid;
        private string tableObject;
        private string element;
        private string translation;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObjectTranslationContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ObjectTranslationContainerDTO(int id, int languageId, string tableObject, string elementGuid, string element, string translation)
            : this()
        {
            log.LogMethodEntry(id, languageId, tableObject, elementGuid, element, translation);
            this.id = id;
            this.elementGuid = elementGuid;
            this.element = element;
            this.translation = translation;
            this.languageId = languageId;
            this.tableObject = tableObject;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value;} }
        
        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        [DisplayName("Language")]
        public int LanguageId { get { return languageId; } set { languageId = value; } }

        /// <summary>
        /// Get/Set method of the ElementGuid field
        /// </summary>
        [DisplayName("ElementGuid")]
        public string ElementGuid { get { return elementGuid; } set { elementGuid = value;} }

        /// <summary>
        /// Get/Set method of the TableObject field
        /// </summary>
        [DisplayName("Table")]
        public string TableObject { get { return tableObject; } set { tableObject = value; } }

        /// <summary>
        /// Get/Set method of the Element field
        /// </summary>
        [DisplayName("Element")]
        public string Element { get { return element; } set { element = value;} }

        /// <summary>
        /// Get/Set method of the Translation field
        /// </summary>
        [DisplayName("Translation")]
        public string Translation { get { return translation; } set { translation = value;} }


    }
}
