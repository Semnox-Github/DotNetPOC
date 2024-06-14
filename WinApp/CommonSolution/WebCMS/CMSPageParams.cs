/********************************************************************************************
 * Project Name - CMSDTO  DTO Programs 
 * Description  - Data object of the CMSPageParams
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        8-feb-2018   Jeevan           Created 
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.WebCMS
{
    public class CMSPageParams
    {
        public CMSPageParams()
        {
            this.SiteId = -1;
            this.LanguageId = -1;
            this.ShowContents = false;
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DefaultValue(-1)]
        public int PageId { get; set; }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DefaultValue(-1)]
        public int SiteId { get; set; }


        /// <summary>
        /// Get/Set method of the Url field
        /// </summary>
        [DefaultValue("")]
        public string PageName { get; set; }


        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        [DisplayName("Language Id")]
        [DefaultValue(-1)]
        public int LanguageId { get; set; }


        /// <summary>
        /// Get/Set method of the ShowContents field
        /// </summary>
        public bool ShowContents { get; set; }


    }
}
