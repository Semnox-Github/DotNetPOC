using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.SMSGateway
{
    public class BaseSMS
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parafait utilities.
        /// </summary>
        protected Utilities utilities;

        /// <summary>
        /// BaseSMSGateway Parameterized Constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public BaseSMS(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            utilities = _utilities;
            log.LogMethodExit();
        }


        /// <summary>
        /// Initialize method of BaseSMSGateway class
        /// </summary>
        /// <returns></returns>
        public virtual bool Initialize(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            log.LogMethodExit(false);
            return false;
        }


        /// <summary>
        /// PopulateTemplate of BaseSMSGateway class
        /// </summary>
        /// <param name="_utilities"></param>
        /// <returns></returns>
        public virtual string PopulateTemplate(string Template, List<KeyValuePair<string, string>> paramsList)
        {
            log.LogMethodEntry(Template, paramsList);
            log.LogMethodExit();
            return "";
        }


        /// <summary>
        /// SendRequest method of BaseSMSGateway class
        /// </summary>
        /// <returns></returns>
        public virtual string SendRequest(string PhoneNos, string Template)
        {
            log.LogMethodEntry("PhoneNos", Template);
            log.LogMethodExit();
            return "";
        }

    }
}
