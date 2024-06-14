using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource
{
    public class WPCybersourceSecurity
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static String sign(IDictionary<string, string> paramsArray, string SECRET_KEY)
        {
            // 1. Build a CSV list of name=value pairs of Request Fields
            // 2. Use HMACSHA256 to sign the CSV list
            // 3. Base64 encode it
            return sign(buildDataToSign(paramsArray), SECRET_KEY);
        }

        private static String sign(String data, String secretKey)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] keyByte = encoding.GetBytes(secretKey);

                HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
                byte[] messageBytes = encoding.GetBytes(data);
                string signature = Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
                log.LogMethodExit(signature);
                return signature;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }

        private static String buildDataToSign(IDictionary<string, string> paramsArray)
        {
            //log.LogMethodEntry(paramsArray);
            try
            {
                String[] signedFieldNames = paramsArray["signed_field_names"].Split(',');
                IList<string> dataToSign = new List<string>();
                foreach (String signedFieldName in signedFieldNames)
                {
                    //log.Debug("signedFieldName " + signedFieldName);
                    dataToSign.Add(signedFieldName + "=" + paramsArray[signedFieldName]);
                }

                String finalData = commaSeparate(dataToSign);
                //log.LogMethodExit(finalData);
                return finalData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        private static String commaSeparate(IList<string> dataToSign)
        {
            return String.Join(",", dataToSign);
        }
    }
}
