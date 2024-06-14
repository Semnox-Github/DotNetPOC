using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Semnox.Core.Utilities
{
    public class RegexUtilities
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            log.LogMethodEntry(strIn);

            invalid = false;
            if (String.IsNullOrEmpty(strIn))
            {
                log.LogMethodExit(false);
                return false;
            }

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper);
            if (invalid)
            {
                log.LogMethodExit(false);
                return false;
            }
            // Return true if strIn is in valid e-mail format. 
            bool returnValue = Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private string DomainMapper(Match match)
        {
            log.LogMethodEntry(match);

            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException ex)
            {
                log.Error("Failed to get ASCII value of domainName ", ex);
                invalid = true;
            }

            string returnValue = match.Groups[1].Value + domainName;

            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}