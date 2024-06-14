/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data structure of LanguageViewContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// Data structure of LanguageViewContainer
    /// </summary>
    public class LanguageContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int languageId;
        private string languageName;
        private string languageCode;
        private string cultureCode;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LanguageContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public LanguageContainerDTO(int languageId, string languageName, string languageCode, string cultureCode)
            : this()
        {
            log.LogMethodEntry(languageId, languageName, languageCode, cultureCode);
            this.languageId = languageId;
            this.languageName = languageName;
            this.languageCode = languageCode;
            this.cultureCode = cultureCode;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the languageId field
        /// </summary>
        public int LanguageId
        {
            get
            {
                return languageId;
            }

            set
            {
                languageId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the languageName field
        /// </summary>
        public string LanguageName
        {
            get
            {
                return languageName;
            }

            set
            {
                languageName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the languageCode field
        /// </summary>
        public string LanguageCode
        {
            get
            {
                return languageCode;
            }

            set
            {
                languageCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cultureCode field
        /// </summary>
        public string CultureCode
        {
            get
            {
                return cultureCode;
            }

            set
            {
                cultureCode = value;
            }
        }

    }
}
