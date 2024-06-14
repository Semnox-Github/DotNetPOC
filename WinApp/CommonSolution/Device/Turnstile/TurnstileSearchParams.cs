/********************************************************************************************
 * Project Name - Device.Turnstile
 * Description  - Class for  of TurnstileSearchParams      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TurnstileSearchParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int turnstileId;
        private string turnstileName;
        private bool active;
        private string type;
        private string make;
        private string model;
        private int gameProfileId;
        private int siteId;


         /// <summary>
        /// Default constructor
        /// </summary>
        public TurnstileSearchParams()
        {
            log.LogMethodEntry();
            turnstileId = -1;
            turnstileName = "";
            active = true;
            type = "";
            make = "";
            model = "";
            gameProfileId = -1;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the TurnstileId field
        /// </summary>
        [DisplayName("TurnstileId")]
        public int TurnstileId { get { return turnstileId; } set { turnstileId = value; } }

        /// <summary>
        /// Get/Set method of the TurnstileName field
        /// </summary>
        [DisplayName("TurnstileName")]
        public string TurnstileName { get { return turnstileName; } set { turnstileName = value; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; } }

     
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type { get { return type; } set { type = value;  } }

        /// <summary>
        /// Get/Set method of the Make field
        /// </summary>
        [DisplayName("Make")]
        public string Make { get { return make; } set { make = value; } }

        /// <summary>
        /// Get/Set method of the Model field
        /// </summary>
        [DisplayName("Model")]
        public string Model { get { return model; } set { model = value;  } }

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>
        [DisplayName("GameProfileId")]
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }

       /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }


    }
}
