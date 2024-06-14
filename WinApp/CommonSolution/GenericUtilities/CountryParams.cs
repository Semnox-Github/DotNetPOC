/********************************************************************************************
 * Project Name - Country Params Programs 
 * Description  - Data object of the CountryParams
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        20-April-2017   Rakshith           Created 
 *2.70.2        09-Aug-2019     Deeksha            Added logger Methods.
 ********************************************************************************************/

using System.ComponentModel;


namespace Semnox.Core.GenericUtilities
{
    public class CountryParams
    {
        private int countryId;
        private string countryName;
        private int siteId;
        private bool showState;
        private bool showLookupCountry;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CountryParams()
        {
            log.LogMethodEntry();
            this.countryId = -1;
            this.countryName = "";
            this.siteId = -1;
            this.showState = false;
            this.showLookupCountry = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        [DefaultValue(-1)]
        public int CountryId { get { return countryId; } set { countryId = value; } }

        /// <summary>
        /// Get/Set method of the CountryName field
        /// </summary>
        [DisplayName("CountryName")]
        public string CountryName { get { return countryName; } set { countryName = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the ShowState field
        /// </summary>
        [DisplayName("ShowState")]
        public bool ShowState { get { return showState; } set { showState = value; } }

        /// <summary>
        /// Get/Set method of the ShowLookupCountry field
        /// </summary>
        [DisplayName("ShowLookupCountry")]
        public bool ShowLookupCountry { get { return showLookupCountry; } set { showLookupCountry = value; } }

    }
}
