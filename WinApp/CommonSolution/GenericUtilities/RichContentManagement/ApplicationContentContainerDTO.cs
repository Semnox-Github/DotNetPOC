/********************************************************************************************
* Project Name - GenericUtilities
* Description  - ApplicationContentContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int applicationContentId;
        private string application;
        private string module;
        private string chapter;
        private string fileName;
        private int contentId;

        public ApplicationContentContainerDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public ApplicationContentContainerDTO(int applicationContentIdPassed, string application, string module, string chapter, string fileName, int contentId) : this()
        {
            log.LogMethodEntry(applicationContentIdPassed, application);
            this.applicationContentId = applicationContentIdPassed;
            this.application = application;
            this.module = module;
            this.chapter = chapter;
            this.fileName = fileName;
            this.contentId = contentId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int ApplicationContentId { get { return applicationContentId; } set { applicationContentId = value; } }

        /// <summary>
        /// Get/Set method of the Application field
        /// </summary>
        [DisplayName("Application")]
        public string Application { get { return application; } set { application = value; } }
          /// <summary>
        /// Get/Set method of the Module field
        /// </summary>
        [DisplayName("Module")]
        public string Module { get { return module; } set { module = value; } }
        /// <summary>
        /// Get/Set method of the Chapter field
        /// </summary>
        [DisplayName("Chapter")]
        public string Chapter { get { return chapter; } set { chapter = value; } }
        /// <summary>
        /// Get/Set method of the FileName field
        /// </summary>
        [DisplayName("FileName")]
        public string FileName { get { return fileName; } set { fileName = value; } }
        /// <summary>
        /// Get/Set method of the ContentId field
        /// </summary>
        [DisplayName("ContentId")]
        public int ContentId { get { return contentId; } set { contentId = value; } }

    }
}
